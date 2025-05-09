using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Lab_7;
using static Lab_7.Blue_2;
using static Lab_7.Blue_3;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension => "txt";


        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            string str;
            if (participant is Blue_1.HumanResponse instance)
            {
                str = $"{instance.Name} {instance.Surname} {instance.Votes}";
            }
            else
            {
                str = $"{participant.Name} {participant.Votes}";
            }

            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.Write(str);
            }
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

            SelectFile(fileName);
            using (var writer = new StreamWriter(FilePath))
            {
                writer.Write(content.ToString());
            }
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;

            var sb = new StringBuilder();

            sb.AppendLine(student switch
            {
                Blue_3.BasketballPlayer => "BasketballPlayer",
                Blue_3.HockeyPlayer => "HockeyPlayer",
                _ => "Participant"
            });

            sb.AppendLine($"{student.Name} {student.Surname}");
            sb.AppendLine(string.Join(" ", student.Penalties)); 
            sb.AppendLine(student.Total.ToString());
            sb.AppendLine(student.IsExpelled.ToString());

            SelectFile(fileName);
            using (var writer = new StreamWriter(FilePath))
            {
                writer.Write(sb.ToString());
            }
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

            SelectFile(fileName);
            using (var writer = new StreamWriter(FilePath))
            {
                writer.Write(sb.ToString());
            }
        }

        private string BuildTeamsInfo(Blue_4.Team[] teams)
        {
            if (teams == null || teams.Length == 0) return string.Empty;

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



        public override void SerializeBlue5Team<T>(T team, string fileName)
        {
            if (team == null || string.IsNullOrEmpty(fileName)) return;

            var content = new StringBuilder();
            if (team is Blue_5.ManTeam) content.Append("man ");
            else if (team is Blue_5.WomanTeam) content.Append("woman ");
            else return;

            content.AppendLine(team.Name);
            foreach (var sportsman in team.Sportsmen)
            {
                if (sportsman == null) break;
                content.AppendLine($"{sportsman.Name} {sportsman.Surname} {sportsman.Place}");
            }

            SelectFile(fileName);
            using (var writer = new StreamWriter(FilePath))
            {
                writer.Write(content.ToString());
            }
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);

            using (var reader = new StreamReader(FilePath))
            {
                string fileContent = reader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(fileContent))
                    return null;

                var words = fileContent.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length == 2 && int.TryParse(words[1], out int votes))
                    return new Blue_1.Response(words[0], votes);

                if (words.Length >= 3 && int.TryParse(words[2], out int humanVotes))
                    return new Blue_1.HumanResponse(words[0], words[1], humanVotes);
            }

            return null;
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);

            using (var reader = new StreamReader(FilePath))
            {
                string fileContent = reader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(fileContent)) return null;

                string[] lines = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length == 0) return null;

                string[] header = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (header.Length < 3)
                    return null;

                var jump = GetWaterJump(header[0], header[1], header[2]);
                if (jump == null || lines.Length == 1) return jump;

                for (int i = 1; i < lines.Length; i += 3)
                {
                    if (i + 2 >= lines.Length) break;

                    string[] participantInfo = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (participantInfo.Length < 2) continue;

                    var participant = new Blue_2.Participant(participantInfo[0], participantInfo[1]);

                    int[] marks1 = GetMarks(lines[i + 1]);
                    int[] marks2 = GetMarks(lines[i + 2]);

                    if (marks1.Length > 0) participant.Jump(marks1);
                    if (marks2.Length > 0) participant.Jump(marks2);

                    jump.Add(participant);
                }

                return jump;
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
        private Blue_2.WaterJump GetWaterJump(string type, string name, string bank)
        {
            if (type == "3") return new Blue_2.WaterJump3m(name, int.Parse(bank));
            else if (type == "5") return new Blue_2.WaterJump5m(name, int.Parse(bank));
            return null;
        }


        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            SelectFile(fileName);

            using (var reader = new StreamReader(FilePath))
            {
                string str = reader.ReadToEnd();
                if (String.IsNullOrEmpty(str)) return null;

                string[] lines = str.Split(Environment.NewLine);
                string[] words = lines[1].Split(" ");
                T participant = (T)GetParticipant(lines[0], words[0], words[1]);
                if (lines.Length == 2) return participant;

                int[] penalties = GetMarks(lines[2]);
                foreach (int penalty in penalties)
                {
                    participant.PlayMatch(penalty);
                }

                return participant;
            }
        }
        private Blue_3.Participant GetParticipant(string type, string name, string surname)
        {
            switch (type)
            {
                case "BasketballPlayer":
                    return new Blue_3.BasketballPlayer(name, surname);
                case "HockeyPlayer": 
                    return new Blue_3.HockeyPlayer(name, surname);
                case "Participant":
                    return new Blue_3.Participant(name, surname);
                default:
                    return null;
            }
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);

            using (var reader = new StreamReader(FilePath))
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
        public override T DeserializeBlue5Team<T>(string fileName)
        {
            SelectFile(fileName);

            using (var reader = new StreamReader(FilePath))
            {
                string str = reader.ReadToEnd();
                if (String.IsNullOrEmpty(str)) return default(T);

                string[] lines = str.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length == 0) return default(T);

                string[] words = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (words.Length < 2) return default(T);

                T team = (T)GetGroup(words[0], words[1]);
                if (team == null || lines.Length == 1) return team;

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] sportsmanWords = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (sportsmanWords.Length < 3) continue;

                    var sportsman = new Blue_5.Sportsman(sportsmanWords[0], sportsmanWords[1]);
                    sportsman.SetPlace(int.Parse(sportsmanWords[2]));
                    team.Add(sportsman);
                }

                return team;
            }
        }
        private Blue_5.Team GetGroup(string type, string name)
        {
            switch (type)
            {
                case "man":
                    return new Blue_5.ManTeam(name);
                case "woman":
                    return new Blue_5.WomanTeam(name);
                default:
                    return null;
            }
        }
    }
}
