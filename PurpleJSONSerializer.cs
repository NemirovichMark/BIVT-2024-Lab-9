using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Lab_7;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using static Lab_7.Purple_1;
using static Lab_7.Purple_3;
using static Lab_7.Purple_2;
namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension => "json";
        #region Purple_1
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);

            var jsonObj = JObject.FromObject(obj);
            string Type;
            if (obj is Purple_1.Participant p)
            {
                Type = "Participant";
            }
            else if (obj is Purple_1.Judge J)
            {
                Type = "Judge";
            }
            else if (obj is Purple_1.Competition comp)
            {
                Type = "Competition";
            }
            else
            {
                Type = "";
            }
            jsonObj["Type"] = Type;

            File.WriteAllText(FilePath, jsonObj.ToString());



        }
        private Purple_1.Participant deserializeParticipant(JObject jsonObj)
        {
            string name = (string)jsonObj["Name"];
            string surname = (string)jsonObj["Surname"];
            double[] coefs = jsonObj["Coefs"].ToObject<double[]>();
            int[,] marks = jsonObj["Marks"].ToObject<int[,]>();
            Purple_1.Participant participant = new Purple_1.Participant(name, surname);
            participant.SetCriterias(coefs);
            for (int i = 0; i < 4; i++)
            {
                int[] marksArray = new int[7];
                for (int j = 0; j < 7; j++)
                {
                    marksArray[j] = marks[i, j];
                }
                participant.Jump(marksArray);
            }
            return participant;

        }
        private Purple_1.Judge deserializeJudge(JObject jsonObj)
        {
            string name = (string)(jsonObj["Name"]);
            int[] marks = jsonObj["Marks"].ToObject<int[]>();
            var Judge = new Purple_1.Judge(name, marks);
            return Judge;
        }
        private Purple_1.Competition deserializeCompetition(JObject jsonObj)
        {
            var jsonObj_Judges = jsonObj["Judges"].ToObject<JObject[]>();
            Purple_1.Judge[] judges = new Purple_1.Judge[jsonObj_Judges.Length];
            for (int i = 0;i < judges.Length; i++)
            {
                judges[i] = deserializeJudge(jsonObj_Judges[i]);
            }
            var jsonObj_Participants = jsonObj["Participants"].ToObject<JObject[]>();
            Purple_1.Participant[] participants = new Purple_1.Participant[jsonObj_Judges.Length];
            for (int i = 0; i < participants.Length; i++)
            {
                participants[i] = deserializeParticipant(jsonObj_Participants[i]);
            }
            Competition competition = new Competition(judges);
            competition.Add(participants);
            return competition;

        }
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var jsonObj = JObject.Parse(File.ReadAllText(FilePath));

            var type = jsonObj["Type"];

            if (type.Contains("Participant"))
            {
                return deserializeParticipant(jsonObj) as T;
            }
            else if (type.Contains("Judge"))
            {
                return deserializeJudge(jsonObj) as T;
            }
            else if (type.Contains("Competition"))
            {
                return deserializeCompetition(jsonObj) as T;
            }
            else return null;
        }
        #endregion
        #region Purple_2
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            var jsonObj = JObject.FromObject(jumping);
            string Type = "";
            if (jumping is Purple_2.JuniorSkiJumping obj)
            {
                Type = "Junior";
            }
            else if(jumping is Purple_2.ProSkiJumping obj2)
            {
                Type = "Pro";
            }
            jsonObj["Type"] = Type;
            File.WriteAllText(FilePath, jsonObj.ToString());
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var jsonObj = JObject.Parse(File.ReadAllText(FilePath));
            string type = (string)jsonObj["Type"];
            int standard = jsonObj.ToObject<int>();
            Purple_2.SkiJumping p;
            if (type.Contains("Junior"))
                p = new Purple_2.JuniorSkiJumping();
            else
                p = new Purple_2.ProSkiJumping();
            var jsonObj_Participants = jsonObj["Participants"].ToObject<JObject[]>();
            var participants = new Purple_2.Participant[jsonObj_Participants.Length];
            for (int i = 0;i<jsonObj_Participants.Length;i++)
            {
                participants[i] = deserealizeParticipants_2(jsonObj_Participants[i], standard);
            }
            p.Add(participants);
            return p as T;
        }
        private Purple_2.Participant deserealizeParticipants_2(JObject jsonObj, int target)
        {
            string Name = (string)jsonObj["Name"];
            string Surname = (string)jsonObj["Surname"];
            int Distance = jsonObj["Distance"].ToObject<int>();
            int[] Marks = jsonObj["Marks"].ToObject<int[]>();
            var participant = new Purple_2.Participant(Name, Surname);
            participant.Jump(Distance, Marks, target);
            return participant;

        }
        #endregion
        #region Purple_3

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            var jsonObj = JObject.FromObject(skating);
            string Type = "";
            if (skating is Purple_3.FigureSkating p) Type = "FigureSkating";
            else Type = "IceSkating";
            jsonObj["Type"] = Type;
            File.WriteAllText(FilePath, jsonObj.ToString());
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var jsonObj = JObject.Parse(File.ReadAllText(FilePath));
            string Type = (string)jsonObj["Type"];
            double[] Moods = jsonObj.ToObject<double[]>();

            Purple_3.Skating skating;
            if (Type.Contains("FigureSkating"))
            {
                skating = new Purple_3.FigureSkating(Moods, false);
            }
            else
            {
                skating = new Purple_3.IceSkating(Moods, false);
            }
            
            var jsonObj_participants = jsonObj.ToObject<JObject[]>();
            var p = new Purple_3.Participant[jsonObj_participants.Length];
            for (int i = 0; i < jsonObj_participants.Length; i++)
            {
                p[i] = deserealizeParticipants_3(jsonObj_participants[i]);
            }
            skating.Add(p);

            return skating as T;


        }
        private Purple_3.Participant deserealizeParticipants_3(JObject jsonObj)
        {
            string Name = (string)jsonObj["Name"];
            string Surname = (string)jsonObj["Surname"];
            int[] Marks = jsonObj["Marks"].ToObject<int[]>();
            var participant = new Purple_3.Participant(Name, Surname);
            for (int i = 0; i < Marks.Length; i++)
            {
                participant.Evaluate(Marks[i]);
            }

            return participant;

        }

        #endregion
        #region Purple_4
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            SelectFile(fileName);
            var jsonObj = JObject.FromObject(participant);
            File.WriteAllText(fileName, jsonObj.ToString());
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var jsonObj = JObject.Parse(File.ReadAllText(FilePath));
            string nameGroup = (string)jsonObj["Name"];
            Purple_4.Group group = new Purple_4.Group(nameGroup);
            var jsonObj_Sportsmen = jsonObj["Sportsmen"].ToObject<JObject[]>();
            var s = new Purple_4.Sportsman[jsonObj_Sportsmen.Length];
            for (int i = 0;i< s.Length; i++)
            {
                s[i] = deserealizeSportsmen(jsonObj_Sportsmen[i]);
            }
            group.Add(s);
            return group;
        }
        private Purple_4.Sportsman deserealizeSportsmen(JObject jsonObj)
        {
            string Name = (string)(jsonObj["Name"]);
            string Surname = (string)(jsonObj["Surname"]);
            double Time = jsonObj["Time"].ToObject<double>();
            Purple_4.Sportsman s = new Purple_4.Sportsman(Name, Surname);
            s.Run(Time);
            return s;
        }
        #endregion
        #region Purple_5
        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            SelectFile(fileName);
            var jsonObj = JObject.FromObject(group);
            File.WriteAllText(fileName, jsonObj.ToString());
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var jsonObj = JObject.Parse(File.ReadAllText(FilePath));
            var r = new Purple_5.Report();
            var jsonObj_researches = jsonObj["Researches"].ToObject<JObject[]>();
            for (int i = 0; i < jsonObj_researches.Length; i++)
            {
                r.AddResearch(deserializeResearches(jsonObj_researches[i]));
            }
            return r;
        }
        private string[] deserializeResponses(JObject jsonObj)
        {
            string animal = (string)jsonObj[$"Animal"] == "" ? null : (string)jsonObj[$"Animal"];
            string characterTrait = (string)jsonObj[$"characterTrait"] == "" ? null : (string)jsonObj[$"characterTrait"];
            string concept = (string)jsonObj[$"Concept"] == "" ? null : (string)jsonObj[$"Concept"];
            string[] r = new string[3] { animal, characterTrait, concept };
            return r;
        }
        private Purple_5.Research deserializeResearches(JObject jsonObj)
        {
            string nameResearch = (string)jsonObj["Name"];
            var research = new Purple_5.Research(nameResearch);
            var jsonObj_responses = jsonObj["Responses"].ToObject<JObject[]>(); 
            for (int i = 0; i < jsonObj_responses.Length; i++)
            {
                research.Add(deserializeResponses(jsonObj_responses[i]));
            }
            return research;
        }
        #endregion
    }
}