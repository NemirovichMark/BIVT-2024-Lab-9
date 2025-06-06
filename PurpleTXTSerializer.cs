using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Lab_7.Purple_1;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";

        #region Purple_1
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            StringBuilder contentBuilder = new StringBuilder();

            if (obj is Participant participantObj)
            {
                contentBuilder.AppendLine($"NameP0:{participantObj.Name}");
                contentBuilder.AppendLine($"SurnameP0:{participantObj.Surname}");
                contentBuilder.AppendLine($"CoefsP0:{string.Join(" ", participantObj.Coefs)}");
                contentBuilder.AppendLine($"MarksP0:{string.Join(" ", participantObj.Marks.Cast<int>())}");
                contentBuilder.AppendLine($"TotalScoreP0:{participantObj.TotalScore}");
            }
            else if (obj is Judge judgeObj)
            {
                contentBuilder.AppendLine($"NameJ0:{judgeObj.Name}");
                contentBuilder.AppendLine($"MarksJ0:{string.Join(" ", judgeObj.Marks)}");
            }
            else if (obj is Competition competitionObj)
            {
                contentBuilder.AppendLine("JudgeStart");
                contentBuilder.AppendLine($"CountJudges:{competitionObj.Judges.Length}");

                for (int j = 0; j < competitionObj.Judges.Length; j++)
                {
                    var currentJudge = competitionObj.Judges[j];
                    contentBuilder.AppendLine($"NameJ{j}:{currentJudge.Name}");
                    contentBuilder.AppendLine($"MarksJ{j}:{string.Join(" ", currentJudge.Marks)}");
                    contentBuilder.AppendLine("___");
                }
                contentBuilder.AppendLine("JudgeEnd");

                contentBuilder.AppendLine("participantStart");
                contentBuilder.AppendLine($"CountParticipants:{competitionObj.Participants.Length}");

                for (int p = 0; p < competitionObj.Participants.Length; p++)
                {
                    var currentParticipant = competitionObj.Participants[p];
                    contentBuilder.AppendLine($"NameP{p}:{currentParticipant.Name}");
                    contentBuilder.AppendLine($"SurnameP{p}:{currentParticipant.Surname}");
                    contentBuilder.AppendLine($"CoefsP{p}:{string.Join(" ", currentParticipant.Coefs)}");
                    contentBuilder.AppendLine($"MarksP{p}:{string.Join(" ", currentParticipant.Marks.Cast<int>())}");
                    contentBuilder.AppendLine($"TotalScoreP{p}:{currentParticipant.TotalScore}");
                    contentBuilder.AppendLine("___");
                }
                contentBuilder.AppendLine("participantEnd");
            }
            File.WriteAllText(FilePath, contentBuilder.ToString());
        }

        private Participant DeserializeParticipant(int idx = 0)
        {
            string[] fileLines = File.ReadAllLines(FilePath);
            Dictionary<string, string> dataMap = new Dictionary<string, string>();

            for (int l = 0; l < fileLines.Length; l++)
            {
                string currentLine = fileLines[l];
                if (currentLine.Contains(":"))
                {
                    string[] parts = currentLine.Split(':');
                    dataMap[parts[0].Trim()] = parts[1];
                }
            }

            string firstName = dataMap[$"NameP{idx}"];
            string lastName = dataMap[$"SurnameP{idx}"];
            double[] coefficients = dataMap[$"CoefsP{idx}"].Split(' ')
                .Select(num => double.Parse(num))
                .ToArray();

            int[] scoresFlat = dataMap[$"MarksP{idx}"].Split(' ')
                .Select(num => int.Parse(num))
                .ToArray();

            int[,] scoreMatrix = new int[4, 7];
            int counter = 0;
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    scoreMatrix[row, col] = scoresFlat[counter++];
                }
            }

            Participant result = new Participant(firstName, lastName);
            result.SetCriterias(coefficients);
            for (int r = 0; r < 4; r++)
            {
                int[] roundScores = new int[7];
                for (int c = 0; c < 7; c++)
                {
                    roundScores[c] = scoreMatrix[r, c];
                }
                result.Jump(roundScores);
            }
            return result;
        }

        private Judge DeserializeJudge(int idx = 0)
        {
            string[] fileLines = File.ReadAllLines(FilePath);
            Dictionary<string, string> dataMap = new Dictionary<string, string>();

            for (int l = 0; l < fileLines.Length; l++)
            {
                string currentLine = fileLines[l];
                if (currentLine.Contains(":"))
                {
                    string[] parts = currentLine.Split(':');
                    dataMap[parts[0].Trim()] = parts[1];
                }
            }

            string judgeName = dataMap[$"NameJ{idx}"];
            int[] marks = dataMap[$"MarksJ{idx}"].Split(' ')
                .Select(num => int.Parse(num))
                .ToArray();

            return new Judge(judgeName, marks);
        }

        private Competition DeserializeCompetition()
        {
            string[] fileLines = File.ReadAllLines(FilePath);
            Dictionary<string, string> dataMap = new Dictionary<string, string>();

            for (int l = 0; l < fileLines.Length; l++)
            {
                string currentLine = fileLines[l];
                if (currentLine.Contains(":"))
                {
                    string[] parts = currentLine.Split(':');
                    dataMap[parts[0].Trim()] = parts[1];
                }
            }

            int participantsCount = int.Parse(dataMap["CountParticipants"]);
            int judgesCount = int.Parse(dataMap["CountJudges"]);

            Participant[] participantsList = new Participant[participantsCount];
            Judge[] judgesList = new Judge[judgesCount];

            for (int p = 0; p < participantsCount; p++)
            {
                participantsList[p] = DeserializeParticipant(p);
            }

            for (int j = 0; j < judgesCount; j++)
            {
                judgesList[j] = DeserializeJudge(j);
            }

            Competition result = new Competition(judgesList);
            result.Add(participantsList);
            return result;
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            if (typeof(T) == typeof(Participant))
            {
                return (T)(object)DeserializeParticipant();
            }
            else if (typeof(T) == typeof(Judge))
            {
                return (T)(object)DeserializeJudge();
            }
            else if (typeof(T) == typeof(Competition))
            {
                return (T)(object)DeserializeCompetition();
            }
            return default(T);
        }
        #endregion

        #region Purple_2
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            StringBuilder contentBuilder = new StringBuilder();
            contentBuilder.AppendLine($"Type:{jumping.GetType().Name}");
            contentBuilder.AppendLine($"nameComp:{jumping.Name}");
            contentBuilder.AppendLine($"Standard:{jumping.Standard}");
            contentBuilder.AppendLine($"Count:{jumping.Participants.Length}");
            contentBuilder.AppendLine("participantStart");

            Purple_2.Participant[] jumpers = jumping.Participants;
            for (int i = 0; i < jumpers.Length; i++)
            {
                var current = jumpers[i];
                contentBuilder.AppendLine($"Name{i}:{current.Name}");
                contentBuilder.AppendLine($"Surname{i}:{current.Surname}");
                contentBuilder.AppendLine($"Distance{i}:{current.Distance}");
                contentBuilder.AppendLine($"Marks{i}:{string.Join(' ', current.Marks)}");
                contentBuilder.AppendLine($"Result{i}:{current.Result}");
            }
            contentBuilder.AppendLine("participantEnd");
            File.WriteAllText(FilePath, contentBuilder.ToString());
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            string[] fileLines = File.ReadAllLines(FilePath);
            Dictionary<string, string> dataMap = new Dictionary<string, string>();

            for (int l = 0; l < fileLines.Length; l++)
            {
                string currentLine = fileLines[l];
                if (currentLine.Contains(":"))
                {
                    string[] parts = currentLine.Split(':');
                    dataMap[parts[0].Trim()] = parts[1];
                }
            }

            string jumpType = dataMap["Type"];
            string compName = dataMap["nameComp"];
            int participantsCount = int.Parse(dataMap["Count"]);
            int standardValue = int.Parse(dataMap["Standard"]);

            Purple_2.SkiJumping result;
            if (jumpType.Contains("JuniorSkiJumping"))
                result = new Purple_2.JuniorSkiJumping();
            else
                result = new Purple_2.ProSkiJumping();

            Purple_2.Participant[] jumpers = new Purple_2.Participant[participantsCount];
            for (int i = 0; i < participantsCount; i++)
            {
                jumpers[i] = DeserializeSkiJumper(dataMap, standardValue, i);
            }
            result.Add(jumpers);
            return (T)(object)result;
        }

        private Purple_2.Participant DeserializeSkiJumper(Dictionary<string, string> data, int target, int idx)
        {
            string firstName = data[$"Name{idx}"];
            string lastName = data[$"Surname{idx}"];
            int distance = int.Parse(data[$"Distance{idx}"]);
            int[] marks = data[$"Marks{idx}"].Split(' ').Select(x => int.Parse(x)).ToArray();
            int finalScore = int.Parse(data[$"Result{idx}"]);

            var jumper = new Purple_2.Participant(firstName, lastName);
            jumper.Jump(distance, marks, target);
            return jumper;
        }
        #endregion

        #region Purple_3
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            StringBuilder contentBuilder = new StringBuilder();
            contentBuilder.AppendLine(skating is Purple_3.FigureSkating ?
                "Type:FigureSkating" : "Type:IceSkating");
            contentBuilder.AppendLine($"Moods:{string.Join(' ', skating.Moods)}");
            contentBuilder.AppendLine($"Count:{skating.Participants.Length}");

            for (int i = 0; i < skating.Participants.Length; i++)
            {
                SerializeSkater(contentBuilder, skating.Participants[i], i);
            }
            File.WriteAllText(FilePath, contentBuilder.ToString());
        }

        private void SerializeSkater(StringBuilder builder, Purple_3.Participant skater, int idx)
        {
            builder.AppendLine($"Name{idx}:{skater.Name}");
            builder.AppendLine($"Surname{idx}:{skater.Surname}");
            builder.AppendLine($"Marks{idx}:{string.Join(' ', skater.Marks)}");
            builder.AppendLine($"Places{idx}:{string.Join(' ', skater.Places)}");
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            string[] fileLines = File.ReadAllLines(FilePath);
            Dictionary<string, string> dataMap = new Dictionary<string, string>();

            for (int l = 0; l < fileLines.Length; l++)
            {
                string currentLine = fileLines[l];
                if (currentLine.Contains(":"))
                {
                    string[] parts = currentLine.Split(':');
                    dataMap[parts[0].Trim()] = parts[1].Trim();
                }
            }

            string skatingType = dataMap["Type"];
            int skatersCount = int.Parse(dataMap["Count"]);
            double[] moodValues = dataMap["Moods"].Split(' ')
                .Select(val => double.Parse(val))
                .ToArray();

            Purple_3.Skating result;
            if (skatingType == "FigureSkating")
            {
                result = new Purple_3.FigureSkating(moodValues, false);
            }
            else
            {
                result = new Purple_3.IceSkating(moodValues, false);
            }

            Purple_3.Participant[] skaters = new Purple_3.Participant[skatersCount];
            for (int i = 0; i < skatersCount; i++)
            {
                skaters[i] = DeserializeSkater(dataMap, i);
            }
            result.Add(skaters);
            Purple_3.Participant.SetPlaces(skaters);
            return (T)(object)result;
        }

        private Purple_3.Participant DeserializeSkater(Dictionary<string, string> data, int idx)
        {
            string firstName = data[$"Name{idx}"];
            string lastName = data[$"Surname{idx}"];
            int[] placements = data[$"Places{idx}"].Split(' ').Select(p => int.Parse(p)).ToArray();
            double[] scores = data[$"Marks{idx}"].Split(' ').Select(s => double.Parse(s)).ToArray();

            var skater = new Purple_3.Participant(firstName, lastName);
            for (int s = 0; s < scores.Length; s++)
            {
                skater.Evaluate(scores[s]);
            }
            return skater;
        }
        #endregion

        #region Purple_4
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            SelectFile(fileName);
            StringBuilder contentBuilder = new StringBuilder();
            contentBuilder.AppendLine($"nameGroup:{participant.Name}");
            contentBuilder.AppendLine($"Count:{participant.Sportsmen.Length}");

            for (int i = 0; i < participant.Sportsmen.Length; i++)
            {
                var current = participant.Sportsmen[i];
                contentBuilder.AppendLine($"Name{i}:{current.Name}");
                contentBuilder.AppendLine($"Surname{i}:{current.Surname}");
                contentBuilder.AppendLine($"Time{i}:{current.Time}");
            }
            File.WriteAllText(FilePath, contentBuilder.ToString());
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            string[] fileLines = File.ReadAllLines(FilePath);
            Dictionary<string, string> dataMap = new Dictionary<string, string>();

            for (int l = 0; l < fileLines.Length; l++)
            {
                string currentLine = fileLines[l];
                if (currentLine.Contains(":"))
                {
                    string[] parts = currentLine.Split(':');
                    dataMap[parts[0].Trim()] = parts[1].Trim();
                }
            }

            string groupName = dataMap["nameGroup"];
            int membersCount = int.Parse(dataMap["Count"]);
            Purple_4.Sportsman[] athletes = new Purple_4.Sportsman[membersCount];

            for (int i = 0; i < membersCount; i++)
            {
                athletes[i] = DeserializeAthlete(dataMap, i);
            }

            Purple_4.Group result = new Purple_4.Group(groupName);
            result.Add(athletes);
            return result;
        }

        private Purple_4.Sportsman DeserializeAthlete(Dictionary<string, string> data, int idx)
        {
            string firstName = data[$"Name{idx}"];
            string lastName = data[$"Surname{idx}"];
            double timeResult = double.Parse(data[$"Time{idx}"]);

            Purple_4.Sportsman athlete = new Purple_4.Sportsman(firstName, lastName);
            athlete.Run(timeResult);
            return athlete;
        }
        #endregion

        #region Purple_5
        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            SelectFile(fileName);   
            StringBuilder contentBuilder = new StringBuilder();
            contentBuilder.AppendLine($"researchesCount:{group.Researches.Length}");

            for (int i = 0; i < group.Researches.Length; i++)
            {
                SerializeResearch(contentBuilder, i, group.Researches[i]);
            }
            File.WriteAllText(FilePath, contentBuilder.ToString());
        }

        private void SerializeResponse(StringBuilder builder, int resIdx, int respIdx, Purple_5.Response response)
        {
            builder.AppendLine($"Animal_{resIdx}_{respIdx}:{response.Animal}");
            builder.AppendLine($"characterTrait_{resIdx}_{respIdx}:{response.CharacterTrait}");
            builder.AppendLine($"Concept_{resIdx}_{respIdx}:{response.Concept}");
        }

        private void SerializeResearch(StringBuilder builder, int idx, Purple_5.Research research)
        {
            builder.AppendLine($"nameResearch{idx}:{research.Name}");
            builder.AppendLine($"responsesCount{idx}:{research.Responses.Length}");

            for (int r = 0; r < research.Responses.Length; r++)
            {
                SerializeResponse(builder, idx, r, research.Responses[r]);
            }
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            string[] fileLines = File.ReadAllLines(FilePath);
            Dictionary<string, string> dataMap = new Dictionary<string, string>();

            for (int l = 0; l < fileLines.Length; l++)
            {
                string currentLine = fileLines[l];
                if (currentLine.Contains(":"))
                {
                    string[] parts = currentLine.Split(':');
                    dataMap[parts[0].Trim()] = parts[1].Trim();
                }
            }

            int researchCount = int.Parse(dataMap["researchesCount"]);
            var report = new Purple_5.Report();

            for (int i = 0; i < researchCount; i++)
            {
                report.AddResearch(DeserializeResearch(dataMap, i));
            }
            return report;
        }

        private Purple_5.Research DeserializeResearch(Dictionary<string, string> data, int idx)
        {
            string researchName = data[$"nameResearch{idx}"];
            int responseCount = int.Parse(data[$"responsesCount{idx}"]);
            var research = new Purple_5.Research(researchName);

            for (int r = 0; r < responseCount; r++)
            {
                string animal = GetValueOrNull(data, $"Animal_{idx}_{r}");
                string trait = GetValueOrNull(data, $"characterTrait_{idx}_{r}");
                string concept = GetValueOrNull(data, $"Concept_{idx}_{r}");

                research.Add(new string[] { animal, trait, concept });
            }
            return research;
        }

        private string GetValueOrNull(Dictionary<string, string> data, string key)
        {
            if (data.TryGetValue(key, out string value))
            {
                return string.IsNullOrEmpty(value) ? null : value;
            }
            return null;
        }
        #endregion
    }
}