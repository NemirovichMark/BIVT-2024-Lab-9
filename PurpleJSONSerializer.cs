using Lab_7;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension => "json";
        public override void SerializePurple1<T>(T obj, string nameFile)
        {
            var a = JObject.FromObject(obj);
            a.Add("Type", obj.GetType().Name);
            SelectFile(nameFile);
            File.WriteAllText(FilePath, a.ToString());
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string nameFile)
        {
            var a = JObject.FromObject(jumping);
            a.Add("Type", jumping.GetType().Name);
            SelectFile(nameFile);
            File.WriteAllText(FilePath, a.ToString());
        }

        public override void SerializePurple3Skating<T>(T skating, string nameFile)
        {
            var a = JObject.FromObject(skating);
            a.Add("Type", skating.GetType().Name);
            SelectFile(nameFile);
            File.WriteAllText(FilePath, a.ToString());
        }

        public override void SerializePurple4Group(Purple_4.Group participant, string nameFile)
        {
            var a = JObject.FromObject(participant);
            a.Add("Type", participant.GetType().Name);
            SelectFile(nameFile);
            File.WriteAllText(FilePath, a.ToString());
        }
        public override void SerializePurple5Report(Purple_5.Report group, string nameFile)
        {
            var a = JObject.FromObject(group);
            a.Add("Type", group.GetType().Name);
            SelectFile(nameFile);
            File.WriteAllText(FilePath, a.ToString());
        }
        public override T DeserializePurple1<T>(string nameFile)
        {
            SelectFile(nameFile);
            string fileContent = File.ReadAllText(FilePath);
            JObject parsedData = JObject.Parse(fileContent);
            
            string objectType = parsedData["Type"].ToString();
            
            if (objectType == "Participant")
            {
                var participant = GetParticipantFromData(parsedData);
                return (T)(object)participant;
            }
            
            if (objectType == "Judge")
            {
                var judge = GetJudgeFromData(parsedData);
                return (T)(object)judge;
            }
            
            if (objectType == "Competition")
            {
                var competition = GetCompetitionFromData(parsedData);
                return (T)(object)competition;
            }

            return default;
        }

        private Purple_1.Participant GetParticipantFromData(JObject participantData)
        {
            string participantName = participantData["Name"].ToString();
            string participantSurname = participantData["Surname"].ToString();
            var participant = new Purple_1.Participant(participantName, participantSurname);
            
            double[] coefficients = participantData["Coefs"].ToObject<double[]>();
            participant.SetCriterias(coefficients);
            
            int[][] marksData = participantData["Marks"].ToObject<int[][]>();
            foreach (int[] markSet in marksData)
            {
                participant.Jump(markSet);
            }
            
            return participant;
        }

        private Purple_1.Judge GetJudgeFromData(JObject judgeData)
        {
            string judgeName = judgeData["Name"].ToString();
            int[] judgeMarks = judgeData["Marks"].ToObject<int[]>();
            
            return new Purple_1.Judge(judgeName, judgeMarks);
        }

        private Purple_1.Competition GetCompetitionFromData(JObject competitionData)
        {
            var judgesData = competitionData["Judges"].ToObject<JObject[]>();
            var judges = judgesData.Select(GetJudgeFromData).ToArray();
            
            var competition = new Purple_1.Competition(judges);
            
            var participantsData = competitionData["Participants"].ToObject<JObject[]>();
            var participants = participantsData.Select(GetParticipantFromData).ToArray();
            
            competition.Add(participants);
            return competition;
        }
        public override T DeserializePurple2SkiJumping<T>(string nameFile)
        {
            if (string.IsNullOrWhiteSpace(nameFile))
                throw new ArgumentException("Invalid file name", nameof(nameFile));

            SelectFile(nameFile);
            string fileContent = File.ReadAllText(FilePath);
            var parsedJson = JObject.Parse(fileContent);

            Purple_2.SkiJumping skiCompetition;
            string competitionType = parsedJson["Type"]?.ToString();

            if (competitionType == "JuniorSkiJumping")
            {
                skiCompetition = new Purple_2.JuniorSkiJumping();
            }
            else if (competitionType == "ProSkiJumping")
            {
                skiCompetition = new Purple_2.ProSkiJumping();
            }
            else
            {
                return default(T);
            }

            var competitorsData = parsedJson["Participants"]?.ToObject<JObject[]>();
            if (competitorsData != null)
            {
                var competitors = competitorsData.Select(competitor =>
                {
                    var athlete = new Purple_2.Participant(
                        competitor["Name"]?.ToString() ?? string.Empty,
                        competitor["Surname"]?.ToString() ?? string.Empty
                    );

                    int jumpLength = competitor["Distance"]?.ToObject<int>() ?? 0;
                    int[] scores = competitor["Marks"]?.ToObject<int[]>() ?? new int[0];
                    int targetDistance = parsedJson["Standard"]?.ToObject<int>() ?? 0;

                    athlete.Jump(jumpLength, scores, targetDistance);
                    return athlete;
                }).ToArray();

                skiCompetition.Add(competitors);
            }

            return (T)(object)skiCompetition;
        }
        public override T DeserializePurple3Skating<T>(string nameFile)
        {
            if (string.IsNullOrWhiteSpace(nameFile))
                throw new ArgumentException("File path cannot be empty", nameof(nameFile));

            SelectFile(nameFile);
            string jsonContent = File.ReadAllText(FilePath);
            var jsonData = JObject.Parse(jsonContent);

            Purple_3.Skating skatingCompetition = null;
            string competitionType = jsonData["Type"]?.ToString();

            if (competitionType == "IceSkating")
            {
                double[] moodValues = jsonData["Moods"]?.ToObject<double[]>() ?? Array.Empty<double>();
                skatingCompetition = new Purple_3.IceSkating(moodValues, false);
            }
            else if (competitionType == "FigureSkating")
            {
                double[] moodScores = jsonData["Moods"]?.ToObject<double[]>() ?? Array.Empty<double>();
                skatingCompetition = new Purple_3.FigureSkating(moodScores, false);
            }

            if (skatingCompetition == null)
                return default(T);

            var participantsData = jsonData["Participants"]?.ToObject<JObject[]>();
            if (participantsData != null)
            {
                var competitors = participantsData.Select(participantJson =>
                {
                    var competitor = new Purple_3.Participant(
                        participantJson["Name"]?.ToString() ?? string.Empty,
                        participantJson["Surname"]?.ToString() ?? string.Empty
                    );

                    var evaluations = participantJson["Marks"]?.ToObject<double[]>() ?? Array.Empty<double>();
                    foreach (var evaluation in evaluations)
                    {
                        competitor.Evaluate(evaluation);
                    }

                    return competitor;
                }).ToArray();

                skatingCompetition.Add(competitors);
            }

            Purple_3.Participant.SetPlaces(skatingCompetition.Participants);
            return (T)(object)skatingCompetition;
        }
        public override Purple_4.Group DeserializePurple4Group(string nameFile)
        {
            if (string.IsNullOrWhiteSpace(nameFile))
                throw new ArgumentException("File name cannot be empty", nameof(nameFile));

            SelectFile(nameFile);
            string fileContents = File.ReadAllText(FilePath);
            var parsedData = JObject.Parse(fileContents);

            var competitionGroup = new Purple_4.Group(parsedData["Name"]?.ToString() ?? "Unnamed Group");

            var athletesData = parsedData["Sportsmen"]?.ToObject<JObject[]>();
            if (athletesData != null)
            {
                var competitors = athletesData.Select(athleteJson =>
                {
                    var competitor = new Purple_4.Sportsman(
                        athleteJson["Name"]?.ToString() ?? string.Empty,
                        athleteJson["Surname"]?.ToString() ?? string.Empty
                    );

                    double raceTime = athleteJson["Time"]?.ToObject<double>() ?? 0.0;
                    if (raceTime > 0)
                    {
                        competitor.Run(raceTime);
                    }

                    return competitor;
                }).ToArray();

                competitionGroup.Add(competitors);
            }

            return competitionGroup;
        }
        public override Purple_5.Report DeserializePurple5Report(string nameFile)
        {
            SelectFile(nameFile);
            var line = File.ReadAllText(FilePath);
            var a = JObject.Parse(line);
            Console.WriteLine(a);
            var report = new Purple_5.Report();
            report.AddResearch(a["Researches"].ToObject<JObject[]>().Select(js =>
            {
                var res = new Purple_5.Research(js["Name"].ToString());
                var resps = js["Responses"].ToObject<JObject[]>();
                foreach (var resp in resps)
                {
                    res.Add([resp["Animal"].ToObject<String>(), resp["CharacterTrait"].ToObject<String>(), resp["Concept"].ToObject<String>()]);
                }
                return res;
            }).ToArray());
            return report;
        }
    }
}