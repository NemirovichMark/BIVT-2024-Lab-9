using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;

namespace Lab_9;

using Lab_7;
using Newtonsoft.Json;

public class PurpleJSONSerializer : PurpleSerializer {
    
    public override string Extension => "json";
    
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
    
    public override void SerializePurple1<T>(T obj, string fileName) {
        if (!(obj is Purple_1.Participant | obj is Purple_1.Competition | obj is Purple_1.Judge)) return;
        
        SelectFile(fileName);
        string json = String.Empty;

        switch (obj) {
            case Purple_1.Participant participant:
                json = JsonConvert.SerializeObject(participant, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                break;

            case Purple_1.Judge judge:
                json = JsonConvert.SerializeObject(judge, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                break;

            case Purple_1.Competition competition:
                json = JsonConvert.SerializeObject(competition, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                break;
        }
        File.WriteAllText(FilePath, json);
    }
    
    public override void SerializePurple2SkiJumping<T>(T jumping, string fileName) {
        if (jumping == null) return;
        SelectFile(fileName);
        string json = JsonConvert.SerializeObject(jumping, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
        File.WriteAllText(FilePath, json);
    }
    
    public override void SerializePurple3Skating<T>(T skating, string fileName)
    {
        if (skating == null) return;
        SelectFile(fileName);
        string json = JsonConvert.SerializeObject(skating, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
        File.WriteAllText(FilePath, json);
    }
    
    public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
    {
        if (participant == null) return;
        SelectFile(fileName);
        string json = JsonConvert.SerializeObject(participant, Formatting.Indented);
        File.WriteAllText(FilePath, json);
    }
    
    public override void SerializePurple5Report(Purple_5.Report group, string fileName)
    {
        if (group == null) return;
        SelectFile(fileName);
        string json = JsonConvert.SerializeObject(group, Formatting.Indented);
        File.WriteAllText(FilePath, json);
    }
    
    public override T DeserializePurple1<T>(string fileName) {
        SelectFile(fileName);
        string json = File.ReadAllText(FilePath);
        var obj = JObject.Parse(json);
        switch ((string)obj["$type"])
        {
            case "Lab_7.Purple_1+Judge, Lab_7":
                return new Purple_1.Judge(
                    (string)obj["Name"],
                    obj["Marks"].Select(x => (int)x).ToArray()) as T;
            case "Lab_7.Purple_1+Participant, Lab_7":
                var newP = new Purple_1.Participant((string)obj["Name"], (string)obj["Surname"]);
                newP.SetCriterias(obj["Coefs"].Select(x => (double)x).ToArray());
                var allMarks = Rectangularize(((JArray)obj["Marks"]).Select(row => row.Values<int>().ToArray()).ToArray());
                for (int i = 0; i < allMarks.GetLength(0); i++)
                {
                    int[] marks = new int[allMarks.GetLength(1)];
                    for(int j = 0; j < marks.Length; j++) marks[j] = allMarks[i, j];
                    newP.Jump(marks);
                }
                return newP as T;
            case "Lab_7.Purple_1+Competition, Lab_7":
                Purple_1.Judge[] judges = ((JArray)obj["Judges"])
                    .ToObject<JudgeDTO[]>()
                    .Select(dto => new Purple_1.Judge(dto.Name, dto.Marks))
                    .ToArray();
                Purple_1.Participant[] participants = ((JArray)obj["Participants"])
                    .ToObject<Purple1_ParticipantDTO[]>()
                    .Select(dto =>
                    {
                        var p = new Purple_1.Participant(dto.Name, dto.Surname);
                        p.SetCriterias(dto.Coefs);
                        for(int i = 0; i < dto.Marks.Length; i++) p.Jump(dto.Marks[i]);
                        return p;
                    })
                    .ToArray();
                var competition = new Purple_1.Competition(judges);
                foreach(var p in participants) competition.Add(p);
                return competition as T;
        }
        return null as T; // bebebebe
    }
    
    public override T DeserializePurple2SkiJumping<T>(string fileName) {
        SelectFile(fileName);
        string json = File.ReadAllText(FilePath);
        var obj = JObject.Parse(json);
        
        
        var participants = ((JArray)obj["Participants"]).ToObject<Purple2_ParticipantDTO[]>();
        
        Purple_2.SkiJumping jumping = (string)obj["$type"] == "Lab_7.Purple_2+JuniorSkiJumping, Lab_7" ? new Purple_2.JuniorSkiJumping() : new Purple_2.ProSkiJumping();
        foreach (var pd in participants)
        {
            var newP = new Purple_2.Participant(pd.Name, pd.Surname);
            newP.Jump(pd.Distance, pd.Marks, jumping.Standard);
            jumping.Add(newP);
        }
        return jumping as T;
    }
    
    public override T DeserializePurple3Skating<T>(string fileName) {
        SelectFile(fileName);
        string json = File.ReadAllText(FilePath);
        var obj = JObject.Parse(json);
        Purple_3.Participant[] participants = ((JArray)obj["Participants"])
            .ToObject<Purple3_ParticipantDTO[]>()
            .Select(dto => new Purple_3.Participant(dto.Name, dto.Surname))
            .ToArray();
        
        switch ((string)obj["$type"])
        {
            case "Lab_7.Purple_3+IceSkating, Lab_7":
                var ice = new Purple_3.IceSkating(obj["Moods"].Select(x => (double)x).ToArray(), false);
                ice.Add(participants);
                return ice as T;
            case "Lab_7.Purple_3+FigureSkating, Lab_7":
                var figure = new Purple_3.FigureSkating(obj["Moods"].Select(x => (double)x).ToArray(), false);
                figure.Add(participants);
                return figure as T;
        }
        return null as T; // bebebebebebebebebebbe (bebebe)
    }
    
    public override Purple_4.Group DeserializePurple4Group(string fileName) {
        SelectFile(fileName);
        string json = File.ReadAllText(FilePath);
        var obj = JObject.Parse(json);
        Purple_4.Sportsman[] sporstmen = ((JArray)obj["Sportsmen"])
            .ToObject<SportsmanDTO[]>()
            .Select(dto =>
            {
                var sportsman = new Purple_4.Sportsman(dto.Name, dto.Surname);
                sportsman.Run(dto.Time);
                return sportsman;
            })
            .ToArray();

        Purple_4.Group group = new Purple_4.Group((string)obj["Name"]);
        group.Add(sporstmen);
        return group;
    }
    
    public override Purple_5.Report DeserializePurple5Report(string fileName) {
        SelectFile(fileName);
        string json = File.ReadAllText(FilePath);
        var obj = JObject.Parse(json);
        var report = new Purple_5.Report();
        
        var researches = ((JArray)obj["Researches"])
            .ToObject<ResearchDTO[]>()
            .Select(dto => new Purple_5.Research(dto.Name))
            .ToArray();
        
        foreach (var r in researches)
        {
            var research = new Purple_5.Research(r.Name);
            foreach (var resp in r.Responses) research.Add([resp.Animal, resp.CharacterTrait, resp.Concept]); 
            report.AddResearch(research);
        }
        return report;
    }
}

    
     