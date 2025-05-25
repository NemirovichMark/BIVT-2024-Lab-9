using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using static Lab_7.Purple_1;
using static Lab_7.Purple_2;
using static Lab_7.Purple_3;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";
        private void Serialize<T>(T obj)
        {
            var serializ = new XmlSerializer(typeof(T));
            using (var writer = XmlWriter.Create(FilePath))
            {
                serializ.Serialize(writer, obj);
            }
        }
        private T Deserialize<T>()
        {
            var derializ = new XmlSerializer(typeof(T));
            T obj;
            using (var reader = File.OpenRead(FilePath))
            {
                obj = (T)derializ.Deserialize(reader);
            }

            return obj;
        }
        public override void SerializePurple1<T>(T obj, string fileName) 
        {
            SelectFile(fileName);
            if (obj is Purple_1.Participant participant)
            {
                var participant1 = new Purple_1_Participant
                {
                    Name = participant.Name,
                    Surname = participant.Surname,
                    Coefs = participant.Coefs,
                    Marks = participant.Marks.Cast<int>().ToArray()
                };
                Serialize(participant1);
            }
            else if (obj is Purple_1.Judge judge)
            {
                var judge1 = new Purple_1_Judge
                {
                    Name = judge.Name,
                    Marks = judge.Marks
                };
                Serialize(judge1);
            }
            else if (obj is Purple_1.Competition competition)
            {

                var answer = new Purple_1_Competition
                {
                    JudgeSum = competition.Judges.Length,
                    Participants = competition.Participants.Select(participant =>
                    new Purple_1_Participant
                    {
                        Name = participant.Name,
                        Surname = participant.Surname,
                        Coefs = participant.Coefs,
                        Marks = participant.Marks.Cast<int>().ToArray()
                    }).ToArray(),
                    Judges = competition.Judges.Select(judge =>
                    new Purple_1_Judge
                    {
                        Name = judge.Name,
                        Marks = judge.Marks
                    }
                    ).ToArray(),                    
                };
                Serialize(answer);
            }
        }
        public override T DeserializePurple1<T>(string fileName) 
        {
            SelectFile(fileName);
            if (typeof(T) == typeof(Purple_1.Participant))
            {
                var answer = Deserialize<Purple_1_Participant>();
                return DeserializeParticipantPurple1(answer) as T;
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                var answer = Deserialize<Purple_1_Judge>();
                return DeserializeJudgePurple1(answer) as T;
            }
            else if (typeof(T) == typeof(Purple_1.Competition))
            {
                var competition = Deserialize<Purple_1_Competition>();
                var judges = competition.Judges;
                Purple_1.Judge[] judges1 = new Judge[competition.JudgeSum];
                int i = 0;
                foreach (var judge in judges)
                {
                    judges1[i] = DeserializeJudgePurple1(judge);
                    i++;
                }
                Purple_1.Competition answer = new Purple_1.Competition(judges1);
                var participants = competition.Participants; 
                foreach (var participant in participants)
                {
                    answer.Add(DeserializeParticipantPurple1(participant));
                }
                return answer as T;
            }
            else
            {
                return null;
            }
        }
        public class Purple_1_Participant
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int[] Marks { get; set; }
        }
        public class Purple_1_Judge
        {
            public string Name { get; set; }
            public int[] Marks { get; set; }
        }
        public class Purple_1_Competition
        {
            public Purple_1_Judge[] Judges { get; set; }
            public int JudgeSum { get; set; }
            public Purple_1_Participant[] Participants { get; set; }
        }
        private Purple_1.Participant DeserializeParticipantPurple1(Purple_1_Participant obj)
        {
            var participant = new Purple_1.Participant(obj.Name, obj.Surname);
            double[] ParticipantCoefs = obj.Coefs;
            int[] ParticipantMarks = obj.Marks;
            int[,] marks = new int[4, 7];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 7; j++)
                    marks[i, j] = ParticipantMarks[i * 7 + j];
            participant.SetCriterias(ParticipantCoefs);
            for (int i = 0; i < 4; i++)
            {
                int[] marks1 = new int[7];
                for (int j = 0; j < 7; j++)
                {
                    marks1[j] = marks[i, j];
                }
                participant.Jump(marks1);
            }
            return participant;
        }
        private Purple_1.Judge DeserializeJudgePurple1(Purple_1_Judge obj)
        {
            var judge = new Purple_1.Judge(obj.Name, obj.Marks);
            return judge;
        }
          




        // Purple_2
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            if (jumping is Purple_2.SkiJumping jumping1)
            {
                Purple_2_SkiJumping answer = new Purple_2_SkiJumping
                {
                    Name = jumping1.Name,
                    Standart = jumping1.Standard,
                    Participants = jumping1.Participants.Select(participant =>
                    new Participant_Purple_2
                    {
                        Name = participant.Name,
                        Surname = participant.Surname,
                        Distance = participant.Distance,
                        Marks = participant.Marks,
                    }).ToArray()
                };
                Serialize(answer);
            }
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var SkiJumping = Deserialize<Purple_2_SkiJumping>();
            Purple_2.SkiJumping answer;
            if (SkiJumping.Standart == 100)
            {
                answer = new Purple_2.JuniorSkiJumping();
            }
            else if (SkiJumping.Standart == 150)
            {
                answer = new Purple_2.ProSkiJumping();
            }
            else
            {
                return null;
            }
            var participants = SkiJumping.Participants;
            foreach (var participant in participants)
            {
                answer.Add(DeserializeParticipantPurple2(participant, SkiJumping.Standart));
            }
            return answer as T;
        }
        public class Participant_Purple_2
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }
        }
        public class Purple_2_SkiJumping
        {
            public string Name { get; set; }
            public int Standart { get; set; }
            public Participant_Purple_2[] Participants { get; set; }
        }
        private Purple_2.Participant DeserializeParticipantPurple2(Participant_Purple_2 obj, int standart)
        {
            Purple_2.Participant participant = new Purple_2.Participant(obj.Name, obj.Surname);
            participant.Jump(obj.Distance, obj.Marks, standart);
            return participant;
        }




        // Purple_3
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            if (skating is Purple_3.Skating skiting1)
            {
                Purple_3_Skating answer = new Purple_3_Skating
                {
                    Type = skiting1.GetType().Name,
                    Moods = skiting1.Moods,
                    Participants = skiting1.Participants.Select(participant =>
                    new Purple_3_Participant
                    {
                        Name = participant.Name,
                        Surname = participant.Surname,
                        Marks = participant.Marks,
                        Places = participant.Places,
                    }).ToArray(),
                };
                Serialize(answer);
            }
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var Skating = Deserialize<Purple_3_Skating>();
            Purple_3.Skating answer;
            if (Skating.Type == nameof(Purple_3.IceSkating))
            {
                answer = new Purple_3.IceSkating(Skating.Moods, false);
            }
            else if (Skating.Type == nameof(Purple_3.FigureSkating))
            {
                answer = new Purple_3.FigureSkating(Skating.Moods, false);
            }
            else
            {
                return default;
            }
            var participants = Skating.Participants;
            foreach (var participant in participants)
            {
                answer.Add(DeserializeParticipantPurple3(participant));
            }
            return answer as T;
        }
        public class Purple_3_Participant
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Marks { get; set; }
            public int[] Places { get; set; }
        }
        public class Purple_3_Skating
        {
            public double[] Moods { get; set; }
            public string Type { get; set; }
            public Purple_3_Participant[] Participants { get; set; }
        }
        private Purple_3.Participant DeserializeParticipantPurple3(Purple_3_Participant obj)
        {
            Purple_3.Participant participant = new Purple_3.Participant(obj.Name, obj.Surname);
            var marks = obj.Marks;
            foreach (var mark in marks)
            {
                participant.Evaluate(mark);
            }
            return participant;
        }







        // Purple_4
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            if (group is Purple_4.Group group1)
            {
                var answer = new Purple_4_Group
                {
                    Name = group1.Name,
                    Sportsmen = group1.Sportsmen.Select(sportsmen =>
                    new Purple_4_Sportsman
                    {
                        Name = sportsmen.Name,
                        Surname = sportsmen.Surname,
                        Time = sportsmen.Time,
                    }).ToArray()
                };
                Serialize(answer);
            }
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var Group = Deserialize<Purple_4_Group>();
            Purple_4.Group answer = new Purple_4.Group(Group.Name);
            var sportsmens = Group.Sportsmen;
            foreach (var sportsmen in sportsmens)
            {
                Purple_4.Sportsman sportsmens1 = new Purple_4.Sportsman(sportsmen.Name, sportsmen.Surname);
                sportsmens1.Run(sportsmen.Time);
                answer.Add(sportsmens1);
            }
            return answer;
        }
        public class Purple_4_Sportsman
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }
        }
        public class Purple_4_Group
        {
            public string Name { get; set; }
            public Purple_4_Sportsman[] Sportsmen { get; set; }
        }





        // Purple_5
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            Purple_5_Research[] researches = new Purple_5_Research[report.Researches.Length];
            int i = 0;
            foreach (var research in report.Researches)
            {
                int j = 0;
                Purple_5_Response[] responses = new Purple_5_Response[research.Responses.Length];
                foreach (var response in research.Responses)
                {
                    Purple_5_Response response1 = new Purple_5_Response
                    {
                        Animal = response.Animal,
                        CharacterTrait = response.CharacterTrait,
                        Concept = response.Concept,
                    };
                    responses[j] = response1;
                    j++;
                }

                Purple_5_Research research1 = new Purple_5_Research
                {
                    Name = research.Name,
                    Responses = responses,
                };
                researches[i] = research1;
                i++;
            }
            Purple_5_Report answer = new Purple_5_Report
            {
                Researches = researches,
            };
            Serialize(answer);
        }
    



    public override Purple_5.Report DeserializePurple5Report(string fileName)
    {
            SelectFile(fileName);
            var Report = Deserialize<Purple_5_Report>();
            Purple_5.Report answer= new Purple_5.Report();
            foreach (var research in Report.Researches)
            {
                Purple_5.Research research1 = new Purple_5.Research(research.Name);
                foreach (Purple_5_Response response in research.Responses)
                {
                    string ResponseAnimal = response.Animal == "" ? null : response.Animal;
                    string ResponseCharacterTrait = response.CharacterTrait == "" ? null : response.CharacterTrait;
                    string ResponseConcept = response.Concept == "" ? null : response.Concept;
                    research1.Add(new string[] { ResponseAnimal, ResponseCharacterTrait, ResponseConcept });
                }
                answer.AddResearch(research1);
            }
            return answer;
    }
    
  



        public class Purple_5_Response
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }
        }
        public class Purple_5_Research
        {
            public string Name { get; set; }
            public Purple_5_Response[] Responses { get; set; }
        }
        public class Purple_5_Report
        {
            public Purple_5_Research[] Researches { get; set; }
        }
    }
}

