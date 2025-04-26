namespace Lab_9;

using Lab_7;
using Newtonsoft.Json;

public class PurpleJSONSerializer : PurpleSerializer {
    
    public override string Extension => "json";
    
    public override void SerializePurple1<T>(T obj, string fileName) {
        if (!(obj is Purple_1.Participant | obj is Purple_1.Competition | obj is Purple_1.Judge)) return;
        
        SelectFile(fileName);
        string json = String.Empty;

        switch (obj) {
            case Purple_1.Participant participant:
                json = JsonConvert.SerializeObject(participant, Formatting.Indented);
                break;

            case Purple_1.Judge judge:
                json = JsonConvert.SerializeObject(judge, Formatting.Indented);
                break;

            case Purple_1.Competition competition:
                json = JsonConvert.SerializeObject(competition, Formatting.Indented);
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
        var obj = JsonConvert.DeserializeObject<T>(json);
        if (obj is Purple_1.Competition comp) {
            var competition = new Purple_1.Competition(comp.Judges);
            foreach(var p in comp.Participants)
            {
                var newP = new Purple_1.Participant(p.Name, p.Surname);
                newP.SetCriterias(p.Coefs);
                for (int i = 0; i < p.Marks.GetLength(0); i++)
                {
                    int[] marks = new int[p.Marks.GetLength(1)];
                    for(int j = 0; j < marks.Length; j++) marks[j] = p.Marks[i, j];
                    newP.Jump(marks);
                }
                competition.Add(newP);
            }
            return competition as T;
        }
        if (obj is Purple_1.Participant part)
        {
            var newP = new Purple_1.Participant(part.Name, part.Surname);
            newP.SetCriterias(part.Coefs);
            for (int i = 0; i < part.Marks.GetLength(0); i++)
            {
                int[] marks = new int[part.Marks.GetLength(1)];
                for(int j = 0; j < marks.Length; j++) marks[j] = part.Marks[i, j];
                newP.Jump(marks);
            }
            
            return newP as T;
        }
        return obj as T;
    }
    
    public override T DeserializePurple2SkiJumping<T>(string fileName) {
        SelectFile(fileName);
        string json = File.ReadAllText(FilePath);
        T obj = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
        return obj;
    }
    
    public override T DeserializePurple3Skating<T>(string fileName) {
        SelectFile(fileName);
        string json = File.ReadAllText(FilePath);
        T obj = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
        switch (obj)
        {
            case Purple_3.IceSkating iceSkating:
                var ice = new Purple_3.IceSkating(obj.Moods, false);
                ice.Add(iceSkating.Participants);
                return ice as T;
            case Purple_3.FigureSkating figureSkating:
                var figure = new Purple_3.FigureSkating(obj.Moods, false);
                figure.Add(figureSkating.Participants);
                return figure as T;
        }
        return obj;
    }
    
    public override Purple_4.Group DeserializePurple4Group(string fileName) {
        SelectFile(fileName);
        string json = File.ReadAllText(FilePath);
        Purple_4.Group obj = JsonConvert.DeserializeObject<Purple_4.Group>(json);
        return obj;
    }
    
    public override Purple_5.Report DeserializePurple5Report(string fileName) {
        SelectFile(fileName);
        string json = File.ReadAllText(FilePath);
        Purple_5.Report obj = JsonConvert.DeserializeObject<Purple_5.Report>(json);
        return obj;
    }
}

    
     