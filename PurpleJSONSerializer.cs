using Lab_7;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab_7.Purple_1;

namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension => "json";
        

        private void ToJSON<T>(T obj, string fileName) where T: class
        {
            SelectFile(fileName);
            if(obj == null || FilePath == null)
            {
                return;
            }
            var jobj = JObject.FromObject(obj);
            jobj.Add("Type", obj.GetType().ToString());
            File.WriteAllText(FilePath, jobj.ToString());
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
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName) 
        {
            ToJSON(participant, fileName);
        }
        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            ToJSON(group, fileName);
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var content = JObject.Parse(text);
            string type = content["Type"].ToString();

            
            if (type == typeof(Purple_1.Participant).ToString())
            {
                return DeserializeParticipant(content) as T;
            }
            else if (type == typeof(Purple_1.Judge).ToString())
            {
                return DeserializeJudge(content) as T;
            }
            else if (type == typeof(Purple_1.Competition).ToString())
            {
                return DeserializeCompetition(content) as T;
            }
            return null;
        }

        private Purple_1.Participant DeserializeParticipant(JObject content)
        {

            var Name = content["Name"].Value<string>();
            var Surname = content["Surname"].Value<string>();
            var Coefs = content["Coefs"].ToObject<double[]>();
            var Marks = content["Marks"].ToObject<int[][]>();

            var participant = new Purple_1.Participant(Name, Surname);


            participant.SetCriterias(Coefs);
            foreach( var arr in Marks)
            {
                participant.Jump(arr);
            }
            return participant;
        }

        private Purple_1.Judge DeserializeJudge(JObject content)
        {
            var Name = content["Name"].Value<string>();
            var Marks = content["Marks"].ToObject<int[]>();

            var judge = new Purple_1.Judge(Name, Marks);
            return judge;
        }

        private Purple_1.Competition DeserializeCompetition(JObject content)
        {
            var Judges = content["Judges"].ToObject<Purple_1.Judge[]>();
            var comp = new Purple_1.Competition(Judges);
            foreach (var p in content["Participants"])
            {
                var Coefs = p["Coefs"].ToObject<double[]>();
                var Marks = p["Marks"].ToObject<int[][]>();

                var participant = p.ToObject<Purple_1.Participant>();

                participant.SetCriterias(Coefs);
                foreach (var arr in Marks) participant.Jump(arr);

                comp.Add(participant);
            }
            return comp;
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var content = JObject.Parse(text);
            string type = content["Type"].ToString();
            Purple_2.SkiJumping jumping;
            if (type == typeof(Purple_2.JuniorSkiJumping).ToString())
            {
                jumping = new Purple_2.JuniorSkiJumping();
            }
            else
            {
                jumping = new Purple_2.ProSkiJumping();
            }

            foreach (var p in content["Participants"])
            {
                var marks = p["Marks"].ToObject<int[]>();
                var res = p["Result"].Value<int>();
                var dist = p["Distance"].Value<int>();

                var part = p.ToObject<Purple_2.Participant>();

                int sumWithoutMaxes = marks.Sum() - marks.Min() - marks.Max();

                double target;

                if (res == 0) target = dist + (sumWithoutMaxes + 60) / 2.0;
                else target = dist - (res - sumWithoutMaxes - 60) / 2.0;

                int targ = (int)Math.Ceiling(target);
                part.Jump(dist, marks, targ);
                jumping.Add(part);
            }
            return jumping as T;
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var content = JObject.Parse(text);

            string type = content["Type"].ToString();

            Purple_3.Skating skating;

            if (type == typeof(Purple_3.FigureSkating).ToString())
            {
                skating = new Purple_3.FigureSkating(content["Moods"].ToObject<double[]>(), false);

            }
            else
            {
                skating = new Purple_3.IceSkating(content["Moods"].ToObject<double[]>(), false);
            }

            var parts = new Purple_3.Participant[0];

            foreach (var p in content["Participants"])
            {
                var marks = p["Marks"].ToObject<double[]>();
                var participant = p.ToObject<Purple_3.Participant>();
                foreach (var mark in marks)
                {
                    participant.Evaluate(mark);
                }
                Array.Resize(ref parts, parts.Length + 1);
                parts[^1] = participant;
            }

            Purple_3.Participant.SetPlaces(parts);
            skating.Add(parts);

            return skating as T;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var content = JObject.Parse(text);

            var gp = new Purple_4.Group(content["Name"].Value<string>());

            foreach (var s in content["Sportsmen"])
            {
                var sportsman = s.ToObject<Purple_4.Sportsman>();
                sportsman.Run(s["Time"].Value<double>());
                gp.Add(sportsman);
            }

            return gp;

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
