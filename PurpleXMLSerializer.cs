using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using static Lab_9.PurpleXMLSerializer.Sportsman_2_0;

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
        private void SerializeXml<T>(T temp)
        {
            var serializer = new XmlSerializer(typeof(T)); //создание сериализатора
            using (var wr = new StreamWriter(FilePath))// XmlWriter - обьект, который записывает xml в файл
            {
                serializer.Serialize(wr, temp);//сериализация - преобразует обьект типа темп и записывает с помощью wr
            }
            
        }
        /*
        private T DeserializeXml<T>(string fileName)
        {
            var serializer = new XmlSerializer(typeof(T));
            T obj;

            using (var rd = new StreamReader($"{Path.Combine(FolderPath, fileName)}.{Extension}"))
            {
                obj = (T)serializer.Deserialize(rd);
            }
            return obj;
        }

        */
        /////////////////////////////////////
        public class Participant_2_0
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int[][] Marks { get; set; }
            public Participant_2_0() { }
            public Participant_2_0(Purple_1.Participant p)
            {
                Name = p.Name;
                Surname = p.Surname;
                Coefs = p.Coefs;
                /*int rows = p.Marks.GetLength(0);
                int cols = p.Marks.GetLength(1);
                var marks = new int[rows][];//массив массивов
                for (int i = 0; i < rows; i++)
                {
                    marks[i] = new int[cols];
                    Array.Copy(p.Marks, i * cols, marks[i], 0, cols);
                }
                Marks = marks;*/
                var marks = new int[p.Marks.GetLength(0)][];
                for (int i = 0; i < p.Marks.GetLength(0); i++)
                {
                    marks[i] = new int[p.Marks.GetLength(1)];
                    for (int j = 0; j < p.Marks.GetLength(1); j++)
                    {
                        marks[i][j] = p.Marks[i, j];
                    }
                }
                Marks = marks;
            }
            public Purple_1.Participant pp()
            {
                var temp = new Purple_1.Participant(Name, Surname);
                temp.SetCriterias(Coefs);
                foreach (var x in Marks) temp.Jump(x);
                return temp;
            }
        }
        
        public class Judge_2_0
        {
            public string Name { get; set; }
            public int[] Marks { get; set; }
            public Judge_2_0() { }
            public Judge_2_0(Purple_1.Judge j)
            {
                Name=j.Name;
                Marks = j.Marks;
            }
            public Purple_1.Judge jj()
            {
                var temp = new Purple_1.Judge(Name, Marks);
                return temp;
            }
        }
        
        public class Competition_2_0
        {
            public Judge_2_0[] Judges {  get; set; }
            public Participant_2_0[] Participant { get; set; }
            public Competition_2_0() { }
            public Competition_2_0(Purple_1.Competition c)
            {
                Judges = new Judge_2_0[c.Judges.Length];
                for (int i = 0; i < c.Judges.Length; i++)
                {
                    Judges[i]=new Judge_2_0(c.Judges[i]);
                }
                Participant = new Participant_2_0[c.Participants.Length];
                for (int i = 0;i < c.Participants.Length;i++)
                {
                    Participant[i] = new Participant_2_0(c.Participants[i]);
                }
            }
            public Purple_1.Competition cc()
            {
                var temp = new Purple_1.Competition(Judges.Select(x => x.jj()).ToArray());
                temp.Add(Participant.Select(x => x.pp()).ToArray());
                return temp;
            }
        }
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);

            if (obj is Purple_1.Participant participant)
            {
                var temp = new Participant_2_0(participant);
                SerializeXml(temp);

            }
            else if (obj is Purple_1.Judge judge)
            {
                var temp = new Judge_2_0(judge);
                SerializeXml(temp);
            }
            else if (obj is Purple_1.Competition competition)
            {
                var temp = new Competition_2_0(competition);
                SerializeXml(temp);
            }
           // else return;
        }
        /*
        private void Serialize_Participant(Purple_1.Participant part, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Purple_1.Participant));
            using (var wr = new StreamWriter(fileName))
            {
                serializer.Serialize(wr, part);
            }
        }
        private void Serialize_Judge(Purple_1.Judge judge, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Purple_1.Judge));
            using (var wr = new StreamWriter(fileName))
            {
                serializer.Serialize(wr, judge);
            }
        }
        private void Serialize_Competition(Purple_1.Competition competition, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer (typeof(Purple_1.Competition));
            using (var wr = new StreamWriter(fileName))
            {
                serializer.Serialize(wr, competition);
            }
        }
    */
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);

            using (var temp = new StreamReader(FilePath))
            {
                if (typeof(T) == typeof(Purple_1.Participant))
                {
                    var serializer = new XmlSerializer(typeof(Participant_2_0));
                    var participant = serializer.Deserialize(temp) as Participant_2_0;
                    var participant1 = participant.pp();

                    return participant1 as T;
                }
                else if (typeof(T) == typeof(Purple_1.Judge))
                {
                    var serializer = new XmlSerializer(typeof(Judge_2_0));
                    var judge = serializer.Deserialize(temp) as Judge_2_0;
                    var judge1 = judge.jj();

                    return judge1 as T;
                }
                else if (typeof(T) == typeof(Purple_1.Competition))
                {
                    var serializer = new XmlSerializer(typeof(Competition_2_0));
                    var competition = serializer.Deserialize(temp) as Competition_2_0;
                    var competition1 = competition.cc();

                    return competition1 as T;
                }
                else return null;

            }
        }
        public class Participant2_2_0
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }
            public Participant2_2_0() { }
            public Participant2_2_0(Purple_2.Participant p)
            {
                Name= p.Name;
                Surname= p.Surname;
                Distance= p.Distance;
                Marks = p.Marks;

            }
            public Purple_2.Participant pp(int target)
            {
                var temp = new Purple_2.Participant(Name, Surname);
                temp.Jump(Distance, Marks, target);
                return temp;
            }
        }
        public class SkiJumping_2_0
        {
            public string Name { get; set; }
            public int Standard { get; set; }
            public int ParticipantsCount { get; set; }
            public Participant2_2_0[] Participants { get; set; }
            public SkiJumping_2_0() { }
            public SkiJumping_2_0(Purple_2.SkiJumping s)
            {
                Name= s.Name;
                Standard = s.Standard;
                Participants = new Participant2_2_0[s.Participants.Length];
                for (int i=0;  i<s.Participants.Length; i++)
                {
                    Participants[i] = new Participant2_2_0(s.Participants[i]);
                }
            }
            public Purple_2.SkiJumping ss()
            {
                
                if (Standard == 100)
                {
                    var temp = new Purple_2.JuniorSkiJumping();
                    var participant = new Purple_2.Participant[Participants.Length];
                    for (int i = 0; i < participant.Length; i++)
                    {
                        participant[i] = Participants[i].pp(Standard);
                    }
                    temp.Add(participant);
                    return temp;
                }
                else
                {
                    var temp = new Purple_2.ProSkiJumping();
                    var participant = new Purple_2.Participant[Participants.Length];
                    for (int i = 0; i < participant.Length; i++)
                    {
                        participant[i] = Participants[i].pp(Standard);
                    }
                    temp.Add(participant);
                    return temp;
                }
                
            }

        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            if (jumping is Purple_2.SkiJumping skijumping)
            {
                var temp = new SkiJumping_2_0(skijumping);
                SerializeXml(temp);
            }
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);

            using (var temp = new StreamReader(FilePath))
            {
                var serializer = new XmlSerializer(typeof(SkiJumping_2_0));
                var jumping = serializer.Deserialize(temp) as SkiJumping_2_0;
                var jumping1 = jumping.ss();

                return jumping1 as T;
            }
        }

        public class Participant3_2_0
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Places { get; set; }
            public double[] Marks { get; set; }
            public Participant3_2_0() { }
            public Participant3_2_0(Purple_3.Participant p)
            {
                Name = p.Name;
                Surname = p.Surname;
                Places = p.Places;
                Marks = p.Marks;
            }
            public Purple_3.Participant pp()
            {
                var temp = new Purple_3.Participant(Name, Surname);
                foreach (var x in Marks) temp.Evaluate(x);
                return temp;
            }
        }
        public class Skating_2_0
        {
            public double[] Moods { get; set; }
            public string Type { get; set; }
            public Participant3_2_0[] Participants { get; set; }
            public Skating_2_0() { }
            public Skating_2_0(Purple_3.Skating s)
            {
                Moods = s.Moods;
                Type = s.GetType().Name;
                Participants = new Participant3_2_0[s.Participants.Length];
                for (int i = 0; i < Participants.Length; i++)
                {
                    Participants[i] = new Participant3_2_0(s.Participants[i]);
                }
            }
            public Purple_3.Skating ss()
            {
                if (Type == "FigureSkating")
                {
                    var skating = new Purple_3.FigureSkating(Moods, false);
                    skating.Add(Participants.Select(x => x.pp()).ToArray());
                    Purple_3.Participant.SetPlaces(skating.Participants);
                    return skating;
                }

                else if (Type == "IceSkating")
                {
                    var skating = new Purple_3.IceSkating(Moods, false);
                    skating.Add(Participants.Select(x => x.pp()).ToArray());
                    Purple_3.Participant.SetPlaces(skating.Participants);
                    return skating;
                }
                else return null;
            }
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            var temp = new Skating_2_0(skating);
            SerializeXml(temp);
        }
        

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);

            using (var temp = new StreamReader(FilePath))
            {
                var serializer = new XmlSerializer(typeof(Skating_2_0));
                var skating = serializer.Deserialize(temp) as Skating_2_0;
                var skating1 = skating.ss();

                return skating1 as T;
            }
        }
        
        
        
        public class Sportsman_2_0
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }
            public Sportsman_2_0() { }
            public Sportsman_2_0(Purple_4.Sportsman s)
            {
                Name= s.Name;
                Surname= s.Surname;
                Time= s.Time;
            }
            public Purple_4.Sportsman ss()
            {
                var sportsman = new Purple_4.Sportsman(Name, Surname);
                sportsman.Run(Time);
                return sportsman;
            }
            public class Group_2_0
            {
                public string Name { get; set; }
                public Sportsman_2_0[] Sportsmen { get; set; }
                public Group_2_0() { }
                public Group_2_0(Purple_4.Group g)
                {
                    Name= g.Name;
                    Sportsmen = new Sportsman_2_0[g.Sportsmen.Length];
                    for (int i = 0; i < g.Sportsmen.Length; i++)
                    {
                        Sportsmen[i] = new Sportsman_2_0(g.Sportsmen[i]);
                    }

                }
                public Purple_4.Group gg()
                {
                    var group = new Purple_4.Group(Name);
                    group.Add(Sportsmen.Select(x => x.ss()).ToArray());
                    return group;
                }
            }
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            var temp = new Group_2_0(group);
            SerializeXml(temp);

        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);

            using (var temp = new StreamReader(FilePath))
            {
                var serializer = new XmlSerializer (typeof(Group_2_0));
                var group = serializer.Deserialize (temp) as Group_2_0;
                var group1 = group.gg();

                return group1;
            }
        }

        // public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        //{
        //    throw new NotImplementedException();
        //}
        public class Response_2_0
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept {  get; set; }
            public Response_2_0() { }
            public Response_2_0(Purple_5.Response r)
            {
                Animal = r.Animal;
                CharacterTrait = r.CharacterTrait;
                Concept = r.Concept;


            }
            public Purple_5.Response rr()
            {
                var temp = new Purple_5.Response(Animal, CharacterTrait, Concept);
                return temp;
            }
        }
        public class Researches_2_0
        {
            public string Name { get; set; }
            public Response_2_0[] Responses { get; set; }
            public Researches_2_0() { }
            public Researches_2_0(Purple_5.Research r)
            {
                Name = r.Name;
                Responses = new Response_2_0[r.Responses.Length];
                for (int i = 0; i < Responses.Length; i++)
                {
                    Responses[i] = new Response_2_0(r.Responses[i]);
                }

            }
            public Purple_5.Research rr()
            {
                var temp = new Purple_5.Research(Name);
                for (int i=0; i<Responses.Length; i++)
                {
                    var temp1 = new string[] { Responses[i].Animal, Responses[i].CharacterTrait, Responses[i].Concept };
                    temp.Add(temp1);
                }
                return temp;
            }
        }
        public class Report_2_0
        {
            public Researches_2_0[] Research {  get; set; }
            public Report_2_0() { }
            public Report_2_0 (Purple_5.Report r)
            {
                Research = new Researches_2_0[r.Researches.Length];
                for (int i =0; i<Research.Length; i++)
                {
                    Research[i] = new Researches_2_0(r.Researches[i]);
                }
            }
            public Purple_5.Report rr()
            {
                var temp = new Purple_5.Report();
                temp.AddResearch(Research.Select(x => x.rr()).ToArray());
                return temp;
            }
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);

            var temp = new Report_2_0(report);
            SerializeXml(temp);
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);

            using (var temp = new StreamReader(FilePath))
            {
                var serializer = new XmlSerializer(typeof(Report_2_0));
                var report = serializer.Deserialize(temp) as Report_2_0;
                var report1 = report.rr();

                return report1;
            }
        }
        
        
    }
}
