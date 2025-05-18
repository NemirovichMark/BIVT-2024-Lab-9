using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Net.Http.Json;

namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension
        {
            get
            {
                return "json";
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
        private  void SerializeJudge(Purple_1.Judge obj, string fileName)
        {
            SelectFile(fileName);
            Judge objj = new Judge
            {
                Name = obj.Name,
                Marks = obj.Marks
            };
            //string json = System.Text.Json.JsonSerializer.Serialize(objj);
            //File.WriteAllText(FilePath, json);
            string json = JsonConvert.SerializeObject(objj);
            File.WriteAllText(FilePath, json);
        }
        private class Judge
        {
            public string Name { get; set; }
            public int[] Marks { get; set; }
        }
        private void SerializeCompetition(Purple_1.Competition obj, string fileName)
        {
            SelectFile(fileName);
            Competition objj = new Competition
            {
                Judges = obj.Judges.Select(j => new Judge
                {
                    Name = j.Name,
                    Marks = j.Marks
                }).ToArray(),
                Participants = obj.Participants.Select(p => new Participant1
                {
                    Name = p.Name,
                    Surname = p.Surname,
                    Coefs = p.Coefs,
                    M0 = p.Marks.GetLength(0),
                    M1 = p.Marks.GetLength(1),
                    Marks = matr(p.Marks)
                }).ToArray()
            };
            string json = System.Text.Json.JsonSerializer.Serialize(objj);
            File.WriteAllText(FilePath, json);
            //string json = JsonConvert.SerializeObject(objj);
            //File.WriteAllText(FilePath, json);
        }
        private int[] matr(int[,] Marks)
        {
            int[] m = new int[Marks.GetLength(0) * Marks.GetLength(1)];
            for (int i = 0, k = 0; i < Marks.GetLength(0); i++)
            {
                for (int j = 0; j < Marks.GetLength(1); j++, k++)
                {
                    m[k] = Marks[i, j];
                }
            }
            return m;
        }
        private class Competition
        {
            public Judge[] Judges { get; set; }
            public Participant1[] Participants { get; set; }
        }
        private void SerializeParticipant(Purple_1.Participant obj, string fileName)
        {
            SelectFile(fileName);
            int[] m = new int[obj.Marks.GetLength(0) * obj.Marks.GetLength(1)];
            for (int i = 0, k = 0; i < obj.Marks.GetLength(0); i++)
            {
                for (int j = 0; j < obj.Marks.GetLength(1); j++, k++)
                {
                    m[k] = obj.Marks[i, j];
                }
            }
            Participant1 objj = new Participant1
            {
                Name = obj.Name,
                Surname = obj.Surname,
                Coefs = obj.Coefs,
                M0 = obj.Marks.GetLength(0),
                M1 = obj.Marks.GetLength(1),
                Marks = m
            };
            string json = System.Text.Json.JsonSerializer.Serialize(objj);
            File.WriteAllText(FilePath, json);
            //string json = JsonConvert.SerializeObject(objj);
            //File.WriteAllText(FilePath, json);
        }
        private class Participant1
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int M0 { get; set; }
            public int M1 { get; set; }
            public int[] Marks { get; set; }
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            var obj = (Purple_2.SkiJumping)(object)jumping;
            SkiJumping objj = new SkiJumping
            {
                Name = obj.Name,
                Standard = obj.Standard,
                Participant = obj.Participants.Select(p => new Participant2
                {
                    Name = p.Name,
                    Surname = p.Surname,
                    Distance = p.Distance,
                    Marks = p.Marks
                }).ToArray()
            };
            Console.WriteLine("S");
            if (objj != null ) Console.WriteLine("Null");
            jumping.Print();
            string json = System.Text.Json.JsonSerializer.Serialize(objj);
            //string json = JsonConvert.SerializeObject(objj, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }
        private class SkiJumping
        {
            public string Name { get; set; }
            public int Standard { get; set; }
            public Participant2[] Participant { get; set; }
        }
        private class Participant2
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            if (skating is Purple_3.Skating obj)
            {
                Skating objj = new Skating
                {
                    Name = obj.GetType().Name,
                    Moods = obj.Moods,
                    Participants = obj.Participants.Select(p => new Participant3
                    {
                        Name = p.Name,
                        Surname = p.Surname,
                        Places = p.Places,
                        Marks = p.Marks
                    }).ToArray()
                };
                string json = System.Text.Json.JsonSerializer.Serialize(objj);
                File.WriteAllText(FilePath, json);
                //var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
                //File.WriteAllText(FilePath, json);
            }
        }
        private class Skating
        {
            public string Name { get; set; }
            public Participant3[] Participants { get; set; }
            public double[] Moods { get; set; }
        }
        private class Participant3
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Places { get; set; }
            public double[] Marks { get; set; }
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            Group objj = new Group
            {
                Name = group.Name,
                Sportsmen = group.Sportsmen.Select(s => new Sportsman
                {
                    TName = group.GetType().Name,
                    Name = s.Name,
                    Surname = s.Surname,
                    Time = s.Time
                }).ToArray()
            };
            string json = System.Text.Json.JsonSerializer.Serialize(objj);
            File.WriteAllText(FilePath, json);
            //var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            //File.WriteAllText(FilePath, json);
        }
        private class Group
        {
            public string Name { get; set; }
            public Sportsman[] Sportsmen { get; set; }
        }
        public class Sportsman
        {
            public string TName { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            Report objj = new Report
            {
                Researches = report.Researches.Select(rese => new Research
                {
                    Name = rese.Name,
                    Responses = rese.Responses.Select(resp => new Response
                    {
                        Animal = resp.Animal,
                        CharacterTrait = resp.CharacterTrait,
                        Concept = resp.Concept
                    }).ToArray()
                }).ToArray()
            };
            string json = System.Text.Json.JsonSerializer.Serialize(objj);
            File.WriteAllText(FilePath, json);
            //var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            //File.WriteAllText(FilePath, json);
        }
        private class Report
        {
            public Research[] Researches { get; set; }
        }
        private class Research
        {
            public string Name { get; set; }
            public Response[] Responses { get; set; }
        }
        private class Response
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            if (typeof(T) == typeof(Purple_1.Judge))
            {
                var content = File.ReadAllText(FilePath);
                var obj = JsonConvert.DeserializeObject<Judge>(content);

                Purple_1.Judge J = new Purple_1.Judge(obj.Name, obj.Marks);
                return J as T;
            }
            else if (typeof(T) == typeof(Purple_1.Competition))
            {
                var content = File.ReadAllText(FilePath);
                var obj = JsonConvert.DeserializeObject<Competition>(content);

                Purple_1.Judge[] Jud = obj.Judges.Select(j => new Purple_1.Judge(j.Name, j.Marks)).ToArray();
                Purple_1.Competition C = new Purple_1.Competition(Jud);
                for (int i = 0; i < obj.Participants.Length; i++)
                {
                    Purple_1.Participant P = new Purple_1.Participant(obj.Participants[i].Name, obj.Participants[i].Surname);
                    P.SetCriterias(obj.Participants[i].Coefs);
                    for (int j = 0; j < obj.Participants[i].M0; j++)
                    {
                        int[] M = new int[obj.Participants[i].M1];
                        for (int jj = 0; jj < obj.Participants[i].M1; jj++)
                        {
                            M[jj] = obj.Participants[i].Marks[j * obj.Participants[i].M1 + jj];
                        }
                        P.Jump(M);
                    }
                    C.Add(P);
                }
                return C as T;
            }
            else
            {
                var content = File.ReadAllText(FilePath);
                var obj = JsonConvert.DeserializeObject<Participant1>(content);

                Purple_1.Participant P = new Purple_1.Participant(obj.Name, obj.Surname);
                P.SetCriterias(obj.Coefs);
                for (int j = 0; j < obj.M0; j++)
                {
                    int[] M = new int[obj.M1];
                    for (int jj = 0; jj < obj.M1; jj++)
                    {
                        M[jj] = obj.Marks[j * obj.M1 + jj];
                    }
                    P.Jump(M);
                }
                return P as T;
            }
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var content = File.ReadAllText(FilePath);
            var obj = JsonConvert.DeserializeObject<SkiJumping>(content);

            if (obj == null)
            {
                Console.WriteLine("null");
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
            if (objj == null)
            {
                Console.WriteLine("null");
            }
            Console.WriteLine("D");
            if (objj != null) Console.WriteLine("Null");
            objj.Print();
            return (T)(object)objj;
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var content = File.ReadAllText(FilePath);
            var obj = JsonConvert.DeserializeObject<Skating>(content);

            Purple_3.Skating S;
            if (obj.Name == "FigureSkating")
            {
                S = new Purple_3.FigureSkating(obj.Moods, false);
            }
            else
            {
                S = new Purple_3.IceSkating(obj.Moods, false);
            }

            for (int i = 0; i < obj.Participants.Length; i++)
            {
                Purple_3.Participant P = new Purple_3.Participant(obj.Participants[i].Name, obj.Participants[i].Surname);
                for (int j = 0; j < obj.Participants[i].Marks.Length; j++)
                {
                    P.Evaluate(obj.Participants[i].Marks[j]);
                }
                S.Add(P);
            }
            return S as T;
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var content = File.ReadAllText(FilePath);
            var obj = JsonConvert.DeserializeObject<Group>(content);

            Purple_4.Group G = new Purple_4.Group(obj.Name);
            for (int i = 0; i < obj.Sportsmen.Length; i++)
            {
                Purple_4.Sportsman S = new Purple_4.Sportsman(obj.Sportsmen[i].Name, obj.Sportsmen[i].Surname); ;
                if (obj.Sportsmen[i].Time != 0)
                    S.Run(obj.Sportsmen[i].Time);
                
                //if (obj.Sportsmen[i].TName == "SkiMan")
                //{
                //    if (obj.Sportsmen[i].Time == 0)
                //        S = new Purple_4.SkiMan(obj.Sportsmen[i].Name, obj.Sportsmen[i].Surname);
                //    else
                //        S = new Purple_4.SkiMan(obj.Sportsmen[i].Name, obj.Sportsmen[i].Surname, obj.Sportsmen[i].Time);
                //}
                //else
                //{
                //    if (obj.Sportsmen[i].Time == 0)
                //        S = new Purple_4.SkiWoman(obj.Sportsmen[i].Name, obj.Sportsmen[i].Surname);
                //    else
                //        S = new Purple_4.SkiWoman(obj.Sportsmen[i].Name, obj.Sportsmen[i].Surname, obj.Sportsmen[i].Time);
                //}
                G.Add(S);
            }
            return G;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var content = File.ReadAllText(FilePath);
            var obj = JsonConvert.DeserializeObject<Report>(content);

            Purple_5.Report report = new Purple_5.Report();
            for (int i = 0; i < obj.Researches.Length; i++)
            {
                Purple_5.Research rese = new Purple_5.Research(obj.Researches[i].Name);
                for (int j = 0; j < obj.Researches[i].Responses.Length; j++)
                {
                    string[] s = { obj.Researches[i].Responses[j].Animal, obj.Researches[i].Responses[j].CharacterTrait, obj.Researches[i].Responses[j].Concept };
                    rese.Add(s);
                }
                report.AddResearch(rese);
            }
            return report;
        }
    }
}
