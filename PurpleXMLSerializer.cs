using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Lab_7;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";

        private void WriteXml<T>(T dto)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, dto);
        }

        private T ReadXml<T>()
        {
            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StreamReader(FilePath);
            return (T)serializer.Deserialize(reader);
        }

        private JaggedInt[] ToJagged(int[,] matrix)
            => matrix.Cast<int>()
                     .Chunk(matrix.GetLength(1))
                     .Select(r => new JaggedInt { Items = r })
                     .ToArray();

        private int[,] ToMatrix(JaggedInt[] jagged)
        {
            int rows = jagged.Length, cols = jagged[0].Items.Length;
            var matrix = new int[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i, j] = jagged[i].Items[j];
            return matrix;
        }

        public class JaggedInt { [XmlElement("Value")] public int[] Items; }

        // DTOs for Purple1
        public class P1Participant { public string Name; public string Surname; public double[] Coefs; public JaggedInt[] Marks; }
        public class P1Judge { public string Name;  public JaggedInt Marks; }
        public class P1Competition {  public P1Judge[] Judges;  public P1Participant[] Participants; }

        // DTOs for Purple2
        public class P2Participant { public string Name; public string Surname; public int Distance;  public JaggedInt Marks; }
        public class P2Jumping { public string Type; public string Name; public int Standard;  public P2Participant[] Participants; }

        // DTOs for Purple3
        public class P3Participant { public string Name; public string Surname; public double[] Marks;  public int[] Places; }
        public class P3Skating { public string Type;  public double[] Moods;  public P3Participant[] Participants; }

        // DTOs for Purple4
        public class P4Sportsman { public string Name; public string Surname; public double Time; }
        public class P4Group { public string Name; public P4Sportsman[] Sportsmen; }

        // DTOs for Purple5
        public class JResponse { public string Animal; public string CharacterTrait; public string Concept; }
        public class JResearch { public string Name;  public JResponse[] Responses; }
        public class JReport {  public JResearch[] Researches; }

        // Purple1
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);

            switch (obj)
            {
                case Purple_1.Participant p:
                    var dto1 = new P1Participant
                    {
                        Name = p.Name,
                        Surname = p.Surname,
                        Coefs = p.Coefs,
                        Marks = ToJagged(p.Marks)
                    };
                    WriteXml(dto1);
                    break;

                case Purple_1.Judge j:
                    var dto2 = new P1Judge { Name = j.Name, Marks = new JaggedInt { Items = j.Marks } };
                    WriteXml(dto2); 
                    break;

                case Purple_1.Competition c:
                    var dto3 = new P1Competition
                    {
                        Judges = c.Judges.Select(jj => new P1Judge { Name = jj.Name, Marks = new JaggedInt { Items = jj.Marks } }).ToArray(),
                        Participants = c.Participants.Select(pp => new P1Participant { Name = pp.Name, Surname = pp.Surname, Coefs = pp.Coefs, Marks = ToJagged(pp.Marks) }).ToArray()
                    };
                    WriteXml(dto3);
                    break;
            }
        }


        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var t = typeof(T);
            if (t == typeof(Purple_1.Participant))
            {
                var d = ReadXml<P1Participant>();
                var p = new Purple_1.Participant(d.Name, d.Surname);
                p.SetCriterias(d.Coefs);
                foreach (var row in d.Marks) p.Jump(row.Items);
                return p as T;
            }
            if (t == typeof(Purple_1.Judge))
            {
                var d = ReadXml<P1Judge>();
                return new Purple_1.Judge(d.Name, d.Marks.Items) as T;
            }
            var cd = ReadXml<P1Competition>();
            var judges = cd.Judges.Select(j => new Purple_1.Judge(j.Name, j.Marks.Items)).ToArray();
            var comp = new Purple_1.Competition(judges);
            foreach (var pr in cd.Participants)
            {
                var p = new Purple_1.Participant(pr.Name, pr.Surname);
                p.SetCriterias(pr.Coefs);
                foreach (var row in pr.Marks) p.Jump(row.Items);
                comp.Add(p);
            }
            return comp as T;
        }

        // Purple2
        public override void SerializePurple2SkiJumping<T>(T skiing, string fileName)
        {
            SelectFile(fileName);
            var s = skiing as Purple_2.SkiJumping;
            var dto = new P2Jumping
            {
                Type = s.GetType().Name,
                Name = s.Name,
                Standard = s.Standard,
                Participants = s.Participants.Select(p => new P2Participant { Name = p.Name, Surname = p.Surname, Distance = p.Distance, Marks = new JaggedInt { Items = p.Marks } }).ToArray()
            };
            WriteXml(dto);
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var d = ReadXml<P2Jumping>();
            Purple_2.SkiJumping sk;
            if (d.Type == nameof(Purple_2.JuniorSkiJumping))
                sk = new Purple_2.JuniorSkiJumping();
            else
                sk = new Purple_2.ProSkiJumping();
            foreach (var p in d.Participants)
            {
                var pr = new Purple_2.Participant(p.Name, p.Surname);
                pr.Jump(p.Distance, p.Marks.Items, sk.Standard);
                sk.Add(pr);
            }
            return sk as T;
        }

        // Purple3
        public override void SerializePurple3Skating<T>(T skate, string fileName)
        {
            SelectFile(fileName);
            var s = skate as Purple_3.Skating;
            var dto = new P3Skating
            {
                Type = s.GetType().Name,
                Moods = s.Moods,
                Participants = s.Participants.Select(p => new P3Participant { Name = p.Name, Surname = p.Surname, Marks = p.Marks, Places = p.Places }).ToArray()
            };
            WriteXml(dto);
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var d = ReadXml<P3Skating>();
            Purple_3.Skating s;
            if (d.Type == nameof(Purple_3.IceSkating))
                s = new Purple_3.IceSkating(d.Moods, false);
            else
                s = new Purple_3.FigureSkating(d.Moods, false);
            foreach (var p in d.Participants)
            {
                var np = new Purple_3.Participant(p.Name, p.Surname);
                foreach (var m in p.Marks) np.Evaluate(m);
                s.Add(np);
            }
            Purple_3.Participant.SetPlaces(s.Participants);
            return s as T;
        }

        // Purple4
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            var dto = new P4Group { Name = group.Name, Sportsmen = group.Sportsmen.Select(s => new P4Sportsman { Name = s.Name, Surname = s.Surname, Time = s.Time }).ToArray() };
            WriteXml(dto);
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var d = ReadXml<P4Group>();
            var g = new Purple_4.Group(d.Name);
            foreach (var s in d.Sportsmen)
            {
                var sp = new Purple_4.Sportsman(s.Name, s.Surname);
                sp.Run(s.Time);
                g.Add(sp);
            }
            return g;
        }

        // Purple5
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            var dto = new JReport { Researches = report.Researches.Select(r => new JResearch { Name = r.Name, Responses = r.Responses.Select(rr => new JResponse { Animal = rr.Animal, CharacterTrait = rr.CharacterTrait, Concept = rr.Concept }).ToArray() }).ToArray() };
            WriteXml(dto);
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var d = ReadXml<JReport>();
            var rpt = new Purple_5.Report();
            foreach (var rs in d.Researches)
            {
                var research = new Purple_5.Research(rs.Name);
                foreach (var rr in rs.Responses)
                    research.Add(new[] { rr.Animal, rr.CharacterTrait, rr.Concept });
                rpt.AddResearch(research);
            }
            return rpt;
        }
    }
}
