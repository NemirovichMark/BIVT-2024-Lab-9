using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Lab_9.PurpleXMLSerializer;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";

        private void Ser<T>(T obj)
        {
            using var writer = new StreamWriter(FilePath);
            var ser = new XmlSerializer(typeof(T));
            ser.Serialize(writer, obj);
            writer.Close();
        }

        private T Deser<T>()
        {
            using var reader = new StreamReader(FilePath);
            var ser = new XmlSerializer(typeof(T));
            var obj = (T)ser.Deserialize(reader);
            reader.Close();
            return obj;
        }



        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);

            if (typeof(T) == typeof(Purple_1.Participant))
            {
                var p = obj as Purple_1.Participant;
                var par = new P1_Participant(p);
                Ser(par);
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                var j = obj as Purple_1.Judge;
                var judge = new P1_Judge(j);
                Ser(judge);
            }
            else if (typeof(T) == typeof(Purple_1.Competition))
            {
                var c = obj as Purple_1.Competition;
                var comp = new P1_Competition(c);
                Ser(comp);
            }
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);

            if (typeof(T) == typeof(Purple_1.Participant))
            {
                var p = Deser<P1_Participant>();
                var par = new Purple_1.Participant(p.Name, p.Surname);
                par.SetCriterias(p.Coefs);
                foreach (var m in p.Marks) par.Jump(m);

                return par as T;
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                var j = Deser<P1_Judge>();
                var judge = new Purple_1.Judge(j.Name, j.Marks);

                return judge as T;
            }
            else if (typeof(T) == typeof(Purple_1.Competition))
            {
                var c = Deser<P1_Competition>();
                var comp = new Purple_1.Competition(c.Judges.Select(j => new Purple_1.Judge(j.Name, j.Marks)).ToArray());
                foreach (var p in c.Participants)
                {
                    var par = new Purple_1.Participant(p.Name, p.Surname);
                    par.SetCriterias(p.Coefs);
                    foreach (var m in p.Marks) par.Jump(m);
                    comp.Add(par);
                }

                return comp as T;
            }
            return default(T);
        }

        public class P1_Participant
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int[][] Marks { get; set; }
            public double TotalScore { get; set; }

            public P1_Participant(Purple_1.Participant p)
            {
                Name = p.Name;
                Surname = p.Surname;
                Coefs = p.Coefs;
                int[][] marks = new int[p.Marks.GetLength(0)][];
                for (int i = 0; i < p.Marks.GetLength(0); i++)
                {
                    marks[i] = new int[p.Marks.GetLength(1)];
                    for (int j = 0; j < p.Marks.GetLength(1); j++)
                    {
                        marks[i][j] = p.Marks[i, j];
                    }
                }
                Marks = marks;
                TotalScore = p.TotalScore;
            }
            public P1_Participant() { }
        }

        public class P1_Judge
        {
            public string Name { get; set; }
            public int[] Marks { get; set; }

            public P1_Judge(Purple_1.Judge j)
            {
                Name = j.Name;
                Marks = j.Marks;
            }
            public P1_Judge() { }
        }

        public class P1_Competition
        {
            public P1_Judge[] Judges { get; set; }
            public P1_Participant[] Participants { get; set; }

            public P1_Competition(Purple_1.Competition c)
            {
                Participants = c.Participants.Select(p => new P1_Participant(p)).ToArray();
                Judges = c.Judges.Select(j => new P1_Judge(j)).ToArray();
            }
            public P1_Competition() { }
        }



        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);

            if (jumping is Purple_2.JuniorSkiJumping)
            {
                var j = jumping as Purple_2.JuniorSkiJumping;
                var jump = new P2_SkiJumping(j);
                Ser(jump);
            }
            else if (jumping is Purple_2.ProSkiJumping)
            {
                var j = jumping as Purple_2.ProSkiJumping;
                var jump = new P2_SkiJumping(j);
                Ser(jump);
            }
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);

            var j = Deser<P2_SkiJumping>();

            if (j.Standard == 100)
            {
                var jump = new Purple_2.JuniorSkiJumping();
                foreach (var p in j.Participants)
                {
                    var par = new Purple_2.Participant(p.Name, p.Surname);
                    par.Jump(p.Distance, p.Marks, 100);
                    jump.Add(par);
                }
                return jump as T;
            }

            else if (j.Standard == 150)
            {
                var jump = new Purple_2.ProSkiJumping();
                foreach (var p in j.Participants)
                {
                    var par = new Purple_2.Participant(p.Name, p.Surname);
                    par.Jump(p.Distance, p.Marks, 150);
                    jump.Add(par);
                }
                return jump as T;
            }

            else return null;
        }

        public class P2_Participant
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }
            public int Result { get; set; }

            public P2_Participant(Purple_2.Participant p)
            {
                Name = p.Name;
                Surname = p.Surname;
                Distance = p.Distance;
                Marks = p.Marks;
                Result = p.Result;
            }
            public P2_Participant() { }
        }

        public class P2_SkiJumping
        {
            public string Name { get; set; }
            public int Standard { get; set; }
            public P2_Participant[] Participants { get; set; }
            public string Type { get; set; }

            public P2_SkiJumping(Purple_2.SkiJumping s)
            {
                Name = s.Name;
                Participants = s.Participants.Select(p => new P2_Participant(p)).ToArray();
                Standard = s.Standard;
                Type = s.GetType().Name;
            }
            public P2_SkiJumping() { }
        }



        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);

            if (skating is Purple_3.FigureSkating)
            {
                var s = skating as Purple_3.FigureSkating;
                var skat = new P3_Skating(s);
                Ser(skat);
            }
            else if (skating is Purple_3.IceSkating)
            {
                var s = skating as Purple_3.IceSkating;
                var skat = new P3_Skating(s);
                Ser(skat);
            }
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);

            var s = Deser<P3_Skating>();

            if (s.Type == nameof(Purple_3.FigureSkating))
            {
                var skat = new Purple_3.FigureSkating(s.Moods, false);
                foreach (var p in skat.Participants)
                {
                    var par = new Purple_3.Participant(p.Name, p.Surname);
                    foreach (var m in p.Marks) par.Evaluate(m);
                    skat.Add(par);
                }
                Purple_3.Participant.SetPlaces(skat.Participants);

                return skat as T;
            }

            else if (s.Type == nameof(Purple_3.IceSkating))
            {
                var skat = new Purple_3.IceSkating(s.Moods, false);
                foreach (var p in skat.Participants)
                {
                    var par = new Purple_3.Participant(p.Name, p.Surname);
                    foreach (var m in p.Marks) par.Evaluate(m);
                    skat.Add(par);
                }
                Purple_3.Participant.SetPlaces(skat.Participants);

                return skat as T;
            }
            else return null;
        }

        public class P3_Participant
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Marks { get; set; }
            public int[] Places { get; set; }
            public int Score { get; set; }

            public P3_Participant(Purple_3.Participant p)
            {
                Name = p.Name;
                Surname = p.Surname;
                Marks = p.Marks;
                Places = p.Places;
                Score = p.Score;
            }
            public P3_Participant() { }
        }

        public class P3_Skating
        {
            public double[] Moods { get; set; }
            public P3_Participant[] Participants { get; set; }
            public string Type { get; set; }

            public P3_Skating(Purple_3.Skating s)
            {
                Moods = s.Moods;
                Participants = s.Participants.Select(p => new P3_Participant(p)).ToArray();
                if (s is Purple_3.FigureSkating) Type = nameof(Purple_3.FigureSkating);
                else Type = nameof(Purple_3.IceSkating);
            }
            public P3_Skating() { }
        }



        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            SelectFile(fileName);

            var g = new P4_Group(participant);
            Ser(g);
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);

            var g = Deser<P4_Group>();

            var group = new Purple_4.Group(g.Name);

            foreach (var s in g.Sportsmen)
            {
                if (s.Type == typeof(Purple_4.SkiMan).ToString())
                {
                    var sp = new Purple_4.SkiMan(s.Name, s.Surname, s.Time);
                    group.Add(sp);
                }
                if (s.Type == typeof(Purple_4.SkiWoman).ToString())
                {
                    var sp = new Purple_4.SkiWoman(s.Name, s.Surname, s.Time);
                    group.Add(sp);
                }
                else
                {
                    var sp = new Purple_4.Sportsman(s.Name, s.Surname);
                    sp.Run(s.Time);
                    group.Add(sp);
                }
            }

            return group;
        }

        public class P4_Sportsman
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }
            public string Type { get; set; }

            public P4_Sportsman(Purple_4.Sportsman s)
            {
                Name = s.Name;
                Surname = s.Surname;
                Time = s.Time;
                if (s is Purple_4.SkiMan) Type = nameof(Purple_4.SkiMan);
                else Type = nameof(Purple_4.SkiWoman);
            }
            public P4_Sportsman() { }
        }

        public class P4_Group
        {
            public string Name { get; set; }
            public P4_Sportsman[] Sportsmen { get; set; }

            public P4_Group(Purple_4.Group g)
            {
                Name = g.Name;
                Sportsmen = g.Sportsmen.Select(s => new P4_Sportsman(s)).ToArray();
            }
            public P4_Group() { }
        }



        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            SelectFile(fileName);

            var g = new P5_Report(group);
            Ser(g);
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);

            var g = Deser<P5_Report>();

            var rep = new Purple_5.Report();
            foreach (var r in g.Researches)
            {
                var research = new Purple_5.Research(r.Name);
                if (r.Responses != null)
                {
                    foreach (var rr in r.Responses)
                    {
                        var ans1 = rr.Animal == "" ? null : rr.Animal;
                        var ans2 = rr.CharacterTrait == "" ? null : rr.CharacterTrait;
                        var ans3 = rr.Concept == "" ? null : rr.Concept;
                        research.Add(new string[] { ans1, ans2, ans3 });
                    }
                }
                rep.AddResearch(research);
            }

            return rep;
        }

        public class P5_Response
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }

            public P5_Response(Purple_5.Response r)
            {
                Animal = r.Animal;
                CharacterTrait = r.CharacterTrait;
                Concept = r.Concept;
            }
            public P5_Response() { }
        }

        public class P5_Research
        {
            public string Name { get; set; }
            public P5_Response[] Responses { get; set; }

            public P5_Research(Purple_5.Research r)
            {
                Name = r.Name;
                Responses = r.Responses.Select(rr => new P5_Response(rr)).ToArray();
            }
            public P5_Research() { }
        }

        public class P5_Report
        {
            public P5_Research[] Researches { get; set; }

            public P5_Report(Purple_5.Report r)
            {
                Researches = r.Researches.Select(rr => new P5_Research(rr)).ToArray();
            }
            public P5_Report() { }
        }
    }
}
