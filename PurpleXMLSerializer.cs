using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(_folderPath) || obj == null) return;

            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            File.WriteAllText(fullPath, string.Empty);

            if (obj is Purple_1.Participant)
            {
                ParticipantWrapper participantWrapper = new ParticipantWrapper(obj as Purple_1.Participant);

                //передаем в конструктор тип класса ParticipantWrapper
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ParticipantWrapper));


                //получаем поток, куда будем записывать сериализованный объект
                using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
                {
                    xmlSerializer.Serialize(fs, participantWrapper);
                }
            }
            if (obj is Purple_1.Judge)
            {
                JudgeWrapper judgeWrapper = new JudgeWrapper(obj as Purple_1.Judge);

                //передаем в конструктор тип класса ParticipantWrapper
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(JudgeWrapper));

                //получаем поток, куда будем записывать сериализованный объект
                using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
                {
                    xmlSerializer.Serialize(fs, judgeWrapper);
                }
            }
            if (obj is Purple_1.Competition)
            { 
                //System.Console.WriteLine("comp");
                CompetitionWrapper competitionWrapper = new CompetitionWrapper(obj as Purple_1.Competition);

                //передаем в конструктор тип класса 
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(CompetitionWrapper));

                //получаем поток, куда будем записывать сериализованный объект
                using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
                {
                    xmlSerializer.Serialize(fs, competitionWrapper);
                }
            }

        }

        public override T DeserializePurple1<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(_folderPath)) return default(T);

            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);

            if (!File.Exists(fullPath)) return default(T);

            string type = "";

            //читаем xml для определения класса
            using (XmlReader reader = XmlReader.Create(fullPath))
            {
                while (reader.Read()) //перемещает курсор 
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "ParticipantWrapper")
                    {
                        type = "ParticipantWrapper";
                        break;
                    }
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "JudgeWrapper")
                    {
                        type = "JudgeWrapper";
                        break;
                    }
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "CompetitionWrapper")
                    {
                        type = "CompetitionWrapper";
                        break;
                    }
                }
            }
            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                
                if (type == "ParticipantWrapper")
                {
                   
                    //передаем в конструктор тип класса ParticipantWrapper
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ParticipantWrapper));
                    ParticipantWrapper participantWrapper = xmlSerializer.Deserialize(fs) as ParticipantWrapper;

                    Purple_1.Participant participant = new Purple_1.Participant(participantWrapper.Name, participantWrapper.Surname);
                    participant.SetCriterias(participantWrapper.Coefs);

                    for (int i = 0; i < 4; i++)
                    {
                        int[] marksJump = new int[7];
                        for (int j = 0; j < 7; j++)
                        {
                            marksJump[j] = participantWrapper.Marks[i * 7 + j];
                        }
                        participant.Jump(marksJump);
                    }
                    return participant as T;
                }
            
                if (type == "JudgeWrapper")
                {
                    //передаем в конструктор тип класса ParticipantWrapper
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(JudgeWrapper));
                    JudgeWrapper judgeWrapper = xmlSerializer.Deserialize(fs) as JudgeWrapper;

                    Purple_1.Judge judge = new Purple_1.Judge(judgeWrapper.Name, judgeWrapper.Marks);
                    return judge as T;
                  
                }
            
                if (type == "CompetitionWrapper")
                {
                    //передаем в конструктор тип класса ParticipantWrapper
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(CompetitionWrapper));
                    CompetitionWrapper competitonWrapper = xmlSerializer.Deserialize(fs) as CompetitionWrapper;

                    Purple_1.Judge[] judges = new Purple_1.Judge[0];
                    foreach (JudgeWrapper judgeWrapper in competitonWrapper.Judges)
                    {
                        Purple_1.Judge judge = new Purple_1.Judge(judgeWrapper.Name, judgeWrapper.Marks);
                        Array.Resize(ref judges, judges.Length + 1);
                        judges[judges.Length - 1] = judge;
                    }

                    Purple_1.Competition competiton = new Purple_1.Competition(judges);

                    Purple_1.Participant[] participants = new Purple_1.Participant[0];
                    foreach (ParticipantWrapper participantWrapper in competitonWrapper.Participants)
                    {
                        Purple_1.Participant participant = new Purple_1.Participant(participantWrapper.Name, participantWrapper.Surname);
                        participant.SetCriterias(participantWrapper.Coefs);

                        for (int i = 0; i < 4; i++)
                        {
                            int[] marksJump = new int[7];
                            for (int j = 0; j < 7; j++)
                            {
                                marksJump[j] = participantWrapper.Marks[i * 7 + j];
                            }
                            participant.Jump(marksJump);
                        }
                        Array.Resize(ref participants, participants.Length + 1);
                        participants[participants.Length - 1] = participant;
                    }
                    competiton.Add(participants);
                    return competiton as T;
                }
            }
            return default(T);
        }


        public class CompetitionWrapper
        {
            //автосвойтсва
            public ParticipantWrapper[] Participants { get; set; }
            public JudgeWrapper[] Judges { get; set; }


            //конструктор без параметров 
            public CompetitionWrapper() { }

            //констуктор 
            public CompetitionWrapper(Purple_1.Competition competiton)
            {
                ParticipantWrapper[] participantWrappers = new ParticipantWrapper[competiton.Participants.Length];
                for (int i = 0; i < participantWrappers.Length; i++)
                {
                    ParticipantWrapper participantWrapper = new ParticipantWrapper(competiton.Participants[i]);
                    participantWrappers[i] = participantWrapper;
                }
                Participants = participantWrappers;

                JudgeWrapper[] judgeWrappers = new JudgeWrapper[competiton.Judges.Length];
                for (int i = 0; i < judgeWrappers.Length; i++)
                {
                    JudgeWrapper judgeWrapper = new JudgeWrapper(competiton.Judges[i]);
                    judgeWrappers[i] = judgeWrapper;
                }
                Judges = judgeWrappers;

            }

        }

        public class JudgeWrapper
        {
            //автосвойтсва
            public string Name { get; set; }
            public int[] Marks { get; set; }


            //конструктор без параметров 
            public JudgeWrapper() { }

            //констуктор 
            public JudgeWrapper(Purple_1.Judge judge)
            {
                Name = judge.Name;

                Marks = judge.Marks;
            }

        }

         public class ParticipantWrapper
        {
            //автосвойтсва
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int[] Marks { get;  set; }
            public double TotalScore { get;  set; }

            //public int Jump { private get; private set; }
            
           
            //конструктор без параметров 
            public ParticipantWrapper() { }

            //констуктор из партисипант в партиспантреппер
            public ParticipantWrapper(Purple_1.Participant participant)
            { 
                Name = participant.Name;
                Surname = participant.Surname;
                Coefs = participant.Coefs;
                //в xml сериализации не поддерживаются многомерные массивы...

                Marks = new int[participant.Marks.GetLength(0) * participant.Marks.GetLength(1)];

                for (int i = 0; i < participant.Marks.GetLength(0); i++)
                {
                    for (int j = 0; j < participant.Marks.GetLength(1); j++)
                    { 
                        Marks[i*7+j] = participant.Marks[i,j];
                    } 
                }
                    

                TotalScore = participant.TotalScore;
            }


        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(_folderPath) || jumping == null) return;
            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            File.WriteAllText(fullPath, string.Empty);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SkiJumpingWrapper));
            SkiJumpingWrapper skiJumpingWrapper = new SkiJumpingWrapper(jumping);
            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, skiJumpingWrapper);
            }
            

        }

        public class SkiJumpingWrapper
        {
            //авто-свойтсва с публичными сеттерами
            public string Name { get; set; }
            public int Standard { get; set; }
            public SkiParticipantWrapper[] Participants{ get; set; }

            //конструкторы
            public SkiJumpingWrapper() { }

            public SkiJumpingWrapper(Purple_2.SkiJumping skiJumping)
            {
                Name = skiJumping.Name;
                Standard = skiJumping.Standard;
                Participants = new SkiParticipantWrapper[skiJumping.Participants.Length];
                for (int i = 0; i < Participants.Length; i++)
                {
                    SkiParticipantWrapper skiParticipantWrapper = new SkiParticipantWrapper(skiJumping.Participants[i]);
                    Participants[i] = skiParticipantWrapper;
                }
            }
        }

        public class SkiParticipantWrapper
        { 
            //авто-свойтсва с публичными сеттерами
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Marks { get; set; }
            public int Distance { get; set; }
            public int Result { get; set; }

            //конструкторы
            public SkiParticipantWrapper() { }

            public SkiParticipantWrapper(Purple_2.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = participant.Marks;
                Distance = participant.Distance;
                Result = participant.Result;
            }
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(_folderPath)) return default(T);
            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            if (!File.Exists(fullPath)) return default(T);

            string type = "";
            //понять кто из наследников, для этого последовательно читать 
            using (XmlReader reader = XmlReader.Create(fullPath))
            {
                while (reader.Read())
                {
                    //тип тега и имя тега
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Name")
                    {
                        //значение тега
                        type = reader.ReadElementContentAsString();
                        break;
                    }
                }
            }
            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SkiJumpingWrapper));
                SkiJumpingWrapper skiJumpingWrapper = xmlSerializer.Deserialize(fs) as SkiJumpingWrapper;

                Purple_2.SkiJumping skiJumping;
                if (type == "100m") skiJumping = new Purple_2.JuniorSkiJumping () ;
                else skiJumping = new Purple_2.ProSkiJumping ();

                for (int i = 0; i < skiJumpingWrapper.Participants.Length; i++)
                {
                    Purple_2.Participant participant = new Purple_2.Participant(skiJumpingWrapper.Participants[i].Name, skiJumpingWrapper.Participants[i].Surname);
                    skiJumping.Add(participant);
                    skiJumping.Jump(skiJumpingWrapper.Participants[i].Distance, skiJumpingWrapper.Participants[i].Marks);
                }
                return skiJumping as T;

            }

        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(_folderPath) || skating == null) return;
            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            File.WriteAllText(fullPath, string.Empty);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SkatingWrapper));
            SkatingWrapper skatingWrapper = new SkatingWrapper(skating);
            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, skatingWrapper);
            }
        }

        public class SkatingWrapper
        {
            //авто-свойтсва с публичными сеттерами
            public string Type { get; set; }
            public double[] Moods { get; set; }
            public ParticipantSkatingWrapper[] Participants{ get; set; }
            

            //конструкторы
            public SkatingWrapper() { }

            public SkatingWrapper(Purple_3.Skating skating)
            {
                Moods = skating.Moods;
                Participants = new ParticipantSkatingWrapper[skating.Participants.Length];
                for (int i = 0; i < Participants.Length; i++)
                {
                    ParticipantSkatingWrapper participantSkatingWrapper = new ParticipantSkatingWrapper(skating.Participants[i]);
                    Participants[i] = participantSkatingWrapper;
                }

                if (skating is Purple_3.IceSkating) Type = "IceSkating";
                else Type = "FigureSkating";
            }
        }

        public class ParticipantSkatingWrapper
        { 
            //авто-свойтсва с публичными сеттерами
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Marks { get; set; }
            public int[] Places { get; set; }
            public int Score { get; set; }

            //конструкторы
            public ParticipantSkatingWrapper() { }

            public ParticipantSkatingWrapper(Purple_3.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = participant.Marks;
                Places = participant.Places;
                Score = participant.Score;
            }
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(_folderPath)) return default(T);
            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            if (!File.Exists(fullPath)) return default(T);

            string type = "";
            //понять кто из наследников, для этого последовательно читать 
            using (XmlReader reader = XmlReader.Create(fullPath))
            {
                while (reader.Read())
                {
                    //тип тега и имя тега
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Type")
                    {
                        //значение тега
                        type = reader.ReadElementContentAsString();
                        break;
                    }
                }
            }
            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SkatingWrapper));
                SkatingWrapper skatingWrapper = xmlSerializer.Deserialize(fs) as SkatingWrapper;

                Purple_3.Skating skating;
                if (type == "FigureSkating") skating = new Purple_3.FigureSkating (skatingWrapper.Moods, false) ;
                else skating = new Purple_3.IceSkating (skatingWrapper.Moods, false);

                for (int i = 0; i < skatingWrapper.Participants.Length; i++)
                {
                    Purple_3.Participant participant = new Purple_3.Participant(skatingWrapper.Participants[i].Name, skatingWrapper.Participants[i].Surname);
                
                    double[] marks = skatingWrapper.Participants[i].Marks;
                    for (int j = 0; j < marks.Length; j++)
                    {
                        participant.Evaluate(marks[j]);
                    } 
                    skating.Add(participant);
                }
                Purple_3.Participant.SetPlaces(skating.Participants);

                return skating as T;

            }



        }

        public class GroupWrapper
        {
            //авто-свойтсва
            public string Name { get; set; }
            public SportsmanWrapper[] Sportsmen { get; set; }

            //конструкторы 
            public GroupWrapper() { }
            public GroupWrapper(Purple_4.Group group)
            { 
                Name = group.Name;
                Sportsmen = new SportsmanWrapper[group.Sportsmen.Length];
                for (int i = 0; i < Sportsmen.Length; i++)
                {
                    SportsmanWrapper sportsmanWrapper = new SportsmanWrapper(group.Sportsmen[i]);
                    Sportsmen[i] = sportsmanWrapper;
                }
            }

        }

        public class SportsmanWrapper
        {
            //авто-свойтсва с публичными сеттерами
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }
            

            //конструкторы
            public SportsmanWrapper() { }

            public SportsmanWrapper(Purple_4.Sportsman sportsman)
            {
                Name = sportsman.Name;
                Surname = sportsman.Surname;
                Time = sportsman.Time;

                if (sportsman is Purple_4.SkiWoman) Type = "SkiWoman";
                else Type = "SkiMan";
            }
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(_folderPath)) return default(Purple_4.Group);
            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            if (!File.Exists(fullPath)) return default(Purple_4.Group);

            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(GroupWrapper));
                GroupWrapper groupWrapper = xmlSerializer.Deserialize(fs) as GroupWrapper;

                Purple_4.Group group = new Purple_4.Group(groupWrapper.Name);
                
                for (int i = 0; i < groupWrapper.Sportsmen.Length; i++)
                {
                    Purple_4.Sportsman sportsman;
                    if (groupWrapper.Sportsmen[i].Type == "SkiWoman") sportsman = new Purple_4.Sportsman(groupWrapper.Sportsmen[i].Name, groupWrapper.Sportsmen[i].Surname);
                    else sportsman = new Purple_4.Sportsman(groupWrapper.Sportsmen[i].Name, groupWrapper.Sportsmen[i].Surname);
                    sportsman.Run(groupWrapper.Sportsmen[i].Time);
                    group.Add(sportsman);
                }
                
                return group ;

            }



        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(_folderPath) || group == null) return;
            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            File.WriteAllText(fullPath, string.Empty);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GroupWrapper));
            GroupWrapper groupWrapper = new GroupWrapper(group);
            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, groupWrapper);
            }
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(_folderPath) || report == null) return;
            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            File.WriteAllText(fullPath, string.Empty);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ReportWrapper));
            ReportWrapper reportWrapper = new ReportWrapper(report);
            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, reportWrapper);
            }
        }


        public class ReportWrapper
        { 
             //авто-свойтсва
            public ResearchWrapper[] Researches { get; set; }

            //конструкторы 
            public ReportWrapper() { }
            public ReportWrapper(Purple_5.Report report)
            { 
                Researches = new ResearchWrapper[report.Researches.Length];
                for (int i = 0; i < Researches.Length; i++)
                {
                    ResearchWrapper researchWrapper = new ResearchWrapper(report.Researches[i]);
                    Researches[i] = researchWrapper;
                }
            }
        }

        public class ResearchWrapper
        {
            //авто-свойтсва
            public string Name {get;set;}
            public ResponseWrapper[] Responses {get;set;}

            //констукторы
            public ResearchWrapper(){}
            public ResearchWrapper(Purple_5.Research research)
            {
                Name = research.Name;
                Responses = new ResponseWrapper[research.Responses.Length];
                for (int i = 0; i < research.Responses.Length; i++)
                {
                    Responses[i] = new ResponseWrapper(research.Responses[i]); 
                }
            } 
        }

        public class ResponseWrapper
        {
            //авто - свойтсва
            public string Animal {get; set;}
            public string CharacterTrait {get; set;}
            public string Concept {get; set;}

            //конструкторы 
            public ResponseWrapper(){}
            public ResponseWrapper(Purple_5.Response response)
            {
                Animal = response.Animal;
                CharacterTrait = response.CharacterTrait;
                Concept = response.Concept;
            }
        }


        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(_folderPath)) return default(Purple_5.Report);
            string fullPath = Path.Combine(_folderPath, fileName + "." + Extension);
            if (!File.Exists(fullPath)) return default(Purple_5.Report);

            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ReportWrapper));
                ReportWrapper reportWrapper = xmlSerializer.Deserialize(fs) as ReportWrapper;

                Purple_5.Report report = new Purple_5.Report();
                
                for (int i = 0; i < reportWrapper.Researches.Length; i++)
                {
                    Purple_5.Research research = new Purple_5.Research(reportWrapper.Researches[i].Name);
                    for (int j = 0; j < reportWrapper.Researches[i].Responses.Length;j++)
                    {
                        research.Add(new string[] {reportWrapper.Researches[i].Responses[j].Animal,
                        reportWrapper.Researches[i].Responses[j].CharacterTrait,
                        reportWrapper.Researches[i].Responses[j].Concept});
                    }
                    report.AddResearch(research);
                }
                
                return report ;

            }
        }

        
    }
}
