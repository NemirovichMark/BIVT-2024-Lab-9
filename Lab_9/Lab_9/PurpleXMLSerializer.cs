using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using static Lab_7.Purple_1;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";
        
        private void WriteDto<T> (T dto)
        {
            var xs = new XmlSerializer(typeof(T));
            using var xw = XmlWriter.Create(FilePath);
            xs.Serialize(xw, dto);
        }
        private T GetDto<T>()
        {
            var xs = new XmlSerializer(typeof(T));
            using var reader = XmlReader.Create(FilePath);
            var dto = (T)xs.Deserialize(reader);
            return dto;
        }
        public class Purple1ParticipantDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int[] Marks { get; set; }

            public static Purple1ParticipantDto ToDto(Purple_1.Participant participant)
            {
                return new Purple1ParticipantDto
                {
                    Name = participant.Name,
                    Surname = participant.Surname,
                    Coefs = (double[])participant.Coefs?.Clone(),
                    Marks = ((int[,])participant.Marks?.Clone()).Cast<int>().ToArray()
                };
            }
            public Purple_1.Participant FromDto()
            {
                var participant = new Purple_1.Participant(Name, Surname);
                participant.SetCriterias(Coefs);
                for (int i = 1; i <= 4; i++)
                {
                    participant.Jump(Marks[(7 * (i - 1))..(7 * i)]);
                }
                return participant;
            }
        }
        public class Purple1JudgeDto
        {
            public string Name { get; set; }
            public int[] Marks { get; set; }

            public static Purple1JudgeDto ToDto(Purple_1.Judge judge)
            {
                return new Purple1JudgeDto
                {
                    Name = judge.Name,
                    Marks = (int[])judge.Marks?.Clone(),
                };
            }
            public Purple_1.Judge FromDto()
            {
                var judge = new Purple_1.Judge(Name, Marks);
                return judge;
            }
        }
        public class Purple1CompetitionDto
        {
            public Purple1JudgeDto[] Judges { get; set; }
            public Purple1ParticipantDto[] Participants { get; set; }

            public static Purple1CompetitionDto ToDto(Purple_1.Competition competition)
            {
                return new Purple1CompetitionDto
                {
                    Judges = competition.Judges.Select(Purple1JudgeDto.ToDto).ToArray(),
                    Participants = competition.Participants.Select(Purple1ParticipantDto.ToDto).ToArray()
                };
            }
            public Purple_1.Competition FromDto()
            {
                var competition = new Purple_1.Competition(Judges.Select(dto => dto.FromDto()).ToArray());
                competition.Add(Participants.Select(dto => dto.FromDto()).ToArray());
                return competition;
            }
        }
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);

            switch (obj)
            {
                case Purple_1.Participant participant:
                    WriteDto<Purple1ParticipantDto>(Purple1ParticipantDto.ToDto(participant));
                    break;
                case Purple_1.Judge judge:
                    WriteDto<Purple1JudgeDto>(Purple1JudgeDto.ToDto(judge));
                    break;
                case Purple_1.Competition competition:
                    WriteDto<Purple1CompetitionDto>(Purple1CompetitionDto.ToDto(competition));
                    break;
                default:
                    return;
            }
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);

            if (typeof(T) == typeof(Purple_1.Participant))
            {
                return GetDto<Purple1ParticipantDto>().FromDto() as T;
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                return GetDto<Purple1JudgeDto>().FromDto() as T;
            }
            else
            {
                return GetDto<Purple1CompetitionDto>().FromDto() as T;
            }
        }
        public class Purple2ParticipantDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }

            public static Purple2ParticipantDto ToDto(Purple_2.Participant participant)
            {
                return new Purple2ParticipantDto
                {
                    Name = participant.Name,
                    Surname = participant.Surname,
                    Distance = participant.Distance,
                    Marks = (int[])participant.Marks?.Clone()
                };
            }
            public Purple_2.Participant FromDto(int standard)
            {
                var participant = new Purple_2.Participant(Name, Surname);
                participant.Jump(Distance, Marks, standard);
                return participant;
            }
        }
        public class Purple2SkiJumpingDto
        {
            public string Type { get; set; }
            public Purple2ParticipantDto[] Participants { get; set; }

            public static Purple2SkiJumpingDto ToDto(Purple_2.SkiJumping jumping)
            {
                return new Purple2SkiJumpingDto
                {
                    Type = jumping.GetType().Name,
                    Participants = jumping.Participants.Select(Purple2ParticipantDto.ToDto).ToArray()
                };
            }
            public Purple_2.SkiJumping FromDto()
            {
                Purple_2.SkiJumping jumping;

                if (Type == "JuniorSkiJumping") jumping = new Purple_2.JuniorSkiJumping();
                else jumping = new Purple_2.ProSkiJumping();

                jumping.Add(Participants.Select(dto => dto.FromDto(jumping.Standard)).ToArray());
                return jumping;
            }
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            WriteDto<Purple2SkiJumpingDto>(Purple2SkiJumpingDto.ToDto(jumping));
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            return GetDto<Purple2SkiJumpingDto>().FromDto() as T;
        }
        public class Purple3ParticipantDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Marks { get; set; }

            public static Purple3ParticipantDto ToDto(Purple_3.Participant participant)
            {
                return new Purple3ParticipantDto
                {
                    Name = participant.Name,
                    Surname = participant.Surname,
                    Marks = (double[])participant.Marks?.Clone()
                };
            }
            public Purple_3.Participant FromDto()
            {
                var participant = new Purple_3.Participant(Name, Surname);
                for (int i = 0; i < Marks.Length; i++) participant.Evaluate(Marks[i]);
                return participant;
            }
        }
        public class Purple3SkatingDto
        {
            public string Type { get; set; }
            public Purple3ParticipantDto[] Participants { get; set; }
            public double[] Moods { get; set; }

            public static Purple3SkatingDto ToDto(Purple_3.Skating skating)
            {
                return new Purple3SkatingDto
                {
                    Type = skating.GetType().Name,
                    Participants = skating.Participants.Select(Purple3ParticipantDto.ToDto).ToArray(),
                    Moods = (double[])skating.Moods?.Clone()
                };
            }
            public Purple_3.Skating FromDto()
            {
                Purple_3.Skating skating;
                if (Type == "IceSkating") skating = new Purple_3.IceSkating(Moods, false);
                else skating = new Purple_3.FigureSkating(Moods, false);

                skating.Add(Participants.Select(dto => dto.FromDto()).ToArray());
                Purple_3.Participant.SetPlaces(skating.Participants);

                return skating;
            }
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            WriteDto(Purple3SkatingDto.ToDto(skating));
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            return GetDto<Purple3SkatingDto>().FromDto() as T;
        }
        public class Purple4SportsmanDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }

            public static Purple4SportsmanDto ToDto(Purple_4.Sportsman sportsman)
            {
                return new Purple4SportsmanDto
                {
                    Name = sportsman.Name,
                    Surname = sportsman.Surname,
                    Time = sportsman.Time
                };
            }
            public Purple_4.Sportsman FromDto()
            {
                var sportsman = new Purple_4.Sportsman(Name, Surname);
                sportsman.Run(Time);
                return sportsman;
            }
        }
        public class Purple4GroupDto
        {
            public string Name { get; set; }
            public Purple4SportsmanDto[] Sportsmen { get; set; }

            public static Purple4GroupDto ToDto(Purple_4.Group group)
            {
                return new Purple4GroupDto
                {
                    Name = group.Name,
                    Sportsmen = group.Sportsmen.Select(Purple4SportsmanDto.ToDto).ToArray(),
                };
            }
            public Purple_4.Group FromDto()
            {
                var group = new Purple_4.Group(Name);
                group.Add(Sportsmen.Select(dto => dto.FromDto()).ToArray());
                return group;
            }
        }
        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            SelectFile(fileName);
            WriteDto<Purple4GroupDto>(Purple4GroupDto.ToDto(participant));
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            return GetDto<Purple4GroupDto>().FromDto();
        }
        public class Purple5ResponseDto
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }

            public static Purple5ResponseDto ToDto(Purple_5.Response response)
            {
                return new Purple5ResponseDto
                {
                    Animal = response.Animal,
                    CharacterTrait = response.CharacterTrait,
                    Concept = response.Concept
                };
            }
            public Purple_5.Response FromDto()
            {
                return new Purple_5.Response(Animal, CharacterTrait, Concept);
            }
        }
        public class Purple5ResearchDto
        {
            public string Name { get; set; }
            public Purple5ResponseDto[] Responses { get; set; }

            public static Purple5ResearchDto ToDto(Purple_5.Research research)
            {
                return new Purple5ResearchDto
                {
                    Name = research.Name,
                    Responses = research.Responses.Select(Purple5ResponseDto.ToDto).ToArray()
                };
            }
            public Purple_5.Research FromDto()
            {
                var research = new Purple_5.Research(Name);
                for (int i = 0; i < Responses.Length; i++)
                {
                    var response = Responses[i];
                    research.Add(new string[3] { response.Animal, response.CharacterTrait, response.Concept });
                }
                return research;
            }
        }
        public class Purple5ReportDto
        {
            public Purple5ResearchDto[] Researches { get; set; }

            public static Purple5ReportDto ToDto(Purple_5.Report report)
            {
                return new Purple5ReportDto
                {
                    Researches = report.Researches.Select(Purple5ResearchDto.ToDto).ToArray()
                };
            }
            public Purple_5.Report FromDto()
            {
                var report = new Purple_5.Report();
                report.AddResearch(Researches.Select(dto => dto.FromDto()).ToArray());
                return report;
            }
        }
        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            SelectFile(fileName);
            WriteDto<Purple5ReportDto>(Purple5ReportDto.ToDto(group));
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            return GetDto<Purple5ReportDto>().FromDto();
        }
    }
}
