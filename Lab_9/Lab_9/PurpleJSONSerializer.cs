using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;

namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension => "json";
        private void Serializer_json<T>(T obj, string fileName) where T : class
        {
            SelectFile(fileName);
            if (obj == null || FilePath == null) return;
            
            var json = JObject.FromObject(obj);
            json.Add("Type", obj.GetType().ToString());
            File.WriteAllText(FilePath, json.ToString());

        }
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            Serializer_json(obj, fileName);
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            Serializer_json(jumping, fileName);
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            Serializer_json(skating, fileName);
        }
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            Serializer_json(participant, fileName);
        }
        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            Serializer_json(group, fileName);
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            if ( FilePath == null) return null;
            var content = File.ReadAllText(FilePath);
            var deserializedPerson = JObject.Parse(content);
            
            string type = deserializedPerson["Type"].ToString();
            if (type == typeof(Purple_1.Participant).ToString())
            {
                var obj = new Purple_1.Participant(deserializedPerson["Name"].ToString(), deserializedPerson["Surname"].ToString());
                obj.SetCriterias(deserializedPerson["Coefs"].ToObject<double[]>());
                int[,] marks = deserializedPerson["Marks"].ToObject<int[,]>();
                for (int i = 0; i < marks.GetLength(0); i++)
                {
                    int[] jump = new int[7];
                    for (int j = 0; j < marks.GetLength(1); j++)
                    {
                        jump[j] = marks[i, j];
                    }
                    obj.Jump(jump);
                }
                return obj as T;
            }
            else if (type == typeof(Purple_1.Judge).ToString())
            {
                var obj = new Purple_1.Judge(deserializedPerson["Name"].ToString(), deserializedPerson["Marks"].ToObject<int[]>());
                return obj as T;
            }
            else if (type == typeof(Purple_1.Competition).ToString())
            {
                var obj = new Purple_1.Competition(deserializedPerson["Judges"].ToObject<Purple_1.Judge[]>());
                var participants = deserializedPerson["Participants"].ToObject<Purple_1.Participant[]>();
                for (int i = 0; i <participants.Length; i++)
                {
                    var coefs = deserializedPerson["Participants"][i]["Coefs"].ToObject<double[]>();
                    var marks = deserializedPerson["Participants"][i]["Marks"].ToObject<int[,]>();
                    participants[i].SetCriterias(coefs);
                    for (int j = 0; j <4; j++)
                    {
                        var marksline = new int[7];
                        for (int k = 0; k < 7; k++)
                            marksline[k] = marks[j, k];
                        participants[i].Jump(marksline);
                    }
                }
                obj.Add(participants);
                return obj as T;
            }
            else return null;
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            if (FilePath == null) return null;
            var content = File.ReadAllText(FilePath);
            var deserializedPerson = JObject.Parse(content);

            string type = deserializedPerson["Type"].ToString();
            string name = deserializedPerson["Name"].ToString();
            int standart = deserializedPerson["Standard"].ToObject<int>();
            Purple_2.SkiJumping obj = null ;
            if (standart == 100)
                obj = new Purple_2.JuniorSkiJumping();
            else if (standart == 150)
                obj = new Purple_2.ProSkiJumping();
            var participant = deserializedPerson["Participants"].ToObject<Purple_2.Participant[]>();
            for (int i = 0; i < participant.Length; i++)
            {
                participant[i].Jump(deserializedPerson["Participants"][i]["Distance"].ToObject<int>(), deserializedPerson["Participants"][i]["Marks"].ToObject<int[]>(), standart);
            }
            obj.Add(participant);
            return obj as T;
            
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            if (FilePath == null) return null;
            var content = File.ReadAllText(FilePath);
            var deserializedPerson = JObject.Parse(content);

            string type = deserializedPerson["Type"].ToString();

            double[] moods = deserializedPerson["Moods"].ToObject<double[]>();
            Purple_3.Skating obj = null;
            if (type == typeof(Purple_3.FigureSkating).ToString())
                obj = new Purple_3.FigureSkating(moods,false);
            else if (type == typeof(Purple_3.IceSkating).ToString())
                obj = new Purple_3.IceSkating(moods,false);
            var participant = deserializedPerson["Participants"].ToObject<Purple_3.Participant[]>();
            for (int i = 0; i < participant.Length; i++)
            {
                var marks = deserializedPerson["Participants"][i]["Marks"].ToObject<double[]>();
                for (int j = 0; j < marks.Length; j++) 
                { 
                    participant[i].Evaluate(marks[j]);
                }

            }
            Purple_3.Participant.SetPlaces(participant);
            obj.Add(participant);
            return obj as T;
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            if (FilePath == null) return null;
            var content = File.ReadAllText(FilePath);
            var deserializedPerson = JObject.Parse(content);
            var obj = new Purple_4.Group(deserializedPerson["Name"].ToString());
            var sportsmen = deserializedPerson["Sportsmen"].ToObject<Purple_4.Sportsman[]>();
            for (int i=0; i<sportsmen.Length; i++)
            {
                sportsmen[i].Run(deserializedPerson["Sportsmen"][i]["Time"].ToObject<double>());
            }
            obj.Add(sportsmen);
            return obj;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            if (FilePath == null) return null;
            var content = File.ReadAllText(FilePath);
            var deserializedPerson = JObject.Parse(content);
            var researches = deserializedPerson["Researches"].ToObject<Purple_5.Research[]>();
            for (int i =0; i< researches.Length; i++)
            {
                var responses = deserializedPerson["Researches"][i]["Responses"].ToObject<Purple_5.Response[]>();
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

