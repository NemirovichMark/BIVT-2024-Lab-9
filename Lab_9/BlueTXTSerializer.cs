using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_7;
using System.IO;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension => "txt";

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            string t;
            var pers = participant as Blue_1.HumanResponse;
            if (pers == null)
                t = $"{participant.Name} {participant.Votes}";
            else
                t = $"{pers.Name} {pers.Surname} {pers.Votes}";

            SelectFile(fileName);
            File.WriteAllText(FilePath, t);
        }
        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            string t = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(t)) return null; 
            Blue_1.Response res;
            var ws = t.Split(' ');
            if (ws.Length == 2)
            {
                res = new Blue_1.Response(ws[0], int.Parse(ws[1]));
            }
            else
            {
                res = new Blue_1.HumanResponse(ws[0], ws[1], int.Parse(ws[2]));
            }
            return res;
        }
        
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            string t;
            if (!(participant is Blue_2.WaterJump5m))
                t = "3 ";
            else
                t = "5 ";
            t += $"{participant.Name} {participant.Bank}";
            foreach (var p in participant.Participants)
            {
                t += $"{Environment.NewLine}{p.Name} {p.Surname}";
                t += $"{Environment.NewLine}";
                for(int kjump = 0; kjump < 2; kjump++)
                {
                    for(int i = 0; i < 5; i++)
                    {
                        t += $" {p.Marks[kjump, i]}";
                    }
                    t += $"{Environment.NewLine}";
                }
            }

            SelectFile(fileName);
            File.WriteAllText(FilePath, t);
        }
        private Blue_2.WaterJump CreateJumpInstance(string heightType, string jumpName, string bankValue)
        {
            if (heightType == null || jumpName == null || bankValue == null) return null;
            int bank = 0;
            try
            {
                bank = int.Parse(bankValue);
            }
            catch
            {
        
            }
            if (heightType == "3")
            {
                return new Blue_2.WaterJump3m(jumpName, bank);
            }
            else if (heightType == "5")
            {
                return new Blue_2.WaterJump5m(jumpName, bank);
            }
            else
            {
                return null;
            }
        }
        private int[] GetMarks(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;

            string[] ps = input.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            int[] res = new int[ps.Length];
            int k = 0;

            foreach (var p in ps)
            {
                if (int.TryParse(p, out int number))
                {
                    res[k++] = number;
                }
            }

            if (k < res.Length)
            {
                Array.Resize(ref res, k);
            }

            return res;
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string file)
        {
            SelectFile(file);
            string[] lines = File.ReadAllLines(FilePath);
            if (lines.Length == 0) return null;

            string[] head = lines[0].Split(' ');
            if (head.Length < 3) return null;

            Blue_2.WaterJump jump = CreateJumpInstance(head[0], head[1], head[2]);
            if (jump == null) return null;

            for (int i = 1; i < lines.Length; i += 4)
            {
                if (i + 2 >= lines.Length) break;
                string[] name = lines[i].Split(' ');
                if (name.Length < 2) continue;
                var p = new Blue_2.Participant(name[0], name[1]);
                int[] a1 = GetMarks(lines[i + 1]);
                int[] a2 = GetMarks(lines[i + 2]);
                if (a1 != null && a1.Length > 0) p.Jump(a1);
                if (a2 != null && a2.Length > 0) p.Jump(a2);
                jump.Add(p);
            }

            return jump;
        }

        public override void SerializeBlue3Participant<T>(T participant, string fileName)
        {
            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine(participant.GetType().Name);
                writer.WriteLine(participant.Name);
                writer.WriteLine(participant.Surname);
                int[] pen = participant.Penalties;
                writer.WriteLine(pen.Length);
                foreach (int el in pen)
                {
                    writer.WriteLine(el);
                }
            }
        }
        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(fileName))
            {
                string typeN = reader.ReadLine();
                string n = reader.ReadLine();
                string s = reader.ReadLine();

                Blue_3.Participant participant = typeN switch
                {
                    "BasketballPlayer" => new Blue_3.BasketballPlayer(n, s),
                    "HockeyPlayer" => new Blue_3.HockeyPlayer(n, s),
                    "Participant" => new Blue_3.Participant(n, s)

                };

                int penCount = int.Parse(reader.ReadLine());
                for (int i = 0; i < penCount; i++)
                {
                    participant.PlayMatch(int.Parse(reader.ReadLine()));
                }

                return (T)participant;
            }
        }
    
        private void InfoAboutTeamsWrite(Blue_4.Team[] ts, ref string text)
        {
            string[] sTs = new string[ts.Length];
            for(int i = 0; i < ts.Length; i++)
            {
                sTs[i] = InfoAboutTeams(ts[i]);
            }
            text += String.Join($"{Environment.NewLine}#{Environment.NewLine}", sTs);
        }
        private Blue_4.Team TeamGetIt(string name, string typeofteam)
        {
            switch(typeofteam)
            {
                case "Man":
                    var mT = new Blue_4.ManTeam(name);
                    return mT;
                case "Woman":
                    var wT = new Blue_4.WomanTeam(name);
                    return wT;
                default:
                    return null;
            }
        }
        private string InfoAboutTeams(Blue_4.Team t)
        {
            if (t == null) return "";

            string text =  t.Name + Environment.NewLine ;
            foreach (int s in t.Scores)
                text += $"{s} ";
            return text;
        }
        private void AddTs(string[] teams, string type, Blue_4.Group group)
        {
            foreach (string sTeam in teams)
            {
                if (String.IsNullOrWhiteSpace(sTeam)) continue;

                var teamInfo = sTeam.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                var team = TeamGetIt(teamInfo[0].Replace(Environment.NewLine, ""), type);
                int[] scores = GetMarks(teamInfo[1]);
                foreach (int s in scores)
                    team.PlayMatch(s);

                group.Add(team);
            }
        }

         public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            string t = participant.Name + Environment.NewLine + "&" + Environment.NewLine;
            InfoAboutTeamsWrite(participant.ManTeams, ref t);
            t += Environment.NewLine + "^" + Environment.NewLine;
            InfoAboutTeamsWrite(participant.WomanTeams, ref t);

            SelectFile(fileName);
            File.WriteAllText(FilePath, t);
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);
            string t = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(t)) return null;
            string[] inf = t.Split("&", StringSplitOptions.RemoveEmptyEntries);
            var grp = new Blue_4.Group(inf[0].Replace(Environment.NewLine, ""));
            if(inf.Length == 1) return grp;
            string[] ts = inf[1].Split("^");
            string[] manTs = ts[0].Split("#", StringSplitOptions.RemoveEmptyEntries);
            string[] womanTs = ts[1].Split("#", StringSplitOptions.RemoveEmptyEntries);
            AddTs(manTs, "Man", grp);
            AddTs(womanTs, "Woman", grp);

            return grp;
        }

        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            SelectFile(fileName);
            if (group == null || string.IsNullOrEmpty(fileName)) return;

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine($"Type: {group.GetType().Name}");
                writer.WriteLine($"Name: {group.Name}");
                foreach (var sportsman in group.Sportsmen)
                {
                    if (sportsman != null)
                    {
                        writer.WriteLine($"Name: {sportsman.Name}; Surname: {sportsman.Surname}; Place: {sportsman.Place}");
                    }
                }
            }
        }

        public override T DeserializeBlue5Team<T>(string fileName) //where T : class
        {
            SelectFile(fileName);
            using (var reader = new StreamReader(FilePath))
            {
                string tstr = reader.ReadLine();
                string grpstr = reader.ReadLine();
                string tname = tstr.Substring("Type:".Length).Trim();
                string grpname = grpstr.Substring("Name:".Length).Trim();
                Blue_5.Team t = null;
                if (tname == "ManTeam") t = new Blue_5.ManTeam(grpname);
                if (tname == "WomanTeam") t = new Blue_5.WomanTeam(grpname);
                string l;
                while ((l = reader.ReadLine()) != null)
                {
                    string[] ps = l.Split(';');
                    if (ps.Length < 3) continue;
                    string n = ps[0].Split(':')[1].Trim();
                    string sur = ps[1].Split(':')[1].Trim();
                    int place = 0;
                    int.TryParse(ps[2].Split(':')[1].Trim(), out place);
                    var sportsman = new Blue_5.Sportsman(n, sur);
                    sportsman.SetPlace(place);
                    t.Add(sportsman);
                }
                return (T)t;
            }

        }
    }
}