using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_7;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var type = text.Split('\n')[0].Trim();
            if (type == "Participant")
                return (T) (Object) ParticipantFromString(text);
            if (type == "Judge")
                return (T) (Object) JudgeFromString(text);
            if (type == "Competition")
                return (T) (Object) CompetitionFromString(text);

            return null;
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var type = text.Split('\n')[0].Trim();
            var vals = text.Split('\n');
            Purple_2.SkiJumping jumping;
            if (type == "JuniorSkiJumping") jumping = new Purple_2.JuniorSkiJumping();
            else if (type == "ProSkiJumping") jumping = new Purple_2.ProSkiJumping();
            else return null;
            int pCount = int.Parse(vals[3].Trim().Split(':')[1]);
            IEnumerable<Purple_2.Participant> participants = [];
            for (int i = 0; i < pCount; i++)
            {
                int ind = 4 + i * 4;
                Dictionary<string, string> dict = [];
                for (int j = ind; j < ind + 4; j++) dict[vals[j].Split(':')[0].Trim()] = vals[j].Split(':')[1].Trim();
                var p = new Purple_2.Participant(dict["name"], dict["surname"]);
                p.Jump(int.Parse(dict["distance"]), dict["marks"].Split(' ').Select(x => int.Parse(x)).ToArray(), jumping.Standard);
                participants = participants.Append(p);
            }
            jumping.Add(participants.ToArray());
            return (T) jumping;
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var type = text.Split('\n')[0].Trim();
            var vals = text.Split('\n');
            double[] moods = vals[1].Split(':')[1].Trim().Split(' ').Select(x => double.Parse(x)).ToArray();
            Purple_3.Skating skating;
            if (type == "IceSkating") skating = new Purple_3.IceSkating(moods, false);
            else if (type == "FigureSkating") skating = new Purple_3.FigureSkating(moods, false);
            else return null;
            int pCount = int.Parse(vals[2].Trim().Split(':')[1]);
            IEnumerable<Purple_3.Participant> participants = [];
            for (int i = 0; i < pCount; i++)
            {
                int ind = 3 + i * 4;
                Dictionary<string, string> dict = [];
                for (int j = ind; j < ind + 4; j++) dict[vals[j].Split(':')[0].Trim()] = vals[j].Split(':')[1].Trim();
                var p = new Purple_3.Participant(dict["name"], dict["surname"]);
                var results = dict["marks"].Trim().Split(' ').Select(x => double.Parse(x)).ToArray();
                for (int j = 0; j < results.Length; j++) p.Evaluate(results[j]);
                participants = participants.Append(p);
            }
            skating.Add(participants.ToArray());
            Purple_3.Participant.SetPlaces(skating.Participants);
            return (T) skating;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var vals = text.Split('\n');
            var group = new Purple_4.Group(vals[0].Split(':')[1].Trim());
            int pCount = int.Parse(vals[1].Split(':')[1].Trim());
            for (int i = 0; i < pCount; i++)
            {
                int ind = 2 + i * 4;
                Dictionary<string, string> dict = [];
                for (int j = ind; j < ind + 4; j++) dict[vals[j].Split(':')[0].Trim()] = vals[j].Split(':')[1].Trim();
                Purple_4.Sportsman p;
                if (dict["type"] == "SkiMan") p = new Purple_4.SkiMan(dict["name"], dict["surname"]);
                else if (dict["type"] == "SkiWoman") p = new Purple_4.SkiWoman(dict["name"], dict["surname"]);
                else p = new Purple_4.Sportsman(dict["name"], dict["surname"]);
                p.Run(double.Parse(dict["time"]));
                group.Add(p);
            }

            return group;
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var vals = text.Split('\n');
            var report = new Purple_5.Report();
            int rCount = int.Parse(vals[0].Trim().Split(':')[1]);
            int ind = 1;
            for (int i = 0; i < rCount; i++)
            {
                var research = new Purple_5.Research(vals[ind++].Split(':')[1].Trim());
                int ansCount = int.Parse(vals[ind++].Trim().Split(':')[1]);
                for (int j = 0; j < ansCount; j++)
                {
                    Dictionary<string, string?> dict = [];
                    for (int k = 0; k < 3; k++)
                    {
                        var f = vals[ind].Split(':')[0].Trim();
                        var v = vals[ind].Split(':')[1].Trim();
                        dict[f] = v == "" ? null : v;
                        ind++;
                    }
                    research.Add([dict["animal"], dict["characterTrait"], dict["concept"]]);
                }
                report.AddResearch(research);
            }

            return report;
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            string text = "";
            if (obj is Purple_1.Participant p)
                text = ObjToString(p);
            if (obj is Purple_1.Judge j)
                text = ObjToString(j);
            if (obj is Purple_1.Competition c)
                text = ObjToString(c);

            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            if (jumping is Purple_2.JuniorSkiJumping) sb.AppendLine("JuniorSkiJumping");
            else if (jumping is Purple_2.ProSkiJumping) sb.AppendLine("ProSkiJumping");
            else sb.AppendLine("UnknownSkiJumping");
            Dictionary<string, string> dict = [];
            dict["name"] = jumping.Name;
            dict["standart"] = jumping.Standard.ToString();
            dict["participants"] = jumping.Participants.Length.ToString();
            sb.Append(DictToString(dict));
            foreach (var p in jumping.Participants)
            {
                Dictionary<string, string> pDict = [];
                pDict["name"] = p.Name;
                pDict["surname"] = p.Surname;
                pDict["distance"] = p.Distance.ToString();
                pDict["marks"] = String.Join(" ", p.Marks);
                sb.Append(DictToString(pDict));
            }

            SelectFile(fileName);
            File.WriteAllText(FilePath, sb.ToString());
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            if (skating is Purple_3.FigureSkating) sb.AppendLine("FigureSkating");
            else if (skating is Purple_3.IceSkating) sb.AppendLine("IceSkating");
            else sb.AppendLine("UnknownSkating");
            sb.AppendLine("moods:" + String.Join(' ', skating.Moods));
            sb.AppendLine("participants:" + skating.Participants.Length);
            foreach (var p in skating.Participants)
            {
                Dictionary<string, string> dict = [];
                dict["name"] = p.Name;
                dict["surname"] = p.Surname;
                dict["marks"] = String.Join(' ', p.Marks);
                dict["places"] = String.Join(' ', p.Places);
                sb.Append(DictToString(dict));
            }

            SelectFile(fileName);
            File.WriteAllText(FilePath, sb.ToString());
        }

        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("name:" + participant.Name);
            sb.AppendLine("participants:" + participant.Sportsmen.Length);
            foreach (var p in participant.Sportsmen)
            {
                Dictionary<string, string> dict = [];
                string type = "unknown";
                if (p is Purple_4.SkiMan) type = "SkiMan";
                else if (p is Purple_4.SkiWoman) type = "SkiWoman";
                dict["type"] = type;
                dict["name"] = p.Name;
                dict["surname"] = p.Surname;
                dict["time"] = p.Time.ToString();
                sb.Append(DictToString(dict));
            }

            SelectFile(fileName);
            File.WriteAllText(FilePath, sb.ToString());
        }

        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("researches:" + group.Researches.Length);
            foreach (var researches in group.Researches)
            {
                Dictionary<string, string> dict = [];
                dict["name"] = researches.Name;
                dict["responses"] = researches.Responses.Length.ToString();
                sb.Append(DictToString(dict));
                foreach (var response in researches.Responses)
                {
                    Dictionary<string, string> dict2 = [];
                    dict2["animal"] = response.Animal;
                    dict2["characterTrait"] = response.CharacterTrait;
                    dict2["concept"] = response.Concept;
                    sb.Append(DictToString(dict2));
                }
            }

            SelectFile(fileName);
            File.WriteAllText(FilePath, sb.ToString());
        }

        private static string DictToString(Dictionary<string, string> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var k in dict.Keys)
            {
                sb.Append(k);
                sb.Append(':');
                sb.AppendLine(dict[k]);
            }
            return sb.ToString();
        }
        private static string ObjToString(Purple_1.Participant p)
        {
            Dictionary<string, string> dict = [];
            dict["name"] = p.Name;
            dict["surname"] = p.Surname;
            dict["coefs"] = String.Join(" ", p.Coefs);
            IEnumerable<int> marks = [];
            foreach (var m in p.Marks) marks = marks.Append(m);
            dict["marks"] = String.Join(" ", marks.ToArray());
            dict["total_score"] = p.TotalScore.ToString();
            return p.GetType().Name + "\n" + DictToString(dict);
        }

        private static Purple_1.Participant ParticipantFromString(string s)
        {
            var vals = s.Trim().Split('\n');
            Dictionary<string, string> dict = [];
            for (int i = 1; i < vals.Length; i++)
            {
                var val = vals[i];
                dict[val.Split(':')[0].Trim()] = val.Split(':')[1].Trim();
            }
            var p = new Purple_1.Participant(dict["name"], dict["surname"]);
            p.SetCriterias(dict["coefs"].Split(' ').Select(x => double.Parse(x)).ToArray());
            var matrix = dict["marks"].Split(' ').Select(x => int.Parse(x)).ToArray();
            int ind = 0;
            for (int i = 0; i < 4; i++)
            {
                IEnumerable<int> marks = [];
                for (int j = 0; j < 7; j++) marks = marks.Append(matrix[ind++]);
                p.Jump(marks.ToArray());
            }
            return p;
        }

        private static string ObjToString(Purple_1.Judge p)
        {
            Dictionary<string, string> dict = [];
            dict["name"] = p.Name;
            dict["marks"] = String.Join(" ", p.Marks);
            return p.GetType().Name + "\n" + DictToString(dict);
        }

        private static Purple_1.Judge JudgeFromString(string s)
        {
            var vals = s.Trim().Split('\n');
            Dictionary<string, string> dict = [];
            for (int i = 1; i < vals.Length; i++)
            {
                var val = vals[i];
                dict[val.Split(':')[0].Trim()] = val.Split(':')[1].Trim();
            }
            var j = new Purple_1.Judge(dict["name"], dict["marks"].Split(' ').Select(x => int.Parse(x)).ToArray());
            return j;
        }

        private static string ObjToString(Purple_1.Competition p)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(p.GetType().Name);
            sb.AppendLine("Judjes:" + p.Judges.Length);
            foreach (var j in p.Judges) sb.Append(ObjToString(j));

            sb.AppendLine("Participants:" + p.Participants.Length);
            foreach (var x in p.Participants) sb.Append(ObjToString(x));
            return sb.ToString();
        }

        private static Purple_1.Competition CompetitionFromString(string s)
        {
            var vals = s.Trim().Split('\n');
            int jCount = int.Parse(vals[1].Split(':')[1].Trim());
            IEnumerable<Purple_1.Judge> judges = [];
            int ind = 2;
            for (int i = 0; i < jCount; i++)
            {
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < 3; j++)
                    sb.AppendLine(vals[ind++].Trim());
                judges = judges.Append(JudgeFromString(sb.ToString()));
            }
            int pCount = int.Parse(vals[ind++].Split(':')[1].Trim());
            IEnumerable<Purple_1.Participant> participants = [];
            for (int i = 0; i < pCount; i++)
            {
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < 6; j++)
                    sb.AppendLine(vals[ind++].Trim());
                participants = participants.Append(ParticipantFromString(sb.ToString()));
            }
            var c = new Purple_1.Competition(judges.ToArray());
            c.Add(participants.ToArray());
            return c;
        }
    }
}
