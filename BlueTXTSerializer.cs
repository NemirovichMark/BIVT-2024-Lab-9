using Lab_7;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension => "txt";
        
        public override void SerializeBlue1Response(Blue_1.Response response, string filename)
        {
            if (response == null || string.IsNullOrWhiteSpace(filename)) return;

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);

            StringBuilder builder = new StringBuilder();
            if (response is Blue_1.HumanResponse human)
            {
                builder.Append($"{human.Name} {human.Surname} {human.Votes}");
            }
            else
            {
                builder.Append($"{response.Name} {response.Votes}");
            }

            File.WriteAllText(path, builder.ToString());
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (!File.Exists(fullPath)) return null;
            var text = File.ReadAllText(fullPath);
            if (String.IsNullOrEmpty(text)) return null;
            var temp = text.Split(' ');
            if (temp.Length == 2)
            {
                return new Blue_1.Response(temp[0], Int32.Parse(temp[1]));
            }
            else if (temp.Length == 3)
            {
                return new Blue_1.HumanResponse(temp[0], temp[1], Int32.Parse(temp[2]));
            }
            return null;
        }
        //Blue_2
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
                foreach (var part in participant.Participants)
                {                    
                    string marks = "[";
                    for (int i = 0; i < 5; i++)
                        marks += $"{part.Marks[0, i]},";
                    marks = marks.Remove(marks.Length - 1);
                    marks += "],[";
                    for (int i = 0; i < 5; i++)
                        marks += $"{part.Marks[1, i]},";
                    marks = marks.Remove(marks.Length - 1);
                    marks += "]";
                    writer.WriteLine($"{part.Name}|{part.Surname}|{marks}");
                }
            }
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {            
            if (string.IsNullOrEmpty(fileName))
                return default(Blue_2.WaterJump);

            string[] lines = File.ReadAllLines(fileName);            
            Dictionary<string, string> parsing = new Dictionary<string, string>();
            List<Blue_2.Participant> participants = new List<Blue_2.Participant>();
            foreach (string line in lines)
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
        /
        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;

            string type = student switch
            {
                Blue_3.BasketballPlayer _ => "Basketball",
                Blue_3.HockeyPlayer _ => "Hockey",
                Blue_3.Participant _ => "Participant",
                _ => null
            };

            if (type == null) return;

            string text = type;

            if (!string.IsNullOrEmpty(student.Name) && !string.IsNullOrEmpty(student.Surname))
            {
                text += $"{Environment.NewLine}{student.Name} {student.Surname}";
            }
            else
            {
                return;
            }

            if (student.Penalties != null && student.Penalties.Length > 0)
            {
                text += Environment.NewLine + string.Join(" ", student.Penalties);
            }

            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            if (!File.Exists(FilePath)) return null;
            string text = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(text)) return null;
            var temp = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length < 2) return null;
            var tempNameSurname = temp[1].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (tempNameSurname.Length < 2) return null;
            Blue_3.Participant t;
            if (temp[0] == "Basketball") t = new Blue_3.BasketballPlayer(tempNameSurname[0], tempNameSurname[1]);
            else if (temp[0] == "Hockey") t = new Blue_3.HockeyPlayer(tempNameSurname[0], tempNameSurname[1]);
            else if (temp[0] == "Participant") t = new Blue_3.Participant(tempNameSurname[0], tempNameSurname[1]);
            else
            {
                return null;
            }
            if (temp.Length > 2 && t != null)
            {
                var tempPenalties = temp[2].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var penalty in tempPenalties)
                {
                    if (int.TryParse(penalty, out int penaltyValue))
                    {
                        t.PlayMatch(penaltyValue);
                    }
                }
            }
            return (T)t;
        }

        //Blue_4
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            string text = participant.Name + Environment.NewLine;
            if (participant.ManTeams != null)
            {
                for (int i = 0; i < participant.ManTeams.Length; i++)
                {
                    if (participant.ManTeams[i] == null) continue;
                    text += $"{participant.ManTeams[i].Name} ";
                    for (int j = 0; j < participant.ManTeams[i].Scores.Length; j++)
                    {
                        text += $"{participant.ManTeams[i].Scores[j]} ";
                    }
                    text += "man";
                }
            }
            text += Environment.NewLine;
            if (participant.WomanTeams != null)
            {
                for (int i = 0; i < participant.WomanTeams.Length; i++)
                {
                    if (participant.WomanTeams[i] == null) continue;
                    text += $"{participant.WomanTeams[i].Name} ";
                    for (int j = 0; j < participant.WomanTeams[i].Scores.Length; j++)
                    {
                        text += $"{participant.WomanTeams[i].Scores[j]} ";
                    }
                    text += "woman";
                }
            }
            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(text)) return null;
            var temp = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length < 1) return null;  
            Blue_4.Group result = new Blue_4.Group(temp[0]);            
            if (temp.Length > 1)
            {
                string[] manTeams = temp[1].Split(new[] { "man" }, StringSplitOptions.RemoveEmptyEntries);
                Blue_4.ManTeam[] ManTeams = new Blue_4.ManTeam[manTeams.Length];
                for (int i = 0; i < ManTeams.Length; i++)
                {
                    var tempNameScores = manTeams[i].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tempNameScores.Length == 0) continue;  
                    ManTeams[i] = new Blue_4.ManTeam(tempNameScores[0]);
                    for (int j = 1; j < tempNameScores.Length; j++)
                    {
                        ManTeams[i].PlayMatch(Int32.Parse(tempNameScores[j]));
                    }
                }
                result.Add(ManTeams);
            }
            if (temp.Length > 2)
            {
                string[] womanTeams = temp[2].Split(new[] { "woman" }, StringSplitOptions.RemoveEmptyEntries);
                Blue_4.WomanTeam[] WomanTeams = new Blue_4.WomanTeam[womanTeams.Length];
                for (int i = 0; i < WomanTeams.Length; i++)
                {
                    var tempNameScores = womanTeams[i].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tempNameScores.Length == 0) continue;
                    WomanTeams[i] = new Blue_4.WomanTeam(tempNameScores[0]);
                    for (int j = 1; j < tempNameScores.Length; j++)
                    {
                        WomanTeams[i].PlayMatch(Int32.Parse(tempNameScores[j]));
                    }
                }
                result.Add(WomanTeams);
            }
            return result;
        }
        
        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;

            string gender = group switch
            {
                Blue_5.ManTeam _ => "man",
                Blue_5.WomanTeam _ => "woman",
                _ => null
            };

            if (gender == null) return;

            StringBuilder textBuilder = new StringBuilder();
            textBuilder.AppendLine(gender);
            textBuilder.AppendLine(group.Name);

            if (group.Sportsmen?.Length > 0)
            {
                foreach (var sportsman in group.Sportsmen)
                {
                    if (sportsman == null) continue;
                    textBuilder.AppendLine($"{sportsman.Name} {sportsman.Surname} {sportsman.Place}");
                }
            }

            SelectFile(fileName);
            File.WriteAllText(FilePath, textBuilder.ToString());
        }
        public override T DeserializeBlue5Team<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return default;

            SelectFile(fileName);
            string content = File.ReadAllText(FilePath);
            if (string.IsNullOrWhiteSpace(content)) return default;

            string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2) return default;

            Blue_5.Team team = lines[0] switch
            {
                "man" => new Blue_5.ManTeam(lines[1]),
                "woman" => new Blue_5.WomanTeam(lines[1]),
                _ => null
            };

            if (team == null) return default;

            T result = team as T;
            if (result == null || lines.Length == 2) return result;

            for (int i = 2; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2) continue;

                Blue_5.Sportsman athlete = new(parts[0], parts[1]);

                if (parts.Length >= 3)
                {
                    for (int j = 2; j < parts.Length; j++)
                    {
                        if (int.TryParse(parts[j], out int place))
                            athlete.SetPlace(place);
                    }
                }

                result.Add(athlete);
            }

            return result;
        }
    }
}