using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using Lab_7;

namespace Lab_9{
    public class BlueTXTSerializer : BlueSerializer{
        public override string Extension{
            get{
                return "txt";
            }
        }
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null) return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Type:{participant.GetType().Name}");
            sb.AppendLine($"Name:{participant.Name}");
            if (participant is Blue_1.HumanResponse hr)
            {
                sb.AppendLine($"Surname:{hr.Surname}");
            }
            sb.Append($"Votes:{participant.Votes}");

            string path = Path.Combine(FolderPath, fileName);
            File.WriteAllText(path, sb.ToString());
            
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            var content = new StringBuilder();
            content.Append(participant is Blue_2.WaterJump3m ? "3 " : "5 ");
            content.Append($"{participant.Name} {participant.Bank}");

            foreach (var p in participant.Participants)
            {
                content.AppendLine().AppendLine($"{p.Name} {p.Surname}");
                for (int jump = 0; jump < 2; jump++)
                {
                    for (int i = 0; i < 5; i++)
                        content.Append($" {p.Marks[jump, i]}");
                    content.AppendLine();
                }
            }
            string path = Path.Combine(FolderPath, fileName);
            using (var writer = new StreamWriter(path))
            {
                writer.Write(content.ToString());
            }
        }
        


        public override void SerializeBlue3Participant<T>(T student, string fileName){
            if (student == null) return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Type:{student.GetType().Name}");
            sb.AppendLine($"Name:{student.Name}");
            sb.AppendLine($"Surname:{student.Surname}");
            for (int i = 0; i < student.Penalties.Length; i++){
                    sb.Append($"{student.Penalties[i]} ");
                }
            string path = Path.Combine(FolderPath, fileName);
            File.WriteAllText(path, sb.ToString());
        }
        
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            var sb = new StringBuilder();
            sb.AppendLine(participant.Name)
              .AppendLine("<NewBlock>")
              .AppendLine(BuildTeamsInfo(participant.ManTeams))
              .AppendLine()
              .AppendLine("<NewTitle>")
              .AppendLine(BuildTeamsInfo(participant.WomanTeams));

            string path = Path.Combine(FolderPath, fileName);
            using (var writer = new StreamWriter(path))
            {
                writer.Write(sb.ToString());
            }
        }

        private string BuildTeamsInfo(Blue_4.Team[] teams)
        {
            if (teams == null || teams.Length == 0){
                return string.Empty;
            }

            var teamStrings = new string[teams.Length];

            for (int i = 0; i < teams.Length; i++)
            {
                if (teams[i] == null)
                {
                    teamStrings[i] = string.Empty;
                    continue;
                }
                var scores = string.Join(" ", teams[i].Scores);
                teamStrings[i] = $"{teams[i].Name}{Environment.NewLine}{scores}";
            }
            return string.Join($"{Environment.NewLine} --- {Environment.NewLine}", teamStrings);
        }


        private void SerializeBlue5Sportsmen(Blue_5.Team group, StreamWriter writer)
        {
            writer.Write("Sportsmen:: ");
            string text = "<";
            foreach(var sportsman in group.Sportsmen)
            {   
                if (sportsman != null) 
                {
                    text += "{";
                    text += $"Name:{sportsman.Name} ";
                    text += $"Surname:{sportsman.Surname} ";
                    text += $"Place:{sportsman.Place}";
                    text += "};";
                }
            }
            text = text.Length > 2 ? text.Remove(text.Length - 1): text;
            text += ">";
            writer.WriteLine(text);
        }

        public override void SerializeBlue5Team<T>(T group, string fileName) where T: class
        {
            if (group == null || String.IsNullOrEmpty(fileName)) {return;}
            string path = Path.Combine(FolderPath, fileName);
            File.WriteAllText(path, string.Empty);
            using(StreamWriter writer = File.AppendText(path))
            {
                writer.WriteLine($"Type:: {group.GetType().Name}");
                writer.WriteLine($"Name:: {group.Name}");
                SerializeBlue5Sportsmen(group, writer);
            }
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            string path = Path.Combine(FolderPath, fileName);

            using (StreamReader reader = new StreamReader(path))
            {
                Blue_1.Response result;
                string typeLine = reader.ReadLine(); 
                string type = typeLine?.Split(':')[1].Trim();
                if (type == nameof(Blue_1.HumanResponse)){
                    string nameLine = reader.ReadLine();   // Name:...
                    string name = nameLine?.Split(':')[1].Trim();
                    string surnameLine = reader.ReadLine();
                    string surname = surnameLine?.Split(':')[1].Trim();
                    string votesLine = reader.ReadLine();  // Votes:...
                    int votes = int.Parse(votesLine?.Split(':')[1] ?? "0");
                    result = new Blue_1.HumanResponse(name, surname, votes);

                }
                else{
                    string nameLine = reader.ReadLine();   // Name:...
                    string name = nameLine?.Split(':')[1].Trim();
                    string votesLine = reader.ReadLine();  // Votes:...
                    int votes = int.Parse(votesLine?.Split(':')[1] ?? "0");
                    result = new Blue_1.Response(name, votes);
                }
                return result;
            }
        }
        private Blue_2.WaterJump CreateJumpInstance(string heightType, string jumpName, string bankValue)
        {
            int bank = int.TryParse(bankValue, out int result) ? result : 0;

            return heightType switch
            {
                "3" => new Blue_2.WaterJump3m(jumpName, bank),
                "5" => new Blue_2.WaterJump5m(jumpName, bank),
                _ => null
            };
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            string fullPath = Path.Combine(FolderPath, fileName);

            if (!File.Exists(fullPath)) return null;

            var lines = File.ReadAllLines(fullPath)
                            .Where(l => !string.IsNullOrWhiteSpace(l))
                            .ToArray();

            if (lines.Length == 0) return null;

            string[] headerParts = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (headerParts.Length < 3) return null;

            var jump = CreateJumpInstance(headerParts[0], headerParts[1], headerParts[2]);
            if (jump == null) return null;

            for (int index = 1; index + 2 < lines.Length; index += 3)
            {
                string[] nameParts = lines[index].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (nameParts.Length < 2) continue;

                var athlete = new Blue_2.Participant(nameParts[0], nameParts[1]);

                var attempt1 = GetMarks(lines[index + 1]);
                var attempt2 = GetMarks(lines[index + 2]);

                if (attempt1.Length > 0) athlete.Jump(attempt1);
                if (attempt2.Length > 0) athlete.Jump(attempt2);

                jump.Add(athlete);
            }

            return jump;
        }

        private Blue_3.Participant CheckParticipantType(string type, string name, string surname) 
        {
            switch (type)
            {
                case "BasketballPlayer":
                    var b = new Blue_3.BasketballPlayer(name, surname) ;
                    return b;
                case "HockeyPlayer":
                    var hp = new Blue_3.HockeyPlayer(name, surname);
                    return hp;
                case "Participant":
                    var p = new Blue_3.Participant(name, surname);
                    return p;
                default:
                    return null;
            }
        }
        public override T DeserializeBlue3Participant<T>(string fileName){
            string path = Path.Combine(FolderPath, fileName);

            using (StreamReader reader = new StreamReader(path))
            {
                string typeLine = reader.ReadLine();
                string type = typeLine?.Split(':')[1].Trim();
                string nameLine = reader.ReadLine();
                string name = nameLine?.Split(':')[1].Trim();
                string surnameLine = reader.ReadLine();
                string surname = surnameLine?.Split(':')[1].Trim();
                string marks = reader.ReadLine();

                T student = (T) CheckParticipantType(type, name, surname);

                string[] sMarks = marks.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                int[] iMarks = new int[sMarks.Length];

                for (int k = 0; k < sMarks.Length; k++)
                {
                    iMarks[k] = int.Parse(sMarks[k]);
                }

                foreach (int p in iMarks)
                    student.PlayMatch(p);

                return student;                 
        }
        }
        private int[] GetMarks(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return Array.Empty<int>();

            var parts = input.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<int>(parts.Length);

            foreach (var part in parts)
            {
                if (int.TryParse(part, out int number)) result.Add(number);
            }

            return result.ToArray();
        }
         public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            string path = Path.Combine(FolderPath, fileName);

            using (var reader = new StreamReader(path))
            {
                string str = reader.ReadToEnd();
                if (String.IsNullOrEmpty(str)) return null;

                string[] info = str.Split("<NewBlock>", StringSplitOptions.RemoveEmptyEntries);

                var group = new Blue_4.Group(info[0].Replace(Environment.NewLine, ""));
                if (info.Length == 1) return group;

                string[] teams = info[1].Split("<NewTitle>");
                string[] manTeamsInfo = teams[0].Split(" --- ", StringSplitOptions.RemoveEmptyEntries);
                string[] womanTeamsInfo = teams[1].Split(" --- ", StringSplitOptions.RemoveEmptyEntries);
                AddTeams(manTeamsInfo, "man", group);
                AddTeams(womanTeamsInfo, "woman", group);

                return group;
            }
        }

        private Blue_4.Team GetTeam(string name, string type)
        {
            switch (type)
            {
                case "man":
                    return new Blue_4.ManTeam(name);
                case "woman":
                    return new Blue_4.WomanTeam(name);
                default:
                    return null;
            }
        }
        private void AddTeams(string[] teams, string type, Blue_4.Group group)
        {
            foreach (string team_tostring in teams)
            {
                if (String.IsNullOrWhiteSpace(team_tostring)) continue;

                var teamInfo = team_tostring.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                var team = GetTeam(teamInfo[0].Replace(Environment.NewLine, ""), type);
                int[] scores = GetMarks(teamInfo[1]);
                foreach (int score in scores)
                {
                    team.PlayMatch(score);
                }    

                group.Add(team);
            }
        }

        private Blue_5.Sportsman[] ParseSportsmen(string rawData)
        {
            var sportsmenList = new List<Blue_5.Sportsman>();

            // Удаляем внешние скобки
            string cleaned = rawData.Substring(1, rawData.Length - 2);
            string[] entries = cleaned.Split(';');

            foreach (var entry in entries)
            {
                if (entry.Length < 2)
                    continue;

                // Удаляем внутренние скобки
                string content = entry.Substring(1, entry.Length - 2);
                var parts = content.Split(' ');

                var attributes = new Dictionary<string, string>();
                foreach (var part in parts)
                {
                    var keyValue = part.Split(':');
                    if (keyValue.Length == 2)
                        attributes[keyValue[0].Trim()] = keyValue[1].Trim();
                }

                var sportsman = new Blue_5.Sportsman(attributes["Name"], attributes["Surname"]);
                sportsman.SetPlace(int.Parse(attributes["Place"]));
                sportsmenList.Add(sportsman);
            }

            return sportsmenList.ToArray();
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            string fullPath = Path.Combine(FolderPath, fileName);
            var teamInfo = new Dictionary<string, string>();

            using (var stream = File.OpenRead(fullPath))
            using (var reader = new StreamReader(stream))
            {
                for (int lineNum = 0; lineNum < 3; lineNum++)
                {
                    string line = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line) && line.Contains("::"))
                    {
                        var pair = line.Split("::");
                        if (pair.Length == 2)
                            teamInfo[pair[0].Trim()] = pair[1].Trim();
                    }
                }
            }

            if (teamInfo.Count == 0)
                return null;

            Blue_5.Team team = teamInfo["Type"] == "ManTeam"
                ? new Blue_5.ManTeam(teamInfo["Name"])
                : new Blue_5.WomanTeam(teamInfo["Name"]);

            var members = ParseSportsmen(teamInfo["Sportsmen"]);
            if (members.Length > 0)
                team.Add(members);

            return team as T;
        }


    }
}