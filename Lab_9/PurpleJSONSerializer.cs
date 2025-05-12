using Lab_9;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_7;

namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        //свойство
        public override string Extension => "json";

        //методы
        private void Serialize<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            
            if (FilePath == null || obj == null) return;

            string serializedData = JsonConvert.SerializeObject(obj);
            var jObj = JObject.Parse(serializedData);
            jObj["$type"] = obj.GetType().AssemblyQualifiedName;
            serializedData = jObj.ToString();

            using (var writer = new StreamWriter(FilePath)) {
                writer.Write(serializedData);
            }
        }
        private JObject ReadJsonAsJObject()
        {
            if (FilePath == null || !File.Exists(FilePath)) return null;

            string json = File.ReadAllText(FilePath);

            return JObject.Parse(json);
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            Serialize(obj, fileName);
        }
        
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            Serialize(jumping, fileName);
        }
        
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            Serialize(skating, fileName);
        }
        
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            Serialize(participant, fileName);
        }

        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            Serialize(group, fileName);
        }
          
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            JObject data = ReadJsonAsJObject();
            if (data == null) return null;

            if (typeof(T) == typeof(Purple_1.Participant))
            {
                string Name = data["Name"]?.Value<string>();
                string Surname = data["Surname"]?.Value<string>();
                double[] Coefs = data["Coefs"]?.ToObject<double[]>();
                int[][] Marks = data["Marks"]?.ToObject<int[][]>();

                var result = new Purple_1.Participant(Name, Surname);
                result.SetCriterias(Coefs);

                foreach (int[] jump in Marks)
                {
                    result.Jump(jump);
                }

                return result as T;
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                string Name = data["Name"].Value<string>();
                int[] Marks = data["Marks"].ToObject<int[]>();

                Purple_1.Judge result = new Purple_1.Judge(Name, Marks);
                return result as T;
            }
            else
            {
                JToken dataJudges = data["Judges"];
                Purple_1.Judge[] judges = [];
                foreach (JToken dataJudge in dataJudges)
                {
                    string Name = dataJudge["Name"].Value<string>();
                    int[] Marks = dataJudge["Marks"].ToObject<int[]>();

                    Purple_1.Judge judge = new Purple_1.Judge(Name, Marks);

                    Array.Resize(ref judges, judges.Length + 1);
                    judges[judges.Length - 1] = judge;
                }

                var result = new Purple_1.Competition(judges);

                JToken Participants = data["Participants"];
                foreach (var dataParticipant in Participants)
                {
                    string Name = dataParticipant["Name"].Value<string>();
                    string Surname = dataParticipant["Surname"].Value<string>();
                    double[] Coefs = dataParticipant["Coefs"].ToObject<double[]>();
                    int[][] Marks = dataParticipant["Marks"].ToObject<int[][]>();

                    var participant = new Purple_1.Participant(Name, Surname);
                    participant.SetCriterias(Coefs);

                    foreach (int[] jump in Marks)
                    {
                        participant.Jump(jump);
                    }

                    result.Add(participant);
                }

                return result as T;
            }
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            JObject data = ReadJsonAsJObject();
            if (data == null) return null;

            JToken dataParticipants = data["Participants"];
            Purple_2.Participant[] participants = [];
            
            foreach (JToken dataParticipant in dataParticipants)
            {
                if (dataParticipant == null) continue;

                string Name = dataParticipant["Name"].Value<string>();
                string Surname = dataParticipant["Surname"].Value<string>();

                var participant = new Purple_2.Participant(Name, Surname);

                int Distance = dataParticipant["Distance"].Value<int>();
                int[] Marks = dataParticipant["Marks"].ToObject<int[]>();
                int Result = dataParticipant["Result"].Value<int>();

                int Target = (int) Math.Ceiling(Distance - (Result - (Marks.Sum() - Marks.Min() - Marks.Max()) - 60) / 2.0);

                participant.Jump(Distance, Marks, Target);

                Array.Resize(ref participants, participants.Length + 1);
                participants[participants.Length - 1] = participant;
            }

            string Type = data["$type"]?.Value<string>() ?? "SkiJumping";
            if (Type.Contains("ProSkiJumping"))
            {
                var pro = new Purple_2.ProSkiJumping();
                pro.Add(participants);
                return pro as T;
            }
            else
            {
                var junior = new Purple_2.JuniorSkiJumping();
                junior.Add(participants);
                return junior as T;
            }
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            JObject data = ReadJsonAsJObject();
            if (data == null) return null;

            double[] Moods = data["Moods"].ToObject<double[]>();

            JToken dataParticipants = data["Participants"];
            Purple_3.Participant[] participants = [];
            foreach (var dataParticipant in dataParticipants)
            {
                if (dataParticipant == null) continue;

                string Name = dataParticipant["Name"].Value<string>();
                string Surname = dataParticipant["Surname"].Value<string>();

                Purple_3.Participant participant = new Purple_3.Participant(Name, Surname);

                double[] Marks = dataParticipant["Marks"].ToObject<double[]>();
                foreach (var mark in Marks)
                {
                    participant.Evaluate(mark);
                }
                Array.Resize(ref participants, participants.Length + 1);
                participants[participants.Length - 1] = participant;
            }
            Purple_3.Participant.SetPlaces(participants);

            string Type = data["$type"]?.Value<string>() ?? "Skating";
            if (Type.Contains("FigureSkating"))
            {
                Purple_3.FigureSkating figure = new Purple_3.FigureSkating(Moods, false);
                figure.Add(participants);
                return figure as T;
            }
            else
            {
                Purple_3.IceSkating ice = new Purple_3.IceSkating(Moods, false);
                ice.Add(participants);
                return ice as T;
            }
        }
        
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            JObject data = ReadJsonAsJObject();
            if (data == null) return null;

            string Name = data["Name"].Value<string>();
            Purple_4.Group group = new Purple_4.Group(Name);

            JToken dataSportsmen = data["Sportsmen"];
            foreach (JToken dataSportsman in dataSportsmen)
            {
                if (dataSportsman == null) continue;

                string NameSportsman = dataSportsman["Name"].Value<string>();
                string Surname = dataSportsman["Surname"].Value<string>();
                double Time = dataSportsman["Time"].Value<double>();

                string Type = data["$type"].Value<string>();
                if (Type == null) return null;
                if (Type.Contains("SkiMan"))
                {
                    Purple_4.SkiMan man = new Purple_4.SkiMan(NameSportsman, Surname);
                    man.Run(Time);
                    group.Add(man);
                }
                else if (Type.Contains("SkiWoman"))
                {
                    Purple_4.SkiWoman woman = new Purple_4.SkiWoman(NameSportsman, Surname);
                    woman.Run(Time);
                    group.Add(woman);
                }
                else
                {
                    Purple_4.Sportsman sportsman = new Purple_4.Sportsman(NameSportsman, Surname);
                    sportsman.Run(Time);
                    group.Add(sportsman);
                }
            }

            return group;
        }
        
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            JObject data = ReadJsonAsJObject();
            if (data == null) return null;

            var report = new Purple_5.Report();

            JToken dataResearches = data["Researches"];
            if(dataResearches == null) return report;
            foreach (JToken dataResearch in dataResearches)
            {
                if(dataResearch == null) continue;

                string Name = dataResearch["Name"]?.Value<string>();
                if(Name == null) continue;

                var research = new Purple_5.Research(Name);

                JToken dataResponses = dataResearch["Responses"];
                if(dataResponses != null) {
                    foreach (JToken dataResponse in dataResponses)
                    {
                        if(dataResponse == null) continue;

                        string Animal = dataResponse["Animal"].Value<string>();
                        string CharacterTrait = dataResponse["CharacterTrait"].Value<string>();
                        string Concept = dataResponse["Concept"].Value<string>();

                        research.Add([Animal, CharacterTrait, Concept]);
                    }
                }

                report.AddResearch(research);
            }

            return report;
        }
    }
}