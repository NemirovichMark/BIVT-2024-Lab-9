using Lab_7;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace Lab_9
{
    public class PurpleXMLSerializer :PurpleSerializer
    {
        public override string Extension => "xml";
        public override void SerializePurple1<T> (T obj, string nameFile)
        {
            SelectFile(nameFile);
            using var write = new StreamWriter(FilePath);
            if (obj is Purple_1.Participant partic)
            {
                var data = new Participant1XmlModel(partic);
                new XmlSerializer(typeof(Participant1XmlModel)).Serialize(write,data);
            }
            else if (obj is Purple_1.Judge judg)
            {
                var data = new Judge1XmlModel(judg);
                new XmlSerializer(typeof(Judge1XmlModel)).Serialize(write,data);
            }
            else if (obj is Purple_1.Competition comp)
            {
                var data = new Competition1XmlModel(comp);
                new XmlSerializer(typeof(Competition1XmlModel)).Serialize(write,data);
            }
            write.Close();
        }
        // Класс для хранения данных участника в XML
        public class Participant1XmlModel
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int[][] Marks { get; set; }

            // Пустой конструктор нужен для XML сериализации
            public Participant1XmlModel() {}

            // Создает модель из объекта Participant
            public Participant1XmlModel(Purple_1.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Coefs = participant.Coefs;
                
                // Преобразуем оценки в формат для XML
                Marks = ConvertMarks(participant.Marks);
            }

            // Преобразует обратно в Participant
            public Purple_1.Participant BackParticipant()
            {
                if (Name == null || Surname == null || Coefs == null || Marks == null) return default; 
                var participant = new Purple_1.Participant(Name, Surname);
                participant.SetCriterias(Coefs);
                
                // Восстанавливаем все оценки
                foreach (var mark in Marks)
                {
                    participant.Jump(mark);
                }
                
                return participant;
            }

            // Вспомогательный метод для преобразования оценок
            private int[][] ConvertMarks(int[,] marks)
            {
                var result = new int[marks.GetLength(0)][];
                
                for (int i = 0; i < marks.GetLength(0); i++)
                {
                    result[i] = new int[marks.GetLength(1)];
                    for (int j = 0; j < marks.GetLength(1); j++)
                    {
                        result[i][j] = marks[i, j];
                    }
                }
                
                return result;
            }
        }
        public class Judge1XmlModel
        {
            public string Name{get;set;}
            public int[] Marks{get;set;}
            public Judge1XmlModel(){}
            public Judge1XmlModel(Purple_1.Judge j)
            {
                Name = j.Name;
                Marks = j.Marks;
            }
            public Purple_1.Judge BackJudge()
            {
                var j = new Purple_1.Judge(Name,Marks);
                return j;
            }
        }
        public class Competition1XmlModel
        {
            public Participant1XmlModel[] Participants {get;set;}
            public Judge1XmlModel[] Judges {get;set;}
            public Competition1XmlModel(){}
            public Competition1XmlModel(Purple_1.Competition a)
            {
                Participants = a.Participants.Select(x => new Participant1XmlModel(x)).ToArray();
                Judges = a.Judges.Select(x => new Judge1XmlModel(x)).ToArray();
            }
            public Purple_1.Competition BackCompetition()
            {
                var c = new Purple_1.Competition(Judges.Select(x => x.BackJudge()).ToArray());
                c.Add(Participants.Select(x => x.BackParticipant()).ToArray());
                return c;
            }
        }
        public override void SerializePurple2SkiJumping<T>(T jump, string nameFile)
        {
            SelectFile(nameFile);
            using var write = new StreamWriter(FilePath);
            var data = new SkiJumping2XmlModel(jump);
            new XmlSerializer(typeof(SkiJumping2XmlModel)).Serialize(write,data);
        }
        public class Participant2XmlModel
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }
            public Participant2XmlModel() { }
            public Participant2XmlModel(Purple_2.Participant part)
            {
                Name = part.Name;
                Surname = part.Surname;
                Distance = part.Distance;
                Marks = part.Marks;
            }
            public Purple_2.Participant BackParticipant(int standart)
            {
                var part = new Purple_2.Participant(Name,Surname);
                part.Jump(Distance,Marks,standart);
                return part;
            }
        }
        public class SkiJumping2XmlModel
        {
            public string Name { get; set; }
            public int Standard {  get; set; }
            public Participant2XmlModel[] Participants { get; set; }
            public string Type { get; set; }
            public SkiJumping2XmlModel() { }
            public SkiJumping2XmlModel(Purple_2.SkiJumping ski)
            {
                Name = ski.Name;
                Standard = ski.Standard;
                Participants = ski.Participants.Select(x => new Participant2XmlModel(x)).ToArray();
                Type = ski.GetType().Name;
            }
            public Purple_2.SkiJumping BackSkiJumping()
            {
                if (Participants == null || Type == null) return default;
                Purple_2.SkiJumping jumping;
                if (Type == "JuniorSkiJumping")
                    jumping = new Purple_2.JuniorSkiJumping();
                else if (Type == "ProSkiJumping")
                    jumping = new Purple_2.ProSkiJumping();
                else return null;
                jumping.Add(Participants.Select(x => x.BackParticipant(Standard)).ToArray());
                return jumping;
            }
        }
        public override void SerializePurple3Skating<T>(T skat, string nameFile)
        {
            SelectFile(nameFile);
            using var write = new StreamWriter(FilePath);
            var data = new Skating3XmlModel(skat);
            new XmlSerializer(typeof(Skating3XmlModel)).Serialize(write,data);
        }
        public class Participant3XmlModel
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Marks { get; set; }
            public int[] Places { get; set; }
            public Participant3XmlModel() { }
            public Participant3XmlModel(Purple_3.Participant part)
            {
                Name = part.Name;
                Surname = part.Surname;
                Marks = part.Marks;
                Places = part.Places;
            }
            public Purple_3.Participant BackParticipant()
            {
                var part = new Purple_3.Participant(Name,Surname);
                for (int i =0;i<Marks.Length;i++)
                {
                    part.Evaluate(Marks[i]);
                }
                return part;
            }
        }
        public class Skating3XmlModel
        {
            public Participant3XmlModel[] Participants { get; set; }
            public double[] Moods { get; set; }
            public string Type { get; set; }
            public Skating3XmlModel() { }
            public Skating3XmlModel(Purple_3.Skating skat)
            {
                Participants = skat.Participants.Select(x => new Participant3XmlModel(x)).ToArray();
                Moods = skat.Moods;
                Type = skat.GetType().Name;
            }
            public Purple_3.Skating BackSkating()
            {
                if (Participants == null || Moods == null || Type == null) return default;
                Purple_3.Skating skat;
                if (Type == "FigureSkating")
                    skat = new Purple_3.FigureSkating(Moods, false);
                else if (Type == "IceSkating")
                    skat = new Purple_3.IceSkating(Moods, false);
                else
                    return null; 
                var participants = Participants.Select(p => p.BackParticipant()).ToArray();
                skat.Add(participants);

                // 3. Расставляем места
                Purple_3.Participant.SetPlaces(skat.Participants);

                return skat;
            }
        }
        public override void SerializePurple4Group(Purple_4.Group part, string nameFile)
        {
            SelectFile(nameFile);
            using var write = new StreamWriter(FilePath);
            var data = new Group4XmlModel(part);
            new XmlSerializer(typeof(Group4XmlModel)).Serialize(write,data);
        }
        public class Sportsman4XmlModel
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }

            public Sportsman4XmlModel() { }

            public Sportsman4XmlModel(Purple_4.Sportsman source)
            {
                Name = source.Name;
                Surname = source.Surname;
                Time = source.Time;
            }

            public Purple_4.Sportsman BackSportsman()
            {
                var sportsman = new Purple_4.Sportsman(Name, Surname);
                sportsman.Run(Time); // Устанавливаем время забега
                return sportsman;
            }
        }
        public class Group4XmlModel
        {
            public string Name { get; set; }
            public Sportsman4XmlModel[] Sportsmen { get; set; }

            public Group4XmlModel() { }

            public Group4XmlModel(Purple_4.Group source)
            {
                Name = source.Name;
                Sportsmen = source.Sportsmen?
                    .Select(s => new Sportsman4XmlModel(s))
                    .ToArray();
            }

            public Purple_4.Group BackGroup()
            {
                var group = new Purple_4.Group(Name);
                
                if (Sportsmen != null)
                {
                    group.Add(Sportsmen
                        .Select(s => s.BackSportsman())
                        .ToArray());
                }
                
                return group;
            }
        }
        public override void SerializePurple5Report(Purple_5.Report gr, string nameFile)
        {
            SelectFile(nameFile);
            using var write = new StreamWriter(FilePath);
            var data = new Report5XmlModel(gr);
            new XmlSerializer(typeof(Report5XmlModel)).Serialize(write,data);
        }

        public class Response5XmlModel
        {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }

            public Response5XmlModel() { }

            public Response5XmlModel(Purple_5.Response source)
            {
                Animal = source.Animal;
                CharacterTrait = source.CharacterTrait;
                Concept = source.Concept;
            }

            public Purple_5.Response BackResponse()
            {
                return new Purple_5.Response(Animal, CharacterTrait, Concept);
            }
        }
        public class Research5XmlModel
        {
            public string Name { get; set; }
            public Response5XmlModel[] Responses { get; set; }

            public Research5XmlModel() { }

            public Research5XmlModel(Purple_5.Research source)
            {
                Name = source.Name;
                Responses = source.Responses?
                    .Select(r => new Response5XmlModel(r))
                    .ToArray();
            }

            public Purple_5.Research BackResearch()
            {
                var research = new Purple_5.Research(Name);
                
                if (Responses != null)
                {
                    foreach (var response in Responses)
                    {
                        research.Add(new string[] 
                        {
                            response.Animal,
                            response.CharacterTrait,
                            response.Concept
                        });
                    }
                }
                
                return research;
            }
        }
        public class Report5XmlModel
        {
            public Research5XmlModel[] Researches { get; set; }

            public Report5XmlModel() { }

            public Report5XmlModel(Purple_5.Report sourceReport)
            {
                if (sourceReport != null && sourceReport.Researches != null)
                {
                    Researches = new Research5XmlModel[sourceReport.Researches.Length];
                    for (int i = 0; i < sourceReport.Researches.Length; i++)
                    {
                        Researches[i] = new Research5XmlModel(sourceReport.Researches[i]);
                    }
                }
            }

            public Purple_5.Report BackReport()
            {
                var report = new Purple_5.Report();
                
                if (Researches != null && Researches.Length > 0)
                {
                    var originalResearches = new Purple_5.Research[Researches.Length];
                    for (int i = 0; i < Researches.Length; i++)
                    {
                        originalResearches[i] = Researches[i].BackResearch();
                    }
                    report.AddResearch(originalResearches);
                }
                
                return report;
            }
        }
        public override T DeserializePurple1<T>(string nameFile)
        {
            // 1. Установка файла для чтения
            SelectFile(nameFile);
            
            // 2. Чтение и десериализация файла
            using (var fileReader = new StreamReader(FilePath))
            {
                // 3. Определение типа и выполнение десериализации
                if (typeof(T) == typeof(Purple_1.Participant))
                {
                    return (T)(object)DeserializeParticipant<Purple_1.Participant>(fileReader);
                }
                else if (typeof(T) == typeof(Purple_1.Judge))
                {
                    return (T)(object)DeserializeJudge<Purple_1.Judge>(fileReader);
                }
                else if (typeof(T) == typeof(Purple_1.Competition))
                {
                    return (T)(object)DeserializeCompetition<Purple_1.Competition>(fileReader);
                }
            }
            
            return default(T);
        }

        // Вспомогательные методы для каждого типа:

        private T DeserializeParticipant<T>(StreamReader reader) where T : class
        {
            var serializer = new XmlSerializer(typeof(Participant1XmlModel));
            var participantData = (Participant1XmlModel)serializer.Deserialize(reader);
            return (T)(object)participantData.BackParticipant();
        }

        private T DeserializeJudge<T>(StreamReader reader) where T : class
        {
            var serializer = new XmlSerializer(typeof(Judge1XmlModel));
            var judgeData = (Judge1XmlModel)serializer.Deserialize(reader);
            return (T)(object)judgeData.BackJudge();
        }

        private T DeserializeCompetition<T>(StreamReader reader) where T : class
        {
            var serializer = new XmlSerializer(typeof(Competition1XmlModel));
            var competitionData = (Competition1XmlModel)serializer.Deserialize(reader);
            return (T)(object)competitionData.BackCompetition();
        }
        public override T DeserializePurple2SkiJumping<T>(string nameFile)
        {
            SelectFile(nameFile);
            var ser = new XmlSerializer(typeof(SkiJumping2XmlModel));
            using var read = new StreamReader(FilePath);
            var DTO = (SkiJumping2XmlModel) ser.Deserialize(read);
            return (T)DTO.BackSkiJumping();
        }
        public override T DeserializePurple3Skating<T>(string nameFile)
        {
            SelectFile(nameFile);
            var ser = new XmlSerializer(typeof(Skating3XmlModel));
            using var read = new StreamReader(FilePath);
            var DTO = (Skating3XmlModel) ser.Deserialize(read);
            return (T)DTO.BackSkating();
        }
        public override Purple_4.Group DeserializePurple4Group(string nameFile)
        {
            SelectFile(nameFile);
            var ser = new XmlSerializer(typeof(Group4XmlModel));
            using var read = new StreamReader(FilePath);
            var DTO = (Group4XmlModel) ser.Deserialize(read);
            return DTO.BackGroup();
        }

        public override Purple_5.Report DeserializePurple5Report(string nameFile)
        {
            SelectFile(nameFile);
            var ser = new XmlSerializer(typeof(Report5XmlModel));
            using var read = new StreamReader(FilePath);
            var DTO = (Report5XmlModel) ser.Deserialize(read);
            return DTO.BackReport();
        }
    }   
}