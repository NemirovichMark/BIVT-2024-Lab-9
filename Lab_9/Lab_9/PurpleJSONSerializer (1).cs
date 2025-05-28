using Lab_7;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using static Lab_7.Purple_3;


namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {

        public override string Extension 
        {
            get
            {
                return "json";
            }
        }
        private void Serialize<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            string json_object = JsonConvert.SerializeObject(obj);
            var json = JObject.Parse(json_object);

            json.Add("type", obj.GetType().Name);

            File.WriteAllText(FilePath, json.ToString());
        }
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            Serialize(obj, fileName);
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);

            string type = json["type"].ToString();
            json.Remove("type");//удаление поля тип

            T obj = default;

            switch (type)
            {
                case "Participant":
                    if (json["Name"] != null && json["Surname"] != null && json["Coefs"] != null && json["Marks"] != null)
                    {
                        string name = json["Name"].ToString();
                        string surname = json["Surname"].ToString();
                        double[] coefs = json["Coefs"].ToObject<double[]>();
                        int[,] marks = json["Marks"].ToObject<int[,]>();

                        var participant = new Purple_1.Participant(name, surname);

                        participant.SetCriterias(coefs);
                        for (int i = 0; i < marks.GetLength(0); i++)
                        {
                            int[] jump = new int[marks.GetLength(1)];

                            for (int j = 0; j < marks.GetLength(1); j++) jump[j] = marks[i, j];
                            participant.Jump(jump);
                        }
                        obj = participant as T;
                    }
                    break;

                case "Judge":
                    if (json["Name"] != null && json["Marks"] != null)
                    {
                        string name = json["Name"].ToString();
                        int[] marks = json["Marks"].ToObject<int[]>();

                        var judge = new Purple_1.Judge(name, marks);
                        obj = judge as T;
                    }
                    break;

                case "Competition":
                    if (json["Judges"] != null && json["Participants"] != null)
                    {
                        //Purple_1.Judge[] judges = json["Judges"].ToObject<Purple_1.Judge[]>();
                        //Purple_1.Participant[] participants = json["Participants"].ToObject<Purple_1.Participant[]>();
                        var judgesj = json["Judges"].ToObject<JObject[]>();
                        var judges = new Purple_1.Judge[judgesj.Length];
                        for (int i = 0; i < judgesj.Length; i++)
                        {
                            if (judgesj[i]["Name"] != null && judgesj[i]["Marks"] != null)
                            {
                                string name = judgesj[i]["Name"].ToString();
                                int[] marks = judgesj[i]["Marks"].ToObject<int[]>();
                                judges[i] = new Purple_1.Judge(name, marks);
                            }
                        }

                        var participantj = json["Participants"].ToObject<JObject[]>();
                        var participant = new Purple_1.Participant[participantj.Length];
                        for (int i = 0; i < participantj.Length; i++)
                        {
                            if (participantj[i]["Name"] != null && participantj[i]["Surname"] != null && participantj[i]["Coefs"] != null && participantj[i]["Marks"] != null)
                            {
                                string name = participantj[i]["Name"].ToString();
                                string surname = participantj[i]["Surname"].ToString(); 
                                double[] coefs = participantj[i]["Coefs"].ToObject<double[]>();
                                int[,] marks = participantj[i]["Marks"].ToObject<int[,]>();
                                var participantObj = new Purple_1.Participant(name, surname);
                                participantObj.SetCriterias(coefs);

                                for (int j = 0; j < marks.GetLength(0); j++)
                                {
                                    int[] jump = new int[marks.GetLength(1)];
                                    for (int k = 0; k < marks.GetLength(1); k++)
                                    {
                                        jump[k] = marks[j, k];
                                    }
                                    participantObj.Jump(jump);
                                }
                                participant[i] = participantObj;
                            }
                        }

                        var competition = new Purple_1.Competition(judges);
                        competition.Add(participant);

                        obj = competition as T;
                    }
                    break;

                default:

                    obj = default;
                    break;
            }
        
            return obj;
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            Serialize(jumping, fileName);
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);

            string type = json["type"].ToString();
            json.Remove("type");

             string name = json["Name"].ToString();
             int standard = int.Parse(json["Standard"].ToString());

            T jumping;

            if (type == "JuniorSkiJumping")
            {
                jumping = new Purple_2.JuniorSkiJumping() as T;

            }
            else if (type == "ProSkiJumping")
            {
                jumping = new Purple_2.ProSkiJumping() as T;
            }
            else return null;

            var participants = json["Participants"].ToObject<JObject[]>(); //массив участников в json
            var participants1 = new Purple_2.Participant[participants.Length];//массив участников

            for (int i = 0; i < participants.Length; i++)
            {
                string name_p = participants[i]["Name"].ToString(); // Имя участника
                string surname_p = participants[i]["Surname"].ToString(); // Фамилия участника
                int distance_p = participants[i]["Distance"].ToObject<int>(); // Дистанция
                int[] marks_p = participants[i]["Marks"].ToObject<int[]>(); // Оценки

                var participant = new Purple_2.Participant(name_p, surname_p); 
                participant.Jump(distance_p, marks_p, standard);
                participants1[i] = participant;
            }
            jumping.Add(participants1);
            return jumping;
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            Serialize(skating, fileName);
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);

            string type = json["type"].ToString();
            json.Remove("type");

            var moods = json["Moods"].ToObject<double[]>();

            T skating;

            if (type == "FigureSkating")
            {
                skating = new Purple_3.FigureSkating(moods, false) as T;
            }
            else if (type == "IceSkating")
            {
                skating = new Purple_3.IceSkating(moods, false) as T;
            }
            else
            {
                return null;
            }

            
            var participants = json["Participants"].ToObject<JObject[]>();

           
            var participants1 = new Purple_3.Participant[participants.Length];
            for (int i = 0; i < participants.Length; i++)
            {
                string name_p = participants[i]["Name"].ToString();
                string surname_p = participants[i]["Surname"].ToString();
                double[] marks_p = participants[i]["Marks"].ToObject<double[]>();

                var participant = new Purple_3.Participant(name_p, surname_p);

                
                for (int j = 0; j < marks_p.Length; j++)
                    {
                        participant.Evaluate(marks_p[j]);
                    }
                
                participants1[i] = participant;
            }

            skating.Add(participants1);
            return skating;
        }
        

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            Serialize(group, fileName);
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);

            //string type = json["type"].ToString();
            //json.Remove("type");

            string name = json["Name"].ToString();
            var group = new Purple_4.Group(name);

            var sportsmen = json["Sportsmen"].ToObject<JObject[]>();
            var sportsmen1 = new Purple_4.Sportsman[sportsmen.Length];
            for (int i=0;  i<sportsmen1.Length; i++)
            {
                string name_s = sportsmen[i]["Name"].ToString();
                string surname_s = sportsmen[i]["Surname"].ToString();
                double time_s = sportsmen[i]["Time"].ToObject<double>();

                var sportsman = new Purple_4.Sportsman(name_s, surname_s);
                sportsman.Run(time_s);
                sportsmen1[i] = sportsman;

            }
            group.Add(sportsmen1 );
            return group;

        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            Serialize(report, fileName);
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);

            //string type = json["type"].ToString();
            //json.Remove("type");

            var researches = json["Researches"].ToObject<JObject[]>();
            var researches1 = new Purple_5.Research[researches.Length];
            for (int i=0;  i<researches.Length; i++)
            {
                string name = researches[i]["Name"].ToString();
                var responses = researches[i]["Responses"].ToObject<JObject[]>();

                var research = new Purple_5.Research(name);
                var s = new StringBuilder();
                // var responses1 = new Purple_5.Response[responses.Length];
                foreach (var response in responses)
                {
                    string animal = response["Animal"].ToString();
                    string character = response["CharacterTrait"].ToString();
                    string concept = response["Concept"].ToString();

                    animal = string.IsNullOrEmpty(animal) ? null : animal;
                    character = string.IsNullOrEmpty(character) ? null : character;
                    concept = string.IsNullOrEmpty(concept) ? null : concept;

                    research.Add(new string[] { animal, character, concept });
                    researches1[i] = research;
                }
                
            }
            var report = new Purple_5.Report();
            report.AddResearch(researches1);

            return report;
        }
        
    }
}
