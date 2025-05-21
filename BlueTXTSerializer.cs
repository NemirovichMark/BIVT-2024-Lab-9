using Lab_7;
using System;
using System.IO;
using System.Text;

using static Lab_7.Blue_2;
using static Lab_7.Blue_3;
using static Lab_7.Blue_4;
using static Lab_7.Blue_5;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension => "txt";

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (!(participant != null && fileName != null && fileName != ""))
                return;

            string dataToWrite = "";
            var human = participant as Blue_1.HumanResponse;
            if (human != null)
            {
                dataToWrite += "HumanResponse" + "|" + human.Name + "|" + human.Surname + "|" + human.Votes;
            }
            else
            {
                dataToWrite += "Response" + "|" + participant.Name + "|" + participant.Votes;
            }

            SelectFile(fileName);
            File.WriteAllText(fileName, dataToWrite);
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || fileName == null || fileName.Length == 0)
                return;

            SelectFile(fileName);
            StringBuilder result = new StringBuilder();
            result.AppendLine(participant.GetType().Name + "|" + participant.Name + "|" + participant.Bank);

            int i = 0;
            while (i < participant.Participants.Length)
            {
                var p = participant.Participants[i];
                //if (p != null)
                //{
                    string jumpMarks = "";
                    int a = 0;
                    while (a < 2)
                    {
                        if (a > 0) jumpMarks += ";";
                        int b = 0;
                        while (b < 5)
                        {
                            if (b > 0) jumpMarks += ",";
                            jumpMarks += p.Marks[a, b];
                            b++;
                        }
                        a++;
                    }
                    result.AppendLine(p.Name + "|" + p.Surname + "|" + jumpMarks);
                
                i++;
            }

            File.WriteAllText(fileName, result.ToString());
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || fileName == null || fileName == "") return;

            SelectFile(fileName);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(student.Name + "|" + student.Surname + "|" + student.GetType().Name + "|" + student.Penalties.Length + "|" + string.Join(",", student.Penalties));

            File.WriteAllText(fileName, sb.ToString());
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || fileName == null || fileName.Length == 0) return;

            SelectFile(fileName);
            StreamWriter writer = new StreamWriter(fileName);

            writer.WriteLine("Name:" + participant.Name);
            writer.WriteLine("ManTeamsCount:" + participant.ManTeams.Length);

            int i = 0;
            while (i < participant.ManTeams.Length)
            {
                var mt = participant.ManTeams[i];
                if (mt != null)
                {
                    writer.WriteLine(mt.Name + "|" + string.Join(",", mt.Scores));
                }
                i++;
            }

            writer.WriteLine("WomanTeamsCount:" + participant.WomanTeams.Length);

            int j = 0;
            while (j < participant.WomanTeams.Length)
            {
                var wt = participant.WomanTeams[j];
                if (wt != null)
                {
                    writer.WriteLine(wt.Name + "|" + string.Join(",", wt.Scores));
                }
                j++;
            }

            writer.Close();
        }

        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || fileName == null || fileName == "") return;

            SelectFile(fileName);
            var writer = new StreamWriter(fileName);

            writer.WriteLine("Name=" + group.Name);
            writer.WriteLine("Type=" + group.GetType().Name);

            int count = 0;
            int i = 0;
            while (i < group.Sportsmen.Length)
            {
                if (group.Sportsmen[i] != null) count++;
                i++;
            }
            writer.WriteLine("Count=" + count);

            int j = 0;
            while (j < group.Sportsmen.Length)
            {
                var s = group.Sportsmen[j];
                if (s != null)
                {
                    writer.WriteLine(s.Name + "|" + s.Surname + "|" + s.Place);
                }
                j++;
            }

            writer.Close();
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (fileName == null || fileName == "" || !File.Exists(fileName)) return null;

            string txt = File.ReadAllText(fileName);
            string[] parts = txt.Split('|');

            if (parts.Length >= 4 && parts[0] == "HumanResponse")
            {
                int v;
                if (int.TryParse(parts[3], out v))
                    return new Blue_1.HumanResponse(parts[1], parts[2], v);
                return new Blue_1.HumanResponse(parts[1], parts[2]);
            }
            else if (parts.Length >= 3)
            {
                int v;
                if (int.TryParse(parts[2], out v))
                    return new Blue_1.Response(parts[1], v);
                return new Blue_1.Response(parts[1]);
            }

            return null;
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (fileName == null || fileName == "" || !File.Exists(fileName)) return null;

            var lines = File.ReadAllLines(fileName);
            if (lines.Length == 0) return null;

            string[] header = lines[0].Split('|');
            if (header.Length < 3) return null;

            Blue_2.WaterJump jump;
            if (header[0] == "WaterJump5m")
                jump = new Blue_2.WaterJump5m(header[1], int.Parse(header[2]));
            else
                jump = new Blue_2.WaterJump3m(header[1], int.Parse(header[2]));

            int i = 1;
            while (i < lines.Length)
            {
                var parts = lines[i].Split('|');
                if (parts.Length >= 3)
                {
                    var person = new Blue_2.Participant(parts[0], parts[1]);
                    var jumps = parts[2].Split(';');
                    int j = 0;
                    while (j < jumps.Length && j < 2)
                    {
                        var marks = jumps[j].Split(',');
                        if (marks.Length == 5)
                        {
                            int[] converted = new int[5];
                            int k = 0;
                            while (k < 5)
                            {
                                converted[k] = int.Parse(marks[k]);
                                k++;
                            }
                            person.Jump(converted);
                        }
                        j++;
                    }
                    jump.Add(person);
                }
                i++;
            }

            return jump;
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) return default(T);

            string[] lines = File.ReadAllLines(fileName);
            if (lines.Length == 0) return default(T);

            string[] parts = lines[0].Split('|');
            if (parts.Length < 5) return default(T);

            string name = parts[0];
            string surname = parts[1];
            string type = parts[2];
            string penaltiesStr = parts[4];

            Blue_3.Participant obj = new Blue_3.Participant(name, surname);
            if (type == "BasketballPlayer")
                obj = new Blue_3.BasketballPlayer(name, surname);
            else if (type == "HockeyPlayer")
                obj = new Blue_3.HockeyPlayer(name, surname);

            var penalties = penaltiesStr.Split(',');
            int i = 0;
            while (i < penalties.Length)
            {
                int p;
                if (int.TryParse(penalties[i], out p))
                {
                    obj.PlayMatch(p);
                }
                i++;
            }

            return (T)(object)obj;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (fileName == null || fileName == "" || !File.Exists(fileName)) return null;

            string[] lines = File.ReadAllLines(fileName);
            string name = lines[0].Replace("Name:", "").Trim();
            var group = new Blue_4.Group(name);

            int i = 1;
            bool manSection = false, womanSection = false;
            int mIndex = 0, wIndex = 0;
            Blue_4.ManTeam[] men = null;
            Blue_4.WomanTeam[] women = null;

            while (i < lines.Length)
            {
                var line = lines[i];

                if (line.StartsWith("ManTeamsCount:"))
                {
                    manSection = true;
                    womanSection = false;
                    men = new Blue_4.ManTeam[int.Parse(line.Split(':')[1])];
                    i++;
                    continue;
                }
                if (line.StartsWith("WomanTeamsCount:"))
                {
                    womanSection = true;
                    manSection = false;
                    women = new Blue_4.WomanTeam[int.Parse(line.Split(':')[1])];
                    i++;
                    continue;
                }

                var data = line.Split('|');
                string tName = data[0];
                string[] scores = data[1].Split(',');

                if (manSection)
                {
                    var team = new Blue_4.ManTeam(tName);
                    foreach (var s in scores)
                    {
                        team.PlayMatch(int.Parse(s));
                    }
                    men[mIndex++] = team;
                }
                else if (womanSection)
                {
                    var team = new Blue_4.WomanTeam(tName);
                    foreach (var s in scores)
                    {
                        team.PlayMatch(int.Parse(s));
                    }
                    women[wIndex++] = team;
                }

                i++;
            }

            group.Add(men);
            group.Add(women);
            return group;
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            if (fileName == null || fileName == "" || !File.Exists(fileName)) return default(T);

            var lines = File.ReadAllLines(fileName);
            string name = lines[0].Split('=')[1];
            string type = lines[1].Split('=')[1];
            int count = int.Parse(lines[2].Split('=')[1]);

            Blue_5.Sportsman[] list = new Blue_5.Sportsman[count];
            int i = 3, index = 0;
            while (i < lines.Length && index < count)
            {
                string[] parts = lines[i].Split('|');
                var s = new Blue_5.Sportsman(parts[0], parts[1]);
                s.SetPlace(int.Parse(parts[2]));
                list[index++] = s;
                i++;
            }

            Blue_5.Team team = type == "ManTeam" ? new Blue_5.ManTeam(name) : new Blue_5.WomanTeam(name);
            team.Add(list);
            return (T)team;
        }
    }
}