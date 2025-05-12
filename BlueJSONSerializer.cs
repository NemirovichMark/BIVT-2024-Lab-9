using System;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using Lab_7;

namespace Lab_9;

public class BlueJSONSerializer : BlueSerializer
{
    public override string Extension => "json";
    
    
    public override void SerializeBlue1Response(Blue_1.Response participant, string fileName) 
    {
        if (participant == null || string.IsNullOrEmpty(fileName))
            return;
        
        SelectFile(fileName);
        var ex = new
        {
            Type = participant.GetType().Name, 
            participant.Name,
            participant.Votes,
            Surname = (participant as Blue_1.HumanResponse)?.Surname
        };
        string text = JsonConvert.SerializeObject(ex);
        File.WriteAllText(fileName, text);
    }
    
    private static int[][] ToJaggedArray(int[,] arr)
    {
        if (arr == null) 
            return null;
        int[][] newArr = new int[arr.GetLength(0)][];
        for (int i = 0; i<newArr.Length; i++)
        {
            newArr[i] = new int[arr.GetLength(1)];
            for (int j = 0; j < newArr[i].Length; j++)
                newArr[i][j] = arr[i, j];
        }
        return newArr;
    }
    public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName) 
    {
        if (participant == null || string.IsNullOrEmpty(fileName))
            return;

        SelectFile(fileName);
        var ex = new
        {
            Type = participant.GetType().Name, 
            participant.Name,
            participant.Bank,
            Participants = participant.Participants.Select(p => new
            {
                p.Name,
                p.Surname,
                Marks = ToJaggedArray(p.Marks) // int[,] -> int[][]
            }).ToArray()
        };
        string text = JsonConvert.SerializeObject(ex);
        File.WriteAllText(fileName, text);
    }
    public override void SerializeBlue3Participant<T>(T student, string fileName) 
    {
        if (student == null || string.IsNullOrEmpty(fileName))
            return;

        SelectFile(fileName);
        var ex = new
        {
            Type = student.GetType().Name, 
            student.Name,
            student.Surname,
            student.Penalties
        };
        string text = JsonConvert.SerializeObject(ex);
        File.WriteAllText(fileName, text);
    }
    public override void SerializeBlue4Group(Blue_4.Group participant, string fileName) 
    {
        if (participant == null || string.IsNullOrEmpty(fileName))
            return;
        
        SelectFile(fileName);
        var ex = new 
        {
            Type = participant.GetType().Name,
            participant.Name,
            Women = participant.WomanTeams.Where(t => t != null).Select(p => new  //null reference exception
            {
                p.Name,
                p.Scores
            }).ToArray(),
            Men = participant.ManTeams.Where(t => t != null).Select(p => new
            {
                p.Name,
                p.Scores
            })
        };
        string text = JsonConvert.SerializeObject(ex);
        File.WriteAllText(fileName, text);
    }
    public override void SerializeBlue5Team<T>(T group, string fileName) 
    {
        if (group == null || string.IsNullOrEmpty(fileName))
            return;

        SelectFile(fileName);
        var ex = new
        {
            Type = group.GetType().Name,
            group.Name,
            Sportsmen = group.Sportsmen.Where(t => t != null).Select(p => new
            {
                p.Name,
                p.Surname,
                p.Place 
            }).ToArray()
        };
        string text = JsonConvert.SerializeObject(ex);
        File.WriteAllText(fileName,text);
    }


    public override Blue_1.Response DeserializeBlue1Response(string fileName) 
    {
        // typeof() response humanresponse - Name: (Surname:) Votes:
        if (string.IsNullOrEmpty(fileName))
            return default(Blue_1.Response);
        
        string text = File.ReadAllText(fileName);
        var parsing = JsonConvert.DeserializeObject<dynamic>(text);

        if ((string)parsing.Type == "HumanResponse")
        {
            Blue_1.HumanResponse humanResponse = new Blue_1.HumanResponse((string)parsing.Name, (string)parsing.Surname,(int)parsing.Votes);
            return humanResponse;
        }
        else 
        {
            Blue_1.Response response = new Blue_1.Response((string)parsing.Name, (int)parsing.Votes);
            return response;
        }
    }
    public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName) 
    {
        // participant abstr 3m 5m - name bank part[]
        if (string.IsNullOrEmpty(fileName))
            return default(Blue_2.WaterJump);
        
        string text = File.ReadAllText(fileName);
        var parsing = JsonConvert.DeserializeObject<dynamic>(text);

        Blue_2.WaterJump jump;
        if ((string)parsing.Type == "WaterJump3m")
            jump = new Blue_2.WaterJump3m((string)parsing.Name, (int)parsing.Bank);
        else 
            jump = new Blue_2.WaterJump5m((string)parsing.Name, (int)parsing.Bank);
        
        foreach (var part in parsing.Participants)
        {
            Blue_2.Participant participant = new Blue_2.Participant((string)part.Name, (string)part.Surname);
            int[][] jumps = part.Marks.ToObject<int[][]>(); // JArray в зубчатый массив
            for (int i = 0; i<2; i++)
            {
                if (jumps[i].Length == 5)
                    participant.Jump(jumps[i]);
            }
            jump.Add(participant);
        }
        return jump;
    }
    public override T DeserializeBlue3Participant<T>(string fileName) 
    {
        if (string.IsNullOrEmpty(fileName) || !File.Exists(FilePath))
            return default(T);
        
        string text = File.ReadAllText(fileName);
        var parsing = JsonConvert.DeserializeObject<dynamic>(text);
        Blue_3.Participant player;
        switch ((string)parsing.Type)
        {
            case "BasketballPlayer":
                player = new Blue_3.BasketballPlayer((string)parsing.Name, (string)parsing.Surname);
                break;
            case "HockeyPlayer":
                player = new Blue_3.HockeyPlayer((string)parsing.Name, (string)parsing.Surname);
                break;
            default:
                player = new Blue_3.Participant((string)parsing.Name, (string)parsing.Surname);
                break;
        }
        
        foreach (var penalty in parsing.Penalties)
            player.PlayMatch(int.Parse(penalty.ToString()));

        return (T)(object)player;
    }
    public override Blue_4.Group DeserializeBlue4Group(string fileName) 
    {
        // name manteam womanteam ; team - name scores[]
        if (string.IsNullOrEmpty(fileName))
            return default(Blue_4.Group);
        
        string text = File.ReadAllText(fileName);
        var parsing = JsonConvert.DeserializeObject<dynamic>(text);

        Blue_4.Group group = new Blue_4.Group((string)parsing.Name);
        foreach (var team in parsing.Men)
        {
            Blue_4.ManTeam t = new Blue_4.ManTeam((string)team.Name);
            foreach (var score in team.Scores)
                t.PlayMatch(int.Parse(score.ToString()));
            group.Add(t);
        }
        foreach (var team in parsing.Women)
        {
            Blue_4.WomanTeam t = new Blue_4.WomanTeam((string)team.Name);
            foreach (var score in team.Scores)
                t.PlayMatch(int.Parse(score.ToString()));
            group.Add(t);
        }
        return group;
    }
    public override T DeserializeBlue5Team<T>(string fileName) 
    {
        // abstr manteam womanteam - name sportsman[], sportsmen - name surname place
        if (string.IsNullOrEmpty(fileName) || !File.Exists(FilePath))
            return default(T);
        
        string text = File.ReadAllText(fileName);
        var parsing = JsonConvert.DeserializeObject<dynamic>(text);

        Blue_5.Team team;
        if ((string) parsing.Type == "ManTeam")
            team = new Blue_5.ManTeam((string)parsing.Name);
        else
            team = new Blue_5.WomanTeam((string)parsing.Name);
        foreach (var sportsman in parsing.Sportsmen)
        {
            Blue_5.Sportsman s = new Blue_5.Sportsman((string)sportsman.Name, (string)sportsman.Surname);
            s.SetPlace((int)sportsman.Place);
            team.Add(s);
        }
        return (T)(object)team;
    }
}