using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lab_7;
using static Lab_7.Purple_3;
using static Lab_9.PurpleXMLSerializer;
namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            XmlSerializer XMLserializer;
            using var writer = new StreamWriter(FilePath);
            if (obj is Purple_1.Participant p)
            {
                XMLserializer = new XmlSerializer(typeof(Participant_1));
                XMLserializer.Serialize(writer, new Participant_1(p));
            }
            else if (obj is Purple_1.Judge j)
            {
                XMLserializer = new XmlSerializer(typeof(Judge_1));
                XMLserializer.Serialize(writer, new Judge_1(j));
            }
            else if (obj is Purple_1.Competition c)
            {
                XMLserializer = new XmlSerializer(typeof(Competition_1));
                XMLserializer.Serialize(writer, new Competition_1(c));
            }
            writer.Close();
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);

            using var reader = new StreamReader(FilePath);

            if (typeof(T) == typeof(Purple_1.Participant))
            {
                var XMLserializer = new XmlSerializer(typeof(Participant_1));
                var DTO = (Participant_1)XMLserializer.Deserialize(reader);
                return (T)(Object)DTO.ADD_PARTICIPANT();
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                var XMLserializer = new XmlSerializer(typeof(Judge_1));
                var DTO = (Judge_1)XMLserializer.Deserialize(reader);
                return (T)(Object)DTO.ADD_JUDGE();
            }
            else if (typeof(T) == typeof(Purple_1.Competition))
            {
                var XMLserializer = new XmlSerializer(typeof(Competition_1));
                var DTO = (Competition_1)XMLserializer.Deserialize(reader);
                return (T)(Object)DTO.ADD_COMPETITION();
            }

            return null;
        }
        public class Participant_1
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int[][] Marks { get; set; }
            public Participant_1() { }
            public Participant_1(Purple_1.Participant person)
            {
                Name = person.Name;
                Surname = person.Surname;
                Coefs = person.Coefs;

                int[][] marks = new int[person.Marks.GetLength(0)][];
                for (int i = 0; i < person.Marks.GetLength(0); i++)
                {
                    marks[i] = new int[person.Marks.GetLength(1)];
                    for (int j = 0; j < person.Marks.GetLength(1); j++)
                    {
                        marks[i][j] = person.Marks[i, j];
                    }
                }
                Marks = marks;
            }
            public Purple_1.Participant ADD_PARTICIPANT()
            {
                var human = new Purple_1.Participant(Name, Surname);
                human.SetCriterias(Coefs);
                foreach (var mark in Marks) {
                    human.Jump(mark); 
                }
                return human;
            }
        }
        public class Judge_1
        {
            public string Name { get; set; }
            public int[] Marks { get; set; }
            public Judge_1() { }
            public Judge_1(Purple_1.Judge j)
            {
                Name = j.Name;
                Marks = j.Marks;
            }
            public Purple_1.Judge ADD_JUDGE()
            {
                return new Purple_1.Judge(Name, Marks);
            }
        }
        public class Competition_1
        {
            public Participant_1[] Participants { get; set; }
            public Judge_1[] Judges { get; set; }
            public Competition_1() { }
            public Competition_1(Purple_1.Competition competition)
            {
                Participants = competition.Participants.Select(part => new Participant_1(part)).ToArray();
                Judges = competition.Judges.Select(j => new Judge_1(j)).ToArray();
            }
            public Purple_1.Competition ADD_COMPETITION()
            {
                var competition = new Purple_1.Competition(Judges.Select(competition => competition.ADD_JUDGE()).ToArray());
                competition.Add(Participants.Select(competition => competition.ADD_PARTICIPANT()).ToArray());
                return competition;
            }
        }
        /////////////////////////////////////////////////
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            using var writer = new StreamWriter(FilePath);
            var XMLserializer = new XmlSerializer(typeof(SkiJumping));
            XMLserializer.Serialize(writer, new SkiJumping(jumping));
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);
            var XMLserializer = new XmlSerializer(typeof(SkiJumping));
            var DTO = (SkiJumping)XMLserializer.Deserialize(reader);
            return (T)(Object)DTO.Ski_Adding(); 
        }
        public class Participant_2
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }
            public Participant_2() { }
            public Participant_2(Purple_2.Participant person)
            {
                Name = person.Name;
                Surname = person.Surname;
                Distance = person.Distance;
                Marks = person.Marks;
            }
            public Purple_2.Participant Add_PARTICIPANT2(int stand)
            {
                var person = new Purple_2.Participant(Name, Surname);
                person.Jump(Distance, Marks, stand);
                return person;
            }
        }
        public class SkiJumping
        {
            public string Name { get; set; }
            public int Standard { get; set; }
            public Participant_2[] Participants { get; set; }
            public string Type { get; set; }
            public SkiJumping() { }
            public SkiJumping(Purple_2.SkiJumping x)
            {
                Name = x.Name;
                Standard = x.Standard;
                Participants = x.Participants.Select(y => new Participant_2(y)).ToArray();
                Type = x.GetType().Name;
            }
            public Purple_2.SkiJumping Ski_Adding()
            {
                Purple_2.SkiJumping jump;

                switch (Type)
                {
                    case "JuniorSkiJumping":
                        jump = new Purple_2.JuniorSkiJumping();
                        break;
                    case "ProSkiJumping":
                        jump = new Purple_2.ProSkiJumping();
                        break;
                    default: 
                        return null;
                            
                }
                jump.Add(Participants.Select(x => x.Add_PARTICIPANT2(Standard)).ToArray());
                return jump;
            }
        }
        //////////////////////////
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);

            using var writer = new StreamWriter(FilePath);
            var XMLserializer = new XmlSerializer(typeof(Skating));
            XMLserializer.Serialize(writer, new Skating(skating));
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);
            var XMLserializer = new XmlSerializer(typeof(Skating));
            var DTO = (Skating)XMLserializer.Deserialize(reader);
            return (T)(Object)DTO.Add_Skating(); // ffff
        }

        public class Participant_3
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Marks { get; set; }
            public int[] Places { get; set; }
            public Participant_3() { }
            public Participant_3(Purple_3.Participant person)
            {
                Name = person.Name;
                Surname = person.Surname;
                Marks = person.Marks;
                Places = person.Places;
            }
            public Purple_3.Participant Participant_back()
            {
                var p = new Purple_3.Participant(Name, Surname);
                foreach (var mark in Marks) {
                    p.Evaluate(mark);
                }
                return p;
            }
        }
        public class Skating
        {
            public Participant_3[] Participants { get; set; }
            public double[] Moods { get; set; }
            public string Type { get; set; }
            public Skating() { }
            public Skating(Purple_3.Skating skating)
            {
                Participants = skating.Participants.Select(x => new Participant_3(x)).ToArray();
                Moods = skating.Moods;
                Type = skating.GetType().Name;
            }
            public Purple_3.Skating Add_Skating()
            {
                Purple_3.Skating skating;


                switch (Type)
                {
                    case "FigureSkating":
                        skating = new Purple_3.FigureSkating(Moods, false);
                        break;
                    case "IceSkating":
                        skating = new Purple_3.IceSkating(Moods, false);
                        break;
                    default:
                        return null;
                }

                skating.Add(Participants.Select(x => x.Participant_back()).ToArray());
                Purple_3.Participant.SetPlaces(skating.Participants);
                return skating;
            }
        }
        //////////////////////////////
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            using var writer = new StreamWriter(FilePath);
            var XMLserializer = new XmlSerializer(typeof(Group));
            XMLserializer.Serialize(writer, new Group(group));
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);
            var XMLserializer = new XmlSerializer(typeof(Group));
            var DTO = (Group)XMLserializer.Deserialize(reader);
            return DTO.Group_back();
        }

        public class Sportsman_4
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }
            public Sportsman_4() { }
            public Sportsman_4(Purple_4.Sportsman man)
            {
                Name = man.Name;
                Surname = man.Surname;
                Time = man.Time;
            }
            public Purple_4.Sportsman Add_Sportsman()
            {
                var man = new Purple_4.Sportsman(Name, Surname);
                man.Run(Time);
                return man;
            }
        }
        public class Group
        {
            public string Name { get; set; }
            public Sportsman_4[] Sportsmen { get; set; }
            public Group() { }
            public Group(Purple_4.Group g)
            {
                Name = g.Name;
                Sportsmen = g.Sportsmen.Select(x => new Sportsman_4(x)).ToArray();
            }
            public Purple_4.Group Group_back()
            {
                var group = new Purple_4.Group(Name);
                group.Add(Sportsmen.Select(x => x.Add_Sportsman()).ToArray());
                return group;
            }
        }
        ////////////////// 
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            using var writer = new StreamWriter(FilePath);
            var XMLserializer = new XmlSerializer(typeof(Report));
            XMLserializer.Serialize(writer, new Report(report));
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);
            var XMLserializer = new XmlSerializer(typeof(Report));
            var DTO = (Report)XMLserializer.Deserialize(reader);
            return DTO.ADD_REPORT();
        }

        public class Response
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }
            public Response() { }
            public Response(Purple_5.Response r)
            {
                Animal = r.Animal;
                CharacterTrait = r.CharacterTrait;
                Concept = r.Concept;
            }
        }
        public class Research
        {
            public string Name { get; set; }
            public Response[] Responses { get; set; }
            public Research() { }
            public Research(Purple_5.Research r)
            {
                Name = r.Name;
                Responses = r.Responses.Select(x => new Response(x)).ToArray();
            }
            public Purple_5.Research ADD_RESEARCH()
            {
                var r = new Purple_5.Research(Name);
                foreach (var resp in Responses)
                    r.Add([resp.Animal, resp.CharacterTrait, resp.Concept]);
                return r;
            }
        }
        public class Report
        {
            public Research[] Researches { get; set; }
            public Report() { }
            public Report(Purple_5.Report r)
            {
                Researches = r.Researches.Select(x => new Research(x)).ToArray();
            }
            public Purple_5.Report ADD_REPORT()
            {
                var r = new Purple_5.Report();
                r.AddResearch(Researches.Select(x => x.ADD_RESEARCH()).ToArray());
                return r;
            }
        }

    }
}