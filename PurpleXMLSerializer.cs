using System.Xml.Serialization;
using Lab_7;

namespace Lab_9{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";


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
                Marks = ConvertToDto(participant.Marks)
            };

            SerializeToFile(dto, FilePath);
        }

        private List<RowDto> ConvertToDto(int[,] marks)
        {
            var rows = new List<RowDto>();
            for (int i = 0; i < marks.GetLength(0); i++)
            {
                var row = new int[marks.GetLength(1)];
                for (int j = 0; j < marks.GetLength(1); j++)
                {
                    row[j] = marks[i, j];
                }
                rows.Add(new RowDto { Values = row });
            }
            return rows;
        }

        private void SerializeJudge(Purple_1.Judge judge, string fileName)
        {
            SelectFile(fileName);
            var dto = new JudgeDto
            {
                Name = judge.Name,
                Marks = judge.Marks
            };
            SerializeToFile(dto, FilePath);
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
                }).ToList(),

                Participants = comp.Participants.Select(p => new ParticipantDto
                {
                    Name = p.Name,
                    Surname = p.Surname,
                    Coefs = p.Coefs,
                    Marks = ConvertToDto(p.Marks)
                }).ToList()
            };

            SerializeToFile(dto, FilePath);
        }





        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            var competition = (Purple_2.SkiJumping)(object)jumping;

            var container = new SkiJumpingContainer
            {
                Name = competition.Name,
                Standard = competition.Standard,
                Participants = competition.Participants.Select(p => new SkiParticipantContainer
                {
                    Name = p.Name,
                    Surname = p.Surname,
                    Result = p.Result,
                    Marks = p.Marks.ToList(),
                    Distance = p.Distance
                }).ToList()
            };

            SerializeToFile(container, FilePath);
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            var s = skating as Purple_3.Skating;

            var container = new SkatingContainer
            {
                TypeName = s.GetType().Name,
                Moods = s.Moods.ToList(),
                Participants = s.Participants.Select(p => new SkatingParticipantContainer
                {
                    Name = p.Name,
                    Surname = p.Surname,
                    Marks = p.Marks.ToList()
                }).ToList()
            };

            SerializeToFile(container, FilePath);
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);

            var container = new GroupContainer
            {
                Name = group.Name,
                Sportsmen = group.Sportsmen.Select(s => new GroupSportsmanContainer
                {
                    Name = s.Name,
                    Surname = s.Surname,
                    Time = s.Time
                }).ToList()
            };

            SerializeToFile(container, FilePath);
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);

            var container = new ReportContainer
            {
                Researches = report.Researches.Select(r => new ResearchContainer
                {
                    Name = r.Name,
                    Responses = r.Responses.Select(resp => new ResponseContainer
                    {
                        Animal = resp.Animal,
                        CharacterTrait = resp.CharacterTrait,
                        Concept = resp.Concept
                    }).ToList()
                }).ToList()
            };

            SerializeToFile(container, FilePath);
        }


        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);

            if (typeof(T) == typeof(Purple_1.Participant))
                return (T)(object)DeserializeParticipant();
            else if (typeof(T) == typeof(Purple_1.Judge))
                return (T)(object)DeserializeJudge();
            else
                return (T)(object)DeserializeCompetition();
        }

        private Purple_1.Participant DeserializeParticipant()
        {
            var dto = DeserializeFromFile<ParticipantDto>(FilePath);

            var participant = new Purple_1.Participant(dto.Name, dto.Surname);
            participant.SetCriterias(dto.Coefs);

            int[,] marks = ConvertFromDto(dto.Marks);
            CopyMarks(participant, marks);

            return participant;
        }

        private int[,] ConvertFromDto(List<RowDto> rows)
        {
            int[,] marks = new int[rows.Count, rows[0].Values.Length];
            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < rows[i].Values.Length; j++)
                {
                    marks[i, j] = rows[i].Values[j];
                }
            }
            return marks;
        }

        private void CopyMarks(Purple_1.Participant participant, int[,] marks)
        {
            if (marks == null) return;

            for (int i = 0; i < marks.GetLength(0); i++)
            {
                int[] singleJumpMarks = new int[marks.GetLength(1)];
                for (int j = 0; j < marks.GetLength(1); j++)
                {
                    singleJumpMarks[j] = marks[i, j];
                }
                participant.Jump(singleJumpMarks);
            }
        }


        private Purple_1.Judge DeserializeJudge()
        {
            var dto = DeserializeFromFile<JudgeDto>(FilePath);
            return new Purple_1.Judge(dto.Name, dto.Marks);
        }

        private Purple_1.Competition DeserializeCompetition()
        {
            var dto = DeserializeFromFile<CompetitionDto>(FilePath);
            var competition = new Purple_1.Competition(dto.Judges.Select(j => new Purple_1.Judge(j.Name, j.Marks)).ToArray());

            foreach (var p in dto.Participants)
            {
                var participant = new Purple_1.Participant(p.Name, p.Surname);
                participant.SetCriterias(p.Coefs);
                CopyMarks(participant, ConvertFromDto(p.Marks));
                competition.Add(participant);
            }

            return competition;
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var container = DeserializeFromFile<SkiJumpingContainer>(FilePath);

            var participants = container.Participants.Select(p =>
            {
                var part = new Purple_2.Participant(p.Name, p.Surname);
                part.Jump(p.Distance, p.Marks.ToArray(), container.Standard);
                return part;
            }).ToArray();

            if (container.Name == "100m")
            {
                var competition = new Purple_2.JuniorSkiJumping();
                competition.Add(participants);
                return (T)(object)competition;
            }
            else
            {
                var competition = new Purple_2.ProSkiJumping();
                competition.Add(participants);
                return (T)(object)competition;
            }
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var container = DeserializeFromFile<SkatingContainer>(FilePath);

            Purple_3.Skating result = container.TypeName switch
            {
                "FigureSkating" => new Purple_3.FigureSkating(container.Moods.ToArray(), needModificate: false),
                "IceSkating" => new Purple_3.IceSkating(container.Moods.ToArray(), needModificate: false),
            };

            foreach (var p in container.Participants)
            {
                var participant = new Purple_3.Participant(p.Name, p.Surname);
                foreach (var mark in p.Marks)
                    participant.Evaluate(mark);

                result.Add(participant);
            }

            return (T)(object)result;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var container = DeserializeFromFile<GroupContainer>(FilePath);

            var group = new Purple_4.Group(container.Name);

            foreach (var s in container.Sportsmen)
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
            var container = DeserializeFromFile<ReportContainer>(FilePath);

            var report = new Purple_5.Report();

            foreach (var r in container.Researches)
            {
                var research = new Purple_5.Research(r.Name);
                if (r.Responses != null)
                {
                    foreach (var resp in r.Responses)
                    {
                        research.Add(new[] { resp.Animal, resp.CharacterTrait, resp.Concept });
                    }
                }
                report.AddResearch(research);
            }

            return report;
        }



        private void SerializeToFile<T>(T obj, string path)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, obj);
            }
        }

        private T DeserializeFromFile<T>(string path)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StreamReader(path))
            {
                return (T)serializer.Deserialize(reader);
            }
        }



        [XmlRoot("Participant")]
        public class ParticipantDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }

            [XmlArray]
            [XmlArrayItem("Coef")]
            public double[] Coefs { get; set; }

            [XmlArray]
            [XmlArrayItem("RowDto")]
            public List<RowDto> Marks { get; set; }
        }

        public class RowDto
        {
            [XmlElement("Value")]
            public int[] Values { get; set; }
        }

        [XmlRoot("Judge")]
        public class JudgeDto
        {
            public string Name { get; set; }

            [XmlArray]
            [XmlArrayItem("Mark")]
            public int[] Marks { get; set; }
        }

        [XmlRoot("Competition")]
        public class CompetitionDto
        {
            [XmlArray]
            [XmlArrayItem("Judge")]
            public List<JudgeDto> Judges { get; set; }

            [XmlArray]
            [XmlArrayItem("Participant")]
            public List<ParticipantDto> Participants { get; set; }
        }



        [XmlRoot("SkiJumping")]
        public class SkiJumpingContainer
        {
            public string Name { get; set; }
            public int Standard { get; set; }

            [XmlArray]
            [XmlArrayItem("Participant")]
            public List<SkiParticipantContainer> Participants { get; set; }
        }

        public class SkiParticipantContainer
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Result { get; set; }
            public int Distance { get; set; }

            [XmlArray]
            [XmlArrayItem("Mark")]
            public List<int> Marks { get; set; }
        }

        [XmlRoot("Skating")]
        public class SkatingContainer
        {
            public string TypeName { get; set; }

            [XmlArray]
            [XmlArrayItem("Mood")]
            public List<double> Moods { get; set; }

            [XmlArray]
            [XmlArrayItem("Participant")]
            public List<SkatingParticipantContainer> Participants { get; set; }
        }

        public class SkatingParticipantContainer
        {
            public string Name { get; set; }
            public string Surname { get; set; }

            [XmlArray]
            [XmlArrayItem("Mark")]
            public List<double> Marks { get; set; }
        }

        [XmlRoot("Group")]
        public class GroupContainer
        {
            public string Name { get; set; }

            [XmlArray]
            [XmlArrayItem("Sportsman")]
            public List<GroupSportsmanContainer> Sportsmen { get; set; }
        }

        public class GroupSportsmanContainer
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }
        }

        [XmlRoot("Report")]
        public class ReportContainer
        {
            [XmlArray]
            [XmlArrayItem("Research")]
            public List<ResearchContainer> Researches { get; set; }
        }

        public class ResearchContainer
        {
            public string Name { get; set; }

            [XmlArray]
            [XmlArrayItem("Response")]
            public List<ResponseContainer> Responses { get; set; }
        }

        public class ResponseContainer
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }
        }
    }
}