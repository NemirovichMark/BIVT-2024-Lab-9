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
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension { get { return "json"; } }

        internal void SerializeJSON<T>(T obj, string fileName) where T : class
        {
            SelectFile(fileName);
            if (obj == null || FilePath == null) return;

            var json = JObject.FromObject(obj);
            json.Add("Type", obj.GetType().ToString());
            File.WriteAllText(FilePath, json.ToString());
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SerializeJSON(obj, fileName);
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SerializeJSON(jumping, fileName);
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SerializeJSON(skating, fileName);
        }
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName) //participantS but whatever
        {
            SerializeJSON(participant, fileName);
        }
        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            SerializeJSON(group, fileName);
        }




        public override T DeserializePurple1<T>(string fileName) where T : class
        {
            SelectFile(fileName);
            var content = File.ReadAllText(FilePath);
            var deser = JObject.Parse(content);
            string typeName = deser["Type"].ToString();

            if (typeName == typeof(Purple_1.Participant).ToString()) //huh??
            {
                var Name = deser["Name"].Value<string>();
                var Surname = deser["Surname"].Value<string>();
                var Coefs = deser["Coefs"].ToObject<double[]>();
                var Marks = deser["Marks"].ToObject<int[][]>(); //praying for this line

                var participant = new Purple_1.Participant(Name, Surname);

                participant.SetCriterias(Coefs);
                foreach (var arr in Marks) participant.Jump(arr);

                return participant as T;
            }
            else if (typeName == typeof(Purple_1.Judge).ToString())
            {
                var Name = deser["Name"].Value<string>();
                var Marks = deser["Marks"].ToObject<int[]>();

                var judge = new Purple_1.Judge(Name, Marks);

                return judge as T;
            }
            else if (typeName == typeof(Purple_1.Competition).ToString())
            {
                var Judges = deser["Judges"].ToObject<Purple_1.Judge[]>();

                var competition = new Purple_1.Competition(Judges);
                foreach (var p in deser["Participants"])
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

            var content = File.ReadAllText(FilePath);
            var deser = JObject.Parse(content);
            string typeName = deser["Type"].ToString();
            Purple_2.SkiJumping skiJumping;
            if (typeName == typeof(Purple_2.JuniorSkiJumping).ToString())
            {
                skiJumping = new Purple_2.JuniorSkiJumping();
            }
            else
            {
                skiJumping = new Purple_2.ProSkiJumping();
            }

            foreach (var p in deser["Participants"])
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

            if (result == 0)  target = distance + (sumWithoutMaxes + 60) / 2.0;
            else target = distance - (result - sumWithoutMaxes - 60) / 2.0;

            return (int)Math.Ceiling(target);
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);

            var content = File.ReadAllText(FilePath);
            var deser = JObject.Parse(content);
            string typeName = deser["Type"].ToString();

            Purple_3.Skating skating;

            if (typeName == typeof(Purple_3.FigureSkating).ToString())
            {
                skating = new Purple_3.FigureSkating(deser["Moods"].ToObject<double[]>(), false);
            }
            else
            {
                skating = new Purple_3.IceSkating(deser["Moods"].ToObject<double[]>(), false);
            }

            var participants = new List<Purple_3.Participant>();

            foreach (var p in deser["Participants"])
            {
                var Marks = p["Marks"].ToObject<double[]>();

                var participant = p.ToObject<Purple_3.Participant>();

                foreach(var mark in Marks) participant.Evaluate(mark);  

                participants.Add(participant);
            }

            Purple_3.Participant.SetPlaces(participants.ToArray());

            skating.Add(participants.ToArray());

            return skating as T;
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);

            var content = File.ReadAllText(FilePath);
            var deser = JObject.Parse(content);

            var group = new Purple_4.Group(deser["Name"].Value<string>());

            foreach(var s in deser["Sportsmen"])
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
            var content = File.ReadAllText(FilePath);
            var deser = JObject.Parse(content);

            var report = new Purple_5.Report();

            foreach(var rs in deser["Researches"])
            {
                var research = new Purple_5.Research(rs["Name"].Value<string>());
                if (rs["Responses"] == null) continue;
                foreach(var rp in rs["Responses"])
                {
                    research.Add(new string[] { rp["Animal"].Value<string>(), rp["CharacterTrait"].Value<string>(), rp["Concept"].Value<string>() });
                }
                report.AddResearch(research);
            }


            return report;
        }
    }
}
