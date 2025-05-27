using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lab_7;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";
        [XmlRoot("Participant")]
        public class Purple1ParticipantDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int[][] Marks { get; set; }

            public Purple1ParticipantDto() { }

            public Purple1ParticipantDto(Purple_1.Participant p)
            {
                Name = p.Name;
                Surname = p.Surname;
                Coefs = p.Coefs;
                Marks = ConvertMarks(p.Marks);
            }

            public Purple_1.Participant ToEntity()
            {
                var participant = new Purple_1.Participant(Name, Surname);
                participant.SetCriterias(Coefs);

                foreach (var mark in Marks)
                {
                    participant.Jump(mark);
                }

                return participant;
            }

            private static int[][] ConvertMarks(int[,] marks)
            {
                var result = new int[marks.GetLength(0)][];
                for (int i = 0; i < marks.GetLength(0); i++)
                {
                    result[i] = new int[marks.GetLength(1)];
                    for (int j = 0; j < marks.GetLength(1); j++)
                    {
                        result[i][j] = marks[i, j];
                    }
                }
                return result;
            }
        }

        [XmlRoot("Judge")]
        public class Purple1JudgeDto
        {
            public string Name { get; set; }
            public int[] Marks { get; set; }

            public Purple1JudgeDto() { }

            public Purple1JudgeDto(Purple_1.Judge j)
            {
                Name = j.Name;
                Marks = j.Marks;
            }

            public Purple_1.Judge ToEntity() => new Purple_1.Judge(Name, Marks);
        }

        [XmlRoot("Competition")]
        public class Purple1CompetitionDto
        {
            public Purple1JudgeDto[] Judges { get; set; }
            public Purple1ParticipantDto[] Participants { get; set; }

            public Purple1CompetitionDto() { }

            public Purple1CompetitionDto(Purple_1.Competition c)
            {
                Judges = c.Judges.Select(j => new Purple1JudgeDto(j)).ToArray();
                Participants = c.Participants.Select(p => new Purple1ParticipantDto(p)).ToArray();
            }

            public Purple_1.Competition ToEntity()
            {
                var competition = new Purple_1.Competition(
                    Judges.Select(j => j.ToEntity()).ToArray());

                foreach (var participant in Participants.Select(p => p.ToEntity()))
                {
                    competition.Add(participant);
                }

                return competition;
            }
        }
        private object CreateDto(object obj)
        {
            return obj switch
            {
                Purple_1.Participant p => new Purple1ParticipantDto(p),
                Purple_1.Judge j => new Purple1JudgeDto(j),
                Purple_1.Competition c => new Purple1CompetitionDto(c),
                _ => throw new ArgumentException("Unsupported type")
            };
        }

        private object ConvertToEntity(object dto)
        {
            return dto switch
            {
                Purple1ParticipantDto p => p.ToEntity(),
                Purple1JudgeDto j => j.ToEntity(),
                Purple1CompetitionDto c => c.ToEntity(),
                _ => throw new ArgumentException("Unsupported DTO type")
            };
        }

        private XmlSerializer GetSerializer(Type type)
        {
            return type switch
            {
                _ when type == typeof(Purple_1.Participant) => new XmlSerializer(typeof(Purple1ParticipantDto)),
                _ when type == typeof(Purple_1.Judge) => new XmlSerializer(typeof(Purple1JudgeDto)),
                _ when type == typeof(Purple_1.Competition) => new XmlSerializer(typeof(Purple1CompetitionDto)),
                _ => throw new ArgumentException("Unsupported type")
            };
        }
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);

            using var writer = new StreamWriter(FilePath);
            var serializer = GetSerializer(typeof(T));
            serializer.Serialize(writer, CreateDto(obj));
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);

            using var reader = new StreamReader(FilePath);
            var serializer = GetSerializer(typeof(T));
            var dto = serializer.Deserialize(reader);

            return (T)ConvertToEntity(dto);
        }
        [XmlRoot("Participant")]
        public class Purple2ParticipantDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }
            public int Result { get; set; }

            public Purple2ParticipantDto() { }

            public Purple2ParticipantDto(Purple_2.Participant p)
            {
                Name = p.Name;
                Surname = p.Surname;
                Distance = p.Distance;
                Marks = p.Marks;
                Result = p.Result;
            }

            public Purple_2.Participant ToEntity(int standard)
            {
                var participant = new Purple_2.Participant(Name, Surname);
                if (Distance != -1)
                {
                    participant.Jump(Distance, Marks, standard);
                }
                return participant;
            }
        }

        [XmlRoot("SkiJumping")]
        public class Purple2SkiJumpingDto
        {
            public string Name { get; set; }
            public int Standard { get; set; }
            public Purple2ParticipantDto[] Participants { get; set; }
            public string Type { get; set; }

            public Purple2SkiJumpingDto() { }

            public Purple2SkiJumpingDto(Purple_2.SkiJumping jumping)
            {
                Name = jumping.Name;
                Standard = jumping.Standard;
                Participants = jumping.Participants.Select(p => new Purple2ParticipantDto(p)).ToArray();
                Type = jumping.GetType().Name;
            }

            public Purple_2.SkiJumping ToEntity()
            {
                Purple_2.SkiJumping jumping = Type switch
                {
                    nameof(Purple_2.JuniorSkiJumping) => new Purple_2.JuniorSkiJumping(),
                    nameof(Purple_2.ProSkiJumping) => new Purple_2.ProSkiJumping(),
                    _ => throw new InvalidOperationException("Unknown SkiJumping type")
                };

                foreach (var participantDto in Participants)
                {
                    jumping.Add(participantDto.ToEntity(Standard));
                }

                return jumping;
            }
        }
        private void SerializeToFile<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            var serializer = new XmlSerializer(typeof(T));
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, obj);
        }

        private T DeserializeFromFile<T>(string fileName) where T : class
        {
            SelectFile(fileName);
            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StreamReader(FilePath);
            return serializer.Deserialize(reader) as T;
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            if (jumping is Purple_2.SkiJumping skiJumping)
            {
                var dto = new Purple2SkiJumpingDto(skiJumping);
                SerializeToFile(dto, fileName);
            }
            else
            {
                throw new ArgumentException("Invalid type for Purple2 serialization");
            }
        } 
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        { 
            var dto = DeserializeFromFile<Purple2SkiJumpingDto>(fileName); 
            return (T) (object) dto.ToEntity();
        }

        [XmlRoot("Participant")]
        public class Purple3ParticipantDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Marks { get; set; }
            public int[] Places { get; set; }

            public Purple3ParticipantDto() { }

            public Purple3ParticipantDto(Purple_3.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = participant.Marks;
                Places = participant.Places;
            }

            public Purple_3.Participant ToParticipant()
            {
                var participant = new Purple_3.Participant(Name, Surname);

                // Восстанавливаем оценки
                if (Marks != null)
                {
                    foreach (var mark in Marks)
                    {
                        if (mark > 0) // Оцениваем только ненулевые оценки
                            participant.Evaluate(mark);
                    }
                }

                // Места будут восстановлены через SetPlaces после десериализации
                return participant;
            }
        }
        [XmlRoot("Skating")]
        public class Purple3SkatingDto
        {
            public string Type { get; set; }
            public double[] Moods { get; set; }
            public Purple3ParticipantDto[] Participants { get; set; }
            public int[][] Places { get; set; }

            public Purple3SkatingDto() { }

            public Purple3SkatingDto(Purple_3.Skating skating)
            {
                Type = skating.GetType().Name;
                Moods = skating.Moods;
                Participants = skating.Participants.Select(p => new Purple3ParticipantDto(p)).ToArray();
                Places = skating.Participants.Select(p => p.Places).ToArray();
            }

            public Purple_3.Skating ToSkating()
            {
                Purple_3.Skating skating = Type switch
                {
                    nameof(Purple_3.FigureSkating) => new Purple_3.FigureSkating(Moods, false),
                    nameof(Purple_3.IceSkating) => new Purple_3.IceSkating(Moods, false),
                    _ => throw new InvalidOperationException("Unknown Skating type")
                };

                // Добавляем участников
                if (Participants != null)
                {
                    foreach (var participantDto in Participants)
                    {
                        skating.Add(participantDto.ToParticipant());
                    }
                }

                return skating;
            }
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            if (skating == null || fileName == null) return;

            SelectFile(fileName);

            if (skating is Purple_3.Skating skatingObj)
            {
                var dto = new Purple3SkatingDto(skatingObj);
                var serializer = new XmlSerializer(typeof(Purple3SkatingDto));
                using var writer = new StreamWriter(FilePath);
                serializer.Serialize(writer, dto);
            }
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            SelectFile(fileName);

            if (!File.Exists(FilePath))
                throw new FileNotFoundException($"File not found: {FilePath}");

            var serializer = new XmlSerializer(typeof(Purple3SkatingDto));
            using var reader = new StreamReader(FilePath);
            var dto = (Purple3SkatingDto)serializer.Deserialize(reader);

            var skating = dto.ToSkating();

            Purple_3.Participant.SetPlaces(skating.Participants);

            return (T)(object)dto.ToSkating();
        }
        [XmlRoot("Sportsman")]
        public class SportsmanDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }

            public SportsmanDto() { }

            public SportsmanDto(Purple_4.Sportsman sportsman)
            {
                Name = sportsman.Name;
                Surname = sportsman.Surname;
                Time = sportsman.Time;
            }

            public Purple_4.Sportsman ToSportsman()
            {
                var sportsman = new Purple_4.Sportsman(Name, Surname);
                if (Time > 0) sportsman.Run(Time);
                return sportsman;
            }
        }

        [XmlRoot("Group")]
        public class GroupDto
        {
            public string Name { get; set; }
            public SportsmanDto[] Sportsmen { get; set; }

            public GroupDto() { }

            public GroupDto(Purple_4.Group group)
            {
                Name = group.Name;
                if (group.Sportsmen != null)
                {
                    Sportsmen = new SportsmanDto[group.Sportsmen.Length];
                    for (int i = 0; i < group.Sportsmen.Length; i++)
                    {
                        Sportsmen[i] = new SportsmanDto(group.Sportsmen[i]);
                    }
                }
            }

            public Purple_4.Group ToGroup()
            {
                var group = new Purple_4.Group(Name);

                if (Sportsmen != null)
                {
                    foreach (var sportsmanDto in Sportsmen)
                    {
                        group.Add(sportsmanDto.ToSportsman());
                    }
                }

                return group;
            }
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            if (group == null || fileName == null) return;

            SelectFile(fileName);

            var dto = new GroupDto(group);
            var serializer = new XmlSerializer(typeof(GroupDto));
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, dto);
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            SelectFile(fileName);

            if (!File.Exists(FilePath))
                throw new FileNotFoundException($"File not found: {FilePath}");

            var serializer = new XmlSerializer(typeof(GroupDto));
            using var reader = new StreamReader(FilePath);
            var dto = (GroupDto)serializer.Deserialize(reader);

            return dto.ToGroup();
        }
        [XmlRoot("Response")]
        public class ResponseDto
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }

            public ResponseDto() { }

            public ResponseDto(Purple_5.Response response)
            {
                Animal = response.Animal;
                CharacterTrait = response.CharacterTrait;
                Concept = response.Concept;
            }

            public Purple_5.Response ToResponse()
            {
                return new Purple_5.Response(Animal, CharacterTrait, Concept);
            }
        }

        [XmlRoot("Research")]
        public class ResearchDto
        {
            public string Name { get; set; }
            public ResponseDto[] Responses { get; set; }

            public ResearchDto() { }

            public ResearchDto(Purple_5.Research research)
            {
                Name = research.Name;
                Responses = research.Responses?.Select(r => new ResponseDto(r)).ToArray();
            }

            public Purple_5.Research ToResearch()
            {
                var research = new Purple_5.Research(Name);
                if (Responses != null)
                {
                    foreach (var responseDto in Responses)
                    {
                        research.Add(new string[] {
                            responseDto.Animal,
                            responseDto.CharacterTrait,
                            responseDto.Concept
                        });
                    }
                }
                return research;
            }
        }

        [XmlRoot("Report")]
        public class ReportDto
        {
            public ResearchDto[] Researches { get; set; }

            public ReportDto() { }

            public ReportDto(Purple_5.Report report)
            {
                Researches = report.Researches?.Select(r => new ResearchDto(r)).ToArray();
            }

            public Purple_5.Report ToReport()
            {
                var report = new Purple_5.Report();

                if (Researches != null)
                {
                    foreach (var researchDto in Researches)
                    {
                        report.AddResearch(researchDto.ToResearch());
                    }
                }

                return report;
            }
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            if (report == null || fileName == null) return;

            SelectFile(fileName);

            var dto = new ReportDto(report);
            var serializer = new XmlSerializer(typeof(ReportDto));
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, dto);
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            SelectFile(fileName);

            if (!File.Exists(FilePath))
                throw new FileNotFoundException($"File not found: {FilePath}");

            var serializer = new XmlSerializer(typeof(ReportDto));
            using var reader = new StreamReader(FilePath);
            var dto = (ReportDto)serializer.Deserialize(reader);

            return dto.ToReport();
        }
    }
}
