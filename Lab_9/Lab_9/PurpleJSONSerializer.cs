using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension => "json";
        
        private void WriteJson(string json)
        {
            File.WriteAllText(FilePath, json);
        }
        private JObject GetDataFromFile()
        {
            return JObject.Parse(File.ReadAllText(FilePath));
        }
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            var jo = JObject.FromObject(obj);
            string type;
            switch (obj)
            {
                case Purple_1.Participant:
                    type = "participant";
                    break;
                case Purple_1.Judge:
                    type = "judge";
                    break;
                case Purple_1.Competition:
                    type = "competition";
                    break;
                default:
                    type = "null";
                    break;
            }
            jo["type"] = type;
            WriteJson(jo.ToString());
        }
        private Purple_1.Participant DeserializePurple1Participant(JObject jo)
        {
            string name = (string)jo["Name"];
            string surname = (string)jo["Surname"];
            double[] coefs = jo["Coefs"].ToObject<double[]>();
            int[,] marks = jo["Marks"].ToObject<int[,]>();
            var participant = new Purple_1.Participant(name, surname);
            participant.SetCriterias(coefs);
            for (int i = 0; i < marks.GetLength(0); i++)
            {
                int[] row = Enumerable
                    .Range(0, marks.GetLength(1))
                    .Select(j => marks[i, j])
                    .ToArray();
                participant.Jump(row);
            }

            return participant;
        }
        private Purple_1.Judge DeserializePurple1Judge(JObject jo)
        {
            string name = (string)jo["Name"];
            int[] marks = jo["Marks"].ToObject<int[]>();
            var judge = new Purple_1.Judge(name, marks);

            return judge;
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var jo = GetDataFromFile();

            string type = (string)jo["type"];
            jo.Remove("type");
            switch (type)
            {
                case "participant":
                    return DeserializePurple1Participant(jo) as T;
                case "judge":
                    return DeserializePurple1Judge(jo) as T;
                case "competition":
                    var joJudges = jo["Judges"].ToObject<JObject[]>();
                    var judges = new Purple_1.Judge[joJudges.Length];
                    for (int i = 0; i < joJudges.Length; i++)
                    {
                        judges[i] = DeserializePurple1Judge(joJudges[i]);
                    }

                    var joParticipants = jo["Participants"].ToObject<JObject[]>();
                    var participants = new Purple_1.Participant[joParticipants.Length];
                    for (int i = 0; i < joParticipants.Length; i++)
                    {
                        participants[i] = DeserializePurple1Participant(joParticipants[i]);
                    }

                    var competition = new Purple_1.Competition(judges);
                    competition.Add(participants);

                    return competition as T;
                default:
                    return null as T;
            }
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            var jo = JObject.FromObject(jumping);
            string type;
            switch (jumping) {
                case Purple_2.JuniorSkiJumping:
                    type = "JuniorSkiJumping";
                    break;
                case Purple_2.ProSkiJumping:
                    type = "ProSkiJumping";
                    break;
                default:
                    type = "null";
                    break;
            }
            jo["type"] = type;
            WriteJson(jo.ToString());
        }
        private Purple_2.Participant DeserializePurple2Participant(JObject jo, int standard)
        {
            string name = (string)jo["Name"];
            string surname = (string)jo["Surname"];
            int distance = jo["Distance"].ToObject<int>();
            int[] marks = jo["Marks"].ToObject<int[]>();

            var participant = new Purple_2.Participant(name, surname);
            participant.Jump(distance, marks, standard);

            return participant;
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var jo = GetDataFromFile();

            string type = (string)jo["type"];
            Purple_2.SkiJumping jumping;
            switch (type)
            {
                case "JuniorSkiJumping":
                    jumping = new Purple_2.JuniorSkiJumping();
                    break;
                case "ProSkiJumping":
                    jumping = new Purple_2.ProSkiJumping();
                    break;
                default:
                    return null as T;
            }
            var joParticipants = jo["Participants"].ToObject<JObject[]>();
            for (int i = 0; i < joParticipants.Length; i++)
            {
                jumping.Add(DeserializePurple2Participant(joParticipants[i], jumping.Standard));
            }
            
            return jumping as T;
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            var jo = JObject.FromObject(skating);

            string type;
            switch (skating)
            {
                case Purple_3.IceSkating:
                    type = "IceSkating";
                    break;
                case Purple_3.FigureSkating:
                    type = "FigureSkating";
                    break;
                default:
                    type = "null";
                    break;
            }
            jo["type"] = type;

            WriteJson(jo.ToString());
        }
        private Purple_3.Participant DeserializePurple3Participant(JObject jo)
        {
            string name = (string)jo["Name"];
            string surname = (string)jo["Surname"];
            double[] marks = jo["Marks"].ToObject<double[]>();

            var participant = new Purple_3.Participant(name, surname);
            for (int i = 0; i < marks.Length; i++) participant.Evaluate(marks[i]);

            return participant;
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var jo = GetDataFromFile();

            string type = (string)jo["type"];
            double[] moods = jo["Moods"].ToObject<double[]>();
            Purple_3.Skating skating;
            switch (type)
            {
                case "IceSkating":
                    skating = new Purple_3.IceSkating(moods, false);
                    break;
                case "FigureSkating":
                    skating = new Purple_3.FigureSkating(moods, false);
                    break;
                default:
                    return null as T;
            }

            var joParticipants = jo["Participants"].ToObject<JObject[]>();
            for (int i = 0; i < joParticipants.Length; i++)
            {
                skating.Add(DeserializePurple3Participant(joParticipants[i]));
            }
            Purple_3.Participant.SetPlaces(skating.Participants);

            return skating as T;
        }
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            SelectFile(fileName);
            var jo = JObject.FromObject(participant);
            WriteJson(jo.ToString());
        }
        private Purple_4.Sportsman DeserializePurple4Sportsman(JObject jo)
        {
            string name = (string)jo["Name"];
            string surname = (string)jo["Surname"];
            double time = jo["Time"].ToObject<double>();

            var sportsman = new Purple_4.Sportsman(name, surname);
            sportsman.Run(time);

            return sportsman;
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var jo = GetDataFromFile();

            string name = (string)jo["Name"];
            var joSportsmen = jo["Sportsmen"].ToObject<JObject[]>();
            var group = new Purple_4.Group(name);
            for (int i = 0; i < joSportsmen.Length; i++)
            {
                group.Add(DeserializePurple4Sportsman(joSportsmen[i]));
            }

            return group;
        }
        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            SelectFile(fileName);
            var jo = JObject.FromObject(group);
            WriteJson(jo.ToString());
        }
        private string[] DeserializePurple5Response(JObject jo)
        {
            string animal = jo["Animal"].ToString() == "" ? null : jo["Animal"].ToString();
            string characterTrait = jo["CharacterTrait"].ToString() == "" ? null : jo["CharacterTrait"].ToString();
            string concept = jo["Concept"].ToString() == "" ? null : jo["Concept"].ToString();

            return new string[3] { animal, characterTrait, concept };
        }
        private Purple_5.Research DeserializePurple5Research(JObject jo)
        {
            string name = (string)jo["Name"];
            var joResponses = jo["Responses"].ToObject<JObject[]>();
            var research = new Purple_5.Research(name);
            for (int i = 0; i < joResponses.Length; i++)
            {
                research.Add(DeserializePurple5Response(joResponses[i]));
            }

            return research;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var jo = GetDataFromFile();

            var joResearches = jo["Researches"].ToObject<JObject[]>();
            var report = new Purple_5.Report();
            for (int i = 0; i < joResearches.Length; i++)
            {
                report.AddResearch(DeserializePurple5Research(joResearches[i]));
            }

            return report;
        }
    }
}
