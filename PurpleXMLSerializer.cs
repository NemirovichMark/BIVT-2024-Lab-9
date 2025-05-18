using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Formats.Asn1.AsnWriter;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension
        {
            get
            {
                return "xml";
            }
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            if (obj is Purple_1.Judge j)
            {
                SerializeJudge(j, fileName);
            }
            if (obj is Purple_1.Competition c)
            {
                SerializeCompetition(c, fileName);
            }
            if (obj is Purple_1.Participant p)
            {
                SerializeParticipant(p, fileName);
            }
        }
        private void SerializeJudge(Purple_1.Judge obj, string fileName)
        {
            SelectFile(fileName);
            Judge objj = new Judge(obj);
            XmlSerializer serializer = new XmlSerializer(typeof(Judge));
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, objj);
            }

        }
        public class Judge
        {
            public string Name { get; set; }
            public int[] Marks { get; set; }
            public Judge() { }
            public Judge(Purple_1.Judge obj)
            {
                Name = obj.Name;
                Marks = obj.Marks;
            }
        }
        private void SerializeCompetition(Purple_1.Competition obj, string fileName)
        {
            SelectFile(fileName);
            Competition objj = new Competition(obj);
            XmlSerializer serializer = new XmlSerializer(typeof(Competition));
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, objj);
            }
        }
        public class Competition
        {
            public Judge[] Judges { get; set; }
            public Participant1[] Participants { get; set; }
            public Competition() { }
            public Competition(Purple_1.Competition obj)
            {
                Judges = new Judge[obj.Judges.Length];
                for (int i = 0; i < obj.Judges.Length; i++)
                {
                    Judges[i] = new Judge(obj.Judges[i]);
                }
                Participants = new Participant1[obj.Participants.Length];
                for (int i = 0; i < obj.Participants.Length; i++)
                {
                    Participants[i] = new Participant1(obj.Participants[i]);
                }
            }
        }
        private void SerializeParticipant(Purple_1.Participant obj, string fileName)
        {
            SelectFile(fileName);
            Participant1 objj = new Participant1(obj);
            XmlSerializer serializer = new XmlSerializer(typeof(Participant1));
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, objj);
            }
        }
        public class Participant1
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int M0 { get; set; }
            public int M1 { get; set; }
            public int[] Marks { get; set; }
            public Participant1() { }
            public Participant1(Purple_1.Participant obj)
            {
                Name = obj.Name;
                Surname = obj.Surname;
                Coefs = obj.Coefs;
                M0 = obj.Marks.GetLength(0);
                M1 = obj.Marks.GetLength(1);
                Marks = new int[M0 * M1];
                for (int i = 0, k = 0; i < M0; i++)
                {
                    for (int j = 0; j < M1; j++, k++)
                    {
                        Marks[k] = obj.Marks[i, j];
                    }
                }
            }
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            SkiJumping objj = new SkiJumping(jumping);
            XmlSerializer serializer = new XmlSerializer(typeof(SkiJumping));
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, objj);
            }
        }
        public class SkiJumping
        {
            public string Name { get; set; }
            public int Standard { get; set; }
            public Participant2[] Participant { get; set; }
            public SkiJumping() { }
            public SkiJumping(Purple_2.SkiJumping obj)
            {
                Name = obj.Name;
                Standard = obj.Standard;
                Participant = new Participant2[obj.Participants.Length];
                for (int i = 0; i < obj.Participants.Length; i++)
                {
                    Participant[i] = new Participant2(obj.Participants[i]);
                }
            }
        }
        public class Participant2
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }
            public Participant2() { }
            public Participant2(Purple_2.Participant obj)
            {
                Name = obj.Name;
                Surname = obj.Surname;
                Distance = obj.Distance;
                Marks = obj.Marks;
            }
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            Skating objj = new Skating(skating);
            XmlSerializer serializer = new XmlSerializer(typeof(Skating));
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, objj);
            }
        }
        public class Skating
        {
            public string Name { get; set; }
            public Participant3[] Participants { get; set; }
            public double[] Moods { get; set; }
            public Skating() { }
            public Skating(Purple_3.Skating obj)
            {
                Name = obj.GetType().Name;
                Participants = new Participant3[obj.Participants.Length];
                for (int i = 0; i < obj.Participants.Length; i++)
                {
                    Participants[i] = new Participant3(obj.Participants[i]);
                }
                Moods = obj.Moods;
            }
        }
        public class Participant3
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Places { get; set; }
            public double[] Marks { get; set; }
            public int Score { get; set; }
            public Participant3() { }
            public Participant3(Purple_3.Participant obj)
            {
                Name = obj.Name;
                Surname = obj.Surname;
                Places = obj.Places;
                Marks = obj.Marks;
                Score = obj.Score;
            }
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            Group objj = new Group(group);
            XmlSerializer Serializer = new XmlSerializer(typeof(Group));
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                Serializer.Serialize(writer, objj);
            }
        }
        public class Group
        {
            public string Name { get; set; }
            public Sportsman[] Sportsmen { get; set; }
            public Group() { }
            public Group(Purple_4.Group obj)
            {
                Name = obj.Name;
                Sportsmen = new Sportsman[obj.Sportsmen.Length];
                for (int i = 0; i < obj.Sportsmen.Length; i++)
                {
                    Sportsmen[i] = new Sportsman(obj.Sportsmen[i]);
                }
            }
        }
        public class Sportsman
        {
            public string TName { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }
            public Sportsman() { }
            public Sportsman(Purple_4.Sportsman obj)
            {
                TName = obj.GetType().Name;
                Name = obj.Name;
                Surname = obj.Surname;
                Time = obj.Time;
            }
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            Report objj = new Report(report);
            XmlSerializer serializer = new XmlSerializer(typeof(Report));
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, objj);
            }
        }
        public class Report
        {
            public Research[] Researches { get; set; }
            public Report() { }
            public Report(Purple_5.Report obj)
            {
                Researches = new Research[obj.Researches.Length];
                for (int i = 0; i < obj.Researches.Length; i++)
                {
                    Researches[i] = new Research(obj.Researches[i]);
                }
            }
        }
        public class Research
        {
            public string Name { get; set; }
            public Response[] Responses { get; set; }
            public Research() { }
            public Research(Purple_5.Research obj)
            {
                Name = obj.Name;
                Responses = new Response[obj.Responses.Length];
                for (int i = 0; i < obj.Responses.Length; i++)
                {
                    Responses[i] = new Response(obj.Responses[i]);
                }
            }
        }
        public class Response
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }
            public Response() { }
            public Response(Purple_5.Response obj)
            {
                Animal = obj.Animal;
                CharacterTrait = obj.CharacterTrait;
                Concept = obj.Concept;
            }
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            if (typeof(T) == typeof(Purple_1.Judge))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Judge));
                Judge obj;
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    obj = (Judge)serializer.Deserialize(reader);
                }
                Purple_1.Judge objj = new Purple_1.Judge(obj.Name, obj.Marks);
                return objj as T;
            }
            else if (typeof(T) == typeof(Purple_1.Competition))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Competition));
                Competition obj;
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    obj = (Competition)serializer.Deserialize(reader);
                }
                Purple_1.Judge[] Js = new Purple_1.Judge[obj.Judges.Length];
                Purple_1.Participant[] Ps = new Purple_1.Participant[obj.Participants.Length];
                for (int i = 0; i < obj.Judges.Length; i++)
                {
                    Js[i] = new Purple_1.Judge(obj.Judges[i].Name, obj.Judges[i].Marks);
                }
                for (int i = 0; i < obj.Participants.Length; i++)
                {
                    Ps[i] = new Purple_1.Participant(obj.Participants[i].Name, obj.Participants[i].Surname);
                    Ps[i].SetCriterias(obj.Participants[i].Coefs);
                    for (int ii = 0; ii < obj.Participants[i].M0; ii++)
                    {
                        int[] Markss = new int[obj.Participants[i].M1];
                        for (int j = 0; j < obj.Participants[i].M1; j++)
                        {
                            Markss[j] = obj.Participants[i].Marks[ii * obj.Participants[i].M1 + j];
                        }
                        Ps[i].Jump(Markss);
                    }
                }
                Purple_1.Competition objj = new Purple_1.Competition(Js);
                objj.Add(Ps);
                return objj as T;
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Participant1));
                Participant1 obj;
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    obj = (Participant1)serializer.Deserialize(reader);
                }
                Purple_1.Participant objj = new Purple_1.Participant(obj.Name, obj.Surname);
                objj.SetCriterias(obj.Coefs);
                for (int i = 0; i < obj.M0; i++)
                {
                    int[] Markss = new int[obj.M1];
                    for (int j = 0; j < obj.M1; j++)
                    {
                        Markss[j] = obj.Marks[i * obj.M1 + j];
                    }
                    objj.Jump(Markss);
                }
                return objj as T;
            }
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            XmlSerializer serializer = new XmlSerializer(typeof(SkiJumping));
            SkiJumping obj;
            using (StreamReader reader = new StreamReader(FilePath))
            {
                obj = (SkiJumping)serializer.Deserialize(reader);
            }
            Purple_2.SkiJumping objj;
            if (obj.Standard == 100) 
            {
                objj = new Purple_2.JuniorSkiJumping();
            }
            else
            {
                objj = new Purple_2.ProSkiJumping();
            }
            for (int i = 0; i < obj.Participant.Length; i++)
            {
                Purple_2.Participant P = new Purple_2.Participant(obj.Participant[i].Name, obj.Participant[i].Surname);
                P.Jump(obj.Participant[i].Distance, obj.Participant[i].Marks, obj.Standard);
                objj.Add(P);
            }
            return objj as T;
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            XmlSerializer serializer = new XmlSerializer(typeof(Skating));
            Skating obj;
            using (StreamReader reader = new StreamReader(FilePath))
            {
                obj = (Skating)serializer.Deserialize(reader);
            }

            Purple_3.Skating objj;
            if (obj.Name == "FigureSkating")
            {
                objj = new Purple_3.FigureSkating(obj.Moods, false);
            }
            else
            {
                objj = new Purple_3.IceSkating(obj.Moods, false);
            }
            for (int i = 0; i < obj.Participants.Length; i++)
            {
                Purple_3.Participant P = new Purple_3.Participant(obj.Participants[i].Name, obj.Participants[i].Surname);
                for (int j = 0; j < obj.Participants[i].Marks.Length; j++)
                {
                    P.Evaluate(obj.Participants[i].Marks[j]);
                }
                objj.Add(P);
            }

            return (T)objj;
            
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            XmlSerializer serializer = new XmlSerializer(typeof(Group));
            Group obj;
            using (StreamReader reader = new StreamReader(FilePath))
            {
                obj = (Group)serializer.Deserialize(reader);
            }
            Purple_4.Group objj;
            Purple_4.Sportsman[] Ss = new Purple_4.Sportsman[obj.Sportsmen.Length];
            for (int i = 0; i < obj.Sportsmen.Length; i++)
            {
                Ss[i] = new Purple_4.Sportsman(obj.Sportsmen[i].Name, obj.Sportsmen[i].Surname);
                if (obj.Sportsmen[i].Time != 0)
                    Ss[i].Run(obj.Sportsmen[i].Time);

                //if (obj.Sportsmen[i].TName == "SkiMan")
                //{
                //    if (obj.Sportsmen[i].Time != 0)
                //        Ss[i] = new Purple_4.SkiMan(obj.Sportsmen[i].Name, obj.Sportsmen[i].Surname, obj.Sportsmen[i].Time);
                //    else
                //        Ss[i] = new Purple_4.SkiMan(obj.Sportsmen[i].Name, obj.Sportsmen[i].Surname);
                //}
                //else
                //{
                //    if (obj.Sportsmen[i].Time != 0)
                //        Ss[i] = new Purple_4.SkiWoman(obj.Sportsmen[i].Name, obj.Sportsmen[i].Surname, obj.Sportsmen[i].Time);
                //    else
                //        Ss[i] = new Purple_4.SkiWoman(obj.Sportsmen[i].Name, obj.Sportsmen[i].Surname);
                //}
            }
            objj = new Purple_4.Group(obj.Name);
            objj.Add(Ss);
            return objj;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var content = File.ReadAllText(FilePath);
            XmlSerializer serializer = new XmlSerializer(typeof(Report));
            Report obj;
            using (StreamReader reader = new StreamReader(FilePath))
            {
                obj = (Report)serializer.Deserialize(reader);
            }
            Purple_5.Report objj = new Purple_5.Report();
            Purple_5.Research[] researches = new Purple_5.Research[obj.Researches.Length];
            for (int i = 0; i < obj.Researches.Length; i++)
            {
                researches[i] = new Purple_5.Research(obj.Researches[i].Name);
                for (int j = 0; j < obj.Researches[i].Responses.Length; j++)
                {
                    string[] s = { obj.Researches[i].Responses[j].Animal, obj.Researches[i].Responses[j].CharacterTrait, obj.Researches[i].Responses[j].Concept };
                    researches[i].Add(s);
                }
            }
            objj.AddResearch(researches);
            return objj;
        }
    }
}
