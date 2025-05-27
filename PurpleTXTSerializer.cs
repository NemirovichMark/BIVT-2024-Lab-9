using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_7;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Xml.Serialization;
using static Lab_7.Purple_2;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";

        internal void ToJSON<T>(T obj, string fileName) where T : class
        {
            SelectFile(fileName);
            if (obj == null || FilePath == null) return;

            var json = JObject.FromObject(obj);
            json.Add("Type", obj.GetType().ToString());
            File.WriteAllText(FilePath, json.ToString());
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            ToJSON(obj, fileName);
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            ToJSON(jumping, fileName);
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            ToJSON(skating, fileName);
        }
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName) //participantS but whatever
        {
            ToJSON(participant, fileName);
        }
        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            ToJSON(group, fileName);
        }   




        public override T DeserializePurple1<T>(string fileName) where T : class
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var content = JObject.Parse(text);
            string typeName = content["Type"].ToString();

            if (typeName == typeof(Purple_1.Participant).ToString())
            {
                var Name = content["Name"].Value<string>();
                var Surname = content["Surname"].Value<string>();
                var Coefs = content["Coefs"].ToObject<double[]>();
                var Marks = content["Marks"].ToObject<int[][]>(); 

                var participant = new Purple_1.Participant(Name, Surname);

                participant.SetCriterias(Coefs);
                foreach (var arr in Marks) participant.Jump(arr);

                return participant as T;
            }
            else if (typeName == typeof(Purple_1.Judge).ToString())
            {
                var Name = content["Name"].Value<string>();
                var Marks = content["Marks"].ToObject<int[]>();

                var judge = new Purple_1.Judge(Name, Marks);

                return judge as T;
            }
            else if (typeName == typeof(Purple_1.Competition).ToString())
            {
                var Judges = content["Judges"].ToObject<Purple_1.Judge[]>();

                var competition = new Purple_1.Competition(Judges);
                foreach (var p in content["Participants"])
                {
                    var Coefs = p["Coefs"].ToObject<double[]>();
                    var Marks = p["Marks"].ToObject<int[][]>();

                    var participant = p.ToObject<Purple_1.Participant>();

                    participant.SetCriterias(Coefs);
                    foreach (var arr in Marks) participant.Jump(arr);

                    competition.Add(participant);
                }

                return competition as T;
            }
            return null;
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);

            var text= File.ReadAllText(FilePath);
            var content = JObject.Parse(text);
            string typeName = content["Type"].ToString();
            Purple_2.SkiJumping skiJumping;
            if (typeName == typeof(Purple_2.JuniorSkiJumping).ToString())
            {
                skiJumping = new Purple_2.JuniorSkiJumping();
            }
            else
            {
                skiJumping = new Purple_2.ProSkiJumping();
            }

            foreach (var p in content["Participants"])
            {
                var Marks = p["Marks"].ToObject<int[]>();
                var Result = p["Result"].Value<int>();
                var Distance = p["Distance"].Value<int>();

                var participant = p.ToObject<Purple_2.Participant>();

                participant.Jump(Distance, Marks, CalcTarget(Distance, Result, Marks));

                skiJumping.Add(participant);
            }

            return skiJumping as T;
        }

        private int CalcTarget(double distance, int result, int[] marks)
        {
            int sumWithoutMaxes = marks.Sum() - marks.Min() - marks.Max();

            double target;

            if (result == 0) target = distance + (sumWithoutMaxes + 60) / 2.0;
            else target = distance - (result - sumWithoutMaxes - 60) / 2.0;

            return (int)Math.Ceiling(target);
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);

            var text = File.ReadAllText(FilePath);
            var content = JObject.Parse(text);
            string typeName = content["Type"].ToString();

            Purple_3.Skating skating;

            if (typeName == typeof(Purple_3.FigureSkating).ToString())
            {
                skating = new Purple_3.FigureSkating(content["Moods"].ToObject<double[]>(), false);
            }
            else
            {
                skating = new Purple_3.IceSkating(content["Moods"].ToObject<double[]>(), false);
            }

            var participants = new List<Purple_3.Participant>();

            foreach (var p in content["Participants"])
            {
                var Marks = p["Marks"].ToObject<double[]>();

                var participant = p.ToObject<Purple_3.Participant>();

                foreach (var mark in Marks) participant.Evaluate(mark);

                participants.Add(participant);
            }

            Purple_3.Participant.SetPlaces(participants.ToArray());

            skating.Add(participants.ToArray());

            return skating as T;
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);

            var text = File.ReadAllText(FilePath);
            var content = JObject.Parse(text);

            var group = new Purple_4.Group(content["Name"].Value<string>());

            foreach (var s in content["Sportsmen"])
            {
                var sportsman = s.ToObject<Purple_4.Sportsman>();
                sportsman.Run(s["Time"].Value<double>());
                group.Add(sportsman);
            }

            return group;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var content = JObject.Parse(text);

            var report = new Purple_5.Report();

            foreach (var rs in content["Researches"])
            {
                var research = new Purple_5.Research(rs["Name"].Value<string>());
                if (rs["Responses"] == null) continue;
                foreach (var rp in rs["Responses"])
                {
                    research.Add(new string[] { rp["Animal"].Value<string>(), rp["CharacterTrait"].Value<string>(), rp["Concept"].Value<string>() });
                }
                report.AddResearch(research);
            }


            return report;
        }
    }
}