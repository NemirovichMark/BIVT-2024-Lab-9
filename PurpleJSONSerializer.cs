using System.Globalization;
using Newtonsoft.Json;

using Lab_7;
namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension => "json";

        private class ParticipantDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int[][] Marks { get; set; }

            public double[][] Marks_d { get; set; }

            public int Distance { get; set; }
        }
        
        private class JudgeDto
        {
            public string Name { get; set; }
            public int[] Marks { get; set; }
        }

        private class CompetitionDto
        {
            public JudgeDto[] Judges { get; set; }
            public ParticipantDto[] Participants { get; set; }
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            if (obj is Purple_1.Participant participant) SerializeParticipant(participant, fileName);
            else if (obj is Purple_1.Judge judge) SerializeJudge(judge, fileName);
            else if (obj is Purple_1.Competition competition) SerializeCompetition(competition, fileName);
        }

        private void SerializeParticipant(Purple_1.Participant participant, string fileName)
        {
            SelectFile(fileName);
            var dto = new ParticipantDto
            {
                Name = participant.Name,
                Surname = participant.Surname,
                Coefs = participant.Coefs,
                Marks = ConvertMarks(participant.Marks)
            };
            var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        private void SerializeJudge(Purple_1.Judge judge, string fileName)
        {
            SelectFile(fileName);
            var dto = new JudgeDto
            {
                Name = judge.Name,
                Marks = judge.Marks
            };
            var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        private void SerializeCompetition(Purple_1.Competition comp, string fileName)
        {
            SelectFile(fileName);
            var dto = new CompetitionDto
            {
                Judges = comp.Judges.Select(j => new JudgeDto
                {
                    Name = j.Name,
                    Marks = j.Marks
                }).ToArray(),
                Participants = comp.Participants.Select(p => new ParticipantDto
                {
                    Name = p.Name,
                    Surname = p.Surname,
                    Coefs = p.Coefs,
                    Marks = ConvertMarks(p.Marks)
                }).ToArray()
            };
            var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }
        private class SkiJumpingDto
        {
            public string Name { get; set; }
            public int Standard { get; set; }
            public ParticipantDto[] Participants { get; set; }
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);

            var obj = (Purple_2.SkiJumping)(object)jumping;
            var dto = new SkiJumpingDto
            {
                Name = obj.Name,
                Standard = obj.Standard,
                Participants = obj.Participants.Select(p => new ParticipantDto
                {
                    Name = p.Name,
                    Surname = p.Surname,
                    Coefs = null, 
                    Marks = new[] { p.Marks },
                    Distance = p.Distance
                }).ToArray()
            };

            var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }
        private class SkatingDto
        {
            public string TypeName { get; set; }
            public double[] Moods { get; set; }
            public ParticipantDto[] Participants { get; set; }
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);

            var obj = skating as Purple_3.Skating;
            var dto = new SkatingDto
            {
                TypeName = obj.GetType().Name,
                Moods = obj.Moods,
                Participants = obj.Participants.Select(p => new ParticipantDto
                {
                    Name = p.Name,
                    Surname = p.Surname,
                    Coefs = null, 
                    Marks_d = p.Marks.Select(m => new[] { (double)m }).ToArray() 
                }).ToArray()
            };

            var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        private class GroupDto
        {
            public string Name { get; set; }
            public SportsmanDto[] Sportsmen { get; set; }
        }

        private class SportsmanDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);

            var dto = new GroupDto
            {
                Name = group.Name,
                Sportsmen = group.Sportsmen.Select(s => new SportsmanDto
                {
                    Name = s.Name,
                    Surname = s.Surname,
                    Time = s.Time
                }).ToArray()
            };

            var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        private class ReportDto
        {
            public ResearchDto[] Researches { get; set; }
        }

        private class ResearchDto
        {
            public string Name { get; set; }
            public ResponseDto[] Responses { get; set; }
        }

        private class ResponseDto
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);

            var dto = new ReportDto
            {
                Researches = report.Researches.Select(r => new ResearchDto
                {
                    Name = r.Name,
                    Responses = r.Responses.Select(resp => new ResponseDto
                    {
                        Animal = resp.Animal,
                        CharacterTrait = resp.CharacterTrait,
                        Concept = resp.Concept
                    }).ToArray()
                }).ToArray()
            };

            var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }





        public override T DeserializePurple1<T>(string fileName)
        {
            if (typeof(T) == typeof(Purple_1.Participant)) return DeserializeParticipant(fileName) as T;
            else if (typeof(T) == typeof(Purple_1.Judge)) return DeserializeJudge(fileName) as T;
            else return DeserializeCompetition(fileName) as T;
        }

        private Purple_1.Participant DeserializeParticipant(string fileName)
        {
            SelectFile(fileName);
            var json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<ParticipantDto>(json);

            var participant = new Purple_1.Participant(dto.Name, dto.Surname);
            participant.SetCriterias(dto.Coefs);

            foreach (var marks in dto.Marks)
                participant.Jump(marks);

            return participant;
        }

        private Purple_1.Judge DeserializeJudge(string fileName)
        {
            SelectFile(fileName);
            var json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<JudgeDto>(json);

            return new Purple_1.Judge(dto.Name, dto.Marks);
        }

        private Purple_1.Competition DeserializeCompetition(string fileName)
        {
            SelectFile(fileName);
            var json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<CompetitionDto>(json);

            var judges = dto.Judges.Select(j => new Purple_1.Judge(j.Name, j.Marks)).ToArray();
            var comp = new Purple_1.Competition(judges);

            foreach (var part in dto.Participants)
            {
                var participant = new Purple_1.Participant(part.Name, part.Surname);
                participant.SetCriterias(part.Coefs);

                foreach (var marks in part.Marks)
                    participant.Jump(marks);

                comp.Add(participant);
            }

            return comp;
        }

        private int[][] ConvertMarks(int[,] marks)
        {
            int rows = marks.GetLength(0);
            int cols = marks.GetLength(1);
            var result = new int[rows][];
            for (int i = 0; i < rows; i++)
            {
                result[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    result[i][j] = marks[i, j];
                }
            }
            return result;
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<SkiJumpingDto>(json);

            List<Purple_2.Participant> participants = new List<Purple_2.Participant>();
            foreach (var p in dto.Participants)
            {
                var part = new Purple_2.Participant(p.Name, p.Surname);
                part.Jump(p.Distance, p.Marks[0], dto.Standard);
                participants.Add(part);
            }

            if (dto.Name == "100m")
            {
                var comp = new Purple_2.JuniorSkiJumping();
                comp.Add(participants.ToArray());
                return (T)(object)comp;
            }
            else
            {
                var comp = new Purple_2.ProSkiJumping();
                comp.Add(participants.ToArray());
                return (T)(object)comp;
            }
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<SkatingDto>(json);

            Purple_3.Skating skating = dto.TypeName switch
            {
                "FigureSkating" => new Purple_3.FigureSkating(dto.Moods, needModificate: false),
                "IceSkating" => new Purple_3.IceSkating(dto.Moods, needModificate: false),
            };

            foreach (var p in dto.Participants)
            {
                var participant = new Purple_3.Participant(p.Name, p.Surname);
                foreach (var m in p.Marks_d)
                    participant.Evaluate(m[0]);
                skating.Add(participant);
            }

            return (T)(object)skating;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<GroupDto>(json);

            var group = new Purple_4.Group(dto.Name);
            foreach (var s in dto.Sportsmen)
            {
                var sportsman = new Purple_4.Sportsman(s.Name, s.Surname);
                sportsman.Run(s.Time);
                group.Add(sportsman);
            }

            return group;
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<ReportDto>(json);

            var report = new Purple_5.Report();
            foreach (var r in dto.Researches)
            {
                var research = new Purple_5.Research(r.Name);
                if (r.Responses != null)
                {
                    foreach (var resp in r.Responses)
                        research.Add(new[] { resp.Animal, resp.CharacterTrait, resp.Concept });
                }
                report.AddResearch(research);
            }

            return report;
        }
    }
}
