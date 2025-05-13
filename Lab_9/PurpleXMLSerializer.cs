using Lab_7;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Formats.Asn1.AsnWriter;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        //свойства
        public override string Extension => "xml";

        //классы
        public class Purple_1_Participant_DTO
        {
            //конструкторы
            public Purple_1_Participant_DTO() { }
            public Purple_1_Participant_DTO(Purple_1.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Coefs = participant.Coefs;

                int[][] jagged;
                ToJaggedArray(out jagged, participant.Marks);
                Marks = jagged;
            }
            //свойства
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int[][] Marks { get; set; }
            public double TotalScore { get; set; }
        }
        public class Purple_1_Judge_DTO
        {
            //конструкторы
            public Purple_1_Judge_DTO() { }
            public Purple_1_Judge_DTO(Purple_1.Judge judge)
            {
                Name = judge.Name;
                Marks = judge.Marks;
            }
            //свойства
            public string Name { get; set; }
            public int[] Marks { get; set; }
        }
        public class Purple_1_Competition_DTO
        {
            //конструкторы
            public Purple_1_Competition_DTO() { }
            public Purple_1_Competition_DTO(Purple_1.Competition competition)
            {
                Judges = new Purple_1_Judge_DTO[competition.Judges.Length];
                for(int i = 0; i < Judges.Length; i++)
                {
                    Judges[i] = new Purple_1_Judge_DTO(competition.Judges[i]);
                }

                Participants = new Purple_1_Participant_DTO[competition.Participants.Length];
                for(int i = 0; i < Participants.Length; i++)
                {
                    Participants[i] = new Purple_1_Participant_DTO(competition.Participants[i]);
                }
            }
            //свойства
            public Purple_1_Judge_DTO[] Judges { get; set; }
            public Purple_1_Participant_DTO[] Participants { get; set; }
        }
        public class Purple_2_Participant_DTO {
            //конструкторы
            public Purple_2_Participant_DTO() { }
            public Purple_2_Participant_DTO(Purple_2.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Distance = participant.Distance;
                Marks = participant.Marks;
                Result = participant.Result;
            }
            //свойства
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }
            public int Result { get; set; }
        }
        public class Purple_2_SkiJumping_DTO
        {
            //конструкторы
            public Purple_2_SkiJumping_DTO() { }
            public Purple_2_SkiJumping_DTO(Purple_2.SkiJumping jumper) 
            {
                Type = jumper.GetType().ToString();
                Name = jumper.Name;
                Standard = jumper.Standard;
                Participants = new Purple_2_Participant_DTO[jumper.Participants.Length];
                for (int i = 0; i < Participants.Length; i++)
                {
                    Participants[i] = new Purple_2_Participant_DTO(jumper.Participants[i]);
                }
            }
            //свойства
            public string Type { get; set; }
            public string Name { get; set; }
            public int Standard { get; set; }
            public Purple_2_Participant_DTO[] Participants { get; set; }
        }
        public class Purple_3_Participant_DTO
        {
            //конструкторы
            public Purple_3_Participant_DTO() { }
            public Purple_3_Participant_DTO(Purple_3.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = participant.Marks;
                Places = participant.Places;
                Score = participant.Score;
            }
            //свойства
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Marks { get; set; }
            public int[] Places { get; set; }
            public int Score { get; set; }
        }
        public class Purple_3_Skating_DTO
        {
            //конструкторы
            public Purple_3_Skating_DTO() { }
            public Purple_3_Skating_DTO(Purple_3.Skating skating)

            {
                Type = skating.GetType().ToString();

                Participants = new Purple_3_Participant_DTO[skating.Participants.Length];
                for(int i = 0; i < Participants.Length; i++)
                {
                    Participants[i] = new Purple_3_Participant_DTO(skating.Participants[i]);
                }

                Moods = skating.Moods;
            }
            //свойства
            public string Type { get; set; }
            public Purple_3_Participant_DTO[] Participants { get; set; }
            public double[] Moods { get; set; }
        }
        public class Purple_4_Sportsman_DTO
        {
            //конструкторы
            public Purple_4_Sportsman_DTO() { }
            public Purple_4_Sportsman_DTO(Purple_4.Sportsman sportsman)
            {
                Type = sportsman.GetType().ToString();
                Name = sportsman.Name;
                Surname = sportsman.Surname;
                Time = sportsman.Time;
            }
            //свойства
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }
        }
        public class Purple_4_Group_DTO
        {
            //конструкторы
            public Purple_4_Group_DTO() { }
            public Purple_4_Group_DTO(Purple_4.Group group)
            {
                Name = group.Name;
                Sportsmen = new Purple_4_Sportsman_DTO[group.Sportsmen.Length];
                for(int i = 0; i < Sportsmen.Length; i++)
                {
                    Sportsmen[i] = new Purple_4_Sportsman_DTO(group.Sportsmen[i]);
                }
            }
            //свойства
            public string Name { get; set; }
            public Purple_4_Sportsman_DTO[] Sportsmen { get; set; }
        }
        public class Purple_5_Response_DTO
        {
            //конструкторы
            public Purple_5_Response_DTO() { }
            public Purple_5_Response_DTO(Purple_5.Response response)
            {
                Animal = response.Animal;
                CharacterTrait = response.CharacterTrait;
                Concept = response.Concept;
            }
            //свойства
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }
        }
        public class Purple_5_Research_DTO
        {
            //конструкторы
            public Purple_5_Research_DTO() { }
            public Purple_5_Research_DTO(Purple_5.Research research) {
                Name = research.Name;
                Responses = new Purple_5_Response_DTO[research.Responses.Length];
                for (int i = 0; i < Responses.Length; i++)
                {
                    Responses[i] = new Purple_5_Response_DTO(research.Responses[i]);
                }
            }
            //свойства
            public string Name { get; set;}
            public Purple_5_Response_DTO[] Responses { get; set; }
        }
        public class Purple_5_Report_DTO
        {
            //конструторы
            public Purple_5_Report_DTO() { }
            public Purple_5_Report_DTO(Purple_5.Report report)
            {
                Researches = new Purple_5_Research_DTO[report.Researches.Length];
                for(int i = 0; i < Researches.Length; i++)
                {
                    Researches[i] = new Purple_5_Research_DTO(report.Researches[i]);
                }
            }
            //свойство
            public Purple_5_Research_DTO[] Researches { get; set; }
        }

        //методы
        private static void ToJaggedArray<T>(out T[][] jagged, T[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            jagged = new T[rows][];
            for(int i = 0; i < rows; i++)
            {
                jagged[i] = new T[cols];
                for(int j = 0; j < cols; j++)
                {
                    jagged[i][j] = matrix[i, j];
                }
            }
        }
        
        private static void ToMatrix<T>(out T[,] matrix, T[][] jagged)
        {
            int rows = jagged.Length;
            int cols = jagged[0].Length;
            matrix = new T[rows, cols];
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    matrix[i,j] = jagged[i][j];
                }
            }
        }
        
        private void Serialize<T>(T obj, string fileName)
        {
            SelectFile(fileName);

            if (FilePath == null || obj == null) return;

            XmlSerializer xml = new XmlSerializer(obj.GetType());

            using(StreamWriter stream = new StreamWriter(FilePath))
            {
                xml.Serialize(stream, obj);
            }
        }
        
        private T Deserialize<T>()
        {
            if (!File.Exists(FilePath)) return default(T);

            var xml = new XmlSerializer(typeof(T));

            using(var stream = new StreamReader(FilePath))
            {
                return (T) xml.Deserialize(stream);
            }
        }
        
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            if(typeof(T) == typeof(Purple_1.Participant))
            {
                var participant = new Purple_1_Participant_DTO(obj as Purple_1.Participant);
                Serialize(participant, fileName);
            }
            else if (typeof(T) == typeof(Purple_1.Judge))
            {
                var judge = new Purple_1_Judge_DTO(obj as Purple_1.Judge);
                Serialize(judge, fileName);
            }
            else
            {
                var competition = new Purple_1_Competition_DTO(obj as Purple_1.Competition);
                Serialize(competition, fileName);
            }
        }
        
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            var obj = new Purple_2_SkiJumping_DTO(jumping);
            Serialize(obj, fileName);
        }
        
        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            var obj = new Purple_3_Skating_DTO(skating);
            Serialize(obj, fileName);
        }
        
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            var obj = new Purple_4_Group_DTO(group);
            Serialize(obj, fileName);
        }
        
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            var obj = new Purple_5_Report_DTO(report);
            Serialize(obj, fileName);
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            if(typeof(T) == typeof(Purple_1.Participant))
            {
                var data = Deserialize<Purple_1_Participant_DTO>();
                if (data == null) return null;
                //извлекаем данные
                string Name = data.Name;
                string Surname = data.Surname;
                double[] Coefs = data.Coefs;
                int[,] Marks;
                ToMatrix(out Marks, data.Marks);
                //заполняем
                var result = new Purple_1.Participant(Name, Surname);
                result.SetCriterias(Coefs);
                for (int i = 0; i < 4; i++)
                {
                    int[] jump = new int[7];
                    for (int j = 0; j < 7; j++)
                    {
                        jump[j] = Marks[i, j];
                    }
                    result.Jump(jump);
                }

                return result as T;
            }
            else if(typeof(T) == typeof(Purple_1.Judge)) 
            {
                var data = Deserialize<Purple_1_Judge_DTO>();
                if (data == null) return null;
                //извлекаем данные
                string Name = data.Name;
                int[] Marks = data.Marks;
                //заполняем данными
                Purple_1.Judge result = new Purple_1.Judge(Name, Marks);
                return result as T;
            }
            else
            {
                var data = Deserialize<Purple_1_Competition_DTO>();
                if (data == null) return null;
                //извлекаем данные
                var Judges = data.Judges.Select(judges => new Purple_1.Judge(judges.Name, judges.Marks)).ToArray();
                var Participants = data.Participants.Select(participant =>
                {
                    var result = new Purple_1.Participant(participant.Name, participant.Surname);
                    result.SetCriterias(participant.Coefs);
                    int[,] Marks;
                    ToMatrix(out Marks, participant.Marks);
                    for (int i = 0; i < 4; i++)
                    {
                        int[] jump = new int[7];
                        for (int j = 0; j < 7; j++)
                        {
                            jump[j] = Marks[i, j];
                        }
                        result.Jump(jump);
                    }
                    return result;
                }).ToArray();
                //заполняем данными
                var result = new Purple_1.Competition(Judges);
                result.Add(Participants);
                return result as T;
            }
        }
        
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var data = Deserialize<Purple_2_SkiJumping_DTO>();
            if (data == null) return null;
            //извлекаем данные
            var Participants = data.Participants.Select(participant =>
            {
                //извлекаем данные
                var Name = participant.Name;
                var Surname = participant.Surname;
                int Distance = participant.Distance;
                int[] Marks = participant.Marks;
                int Result = participant.Result;
                int Target = (int) Math.Ceiling(Distance - (Result - (Marks.Sum() - Marks.Min() - Marks.Max()) - 60) / 2.0);
                //заполняем данными
                var result = new Purple_2.Participant(Name, Surname);
                result.Jump(Distance, Marks, Target);

                return result;
            }).ToArray();
            //заполняем данными
            if(data.Type.Contains("JuniorSkiJumping"))
            {
                var result = new Purple_2.JuniorSkiJumping();
                result.Add(Participants);
                return result as T;
            }
            else
            {
                var result = new Purple_2.ProSkiJumping();
                result.Add(Participants);
                return result as T;
            }
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var data = Deserialize<Purple_3_Skating_DTO>();
            if (data == null) return null;
            //извлекаем данные
            var Moods = data.Moods;
            var Participants = data.Participants.Select(participant =>
            {
                //извлекаем данные участника
                var Name = participant.Name;
                var Surname = participant.Surname;
                var Marks = participant.Marks;

                //заполняем участника данными
                var result = new Purple_3.Participant(Name, Surname);
                foreach (var mark in Marks)
                {
                    result.Evaluate(mark);
                }

                return result;
            }).ToArray();
            Purple_3.Participant.SetPlaces(Participants);
            //заполняем данными
            if (data.Type.Contains("FigureSkating"))
            {
                var result = new Purple_3.FigureSkating(Moods, false);
                result.Add(Participants);
                return result as T;
            }
            else
            {
                var result = new Purple_3.IceSkating(Moods, false);
                result.Add(Participants);
                return result as T;
            }
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var data = Deserialize<Purple_4_Group_DTO>();
            if (data == null) return null;
            //извлекаем данные
            var Name = data.Name;
            var Sportsmen = data.Sportsmen.Select(sportsman =>
            {

                string NameSportsman = sportsman.Name;
                string Surname = sportsman.Surname;
                double Time = sportsman.Time;

                if (sportsman.Type.Contains("SkiMan"))
                {
                    var man = new Purple_4.SkiMan(NameSportsman, Surname);
                    man.Run(Time);
                    return man;
                }
                else if (sportsman.Type.Contains("SkiWoman"))
                {
                    var woman = new Purple_4.SkiWoman(NameSportsman, Surname);
                    woman.Run(Time);
                    return woman;
                }
                else
                {
                    var sport = new Purple_4.Sportsman(NameSportsman, Surname);
                    sport.Run(Time);
                    return sport;
                }
            }).ToArray();
            //заполняем данными
            var result = new Purple_4.Group(Name);
            result.Add(Sportsmen);

            return result;
        }
        
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var data = Deserialize<Purple_5_Report_DTO>();
            //извлекаем данные
            var Researches = data.Researches.Select(research => {
                //извлекаем данные
                var Name = research.Name;
                var Responses = research.Responses;
                //заполняем данными
                var result = new Purple_5.Research(Name);
                foreach(var response in Responses)
                {
                    result.Add([response.Animal, response.CharacterTrait, response.Concept]);
                }
                return result;
            }).ToArray();
            //заполняем данными
            var result = new Purple_5.Report();
            result.AddResearch(Researches);
            
            return result;
        }
    }
}
