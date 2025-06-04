using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lab_7;
using Newtonsoft.Json.Linq;
using static Lab_7.Purple_1;

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
            string typeIdentifier = string.Empty;

            if (obj is Participant)
                typeIdentifier = "Participant";
            else if (obj is Judge)
                typeIdentifier = "Judge";
            else if (obj is Competition)
                typeIdentifier = "Competition";

            jsonObj["ObjectKind"] = typeIdentifier;
            File.WriteAllText(FilePath, jsonObj.ToString());
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var jsonObj = JObject.Parse(File.ReadAllText(FilePath));
            string objectKind = (string)jsonObj["ObjectKind"];

            if (objectKind == "Participant")
                return (T)(object)CreateParticipant(jsonObj);
            else if (objectKind == "Judge")
                return (T)(object)CreateJudge(jsonObj);
            else if (objectKind == "Competition")
                return (T)(object)CreateCompetition(jsonObj);

            return default(T);
        }

        private Participant CreateParticipant(JObject data)
        {
            string nameValue = (string)data["Name"];
            string surnameValue = (string)data["Surname"];
            double[] coefValues = data["Coefs"].ToObject<double[]>();
            int[,] marksMatrix = data["Marks"].ToObject<int[,]>();

            Participant result = new Participant(nameValue, surnameValue);
            result.SetCriterias(coefValues);

            for (int i = 0; i < 4; i++)
            {
                int[] roundMarks = new int[7];
                for (int j = 0; j < 7; j++)
                {
                    roundMarks[j] = marksMatrix[i, j];
                }
                result.Jump(roundMarks);
            }
            return result;
        }

        private Judge CreateJudge(JObject data)
        {
            string nameValue = (string)data["Name"];
            int[] marksArray = data["Marks"].ToObject<int[]>();
            return new Judge(nameValue, marksArray);
        }

        private Competition CreateCompetition(JObject data)
        {
            JObject[] judgesData = data["Judges"].ToObject<JObject[]>();
            Judge[] judgesArray = new Judge[judgesData.Length];
            for (int j = 0; j < judgesData.Length; j++)
            {
                judgesArray[j] = CreateJudge(judgesData[j]);
            }

            JObject[] participantsData = data["Participants"].ToObject<JObject[]>();
            Participant[] participantsArray = new Participant[participantsData.Length];
            for (int p = 0; p < participantsData.Length; p++)
            {
                participantsArray[p] = CreateParticipant(participantsData[p]);
            }

            Competition result = new Competition(judgesArray);
            result.Add(participantsArray);
            return result;
        }
        #endregion

        #region Purple_2
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            var jsonObj = JObject.FromObject(jumping);
            string typeValue = string.Empty;

            if (jumping is Purple_2.JuniorSkiJumping)
                typeValue = "Junior";
            else if (jumping is Purple_2.ProSkiJumping)
                typeValue = "Pro";

            jsonObj["JumpType"] = typeValue;
            File.WriteAllText(FilePath, jsonObj.ToString());
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var jsonObj = JObject.Parse(File.ReadAllText(FilePath));
            string jumpCategory = (string)jsonObj["JumpType"];

            Purple_2.SkiJumping result;
            if (jumpCategory == "Junior")
                result = new Purple_2.JuniorSkiJumping();
            else
                result = new Purple_2.ProSkiJumping();

            JObject[] participantsData = jsonObj["Participants"].ToObject<JObject[]>();
            Purple_2.Participant[] jumpers = new Purple_2.Participant[participantsData.Length];
            for (int i = 0; i < participantsData.Length; i++)
            {
                jumpers[i] = CreateSkiJumper(participantsData[i], result.Standard);
            }
            result.Add(jumpers);
            return (T)(object)result;
        }

        private Purple_2.Participant CreateSkiJumper(JObject data, int standardValue)
        {
            string nameValue = (string)data["Name"];
            string surnameValue = (string)data["Surname"];
            int distanceValue = data["Distance"].ToObject<int>();
            int[] marksArray = data["Marks"].ToObject<int[]>();

            Purple_2.Participant result = new Purple_2.Participant(nameValue, surnameValue);
            result.Jump(distanceValue, marksArray, standardValue);
            return result;
        }
        #endregion

        #region Purple_3
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            var jsonObj = JObject.FromObject(skating);
            string typeIdentifier = string.Empty;

            if (skating is Purple_3.FigureSkating)
                typeIdentifier = "FigureSkating";
            else
                typeIdentifier = "IceSkating";

            jsonObj["SkatingType"] = typeIdentifier;
            File.WriteAllText(FilePath, jsonObj.ToString());
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var jsonObj = JObject.Parse(File.ReadAllText(FilePath));
            string skatingCategory = (string)jsonObj["SkatingType"];
            double[] moodsArray = jsonObj["Moods"].ToObject<double[]>();

            Purple_3.Skating result;
            if (skatingCategory == "FigureSkating")
                result = new Purple_3.FigureSkating(moodsArray, false);
            else
                result = new Purple_3.IceSkating(moodsArray, false);

            JObject[] participantsData = jsonObj["Participants"].ToObject<JObject[]>();
            Purple_3.Participant[] skaters = new Purple_3.Participant[participantsData.Length];
            for (int i = 0; i < participantsData.Length; i++)
            {
                skaters[i] = CreateSkater(participantsData[i]);
            }
            result.Add(skaters);
            Purple_3.Participant.SetPlaces(skaters);
            return (T)(object)result;
        }

        private Purple_3.Participant CreateSkater(JObject data)
        {
            string nameValue = (string)data["Name"];
            string surnameValue = (string)data["Surname"];
            double[] marksArray = data["Marks"].ToObject<double[]>();

            Purple_3.Participant result = new Purple_3.Participant(nameValue, surnameValue);
            for (int m = 0; m < marksArray.Length; m++)
            {
                result.Evaluate(marksArray[m]);
            }
            return result;
        }
        #endregion

        #region Purple_4
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            SelectFile(fileName);
            var jsonObj = JObject.FromObject(participant);
            File.WriteAllText(FilePath, jsonObj.ToString());
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var jsonObj = JObject.Parse(File.ReadAllText(FilePath));
            string groupName = (string)jsonObj["Name"];

            Purple_4.Group result = new Purple_4.Group(groupName);
            JObject[] sportsmenData = jsonObj["Sportsmen"].ToObject<JObject[]>();
            Purple_4.Sportsman[] athletes = new Purple_4.Sportsman[sportsmenData.Length];

            for (int i = 0; i < sportsmenData.Length; i++)
            {
                athletes[i] = CreateSportsman(sportsmenData[i]);
            }
            result.Add(athletes);
            return result;
        }

        private Purple_4.Sportsman CreateSportsman(JObject data)
        {
            string nameValue = (string)data["Name"];
            string surnameValue = (string)data["Surname"];
            double timeValue = data["Time"].ToObject<double>();

            Purple_4.Sportsman result = new Purple_4.Sportsman(nameValue, surnameValue);
            result.Run(timeValue);
            return result;
        }
        #endregion

        #region Purple_5
        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            SelectFile(fileName);
            var jsonObj = JObject.FromObject(group);
            File.WriteAllText(FilePath, jsonObj.ToString());
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var jsonObj = JObject.Parse(File.ReadAllText(FilePath));
            Purple_5.Report result = new Purple_5.Report();
            JObject[] researchesData = jsonObj["Researches"].ToObject<JObject[]>();

            for (int r = 0; r < researchesData.Length; r++)
            {
                result.AddResearch(ProcessResearch(researchesData[r]));
            }
            return result;
        }

        private Purple_5.Research ProcessResearch(JObject data)
        {
            string researchName = (string)data["Name"];
            Purple_5.Research result = new Purple_5.Research(researchName);
            JObject[] responsesData = data["Responses"].ToObject<JObject[]>();

            for (int i = 0; i < responsesData.Length; i++)
            {
                result.Add(ExtractResponse(responsesData[i]));
            }
            return result;
        }

        private string[] ExtractResponse(JObject data)
        {
            string animalValue = (string)data["Animal"];
            string traitValue = (string)data["CharacterTrait"];
            string conceptValue = (string)data["Concept"];

            return new string[] {
                string.IsNullOrEmpty(animalValue) ? null : animalValue,
                string.IsNullOrEmpty(traitValue) ? null : traitValue,
                string.IsNullOrEmpty(conceptValue) ? null : conceptValue
            };
        }
        #endregion
    }
}