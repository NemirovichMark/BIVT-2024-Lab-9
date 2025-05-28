using Lab_7;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
            SelectFile(fileName);

            StringBuilder txt = new StringBuilder();
            txt.AppendLine($"{student.Name}|{student.Surname}|{student.GetType().Name}|{student.Penalties.Length}|{string.Join(',', student.Penalties)}");

            File.WriteAllText(fileName, txt.ToString());
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            using (var txt = new StreamWriter(fileName)) 
            {
                txt.WriteLine($"Name:{participant.Name}");

                txt.WriteLine($"ManTeamsCount:{participant.ManTeams.Length}");
                foreach (var manteam in participant.ManTeams)
                {
                    if (manteam != null)
                    {
                        txt.WriteLine($"{manteam.Name}|{string.Join(',', manteam.Scores)}");
                    }
                }

                txt.WriteLine($"WomanTeamsCount:{participant.WomanTeams.Length}");
                foreach (var womanteam in participant.WomanTeams)
                {
                    if (womanteam != null)
                    {
                        txt.WriteLine($"{womanteam.Name}|{string.Join(',', womanteam.Scores)}");
                    }
                }
            }
        }

        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            using (var txt = new StreamWriter(fileName))
            {
                txt.WriteLine($"Name={group.Name}");
                txt.WriteLine($"Type={group.GetType().Name}");

                int cnt = 0;
                for (int i = 0; i < group.Sportsmen.Length; i++)
                {
                    if (group.Sportsmen[i] != null)
                    {
                        cnt++;
                    }
                }
                txt.WriteLine($"Count={cnt}");

                for (int i = 0; i < group.Sportsmen.Length; i++)
                {
                    if (group.Sportsmen[i] != null)
                    {
                        txt.WriteLine($"{group.Sportsmen[i].Name}|{group.Sportsmen[i].Surname}|{group.Sportsmen[i].Place}");
                    }

                }
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
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) return default(T);

            string[] txt = File.ReadAllLines(fileName); 

            string[] parts = txt[0].Split('|');
            if (parts.Length < 5) return default(T);

            string name = parts[0];
            string surname = parts[1];
            string type = parts[2];
            int[] penalties = new int[0];

            if (!string.IsNullOrEmpty(parts[4]))
            {
                string[] penaltyStrings = parts[4].Split(',');
                int[] tempPenalties = new int[penaltyStrings.Length];
                int count = 0;

                for (int i = 0; i < penaltyStrings.Length; i++)
                {
                    if (int.TryParse(penaltyStrings[i].Trim(), out int value))
                    {
                        tempPenalties[count] = value;
                        count++;
                    }
                }
                penalties = new int[count];
                Array.Copy(tempPenalties, penalties, count);
            }

            Blue_3.Participant participant = new Blue_3.Participant(name, surname);
            if (type == "BasketballPlayer")
            {
                participant = new Blue_3.BasketballPlayer(name, surname);
            }
            else if (type == "HockeyPlayer")
            {
                participant = new Blue_3.HockeyPlayer(name, surname);
            }

            foreach (int penalty in penalties)
            {
                participant.PlayMatch(penalty);
            }

            return (T)participant;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) return null;

            string[] txt = File.ReadAllLines(fileName); 

            string groupName = txt[0].Replace("Name:", "").Trim(); 
            var result = new Blue_4.Group(groupName);

            int manTeamsCount = 0, womanTeamsCount = 0;
            bool flagManTeams = false, flagWomanTeams = false;  

            foreach (string line in txt) 
            {
                if (line.StartsWith("ManTeamsCount:")) 
                {
                    manTeamsCount = int.Parse(line.Replace("ManTeamsCount:", "").Trim());
                    flagManTeams = true; flagWomanTeams = false;
                }
                else if (line.StartsWith("WomanTeamsCount:")) 
                {
                    womanTeamsCount = int.Parse(line.Replace("WomanTeamsCount:", "").Trim());
                    flagWomanTeams = true; flagManTeams = false;
                }
            }

            var manTeams = new Blue_4.ManTeam[manTeamsCount];
            var womanTeams = new Blue_4.WomanTeam[womanTeamsCount];
            int manIndex = 0, womanIndex = 0; 

            flagManTeams = false; 
            flagWomanTeams = false;

            foreach (string line in txt)
            {
                if (line.StartsWith("ManTeamsCount:"))
                {
                    flagManTeams = true; flagWomanTeams = false;
                    continue;
                }

                if (line.StartsWith("WomanTeamsCount:"))
                {
                    flagWomanTeams = true; flagManTeams = false;
                    continue;
                }

                if (line.Contains("|")) 
                {
                    string[] parts = line.Split('|');

                    string teamName = parts[0].Trim();
                    string[] scoresStr = parts[1].Split(',');

                    if (flagManTeams && manIndex < manTeams.Length)
                    {
                        var team = new Blue_4.ManTeam(teamName);
                        foreach (string scoreStr in scoresStr)
                        {
                            if (int.TryParse(scoreStr.Trim(), out int score))
                            {
                                team.PlayMatch(score);
                            }
                        }
                        manTeams[manIndex++] = team;
                    }
                    else if (flagWomanTeams && womanIndex < womanTeams.Length)
                    {
                        var team = new Blue_4.WomanTeam(teamName);
                        foreach (string scoreStr in scoresStr)
                        {
                            if (int.TryParse(scoreStr.Trim(), out int score))
                            {
                                team.PlayMatch(score);
                            }
                        }
                        womanTeams[womanIndex++] = team;
                    }
                }
            }

            result.Add(manTeams);
            result.Add(womanTeams);

            return result;
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName)) return null;

            string[] txt = File.ReadAllLines(fileName); 

            string name = txt[0].Replace("Name=", "").Trim(); 
            string type = txt[1].Replace("Type=", "").Trim();
            int count = int.Parse(txt[2].Replace("Count=", "").Trim()); 

            var sportsmen = new Blue_5.Sportsman[count]; 
            int ind = 0;

            for (int i = 3; i < txt.Length && ind < count; i++) 
            {
                string[] parts = txt[i].Split('|');
                if (parts.Length >= 3)
                {
                    var sportsman = new Blue_5.Sportsman( 
                        parts[0].Trim(),
                        parts[1].Trim()
                    );
                    sportsman.SetPlace(int.Parse(parts[2].Trim()));
                    sportsmen[ind++] = sportsman; 
                }
            }
            Blue_5.Team team = new Blue_5.WomanTeam(name);
            if (type == "ManTeam")
            {
                team = new Blue_5.ManTeam(name);
            }

            team.Add(sportsmen);
            return (T)team;
        }
    }
}