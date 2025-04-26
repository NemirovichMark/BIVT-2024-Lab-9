using System.Runtime.CompilerServices;

namespace Lab_9;

using Lab_7;
using System.Xml.Serialization;

public class PurpleXMLSerializer : PurpleSerializer
{
    public override string Extension => "xml";
    public class Purple1_ParticipantDTO {
        public string Type {get; set;} 
        public string Name { get; set; }
        public string Surname { get; set; }
        public double TotalScore { get; set; }
        public double[] Coefs { get; set; }
        public int[][] Marks { get; set; }
    }
    public class JudgeDTO
    {
        public string Type {get; set;}
        public string Name { get; set; }
        public int[] Marks {get; set;}
    }
    public class CompetitionDTO
    {
        public string Type {get; set;}
        public JudgeDTO[] Judges { get; set; }
        public Purple1_ParticipantDTO[] Participants { get; set; }
    }
    
    public class Purple2_ParticipantDTO {
        public string Type {get; set;} 
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Distance {get; set;}
        public int Result {get; set;}
        public int[] Marks { get; set; }
    }
    public class SkiJumpingDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public int Standard { get; set; }
        
        public Purple2_ParticipantDTO[] Participants { get; set; }
    }
    
    public class Purple3_ParticipantDTO {
        public string Type {get; set;} 
        public string Name { get; set; }
        public string Surname { get; set; }
        public int[] Places {get; set;}
        public int Score {get; set;}
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

    private T[,] Rectangularize<T>(T[][] jaggedArr)
    {
        int rows = jaggedArr.Length, cols = jaggedArr[0].Length;
        T[,] arr = new T[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for(int j = 0; j < cols; j++) arr[i, j] = jaggedArr[i][j];
        }
        return arr;
    }

    private T[][] Jaggedize<T>(T[,] rectangularArr)
    {
        int rows = rectangularArr.GetLength(0), cols = rectangularArr.GetLength(1);
        T[][] arr = new T[rows][];
        for (int i = 0; i < rows; i++)
        {
            arr[i] = new T[cols];
            for(int j = 0; j < cols; j++) arr[i][j] = rectangularArr[i, j];
        }
        return arr;
    }
    
    private void SerializeDTO<T>(T dto)
    {
        using (var writer = new StreamWriter(FilePath))
        {
            new XmlSerializer(typeof(T)).Serialize(writer, dto);
            writer.Close();
        }
    }

    private T DeserializeDTO<T>()
    {
        using (var reader = new StreamReader(FilePath))
        {
            var dto = (T)new XmlSerializer(typeof(T)).Deserialize(reader);
            reader.Close();
            return dto;
        }
    }

    public override void SerializePurple1<T>(T obj, string fileName)
    {
        SelectFile(fileName);
        switch (obj)
        {
            case Purple_1.Participant p:
                var pDTO = new Purple1_ParticipantDTO
                {
                    Type = "Purple_1.Participant",
                    Name = p.Name,
                    Surname = p.Surname,
                    TotalScore = p.TotalScore,
                    Coefs = p.Coefs,
                    Marks = Jaggedize(p.Marks),
                };
                SerializeDTO(pDTO);
                break;
            case Purple_1.Judge j:
                var jDTO = new JudgeDTO
                {
                    Type = "Judge",
                    Name = j.Name,
                    Marks = j.Marks,
                };
                SerializeDTO(jDTO);
                break;
            case Purple_1.Competition c:
                JudgeDTO[] judges = new JudgeDTO[c.Judges.Length];
                for(int i = 0; i < judges.Length; i++)
                {
                    judges[i] = new JudgeDTO()
                    {
                        Type = "Judge",
                        Name = c.Judges[i].Name,
                        Marks = c.Judges[i].Marks,
                    };
                }
                
                Purple1_ParticipantDTO[] participants = new Purple1_ParticipantDTO[c.Participants.Length];
                for(int i = 0; i < participants.Length; i++)
                {
                    participants[i] = new Purple1_ParticipantDTO()
                    {
                        Type = "Participant",
                        Name = c.Participants[i].Name,
                        Surname = c.Participants[i].Surname,
                        TotalScore = c.Participants[i].TotalScore,
                        Coefs = c.Participants[i].Coefs,
                        Marks = Jaggedize(c.Participants[i].Marks),
                    };
                }
                
                
                var cDTO = new CompetitionDTO
                {
                    Type = "Competition",
                    Judges = judges,
                    Participants = participants,
                };
                SerializeDTO(cDTO);
                break;
        }
    }

    public override T DeserializePurple1<T>(string fileName)
    {
        SelectFile(fileName);
        if (typeof(T) == typeof(Purple_1.Participant))
        {
            var pDTO = DeserializeDTO<Purple1_ParticipantDTO>();
            var p = new Purple_1.Participant(pDTO.Name, pDTO.Surname);
            p.SetCriterias(pDTO.Coefs);
            for (int i = 0; i < pDTO.Marks.GetLength(0); i++)
            {
                int[] marks = new int[pDTO.Marks[i].Length];
                for(int j = 0; j < marks.Length; j++) marks[j] = pDTO.Marks[i][j];
                p.Jump(marks);
            }
            return p as T;
        }

        if (typeof(T) == typeof(Purple_1.Judge))
        {
            var jDTO = DeserializeDTO<JudgeDTO>();
            return new Purple_1.Judge(jDTO.Name, jDTO.Marks) as T;
        }

        if (typeof(T) == typeof(Purple_1.Competition))
        {
            var cDTO = DeserializeDTO<CompetitionDTO>();
            
            Purple_1.Judge[] judges = new Purple_1.Judge[cDTO.Judges.Length];
            for(int i = 0; i < judges.Length; i++)
            {
                judges[i] = new Purple_1.Judge(cDTO.Judges[i].Name, cDTO.Judges[i].Marks);
            }
            
            var c = new Purple_1.Competition(judges);
            foreach(var p in cDTO.Participants)
            {
                var newP = new Purple_1.Participant(p.Name, p.Surname);
                newP.SetCriterias(p.Coefs);
                for (int i = 0; i < p.Marks.GetLength(0); i++)
                {
                    int[] marks = new int[p.Marks[i].Length];
                    for(int j = 0; j < marks.Length; j++) marks[j] = p.Marks[i][j];
                    newP.Jump(marks);
                }
                c.Add(newP);
            }
            return c as T;
        }

        return null as T; // bebebebebe
    }

    public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
    {
        SelectFile(fileName);
        
        Purple2_ParticipantDTO[] participants = new Purple2_ParticipantDTO[jumping.Participants.Length];
        for(int i = 0; i < participants.Length; i++)
        {
            participants[i] = new Purple2_ParticipantDTO()
            {
                Type = "Purple_2.Participant",
                Name = jumping.Participants[i].Name,
                Surname = jumping.Participants[i].Surname,
                Result = jumping.Participants[i].Result,
                Marks = jumping.Participants[i].Marks,
            };
        }
        
        var dto = new SkiJumpingDTO
        {
            Type = jumping is Purple_2.JuniorSkiJumping ? "JuniorSkiJumping" : "ProSkiJumping",
            Name = jumping.Name,
            Standard = jumping.Standard,
            Participants = participants,
        };
        SerializeDTO(dto);
    }

    public override T DeserializePurple2SkiJumping<T>(string fileName)
    {
        SelectFile(fileName);
        var dto = DeserializeDTO<SkiJumpingDTO>();
        Purple_2.SkiJumping jumping = dto.Type == "JuniorSkiJumping" ? new Purple_2.JuniorSkiJumping() : new Purple_2.ProSkiJumping();
        foreach (var pd in dto.Participants)
        {
            var newP = new Purple_2.Participant(pd.Name, pd.Surname);
            newP.Jump(pd.Distance, pd.Marks, jumping.Standard);
            jumping.Add(newP);
        }
        return jumping as T;
    }

    public override void SerializePurple3Skating<T>(T skating, string fileName)
    {
        SelectFile(fileName);
        
        Purple3_ParticipantDTO[] participants = new Purple3_ParticipantDTO[skating.Participants.Length];
        for(int i = 0; i < participants.Length; i++)
        {
            participants[i] = new Purple3_ParticipantDTO()
            {
                Type = "Purple_3.Participant",
                Name = skating.Participants[i].Name,
                Surname = skating.Participants[i].Surname,
                Marks = skating.Participants[i].Marks,
                Places = skating.Participants[i].Places,
                Score = skating.Participants[i].Score,
            };
        }
        
        var dto = new SkatingDTO
        {
            Type = skating is Purple_3.IceSkating ? "IceSkating" : "FigureSkating",
            Participants = participants,
            Moods = skating.Moods,
        };
        SerializeDTO(dto);
    }

    public override T DeserializePurple3Skating<T>(string fileName)
    {
        SelectFile(fileName);
        var dto = DeserializeDTO<SkatingDTO>();
        Purple_3.Skating skating = dto.Type == "IceSkating"
            ? new Purple_3.IceSkating(dto.Moods, needModificate: false)
            : new Purple_3.FigureSkating(dto.Moods, needModificate: false);

        foreach (var pd in dto.Participants)
        {
            skating.Add(new Purple_3.Participant(pd.Name, pd.Surname));
        }
        return skating as T;
    }

    public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
    {
        SelectFile(fileName);
        
        SportsmanDTO[] sporstmen = new SportsmanDTO[participant.Sportsmen.Length];
        for(int i = 0; i < sporstmen.Length; i++)
        {
            sporstmen[i] = new SportsmanDTO()
            {
                Name = participant.Sportsmen[i].Name,
                Surname = participant.Sportsmen[i].Surname,
                Time = participant.Sportsmen[i].Time,
            };
        }
        
        var dto = new GroupDTO
        {
            Name = participant.Name,
            Sportsmen = sporstmen,
        };
        SerializeDTO(dto);
    }

    public override Purple_4.Group DeserializePurple4Group(string fileName)
    {
        SelectFile(fileName);
        var dto  = DeserializeDTO<GroupDTO>();
        var group = new Purple_4.Group(dto.Name);

        foreach (var s in dto.Sportsmen)
        {
            Purple_4.Sportsman sportsman = new Purple_4.Sportsman(s.Name, s.Surname);
            sportsman.Run(s.Time);
            group.Add(sportsman);
        }
        return group;
    }

    public override void SerializePurple5Report(Purple_5.Report group, string fileName)
    {
        SelectFile(fileName);
        
        ResearchDTO[] researches = new ResearchDTO[group.Researches.Length];
        for(int i = 0; i < researches.Length; i++)
        {
            ResponseDTO[] responses = new ResponseDTO[group.Researches[i].Responses.Length];

            for (int j = 0; j < responses.Length; j++)
            {
                responses[i] = new ResponseDTO()
                {
                    Animal = group.Researches[i].Responses[i].Animal,
                    CharacterTrait = group.Researches[i].Responses[i].CharacterTrait,
                    Concept = group.Researches[i].Responses[i].Concept,
                };
            }
            
            researches[i] = new ResearchDTO()
            {
                Name = group.Researches[i].Name,
                Responses = responses,
            };
        }
        
        var dto = new ReportDTO
        {
            Researches = researches,
        };
        SerializeDTO(dto);
    }

    public override Purple_5.Report DeserializePurple5Report(string fileName)
    {
        SelectFile(fileName);
        var dto  = DeserializeDTO<ReportDTO>();
        var report = new Purple_5.Report();
        foreach (var r in dto.Researches)
        {
            var research = new Purple_5.Research(r.Name);
            foreach (var resp in r.Responses) research.Add([resp.Animal, resp.CharacterTrait, resp.Concept]); 
            report.Add(research);
        }
        return report;
    }
}
