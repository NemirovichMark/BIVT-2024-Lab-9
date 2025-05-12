using Lab_7;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Lab_7.Purple_3;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);

            if (typeof(T) == typeof(Purple_1.Participant))
            {
                var serializer = new XmlSerializer(typeof(Purple1Participant));
                var dto = (Purple1Participant) serializer.Deserialize(reader);
                return (T) (Object) dto.ToParticipant();
            } else if (typeof(T) == typeof(Purple_1.Judge))
            {
                var serializer = new XmlSerializer(typeof(Purple1Judge));
                var dto = (Purple1Judge)serializer.Deserialize(reader);
                return (T) (Object) dto.ToJudge();
            } else if (typeof(T) == typeof(Purple_1.Competition))
            {
                var serializer = new XmlSerializer(typeof(Purple1Competition));
                var dto = (Purple1Competition)serializer.Deserialize(reader);
                return (T)(Object)dto.ToCompetition();
            }

            return null;
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);
            var serializer = new XmlSerializer(typeof(Purple2SkiJumping));
            var dto = (Purple2SkiJumping) serializer.Deserialize(reader);
            return (T)dto.ToSkiJumping();
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);
            var serializer = new XmlSerializer(typeof(Purple3Skating));
            var dto = (Purple3Skating)serializer.Deserialize(reader);
            return (T)dto.ToSkating();
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);
            var serializer = new XmlSerializer(typeof(Purple4Group));
            var dto = (Purple4Group)serializer.Deserialize(reader);
            return dto.ToGroup();
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);
            var serializer = new XmlSerializer(typeof(Purple5Report));
            var dto = (Purple5Report)serializer.Deserialize(reader);
            return dto.ToReport();
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            XmlSerializer serializer;
            using var writer = new StreamWriter(FilePath);
            if (obj is Purple_1.Participant p)
            {
                serializer = new XmlSerializer(typeof(Purple1Participant));
                serializer.Serialize(writer, new Purple1Participant(p));
            } else if (obj is Purple_1.Judge j)
            {
                serializer = new XmlSerializer(typeof(Purple1Judge));
                serializer.Serialize(writer, new Purple1Judge(j));
            } else if (obj is Purple_1.Competition c)
            {
                serializer = new XmlSerializer(typeof(Purple1Competition));
                serializer.Serialize(writer, new Purple1Competition(c));
            }
            writer.Close();
        }

        public class Purple1Participant
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int[][] Marks { get; set; }
            public Purple1Participant() { }
            public Purple1Participant(Purple_1.Participant p)
            {
                Name = p.Name;
                Surname = p.Surname;
                Coefs = p.Coefs;
                int[][] marks = new int[p.Marks.GetLength(0)][];
                for (int i = 0; i < p.Marks.GetLength(0); i++)
                {
                    marks[i] = new int[p.Marks.GetLength(1)];
                    for (int j = 0; j < p.Marks.GetLength(1); j++)
                        marks[i][j] = p.Marks[i, j];
                }
                Marks = marks;
            }
            public Purple_1.Participant ToParticipant()
            {
                var p = new Purple_1.Participant(Name, Surname);
                p.SetCriterias(Coefs);
                foreach (var arr in Marks) p.Jump(arr);
                return p;
            }
        }
        public class Purple1Judge
        {
            public string Name { get; set; }
            public int[] Marks {  get; set; }
            public Purple1Judge() { }
            public Purple1Judge(Purple_1.Judge j)
            {
                Name = j.Name;
                Marks = j.Marks;
            }
            public Purple_1.Judge ToJudge()
            {
                var j = new Purple_1.Judge(Name, Marks);
                return j;
            }
        }
        public class Purple1Competition
        {
            public Purple1Participant[] Participants { get; set; }
            public Purple1Judge[] Judges { get; set; }
            public Purple1Competition() { }
            public Purple1Competition(Purple_1.Competition c)
            {
                Participants = c.Participants.Select(x => new Purple1Participant(x)).ToArray();
                Judges = c.Judges.Select(x => new Purple1Judge(x)).ToArray();
            }
            public Purple_1.Competition ToCompetition()
            {
                var c = new Purple_1.Competition(Judges.Select(x => x.ToJudge()).ToArray());
                c.Add(Participants.Select(x => x.ToParticipant()).ToArray());
                return c;
            }
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            using var writer = new StreamWriter(FilePath);
            XmlSerializer serializer = new XmlSerializer(typeof(Purple2SkiJumping));
            serializer.Serialize(writer, new Purple2SkiJumping(jumping));
        }

        public class Purple2Participant
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }
            public Purple2Participant() { }
            public Purple2Participant(Purple_2.Participant p)
            {
                Name = p.Name;
                Surname = p.Surname;
                Distance = p.Distance;
                Marks = p.Marks;
            }
            public Purple_2.Participant ToParticipant(int standard)
            {
                var p = new Purple_2.Participant(Name, Surname);
                p.Jump(Distance, Marks, standard);
                return p;
            }
        }
        public class Purple2SkiJumping
        {
            public string Name { get; set; }
            public int Standard {  get; set; }
            public Purple2Participant[] Participants { get; set; }
            public string Type { get; set; }
            public Purple2SkiJumping() { }
            public Purple2SkiJumping(Purple_2.SkiJumping j)
            {
                Name = j.Name;
                Standard = j.Standard;
                Participants = j.Participants.Select(x => new Purple2Participant(x)).ToArray();
                Type = j.GetType().Name;
            }
            public Purple_2.SkiJumping ToSkiJumping()
            {
                Purple_2.SkiJumping jumping;
                if (Type == "JuniorSkiJumping")
                    jumping = new Purple_2.JuniorSkiJumping();
                else if (Type == "ProSkiJumping")
                    jumping = new Purple_2.ProSkiJumping();
                else return null;
                jumping.Add(Participants.Select(x => x.ToParticipant(Standard)).ToArray());
                return jumping;
            }
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            using var writer = new StreamWriter(FilePath);
            XmlSerializer serializer = new XmlSerializer(typeof(Purple3Skating));
            serializer.Serialize(writer, new Purple3Skating(skating));
        }
        public class Purple3Participant
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Marks { get; set; }
            public int[] Places { get; set; }
            public Purple3Participant() { }
            public Purple3Participant(Purple_3.Participant p)
            {
                Name = p.Name;
                Surname = p.Surname;
                Marks = p.Marks;
                Places = p.Places;
            }
            public Purple_3.Participant ToParticipant()
            {
                var p = new Purple_3.Participant(Name, Surname);
                foreach (var mark in Marks) p.Evaluate(mark);
                return p;
            }
        }
        public class Purple3Skating
        {
            public Purple3Participant[] Participants { get; set; }
            public double[] Moods { get; set; }
            public string Type { get; set; }
            public Purple3Skating() { }
            public Purple3Skating(Purple_3.Skating skating)
            {
                Participants = skating.Participants.Select(x => new Purple3Participant(x)).ToArray();
                Moods = skating.Moods;
                Type = skating.GetType().Name;
            }
            public Purple_3.Skating ToSkating()
            {
                Purple_3.Skating skating;
                if (Type == "FigureSkating") skating = new Purple_3.FigureSkating(Moods, false);
                else if (Type == "IceSkating") skating = new Purple_3.IceSkating(Moods, false);
                else return null;
                skating.Add(Participants.Select(x => x.ToParticipant()).ToArray());
                Purple_3.Participant.SetPlaces(skating.Participants);
                return skating;
            }
        }

        public override void SerializePurple4Group(Purple_4.Group participant, string fileName)
        {
            SelectFile(fileName);
            using var writer = new StreamWriter(FilePath);
            XmlSerializer serializer = new XmlSerializer(typeof(Purple4Group));
            serializer.Serialize(writer, new Purple4Group(participant));
        }

        public class Purple4Sportsman
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time {  get; set; }
            public Purple4Sportsman() { }
            public Purple4Sportsman(Purple_4.Sportsman man)
            {
                Name = man.Name;
                Surname = man.Surname;
                Time = man.Time;
            }
            public Purple_4.Sportsman ToSportsman()
            {
                var man = new Purple_4.Sportsman(Name, Surname);
                man.Run(Time);
                return man;
            }
        }
        public class Purple4Group
        {
            public string Name { get; set; }
            public Purple4Sportsman[] Sportsmen {  get; set; }
            public Purple4Group() { }
            public Purple4Group(Purple_4.Group g)
            {
                Name = g.Name;
                Sportsmen = g.Sportsmen.Select(x => new Purple4Sportsman(x)).ToArray();
            }
            public Purple_4.Group ToGroup()
            {
                var group = new Purple_4.Group(Name);
                group.Add(Sportsmen.Select(x => x.ToSportsman()).ToArray());
                return group;
            }
        }

        public override void SerializePurple5Report(Purple_5.Report group, string fileName)
        {
            SelectFile(fileName);
            using var writer = new StreamWriter(FilePath);
            XmlSerializer serializer = new XmlSerializer(typeof(Purple5Report));
            serializer.Serialize(writer, new Purple5Report(group));
        }
        public class Purple5Response
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }
            public Purple5Response() { }
            public Purple5Response(Purple_5.Response r)
            {
                Animal = r.Animal;
                CharacterTrait = r.CharacterTrait;
                Concept = r.Concept;
            }
            public Purple_5.Response ToResponse()
            {
                var r = new Purple_5.Response(Animal, CharacterTrait, Concept);
                return r;
            }
        }
        public class Purple5Research
        {
            public string Name {  get; set; }
            public Purple5Response[] Responses { get; set; }
            public Purple5Research() { }
            public Purple5Research(Purple_5.Research r)
            {
                Name = r.Name;
                Responses = r.Responses.Select(x => new Purple5Response(x)).ToArray();
            }
            public Purple_5.Research ToResearch()
            {
                var r = new Purple_5.Research(Name);
                foreach (var resp in Responses)
                    r.Add([resp.Animal, resp.CharacterTrait, resp.Concept]);
                return r;
            }
        }
        public class Purple5Report
        {
            public Purple5Research[] Researches { get; set; }
            public Purple5Report() { }
            public Purple5Report(Purple_5.Report r)
            {
                Researches = r.Researches.Select(x => new Purple5Research(x)).ToArray();
            }
            public Purple_5.Report ToReport()
            {
                var r = new Purple_5.Report();
                r.AddResearch(Researches.Select(x => x.ToResearch()).ToArray());
                return r;
            }
        }
    }
}
