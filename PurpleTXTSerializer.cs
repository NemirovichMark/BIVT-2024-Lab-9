using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension
        {
            get
            {
                return "txt";
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
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine("Judge");
                writer.WriteLine($"Name: {obj.Name}");
                writer.WriteLine($"Marcs: {string.Join(" ", obj.Marks)}");
            }
        }
        private void SerializeCompetition(Purple_1.Competition obj, string fileName)
        {
            SelectFile(fileName);
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine("Competition");
                writer.WriteLine($"JCount: {obj.Judges.Length}");
                writer.WriteLine($"PCount: {obj.Participants.Length}");
                for (int i = 0; i < obj.Judges.Length; i++)
                {
                    writer.WriteLine($"Name: {obj.Judges[i].Name}");
                    writer.WriteLine($"Marcs: {string.Join(" ", obj.Judges[i].Marks)}");
                }
                for (int ii = 0; ii < obj.Participants.Length; ii++)
                {
                    writer.WriteLine($"Name: {obj.Participants[ii].Name}");
                    writer.WriteLine($"Surname: {obj.Participants[ii].Surname}");
                    writer.WriteLine($"TotalScore: {obj.Participants[ii].TotalScore}");
                    writer.WriteLine($"Coefs: {string.Join(" ", obj.Participants[ii].Coefs)}");
                    writer.WriteLine($"MarksCount0: {obj.Participants[ii].Marks.GetLength(0)}");
                    writer.WriteLine($"MarksCount1: {obj.Participants[ii].Marks.GetLength(1)}");

                    for (int i = 0; i < obj.Participants[ii].Marks.GetLength(0); i++)
                    {
                        int[] p = new int[obj.Participants[ii].Marks.GetLength(1)];
                        for (int j = 0; j < obj.Participants[ii].Marks.GetLength(1); j++)
                        {
                            p[j] = obj.Participants[ii].Marks[i, j];
                        }
                        writer.WriteLine(string.Join(" ", p));
                    }
                }

            }
        }
        private void SerializeParticipant(Purple_1.Participant obj, string fileName)
        {
            SelectFile(fileName);
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine("Participant");
                writer.WriteLine($"Name: {obj.Name}");
                writer.WriteLine($"Surname: {obj.Surname}");
                writer.WriteLine($"TotalScore: {obj.TotalScore}");
                writer.WriteLine($"Coefs: {string.Join(" ", obj.Coefs)}");
                writer.WriteLine($"MarksCount0: {obj.Marks.GetLength(0)}");
                writer.WriteLine($"MarksCount1: {obj.Marks.GetLength(1)}");

                for (int i = 0; i < obj.Marks.GetLength(0); i++)
                {
                    int[] p = new int[obj.Marks.GetLength(1)];
                    for (int j = 0; j < obj.Marks.GetLength(1); j++)
                    {
                        p[j] = obj.Marks[i, j];
                    }
                    writer.WriteLine(string.Join(" ", p));
                }
            }
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            Console.WriteLine("SERIALIZE");
            SelectFile(fileName);
            File.WriteAllText(FilePath, "");
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine($"Name: {jumping.Name}");
                writer.WriteLine($"Standard: {jumping.Standard}");
                writer.WriteLine($"PCount: {jumping.Participants.Length}");
                for (int i = 0; i < jumping.Participants.Length; i++)
                {
                    writer.WriteLine($"Name: {jumping.Participants[i].Name}");
                    writer.WriteLine($"Surname: {jumping.Participants[i].Surname}");
                    writer.WriteLine($"Distance: {jumping.Participants[i].Distance}");
                    writer.WriteLine($"Marks: {string.Join(" ", jumping.Participants[i].Marks)}");
                }
            }
            //Console.WriteLine("SERIALIZE");
            //jumping.Print();
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            if (skating is Purple_3.Skating obj)
            {
                using (StreamWriter writer = File.AppendText(FilePath))
                {
                    if (obj is Purple_3.FigureSkating)
                    {
                        writer.WriteLine("FigureSkating");
                    }
                    else
                    {
                        writer.WriteLine("IceSkating");
                    }
                    writer.WriteLine($"Moods: {string.Join(" ", obj.Moods)}");
                    writer.WriteLine($"PCount: {obj.Participants.Length}");
                    for (int i = 0; i < obj.Participants.Length; i++)
                    {
                        writer.WriteLine($"Name: {obj.Participants[i].Name}");
                        writer.WriteLine($"Surname: {obj.Participants[i].Surname}");
                        writer.WriteLine($"Marks: {string.Join(" ", obj.Participants[i].Marks)}");
                    }
                }
            }
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine($"Name: {group.Name}");
                writer.WriteLine($"SCount: {group.Sportsmen.Length}");
                for (int i = 0; i < group.Sportsmen.Length; i++)
                {
                    writer.WriteLine($"Name: {group.Sportsmen[i].Name}");
                    writer.WriteLine($"Surname: {group.Sportsmen[i].Surname}");
                    writer.WriteLine($"Time: {group.Sportsmen[i].Time}");
                }
            }
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            File.WriteAllText(FilePath, "");

            Console.WriteLine("S");
            foreach (var r in report.Researches)
            {
                foreach (var rr in r.Responses)
                {
                    Console.WriteLine($"{rr.Animal} {rr.CharacterTrait} {rr.Concept}");
                }
            }

            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine($"ResearchCount: {report.Researches.Length}");
                for (int i = 0; i < report.Researches.Length; i++)
                {
                    writer.WriteLine($"Name: {report.Researches[i].Name}");
                    writer.WriteLine($"ResponseCount: {report.Researches[i].Responses.Length}");
                    for (int j = 0; j < report.Researches[i].Responses.Length; j++)
                    {
                        writer.WriteLine($"Animal: {report.Researches[i].Responses[j].Animal}");
                        writer.WriteLine($"CharacterTrait: {report.Researches[i].Responses[j].CharacterTrait}");
                        writer.WriteLine($"Concept: {report.Researches[i].Responses[j].Concept}");
                    }
                }
            }
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            string[] s = File.ReadAllLines(FilePath);
            if (s == null) return null;

            if (s[0] == "Judge")
            {
                string Name = s[1].Substring(6);
                int[] Marks = s[2].Substring(7).Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                Purple_1.Judge J = new Purple_1.Judge(Name, Marks);
                return J as T;
            }
            else if (s[0] == "Competition")
            {
                int JCount = int.Parse(s[1].Substring(8));
                int PCount = int.Parse(s[2].Substring(8));
                Purple_1.Judge[] JJ = new Purple_1.Judge[JCount];
                Purple_1.Participant[] PP = new Purple_1.Participant[PCount];
                int k = 3;
                for (int i = 0; i < JCount; i++)
                {
                    string Name = s[k].Substring(6); k++;
                    int[] Marks = s[k].Substring(7).Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray(); k++;
                    Purple_1.Judge J = new Purple_1.Judge(Name, Marks);
                    JJ[i] = J;
                }
                for (int il = 0; il < PCount; il++)
                {
                    string Name = s[k].Substring(6); k++;
                    string Surname = s[k].Substring(9); k++;
                    double TotalScore = double.Parse(s[k].Substring("TotalScore: ".Length)); k++;
                    double[] Coefs = s[k].Substring("Coefs: ".Length).Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray(); k++;
                    int M0 = int.Parse(s[k].Substring("MarksCount0: ".Length)); k++;
                    int M1 = int.Parse(s[k].Substring("MarksCount0: ".Length)); k++;
                    Purple_1.Participant P = new Purple_1.Participant(Name, Surname);
                    P.SetCriterias(Coefs);
                    for (int i = 0; i < M0; i++)
                    {
                        int[] p = s[k].Split(' ').Select(int.Parse).ToArray();
                        P.Jump(p);
                        k++;
                    }
                    PP[il] = P;
                }
                Purple_1.Competition C = new Purple_1.Competition(JJ);
                C.Add(PP);
                return C as T;
            }
            else
            {
                string Name = s[1].Substring(6);
                string Surname = s[2].Substring(9);
                double TotalScore = double.Parse(s[3].Substring("TotalScore: ".Length));
                double[] Coefs = s[4].Substring("Coefs: ".Length).Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray();
                int M0 = int.Parse(s[5].Substring("MarksCount0: ".Length));
                int M1 = int.Parse(s[6].Substring("MarksCount0: ".Length));
                Purple_1.Participant P = new Purple_1.Participant(Name, Surname);
                P.SetCriterias(Coefs);
                for (int i = 7, ii = 0; i <= 6 + M0; i++, ii++)
                {
                    int[] p = s[i].Split(' ').Select(int.Parse).ToArray();
                    P.Jump(p);
                }
                return P as T;
            }
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            Console.WriteLine("DESERIALIZE");
            SelectFile(fileName);
            string[] s = File.ReadAllLines(FilePath);
            if (s == null) return null;
            string Name = s[0].Substring(6);
            int Standard = int.Parse(s[1].Substring(10));
            int PCount = int.Parse(s[2].Substring(8));
            
            Purple_2.SkiJumping SJ;
            if (Standard == 100)
            {
                SJ = new Purple_2.JuniorSkiJumping();
            }
            else
            {
                SJ = new Purple_2.ProSkiJumping();
            }
            
            for (int j = 3; j < 2 + PCount * 4; j += 4)
            {
                string PN = s[j].Substring(6);
                string PS = s[j + 1].Substring(9);
                int PD = int.Parse(s[j + 2].Substring(10));
                int[] PM = s[j + 3].Substring(7).Split(" ").Select(int.Parse).ToArray();
                Console.WriteLine(s[j + 3]);
                Purple_2.Participant p = new Purple_2.Participant(PN, PS);
                p.Jump(PD, PM, Standard);
                SJ.Add(p);
            }
            //for (int j = 0; j < s.Length; j++)
            //{
            //    if (s[j][0] == 'M')
            //    {
            //        Console.WriteLine(s[j].Substring(7));
            //        Console.WriteLine(string.Join(" ", s[j].Substring(7).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()));
            //    }
            //}
            //Console.WriteLine("DESERIALIZE");
            //SJ.Print();
            return (T)SJ;

        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            string[] s = File.ReadAllLines(FilePath);
            if (s == null) return null;
            string Skating = s[0];
            double[] Moods = s[1].Substring(7).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray();
            int PCount = int.Parse(s[2].Substring(7));
            Purple_3.Skating S;
            if (Skating == "FigureSkating")
            {
                S = new Purple_3.FigureSkating(Moods, false);
            }
            else
            {
                S = new Purple_3.IceSkating(Moods, false);
            }
            for (int i = 3; i < 3 + PCount * 3;)
            {
                string PName = s[i].Substring(6); i++;
                string PSurname = s[i].Substring(9); i++;
                double[] PMarks = s[i].Substring(7).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray(); i++;
                Purple_3.Participant p = new Purple_3.Participant(PName, PSurname);
                foreach (double m in PMarks)
                {
                    p.Evaluate(m);
                }
                S.Add(p);
            }
            return (T)S;
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            string[] s = File.ReadAllLines(FilePath);
            if (s == null) return null;

            string Name = s[0].Substring(6);
            Purple_4.Group G = new Purple_4.Group(Name);
            int SCount = int.Parse(s[1].Substring(8));
            for (int i = 2; i < 2 + SCount * 3; i += 3)
            {
                string SName = s[i].Substring(6);
                string SSurname = s[i + 1].Substring(9);
                double STime = double.Parse(s[i + 2].Substring(6));
                Purple_4.Sportsman S = new Purple_4.Sportsman(SName, SSurname);
                S.Run(STime);
                //if (m == "SkiMan")
                //{
                //    if (s[i].Substring(6) != "")
                //    {
                //        S = new Purple_4.SkiMan(SName, SSurname, STime);
                //    }
                //    else
                //    {
                //        S = new Purple_4.SkiMan(SName, SSurname);
                //    }
                //}
                //else
                //{
                //    if (s[i].Substring(6) != "")
                //    {
                //        S = new Purple_4.SkiWoman(SName, SSurname, STime);
                //    }
                //    else
                //    {
                //        S = new Purple_4.SkiWoman(SName, SSurname);
                //    }
                //}
                G.Add(S);
            }
            return G;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            string[] s = File.ReadAllLines(FilePath);
            if (s == null) return null;

            Purple_5.Report R = new Purple_5.Report();
            int ResearchCount = 0;
            if (s[0].Substring("ResearchCount: ".Length) != "")
            {
                ResearchCount = int.Parse(s[0].Substring("ResearchCount: ".Length));
            }
            for (int i = 0, k = 1; i < ResearchCount; i++)
            {
                string ResearchName = s[k].Substring(6); k++;
                Purple_5.Research research = new Purple_5.Research(ResearchName);
                int ResponseCount = 0;
                if (s[k].Substring("ResponseCount: ".Length) != "")
                {
                    ResponseCount = int.Parse(s[k].Substring("ResponseCount: ".Length));
                }
                k++;
                for (int j = 0; j < ResponseCount; j++)
                {
                    string ResponseAnimal = s[k].Substring("Animal: ".Length); k++;
                    string ResponseCharacterTrait = s[k].Substring("CharacterTrait: ".Length); k++;
                    string ResponseConcept = s[k].Substring("Concept: ".Length); k++;
                    if (ResponseAnimal == "") ResponseAnimal = null;
                    if (ResponseCharacterTrait == "") ResponseCharacterTrait = null;
                    if (ResponseConcept == "") ResponseConcept = null;
                    research.Add(new[] { ResponseAnimal, ResponseCharacterTrait, ResponseConcept });
                }
                R.AddResearch(research);
            }

            Console.WriteLine("D");
            foreach (var r in R.Researches)
            {
                foreach (var rr in r.Responses)
                {
                    Console.WriteLine($"{rr.Animal} {rr.CharacterTrait} {rr.Concept}");
                }
            }

            return R;
        }
    }
}
