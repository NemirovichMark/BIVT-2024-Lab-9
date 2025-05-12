using System;
using System.Text;
using Lab_7;
namespace Lab_9;



public class BlueTXTSerializer : BlueSerializer
{
    public override string Extension => "txt";

    public override void SerializeBlue1Response(Blue_1.Response participant, string fileName) 
    {
        if (participant == null || string.IsNullOrEmpty(fileName))
            return;
         
        SelectFile(fileName);

        StringBuilder data = new StringBuilder();
        data.AppendLine($"Name={participant.Name}");
        if (participant is Blue_1.HumanResponse human)
            data.AppendLine($"Surname={human.Surname}");
        data.AppendLine($"Votes={participant.Votes}");
        File.WriteAllText(fileName, data.ToString());
    }
    public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName) 
    {
        if (participant == null || string.IsNullOrEmpty(fileName))
            return;
         
        SelectFile(fileName);
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            writer.WriteLine($"Type={participant.GetType().Name}");
            writer.WriteLine($"Name={participant.Name}");
            writer.WriteLine($"Bank={participant.Bank}");
            foreach(var part in participant.Participants)
            {
                // у каждого своя матрциа интов 2x5 [1,2,...],[1,2,...]
                string marks = "[";
                for (int i = 0; i<5; i++)
                    marks+=$"{part.Marks[0,i]},";
                marks = marks.Remove(marks.Length-1);
                marks+="],[";
                for (int i = 0; i<5; i++)
                    marks+=$"{part.Marks[1,i]},";
                marks = marks.Remove(marks.Length-1);
                marks+="]";
                writer.WriteLine($"{part.Name}|{part.Surname}|{marks}");
            }
        }
    }
    public override void SerializeBlue3Participant<T>(T student, string fileName) 
    {
        if (student == null || string.IsNullOrEmpty(fileName))
            return;
        
        SelectFile(fileName);
        StringBuilder data = new StringBuilder();
        data.AppendLine($"Name={student.Name}");
        data.AppendLine($"Surname={student.Surname}");
        data.AppendLine($"Type={student.GetType().Name}");
        data.AppendLine($"Count={student.Penalties.Length}");
        data.AppendLine($"{string.Join(',',student.Penalties)}"); 

        File.WriteAllText(fileName, data.ToString());
    }
    public override void SerializeBlue4Group(Blue_4.Group participant, string fileName) 
    {
        if (participant == null || string.IsNullOrEmpty(fileName))
            return;
        
        SelectFile(fileName);
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            writer.WriteLine($"Name:{participant.Name}");
            writer.WriteLine($"ManTeamsCount:{participant.ManTeams.Length}");
            foreach (var team in participant.ManTeams)
            {
                if (team != null)
                    writer.WriteLine($"{team.Name}|{string.Join(',',team.Scores)}");
            }
            writer.WriteLine($"WomanTeamsCount:{participant.WomanTeams.Length}");
            foreach (var team in participant.WomanTeams)
            {
                if (team != null)
                    writer.WriteLine($"{team.Name}|{string.Join(',',team.Scores)}");
            }
        }
    }
    public override void SerializeBlue5Team<T>(T group, string fileName) 
    {
        if (group == null || string.IsNullOrEmpty(fileName))
            return;
        
        SelectFile(fileName);
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            writer.WriteLine($"Name={group.Name}");
            writer.WriteLine($"Type={group.GetType().Name}");
            var validSportsmen = group.Sportsmen.Where(s => s != null).ToArray();
            writer.WriteLine($"Count={validSportsmen.Length}");
        
            foreach(var sport in validSportsmen)
            {
                writer.WriteLine($"{sport.Name}|{sport.Surname}|{sport.Place}");
            }
        }
    }


    public override Blue_1.Response DeserializeBlue1Response(string fileName) 
    {
        // typeof() response humanresponse - Name: (Surname:) Votes:
        if (string.IsNullOrEmpty(fileName))
            return default(Blue_1.Response);
        
        string[] lines = File.ReadAllLines(fileName);
        Dictionary<string,string> responseParsing = new Dictionary<string, string>();
        foreach (var line in lines)
        {
            if (line.Contains('='))
            {
                string[] parts = line.Split('=');
                responseParsing[parts[0].Trim()] = parts[1].Trim();
            }
        }
        if (responseParsing.ContainsKey("Surname"))
            return new Blue_1.HumanResponse(responseParsing["Name"], responseParsing["Surname"],int.Parse(responseParsing["Votes"]));
        else
            return new Blue_1.Response(responseParsing["Name"],int.Parse(responseParsing["Votes"]));
    }
    public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName) 
    {
        // participant abstr 3m 5m - name bank part[]
        if (string.IsNullOrEmpty(fileName))
            return default(Blue_2.WaterJump);

        string[] lines = File.ReadAllLines(fileName);
        // Console.WriteLine(lines.Length);
        Dictionary<string,string> parsing = new Dictionary<string, string>(); 
        List<Blue_2.Participant> participants = new List<Blue_2.Participant>();
        foreach(string line in lines)
        {
            if (line.Contains("="))
            {
                string[] parts = line.Split("=");
                parsing[parts[0].Trim()] = parts[1].Trim(); 
            }
            else
            {
                string[] inf = line.Split("|");
                if (inf.Length != 3) continue;
                var part = new Blue_2.Participant(inf[0], inf[1]);
                if (!string.IsNullOrEmpty(inf[2]))
                {
                    string marksStr = inf[2].Trim();
                    marksStr = marksStr.Substring(1, marksStr.Length - 2);
                    string[] jumps = marksStr.Split("],[");
                    for (int i = 0; i < jumps.Length; i++)
                    {   
                        int[] marks = Array.ConvertAll(jumps[i].Split(",", StringSplitOptions.RemoveEmptyEntries), int.Parse); 
                        part.Jump(marks);
                    }
                }
                participants.Add(part);
            }     
        }

        if (parsing["Type"] == "WaterJump3m")
        {
            var res = new Blue_2.WaterJump3m(parsing["Name"], int.Parse(parsing["Bank"]));
            res.Add(participants.ToArray());
            return res;
        }
        else
        {
            var res = new Blue_2.WaterJump5m(parsing["Name"], int.Parse(parsing["Bank"]));
            res.Add(participants.ToArray());
            return res;
        }
    }
    public override T DeserializeBlue3Participant<T>(string fileName) 
    {
        if (string.IsNullOrEmpty(fileName))
            return default(T);
        
        string[] lines = File.ReadAllLines(fileName);
        Dictionary<string,string> parsing = new Dictionary<string, string>();
        int[] penalties = new int[0];
        foreach (string line in lines)
        {
            if (line.Contains('='))
            {
                string[] parts = line.Split('=');
                parsing[parts[0].Trim()] = parts[1].Trim();
            }
            else // penalties
            {
                penalties = new int[int.Parse(parsing["Count"])];
                string[] pens = line.Split(',');
                for (int i = 0; i<penalties.Length; i++)
                    penalties[i] = int.Parse(pens[i]);
            }
        }
        Blue_3.Participant participant;
        if (parsing["Type"] == "BasketballPlayer")
            participant = new Blue_3.BasketballPlayer(parsing["Name"], parsing["Surname"]);
        else if (parsing["Type"] == "HockeyPlayer")
            participant = new Blue_3.HockeyPlayer(parsing["Name"], parsing["Surname"]);
        else
            participant = new Blue_3.Participant(parsing["Name"], parsing["Surname"]);
        foreach (var penalty in penalties)
            participant.PlayMatch(penalty);
        return (T)(object)participant;
    }
    public override Blue_4.Group DeserializeBlue4Group(string fileName) 
    {
        // name manteam womanteam ; team - name scores[]
        if (string.IsNullOrEmpty(fileName))
            return default(Blue_4.Group);

        string[] lines = File.ReadAllLines(fileName);
        Dictionary<string,string> parsing = new Dictionary<string, string>();

        Blue_4.ManTeam[] men = new Blue_4.ManTeam[0]; Blue_4.WomanTeam[] women = new Blue_4.WomanTeam[0];
        string name = "";
        bool wasMan = false, wasWoman = false; int ind = 0;
        foreach(string line in lines)
        {
            if (line.StartsWith("Name:"))
                name = line.Split(':')[1].Trim();
            else if (line.StartsWith("ManTeamsCount:"))
            {
                wasMan = true;
                wasWoman = false;
                men = new Blue_4.ManTeam[int.Parse(line.Split(':')[1])];
                ind = 0;
            }
            else if (line.StartsWith("WomanTeamsCount:"))
            {
                wasWoman = true;
                wasMan = false;
                women = new Blue_4.WomanTeam[int.Parse(line.Split(':')[1])];
                ind = 0;
            }
            else if (wasMan)
            {
                string[] data = line.Split('|');
                Blue_4.ManTeam man = new Blue_4.ManTeam(data[0].Trim());
                foreach (string score in data[1].Split(','))
                    man.PlayMatch(int.Parse(score.Trim()));
                men[ind++] = man;
            }
            else if (wasWoman)
            {
                string[] data = line.Split('|');
                Blue_4.WomanTeam woman = new Blue_4.WomanTeam(data[0].Trim());
                foreach (string score in data[1].Split(','))
                    woman.PlayMatch(int.Parse(score.Trim()));
                women[ind++] = woman;
            }
        }
        Blue_4.Group res = new Blue_4.Group(name);
        res.Add(men);
        res.Add(women);
        return res;
    }
    public override T DeserializeBlue5Team<T>(string fileName) 
    {
        // abstr manteam womanteam - name sportsman[], sportsmen - name surname place
        if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
            return default(T);
        
        string[] lines = File.ReadAllLines(fileName);
        Dictionary<string,string> parsing = new Dictionary<string, string>();
        foreach (string line in lines)
        {
            if (line.Contains('='))
            {
                string[] parts = line.Split('=');
                parsing[parts[0].Trim()] = parts[1].Trim();
            }
        }
        Blue_5.Team team = (parsing["Type"] == "ManTeam") ? new Blue_5.ManTeam(parsing["Name"]) : new Blue_5.WomanTeam(parsing["Name"]);
        foreach (string line in lines)
        {
            if (line.Contains('=') || string.IsNullOrEmpty(line)) continue;
            string[] inf = line.Split('|');
            Blue_5.Sportsman sportsman = new Blue_5.Sportsman(inf[0].Trim(), inf[1].Trim());
            sportsman.SetPlace(int.Parse(inf[2].Trim()));
            team.Add(sportsman);
        }
        return (T)(object)team;
    }
}   