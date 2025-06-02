using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lab_7;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";

        public override T DeserializePurple1<T>(string nameFile)
        {
            SelectFile(nameFile);
            var fileData = ReadAllFileContent();
            var firstLine = GetFirstLine(fileData);
            
            return firstLine switch
            {
                "Participant" => (T)(object)ParseParticipantData(fileData),
                "Judge" => (T)(object)ParseJudgeData(fileData),
                "Competition" => (T)(object)ParseCompetitionData(fileData),
                _ => default
            };
        }

        public override T DeserializePurple2SkiJumping<T>(string nameFile)
        {
            SelectFile(nameFile);
            var contentLines = GetFileLines();
            var competitionType = contentLines.First().Trim();

            var competition = CreateSkiJumpingCompetition(competitionType);
            if (competition == null) return default;

            var participantsData = ExtractParticipantsData(contentLines);
            competition.Add(participantsData.ToArray());

            return (T)(object)competition;
        }

        public override T DeserializePurple3Skating<T>(string nameFile)
        {
            SelectFile(nameFile);
            var lines = GetFileLines();
            var skatingType = lines[0].Trim();
            
            var moods = ParseMoods(lines[1]);
            var skating = CreateSkatingInstance(skatingType, moods);
            if (skating == null) return default;

            var skaters = ProcessSkatingParticipants(lines);
            skating.Add(skaters.ToArray());
            Purple_3.Participant.SetPlaces(skating.Participants);

            return (T)(object)skating;
        }

        public override Purple_4.Group DeserializePurple4Group(string nameFile)
        {
            SelectFile(nameFile);
            var lines = GetFileLines();
            var group = InitializeGroup(lines[0]);

            var athletes = ProcessGroupParticipants(lines);
            group.Add(athletes.ToArray());

            return group;
        }

        public override Purple_5.Report DeserializePurple5Report(string nameFile)
        {
            SelectFile(nameFile);
            var lines = GetFileLines();
            var report = new Purple_5.Report();

            var researchCount = int.Parse(lines[0].Split(':')[1].Trim());
            var currentIndex = 1;

            for (int i = 0; i < researchCount; i++)
            {
                var research = ProcessResearchData(lines, ref currentIndex);
                report.AddResearch(research);
            }

            return report;
        }

        public override void SerializePurple1<T>(T obj, string nameFile)
        {
            string serializedData = obj switch
            {
                Purple_1.Participant p => SerializeParticipant(p),
                Purple_1.Judge j => SerializeJudge(j),
                Purple_1.Competition c => SerializeCompetition(c),
                _ => string.Empty
            };

            SaveToFile(nameFile, serializedData);
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string nameFile)
        {
            var sb = new StringBuilder();
            AppendCompetitionHeader(sb, jumping);
            AppendParticipantsData(sb, jumping.Participants);

            SaveToFile(nameFile, sb.ToString());
        }

        public override void SerializePurple3Skating<T>(T skating, string nameFile)
        {
            var sb = new StringBuilder();
            AppendSkatingHeader(sb, skating);
            AppendSkatersData(sb, skating.Participants);

            SaveToFile(nameFile, sb.ToString());
        }

        public override void SerializePurple4Group(Purple_4.Group group, string nameFile)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"name:{group.Name}");
            sb.AppendLine($"participants:{group.Sportsmen.Length}");

            foreach (var athlete in group.Sportsmen)
            {
                AppendAthleteData(sb, athlete);
            }

            SaveToFile(nameFile, sb.ToString());
        }

        public override void SerializePurple5Report(Purple_5.Report report, string nameFile)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"researches:{report.Researches.Length}");

            foreach (var research in report.Researches)
            {
                AppendResearchHeader(sb, research);
                foreach (var response in research.Responses)
                {
                    AppendResponseData(sb, response);
                }
            }

            SaveToFile(nameFile, sb.ToString());
        }

        

        private string ReadAllFileContent()
        {
            return File.ReadAllText(FilePath);
        }

        private string[] GetFileLines()
        {
            return File.ReadAllLines(FilePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToArray();
        }

        private string GetFirstLine(string content)
        {
            return content.Split('\n')[0].Trim();
        }

        private void SaveToFile(string nameFile, string content)
        {
            SelectFile(nameFile);
            File.WriteAllText(FilePath, content);
        }

        
        private Purple_1.Participant ParseParticipantData(string data)
        {
            var lines = data.Trim().Split('\n');
            var properties = ExtractProperties(lines);

            var participant = new Purple_1.Participant(properties["name"], properties["surname"]);
            participant.SetCriterias(properties["coefs"].Split(' ').Select(double.Parse).ToArray());

            var marksData = properties["marks"].Split(' ').Select(int.Parse).ToArray();
            for (int i = 0; i < 4; i++)
            {
                var jumpMarks = marksData.Skip(i * 7).Take(7).ToArray();
                participant.Jump(jumpMarks);
            }

            return participant;
        }

        private Purple_1.Judge ParseJudgeData(string data)
        {
            var lines = data.Trim().Split('\n');
            var properties = ExtractProperties(lines);

            return new Purple_1.Judge(
                properties["name"],
                properties["marks"].Split(' ').Select(int.Parse).ToArray());
        }

        private Purple_1.Competition ParseCompetitionData(string data)
        {
            var lines = data.Trim().Split('\n');
            var judges = ProcessJudges(lines);
            var participants = ProcessCompetitionParticipants(lines);

            var competition = new Purple_1.Competition(judges.ToArray());
            competition.Add(participants.ToArray());

            return competition;
        }
        

        
        private Purple_2.SkiJumping CreateSkiJumpingCompetition(string type)
        {
            return type switch
            {
                "JuniorSkiJumping" => new Purple_2.JuniorSkiJumping(),
                "ProSkiJumping" => new Purple_2.ProSkiJumping(),
                _ => null
            };
        }

        private IEnumerable<Purple_2.Participant> ExtractParticipantsData(string[] lines)
        {
            var participantCount = int.Parse(lines[3].Split(':')[1].Trim());
            var participants = new List<Purple_2.Participant>();

            for (int i = 0; i < participantCount; i++)
            {
                var startIdx = 4 + i * 4;
                var participantData = lines.Skip(startIdx).Take(4)
                    .ToDictionary(
                        line => line.Split(':')[0].Trim().ToLower(),
                        line => line.Split(':')[1].Trim());

                var participant = new Purple_2.Participant(
                    participantData["name"],
                    participantData["surname"]);

                participant.Jump(
                    int.Parse(participantData["distance"]),
                    participantData["marks"].Split(' ').Select(int.Parse).ToArray(),
                    int.Parse(lines[2].Split(':')[1].Trim()));

                participants.Add(participant);
            }

            return participants;
        }
        
        private double[] ParseMoods(string moodLine)
        {
            return moodLine.Split(':')[1].Trim()
                .Split(' ')
                .Select(double.Parse)
                .ToArray();
        }

        private Purple_3.Skating CreateSkatingInstance(string type, double[] moods)
        {
            return type switch
            {
                "IceSkating" => new Purple_3.IceSkating(moods, false),
                "FigureSkating" => new Purple_3.FigureSkating(moods, false),
                _ => null
            };
        }

        private IEnumerable<Purple_3.Participant> ProcessSkatingParticipants(string[] lines)
        {
            var participantCount = int.Parse(lines[2].Split(':')[1].Trim());
            var participants = new List<Purple_3.Participant>();

            for (int i = 0; i < participantCount; i++)
            {
                var startIdx = 3 + i * 4;
                var participantData = lines.Skip(startIdx).Take(4)
                    .ToDictionary(
                        line => line.Split(':')[0].Trim().ToLower(),
                        line => line.Split(':')[1].Trim());

                var participant = new Purple_3.Participant(
                    participantData["name"],
                    participantData["surname"]);

                var marks = participantData["marks"].Split(' ')
                    .Select(double.Parse)
                    .ToArray();

                foreach (var mark in marks)
                {
                    participant.Evaluate(mark);
                }

                participants.Add(participant);
            }

            return participants;
        }
        
        private Purple_4.Group InitializeGroup(string groupLine)
        {
            return new Purple_4.Group(groupLine.Split(':')[1].Trim());
        }

        private IEnumerable<Purple_4.Sportsman> ProcessGroupParticipants(string[] lines)
        {
            var participantCount = int.Parse(lines[1].Split(':')[1].Trim());
            var athletes = new List<Purple_4.Sportsman>();

            for (int i = 0; i < participantCount; i++)
            {
                var startIdx = 2 + i * 4;
                var athleteData = lines.Skip(startIdx).Take(4)
                    .ToDictionary(
                        line => line.Split(':')[0].Trim().ToLower(),
                        line => line.Split(':')[1].Trim());

                var athlete = CreateAthlete(athleteData);
                athlete.Run(double.Parse(athleteData["time"]));
                athletes.Add(athlete);
            }

            return athletes;
        }

        private Purple_4.Sportsman CreateAthlete(Dictionary<string, string> data)
        {
            return data["type"] switch
            {
                "skiman" => new Purple_4.SkiMan(data["name"], data["surname"]),
                "skiwoman" => new Purple_4.SkiWoman(data["name"], data["surname"]),
                _ => new Purple_4.Sportsman(data["name"], data["surname"])
            };
        }
        
        private Purple_5.Research ProcessResearchData(string[] lines, ref int currentIndex)
        {
            var researchName = lines[currentIndex++].Split(':')[1].Trim();
            var responseCount = int.Parse(lines[currentIndex++].Split(':')[1].Trim());
            var research = new Purple_5.Research(researchName);

            for (int j = 0; j < responseCount; j++)
            {
                var responseData = new Dictionary<string, string?>();
                for (int k = 0; k < 3; k++)
                {
                    var parts = lines[currentIndex].Split(':');
                    responseData[parts[0].Trim().ToLower()] = 
                        string.IsNullOrWhiteSpace(parts[1].Trim()) ? null : parts[1].Trim();
                    currentIndex++;
                }
                research.Add(new[] {
                    responseData["animal"],
                    responseData["charactertrait"],
                    responseData["concept"]
                });
            }

            return research;
        }
        
        private Dictionary<string, string> ExtractProperties(string[] lines)
        {
            return lines.Skip(1)
                .ToDictionary(
                    line => line.Split(':')[0].Trim().ToLower(),
                    line => line.Split(':')[1].Trim());
        }

        private string SerializeParticipant(Purple_1.Participant p)
        {
            var sb = new StringBuilder();
            sb.AppendLine(p.GetType().Name);

            AppendKeyValue(sb, "name", p.Name);
            AppendKeyValue(sb, "surname", p.Surname);
            AppendKeyValue(sb, "coefs", string.Join(" ", p.Coefs));
            
            var marks = new List<int>();
            for (int i = 0; i < p.Marks.GetLength(0); i++)
            {
                for (int j = 0; j < p.Marks.GetLength(1); j++)
                {
                    marks.Add(p.Marks[i, j]);
                }
            }
            AppendKeyValue(sb, "marks", string.Join(" ", marks));
            AppendKeyValue(sb, "total_score", p.TotalScore.ToString());

            return sb.ToString();
        }

        private string SerializeJudge(Purple_1.Judge j)
        {
            var sb = new StringBuilder();
            sb.AppendLine(j.GetType().Name);

            AppendKeyValue(sb, "name", j.Name);
            AppendKeyValue(sb, "marks", string.Join(" ", j.Marks));

            return sb.ToString();
        }

        private string SerializeCompetition(Purple_1.Competition c)
        {
            var sb = new StringBuilder();
            sb.AppendLine(c.GetType().Name);

            sb.AppendLine($"Judges:{c.Judges.Length}");
            foreach (var judge in c.Judges)
            {
                sb.Append(SerializeJudge(judge));
            }

            sb.AppendLine($"Participants:{c.Participants.Length}");
            foreach (var participant in c.Participants)
            {
                sb.Append(SerializeParticipant(participant));
            }

            return sb.ToString();
        }

        private void AppendCompetitionHeader<T>(StringBuilder sb, T jumping) where T : Purple_2.SkiJumping
        {
            sb.AppendLine(jumping switch
            {
                Purple_2.JuniorSkiJumping => "JuniorSkiJumping",
                Purple_2.ProSkiJumping => "ProSkiJumping",
                _ => "UnknownSkiJumping"
            });

            AppendKeyValue(sb, "name", jumping.Name);
            AppendKeyValue(sb, "standard", jumping.Standard.ToString());
            AppendKeyValue(sb, "participants", jumping.Participants.Length.ToString());
        }

        private void AppendParticipantsData(StringBuilder sb, Purple_2.Participant[] participants)
        {
            foreach (var p in participants)
            {
                AppendKeyValue(sb, "name", p.Name);
                AppendKeyValue(sb, "surname", p.Surname);
                AppendKeyValue(sb, "distance", p.Distance.ToString());
                AppendKeyValue(sb, "marks", string.Join(" ", p.Marks));
            }
        }

        private void AppendSkatingHeader<T>(StringBuilder sb, T skating) where T : Purple_3.Skating
        {
            sb.AppendLine(skating switch
            {
                Purple_3.FigureSkating => "FigureSkating",
                Purple_3.IceSkating => "IceSkating",
                _ => "UnknownSkating"
            });

            sb.AppendLine("moods:" + string.Join(' ', skating.Moods));
            sb.AppendLine("participants:" + skating.Participants.Length);
        }

        private void AppendSkatersData(StringBuilder sb, Purple_3.Participant[] participants)
        {
            foreach (var p in participants)
            {
                AppendKeyValue(sb, "name", p.Name);
                AppendKeyValue(sb, "surname", p.Surname);
                AppendKeyValue(sb, "marks", string.Join(' ', p.Marks));
                AppendKeyValue(sb, "places", string.Join(' ', p.Places));
            }
        }

        private void AppendAthleteData(StringBuilder sb, Purple_4.Sportsman athlete)
        {
            string type = athlete switch
            {
                Purple_4.SkiMan => "SkiMan",
                Purple_4.SkiWoman => "SkiWoman",
                _ => "unknown"
            };

            AppendKeyValue(sb, "type", type);
            AppendKeyValue(sb, "name", athlete.Name);
            AppendKeyValue(sb, "surname", athlete.Surname);
            AppendKeyValue(sb, "time", athlete.Time.ToString());
        }

        private void AppendResearchHeader(StringBuilder sb, Purple_5.Research research)
        {
            AppendKeyValue(sb, "name", research.Name);
            AppendKeyValue(sb, "responses", research.Responses.Length.ToString());
        }

        private void AppendResponseData(StringBuilder sb, Purple_5.Response response)
        {
            AppendKeyValue(sb, "animal", response.Animal);
            AppendKeyValue(sb, "characterTrait", response.CharacterTrait);
            AppendKeyValue(sb, "concept", response.Concept);
        }

        private void AppendKeyValue(StringBuilder sb, string key, string value)
        {
            sb.AppendLine($"{key}:{value}");
        }

        private IEnumerable<Purple_1.Judge> ProcessJudges(string[] lines)
        {
            var judgeCount = int.Parse(lines[1].Split(':')[1].Trim());
            var judges = new List<Purple_1.Judge>();
            var currentIndex = 2;

            for (int i = 0; i < judgeCount; i++)
            {
                var judgeLines = lines.Skip(currentIndex).Take(3).ToArray();
                judges.Add(ParseJudgeData(string.Join("\n", judgeLines)));
                currentIndex += 3;
            }

            return judges;
        }

        private IEnumerable<Purple_1.Participant> ProcessCompetitionParticipants(string[] lines)
        {
            var participantCountLineIndex = Array.FindIndex(lines, l => l.StartsWith("Participants:"));
            var participantCount = int.Parse(lines[participantCountLineIndex].Split(':')[1].Trim());
            var participants = new List<Purple_1.Participant>();
            var currentIndex = participantCountLineIndex + 1;

            for (int i = 0; i < participantCount; i++)
            {
                var participantLines = lines.Skip(currentIndex).Take(6).ToArray();
                participants.Add(ParseParticipantData(string.Join("\n", participantLines)));
                currentIndex += 6;
            }

            return participants;
        }
        
    }
}