using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Lab_7;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension => "txt";

        //СЕРИАЛИЗАЦИЯ

        public override void SerializeBlue1Response(Blue_1.Response response, string fileName)
        {
            var lines = new List<string>
            {
                response.GetType().Name,
                response.Name ?? "",
                response.Votes.ToString()
            };

            if (response is Blue_1.HumanResponse humanResponse)
            {
                lines.Add(humanResponse.Surname ?? "");
            }

            File.WriteAllLines(fileName, lines);
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump jump, string fileName)
        {
            var sb = new StringBuilder();
            sb.AppendLine(jump.GetType().Name);
            sb.AppendLine(jump.Name);
            sb.AppendLine(jump.Bank.ToString());
            sb.AppendLine(jump.Count.ToString());
            sb.AppendLine(jump.Count5m.ToString());

            if (jump.Participants != null)
            {
                foreach (var p in jump.Participants)
                {
                    sb.AppendLine($"{p.Name},{p.Surname}");
                    if (p.Marks != null)
                    {
                        for (int i = 0; i < p.Marks.GetLength(0); i++)
                        {
                            var marks = string.Join(",", Enumerable.Range(0, p.Marks.GetLength(1)).Select(j => p.Marks[i, j]));
                            sb.AppendLine(marks);
                        }
                    }
                }
            }

            File.WriteAllText(fileName, sb.ToString());
        }

        public override void SerializeBlue3Participant<T>(T participant, string fileName)
        {
            var sb = new StringBuilder();
            sb.AppendLine(participant.GetType().Name);
            sb.AppendLine(participant.Name);
            sb.AppendLine(participant.Surname);

            if (participant.Penalties != null)
            {
                sb.AppendLine(string.Join(",", participant.Penalties));
            }
            else
            {
                sb.AppendLine();
            }

            File.WriteAllText(fileName, sb.ToString());
        }

        public override void SerializeBlue4Group(Blue_4.Group group, string fileName)
        {
            var sb = new StringBuilder();
            sb.AppendLine(group.Name);

            if (group.ManTeams != null)
            {
                foreach (var team in group.ManTeams.Where(t => t != null))
                {
                    sb.AppendLine($"ManTeam,{team.Name}");
                    if (team.Scores != null)
                    {
                        sb.AppendLine(string.Join(",", team.Scores));
                    }
                    else
                    {
                        sb.AppendLine();
                    }
                }
            }

            if (group.WomanTeams != null)
            {
                foreach (var team in group.WomanTeams.Where(t => t != null))
                {
                    sb.AppendLine($"WomanTeam,{team.Name}");
                    if (team.Scores != null)
                    {
                        sb.AppendLine(string.Join(",", team.Scores));
                    }
                    else
                    {
                        sb.AppendLine();
                    }
                }
            }

            File.WriteAllText(fileName, sb.ToString());
        }

        public override void SerializeBlue5Team<T>(T team, string fileName)
        {
            var sb = new StringBuilder();
            sb.AppendLine(team.GetType().Name);
            sb.AppendLine(team.Name);

            if (team.Sportsmen != null)
            {
                foreach (var sportsman in team.Sportsmen.Where(s => s != null))
                {
                    sb.AppendLine($"{sportsman.Name},{sportsman.Surname},{sportsman.Place}");
                }
            }

            File.WriteAllText(fileName, sb.ToString());
        }

        //ДЕСЕРИАЛИЗАЦИЯ

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            if (lines.Length < 3) return null;

            var typeName = lines[0];
            var name = lines[1];
            var votes = int.Parse(lines[2]);

            if (typeName == nameof(Blue_1.HumanResponse))
            {
                var surname = lines.Length > 3 ? lines[3] : "";
                return new Blue_1.HumanResponse(name, surname, votes);
            }
            return new Blue_1.Response(name, votes);
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            if (lines.Length < 5) return null;

            var typeName = lines[0];
            var name = lines[1];
            var bank = int.Parse(lines[2]);
            var count = int.Parse(lines[3]);
            var count5m = int.Parse(lines[4]);

            Blue_2.WaterJump jump = typeName == nameof(Blue_2.WaterJump5m) ? 
                new Blue_2.WaterJump5m(name, bank) : 
                new Blue_2.WaterJump3m(name, bank);

            for (int i = 5; i < lines.Length; i += 3)
            {
                if (i >= lines.Length) break;
                
                var nameParts = lines[i].Split(',');
                var participant = new Blue_2.Participant(nameParts[0], nameParts[1]);
                
                if (i + 1 < lines.Length && !string.IsNullOrEmpty(lines[i + 1]))
                {
                    var marks1 = lines[i + 1].Split(',').Select(int.Parse).ToArray();
                    var marks2 = i + 2 < lines.Length ? 
                        lines[i + 2].Split(',').Select(int.Parse).ToArray() : 
                        new int[5];
                    
                    participant.Jump(marks1);
                    participant.Jump(marks2);
                }
                
                jump.Add(participant);
            }

            return jump;
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            if (lines.Length < 4) return default;

            var typeName = lines[0];
            var name = lines[1];
            var surname = lines[2];
            var penalties = lines[3].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToArray();

            object participant;
            if (typeName == nameof(Blue_3.BasketballPlayer))
            {
                participant = new Blue_3.BasketballPlayer(name, surname);
            }
            else if (typeName == nameof(Blue_3.HockeyPlayer))
            {
                participant = new Blue_3.HockeyPlayer(name, surname);
            }
            else
            {
                participant = new Blue_3.Participant(name, surname);
            }

            foreach (var penalty in penalties)
            {
                ((dynamic)participant).PlayMatch(penalty);
            }

            return (T)participant;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            if (lines.Length == 0) return null;

            var group = new Blue_4.Group(lines[0]);

            for (int i = 1; i < lines.Length; i += 2)
            {
                if (i >= lines.Length) break;

                var teamInfo = lines[i].Split(',');
                var teamType = teamInfo[0];
                var teamName = teamInfo[1];

                Blue_4.Team team = teamType == "ManTeam" ? 
                    new Blue_4.ManTeam(teamName) : 
                    new Blue_4.WomanTeam(teamName);

                if (i + 1 < lines.Length && !string.IsNullOrEmpty(lines[i + 1]))
                {
                    var scores = lines[i + 1].Split(',')
                        .Select(int.Parse).ToArray();
                    foreach (var score in scores)
                    {
                        team.PlayMatch(score);
                    }
                }

                group.Add(team);
            }

            return group;
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            if (lines.Length < 2) return default;

            var typeName = lines[0];
            var teamName = lines[1];

            Blue_5.Team team = typeName == nameof(Blue_5.ManTeam) ? 
                new Blue_5.ManTeam(teamName) : 
                new Blue_5.WomanTeam(teamName) as Blue_5.Team;

            for (int i = 2; i < lines.Length; i++)
            {
                var parts = lines[i].Split(',');
                if (parts.Length < 3) continue;

                var sportsman = new Blue_5.Sportsman(parts[0], parts[1]);
                sportsman.SetPlace(int.Parse(parts[2]));
                team.Add(sportsman);
            }

            return (T)team;
        }
    }
}