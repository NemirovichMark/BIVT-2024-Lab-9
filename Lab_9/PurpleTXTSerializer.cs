namespace Lab_9;

using Lab_7;
using Newtonsoft.Json;

public class PurpleTXTSerializer : PurpleSerializer
{
    public override string Extension => "txt";
    
    private void WriteDict(Dictionary<string, string> dict)
    {
        using (var writer = new StreamWriter(FilePath))
        {
            foreach (var pair in dict) writer.WriteLine($"{pair.Key}={pair.Value}");
        }
    }

    private Dictionary<string, string> FileToDict()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        foreach (var line in File.ReadAllLines(FilePath))
        {
            int idx = line.IndexOf('=');
            if (idx <= 0) continue;
            dict[line.Substring(0, idx)] = line.Substring(idx + 1);;
        }
        return dict;
    }

    private string ArrayToString<T>(T[] arr) => string.Join(";", arr.Select(x => x?.ToString()));
    private T[] StringToArray<T>(string s, Func<string, T> parser)
        => string.IsNullOrEmpty(s) ? Array.Empty<T>() : s.Split(';').Select(parser).ToArray();

    private string MatrixToString<T>(T[,] m)
    {
        int rows = m.GetLength(0), cols = m.GetLength(1);
        string[] lines = new string[rows];
        for (int i = 0; i < rows; i++)
        {
            T[] row = new T[cols];
            for(int j = 0; j < cols; j++) row[j] = m[i, j];
            lines[i] = string.Join(";", row);
        }
        return string.Join("&", lines);
    }
    private T[,] StringToMatrix<T>(string s, Func<string, T> parser)
    {
        if (string.IsNullOrEmpty(s)) return new T[0, 0];
        string[] rows = s.Split('&');
        var matrix = new T[rows.Length, rows[0].Split(';').Length];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            string[] row = rows[i].Split(';');
            for (int j = 0; j < matrix.GetLength(1); j++) matrix[i, j] = parser(row[j]);
        }
        return matrix;
    }

    public override void SerializePurple1<T>(T obj, string fileName)
    {
        SelectFile(fileName);
        var dict = new Dictionary<string, string>();
        switch (obj)
        {
            case Purple_1.Participant p:
                dict["Type"] = "Participant";
                dict["Name"] = p.Name;
                dict["Surname"] = p.Surname;
                dict["TotalScore"] = p.TotalScore.ToString();
                dict["Coefs"] = ArrayToString(p.Coefs);
                dict["Marks"] = MatrixToString(p.Marks);
                break;
            case Purple_1.Judge j:
                dict["Type"] = "Judge";
                dict["Name"] = j.Name;
                dict["Marks"] = ArrayToString(j.Marks);
                break;
            case Purple_1.Competition c:
                dict["Type"] = "Competition";
                dict["JudgesCount"] = c.Judges.Length.ToString();
                for (int i = 0; i < c.Judges.Length; i++)
                {
                    var judge = c.Judges[i];
                    dict[$"Judge_{i}_Name"] = judge.Name;
                    dict[$"Judge_{i}_Marks"] = ArrayToString(judge.Marks);
                }
                dict["ParticipantsCount"] = c.Participants.Length.ToString();
                for (int k = 0; k < c.Participants.Length; k++)
                {
                    var participant = c.Participants[k];
                    dict[$"Part_{k}_Name"] = participant.Name;
                    dict[$"Part_{k}_Surname"] = participant.Surname;
                    dict[$"Part_{k}_Coefs"] = ArrayToString(participant.Coefs);
                    dict[$"Part_{k}_Marks"] = MatrixToString(participant.Marks);
                }
                break;
        }
        WriteDict(dict);
    }

    public override T DeserializePurple1<T>(string fileName)
    {
        SelectFile(fileName);
        var d = FileToDict();
        switch (d["Type"])
        {
            case "Participant":
                var name = d["Name"];
                var surname = d["Surname"];
                var coefs = StringToArray(d["Coefs"], double.Parse);
                var marks = StringToMatrix(d["Marks"], int.Parse);
                var p = new Purple_1.Participant(name, surname);
                p.SetCriterias(coefs);
                for (int i = 0; i < marks.GetLength(0); i++)
                {
                    var marksRow = new int[marks.GetLength(1)];
                    for (int j = 0; j < marksRow.Length; j++) marksRow[j] = marks[i, j];
                    p.Jump(marksRow);
                }
                return p as T;

            case "Judge":
                var jName = d["Name"];
                var jMarks = StringToArray(d["Marks"], int.Parse);
                var judge = new Purple_1.Judge(jName, jMarks);
                return judge as T;

            case "Competition":
                int judgesCount = int.Parse(d["JudgesCount"]);
                var judges = new Purple_1.Judge[judgesCount];
                for (int i = 0; i < judgesCount; i++)
                {
                    var judgeName = d[$"Judge_{i}_Name"];
                    var judgeMarks = StringToArray(d[$"Judge_{i}_Marks"], int.Parse);
                    judges[i] = new Purple_1.Judge(judgeName, judgeMarks);
                }
                var comp = new Purple_1.Competition(judges);
                int participantsCount = int.Parse(d["ParticipantsCount"]);
                for (int k = 0; k < participantsCount; k++)
                {
                    var pName = d[$"Part_{k}_Name"];
                    var pSurname = d[$"Part_{k}_Surname"];
                    var pCoefs = StringToArray(d[$"Part_{k}_Coefs"], double.Parse);
                    Console.WriteLine(string.Join(",", pCoefs));
                    var pMarks = StringToMatrix(d[$"Part_{k}_Marks"], int.Parse);
                    var newP = new Purple_1.Participant(pName, pSurname);
                    newP.SetCriterias(pCoefs);
                    for (int i = 0; i < pMarks.GetLength(0); i++)
                    {
                        var row = new int[pMarks.GetLength(1)];
                        for (int j = 0; j < row.Length; j++) row[j] = pMarks[i, j];
                        newP.Jump(row);
                    }
                    comp.Add(newP);
                }
                return comp as T;
        }

        return null as T; // bebebebebebe
    }

    public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
    {
        SelectFile(fileName);
        var dict = new Dictionary<string, string>();
        dict["Type"] = jumping is Purple_2.JuniorSkiJumping ? "JuniorSkiJumping" : "ProSkiJumping";
        dict["Name"] = jumping.Name;
        dict["Standard"] = jumping.Standard.ToString();
        var participantsCount = jumping.Participants;
        dict["ParticipantsCount"] = participantsCount.Length.ToString();
        for (int i = 0; i < participantsCount.Length; i++)
        {
            var p = participantsCount[i];
            dict[$"P_{i}_Name"] = p.Name;
            dict[$"P_{i}_Surname"] = p.Surname;
            dict[$"P_{i}_Distance"] = p.Distance.ToString();
            dict[$"P_{i}_Result"] = p.Result.ToString();
            dict[$"P_{i}_Marks"] = ArrayToString(p.Marks);
        }
        WriteDict(dict);
    }

    public override T DeserializePurple2SkiJumping<T>(string fileName)
    {
        SelectFile(fileName);
        var d = FileToDict();
        Purple_2.SkiJumping jumping = d["Type"] == "JuniorSkiJumping" ? new Purple_2.JuniorSkiJumping() : new Purple_2.ProSkiJumping();
        int participantsCount = int.Parse(d["ParticipantsCount"]);
        for (int i = 0; i < participantsCount; i++)
        {
            var pName = d[$"P_{i}_Name"];
            var pSurname = d[$"P_{i}_Surname"];
            var pDistance = int.Parse(d[$"P_{i}_Distance"]);
            var pMarks = StringToArray(d[$"P_{i}_Marks"], int.Parse);
            var newP = new Purple_2.Participant(pName, pSurname);
            newP.Jump(pDistance, pMarks, jumping.Standard);
            jumping.Add(newP);
        }
        return jumping as T;
    }

    public override void SerializePurple3Skating<T>(T skating, string fileName)
    {
        SelectFile(fileName);
        var dict = new Dictionary<string, string>();
        dict["Type"] = skating is Purple_3.IceSkating ? "IceSkating" : "FigureSkating";
        dict["Moods"] = ArrayToString(skating.Moods);
        var participants = skating.Participants;
        dict["ParticipantsCount"] = participants.Length.ToString();
        for (int i = 0; i < participants.Length; i++)
        {
            dict[$"P_{i}_Name"] = participants[i].Name;
            dict[$"P_{i}_Surname"] = participants[i].Surname;
            dict[$"P_{i}_Marks"] = ArrayToString(participants[i].Marks);
        }
        WriteDict(dict);
    }

    public override T DeserializePurple3Skating<T>(string fileName)
    {
        SelectFile(fileName);
        var d = FileToDict();
        var moods = StringToArray(d["Moods"], double.Parse);
        Purple_3.Skating skating = d["Type"] == "IceSkating" 
            ? new Purple_3.IceSkating(moods, false) : new Purple_3.FigureSkating(moods, false);
        int participantsCount = int.Parse(d["ParticipantsCount"]);
        for (int i = 0; i < participantsCount; i++)
        {
            var pName = d[$"P_{i}_Name"];
            var pSurname = d[$"P_{i}_Surname"];
            var pMarks = StringToArray(d[$"P_{i}_Marks"], double.Parse);
            
            var newP = new Purple_3.Participant(pName, pSurname);
            foreach(var mark in pMarks) newP.Evaluate(mark);
            
            skating.Add(newP);
        }
        return skating as T;
    }

    public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
    {
        SelectFile(fileName);
        var dict = new Dictionary<string, string>();
        dict["Type"] = "Group";
        dict["Name"] = participant.Name;
        var arr = participant.Sportsmen;
        dict["Count"] = arr.Length.ToString();
        for (int i = 0; i < arr.Length; i++)
        {
            var s = arr[i];
            dict[$"S_{i}_Name"] = s.Name;
            dict[$"S_{i}_Surname"] = s.Surname;
            dict[$"S_{i}_Time"] = s.Time.ToString();
        }
        WriteDict(dict);
    }

    public override Purple_4.Group DeserializePurple4Group(string fileName)
    {
        SelectFile(fileName);
        var dict = FileToDict();
        var group = new Purple_4.Group(dict["Name"]);
        int cnt = int.Parse(dict["Count"]);
        for (int i = 0; i < cnt; i++)
        {
            var sName = dict[$"S_{i}_Name"];
            var sSurname = dict[$"S_{i}_Surname"];
            var sTime = double.Parse(dict[$"S_{i}_Time"]);
            var newS = new Purple_4.Sportsman(sName, sSurname);
            newS.Run(sTime);
            group.Add(newS);
        }
        return group;
    }

    public override void SerializePurple5Report(Purple_5.Report group, string fileName)
    {
        SelectFile(fileName);
        var dict = new Dictionary<string, string>();
        dict["Type"] = "Report";
        var res = group.Researches;
        dict["RCount"] = res.Length.ToString();
        for (int i = 0; i < res.Length; i++)
        {
            var r = res[i];
            dict[$"R_{i}_Name"] = r.Name;
            var reps = r.Responses;
            dict[$"R_{i}_RespCount"] = reps.Length.ToString();
            for (int j = 0; j < reps.Length; j++)
            {
                var rp = reps[j];
                dict[$"R_{i}_A{j}"] = (rp.Animal == null) ? "null" : rp.Animal;
                dict[$"R_{i}_T{j}"] = (rp.CharacterTrait == null) ? "null" : rp.CharacterTrait;
                dict[$"R_{i}_C{j}"] = (rp.Concept == null) ? "null" : rp.Concept;
            }
        }
        WriteDict(dict);
    }

    public override Purple_5.Report DeserializePurple5Report(string fileName)
    {
        SelectFile(fileName);
        var dict = FileToDict();
        var report = new Purple_5.Report();
        int respCount = int.Parse(dict["RCount"]);
        for (int i = 0; i < respCount; i++)
        {
            var name = dict[$"R_{i}_Name"];
            var research = new Purple_5.Research(name);
            int cnt = int.Parse(dict[$"R_{i}_RespCount"]);
            for (int j = 0; j < cnt; j++)
            {
                var animal = dict[$"R_{i}_A{j}"] == "null" ? null : dict[$"R_{i}_A{j}"];
                var trait = dict[$"R_{i}_T{j}"] == "null" ? null : dict[$"R_{i}_T{j}"];
                var concept = dict[$"R_{i}_C{j}"] == "null" ? null : dict[$"R_{i}_C{j}"];
                research.Add(new[] { animal, trait, concept });
            }
            report.AddResearch(research);
        }
        return report;
    }
}