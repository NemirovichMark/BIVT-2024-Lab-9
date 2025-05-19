using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static Lab_7.Blue_2;
using Newtonsoft.Json.Linq;
using System.Reflection.Metadata;
using static Lab_7.Blue_1;
using static Lab_7.Blue_3;
using System.Runtime.ExceptionServices;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        override public string Extension => "json";

        override public void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if ((participant == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            SelectFile(fileName);
            var json = JObject.FromObject(participant);
            json.Add("Type", participant.GetType().ToString());
            File.WriteAllText(FilePath, json.ToString());
        }

        override public void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if ((participant == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            SelectFile(fileName);
            var json = JObject.FromObject(participant);
            json.Add("Type", participant.GetType().ToString());
            File.WriteAllText(FilePath, json.ToString());
        }
        override public void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if ((student == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            SelectFile(fileName);
            var json = JObject.FromObject(student);
            json.Add("Type", student.GetType().ToString());
            File.WriteAllText(FilePath, json.ToString());
        }
        override public void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if ((participant == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            SelectFile(fileName);
            var json = JObject.FromObject(participant);
            File.WriteAllText(FilePath, json.ToString());
        }
        override public void SerializeBlue5Team<T>(T group, string fileName)
        {
            if ((group == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            SelectFile(fileName);
            var json = JObject.FromObject(group);
            json.Add("Type", group.GetType().ToString());
            File.WriteAllText(FilePath, json.ToString());
        }

        override public Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);
            Blue_1.Response response;
            if (json["Type"].ToString() == "Lab_7.Blue_1+Response")
            {
                response = new Blue_1.Response(json["Name"].ToString(), json["Votes"].ToObject<int>());
            }
            else
            {
                response = new Blue_1.HumanResponse(json["Name"].ToString(), json["Surname"].ToString(), json["Votes"].ToObject<int>());
            }
            return response;
        }
        override public Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);
            Console.WriteLine(json.ToString());
            Blue_2.WaterJump waterjump;
            if (json["Type"].ToString() == "Lab_7.Blue_2+WaterJump3m")
            {
                waterjump = new Blue_2.WaterJump3m(json["Name"].ToString(), int.Parse(json["Bank"].ToString()));
            }
            else
            {
                waterjump = new Blue_2.WaterJump5m(json["Name"].ToString(), int.Parse(json["Bank"].ToString()));
            }
            Blue_2.Participant[] participants = json["Participants"].ToObject<Blue_2.Participant[]>();
            for (int i = 0; i < participants.Length; i++)
            {
                int[,] marks = json["Participants"][i]["Marks"].ToObject<int[,]>();
                int[] jump = new int[5];
                for (int j = 0; j < 5; j++) jump[j] = marks[0, j];
                participants[i].Jump(jump);
                for (int j = 0; j < 5; j++) jump[j] = marks[1, j];
                participants[i].Jump(jump);
            }
            waterjump.Add(participants);
            return waterjump;
        }
        override public T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);
            Blue_3.Participant participant;
            if (json["Type"].ToString() == "Lab_7.Blue_3+BasketballPlayer")
            {
                participant = new Blue_3.BasketballPlayer(json["Name"].ToString(), json["Surname"].ToString());
            }
            else if (json["Type"].ToString() == "Lab_7.Blue_3+HockeyPlayer")
            {
                participant = new Blue_3.HockeyPlayer(json["Name"].ToString(), json["Surname"].ToString());
            }
            else
            {
                participant = new Blue_3.Participant(json["Name"].ToString(), json["Surname"].ToString());
            }
            int[] penalties = json["Penalties"].ToObject<int[]>();
            for (int i = 0; i < penalties.Length; i++) participant.PlayMatch(penalties[i]);
            return (T)participant;
            
        }
        override public Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);
            Blue_4.Group group = new Blue_4.Group(json["Name"].ToString());
            Blue_4.ManTeam[] manteams = json["ManTeams"].ToObject<Blue_4.ManTeam[]>();
            for (int i = 0; i < manteams.Length; i++)
            {
                if (manteams[i] == null) break;
                int[] scores = json["ManTeams"][i]["Scores"].ToObject<int[]>();
                for (int j = 0; j < scores.Length; j++) manteams[i].PlayMatch(scores[j]);
            }
            group.Add(manteams);
            Blue_4.WomanTeam[] womanteams = json["WomanTeams"].ToObject<Blue_4.WomanTeam[]>();
            for (int i = 0; i < womanteams.Length; i++)
            {
                if (womanteams[i] == null) break;
                int[] scores = json["WomanTeams"][i]["Scores"].ToObject<int[]>();
                for (int j = 0; j < scores.Length; j++) womanteams[i].PlayMatch(scores[j]);
            }
            group.Add(womanteams);
            return group;
        }
        override public T DeserializeBlue5Team<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            var json = JObject.Parse(text);
            Blue_5.Team team;
            if (json["Type"].ToString() == "Lab_7.Blue_5+ManTeam")
            {
                team = new Blue_5.ManTeam(json["Name"].ToString());
            }
            else
            {
                team = new Blue_5.WomanTeam(json["Name"].ToString());
            }
            Blue_5.Sportsman[] sportsmen = json["Sportsmen"].ToObject<Blue_5.Sportsman[]>();
            for (int i = 0; i < team.Sportsmen.Length; i++)
            {
                if (sportsmen[i] == null) break;
                sportsmen[i].SetPlace(json["Sportsmen"][i]["Place"].ToObject<int>());
            }
            team.Add(sportsmen);
            return (T)team;
        }
    }
}
