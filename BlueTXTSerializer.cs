using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab_7.Blue_5;
using static System.Net.Mime.MediaTypeNames;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension
        {
            get
            {
                return "txt";
            }
        }
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            string content;

            if (participant is Blue_1.HumanResponse human)
            {
                content = $"{human.Name} {human.Surname} {human.Votes}";
            }
            else
            {
                content = $"{participant.Name} {participant.Votes}";
            }
            File.WriteAllText(FilePath, content);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            if (!File.Exists(FilePath)) return null;
            var content = File.ReadAllText(FilePath);
            if (string.IsNullOrEmpty(content)) return null;
            var contentToCheck = content.Split(" ");

            if (contentToCheck.Length == 2)
            {
                return new Blue_1.Response(contentToCheck[0], Int32.Parse(contentToCheck[1]));
            }
            else if (contentToCheck.Length == 3)
            {
                return new Blue_1.HumanResponse(contentToCheck[0], contentToCheck[1], Int32.Parse(contentToCheck[2]));
            }
            return null;
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            string content;
            content = $"{participant.GetType().Name}{Environment.NewLine}";
            content += $"{participant.Name} {participant.Bank}{Environment.NewLine}";
            if (participant.Participants != null && participant.Participants.Length > 0)
            {
                for (int i = 0; i < participant.Participants.Length; i++)
                {
                    Blue_2.Participant participantI = participant.Participants[i];
                    if (participantI.Marks == null) continue;
                    content += $"{participantI.Name} {participantI.Surname}";
                    for (int j = 0; j < 2; j++)
                    {
                        for (int k = 0; k < 5; k++)
                        {
                            content += " " + participantI.Marks[j, k];
                        }
                    }
                    content += Environment.NewLine;
                }
            }
            SelectFile(fileName);
            File.WriteAllText(FilePath, content);
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            SelectFile(fileName);

            if (!File.Exists(FilePath))
                return null;

            string[] allLines;
            try
            {
                allLines = File.ReadAllLines(FilePath);
            }
            catch
            {
                return null;
            }

            if (allLines.Length < 2)
                return null;

            string objectType = allLines[0];
            string[] headerParts = allLines[1].Split(' ');

            if (headerParts.Length != 2)
                return null;

            if (!int.TryParse(headerParts[1], out int bank))
                return null;

            string name = headerParts[0];

            Blue_2.WaterJump waterJump;
            if (objectType == nameof(Blue_2.WaterJump3m))
            {
                waterJump = new Blue_2.WaterJump3m(name, bank);
            }
            else if (objectType == nameof(Blue_2.WaterJump5m))
            {
                waterJump = new Blue_2.WaterJump5m(name, bank);
            }
            else
            {
                return null;
            }

            for (int i = 2; i < allLines.Length; i++)
            {
                string line = allLines[i];
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(' ');

                if (parts.Length < 12)
                    continue;

                string participantName = parts[0];
                string participantSurname = parts[1];

                var participant = new Blue_2.Participant(participantName, participantSurname);

                int[] firstJump = new int[5];
                bool firstJumpValid = true;
                for (int j = 0; j < 5; j++)
                {
                    if (!int.TryParse(parts[2 + j], out firstJump[j]))
                    {
                        firstJumpValid = false;
                        break;
                    }
                }
                if (!firstJumpValid) continue;

                int[] secondJump = new int[5];
                bool secondJumpValid = true;
                for (int j = 0; j < 5; j++)
                {
                    if (!int.TryParse(parts[7 + j], out secondJump[j]))
                    {
                        secondJumpValid = false;
                        break;
                    }
                }
                if (!secondJumpValid) continue;

                participant.Jump(firstJump);
                participant.Jump(secondJump);
                waterJump.Add(participant);
            }

            return waterJump;
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            string content = $"{student.GetType().Name} {student.Name} {student.Surname}";
            content += Environment.NewLine;

            if (student.Penalties != null)
            {
                foreach (var minute in student.Penalties)
                {
                    content += minute + " ";
                }
                content += Environment.NewLine;
            }
            File.WriteAllText(FilePath, content);
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            SelectFile(fileName);
            if (!File.Exists(FilePath)) return null;

            string[] lines = File.ReadAllLines(FilePath);
            if (lines.Length == 0) return null;

            string[] info = lines[0].Split();
            string type = info[0];
            string name = info[1];
            string surname = info[2];
            string penalties = lines[1];

            Blue_3.Participant participant;

            switch (type)
            {
                case nameof(Blue_3.BasketballPlayer):
                    participant = new Blue_3.BasketballPlayer(name, surname);
                    break;
                case nameof(Blue_3.HockeyPlayer):
                    participant = new Blue_3.HockeyPlayer(name, surname);
                    break;
                case nameof(Blue_3.Participant):
                    participant = new Blue_3.Participant(name, surname);
                    break;
                default:
                    return null;
            }

            if (!string.IsNullOrWhiteSpace(penalties))
            {
                string[] minutesStr = penalties.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var minuteStr in minutesStr)
                {
                    if (int.TryParse(minuteStr, out int minute))
                    {
                        participant.PlayMatch(minute);
                    }
                }
            }

            return (T)(object)participant;

        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            string content = participant.Name + Environment.NewLine;
            SelectFile(fileName);

            if (participant.ManTeams != null)
            {
                content += "ManTeams";
                foreach (var team in participant.ManTeams)
                {
                    if (team == null) continue;
                    content += team.Name + " ";
                    foreach (var scores in team.Scores)
                    {
                        content += scores.ToString() + " ";
                    }
                }
            }
            content += Environment.NewLine;

            if (participant.WomanTeams != null)
            {
                content += "WomanTeams";
                foreach (var team in participant.WomanTeams)
                {
                    if (team == null) continue;
                    content += team.Name + " ";
                    foreach (var scores in team.Scores)
                    {
                        content += scores.ToString() + " ";
                    }
                }
            }
            File.WriteAllText(FilePath, content);
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);

            if (!File.Exists(FilePath))
                return null;

            string content = File.ReadAllText(FilePath);
            string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            if (lines.Length < 1) return null;

            var group = new Blue_4.Group(lines[0].Trim());

            if (lines.Length > 1 && lines[1].StartsWith("ManTeams"))
            {
                string manTeamsData = lines[1].Substring("ManTeams".Length).Trim();
                if (!string.IsNullOrEmpty(manTeamsData))
                {
                    string[] parts = manTeamsData.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string currentTeamName = null;
                    Blue_4.ManTeam currentTeam = null;

                    foreach (string part in parts)
                    {
                        if (string.IsNullOrWhiteSpace(part)) continue;

                        if (!int.TryParse(part, out int score))
                        {
                            if (currentTeam != null)
                            {
                                group.Add(currentTeam);
                            }
                            currentTeamName = part;
                            currentTeam = new Blue_4.ManTeam(currentTeamName);
                        }
                        else if (currentTeam != null)
                        {
                            currentTeam.PlayMatch(score);
                        }
                    }

                    if (currentTeam != null)
                    {
                        group.Add(currentTeam);
                    }
                }
            }

            if (lines.Length > 2 && lines[2].StartsWith("WomanTeams"))
            {
                string womanTeamsData = lines[2].Substring("WomanTeams".Length).Trim();
                if (!string.IsNullOrEmpty(womanTeamsData))
                {
                    string[] parts = womanTeamsData.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string currentTeamName = null;
                    Blue_4.WomanTeam currentTeam = null;

                    foreach (string part in parts)
                    {
                        if (string.IsNullOrWhiteSpace(part)) continue;

                        if (!int.TryParse(part, out int score))
                        {
                            if (currentTeam != null)
                            {
                                group.Add(currentTeam);
                            }
                            currentTeamName = part;
                            currentTeam = new Blue_4.WomanTeam(currentTeamName);
                        }
                        else if (currentTeam != null)
                        {
                            currentTeam.PlayMatch(score);
                        }
                    }

                    if (currentTeam != null)
                    {
                        group.Add(currentTeam);
                    }
                }
            }

            return group;
        }

        public override void SerializeBlue5Team<T>(T team, string fileName)
        {
            if (team == null || string.IsNullOrEmpty(fileName)) return;

            string type = team is Blue_5.ManTeam ? "man" : "woman";
            string content = type + "\n" + team.Name + "\n";

            if (team.Sportsmen != null)
            {
                foreach (var s in team.Sportsmen)
                {
                    if (s != null)
                        content += $"{s.Name} {s.Surname} {s.Place}\n";
                }
            }

            SelectFile(fileName);
            File.WriteAllText(FilePath, content);
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return default;

            SelectFile(fileName);
            if (!File.Exists(FilePath)) return default;

            string[] lines = File.ReadAllLines(FilePath);
            if (lines.Length < 2) return default;

            Blue_5.Team team = lines[0] == "man"
                ? new Blue_5.ManTeam(lines[1])
                : new Blue_5.WomanTeam(lines[1]);

            for (int i = 2; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(' ');
                if (parts.Length >= 2)
                {
                    var s = new Blue_5.Sportsman(parts[0], parts[1]);
                    if (parts.Length >= 3)
                        s.SetPlace(int.Parse(parts[2]));
                    team.Add(s);
                }
            }

            return (T)(object)team;
        }
    }
}
