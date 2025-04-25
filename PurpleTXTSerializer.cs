using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

using Lab_7;
using static Lab_7.Purple_2;

using Lab_9;

public class PurpleTXTSerializer : PurpleSerializer{
    public override string Extension => "txt";


public override void SerializePurple1<T>(T obj, string fileName)
{
    if (obj is Purple_1.Participant participant)  SerializeParticipant(participant, fileName);

    else if (obj is Purple_1.Judge judge) SerializeJudge(judge, fileName);

    else if (obj is Purple_1.Competition competition) SerializeCompetition(competition, fileName);
}

private void SerializeParticipant(Purple_1.Participant participant, string fileName)
{
    SelectFile(fileName);
    using (var writer = new StreamWriter(FilePath))
    {
        writer.WriteLine($"Name: {participant.Name}");
        writer.WriteLine($"Surname: {participant.Surname}");
        writer.WriteLine($"Coefs: {string.Join(",", participant.Coefs.Select(c => c.ToString(CultureInfo.InvariantCulture)))}");

        var marks = participant.Marks;
        var flat = new List<int>();
        for (int i = 0; i < marks.GetLength(0); i++)
            for (int j = 0; j < marks.GetLength(1); j++)
                flat.Add(marks[i, j]);

        writer.WriteLine($"Marks: {string.Join(",", flat)}");
    }
}

private void SerializeJudge(Purple_1.Judge judge, string fileName)
{
    SelectFile(fileName);
    using (var writer = new StreamWriter(FilePath))
    {
        writer.WriteLine($"Name: {judge.Name}");
        writer.WriteLine($"Marks: {string.Join(",", judge.Marks)}");
    }
}


private void SerializeCompetition(Purple_1.Competition comp, string fileName)
{
    var folder = Path.Combine(FolderPath, $"{fileName}_data");
    if (!Directory.Exists(folder))
        Directory.CreateDirectory(folder);

    var filePath = Path.Combine(FolderPath, $"{fileName}.{Extension}");
    using (var writer = new StreamWriter(filePath))
    {
        writer.WriteLine($"JudgesCount: {comp.Judges.Length}");
        for (int i = 0; i < comp.Judges.Length; i++)
        {
            string judgeFile = $"{fileName}_judge_{i}";
            writer.WriteLine($"Judge_{i}: {judgeFile}");
            SerializeJudge(comp.Judges[i], Path.Combine(folder, $"{judgeFile}.{Extension}"));
        }

        writer.WriteLine($"ParticipantsCount: {comp.Participants.Length}");
        for (int i = 0; i < comp.Participants.Length; i++)
        {
            string partFile = $"{fileName}_participant_{i}";
            writer.WriteLine($"Participant_{i}: {partFile}");
            SerializeParticipant(comp.Participants[i], Path.Combine(folder, $"{partFile}.{Extension}"));
        }
    }
}


public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
{
    SelectFile(fileName);

    using (var writer = new StreamWriter(FilePath))
    {
        writer.WriteLine($"Name: {((SkiJumping)(object)jumping).Name}");
        writer.WriteLine($"Standard: {((SkiJumping)(object)jumping).Standard}");

        writer.WriteLine($"Participants count: {((SkiJumping)(object)jumping).Participants.Length}");
        foreach (var participant in ((SkiJumping)(object)jumping).Participants)
        {
            writer.WriteLine($"{participant.Name} {participant.Surname} Result: {participant.Result}");
            writer.WriteLine($"Marks: {string.Join(", ", participant.Marks)}");
        }
    }
}

