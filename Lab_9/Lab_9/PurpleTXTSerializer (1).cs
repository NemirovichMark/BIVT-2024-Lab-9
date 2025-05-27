using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Lab_7.Purple_1;
using static Lab_7.Purple_5;


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
            SelectFile(fileName);
            //string text = "";

            if (obj is Purple_1.Participant participant)
            {
                Serialize_Participant(participant, fileName);
            }
            else if (obj is Purple_1.Judge judge)
            {
                Serialize_Judge(judge, fileName);
            }
            else if (obj is Purple_1.Competition competition)
            {
                Serialize_Competition(competition, fileName);
            }
            else return;
        }
        private void Serialize_Participant(Purple_1.Participant participant, string fileName)
        {
            using (StreamWriter sw = new StreamWriter(FilePath))
            {//информация записывается в поток. Это и есть процесс сериализации
                sw.WriteLine("Participant");//text[0]
                sw.WriteLine(participant.Name);//text[1]
                sw.WriteLine(participant.Surname);//text[2]
                var b = String.Join(" ", participant.Coefs);
                sw.WriteLine(b);//text[3]
                                // var b1 = String.Join(" ", participant.Marks);
                                // sw.WriteLine(b1);//text[4]
                var marks = "";
                for (int i = 0; i < participant.Marks.GetLength(0); i++)
                {
                    var line = "";
                    for (int j = 0; j < participant.Marks.GetLength(1); j++)
                    {
                        line += $"{participant.Marks[i, j]} ";
                    }
                    marks += line.TrimEnd() + ';';
                }
                sw.WriteLine(marks.TrimEnd(';'));//text[5]



            }
        }
        private void Serialize_Judge(Purple_1.Judge judge, string fileName)
        {
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                sw.WriteLine("Judges");
                sw.WriteLine(judge.Name);
                var b = String.Join(" ", judge.Marks);
                sw.WriteLine(b);
            }
        }


        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            string[] text = File.ReadAllLines(FilePath);
            if (text.Length == 0) return default;

            if (text[0] == "Participant")
            {
                if (text.Length < 4) return default;
                string name_p = text[1];
                string surname_p = text[2];
                double[] coefs_p = text[3].Split(' ').Select(x => double.Parse(x)).ToArray();

                var marks_lines = text[4].Split(';');
                var participant = new Purple_1.Participant(name_p, surname_p);
                participant.SetCriterias(coefs_p);

                foreach (var line in marks_lines)
                {
                    var marks_line = line.Split(' ').Select(int.Parse).ToArray();
                    participant.Jump(marks_line);
                }

                return participant as T;
            }

            else if (text[0] == "Judges")
            {
                string name_j = text[1];
                var marks_j = text[2].Split(' ').Select(int.Parse).ToArray();

                var judges = new Purple_1.Judge(name_j, marks_j);
                return judges as T;
            }
            else if (text[0] == "Competition")
            {
                int kol_par = int.Parse(text[1]);
                var participants = new Purple_1.Participant[kol_par];
                int z = 2;
                for (int i = 0; i < participants.Length; i++)
                {
                    string name_c = text[z++];
                    string surname_c = text[z++];
                    var coefs_c = text[z++].Split(' ').Select(double.Parse).ToArray();
                    var marks_c = text[z++].Split(';');

                    var participant = new Purple_1.Participant(name_c, surname_c);
                    participant.SetCriterias(coefs_c);
                    //participant.Jump(marks_c);

                    foreach (var line in marks_c)
                    {
                        var marks_line = line.Split(' ').Select(int.Parse).ToArray();
                        participant.Jump(marks_line);
                    }

                    participants[i] = participant;
                }
                int kol_jud = int.Parse(text[z++]);
                var judges = new Purple_1.Judge[kol_jud];
                //int s = z + 2;
                for (int i = 0; i < judges.Length; i++)
                {
                    string name_c = text[z++];
                    var marks_c = text[z++].Split(' ').Select(int.Parse).ToArray();

                    var judge = new Purple_1.Judge(name_c, marks_c);
                    judges[i] = judge;
                }
                var competition = new Purple_1.Competition(judges);
                competition.Add(participants);

                return competition as T;
            }
            else return null;
        }
        private void Serialize_Competition(Purple_1.Competition competition, string fileName)
        {
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                sw.WriteLine("Competition");//text[0]

                //sw.WriteLine("Participants");
                sw.WriteLine(competition.Participants.Length);//text[1]

                foreach (var p in competition.Participants)
                {

                    sw.WriteLine(p.Name);
                    sw.WriteLine(p.Surname);
                    var b = String.Join(" ", p.Coefs);
                    sw.WriteLine(b);
                    var marks = "";
                    for (int i = 0; i < p.Marks.GetLength(0); i++)
                    {
                        var line = "";
                        for (int j = 0; j < p.Marks.GetLength(1); j++)
                        {
                            line += $"{p.Marks[i, j]} ";
                        }
                        marks += line.TrimEnd() + ';';
                    }
                    sw.WriteLine(marks.TrimEnd(';'));
                    //Serialize_Participant(p, fileName);
                }

                sw.WriteLine(competition.Judges.Length);

                foreach (var j in competition.Judges)
                {
                    sw.WriteLine(j.Name);
                    var b = String.Join(" ", j.Marks);
                    sw.WriteLine(b);
                }

            }
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                sw.WriteLine(jumping.Name);//text[0]
                sw.WriteLine(jumping.Standard);//text[1]
                sw.WriteLine(jumping.Participants.Length);//text[2]
                foreach (var p in jumping.Participants)
                {
                    sw.WriteLine(p.Name);

                    sw.WriteLine(p.Surname);

                    sw.WriteLine(p.Distance);

                    sw.WriteLine(p.Result);


                    var b1 = String.Join(" ", p.Marks);
                    sw.WriteLine(b1);
                }

            }
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);

            string[] text = File.ReadAllLines(FilePath);
            if (text == null) return null;

            string name = text[0];
            int standard = int.Parse(text[1]);
            int length = int.Parse(text[2]);

            var participant = new Purple_2.Participant[length];
            int z = 3;
            for (int i = 0; i < length; i++)
            {
                string name_p = text[z++];
                string surname_p = text[z++];
                int distance_p = int.Parse(text[z++]);
                int result_p = int.Parse(text[z++]);

                var marks_p = text[z++].Split(' ').Select(int.Parse).ToArray(); //строка с массивчиком оценок делится по пробелам, преобразуется в числа и засовывается в массив
                participant[i] = new Purple_2.Participant(name_p, surname_p);
                participant[i].Jump(distance_p, marks_p, standard);
            }

            Purple_2.SkiJumping skijumping;
            if (standard == 100)
            {
                skijumping = new Purple_2.JuniorSkiJumping();
            }
            else if (standard == 150)
            {
                skijumping = new Purple_2.ProSkiJumping();
            }
            else return null;
            skijumping.Add(participant);
            return skijumping as T;
        }


        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                var b = String.Join(" ", skating.Moods);//text[0]
                sw.WriteLine(b);
                sw.WriteLine(skating.Participants.Length);//text[1]
                foreach (var p in skating.Participants)
                {
                    sw.WriteLine(p.Name);
                    sw.WriteLine(p.Surname);

                    var b1 = String.Join(" ", p.Marks);
                    sw.WriteLine(b1);

                }
                if (skating is Purple_3.FigureSkating) sw.WriteLine("FigureSkating");
                else sw.WriteLine("IceSkating");
            }
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            string[] text = File.ReadAllLines(FilePath);

            var moods = text[0].Split(' ').Select(double.Parse).ToArray();
            int kol_par = int.Parse(text[1]);

            int z = 2;
            var participants = new Purple_3.Participant[kol_par];
            for (int i = 0; i < kol_par; i++)
            {
                string name_p = text[z++];
                string surname_p = text[z++];
                var marks_p = text[z++].Split(' ').Select(double.Parse).ToArray();

                var participant = new Purple_3.Participant(name_p, surname_p);
                participants[i] = participant;
                for (int j = 0; j < marks_p.Length; j++)
                {
                    participants[i].Evaluate(marks_p[j]);

                }

            }
            Purple_3.Skating skating;
            if (text[z] == "FigureSkating") skating = new Purple_3.FigureSkating(moods, false);
            else skating = new Purple_3.IceSkating(moods, false);
            skating.Add(participants);

            return skating as T;

        }


        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                sw.WriteLine(group.Name);//text[0]
                sw.WriteLine(group.Sportsmen.Length);//text[1]
                foreach (var s in group.Sportsmen)
                {
                    sw.WriteLine(s.Name);

                    sw.WriteLine(s.Surname);

                    sw.WriteLine(s.Time);

                }
            }
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            string[] text = File.ReadAllLines(FilePath);
            if (text == null) return null;

            string name_g = text[0];
            int kol_spo = int.Parse(text[1]);
            int z = 2;
            var sportsmen = new Purple_4.Sportsman[kol_spo];
            for (int i = 0; i < sportsmen.Length; i++)
            {
                string name_s = text[z++];
                string surname_s = text[z++];
                double time_s = double.Parse(text[z++]);

                var sportsman = new Purple_4.Sportsman(name_s, surname_s);
                sportsman.Run(time_s);
                sportsmen[i] = sportsman;
            }
            var group = new Purple_4.Group(name_g);
            group.Add(sportsmen);

            return group;


        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(report.Researches.Length);
                foreach (var research in report.Researches)
                {
                    writer.Write(research.Name);
                    writer.Write("|");
                    for (int i = 0; i < research.Responses.Length; i++)
                    {
                        var response = research.Responses[i];
                        writer.Write(response.Animal);
                        writer.Write(";");
                        writer.Write(response.CharacterTrait);
                        writer.Write(";");
                        writer.Write(response.Concept);
                        if (i != research.Responses.Length - 1)
                            writer.Write(':');
                    }
                    writer.WriteLine();
                }
            }
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);

            string[] text = File.ReadAllLines(FilePath);
            int r_count = int.Parse(text[0]);
            Purple_5.Research[] researches = new Purple_5.Research[r_count];

            for (int i = 1; i < text.Length; i++)
            {
                string[] line = text[i].Split('|');
                string name = line[0];
                string[] responses = line[1].Split(':');
                Purple_5.Research research = new Purple_5.Research(name);
                foreach (string res in responses)
                {
                    string[] answers = res.Split(';');
                    if (answers != null)
                    {
                        for (int j = 0; j < answers.Length; j++)
                        {
                            if (string.IsNullOrEmpty(answers[j]))
                                answers[j] = null;
                        }
                    }
                    research.Add(answers);
                }
                researches[i - 1] = research;
            }

            Purple_5.Report report = new Purple_5.Report();
            report.AddResearch(researches);
            return report;
        }
    }

        /*
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                sw.WriteLine(report.Researches.Length);//text[0]
                foreach (var r in report.Researches)
                {

                    sw.WriteLine(r.Name);//text[1]
                    sw.WriteLine(r.Responses.Length);//text[2]
                    for (int i = 0; i < r.Responses.Length; i++)
                    {
                        sw.WriteLine(r.Responses[i].Animal);
                        sw.WriteLine(r.Responses[i].CharacterTrait);
                        sw.WriteLine(r.Responses[i].Concept);

                    }
                }
            }
        }*/



        /*
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            string[] text = File.ReadAllLines(FilePath);
            if (text == null) return null;

            var report = new Purple_5.Report();
            int kol_rese = int.Parse(text[0]);
            var researches = new Purple_5.Research[kol_rese];
            int z = 1;
            for (int i = 0; i < researches.Length; i++)
            {
                string name_rese = text[z++];
                int kol_resp = int.Parse(text[z++]);
                //researches[i] = new Purple_5.Research(name_rese);
                Purple_5.Research research = new Purple_5.Research(name_rese);
                string[] resp = new string[kol_resp];
                for (int j=0; j<kol_resp; j++)
                {
                    resp[j] = text[z++];
                    if (string.IsNullOrEmpty(resp[j]))
                    {
                        resp[j] = null;
                    }
                }


                research.Add(resp);
                researches[i] = research;

            }
            report.AddResearch(researches);
            return report;
        }*/
    }

        