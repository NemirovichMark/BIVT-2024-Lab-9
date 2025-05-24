using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Lab_7;
using static Lab_7.Purple_3;

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

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            string[] text = File.ReadAllLines(FilePath);

            if (text == null) return null;

            if (text[0] == "Participant")
            {

                string name = text[1].Split(":")[1];
                string surname = text[2].Split(":")[1];
                double[] coef = text[3].Split(":")[1].Split(" ").Select(double.Parse).ToArray();
                //Console.WriteLine(.Join(" ", coef);
                var p = new Purple_1.Participant(name, surname);
                p.SetCriterias(coef);
                for (int i = 0; i < int.Parse(text[4]); i++)
                {
                    int[] mark = text[i + 6].Split(" ").Select(int.Parse).ToArray();

                    p.Jump(mark);
                }
                return p as T;

            }
            else if (text[0] == "Judge")
            {
                string name = text[1].Split(":")[1];
                int[] marks = text[2].Split(':')[1].Split(" ").Select(int.Parse).ToArray();
                var p = new Purple_1.Judge(name, marks);
                return p as T;
            }
            else
            {
                int jud_length = int.Parse(text[2]);
                int tot = 3;
                var p_total_judge = new Purple_1.Judge[jud_length];
                for (int i = 0; i < jud_length; i++) 
                {
                    string name = text[tot].Split(":")[1];
                    tot++;
                    Console.WriteLine(text[tot]);
                    int[] marks = text[tot].Split(':')[1].Split(" ").Select(int.Parse).ToArray();
                    
                    tot++;
                    var p = new Purple_1.Judge(name, marks);
                    p_total_judge[i] = p;
                }
                tot++;
                int part_length = int.Parse(text[tot]);
                tot++;
                
                var compitition = new Purple_1.Competition(p_total_judge);
                for (int j = 0; j < part_length; j++) 
                {
                    
                    string name = text[tot].Split(":")[1];
                    tot++;
                    string surname = text[tot].Split(":")[1];
                    tot++;
                    double[] coef = text[tot].Split(':')[1].Split(" ").Select(double.Parse).ToArray();
                    tot++;
                    int a = int.Parse(text[tot].Split(":")[1]);
                    tot++;
                    int b = int.Parse(text[tot].Split(":")[1]);
                    tot++;
                    Console.WriteLine(a);
                    Purple_1.Participant par = new Purple_1.Participant(name, surname);
                    par.SetCriterias(coef);
                    for(int i = 0; i < a; i++)
                    {
                        int[] massiv = text[tot].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                        tot++;
                        Console.WriteLine("aaaaaaaaaaaaa");
                        Console.WriteLine(string.Join(" ", massiv));
                        if(massiv == null)
                        {
                            Console.WriteLine("null");
                        }
                        //Console.WriteLine("aaaaaaaaaaaaa");
                        par.Jump(massiv);
                        
                        //Console.WriteLine("aaaaaaaaaaaaa");
                    }
                    compitition.Add(par);


                }
                return compitition as T;
                
            }

        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            string[] text = File.ReadAllLines(FilePath);
            if (text == null) return null;
            string name = text[0].Split(":")[1];
            int standart = int.Parse(text[1].Split(":")[1]);
            int part_lenght = int.Parse(text[2].Split(":")[1]);
            var partis = new Purple_2.Participant[part_lenght];
            int tot = 3;
            for(int i = 0; i < part_lenght; i++)
            {
                string name_part = text[tot].Split(':')[1];
                tot++;
                string surname = text[tot].Split(':')[1];
                tot++;
                int distanse = int.Parse(text[tot].Split(':')[1]);
                tot++;
                var marks = text[tot].Split(":")[1].Split(",").Select(int.Parse).ToArray();
                tot++;
                partis[i] = new Purple_2.Participant(name_part, surname);
                partis[i].Jump(distanse, marks, standart);
            }
            Purple_2.SkiJumping ski_jump;
            if (standart == 100) 
            {
                ski_jump = new Purple_2.JuniorSkiJumping();
            }
            else
            {
                ski_jump = new Purple_2.ProSkiJumping();
            }
            ski_jump.Add(partis);
            return ski_jump as T;

        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            string[] text = File.ReadAllLines(FilePath);
            int num = int.Parse(text[0].Split(":")[1]);
            var partis = new Purple_3.Participant[num];
            int tot = 1;
            for(int i = 0; i < num; i++)
            {
                string name = text[tot].Split(":")[1];
                tot++;
                string surnme = text[tot].Split(":")[1];
                tot++;
                double[] marks = text[tot].Split(":")[1].Split(" ").Select(double.Parse).ToArray();
                tot++;
                partis[i] = new Purple_3.Participant(name, surnme);
                for(int j = 0; j < marks.Length; j++)
                {
                    partis[i].Evaluate(marks[j]);
                }
            }
            double[] mood = text[tot].Split(":")[1].Split(" ").Select(double.Parse).ToArray();
            tot++;
            string nazvanie = text[tot];
            Purple_3.Skating ski;
            if(nazvanie == "FigureSkating")
            {
                ski = new Purple_3.FigureSkating(mood, false);
            }
            else
            {
                ski = new Purple_3.IceSkating(mood, false);
            }
            ski.Add(partis);
            return ski as T;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            string[] text = File.ReadAllLines(FilePath);
            string name = text[0].Split(":")[1];
            int num = int.Parse(text[1].Split(":")[1]);
            int tot = 2;
            var sport = new Purple_4.Sportsman[num];
            var grop = new Purple_4.Group(name);
            for(int i = 0; i < num; i++)
            {
                string name_sport = text[tot].Split(":")[1];
                tot++;
                string surname = text[tot].Split(":")[1];
                tot++;
                double time = double.Parse(text[tot].Split(":")[1]);
                tot++;
                sport[i] = new Purple_4.Sportsman(name_sport, surname);
                sport[i].Run(time);
            }
            grop.Add(sport);
            return grop;
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            string[] text = File.ReadAllLines(FilePath);
            int number_Researches = int.Parse(text[0].Split(":")[1]);
            int tot = 1;
            var research = new Purple_5.Research[number_Researches];
            var report = new Purple_5.Report();
            for (int i = 0; i < number_Researches; i++)
            {
                string name = text[tot].Split(":")[1];
                tot++;
                int number_Response = int.Parse(text[tot].Split(":")[1]);
                tot++;
                research[i] = new Purple_5.Research(name);
                for (int k = 0; k < number_Response; k++)
                {
                    string animal = text[tot].Split(":")[1];
                    tot++;
                    if (animal == "") animal = null;
                    string character = text[tot].Split(":")[1];
                    tot++;
                    if (character == "") character = null;
                    string concept = text[tot].Split(":")[1];
                    tot++;
                    if (concept == "") concept = null;
                    research[i].Add(new string[] { animal, character, concept});
                }
                
            }
            report.AddResearch(research);
            return report;

        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            
            if (obj is Purple_1.Participant par) 
            {
                File.WriteAllText(FilePath, "");
                using (StreamWriter writer = File.AppendText(FilePath)) 
                {

                    
                    writer.WriteLine($"Participant");
                    writer.WriteLine($"Name:{par.Name}");
                    writer.WriteLine($"Surname:{par.Surname}");
                    writer.WriteLine($"Coefs:{string.Join(" ", par.Coefs)}");
                    //writer.WriteLine($"TotalScore:{par.TotalScore}");
                    writer.WriteLine($"{par.Marks.GetLength(0)}");
                    writer.WriteLine($"{par.Marks.GetLength(1)}");
                    for (int i = 0; i < par.Marks.GetLength(0); i++)
                    {
                        for (int j = 0; j < par.Marks.GetLength(1); j++)
                        {

                            if (j == par.Marks.GetLength(1) - 1)
                            {
                                writer.Write($"{par.Marks[i, j]}{Environment.NewLine}");
                                Console.Write($"{par.Marks[i, j]}{Environment.NewLine}");
                            }
                            else
                            {
                                writer.Write($"{par.Marks[i, j]} ");
                                Console.Write($"{par.Marks[i, j]} ");
                            }
                        }
                        

                    }writer.WriteLine($"{Environment.NewLine}");
                    Console.WriteLine("");
                }
            }
            if(obj is Purple_1.Judge ju)
            {
                File.WriteAllText(FilePath, "");
                using (StreamWriter writer = File.AppendText(FilePath)) 
                {
                    writer.WriteLine($"Judge");
                    writer.WriteLine($"Name:{ju.Name}");
                    
                    writer.WriteLine($"Marks:{string.Join(" ", ju.Marks)}");
                }
            }
            if (obj is Purple_1.Competition com)
            {
                File.WriteAllText(FilePath, "");
                using (StreamWriter writer = File.AppendText(FilePath))
                {
                    writer.WriteLine($"Competition");
                    writer.WriteLine($"Judges");
                    writer.WriteLine($"{com.Judges.Length}"); // 3 строка
                    for (int i = 0; i < com.Judges.Length; i++)
                    {
                        writer.WriteLine($"Name:{com.Judges[i].Name}");
                        writer.WriteLine($"Marks:{string.Join(" ", com.Judges[i].Marks)}");
                    }
                    writer.WriteLine($"Participants"); // 3 + com.Judges.Length * 2
                    writer.WriteLine($"{com.Participants.Length}");                                   // 
                    for (int i = 0; i < com.Participants.Length; i++)
                    {
                        writer.WriteLine($"Name:{com.Participants[i].Name}");
                        writer.WriteLine($"Surname:{com.Participants[i].Surname}");
                        writer.WriteLine($"Coef:{string.Join(" ", com.Participants[i].Coefs)}");
                        writer.WriteLine($"GetLength(0):{com.Participants[i].Marks.GetLength(0)}");
                        writer.WriteLine($"GetLength(1):{com.Participants[i].Marks.GetLength(1)}");
                        for (int j = 0; j < com.Participants[i].Marks.GetLength(0); j++)
                        {
                            for (int z = 0; z < com.Participants[i].Marks.GetLength(1); z++)
                            {
                                if (z == com.Participants[i].Marks.GetLength(1) - 1)
                                {
                                    writer.Write($"{com.Participants[i].Marks[j, z]}{Environment.NewLine}");
                                }
                                else
                                {
                                    writer.Write($"{com.Participants[i].Marks[j, z]} ");
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine($"Name:{jumping.Name}");
                writer.WriteLine($"Standard:{jumping.Standard}");
                writer.WriteLine($"Participants_Length:{jumping.Participants.Length}");
                for(int i = 0; i < jumping.Participants.Length; i++)
                {
                    writer.WriteLine($"Name:{jumping.Participants[i].Name}");
                    writer.WriteLine($"Surname:{jumping.Participants[i].Surname}");
                    writer.WriteLine($"Distance:{jumping.Participants[i].Distance}");
                    writer.WriteLine($"Marks:{string.Join(",", jumping.Participants[i].Marks)}");

                }
            }
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            File.WriteAllText(FilePath, "");
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine($"Part:{skating.Participants.Length}");
                for(int i = 0; i <  skating.Participants.Length; i++)
                {
                    writer.WriteLine($"Name:{skating.Participants[i].Name}");
                    writer.WriteLine($"Surname:{skating.Participants[i].Surname}");
                    writer.WriteLine($"Mark:{string.Join(" ", skating.Participants[i].Marks)}");
                }
                writer.WriteLine($"Moodes:{string.Join(" ", skating.Moods)}");
                writer.WriteLine($"{skating.GetType().Name}");
            }
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine($"Name:{group.Name}");
                writer.WriteLine($"lenght:{group.Sportsmen.Length}");
                for(int i = 0; i < group.Sportsmen.Length; i++)
                {
                    writer.WriteLine($"Name:{group.Sportsmen[i].Name}");
                    writer.WriteLine($"Surname:{group.Sportsmen[i].Surname}");
                    writer.WriteLine($"Time:{group.Sportsmen[i].Time}");

                }
            }
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine($"Number_Researches:{report.Researches.Length}");
                for(int i = 0; i < report.Researches.Length; i++)
                {
                    writer.WriteLine($"Name:{report.Researches[i].Name}");

                    writer.WriteLine($"Number:{report.Researches[i].Responses.Length}");
                    for (int j = 0; j < report.Researches[i].Responses.Length; j++)
                    {
                        writer.WriteLine($"Animal:{report.Researches[i].Responses[j].Animal}");
                        writer.WriteLine($"CharacterTrait:{report.Researches[i].Responses[j].CharacterTrait}");
                        writer.WriteLine($"Concept:{report.Researches[i].Responses[j].Concept}");
                    }
                }
            }
        }
    }
}
