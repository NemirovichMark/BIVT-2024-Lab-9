using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_7;
using System.Runtime.CompilerServices;


namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension => "txt";
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            File.WriteAllText(FilePath, "");
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                if (participant is Blue_1.HumanResponse human)
                {
                    writer.WriteLine($"{human.Name} {human.Surname} {human.Votes}");
                }
                else
                {
                    writer.WriteLine($"{participant.Name} {participant.Votes}");
                }
            }
        }


        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            File.WriteAllText(FilePath, "");
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.Write($"{participant.GetType().Name} "); 
                writer.Write($"{participant.Name} ");
                writer.Write($"{participant.Bank}");
                writer.Write(Environment.NewLine);
                foreach (var p in participant.Participants)
                { 
                    writer.Write($"{p.Name} ");
                    writer.Write($"{p.Surname} ");
                    writer.Write(Environment.NewLine);
                    for (int jump = 0; jump < 2; jump++)
                    {
                        string marksLine = "";
                        for (int i = 0; i < 5; i++)
                        {
                            marksLine += p.Marks[jump, i];
                            if (i < 4)
                                marksLine += " ";
                        }
                        writer.WriteLine(marksLine);
                    }
                }

            }
        }


        public override void SerializeBlue3Participant<T>(T student, string fileName) where T : class
        {
            if (student == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            File.WriteAllText(FilePath, "");
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine($"{student.GetType().Name} {student.Name} {student.Surname}");
                if (student is Blue_3.Participant p && p.Penalties != null && p.Penalties.Length > 0)
                {
                    writer.WriteLine(string.Join(" ", p.Penalties));
                }
            }
        }


        private void AddTeam(Blue_4.Team team, ref string textteam, string teamType)
        {
            textteam += team.Name + " [";
            if (team.Scores != null)
            {
                foreach (int score in team.Scores)
                {
                    textteam += score + ", ";
                }
                textteam = textteam.Remove(textteam.Length - 2);
            }
            textteam += "]; ";
        }
        private void SerializeSeparately(Blue_4.Group group, string teamType, StreamWriter writer)
        {
            writer.Write(teamType + ": ");
            string text = "(";
            if (teamType == "ManTeams")
            {
                foreach (var team in group.ManTeams)
                {
                    if (team != null)
                    {
                        AddTeam(team, ref text, "man");
                    }
                }
            }
            else if (teamType == "WomanTeams")
            {
                foreach (var team in group.WomanTeams)
                {
                    if (team != null)
                    {
                        AddTeam(team, ref text, "woman");
                    }
                }
            }
            if (text.EndsWith(" "))
            {
                text = text.TrimEnd();
            }
            text += ")";
            writer.WriteLine($"{text}");
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            File.WriteAllText(FilePath, "");
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine($"{participant.Name}");
                SerializeSeparately(participant, "ManTeams", writer);
                SerializeSeparately(participant, "WomanTeams", writer);
            }
        }


        private void SerializeSportsmen(Blue_5.Team group, StreamWriter writer)
        {
            string textgroup = "";
            foreach (var sportsman in group.Sportsmen)
            {
                if (sportsman != null)
                {
                    textgroup += $"{sportsman.Name} ";
                    textgroup += $"{sportsman.Surname} ";
                    textgroup += $"{sportsman.Place}";
                    textgroup += Environment.NewLine;
                }
            }
            writer.Write(textgroup);
        }
        public override void SerializeBlue5Team<T>(T group, string fileName) where T : class
        {
            if (group == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            File.WriteAllText(FilePath, "");
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine($"{group.GetType().Name} {((Blue_5.Team)(object)group).Name}");
                SerializeSportsmen((Blue_5.Team)(object)group, writer);
            }
        }


        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            string[] lines = File.ReadAllLines(FilePath);

            if (lines.Length == 0) return new Blue_1.Response("", 0);

            string[] parts = lines[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                string name = parts[0];
                int votes = int.TryParse(parts[1], out var v) ? v : 0;
                return new Blue_1.Response(name, votes);
            }
            else if (parts.Length == 3)
            {
                string name = parts[0];
                string surname = parts[1];
                int votes = int.TryParse(parts[2], out var v) ? v : 0;
                return new Blue_1.HumanResponse(name, surname, votes);
            }

            return new Blue_1.Response("", 0); 
        }


        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            string[] lines = File.ReadAllLines(FilePath);
            if (lines.Length < 1) return null;

            // Разбираем первую строку заголовка
            string[] firstline = lines[0].Split(' ');
            if (firstline.Length < 3) return null;

            string type = firstline[0];
            string name = firstline[1];
            if (!int.TryParse(firstline[2], out int bank)) return null;

            Blue_2.WaterJump waterjump;
            if (type == "WaterJump3m") waterjump = new Blue_2.WaterJump3m(name, bank);
            else if (type == "WaterJump5m") waterjump = new Blue_2.WaterJump5m(name, bank);
            else return null;

            for (int i = 1; i < lines.Length; i += 3)
            {
                
                string[] participantdata = lines[i].Split(' ');
                string participantName = participantdata[0];
                string participantSurname = participantdata[1];

                int[] marks1 = ParseMarksLine(lines[i + 1]);
                int[] marks2 = ParseMarksLine(lines[i + 2]);
                if (marks1.Length != 5 || marks2.Length != 5) continue;

                var participant = new Blue_2.Participant(participantName, participantSurname);
                participant.Jump(marks1);
                participant.Jump(marks2);

                waterjump.Add(participant);
            }
            return waterjump;
        }
        private int[] ParseMarksLine(string line)
        {
            string[] parts = line.Split(' ');
            int[] marks = new int[5];
            int count = 0;

            foreach (string part in parts)
            {
                if (count == 5) 
                    break;

                if (int.TryParse(part, out int number))
                {
                    marks[count] = number;
                    count++;
                }
            }

            if (count < 5)
            {
                int[] result = new int[count];
                Array.Copy(marks, result, count);
                return result;
            }

            return marks;
        }


        public override T DeserializeBlue3Participant<T>(string fileName) where T : class
        {
            SelectFile(fileName);
            string[] lines = File.ReadAllLines(FilePath);
            if (lines.Length == 0)
            {
                return null;
            }
            string line = lines[0];
            if (string.IsNullOrWhiteSpace(line)) return null;
            string[] parts = lines[0].Split(' ');
            if (parts.Length < 3)
            {
                return null;
            }

            string participantType = parts[0];
            string participantName = parts[1];
            string participantSurname = parts[2];


            var participant = default(Blue_3.Participant);

            if (participantType == "BasketballPlayer")
            {
                participant = new Blue_3.BasketballPlayer(participantName, participantSurname);
            }
            else if (participantType == "HockeyPlayer")
            {
                participant = new Blue_3.HockeyPlayer(participantName, participantSurname);
            }
            else if (participantType == "Participant")
            {
                participant = new Blue_3.Participant(participantName, participantSurname);
            }
            
            string[] penaltiesStr = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in penaltiesStr)
            {
                if (int.TryParse(s, out int val))
                {
                    participant.PlayMatch(val);
                }
            }
            return participant as T;
        }


        private void DeserializeTeams(string line, string type, Blue_4.Group group)
        {
            int start = line.IndexOf('('); 
            if (start == -1) return;

            string teamsData = line.Substring(start + 1).TrimEnd(')', ' ');
            string[] teams = teamsData.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string teamStr in teams)
            {
                string trimmed = teamStr.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;

                int Start = trimmed.IndexOf('[');
                int End = trimmed.IndexOf(']');

                if (Start == -1 || End == -1) continue;

                string name = trimmed.Substring(0, Start).Trim();
                string scoresText = trimmed.Substring(Start + 1, End - Start - 1);

                Blue_4.Team team = null;

                if (type == "man")
                {
                    team = new Blue_4.ManTeam(name);
                }
                else if (type == "woman")
                {
                    team = new Blue_4.WomanTeam(name);
                }

                if (!string.IsNullOrWhiteSpace(scoresText))
                {
                    string[] scores = scoresText.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string scoreStr in scores)
                    {
                        if (int.TryParse(scoreStr.Trim(), out int score))
                        {
                            team.PlayMatch(score);
                        }
                    }
                }

                group.Add(team);
            }
        }
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);
            string[] lines = File.ReadAllLines(FilePath);
            if (lines.Length < 3)
            {
                return null;
            }

            string groupName = lines[0].Trim();
            var group = new Blue_4.Group(groupName);

            DeserializeTeams(lines[1], "man", group);
            DeserializeTeams(lines[2], "woman", group);

            return group;
        }


        public override T DeserializeBlue5Team<T>(string fileName) where T : class
        {
            SelectFile(fileName);
            string[] lines = File.ReadAllLines(FilePath);
            if (lines.Length < 1) return null;

            var firststring = lines[0].Split(' ');
            string type = firststring[0];
            string teamName = firststring[1];

            var team = default(Blue_5.Team);

            if (type == "ManTeam")
            {
                team = new Blue_5.ManTeam(teamName);
            }
            else if (type == "WomanTeam")
            {
                team = new Blue_5.WomanTeam(teamName);
            }
            if (team == null) return null;
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 3) continue; 

                string name = parts[0];
                string surname = parts[1];
                if (!int.TryParse(parts[2], out int place)) continue;

                var sportsman = new Blue_5.Sportsman(name, surname);
                sportsman.SetPlace(place);
                team.Add(sportsman);
            }

            return team as T;
        }
    }
}
