using System;
using System.Collections.Generic;
using System.IO;
using Lab_7;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension => "json";
        
        //Serialize
        private void SerializerJson<T>(T obj, string fileName) where T : class
        {
            SelectFile(fileName);
            if (obj == null || FilePath == null) return;

            var json = JObject.FromObject(obj);
            json.Add("Type", obj.GetType().ToString());
            File.WriteAllText(FilePath, json.ToString());
        }

        // Purple_1
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SerializerJson(obj, fileName);
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            if (FilePath == null || !File.Exists(FilePath)) return null;

            var content = File.ReadAllText(FilePath);
            var deser = JObject.Parse(content);

            string typeName = deser["Type"]?.ToString();

            if (typeName == typeof(Purple_1.Participant).ToString())
            {
                string name = deser["Name"].ToString();
                string surname = deser["Surname"].ToString();
                double[] coefs = deser["Coefs"].ToObject<double[]>();
                int[][] marks = deser["Marks"].ToObject<int[][]>();

                var participant = new Purple_1.Participant(name, surname);
                participant.SetCriterias(coefs);

                foreach (var attempt in marks)
                    participant.Jump(attempt);

                return participant as T;
            }
            else if (typeName == typeof(Purple_1.Judge).ToString())
            {
                string name = deser["Name"].ToString();
                int[] marks = deser["Marks"].ToObject<int[]>();

                var judge = new Purple_1.Judge(name, marks);
                return judge as T;
            }
            else if (typeName == typeof(Purple_1.Competition).ToString())
            {
                var judges = deser["Judges"].ToObject<Purple_1.Judge[]>();
                var competition = new Purple_1.Competition(judges);

                foreach (var p in deser["Participants"])
                {
                    string name = p["Name"].ToString();
                    string surname = p["Surname"].ToString();
                    double[] coefs = p["Coefs"].ToObject<double[]>();
                    int[][] marks = p["Marks"].ToObject<int[][]>();

                    var participant = new Purple_1.Participant(name, surname);
                    participant.SetCriterias(coefs);
                    foreach (var attempt in marks)
                        participant.Jump(attempt);

                    competition.Add(participant);
                }

                return competition as T;
            }

            return null;
        }

        // Purple_2
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SerializerJson(jumping, fileName);
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            if (FilePath == null || !File.Exists(FilePath)) return null;

            var content = File.ReadAllText(FilePath);
            var deser = JObject.Parse(content);

            string type = deser["Type"].ToString();
            string name = deser["Name"].ToString();
            int standart = deser["Standard"].ToObject<int>();

            Purple_2.SkiJumping obj = null;

            if (standart == 100)
                obj = new Purple_2.JuniorSkiJumping();
            else if (standart == 150)
                obj = new Purple_2.ProSkiJumping();
            var participant = deser["Participants"].ToObject<Purple_2.Participant[]>();

            for (int i = 0; i < participant.Length; i++)
            {
                participant[i].Jump(deser["Participants"][i]["Distance"].ToObject<int>(), deser["Participants"][i]["Marks"].ToObject<int[]>(), standart);
            }

            obj.Add(participant);

            return obj as T;
        }

        // Purple_3
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SerializerJson(skating, fileName);
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            if (FilePath == null || !File.Exists(FilePath)) return null;

            var content = File.ReadAllText(FilePath);
            var deser = JObject.Parse(content);

            string type = deser["Type"].ToString();
            double[] moods = deser["Moods"].ToObject<double[]>();

            Purple_3.Skating obj = null;

            if (type == typeof(Purple_3.FigureSkating).ToString())
                obj = new Purple_3.FigureSkating(moods, false);
            else if (type == typeof(Purple_3.IceSkating).ToString())
                obj = new Purple_3.IceSkating(moods, false);

            var participant = deser["Participants"].ToObject<Purple_3.Participant[]>();

            for (int i = 0; i < participant.Length; i++)
            {
                var marks = deser["Participants"][i]["Marks"].ToObject<double[]>();
                for (int j = 0; j < marks.Length; j++)
                {
                    participant[i].Evaluate(marks[j]);
                }

            }

            Purple_3.Participant.SetPlaces(participant);
            obj.Add(participant);

            return obj as T;
        }

        // Purple_4
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            SerializerJson(participant, fileName);
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            if (FilePath == null || !File.Exists(FilePath)) return null;

            var content = File.ReadAllText(FilePath);
            var deser = JObject.Parse(content);

            var obj = new Purple_4.Group(deser["Name"].ToString());
            var sportsmen = deser["Sportsmen"].ToObject<Purple_4.Sportsman[]>();

            for (int i = 0; i < sportsmen.Length; i++)
            {
                sportsmen[i].Run(deser["Sportsmen"][i]["Time"].ToObject<double>());
            }

            obj.Add(sportsmen);

            return obj;
        }

        // Purple_5
        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            SerializerJson(group, fileName);
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            if (FilePath == null || !File.Exists(FilePath)) return null;

            var content = File.ReadAllText(FilePath);
            var deser = JObject.Parse(content);

            var researches = deser["Researches"].ToObject<Purple_5.Research[]>();

            for (int i = 0; i < researches.Length; i++)
            {
                var responses = deser["Researches"][i]["Responses"].ToObject<Purple_5.Response[]>();

                foreach (var response in responses)
                {
                    string[] answers = new[] { response.Animal, response.CharacterTrait, response.Concept };
                    researches[i].Add(answers);
                }
            }

            var obj = new Purple_5.Report();
            obj.AddResearch(researches);

            return obj;
        }
    }
}
