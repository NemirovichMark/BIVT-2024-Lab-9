// PurpleJSONSerializer.cs
using System;
using System.IO;
using System.Linq;
using Lab_7;
using Newtonsoft.Json;

namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension => "json";

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            object dto = obj switch
            {
                Purple_1.Participant p => new
                {
                    p.Name,
                    p.Surname,
                    p.Coefs,
                    Marks = p.Marks
                              .Cast<int>()
                              .Chunk(p.Marks.GetLength(1))
                              .ToArray()
                },
                Purple_1.Judge j => new { j.Name, j.Marks },
                Purple_1.Competition c => new
                {
                    Judges = c.Judges.Select(j => new { j.Name, j.Marks }).ToArray(),
                    Participants = c.Participants.Select(p => new
                    {
                        p.Name,
                        p.Surname,
                        p.Coefs,
                        Marks = p.Marks
                                  .Cast<int>()
                                  .Chunk(p.Marks.GetLength(1))
                                  .ToArray()
                    }).ToArray()
                },
                
            };
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(dto, Newtonsoft.Json.Formatting.Indented));
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var json = File.ReadAllText(FilePath);

            if (typeof(T) == typeof(Purple_1.Participant))
            {
                var d = JsonConvert.DeserializeObject<ParticipantDto>(json)!;
                var p = new Purple_1.Participant(d.Name, d.Surname);
                p.SetCriterias(d.Coefs);
                foreach (var row in d.Marks) p.Jump(row);
                return p as T;
            }
            if (typeof(T) == typeof(Purple_1.Judge))
            {
                var d = JsonConvert.DeserializeObject<JudgeDto>(json)!;
                return new Purple_1.Judge(d.Name, d.Marks) as T;
            }
            if (typeof(T) == typeof(Purple_1.Competition))
            {
                var d = JsonConvert.DeserializeObject<CompetitionDto>(json)!;
                var judges = d.Judges.Select(jd => new Purple_1.Judge(jd.Name, jd.Marks)).ToArray();
                var comp = new Purple_1.Competition(judges);
                foreach (var pd in d.Participants)
                {
                    var p = new Purple_1.Participant(pd.Name, pd.Surname);
                    p.SetCriterias(pd.Coefs);
                    foreach (var row in pd.Marks) p.Jump(row);
                    comp.Add(p);
                }
                return comp as T;
            }
            return null;
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            var dto = new SkiJumpDto
            {
                Type = jumping.GetType().Name,
                Name = jumping.Name,
                Standard = jumping.Standard,
                Participants = jumping.Participants
                    .Select(p => new SkiJumpParticipantDto
                    {
                        Name = p.Name,
                        Surname = p.Surname,
                        Distance = p.Distance,
                        Marks = p.Marks
                    })
                    .ToArray()
            };
            File.WriteAllText(
                FilePath,
                JsonConvert.SerializeObject(dto, Newtonsoft.Json.Formatting.Indented)
            );
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var json = File.ReadAllText(FilePath);
            var d = JsonConvert.DeserializeObject<SkiJumpDto>(json)!;

            Purple_2.SkiJumping jmp = d.Type == nameof(Purple_2.JuniorSkiJumping) ? new Purple_2.JuniorSkiJumping() : new Purple_2.ProSkiJumping();

            foreach (var pd in d.Participants)
            {
                var p = new Purple_2.Participant(pd.Name, pd.Surname);
                p.Jump(pd.Distance, pd.Marks, jmp.Standard);
                jmp.Add(p);
            }
            return jmp as T;
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            var dto = new SkatingDto
            {
                Type = skating.GetType().Name,
                Moods = skating.Moods,
                Participants = skating.Participants
                    .Select(p => new SkatingParticipantDto
                    {
                        Name = p.Name,
                        Surname = p.Surname,
                        Marks = p.Marks,
                        Places = p.Places
                    })
                    .ToArray()
            };
            File.WriteAllText(
                FilePath,
                JsonConvert.SerializeObject(dto, Newtonsoft.Json.Formatting.Indented)
            );
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var json = File.ReadAllText(FilePath);
            var d = JsonConvert.DeserializeObject<SkatingDto>(json)!;

            Purple_3.Skating sk = d.Type == nameof(Purple_3.FigureSkating)
                ? new Purple_3.FigureSkating(d.Moods, false)
                : new Purple_3.IceSkating(d.Moods, false);

            foreach (var pd in d.Participants)
            {
                var p = new Purple_3.Participant(pd.Name, pd.Surname);
                foreach (var m in pd.Marks) p.Evaluate(m);
                sk.Add(p);
            }
            Purple_3.Participant.SetPlaces(sk.Participants);
            return sk as T;
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            var dto = new GroupDto
            {
                Name = group.Name,
                Sportsmen = group.Sportsmen
                    .Select(s => new SportDto
                    {
                        Name = s.Name,
                        Surname = s.Surname,
                        Time = s.Time
                    })
                    .ToArray()
            };
            File.WriteAllText(
                FilePath,
                JsonConvert.SerializeObject(dto, Newtonsoft.Json.Formatting.Indented)
            );
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var json = File.ReadAllText(FilePath);
            var d = JsonConvert.DeserializeObject<GroupDto>(json)!;

            var g = new Purple_4.Group(d.Name);
            foreach (var sd in d.Sportsmen)
            {
                var s = new Purple_4.Sportsman(sd.Name, sd.Surname);
                s.Run(sd.Time);
                g.Add(s);
            }
            return g;
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            var dto = new ReportDto
            {
                Researches = report.Researches
                    .Select(r => new ResearchDto
                    {
                        Name = r.Name,
                        Responses = r.Responses
                            .Select(resp => new ResponseDto
                            {
                                Animal = resp.Animal,
                                CharacterTrait = resp.CharacterTrait,
                                Concept = resp.Concept
                            })
                            .ToArray()
                    })
                    .ToArray()
            };
            File.WriteAllText(
                FilePath,
                JsonConvert.SerializeObject(dto, Newtonsoft.Json.Formatting.Indented)
            );
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var json = File.ReadAllText(FilePath);
            var d = JsonConvert.DeserializeObject<ReportDto>(json)!;

            var rpt = new Purple_5.Report();
            foreach (var rd in d.Researches)
            {
                var research = new Purple_5.Research(rd.Name);
                foreach (var rsp in rd.Responses)
                    research.Add(new[] { rsp.Animal, rsp.CharacterTrait, rsp.Concept });
                rpt.AddResearch(research);
            }
            return rpt;
        }

        // DTO-классы для JSON 
        private class ParticipantDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int[][] Marks { get; set; }
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
        private class SkiJumpDto
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Standard { get; set; }
            public SkiJumpParticipantDto[] Participants { get; set; }
        }
        private class SkiJumpParticipantDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }
        }
        private class SkatingDto
        {
            public string Type { get; set; }
            public double[] Moods { get; set; }
            public SkatingParticipantDto[] Participants { get; set; }
        }
        private class SkatingParticipantDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Marks { get; set; }
            public int[] Places { get; set; }
        }
        private class GroupDto
        {
            public string Name { get; set; }
            public SportDto[] Sportsmen { get; set; }
        }
        private class SportDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }
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
    }
}
