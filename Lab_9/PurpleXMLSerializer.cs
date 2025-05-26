using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using Lab_7;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Lab_9;
public class PurpleXMLSerializer : PurpleSerializer
{

    public override string Extension => "xml";

    public class ParticipantDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public double TotalScore { get; set; }
        public double[] Coefs { get; set; }
        public int[][] Marks { get; set; }
    }

    public class JudgeDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public int[] Marks { get; set; }
    }

    public class CompetitionDTO
    {
        public string Type { get; set; }
        public JudgeDTO[] Judges { get; set; }
        public ParticipantDTO[] Participants { get; set; }
    }
    public class Purple2_ParticipantDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Distance { get; set; }
        public int Result { get; set; }
        public int[] Marks { get; set; }
    }
    public class SkiJumpingDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public int Standard { get; set; }

        public Purple2_ParticipantDTO[] Participants { get; set; }
    }

    public class Purple3_ParticipantDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int[] Places { get; set; }
        public int Score { get; set; }
        public double[] Marks { get; set; }
    }

    public class SkatingDTO
    {
        public string Type { get; set; }
        public Purple3_ParticipantDTO[] Participants { get; set; }
        public double[] Moods { get; set; }
    }

    public class SportsmanDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public double Time { get; set; }
    }

    public class GroupDTO
    {
        public string Name { get; set; }
        public SportsmanDTO[] Sportsmen { get; set; }
    }

    public class ResponseDTO
    {
        public string Animal { get; set; }
        public string CharacterTrait { get; set; }
        public string Concept { get; set; }
    }

    public class ResearchDTO
    {
        public string Name { get; set; }
        public ResponseDTO[] Responses { get; set; }
    }

    public class ReportDTO
    {
        public ResearchDTO[] Researches { get; set; }
    }

    private void SerializeDTO<T>(T dto)
    {
        using var writer = new StreamWriter(FilePath);
        var serializer = new XmlSerializer(typeof(T));
        serializer.Serialize(writer, dto);
    }

    private T DeserializeDTO<T>()
    {
        using var reader = new StreamReader(FilePath);
        var serializer = new XmlSerializer(typeof(T));
        return (T)serializer.Deserialize(reader);
    }

    public override void SerializePurple1<T>(T obj, string fileName)
    {
        SelectFile(fileName);
        switch (obj)
        {
            case Purple_1.Participant p:
                var participantDTO = new ParticipantDTO // содзаем дата трансфер обьект
                {
                    Type = nameof(Purple_1.Participant),
                    Name = p.Name,
                    Surname = p.Surname,
                    TotalScore = p.TotalScore,
                    Coefs = p.Coefs,
                    Marks = ConvertToJagged(p.Marks)
                };
                SerializeDTO(participantDTO); // сериализуем, у этого метода уже есть путь к файлу
                break;

            case Purple_1.Judge j:
                var judgeDTO = new JudgeDTO
                {
                    Type = nameof(Purple_1.Judge),
                    Name = j.Name,
                    Marks = j.Marks
                };
                SerializeDTO(judgeDTO);
                break;

            case Purple_1.Competition c:
                var judges = c.Judges
                    .Select(j => new JudgeDTO
                    {
                        Type = nameof(Purple_1.Judge),
                        Name = j.Name,
                        Marks = j.Marks
                    })
                    .ToArray();

                var participants = c.Participants
                    .Select(pItem => new ParticipantDTO
                    {
                        Type = nameof(Purple_1.Participant),
                        Name = pItem.Name,
                        Surname = pItem.Surname,
                        TotalScore = pItem.TotalScore,
                        Coefs = pItem.Coefs,
                        Marks = ConvertToJagged(pItem.Marks)
                    })
                    .ToArray();
                    
                SerializeDTO(new CompetitionDTO
                {
                    Type = nameof(Purple_1.Competition),
                    Judges = judges,
                    Participants = participants
                });
                break;
        }
    }

    public override T DeserializePurple1<T>(string fileName)
    {
        SelectFile(fileName);

        if (typeof(T) == typeof(Purple_1.Participant)) // десереализуем и заполняем через внутр методы обьекта
        {
            var dto = DeserializeDTO<ParticipantDTO>();
            var p = new Purple_1.Participant(dto.Name, dto.Surname);
            p.SetCriterias(dto.Coefs);
            for (var i = 0; i < dto.Marks.GetLength(0); i++) { 
                p.Jump(dto.Marks[i].ToArray());
            }
            return p as T;
        }

        if (typeof(T) == typeof(Purple_1.Judge))
        {
            var dto = DeserializeDTO<JudgeDTO>();
            return new Purple_1.Judge(dto.Name, dto.Marks) as T;
        }

        if (typeof(T) == typeof(Purple_1.Competition))
        {
            var dto = DeserializeDTO<CompetitionDTO>();
            var judges = dto.Judges
                .Select(dj => new Purple_1.Judge(dj.Name, dj.Marks))
                .ToArray();

            var comp = new Purple_1.Competition(judges);
            foreach (var pd in dto.Participants)
            {
                var p = new Purple_1.Participant(pd.Name, pd.Surname);
                p.SetCriterias(pd.Coefs);
                for (var i = 0; i < pd.Marks.GetLength(0); i++) { 
                    p.Jump(pd.Marks[i].ToArray());
                }
                comp.Add(p);
            }
            return comp as T;
        }

        return null as T;
    }

    public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
    {
        SelectFile(fileName);
        
        var participantsData = jumping.Participants
            .Select(pItem => new Purple2_ParticipantDTO
            {
                Type = nameof(Purple_2.Participant),
                Name = pItem.Name,
                Surname = pItem.Surname,
                Result = pItem.Result,
                Marks = pItem.Marks,
                Distance = pItem.Distance
            })
            .ToArray();
        
        var skiDTO = new SkiJumpingDTO
        {
            Type = jumping is Purple_2.JuniorSkiJumping ? "JuniorSkiJumping" : "ProSkiJumping",
            Name = jumping.Name,
            Standard = jumping.Standard,
            Participants = participantsData
        };
        SerializeDTO(skiDTO);
    }

    public override T DeserializePurple2SkiJumping<T>(string fileName)
    {
        SelectFile(fileName);
        var dto = DeserializeDTO<SkiJumpingDTO>();
        Purple_2.SkiJumping jumping = dto.Type == "JuniorSkiJumping" 
            ? new Purple_2.JuniorSkiJumping() 
            : new Purple_2.ProSkiJumping();
            
        foreach (var participantData in dto.Participants)
        {
            var newParticipant = new Purple_2.Participant(participantData.Name, participantData.Surname);
            newParticipant.Jump(participantData.Distance, participantData.Marks, jumping.Standard);
            jumping.Add(newParticipant);
        }
        return jumping as T;
    }

    public override void SerializePurple3Skating<T>(T skating, string fileName)
    {
        SelectFile(fileName);
        
        var participantsData = skating.Participants
            .Select(pItem => new Purple3_ParticipantDTO
            {
                Type = nameof(Purple_3.Participant),
                Name = pItem.Name,
                Surname = pItem.Surname,
                Marks = pItem.Marks,
                Places = pItem.Places,
                Score = pItem.Score
            })
            .ToArray();
        
        var skatingDTO = new SkatingDTO
        {
            Type = skating is Purple_3.IceSkating ? "IceSkating" : "FigureSkating",
            Participants = participantsData,
            Moods = skating.Moods
        };
        SerializeDTO(skatingDTO);
    }



    public override T DeserializePurple3Skating<T>(string fileName)
    {
        SelectFile(fileName);
        var dto = DeserializeDTO<SkatingDTO>();
        Purple_3.Skating skating = dto.Type == "IceSkating"
            ? new Purple_3.IceSkating(dto.Moods, needModificate: false)
            : new Purple_3.FigureSkating(dto.Moods, needModificate: false);

        foreach (var participantData in dto.Participants)
        {
            var participant = new Purple_3.Participant(participantData.Name, participantData.Surname);
            foreach (var mark in participantData.Marks) {
                participant.Evaluate(mark);
            }
            skating.Add(participant);
        }
        return skating as T;
    }

    public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
    {
        SelectFile(fileName);
        
        var sportsmenData = participant.Sportsmen
            .Select(sportsmanItem => new SportsmanDTO
            {
                Name = sportsmanItem.Name,
                Surname = sportsmanItem.Surname,
                Time = sportsmanItem.Time
            })
            .ToArray();
        
        var groupDTO = new GroupDTO
        {
            Name = participant.Name,
            Sportsmen = sportsmenData
        };
        SerializeDTO(groupDTO);
    }

    public override Purple_4.Group DeserializePurple4Group(string fileName)
    {
        SelectFile(fileName);
        var dto = DeserializeDTO<GroupDTO>();
        var group = new Purple_4.Group(dto.Name);

        foreach (var sportsmanData in dto.Sportsmen)
        {
            Purple_4.Sportsman sportsman = new Purple_4.Sportsman(sportsmanData.Name, sportsmanData.Surname);
            sportsman.Run(sportsmanData.Time);
            group.Add(sportsman);
        }
        return group;
    }

    public override void SerializePurple5Report(Purple_5.Report group, string fileName)
    {
        SelectFile(fileName);
        
        var researchesData = group.Researches
            .Select(researchItem => new ResearchDTO
            {
                Name = researchItem.Name,
                Responses = researchItem.Responses
                    .Select(responseItem => new ResponseDTO
                    {
                        Animal = responseItem.Animal,
                        CharacterTrait = responseItem.CharacterTrait,
                        Concept = responseItem.Concept
                    })
                    .ToArray()
            })
            .ToArray();
        
        var reportDTO = new ReportDTO
        {
            Researches = researchesData
        };
        SerializeDTO(reportDTO);
    }

    public override Purple_5.Report DeserializePurple5Report(string fileName)
    {
        SelectFile(fileName);
        var dto = DeserializeDTO<ReportDTO>();
        var report = new Purple_5.Report();
        
        foreach (var researchData in dto.Researches)
        {
            var research = new Purple_5.Research(researchData.Name);
            foreach (var responseData in researchData.Responses) {
                research.Add([responseData.Animal, responseData.CharacterTrait, responseData.Concept]);
            }
            report.AddResearch(research);
        }
        return report;
    }




    private T[,] ConvertToMatrix<T>(T[][] jagged)
    {
        var rowCount = jagged.Length;
        var colCount = jagged[0].Length;
        var result = new T[rowCount, colCount];

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                result[row, col] = jagged[row][col];
            }
        }

        return result;
    }

    private T[][] ConvertToJagged<T>(T[,] matrix)
    {
        var rows = matrix.GetLength(0);
        var cols = matrix.GetLength(1);
        var jagged = new T[rows][];

        for (int i = 0; i < rows; i++)
        {
            jagged[i] = new T[cols];
            for (int j = 0; j < cols; j++)
            {
                jagged[i][j] = matrix[i, j];
            }
        }

        return jagged;
    }
}