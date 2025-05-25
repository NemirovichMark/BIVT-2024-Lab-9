using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_7;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Lab_7.Purple_2;

namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension => "json";
        
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SerJ(obj, fileName);
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SerJ(jumping, fileName);
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SerJ(skating, fileName);
        }

        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            SerJ(participant, fileName);
        }

        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            SerJ(group, fileName);
        }

        private void SerJ<T>(T obj, string fileName) where T : class
        {
            SelectFile(fileName);
            var json = JObject.FromObject(obj);
            json.Add("Type", obj.GetType().ToString());
            File.WriteAllText(FilePath, json.ToString());
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);

            var text = File.ReadAllText(FilePath);
            var obj = JObject.Parse(text);

            if (obj["Type"].ToString() == typeof(Purple_1.Participant).ToString())
            {
                var participant = new Purple_1.Participant(obj["Name"].ToString(), obj["Surname"].ToString());
                participant.SetCriterias(obj["Coefs"].ToObject<double[]>());
                foreach (var m in obj["Marks"].ToObject<int[][]>()) participant.Jump(m);

                return participant as T;
            }

            else if (obj["Type"].ToString() == typeof(Purple_1.Judge).ToString())
            {
                var judge = new Purple_1.Judge(obj["Name"].ToString(), obj["Marks"].ToObject<int[]>());

                return judge as T;
            }

            else if (obj["Type"].ToString() == typeof(Purple_1.Competition).ToString())
            {
                var competition = new Purple_1.Competition(obj["Judges"].ToObject<Purple_1.Judge[]>());
                foreach (var p in obj["Participants"])
                {
                    var participant = p.ToObject<Purple_1.Participant>();

                    participant.SetCriterias(p["Coefs"].ToObject<double[]>());
                    foreach (var m in p["Marks"].ToObject<int[][]>()) participant.Jump(m);

                    competition.Add(participant);
                }

                return competition as T;
            }

            return null;
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);

            var text = File.ReadAllText(FilePath);
            var obj = JObject.Parse(text);

            if (obj["Type"].ToString() == typeof(Purple_2.JuniorSkiJumping).ToString())
            {
                var s = new Purple_2.JuniorSkiJumping();
                foreach (var p in obj["Participants"])
                {
                    var participant = p.ToObject<Purple_2.Participant>();
                    participant.Jump(p["Distance"].Value<int>(), p["Marks"].ToObject<int[]>(), 100);
                    s.Add(participant);
                }
                return s as T;
            }
            else if (obj["Type"].ToString() == typeof(Purple_2.ProSkiJumping).ToString())
            {
                var s = new Purple_2.ProSkiJumping();
                foreach (var p in obj["Participants"])
                {
                    var participant = p.ToObject<Purple_2.Participant>();
                    participant.Jump(p["Distance"].Value<int>(), p["Marks"].ToObject<int[]>(), 150);
                    s.Add(participant);
                }
                return s as T;
            }
            else return null;
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);

            var text = File.ReadAllText(FilePath);
            var obj = JObject.Parse(text);

            Purple_3.Skating skating;
            if (obj["Type"].ToString() == typeof(Purple_3.FigureSkating).ToString())
            {
                skating = new Purple_3.FigureSkating(obj["Moods"].ToObject<double[]>(), false);
            }
            else
            {
                skating = new Purple_3.IceSkating(obj["Moods"].ToObject<double[]>(), false);
            }

            foreach (var p in obj["Participants"])
            {
                var participant = p.ToObject<Purple_3.Participant>();
                foreach (var m in p["Marks"].ToObject<double[]>()) participant.Evaluate(m);
                skating.Add(participant);
            }

            Purple_3.Participant.SetPlaces(skating.Participants);

            return skating as T;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);

            var text = File.ReadAllText(FilePath);
            var obj = JObject.Parse(text);

            var group = new Purple_4.Group(obj["Name"].ToString());

            foreach (var s in obj["Sportsmen"])
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
            var obj = JObject.Parse(text);

            var report = new Purple_5.Report();

            foreach (var r in obj["Researches"])
            {
                var research = new Purple_5.Research(r["Name"].ToString());
                if (r["Responses"] != null)
                {
                    foreach (var rr in r["Responses"])
                    {
                        research.Add(new string[] { rr["Animal"].ToString(), rr["CharacterTrait"].ToString(), rr["Concept"].ToString() });
                    }
                    report.AddResearch(research);
                }
            }

            return report;
        }
    }
}
