using Lab_7;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab_7.Purple_3;

namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension => "json";

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);

            string json_obj = JsonConvert.SerializeObject(obj);

            var json = JObject.Parse(json_obj);

            json.Add("type", obj.GetType().Name);
 

            File.WriteAllText(FilePath, json.ToString());
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);

            string json_obj = JsonConvert.SerializeObject(jumping);

            var json = JObject.Parse(json_obj);

            json.Add("type", jumping.GetType().Name);


            File.WriteAllText(FilePath, json.ToString());
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);

            string json_obj = JsonConvert.SerializeObject(skating);

            var json = JObject.Parse(json_obj);

            json.Add("type", skating.GetType().Name);


            File.WriteAllText(FilePath, json.ToString());
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);

            string json_obj = JsonConvert.SerializeObject(group);

            var json = JObject.Parse(json_obj);

            File.WriteAllText(FilePath, json.ToString());
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);

            string json_obj = JsonConvert.SerializeObject(report);

            var json = JObject.Parse(json_obj);

            File.WriteAllText(FilePath, json.ToString());
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);

            string text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);

            string type = json["type"].ToString();
            json.Remove("type");

            T obj;

            switch(type)
            {
                case "Participant":
                    obj = DeserializePurple_1Participant(json) as T; 
                    break;
                case "Judge":
                    obj = DeserializePurple_1Judge(json) as T;
                    break;
                case "Competition":
                    obj = DeserializePurple_1Competition(json) as T;
                    break;
                default:
                    obj = null;
                    break;
            }

            return obj;
        }

        private Purple_1.Participant DeserializePurple_1Participant(JObject JObj)
        {
            string name = JObj["Name"].ToString();
            string surname = JObj["Surname"].ToString();
            double[] coefs = JObj["Coefs"].ToObject<double[]>();
            int[,] marks = JObj["Marks"].ToObject<int[,]>();

            var participant = new Purple_1.Participant(name, surname);

            participant.SetCriterias(coefs);

            for (int i = 0; i < marks.GetLength(0); i++)
            {
                int[] jump = new int[marks.GetLength(1)];

                for (int j = 0; j < marks.GetLength(1); j++) jump[j] = marks[i, j];
                participant.Jump(jump);
            }

            return participant;
        }

        private Purple_1.Judge DeserializePurple_1Judge(JObject JObj)
        {
            string name = JObj["Name"].ToString();
            int[] marks = JObj["Marks"].ToObject<int[]>();

            Purple_1.Judge judge = new Purple_1.Judge(name, marks);
            return judge;
        }

        private Purple_1.Competition DeserializePurple_1Competition(JObject JObj)
        {
            var judges_json = JObj["Judges"].ToObject<JObject[]>();
            var judges = new Purple_1.Judge[judges_json.Length];

            for (int i = 0; i < judges_json.Length; i++)
                judges[i] = DeserializePurple_1Judge(judges_json[i]);

            var participants_json = JObj["Participants"].ToObject<JObject[]>();
            var participants = new Purple_1.Participant[participants_json.Length];

            for (int i = 0;i < participants_json.Length;i++)
                participants[i] = DeserializePurple_1Participant(participants_json[i]);

            var competition = new Purple_1.Competition(judges);
            competition.Add(participants);

            return competition;
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);

            string text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);

            string type = json["type"].ToString();
            json.Remove("type");

            string name = json["Name"].ToString();
            int standard = json["Standard"].ToObject<int>();

            T jumping;

            switch(type)
            {
                case "JuniorSkiJumping":
                    jumping = new Purple_2.JuniorSkiJumping() as T;
                    break;
                case "ProSkiJumping":
                    jumping = new Purple_2.ProSkiJumping() as T;
                    break;
                default:
                    jumping = null;
                    break;
            }

            var participants_json = json["Participants"].ToObject<JObject[]>();
            var participants = new Purple_2.Participant[participants_json.Length];

            for (int i = 0; i < participants_json.Length; i++)
                participants[i] = DeserializePurple_2Participant(participants_json[i], standard);

            jumping.Add(participants);

            return jumping;
        }

        private Purple_2.Participant DeserializePurple_2Participant(JObject JObj, int standard)
        {
            string name = JObj["Name"].ToString();
            string surname = JObj["Surname"].ToString();
            int distance = JObj["Distance"].ToObject<int>();
            int result = JObj["Result"].ToObject<int>();
            int[] marks = JObj["Marks"].ToObject<int[]>();

            var participant = new Purple_2.Participant(name, surname);
            participant.Jump(distance, marks, standard);

            return participant;
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);

            string text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);

            string type = json["type"].ToString();
            json.Remove("type");

            double[] moods = json["Moods"].ToObject<double[]>();

            T skating;

            switch(type)
            {
                case "FigureSkating":
                    skating = new Purple_3.FigureSkating(moods, false) as T; 
                    break;
                case "IceSkating":
                    skating = new Purple_3.IceSkating(moods, false) as T;
                    break;
                default:
                    skating = null;
                    break;
            }

            var participants_json = json["Participants"].ToObject<JObject[]>();
            var participants = new Purple_3.Participant[participants_json.Length];

            for (int i = 0; i < participants_json.Length; i++)
                participants[i] = DeserializePurple_3Participant(participants_json[i]);

            skating.Add(participants);
            return skating;
        }

        private Purple_3.Participant DeserializePurple_3Participant(JObject JObj)
        {
            string name = JObj["Name"].ToString();
            string surname = JObj["Surname"].ToString();
            double[] marks = JObj["Marks"].ToObject<double[]>();

            var participant = new Purple_3.Participant(name, surname);

            for (int i = 0; i < marks.Length; i++) 
                participant.Evaluate(marks[i]);

            return participant;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);

            string text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);

            string name = json["Name"].ToString();

            var sportsmen_json = json["Sportsmen"].ToObject<JObject[]>();
            var sportsmen = new Purple_4.Sportsman[sportsmen_json.Length];

            for (int i = 0;i < sportsmen_json.Length;i++) 
                sportsmen[i] = DeserializePurple_4Sportsman(sportsmen_json[i]);

            var group = new Purple_4.Group(name);
            group.Add(sportsmen);

            return group;
        }

        private Purple_4.Sportsman DeserializePurple_4Sportsman(JObject JObj)
        {
            string name = JObj["Name"].ToString();
            string surname = JObj["Surname"].ToString();
            double time = JObj["Time"].ToObject<double>();

            var sportsman = new Purple_4.Sportsman(name, surname);
            sportsman.Run(time);

            return sportsman;
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);

            string text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);

            var researches_json = json["Researches"].ToObject<JObject[]>();
            var researches = new Purple_5.Research[researches_json.Length];

            for (int i = 0; i < researches_json.Length; i++)
                researches[i] = DeserializePurple_5Research(researches_json[i]);

            var report = new Purple_5.Report();
            report.AddResearch(researches);

            return report;
        }

        private string[] GetAnswers(JObject JObj)
        {
            string animal = JObj["Animal"].ToString();
            string characterTrait = JObj["CharacterTrait"].ToString();
            string concept = JObj["Concept"].ToString() ;

            if (string.IsNullOrEmpty(animal)) animal = null;
            if (string.IsNullOrEmpty(characterTrait)) characterTrait = null;
            if (string.IsNullOrEmpty(concept)) concept = null;

            return new string[] { animal, characterTrait, concept };
        }
        
        private Purple_5.Research DeserializePurple_5Research(JObject JObj)
        {
            string name = JObj["Name"].ToString();
            var responses_json = JObj["Responses"].ToObject<JObject[]>();

            var research = new Purple_5.Research(name);

            for (int i = 0; i < responses_json.Length; i++)
                research.Add(GetAnswers(responses_json[i]));

            return research;
        }
    }
}
