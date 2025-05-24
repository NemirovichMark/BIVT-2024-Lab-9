// PurpleTXTSerializer.cs
using System;
using System.IO;
using System.Linq;
using Lab_7;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            using var w = new StreamWriter(FilePath);

            if (obj is Purple_1.Participant p)
            {
                w.WriteLine(p.Name);
                w.WriteLine(p.Surname);
                w.WriteLine(string.Join(",", p.Coefs));
                for (int i = 0; i < p.Marks.GetLength(0); i++)
                    w.WriteLine(string.Join(",", Enumerable.Range(0, p.Marks.GetLength(1)).Select(j => p.Marks[i, j])));
            }
            else if (obj is Purple_1.Judge j)
            {
                w.WriteLine(j.Name);
                w.WriteLine(string.Join(",", j.Marks));
            }
            else if (obj is Purple_1.Competition c)
            {
                w.WriteLine(c.Judges.Length);
                foreach (var j2 in c.Judges)
                {
                    w.WriteLine(j2.Name);
                    w.WriteLine(string.Join(",", j2.Marks));
                }
                w.WriteLine(c.Participants.Length);
                foreach (var p2 in c.Participants)
                {
                    w.WriteLine(p2.Name);
                    w.WriteLine(p2.Surname);
                    w.WriteLine(string.Join(",", p2.Coefs));
                    for (int i = 0; i < p2.Marks.GetLength(0); i++)
                        w.WriteLine(string.Join(",", Enumerable.Range(0, p2.Marks.GetLength(1)).Select(j => p2.Marks[i, j])));
                }
            }
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);
            int idx = 0;

            if (typeof(T) == typeof(Purple_1.Participant))
            {
                var name = lines[idx++];
                var surname = lines[idx++];
                var coefs = lines[idx++].Split(',').Select(double.Parse).ToArray();
                var part = new Purple_1.Participant(name, surname);
                part.SetCriterias(coefs);
                for (int k = 0; k < coefs.Length; k++)
                {
                    var marks = lines[idx++].Split(',').Select(int.Parse).ToArray();
                    part.Jump(marks);
                }
                return part as T;
            }
            if (typeof(T) == typeof(Purple_1.Judge))
            {
                var name = lines[idx++];
                var marks = lines[idx++].Split(',').Select(int.Parse).ToArray();
                return new Purple_1.Judge(name, marks) as T;
            }
            if (typeof(T) == typeof(Purple_1.Competition))
            {
                int judgeCount = int.Parse(lines[idx++]);
                var judges = new Purple_1.Judge[judgeCount];
                for (int i = 0; i < judgeCount; i++)
                {
                    var jName = lines[idx++];
                    var jMarks = lines[idx++].Split(',').Select(int.Parse).ToArray();
                    judges[i] = new Purple_1.Judge(jName, jMarks);
                }
                var comp = new Purple_1.Competition(judges);
                int partCount = int.Parse(lines[idx++]);
                for (int p = 0; p < partCount; p++)
                {
                    var pn = lines[idx++];
                    var ps = lines[idx++];
                    var cf = lines[idx++].Split(',').Select(double.Parse).ToArray();
                    var part = new Purple_1.Participant(pn, ps);
                    part.SetCriterias(cf);
                    for (int k = 0; k < cf.Length; k++)
                    {
                        var m = lines[idx++].Split(',').Select(int.Parse).ToArray();
                        part.Jump(m);
                    }
                    comp.Add(part);
                }
                return comp as T;
            }
            return null;
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            using var w = new StreamWriter(FilePath);

            w.WriteLine(jumping.GetType().Name);
            w.WriteLine(jumping.Name);
            w.WriteLine(jumping.Standard);
            w.WriteLine(jumping.Participants.Length);
            foreach (var p in jumping.Participants)
            {
                w.WriteLine(p.Name);
                w.WriteLine(p.Surname);
                w.WriteLine(p.Distance);
                w.WriteLine(string.Join(",", p.Marks));
            }
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);
            int idx = 0;

            var typeName = lines[idx++];
            Purple_2.SkiJumping jmp = typeName == nameof(Purple_2.JuniorSkiJumping) ? new Purple_2.JuniorSkiJumping() : new Purple_2.ProSkiJumping();

            idx += 2;
            int cnt = int.Parse(lines[idx++]);
            for (int i = 0; i < cnt; i++)
            {
                var nm = lines[idx++];
                var sr = lines[idx++];
                var dist = int.Parse(lines[idx++]);
                var marks = lines[idx++].Split(',').Select(int.Parse).ToArray();
                var p = new Purple_2.Participant(nm, sr);
                p.Jump(dist, marks, jmp.Standard);
                jmp.Add(p);
            }
            return jmp as T;
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            using var w = new StreamWriter(FilePath);

            w.WriteLine(skating.GetType().Name);
            w.WriteLine(string.Join(",", skating.Moods));
            w.WriteLine(skating.Participants.Length);
            foreach (var p in skating.Participants)
            {
                w.WriteLine(p.Name);
                w.WriteLine(p.Surname);
                w.WriteLine(string.Join(",", p.Marks));
                w.WriteLine(string.Join(",", p.Places));
            }
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);
            int idx = 0;

            var typeName = lines[idx++];
            var moods = lines[idx++].Split(',').Select(double.Parse).ToArray();
            Purple_3.Skating sk = typeName == nameof(Purple_3.FigureSkating) ? new Purple_3.FigureSkating(moods, false) : new Purple_3.IceSkating(moods, false); 

            int cnt = int.Parse(lines[idx++]);
            for (int i = 0; i < cnt; i++)
            {
                var nm = lines[idx++];
                var sr = lines[idx++];
                var marks = lines[idx++].Split(',').Select(double.Parse).ToArray();
                _ = lines[idx++];
                var p = new Purple_3.Participant(nm, sr);
                foreach (var m in marks) p.Evaluate(m);
                sk.Add(p);
            }
            Purple_3.Participant.SetPlaces(sk.Participants);
            return sk as T;
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            using var w = new StreamWriter(FilePath);

            w.WriteLine(group.Name);
            w.WriteLine(group.Sportsmen.Length);
            foreach (var s in group.Sportsmen)
            {
                w.WriteLine(s.GetType().Name);
                w.WriteLine(s.Name);
                w.WriteLine(s.Surname);
                w.WriteLine(s.Time);
            }
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);
            int idx = 0;

            var name = lines[idx++];
            var grp = new Purple_4.Group(name);
            int cnt = int.Parse(lines[idx++]);
            for (int i = 0; i < cnt; i++)
            {
                _ = lines[idx++]; 
                var nm = lines[idx++];
                var sr = lines[idx++];
                var tm = double.Parse(lines[idx++]);
                var sm = new Purple_4.Sportsman(nm, sr);
                sm.Run(tm);
                grp.Add(sm);
            }
            return grp;
        }


        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            using var w = new StreamWriter(FilePath);

            var researches = report.Researches;
            w.WriteLine(researches.Length);
            foreach (var r in researches)
            {
                w.WriteLine(r.Name);
                var resps = r.Responses;
                w.WriteLine(resps.Length);
                foreach (var resp in resps)
                {
                    w.WriteLine(resp.Animal);
                    w.WriteLine(resp.CharacterTrait);
                    w.WriteLine(resp.Concept);
                }
            }
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);

            if (lines.Length == 0)
                return null;

            int idx = 0;

            if (!int.TryParse(lines[idx++], out int repCount) || repCount == 0)
                return null;

            var report = new Purple_5.Report();
            for (int i = 0; i < repCount; i++)
            {
                if (idx >= lines.Length) return null;
                var researchName = lines[idx++];

                var research = new Purple_5.Research(researchName);

                if (idx >= lines.Length) return null;
                if (!int.TryParse(lines[idx++], out int respCount))
                    return null;

                for (int j = 0; j < respCount; j++)
                {
                    if (idx + 2 >= lines.Length) return null;

                    var rawAnimal = lines[idx++];
                    var rawTrait = lines[idx++];
                    var rawConcept = lines[idx++];

                    var animal = string.IsNullOrEmpty(rawAnimal) ? null : rawAnimal;
                    var trait = string.IsNullOrEmpty(rawTrait) ? null : rawTrait;
                    var concept = string.IsNullOrEmpty(rawConcept) ? null : rawConcept;

                    research.Add(new[] { animal, trait, concept });
                }

                report.AddResearch(research);
            }

            return report;
        }


    }
}
