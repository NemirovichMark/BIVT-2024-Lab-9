using Lab_7;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";

        private void ParticipantSEL(Purple_1.Participant p, string fileName)
        {
            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(p.Name);
                writer.WriteLine(p.Surname);

                var Coefs = string.Join(" ", p.Coefs.Select(x => x.ToString(CultureInfo.InvariantCulture)));
                var Marks = string.Join(" ", p.Marks.OfType<int>().ToList());
                writer.WriteLine(Coefs);
                writer.WriteLine(Marks);
            }
        }

        private void JudgeSel(Purple_1.Judge j, string fileName)
        {
            SelectFile(fileName);
            var Marks = string.Join(" ", j.Marks);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(j.Name);
                writer.WriteLine(Marks);
            }
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            if (obj is Purple_1.Participant participant)
            {
                SelectFile(fileName);
                using (StreamWriter writer = new StreamWriter(FilePath))
                {
                    writer.WriteLine(participant.Name);
                    writer.WriteLine(participant.Surname);

                    var Coefs = string.Join(" ", participant.Coefs.Select(x => x.ToString(CultureInfo.InvariantCulture)));
                    var Marks = string.Join(" ", participant.Marks.OfType<int>().ToList());
                    writer.WriteLine(Coefs);
                    writer.WriteLine(Marks);

                }
            }
            else if (obj is Purple_1.Judge judge)
            {
                SelectFile(fileName);
                var Marks = string.Join(" ", judge.Marks);
                using (StreamWriter writer = new StreamWriter(FilePath))
                {
                    writer.WriteLine(judge.Name);
                    writer.WriteLine(Marks);
                }

            }
            else if (obj is Purple_1.Competition competition)
            {
                SelectFile(fileName);

                var INFO_COMPETITION = Path.Combine(FolderPath, $"INFO_COMPETITION_{fileName}");

                if (!Directory.Exists(INFO_COMPETITION))
                {
                    Directory.CreateDirectory(INFO_COMPETITION);
                }
                var p1 = competition.Participants.Length; var j1 = competition.Judges.Length;
                using (StreamWriter writer = new StreamWriter(FilePath))
                {
                    writer.WriteLine("Participants");
                    writer.WriteLine(p1);
                    writer.WriteLine("Judges");
                    writer.WriteLine(j1);

                    for (int i = 0; i < p1; i++)
                    {
                        var path = Path.Combine(INFO_COMPETITION, $"participant {i}");
                        ParticipantSEL(competition.Participants[i], path);
                    }

                    for (int i = 0; i < j1; i++)
                    {
                        var path = Path.Combine(INFO_COMPETITION, $"judge {i}");
                        JudgeSel(competition.Judges[i], path);
                    }

                }
            }
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                var INFO_SKI_JUMPING = Path.Combine(FolderPath, $"INFO_SKI_JUMPING_{fileName}");
                if (!Directory.Exists(INFO_SKI_JUMPING))
                {
                    Directory.CreateDirectory(INFO_SKI_JUMPING);
                }

                writer.WriteLine(jumping.Name);
                writer.WriteLine(jumping.Standard);
                writer.WriteLine(jumping.Participants.Length);

                int p1 = jumping.Participants.Length;
                for (int i = 0; i < p1; i++)
                {
                    var signle_participant = Path.Combine(INFO_SKI_JUMPING, $"participant {i}.{Extension}");

                    using (StreamWriter writer1 = new StreamWriter(signle_participant)) // записываем каждого спортсмена в наш поток 
                    {

                        var Marks = string.Join(" ", jumping.Participants[i].Marks);

                        writer1.WriteLine(jumping.Participants[i].Name);
                        writer1.WriteLine(jumping.Participants[i].Surname);
                        writer1.WriteLine(jumping.Participants[i].Distance);
                        writer1.WriteLine(jumping.Participants[i].Result);
                        writer1.WriteLine(Marks);
                    }
                }

            }
        }
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(skating.GetType().Name); // FigureSkating or IceSkating
                var Moods = string.Join(" ", skating.Moods);
                writer.WriteLine(Moods);
                writer.WriteLine(skating.Participants.Length);


                var p1 = skating.Participants.Length;
                for (int i = 0; i < p1; i++)
                {
                    var human = skating.Participants[i];
                    writer.WriteLine($"{human.Name} " +
                    $"{human.Surname} " +
                    $"{string.Join(" ", human.Marks)}");
                }
            }
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(group.Name);
                writer.WriteLine(group.Sportsmen.Length);

                var grouplen = group.Sportsmen.Length;
                for (int i = 0; i < grouplen; i++)
                {
                    var sportsman = group.Sportsmen[i];
                    writer.WriteLine($"{sportsman.Name} " +
                        $"{sportsman.Surname} " +
                        $"{sportsman.Time}");
                }

            }
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(report.Researches.Length);
                for (int i = 0; i < report.Researches.Length; i++)
                {
                    var res = report.Researches[i];

                    writer.WriteLine(res.Name);

                    if (res.Responses == null)
                    {
                        continue;
                    }
                    writer.WriteLine(res.Responses.Length);

                    foreach (var resp in res.Responses)
                    {
                        writer.WriteLine($"{resp.Animal} " +
                            $"{resp.CharacterTrait} " +
                            $"{resp.Concept}");
                    }


                }
            }
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var SELtext_DEL = File.ReadAllLines(FilePath);
            if (typeof(T) == typeof(Purple_1.Participant))
            {
                var participant = new Purple_1.Participant(SELtext_DEL[0], SELtext_DEL[1]);

                var coefParts = SELtext_DEL[2].Split(' ');
                var coefs = new double[coefParts.Length];
                for (int i = 0; i < coefParts.Length; i++)
                {
                    coefs[i] = double.Parse(coefParts[i], CultureInfo.InvariantCulture);
                }
                participant.SetCriterias(coefs);

                var markParts = SELtext_DEL[3].Split(' ');
                var marks = new int[markParts.Length];
                for (int i = 0; i < markParts.Length; i++)
                {
                    marks[i] = int.Parse(markParts[i]);
                }

                for (int i = 0; i < 4; i++)
                {
                    int[] MARKS = new int[7];
                    var position_of_jump = i * 7;
                    Array.Copy(marks, position_of_jump, MARKS, 0, 7);
                    participant.Jump(MARKS);
                }

                return participant as T;
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                string[] marks = SELtext_DEL[1].Split(' ');
                int[] marks_massive = new int[marks.Length];
                for (int i = 0; i < marks.Length; i++)
                {
                    marks_massive[i] = int.Parse(marks[i]);
                }

                return new Purple_1.Judge(SELtext_DEL[0], marks_massive) as T;
            }
            else
            {

                var folder = Path.Combine(FolderPath, $"INFO_COMPETITION_{fileName}");

                var judges = new Purple_1.Judge[int.Parse(SELtext_DEL[3])];

                for (int i = 0; i < int.Parse(SELtext_DEL[3]); i++)
                {
                    string judge = Path.Combine(folder, $"judge {i}");
                    judges[i] = JUDGE_SEL_TO_DEL(judge);
                }

                var result = new Purple_1.Competition(judges);

                for (int i = 0; i < int.Parse(SELtext_DEL[1]); i++)
                {
                    string person = Path.Combine(folder, $"participant {i}");
                    result.Add(Participant_SEL_TO_DEL(person));
                }

                return result as T;
            }
        }
        private Purple_1.Participant Participant_SEL_TO_DEL(string fileName)
        {
            SelectFile(fileName);
            var SELtext_DEL = File.ReadAllLines(FilePath);
            var participant = new Purple_1.Participant(SELtext_DEL[0], SELtext_DEL[1]);

            var coefParts = SELtext_DEL[2].Split(' ');
            var coefs = new double[coefParts.Length];
            for (int i = 0; i < coefParts.Length; i++)
            {
                coefs[i] = double.Parse(coefParts[i], CultureInfo.InvariantCulture);
            }

            participant.SetCriterias(coefs);


            var markParts = SELtext_DEL[3].Split(' ');
            var marks = new int[markParts.Length];
            for (int i = 0; i < markParts.Length; i++)
            {
                marks[i] = int.Parse(markParts[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                int[] MARKS = new int[7];
                var position_of_jump = i * 7;
                Array.Copy(marks, position_of_jump, MARKS, 0, 7);
                participant.Jump(MARKS);
            }

            return participant;
        }

        private Purple_1.Judge JUDGE_SEL_TO_DEL(string fileName)
        {
            SelectFile(fileName);
            var SELtext_DEL = File.ReadAllLines(FilePath);

            string[] marks = SELtext_DEL[1].Split(' ');
            int[] marks_massive = new int[marks.Length];
            for (int i = 0; i < marks.Length; i++)
            {
                marks_massive[i] = int.Parse(marks[i]);
            }

            return new Purple_1.Judge(SELtext_DEL[0], marks_massive);
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);

            var SELtext_DEL = File.ReadAllLines(FilePath);

            Purple_2.SkiJumping jump;

            if (SELtext_DEL[0] == "100m")
            {
                jump = new Purple_2.JuniorSkiJumping();
            }
            else
            {
                jump = new Purple_2.ProSkiJumping();
            }

            var INFO_SKI_JUMPING = Path.Combine(FolderPath, $"INFO_SKI_JUMPING_{fileName}");

            var Participants = new Purple_2.Participant[int.Parse(SELtext_DEL[2])];

            int stand_of_jump = int.Parse(SELtext_DEL[1]);

            for (int i = 0; i < int.Parse(SELtext_DEL[2]); i++)
            {
                var participants = File.ReadAllLines(Path.Combine(INFO_SKI_JUMPING, $"participant {i}.{Extension}"));
                var participant = new Purple_2.Participant(participants[0], participants[1]);

                int distance = int.Parse(participants[2]);
                int result = int.Parse(participants[3]);
                int[] marks = participants[4].Split(' ').Select(int.Parse).ToArray();
                participant.Jump(distance, marks, stand_of_jump);
                Participants[i] = participant;
            }
            jump.Add(Participants);

            return (T)(Object)jump;

        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);

            var SELtext_DEL = File.ReadAllLines(FilePath);

            double[] Moods = SELtext_DEL[1].Split(' ').Select(double.Parse).ToArray();

            Purple_3.Skating skating;

            if (SELtext_DEL[0] == "FigureSkating")
            {
                skating = new Purple_3.FigureSkating(Moods, false);
            }
            else
            {
                skating = new Purple_3.IceSkating(Moods, false);
            }

            for (int i = 0; i < int.Parse(SELtext_DEL[2]); i++)
            {
                var info = SELtext_DEL[i + 3].Split(' ');
                var participant = new Purple_3.Participant(info[0], info[1]);
                string[] mark_mass = info[2].Split(' ');
                double[] Marks = new double[mark_mass.Length];

                for (int l = 0; l < mark_mass.Length; l++)
                {
                    Marks[l] = double.Parse(mark_mass[l]);
                }

                foreach (var m in Marks)
                {
                    participant.Evaluate(m);
                }

                skating.Add(participant);
            }

            return (T)(Object)skating;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);

            var SELtext_DEL = File.ReadAllLines(FilePath);

            var group = new Purple_4.Group(SELtext_DEL[0]);

            for (int i = 0; i < int.Parse(SELtext_DEL[1]); i++)
            {
                var sportsman_line_info = SELtext_DEL[i + 2].Split(' ');
                var sportsman = new Purple_4.Sportsman(sportsman_line_info[0], sportsman_line_info[1]);
                sportsman.Run(double.Parse(sportsman_line_info[2]));

                group.Add(sportsman);
            }

            return group;
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {

            SelectFile(fileName);

            var rep = new Purple_5.Report();

            using (StreamReader reader = new StreamReader(FilePath))
            {
                string? line = reader.ReadLine();
                int res_kolvo = int.Parse(line != null ? line : "0");

                for (int i = 0; i < res_kolvo; i++)
                {
                    var info_in_line = reader.ReadLine().Split(' ');

                    var checking = info_in_line.Length > 1 ? info_in_line[1] : "0";
                    var res_amount = int.Parse(checking);

                    var research = new Purple_5.Research(info_in_line[0]);

                    for (int j = 0; j < res_amount; j++)
                    {
                        var response = reader.ReadLine().Split(' ');

                        string? animal;
                        if (string.IsNullOrEmpty(response[0])) {
                            animal = null;
                        }
                        else
                        {
                            animal = response[0];
                        }
                        string? characterTrait;
                        if (string.IsNullOrEmpty(response[1]))
                        {
                            characterTrait = null;
                        }
                        else
                        {
                            characterTrait = response[1];
                        }
                        string? concept;
                        if (string.IsNullOrEmpty(response[2]))
                        {
                            concept = null;
                        }
                        else
                        {
                            concept = response[2];
                        }
                        if (animal != null || characterTrait != null || concept != null)
                        {
                            research.Add(new string[] { animal, characterTrait, concept });
                        }

                        rep.AddResearch(research);
                    }
                }

                return rep;
            }
        }
    }
}

