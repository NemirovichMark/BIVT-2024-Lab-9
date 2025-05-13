using Lab_7;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension => "json";

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);
            Console.WriteLine(json);
            if (json["Type"].ToString() == "Participant")
                return (T) (Object) ParticipantFromJSON(json);
            if (json["Type"].ToString() == "Judge")
                return (T) (Object) JudgeFromJSON(json);
            if (json["Type"].ToString() == "Competition")
            {
                var c = new Purple_1.Competition(json["Judges"].ToObject<JObject[]>().Select(js => JudgeFromJSON(js)).ToArray());
                c.Add(json["Participants"].ToObject<JObject[]>().Select(js => ParticipantFromJSON(js)).ToArray());
                return (T)(Object)c;
            }

            return null;
        }

        private Purple_1.Participant ParticipantFromJSON(JObject json)
        {
            var p = new Purple_1.Participant(json["Name"].ToString(), json["Surname"].ToString());
            p.SetCriterias(json["Coefs"].ToObject<double[]>());
            var marks = json["Marks"].ToObject<int[][]>();
            foreach (var arr in marks) p.Jump(arr);
            return p;
        }

        private Purple_1.Judge JudgeFromJSON(JObject json)
        {
            var j = new Purple_1.Judge(json["Name"].ToString(), json["Marks"].ToObject<int[]>());
            return j;
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);
            Console.WriteLine(json);
            Purple_2.SkiJumping jumping;
            if (json["Type"].ToString() == "JuniorSkiJumping") jumping = new Purple_2.JuniorSkiJumping();
            else if (json["Type"].ToString() == "ProSkiJumping") jumping = new Purple_2.ProSkiJumping();
            else return null;
            jumping.Add(json["Participants"].ToObject<JObject[]>().Select(js =>
            {
                var p = new Purple_2.Participant(js["Name"].ToString(), js["Surname"].ToString());
                p.Jump(js["Distance"].ToObject<int>(), js["Marks"].ToObject<int[]>(), json["Standard"].ToObject<int>());
                return p;
            }).ToArray());
            return (T) jumping;
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);
            Console.WriteLine(json);
            Purple_3.Skating skating;
            if (json["Type"].ToString() == "IceSkating") skating = new Purple_3.IceSkating(json["Moods"].ToObject<double[]>(), false);
            else if (json["Type"].ToString() == "FigureSkating") skating = new Purple_3.FigureSkating(json["Moods"].ToObject<double[]>(), false);
            else return null;
            skating.Add(json["Participants"].ToObject<JObject[]>().Select(js =>
            {
                var p = new Purple_3.Participant(js["Name"].ToString(), js["Surname"].ToString());
                var marks = js["Marks"].ToObject<double[]>();
                foreach (var mark in marks) p.Evaluate(mark);
                return p;
            }).ToArray());
            Purple_3.Participant.SetPlaces(skating.Participants);
            return (T) skating;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);
            Console.WriteLine(json);
            var group = new Purple_4.Group(json["Name"].ToString());
            group.Add(json["Sportsmen"].ToObject<JObject[]>().Select(js =>
            {
                var sp = new Purple_4.Sportsman(js["Name"].ToString(), js["Surname"].ToString());
                sp.Run(js["Time"].ToObject<double>());
                return sp;
            }).ToArray());
            return group;
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);
            Console.WriteLine(json);
            var report = new Purple_5.Report();
            report.AddResearch(json["Researches"].ToObject<JObject[]>().Select(js =>
            {
                var res = new Purple_5.Research(js["Name"].ToString());
                var resps = js["Responses"].ToObject<JObject[]>();
                foreach (var resp in resps)
                {
                    res.Add([resp["Animal"].ToObject<String>(), resp["CharacterTrait"].ToObject<String>(), resp["Concept"].ToObject<String>()]);
                }
                return res;
            }).ToArray());
            return report;
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            var json = JObject.FromObject(obj);
            json.Add("Type", obj.GetType().Name);
            SelectFile(fileName);
            File.WriteAllText(FilePath, json.ToString());
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            var json = JObject.FromObject(jumping);
            json.Add("Type", jumping.GetType().Name);
            SelectFile(fileName);
            File.WriteAllText(FilePath, json.ToString());
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            var json = JObject.FromObject(skating);
            json.Add("Type", skating.GetType().Name);
            SelectFile(fileName);
            File.WriteAllText(FilePath, json.ToString());
        }

        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            var json = JObject.FromObject(participant);
            json.Add("Type", participant.GetType().Name);
            SelectFile(fileName);
            File.WriteAllText(FilePath, json.ToString());
        }

        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            var json = JObject.FromObject(group);
            json.Add("Type", group.GetType().Name);
            SelectFile(fileName);
            File.WriteAllText(FilePath, json.ToString());
        }
    }
}
