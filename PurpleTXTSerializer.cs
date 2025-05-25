using Lab_7;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab_7.Purple_1;
using static Lab_7.Purple_2;
using static Lab_7.Purple_3;
using static Lab_7.Purple_4;
using static System.Net.Mime.MediaTypeNames;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            string text = "";
            if (obj is Purple_1.Participant p) text = SerPar(p);
            if (obj is Purple_1.Judge j) text = SerJudge(j);
            if (obj is Purple_1.Competition c)
            {
                text += "Type: Competition" + Environment.NewLine;
                text += c.Judges.Length.ToString() + Environment.NewLine;
                foreach (var jj in c.Judges) text += SerJudge(jj) + Environment.NewLine;
                text += c.Participants.Length.ToString() + Environment.NewLine;
                foreach (var pp in c.Participants) text += SerPar(pp) + Environment.NewLine;
            }

            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }

        private string SerPar(Purple_1.Participant p)
        {
            string text = "";
            text += "Type: Participant" + Environment.NewLine;
            text += $"Name: {p.Name}" + Environment.NewLine;
            text += $"Surname: {p.Surname}" + Environment.NewLine;
            text += $"Coefs: {String.Join(" ", p.Coefs)}" + Environment.NewLine;
            string marks = "";
            foreach (var i in p.Marks) marks += i.ToString() + " ";
            text += $"Marks: {marks}" + Environment.NewLine;
            text += $"TotalScore: {p.TotalScore}";
            return text;
        }

        private string SerJudge(Purple_1.Judge j)
        {
            string text = "";
            text += "Type: Judge" + Environment.NewLine;
            text += $"Name: {j.Name}" + Environment.NewLine;
            text += $"Marks: {String.Join(" ", j.Marks)}" + Environment.NewLine;
            return text;
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllLines(FilePath);
            if (text[0].Split(' ')[1] == "Participant") return DesPar(text) as T;
            else if (text[0].Split(' ')[1] == "Judge") return DesJudge(text) as T;
            else if (text[0].Split(' ')[1] == "Competition")
            {
                Int32.TryParse(text[1], out int countj);
                var judges = new Purple_1.Judge[0];
                for (int i = 0; i < countj; i++)
                {
                    var info = new string[] { text[2 + i * 3], text[3 + i * 3], text[4 + i * 3] };
                    var j = DesJudge(info);
                    Array.Resize(ref judges, judges.Length + 1);
                    judges[judges.Length - 1] = j;
                }
                var c = new Purple_1.Competition(judges);
                int current = 2 + countj * 3;
                Int32.TryParse(text[current], out int countp);
                current++;
                for (int i = 0; i < countp; i++)
                {
                    var info = new string[] { text[current + i * 6], text[current + 1 + i * 6], text[current + 2 + i * 6], text[current + 3 + i * 6], text[current + 4 + i * 6], text[current + 5 + i * 6] };
                    var p = DesPar(info);
                    c.Add(p);
                }
                return c as T;
            }
            return null;

        }

        private Purple_1.Participant DesPar(string[] text)
        {
            var p = new Purple_1.Participant(text[1].Split(' ')[1], text[2].Split(' ')[1]);

            var coefs = text[3].Split(':')[1].Trim().Split(' ');
            var coefs_double = new double[coefs.Length];
            for (int i = 0; i < coefs.Length; i++) Double.TryParse(coefs[i], out coefs_double[i]);
            p.SetCriterias(coefs_double);

            var marks = text[4].Split(':')[1].Trim().Split(' ');
            var marks_int = new int[4][];
            for (int i = 0; i < marks.Length; i++) Int32.TryParse(marks[i], out marks_int[i / 7][i & 7]);
            foreach (var m in marks_int) p.Jump(m);

            return p;
        }

        private Purple_1.Judge DesJudge(string[] text)
        {
            var marks = text[3].Split(':')[1].Trim().Split(' ');
            var marks_int = new int[marks.Length];
            for (int i = 0; i < marks.Length; i++) Int32.TryParse(marks[i], out marks_int[i]);

            var j = new Purple_1.Judge(text[1].Split(' ')[1], marks_int);
            return j;
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            string text = "";
            int standard = 0;
            if (jumping is JuniorSkiJumping) { text += "Type: JuniorSkiJumping" + Environment.NewLine; standard = 100; }
            if (jumping is ProSkiJumping) { text += "Type: ProSkiJumping" + Environment.NewLine; standard = 150; }
            text += $"Name: {jumping.Name}" + Environment.NewLine;
            text += $"Standard: {standard}" + Environment.NewLine;
            text += jumping.Participants.Length.ToString() + Environment.NewLine;
            foreach (var p in jumping.Participants)
            {
                text += $"Name: {p.Name}" + Environment.NewLine;
                text += $"Surname: {p.Surname}" + Environment.NewLine;
                text += $"Distance: {p.Distance}" + Environment.NewLine;
                text += $"Marks: {String.Join(" ", p.Marks)}" + Environment.NewLine;
                text += $"Result: {p.Result}" + Environment.NewLine;
            }

            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            string[] text = File.ReadAllLines(FilePath);

            if (text[0].Split(' ')[1] == "JuniorSkiJumping")
            {
                var j = new JuniorSkiJumping();
                Int32.TryParse(text[3], out int count);
                for (int i = 0; i < count; i++)
                {
                    var p = new Purple_2.Participant(text[4 + i * 5].Split(' ')[1], text[5 + i * 5].Split(' ')[1]);

                    var marks = text[7 + i * 5].Split(':')[1].Trim().Split(' ');
                    var marks_int = new int[marks.Length];
                    for (int k = 0; k < marks.Length; k++) Int32.TryParse(marks[k], out marks_int[k]);
                    Int32.TryParse(text[6 + i * 5], out int distance);
                    p.Jump(distance, marks_int, 100);
                    j.Add(p);
                }
                return j as T;
            }
            else if (text[0].Split(' ')[1] == "ProSkiJumping")
            {
                var j = new ProSkiJumping();
                Int32.TryParse(text[3], out int count);
                for (int i = 0; i < count; i++)
                {
                    var p = new Purple_2.Participant(text[4 + i * 5].Split(' ')[1], text[5 + i * 5].Split(' ')[1]);

                    var marks = text[7 + i * 5].Split(':')[1].Trim().Split(' ');
                    var marks_int = new int[marks.Length];
                    for (int k = 0; k < marks.Length; k++) Int32.TryParse(marks[k], out marks_int[k]);
                    Int32.TryParse(text[6 + i * 5], out int distance);
                    p.Jump(distance, marks_int, 150);
                    j.Add(p);
                }
                return j as T;
            }
            return null;
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            string text = "";
            if (skating is FigureSkating) text += "Type: FigureSkating" + Environment.NewLine;
            if (skating is IceSkating) text += "Type: IceSkating" + Environment.NewLine;
            text += $"Moods: {String.Join(" ", skating.Moods)}" + Environment.NewLine;
            text += skating.Participants.Length.ToString() + Environment.NewLine;
            foreach (var p in skating.Participants)
            {
                text += $"Name: {p.Name}" + Environment.NewLine;
                text += $"Surname: {p.Surname}" + Environment.NewLine;
                text += $"Marks: {String.Join(" ", p.Marks)}" + Environment.NewLine;
                text += $"Places: {String.Join(" ", p.Places)}" + Environment.NewLine;
            }

            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            string[] text = File.ReadAllLines(FilePath);
            var moods = text[1].Split(':')[1].Trim().Split(' ');
            var moods_double = new double[moods.Length];
            for (int i = 0; i < moods.Length; i++) Double.TryParse(moods[i], out moods_double[i]);

            if (text[0].Split(' ')[1] == "FigureSkating")
            {
                var skating = new FigureSkating(moods_double, false);
                Int32.TryParse(text[2], out int count);
                for (int i = 0; i < count; i++)
                {
                    var p = new Purple_3.Participant(text[3 + i * 4].Split(' ')[1], text[4 + i * 4].Split(' ')[1]);

                    var marks = text[5 + i * 4].Split(':')[1].Trim().Split(' ');
                    var marks_double = new double[marks.Length];
                    for (int k = 0; k < marks.Length; k++) { Double.TryParse(marks[k], out marks_double[k]); p.Evaluate(marks_double[k]); }
                    skating.Add(p);
                }
                Purple_3.Participant.SetPlaces(skating.Participants);
                return skating as T;
            }
            else if (text[0].Split(' ')[1] == "IceSkating")
            {
                var skating = new IceSkating(moods_double, false);
                Int32.TryParse(text[2], out int count);
                for (int i = 0; i < count; i++)
                {
                    var p = new Purple_3.Participant(text[3 + i * 4].Split(' ')[1], text[4 + i * 4].Split(' ')[1]);

                    var marks = text[5 + i * 4].Split(':')[1].Trim().Split(' ');
                    var marks_double = new double[marks.Length];
                    for (int k = 0; k < marks.Length; k++) { Double.TryParse(marks[k], out marks_double[k]); p.Evaluate(marks_double[k]); }
                    skating.Add(p);
                }
                Purple_3.Participant.SetPlaces(skating.Participants);
                return skating as T;
            }
            return null;
        }

        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            string text = "";
            text += $"Name: {participant.Name}" + Environment.NewLine;
            text += participant.Sportsmen.Length.ToString() + Environment.NewLine;
            foreach (var s in participant.Sportsmen)
            {
                if (s is SkiMan) text += "Type: SkiMan" + Environment.NewLine;
                if (s is SkiWoman) text += "Type: SkiWoman" + Environment.NewLine;
                text += $"Name: {s.Name}" + Environment.NewLine;
                text += $"Surname: {s.Surname}" + Environment.NewLine;
                text += $"Time: {s.Time}" + Environment.NewLine;
            }

            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            string[] text = File.ReadAllLines(FilePath);

            var g = new Purple_4.Group(text[0].Split(' ')[1]);

            Int32.TryParse(text[1], out int count);
            for (int i = 0; i < count; i++)
            {
                Double.TryParse(text[5 + i * 4].Split(' ')[1], out double time);
                if (text[2 + i * 4].Split(' ')[1] == "SkiMan") { var s = new Purple_4.SkiMan(text[3 + i * 4].Split(' ')[1], text[4 + i * 4].Split(' ')[1], time); g.Add(s); }
                if (text[2 + i * 4].Split(' ')[1] == "SkiWoman") { var s = new Purple_4.SkiWoman(text[3 + i * 4].Split(' ')[1], text[4 + i * 4].Split(' ')[1], time); g.Add(s); }
            }

            return g;
        }

        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            string text = "";
            text += group.Researches.Length.ToString() + Environment.NewLine;
            foreach (var g in group.Researches)
            {
                text += $"Name: {g.Name}" + Environment.NewLine;
                text += g.Responses.Length.ToString() + Environment.NewLine;
                foreach (var r in g.Responses)
                {
                    text += $"Animal: {r.Animal}" + Environment.NewLine;
                    text += $"CharacterTrait: {r.CharacterTrait}" + Environment.NewLine;
                    text += $"Concept: {r.Concept}" + Environment.NewLine;
                }
            }

            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            string[] text = File.ReadAllLines(FilePath);

            var g = new Purple_5.Report();
            
            Int32.TryParse(text[0], out int count1);
            int current = 1;
            for (int i = 0; i < count1; i++)
            {
                var research = new Purple_5.Research(text[current++].Split(' ')[1]);
                Int32.TryParse(text[current++], out int count2);
                for (int j = 0; j < count2; j++, current += 3)
                {
                    var answers = new string[] { text[current].Split(' ')[1], text[current + 1].Split(' ')[1], text[current + 2].Split(' ')[1] };
                    research.Add(answers);
                }
                g.AddResearch(research);
            }

            return g;
        }
    }
}
