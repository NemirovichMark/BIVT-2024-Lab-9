using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Lab_7;
using System.Xml.Linq;
using System.Numerics;

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


        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            if (typeof(T) == typeof(Purple_1.Judge)) 
            {
                var text = File.ReadAllText(FilePath);
                var pur = JsonConvert.DeserializeObject<Judge1>(text);
                Purple_1.Judge pur_judge = new Purple_1.Judge(pur.Name, pur.Marks);
                return pur_judge as T;

            }
            else if (typeof(T) == typeof(Purple_1.Competition))
            {
                var text = File.ReadAllText(FilePath);
                var pur = JsonConvert.DeserializeObject<Competition1>(text);

                Purple_1.Judge[] pur_judge = pur.Judges.Select(q => new Purple_1.Judge(q.Name, q.Marks)).ToArray();


                Purple_1.Competition pur_competition = new Purple_1.Competition(pur_judge);

                for(int i = 0; i < pur.Participants.Length; i++)
                {
                    Purple_1.Participant pur_partis = new Purple_1.Participant(pur.Participants[i].Name, pur.Participants[i].Surname);
                    pur_partis.SetCriterias(pur.Participants[i].Coefs);
                    for(int j = 0;  j < pur.Participants[i].Marks_length; j++)
                    {
                        int[] jump = new int[pur.Participants[i].Marks.Length / pur.Participants[i].Marks_length];
                        for (int z = 0; z < pur.Participants[i].Marks.Length / pur.Participants[i].Marks_length; z++)
                        {
                            jump[z] = pur.Participants[i].Marks[j * pur.Participants[i].Marks.Length / pur.Participants[i].Marks_length + z];
                            
                        }
                        pur_partis.Jump(jump);
                    }
                    pur_competition.Add(pur_partis);
                }
                return pur_competition as T;
            }
            else 
            {
                var text = File.ReadAllText(FilePath);
                var pur = JsonConvert.DeserializeObject<Participant1>(text);

                Purple_1.Participant pur_partis = new Purple_1.Participant(pur.Name, pur.Surname);
                pur_partis.SetCriterias(pur.Coefs);
                for (int j = 0; j < pur.Marks_length; j++)
                {
                    int[] jump = new int[pur.Marks.Length / pur.Marks_length];
                    for (int z = 0; z < pur.Marks.Length / pur.Marks_length; z++)
                    {
                        jump[z] = pur.Marks[j * pur.Marks.Length / pur.Marks_length + z];

                    }
                    pur_partis.Jump(jump);
                }
                return pur_partis as T;
            }

        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var pur = JsonConvert.DeserializeObject<SkiJumping2>(text);
            Purple_2.SkiJumping pur_SkiJumping;
            if (pur.Standard == 100)
            {
                pur_SkiJumping = new Purple_2.JuniorSkiJumping();
            }
            else
            {
                pur_SkiJumping = new Purple_2.ProSkiJumping();
            }
            for(int i = 0; i < pur.Participants.Length; i++)
            {
                Purple_2.Participant pur_partis = new Purple_2.Participant(pur.Participants[i].Name, pur.Participants[i].Surname);
                pur_partis.Jump(pur.Participants[i].Distance, pur.Participants[i].Marks, pur.Standard);
                pur_SkiJumping.Add(pur_partis);
            }
            return pur_SkiJumping as T;
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var pur = JsonConvert.DeserializeObject<Skating3>(text);
            Purple_3.Skating pur_skating;
            if(pur.Type == "FigureSkating")
            {
                pur_skating = new Purple_3.FigureSkating(pur.Moods, false);
            }
            else
            {
                pur_skating = new Purple_3.IceSkating(pur.Moods, false);
            }

            for (int i = 0; i < pur.Participants.Length; i++)
            {
                Purple_3.Participant pur_partis = new Purple_3.Participant(pur.Participants[i].Name, pur.Participants[i].Surname);
                for(int j = 0; j < pur.Participants[i].Marks.Length; j++)
                {
                    pur_partis.Evaluate(pur.Participants[i].Marks[j]);
                }

                pur_skating.Add(pur_partis);
            }
            return pur_skating as T;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var pur = JsonConvert.DeserializeObject<Group4>(text);
            Purple_4.Group pur_grop = new Purple_4.Group(pur.Name);
            for(int i = 0; i < pur.Sportsmen.Length; i++)
            {
                Purple_4.Sportsman pur_sport = new Purple_4.Sportsman(pur.Sportsmen[i].Name, pur.Sportsmen[i].Surname);
                pur_sport.Run(pur.Sportsmen[i].Time);
                pur_grop.Add(pur_sport);
            }
            return pur_grop;
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var text = File.ReadAllText(FilePath);
            var pur = JsonConvert.DeserializeObject<Report5>(text);
            Purple_5.Report pur_grop = new Purple_5.Report();
            for(int i = 0; i < pur.Researches.Length; i++)
            {
                Purple_5.Research pur_research = new Purple_5.Research(pur.Researches[i].Name);
                
                for (int j = 0; j < pur.Researches[i].Responses.Length; j++)
                {
                    pur_research.Add(new string[] { pur.Researches[i].Responses[j].Animal, pur.Researches[i].Responses[j].CharacterTrait, pur.Researches[i].Responses[j].Concept });

                }
                pur_grop.AddResearch(pur_research);
            }
            return pur_grop;
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            if (obj is Purple_1.Participant pur)
            {
                int[] ma = new int[pur.Marks.GetLength(0) * pur.Marks.GetLength(1)];
                int k = 0;
                for (int i = 0; i < pur.Marks.GetLength(0); i++)
                {
                    for (int j = 0; j < pur.Marks.GetLength(1); j++)
                    {
                        ma[k] = pur.Marks[i, j];
                        k++;
                    }
                }
                Participant1 jud = new Participant1
                {
                    Name = pur.Name,
                    Surname = pur.Surname,
                    Coefs = pur.Coefs,
                    Marks_length = pur.Marks.GetLength(0),
                    Marks = ma

                };
                string js = System.Text.Json.JsonSerializer.Serialize(jud);
                File.WriteAllText(FilePath, js);
            }
            else if (obj is Purple_1.Judge pur2)
            {
                Judge1 jud = new Judge1
                {
                    Name = pur2.Name,
                    Marks = pur2.Marks

                };
                string js = System.Text.Json.JsonSerializer.Serialize(jud);
                File.WriteAllText(FilePath, js);
            }
            else if (obj is Purple_1.Competition pur3)
            {
                Competition1 jud = new Competition1
                {
                    Participants = pur3.Participants.Select(q => new Participant1
                    {
                        Name = q.Name,
                        Surname = q.Surname,
                        Coefs = q.Coefs,
                        Marks_length = q.Marks.GetLength(0),
                        Marks = mam(q.Marks)
                    }).ToArray(),
                    Judges = pur3.Judges.Select(q => new Judge1
                    {
                        Name = q.Name,
                        Marks = q.Marks
                    }).ToArray(),

                };
                string js = System.Text.Json.JsonSerializer.Serialize(jud);
                File.WriteAllText(FilePath, js);
            }
        
        }
        private int[] mam(int[,] matric)
        {
            int[] ma = new int[matric.GetLength(0) * matric.GetLength(1)];
            int k = 0;
            for (int i = 0; i < matric.GetLength(0); i++)
            {
                for (int j = 0; j < matric.GetLength(1); j++)
                {
                    ma[k] = matric[i, j];
                    k++;
                }
            }
            return ma;
        }
        private class Judge1
        {
            public string Name { get; set; }
            public int[] Marks { get; set; }
        }
        private class Competition1
        {
            public Participant1[] Participants { get; set; }
            public Judge1[] Judges { get; set; }
        }
        private class Participant1
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int Marks_length { get; set; }
            public int[] Marks { get; set; }
        }


        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            if (jumping is Purple_2.SkiJumping pur2)
            {
                SkiJumping2 jum = new SkiJumping2
                {
                    Name = pur2.Name,
                    Standard = pur2.Standard,
                    Participants = pur2.Participants.Select(q => new Participant2
                    {
                        Name = q.Name,
                        Surname = q.Surname,
                        Distance = q.Distance,
                        Marks = q.Marks
                    }).ToArray()
                };
                string js = System.Text.Json.JsonSerializer.Serialize(jum);
                File.WriteAllText(FilePath, js);
            }

        }
        private class Participant2
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }
        }
        private class SkiJumping2
        {
            public string Name { get; set; }
            public int Standard { get; set; }
            public Participant2[] Participants { get; set; }
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            if (skating is Purple_3.Skating pur3)
            {
                Skating3 jum = new Skating3
                {
                    Type = pur3.GetType().Name,
                    Moods = pur3.Moods,
                    Participants = pur3.Participants.Select(q => new Participant3
                    {
                        Name = q.Name,
                        Surname = q.Surname,
                        Score = q.Score,
                        Marks = q.Marks,
                        Places = q.Places
                    }).ToArray()
                };
                string js = System.Text.Json.JsonSerializer.Serialize(jum);
                File.WriteAllText(FilePath, js);
            }
        }
        private class Participant3
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Score { get; set; }
            public double[] Marks { get; set; }
            public int[] Places { get; set; }
        }
        private class Skating3
        {
            public string Type { get; set; }
            public Participant3[] Participants { get; set; }
            public double[] Moods { get; set; }
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            if (group is Purple_4.Group pur4)
            {
                Group4 jum = new Group4
                {
                    Name = pur4.Name,
                    Sportsmen = pur4.Sportsmen.Select(q => new Sportsman4
                    { 
                        Type = pur4.GetType().Name,
                        Name = q.Name,
                        Surname = q.Surname,
                        Time = q.Time
                    }).ToArray()
                };
                string js = System.Text.Json.JsonSerializer.Serialize(jum);
                File.WriteAllText(FilePath, js);
            }

        }
        private class Group4
        {
            public string Name { get; set; }
            public Sportsman4[] Sportsmen { get; set; }
        }
        private class Sportsman4
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            if (report is Purple_5.Report pur5)
            {
                Report5 jum = new Report5
                {

                    Researches = pur5.Researches.Select(q => new Research5
                    {
                        Name = q.Name,
                        Responses = q.Responses.Select(w => new Response5
                        {
                            Animal = w.Animal,
                            CharacterTrait = w.CharacterTrait,
                            Concept = w.Concept
                        }).ToArray()
                    }).ToArray()
                };
                string js = System.Text.Json.JsonSerializer.Serialize(jum);
                File.WriteAllText(FilePath, js);
            }
        }
        private class Report5
        {
            public Research5[] Researches { get; set; }
        }
        private class Research5
        {
            public string Name { get; set; }
            public Response5[] Responses { get; set; }
        }
        private class Response5
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }
        }
    }
}
