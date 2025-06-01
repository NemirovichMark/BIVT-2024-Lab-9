using Lab_7;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Lab_7.Purple_3;

namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension => "json";
        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            IncludeFields = true,
            WriteIndented = true
        };
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            var json = JObject.FromObject(obj);
            json.Add("Type", obj.GetType().Name);
            SelectFile(fileName);
            File.WriteAllText(FilePath, json.ToString());
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            string content = File.ReadAllText(FilePath);
            var data = JObject.Parse(content);

            switch (data["Type"].ToString())
            {
                case "Participant":
                    return (T)(object)CreateParticipant(data);
                case "Judge":
                    return (T)(object)CreateJudge(data);
                case "Competition":
                    var comp = new Purple_1.Competition(
                        data["Judges"].ToObject<JObject[]>()
                            .Select(j => CreateJudge(j)).ToArray());
                    comp.Add(data["Participants"].ToObject<JObject[]>()
                        .Select(p => CreateParticipant(p)).ToArray());
                    return (T)(object)comp;
                default:
                    return default(T);
            }
        }

        private Purple_1.Participant CreateParticipant(JObject data)
        {
            var part = new Purple_1.Participant(
                data["Name"].ToString(),
                data["Surname"].ToString());
            part.SetCriterias(data["Coefs"].ToObject<double[]>());
            int[][] scores = data["Marks"].ToObject<int[][]>();
            for (int i = 0; i < scores.Length; i++)
            {
                part.Jump(scores[i]);
            }
            return part;
        }

        private Purple_1.Judge CreateJudge(JObject data)
        {
            return new Purple_1.Judge(
                data["Name"].ToString(),
                data["Marks"].ToObject<int[]>());
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            var json = JObject.FromObject(jumping);
            json.Add("Type", jumping.GetType().Name);
            SelectFile(fileName);
            File.WriteAllText(FilePath, json.ToString());
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            string fileContent = File.ReadAllText(FilePath);
            var parsedJson = JObject.Parse(fileContent);
            Console.WriteLine(parsedJson);

            Purple_2.SkiJumping jumpingInstance;
            string jumpType = parsedJson["Type"].ToString();

            if (jumpType == "JuniorSkiJumping")
                jumpingInstance = new Purple_2.JuniorSkiJumping();
            else if (jumpType == "ProSkiJumping")
                jumpingInstance = new Purple_2.ProSkiJumping();
            else
                return default(T);

            var participantsData = parsedJson["Participants"].ToObject<JObject[]>();
            var loadedParticipants = new Purple_2.Participant[participantsData.Length];

            for (int i = 0; i < participantsData.Length; i++)
            {
                var participantJson = participantsData[i];
                var newParticipant = new Purple_2.Participant(
                    participantJson["Name"].ToString(),
                    participantJson["Surname"].ToString());

                newParticipant.Jump(
                    participantJson["Distance"].ToObject<int>(),
                    participantJson["Marks"].ToObject<int[]>(),
                    parsedJson["Standard"].ToObject<int>());

                loadedParticipants[i] = newParticipant;
            }

            jumpingInstance.Add(loadedParticipants);
            return (T)(object)jumpingInstance;
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            var json = JObject.FromObject(skating);
            json.Add("Type", skating.GetType().Name);
            SelectFile(fileName);
            File.WriteAllText(FilePath, json.ToString());
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            string fileData = File.ReadAllText(FilePath);
            var parsedData = JObject.Parse(fileData);
            Console.WriteLine(parsedData);

            Purple_3.Skating skating;
            string skatingType = parsedData["Type"].ToString();
            double[] moodValues = parsedData["Moods"].ToObject<double[]>();

            if (skatingType == "IceSkating")
                skating = new Purple_3.IceSkating(moodValues, false);
            else if (skatingType == "FigureSkating")
                skating = new Purple_3.FigureSkating(moodValues, false);
            else
                return default(T);

            var participantsArray = parsedData["Participants"].ToObject<JObject[]>();
            var loadedParticipants = new Purple_3.Participant[participantsArray.Length];

            for (int i = 0; i < participantsArray.Length; i++)
            {
                var current = participantsArray[i];
                var participant = new Purple_3.Participant(
                    current["Name"].ToString(),
                    current["Surname"].ToString());

                double[] scores = current["Marks"].ToObject<double[]>();
                for (int j = 0; j < scores.Length; j++)
                    participant.Evaluate(scores[j]);

                loadedParticipants[i] = participant;
            }

            skating.Add(loadedParticipants);
            Purple_3.Participant.SetPlaces(skating.Participants);
            return (T)(object)skating;
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            var json = JObject.FromObject(group);
            json.Add("Type", group.GetType().Name);
            SelectFile(fileName);
            File.WriteAllText(FilePath, json.ToString());
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            string content = File.ReadAllText(FilePath);
            var data = JObject.Parse(content);
            Console.WriteLine(data);

            var newGroup = new Purple_4.Group(data["Name"].ToString());
            var sportsmenData = data["Sportsmen"].ToObject<JObject[]>();
            var sportsmenList = new Purple_4.Sportsman[sportsmenData.Length];

            for (int i = 0; i < sportsmenData.Length; i++)
            {
                var sportsmanInfo = sportsmenData[i];
                var athlete = new Purple_4.Sportsman(
                    sportsmanInfo["Name"].ToString(),
                    sportsmanInfo["Surname"].ToString());

                athlete.Run(sportsmanInfo["Time"].ToObject<double>());
                sportsmenList[i] = athlete;
            }

            newGroup.Add(sportsmenList);
            return newGroup;
        }
        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            var json = JObject.FromObject(group);
            json.Add("Type", group.GetType().Name);
            SelectFile(fileName);
            File.WriteAllText(FilePath, json.ToString());
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            string fileData = File.ReadAllText(FilePath);
            var jsonObj = JObject.Parse(fileData);
            Console.WriteLine(jsonObj);

            var outputReport = new Purple_5.Report();
            var researchItems = jsonObj["Researches"].ToObject<JObject[]>();

            for (int i = 0; i < researchItems.Length; i++)
            {
                var currentResearch = new Purple_5.Research(researchItems[i]["Name"].ToString());
                var responseItems = researchItems[i]["Responses"].ToObject<JObject[]>();

                for (int j = 0; j < responseItems.Length; j++)
                {
                    string animalValue = GetValueOrNull(responseItems[j], "Animal");
                    string traitValue = GetValueOrNull(responseItems[j], "CharacterTrait");
                    string conceptValue = GetValueOrNull(responseItems[j], "Concept");

                    currentResearch.Add(new string[] { animalValue, traitValue, conceptValue });
                }
                outputReport.AddResearch(currentResearch);
            }

            return outputReport;
        }
        private string GetValueOrNull(JObject obj, string propertyName)
        {
            if (obj.TryGetValue(propertyName, out var token) &&
                token != null &&
                !string.IsNullOrEmpty(token.ToString()))
            {
                return token.ToString();
            }
            return null;
        }
    }
}