    public override void SerializePurple3Skating<T>(T skating, string fileName){}
    public override void SerializePurple4Group(Purple_4.Group participant, string fileName){}
    public override void SerializePurple5Report(Purple_5.Report group, string fileName){}






public override T DeserializePurple1<T>(string fileName)
{
    if (typeof(T) == typeof(Purple_1.Participant)) return DeserializeParticipant(fileName) as T;

    if (typeof(T) == typeof(Purple_1.Judge)) return  DeserializeJudge(fileName) as T;
    
    else  return DeserializeCompetition(fileName) as T;

}

private Purple_1.Participant DeserializeParticipant(string fileName)
{
    SelectFile(fileName);
    var lines = File.ReadAllLines(FilePath).ToDictionary(
        l => l.Split(new[] { ": " }, 2, StringSplitOptions.None)[0],
        l => l.Split(new[] { ": " }, 2, StringSplitOptions.None)[1]
    );

    var participant = new Purple_1.Participant(lines["Name"], lines["Surname"]);

    var coefs = lines["Coefs"].Split(',').Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();
    participant.SetCriterias(coefs);

    var flatMarks = lines["Marks"].Split(',').Select(int.Parse).ToArray();
    for (int i = 0; i < 4; i++)
    {
        int[] oneJump = new int[7];
        Array.Copy(flatMarks, i * 7, oneJump, 0, 7);
        participant.Jump(oneJump);
    }

    return participant;
}

private Purple_1.Judge DeserializeJudge(string fileName)
{
    SelectFile(fileName);
    var lines = File.ReadAllLines(FilePath).ToDictionary(
        l => l.Split(new[] { ": " }, 2, StringSplitOptions.None)[0],
        l => l.Split(new[] { ": " }, 2, StringSplitOptions.None)[1]
    );

    string name = lines["Name"];
    int[] marks = lines["Marks"].Split(',').Select(int.Parse).ToArray();

    return new Purple_1.Judge(name, marks);
}


private Purple_1.Competition DeserializeCompetition(string fileName)
{
    var folder = Path.Combine(FolderPath, $"{fileName}_data");
    SelectFile(fileName);

    var lines = File.ReadAllLines(FilePath)
        .Select(l => l.Split(new[] { ": " }, 2, StringSplitOptions.None))
        .Where(p => p.Length == 2)
        .ToDictionary(p => p[0], p => p[1]);

    int judgeCount = int.Parse(lines["JudgesCount"]);
    var judges = new Purple_1.Judge[judgeCount];
    for (int i = 0; i < judgeCount; i++)
    {
        string judgeFile = Path.Combine(folder, $"{lines[$"Judge_{i}"]}.{Extension}");
        judges[i] = DeserializeJudge(judgeFile);
    }

    var competition = new Purple_1.Competition(judges);

    int partCount = int.Parse(lines["ParticipantsCount"]);
    for (int i = 0; i < partCount; i++)
    {
        string partFile = Path.Combine(folder, $"{lines[$"Participant_{i}"]}.{Extension}");
        var participant = DeserializeParticipant(partFile);
        competition.Add(participant);
    }

    return competition;
}


public override T DeserializePurple2SkiJumping<T>(string fileName)
{

    SelectFile(fileName);
    string[] lines = File.ReadAllLines(FilePath);
    
    string name = null;
    int standard = 0;
    List<Participant> participants = new List<Participant>();

    foreach (var line in lines)
    {
        if (line.StartsWith("Name:"))
        {
            name = line.Substring(5).Trim();
        }

        else if (line.StartsWith("Standard:"))
        {
            standard = int.Parse(line.Substring(9).Trim());
        }

        else if (line.Contains("Result:"))
        {
            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string participantName = parts[0];
            string participantSurname = parts[1];
            int result = int.Parse(parts[3]);
            var marksLine = lines[Array.IndexOf(lines, line) + 1]; 
            int[] marks = marksLine.Substring(7) 
                                 .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(m => int.Parse(m.Trim()))
                                 .ToArray();

            var participant = new Participant(participantName, participantSurname);
            participant.Jump(0, marks, standard);

            participants.Add(participant);
        }
    }

    if (name == "100m")
    {
        var competition = new JuniorSkiJumping();
        competition.Add(participants.ToArray());
        return (T)(object)competition;
    }
    else
    {
        var competition = new ProSkiJumping();
        competition.Add(participants.ToArray());
        return (T)(object)competition;
    }
}

    public override T DeserializePurple3Skating<T>(string fileName) { return null;}
    public override Purple_4.Group DeserializePurple4Group(string fileName){ return null;}
    public override Purple_5.Report DeserializePurple5Report(string fileName){ return null;}

}