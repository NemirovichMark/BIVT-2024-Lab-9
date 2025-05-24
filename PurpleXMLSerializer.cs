using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lab_7;

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

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            if (typeof(T) == typeof(Purple_1.Judge))
            {
                XmlSerializer pur_ser = new XmlSerializer(typeof(Judge1));
                Judge1 pur_jud;
                using (StreamReader read = new StreamReader(FilePath))
                {
                    pur_jud = (Judge1)pur_ser.Deserialize(read);
                }
                Purple_1.Judge pur_j = new Purple_1.Judge(pur_jud.Name, pur_jud.Marks);
                return pur_j as T;
            }
            else if (typeof(T) == typeof(Purple_1.Competition))
            {
                XmlSerializer pur_ser = new XmlSerializer(typeof(Competition1));
                Competition1 pur_com;
                using (StreamReader read = new StreamReader(FilePath))
                {
                    pur_com = (Competition1)pur_ser.Deserialize(read);
                }
                Purple_1.Competition pur_j = new Purple_1.Competition(pur_com.Judges.Select(q => new Purple_1.Judge(q.Name, q.Marks)).ToArray());
                
                for(int i = 0; i < pur_com.Participants.Length; i++)
                {
                    Purple_1.Participant pur_par = new Purple_1.Participant(pur_com.Participants[i].Name, pur_com.Participants[i].Surname);
                    pur_par.SetCriterias(pur_com.Participants[i].Coefs);
                    for (int j = 0; j < pur_com.Participants[i].Marks_length; j++)
                    {
                        int[] marks = new int[pur_com.Participants[i].Marks.Length / pur_com.Participants[i].Marks_length];
                        for (int z = 0; z < pur_com.Participants[i].Marks.Length / pur_com.Participants[i].Marks_length; z++)
                        {
                            marks[z] = pur_com.Participants[i].Marks[j * pur_com.Participants[i].Marks.Length / pur_com.Participants[i].Marks_length + z];
                        }
                        pur_par.Jump(marks);
                    }
                    pur_j.Add(pur_par);
                }
                return pur_j as T;
            }
            else
            {
                XmlSerializer pur_ser = new XmlSerializer(typeof(Participant1));
                Participant1 pur_par;
                using (StreamReader read = new StreamReader(FilePath))
                {
                    pur_par = (Participant1)pur_ser.Deserialize(read);
                }
                Purple_1.Participant par = new Purple_1.Participant(pur_par.Name, pur_par.Surname);
                par.SetCriterias(pur_par.Coefs);
                for (int j = 0; j < pur_par.Marks_length; j++)
                {
                    int[] marks = new int[pur_par.Marks.Length / pur_par.Marks_length];
                    for (int z = 0; z < pur_par.Marks.Length / pur_par.Marks_length; z++)
                    {
                        marks[z] = pur_par.Marks[j * pur_par.Marks.Length / pur_par.Marks_length + z];
                    }
                    par.Jump(marks);
                }
                return par as T;
            }

        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            XmlSerializer pur_ser = new XmlSerializer(typeof(SkiJumping2));
            SkiJumping2 pur_ski;
            using (StreamReader read = new StreamReader(FilePath))
            {
                pur_ski = (SkiJumping2)pur_ser.Deserialize(read);
            }
            Purple_2.SkiJumping ski;
            if(pur_ski.Standard == 100)
            {
               ski = new Purple_2.JuniorSkiJumping();
            }
            else
            {
                ski = new Purple_2.ProSkiJumping();
            }
            for (int i = 0; i < pur_ski.Participants.Length; i++) 
            { 
                Purple_2.Participant par = new Purple_2.Participant(pur_ski.Participants[i].Name, pur_ski.Participants[i].Surname);
                par.Jump(pur_ski.Participants[i].Distance, pur_ski.Participants[i].Marks, pur_ski.Standard);
                ski.Add(par);
            }
            return ski as T;
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            XmlSerializer pur_ser = new XmlSerializer(typeof(Skating3));
            Skating3 pur_skat;
            using (StreamReader read = new StreamReader(FilePath))
            {
                pur_skat = (Skating3)pur_ser.Deserialize(read);
            }
            Purple_3.Skating skat;
            if(pur_skat.Type == "FigureSkating")
            {
                skat = new Purple_3.FigureSkating(pur_skat.Moods, false);
            }
            else
            {
                skat = new Purple_3.IceSkating(pur_skat.Moods, false);
            }
            for (int i = 0; i < pur_skat.Participants.Length; i++)
            {
                Purple_3.Participant pur_partis = new Purple_3.Participant(pur_skat.Participants[i].Name, pur_skat.Participants[i].Surname);
                for (int j = 0; j < pur_skat.Participants[i].Marks.Length; j++)
                {
                    pur_partis.Evaluate(pur_skat.Participants[i].Marks[j]);
                }

                skat.Add(pur_partis);
            }
            return skat as T;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            XmlSerializer pur_ser = new XmlSerializer(typeof(Group4));
            Group4 pur_grop;
            using (StreamReader read = new StreamReader(FilePath))
            {
                pur_grop = (Group4)pur_ser.Deserialize(read);
            }
            Purple_4.Group grop = new Purple_4.Group(pur_grop.Name);

            for (int i = 0; i < pur_grop.Sportsmen.Length; i++) 
            {
                Purple_4.Sportsman spo = new Purple_4.Sportsman(pur_grop.Sportsmen[i].Name, pur_grop.Sportsmen[i].Surname);
                spo.Run(pur_grop.Sportsmen[i].Time);
                grop.Add(spo);
            }
            return grop; ;
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            XmlSerializer pur_ser = new XmlSerializer(typeof(Report5));
            Report5 pur_rep;
            using (StreamReader read = new StreamReader(FilePath))
            {
                pur_rep = (Report5)pur_ser.Deserialize(read);
            }
            Purple_5.Report report = new Purple_5.Report();
            for (int i = 0; i < pur_rep.Researches.Length; i++) 
            {
                Purple_5.Research res = new Purple_5.Research(pur_rep.Researches[i].Name);
                for (int j = 0; j < pur_rep.Researches[i].Responses.Length; j++)
                {
                    res.Add(new string[] { pur_rep.Researches[i].Responses[j].Animal, pur_rep.Researches[i].Responses[j].CharacterTrait, pur_rep.Researches[i].Responses[j].Concept });

                }
                report.AddResearch(res);
            }
            return report;

        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            if (obj is Purple_1.Participant pur)
            {
                Participant1 pur_par = new Participant1(pur);
                XmlSerializer xml_ser = new XmlSerializer(typeof(Participant1));
                using (StreamWriter writ = new StreamWriter(FilePath))
                {
                    xml_ser.Serialize(writ, pur_par);
                }

            }
            else if (obj is Purple_1.Judge pur2)
            {
                Judge1 pur_judge = new Judge1(pur2);
                XmlSerializer xml_ser = new XmlSerializer(typeof(Judge1));
                using (StreamWriter writ = new StreamWriter(FilePath))
                {
                    xml_ser.Serialize(writ, pur_judge);
                }
            }
            else if (obj is Purple_1.Competition pur3)
            {
                Competition1 pur_com = new Competition1(pur3);
                XmlSerializer xml_ser = new XmlSerializer(typeof(Competition1));
                using (StreamWriter writ = new StreamWriter(FilePath))
                {
                    xml_ser.Serialize(writ, pur_com);
                }
            }
        }
        public class Judge1
        {
            public string Name { get; set; }
            public int[] Marks { get; set; }
            public Judge1() { }
            public Judge1(Purple_1.Judge pur) 
            {
                Name = pur.Name;
                Marks = pur.Marks;
            }

        }
        public class Competition1
        {
            public Participant1[] Participants { get; set; }
            public Judge1[] Judges { get; set; }
            public Competition1() { }
            public Competition1(Purple_1.Competition pur) 
            {
                Judges = pur.Judges.Select(q =>  new Judge1(q)).ToArray();
                Participants = pur.Participants.Select(q => new Participant1(q)).ToArray();
            }
        }
        public class Participant1
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int Marks_length { get; set; }
            public int[] Marks { get; set; }
            public Participant1() { }
            public Participant1(Purple_1.Participant pur) 
            {
                Name = pur.Name;
                Surname = pur.Surname;
                Coefs = pur.Coefs;
                Marks_length = pur.Marks.GetLength(0);
                Marks = new int[Marks_length * pur.Marks.GetLength(1)];
                int k = 0;
                for (int i = 0; i < Marks_length; i++)
                {
                    for (int j = 0; j < pur.Marks.GetLength(1); j++)
                    {
                        Marks[k] = pur.Marks[i, j];
                        Console.WriteLine(Marks[k]);
                        k++;
                    }

                }
                Console.WriteLine(" ");
            }
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            SkiJumping2 pur_ski = new SkiJumping2(jumping);
            XmlSerializer xml_ser = new XmlSerializer(typeof(SkiJumping2));
            using (StreamWriter writ = new StreamWriter(FilePath))
            {
                xml_ser.Serialize(writ, pur_ski);
            }
        }
        public class Participant2
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }
            public Participant2() { }
            public Participant2(Purple_2.Participant pur2)
            {
                Name = pur2.Name;
                Surname = pur2.Surname;
                Distance = pur2.Distance;
                Marks = pur2.Marks;
            }
        }
        public class SkiJumping2
        {
            public string Name { get; set; }
            public int Standard { get; set; }
            public Participant2[] Participants { get; set; }
            public SkiJumping2() { }
            public SkiJumping2(Purple_2.SkiJumping pur2)
            {
                Name = pur2.Name;
                Standard = pur2.Standard;
                Participants = pur2.Participants.Select(q => new Participant2(q)).ToArray();
            }
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            Skating3 pur_ska = new Skating3(skating);
            XmlSerializer xml_ser = new XmlSerializer(typeof(Skating3));
            using (StreamWriter writ = new StreamWriter(FilePath))
            {
                xml_ser.Serialize(writ, pur_ska);
            }
        }
        public class Participant3
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Score { get; set; }
            public double[] Marks { get; set; }
            public int[] Places { get; set; }
            public Participant3() { }
            public Participant3(Purple_3.Participant pur3) 
            {
                Name = pur3.Name;
                Surname = pur3.Surname;
                Score = pur3.Score;
                Places = pur3.Places;
                Marks = pur3.Marks;
            }

        }
        public class Skating3
        {
            public string Type { get; set; }
            public Participant3[] Participants { get; set; }
            public double[] Moods { get; set; }
            public Skating3() { }
            public Skating3(Purple_3.Skating pur3) 
            {
                Moods = pur3.Moods;
                Type = pur3.GetType().Name;
                Participants = pur3.Participants.Select(q => new Participant3(q)).ToArray();
            }
        }


        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            Group4 pur_grop = new Group4(group);
            XmlSerializer xml_ser = new XmlSerializer(typeof(Group4));
            using (StreamWriter writ = new StreamWriter(FilePath))
            {
                xml_ser.Serialize(writ, pur_grop);
            }
        }
        public class Group4
        {
            public string Name { get; set; }
            public Sportsman4[] Sportsmen { get; set; }
            public Group4() { }
            public Group4(Purple_4.Group pur4) 
            {
                Name = pur4.Name;
                Sportsmen = pur4.Sportsmen.Select(q => new  Sportsman4(q)).ToArray();
            }
        }
        public class Sportsman4
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }
            public Sportsman4() { }
            public Sportsman4(Purple_4.Sportsman pur4) 
            {
                Type = pur4.GetType().Name;
                Name = pur4.Name;
                Surname = pur4.Surname;
                Time = pur4.Time;
            }
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            Report5 pur_rep = new Report5(report);
            XmlSerializer xml_ser = new XmlSerializer(typeof(Report5));
            using (StreamWriter writ = new StreamWriter(FilePath))
            {
                xml_ser.Serialize(writ, pur_rep);
            }
        }
        public class Report5
        {
            public Research5[] Researches { get; set; }
            public Report5() { }
            public Report5(Purple_5.Report pur5) 
            {
                Researches = pur5.Researches.Select(q => new Research5(q)).ToArray();
            }
        }
        public class Research5
        {
            public string Name { get; set; }
            public Response5[] Responses { get; set; }
            public Research5() { }
            public Research5(Purple_5.Research pur5) 
            {
                Name = pur5.Name;
                Responses = pur5.Responses.Select(q =>  new Response5(q)).ToArray();
            }
        }
        public class Response5
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }
            public Response5() { }
            public Response5(Purple_5.Response pur5) 
            {
                Animal = pur5.Animal;
                CharacterTrait = pur5.CharacterTrait;
                Concept = pur5.Concept;
            }

        }
    }
}
