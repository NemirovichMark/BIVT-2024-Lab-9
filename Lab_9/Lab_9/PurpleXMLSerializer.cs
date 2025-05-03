using Lab_7;
using System.Xml.Serialization;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";

        public class Purple_1_Participant_DTO
        {
            public Purple_1_Participant_DTO() {}
            public Purple_1_Participant_DTO(Purple_1.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Coefs = participant.Coefs;
                TotalScore = participant.TotalScore;

                int m = participant.Marks.GetLength(0);
                int n = participant.Marks.GetLength(1);

                Marks = new int[m][];

                for (int i = 0; i < m; i++)
                {
                    Marks[i] = new int[n];
                    for (int j = 0; j < n; j++)
                        Marks[i][j] = participant.Marks[i, j];

                }
            }

            public string Name { 
                get; set; 
            }
            public string Surname { 
                get; set; 
            }
            public double[] Coefs { 
                get; set; 
            }
            public int[][] Marks { 
                get; set; 
            }
            public double TotalScore { 
                get; set; 
            }
        }

        public class Purple_1_Judge_DTO
        {
            public Purple_1_Judge_DTO() {}
            public Purple_1_Judge_DTO(Purple_1.Judge judge)
            {
                Name = judge.Name;
                Marks = judge.Marks;
            }

            public string Name {
                get; set; 
            }
            public int[] Marks { 
                get; set; 
            }
        }

        public class Purple_1_Competition_DTO
        {
            public Purple_1_Competition_DTO() {}
            public Purple_1_Competition_DTO(Purple_1.Competition competition)
            {
                int judgesLen = competition.Judges.Length;
                int participantsLen = competition.Participants.Length;
                Judges = new Purple_1_Judge_DTO[judgesLen];
                Participants = new Purple_1_Participant_DTO[participantsLen];

                for (int i = 0; i < judgesLen; i++)
                    Judges[i] = new Purple_1_Judge_DTO(competition.Judges[i]);

                for (int i = 0; i < participantsLen; i++)
                    Participants[i] = new Purple_1_Participant_DTO(competition.Participants[i]);

            }

            public Purple_1_Judge_DTO[] Judges { 
                get; set; 
            }
            public Purple_1_Participant_DTO[] Participants {
                get; set; 
            }
        }

        public class Purple_2_Participant_DTO
        {
            public Purple_2_Participant_DTO() {}
            public Purple_2_Participant_DTO(Purple_2.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Distance = participant.Distance;
                Result = participant.Result;
                Marks = participant.Marks;
            }

            public string Name { 
                get; set; 
            }
            public string Surname { 
                get; set; 
            }
            public int Distance { 
                get; set; 
            }
            public int Result
            {
                get; set;
            }
            public int[] Marks { 
                get; set; 
            }
        }
        public class Purple_2_SkiJumping_DTO
        {
            public Purple_2_SkiJumping_DTO() {}
            public Purple_2_SkiJumping_DTO(Purple_2.SkiJumping jumping)
            {
                Name = jumping.Name;
                Standard = jumping.Standard;

                int participantsLen = jumping.Participants.Length;
                Participants = new Purple_2_Participant_DTO[participantsLen];
                for (int i = 0; i < participantsLen; i++)
                    Participants[i] = new Purple_2_Participant_DTO(jumping.Participants[i]);

                Type = jumping.GetType().AssemblyQualifiedName;
            }

            public string Name { 
                get; set; 
            }
            public int Standard { 
                get; set; 
            }
            public Purple_2_Participant_DTO[] Participants { 
                get; set; 
            }
            public string Type { 
                get; set; 
            }
        }

        public class Purple_3_Participant_DTO
        {
            public Purple_3_Participant_DTO() {}
            public Purple_3_Participant_DTO(Purple_3.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = participant.Marks;
                Places = participant.Places;
                Score = participant.Score;
            }

            public string Name { 
                get; set; 
            }
            public string Surname { 
                get; set; 
            }
            public double[] Marks { 
                get; set; 
            }
            public int[] Places { 
                get; set; 
            }
            public int Score { 
                get; set; 
            }
        }

        public class Purple_3_Skating_DTO
        {
            public Purple_3_Skating_DTO() {}
            public Purple_3_Skating_DTO(Purple_3.Skating skating)
            {
                
                Moods = skating.Moods;

                int participantsLen = skating.Participants.Length;
                Participants = new Purple_3_Participant_DTO[participantsLen];

                for (int i = 0; i < participantsLen; i++)
                    Participants[i] = new Purple_3_Participant_DTO(skating.Participants[i]);

                Type = skating.GetType().AssemblyQualifiedName;
            }

            public double[] Moods { 
                get; set; 
            }
            public Purple_3_Participant_DTO[] Participants { 
                get; set; 
            }
            public string Type { 
                get; set; 
            }
        }

        public class Purple_4_Sportsman_DTO
        {
            public Purple_4_Sportsman_DTO() {}
            public Purple_4_Sportsman_DTO(Purple_4.Sportsman sportsman)
            {
                Name = sportsman.Name;
                Surname = sportsman.Surname;
                Time = sportsman.Time;
            }

            public string Name { 
                get; set; 
            }
            public string Surname { 
                get; set; 
            }
            public double Time { 
                get; set; 
            }
        }

        public class Purple_4_Group_DTO
        {
            public Purple_4_Group_DTO() {}
            public Purple_4_Group_DTO(Purple_4.Group group)
            {
                Name = group.Name;
                int sportsmenLen = group.Sportsmen.Length;
                Sportsmen = new Purple_4_Sportsman_DTO[sportsmenLen];
                for (int i = 0; i < sportsmenLen; i++)
                    Sportsmen[i] = new Purple_4_Sportsman_DTO(group.Sportsmen[i]);
            }

            public string Name { 
                get; set; 
            }
            public Purple_4_Sportsman_DTO[] Sportsmen { 
                get; set; 
            }
        }

        public class Purple_5_Response_DTO
        {
            public Purple_5_Response_DTO() {}
            public Purple_5_Response_DTO(Purple_5.Response resp)
            {
                Animal = resp.Animal;
                CharacterTrait = resp.CharacterTrait;
                Concept = resp.Concept;
            }

            public string Animal { 
                get; set; 
            }
            public string CharacterTrait { 
                get; set;
            }
            public string Concept { 
                get; set; 
            }
        }
        public class Purple_5_Research_DTO
        {
            public Purple_5_Research_DTO() { }
            public Purple_5_Research_DTO(Purple_5.Research research)
            {
                Name = research.Name;
                int respLen = research.Responses.Length;
                Responses = new Purple_5_Response_DTO[respLen];
                for (int i = 0; i < respLen; i++)
                    Responses[i] = new Purple_5_Response_DTO(research.Responses[i]);
            }

            public string Name { 
                get; set; 
            }
            public Purple_5_Response_DTO[] Responses { 
                get; set; 
            }

        }
        public class Purple_5_Report_DTO
        {
            public Purple_5_Report_DTO() { }
            public Purple_5_Report_DTO(Purple_5.Report report)
            {
                int researchLen = report.Researches.Length;
                Researches = new Purple_5_Research_DTO[researchLen];
                for (int i = 0; i < researchLen; i++)
                    Researches[i] = new Purple_5_Research_DTO(report.Researches[i]);
            }

            public Purple_5_Research_DTO[] Researches { 
                get; set; 
            }
        }

        private void SerializeAny<T>(T obj)
        {
            if (FilePath == null || FolderPath == null) return;

            string filePath = Path.Combine(FolderPath, FilePath);

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, obj);
            }
        }

        private T DeserializeAny<T>()
        {
            if (FilePath == null || FolderPath == null) return default;

            string filePath = Path.Combine(FolderPath, FilePath);
            if (!File.Exists(filePath))
            {
                return default;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamReader reader = new StreamReader(filePath))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        private int[,] ToMatrix(int[][] arr)
        {
            int m = arr.Length; // строки
            int n = arr[0].Length; // столбцы

            int[,] matrix = new int[m, n];

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = arr[i][j];
                }
            }

            return matrix;
        }
        
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);

            if (obj is Purple_1.Participant participant)
            {
                var clone = new Purple_1_Participant_DTO(participant);
                SerializeAny(clone);
            } else if (obj is Purple_1.Judge judge)
            {
                var clone = new Purple_1_Judge_DTO(judge);
                SerializeAny(clone);
            } else if (obj is Purple_1.Competition competition)
            {
                var clone = new Purple_1_Competition_DTO(competition);
                SerializeAny(clone);
            }
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);

            var clone = new Purple_2_SkiJumping_DTO(jumping);
            SerializeAny(clone);
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);

            var clone = new Purple_3_Skating_DTO(skating);
            SerializeAny(clone);
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);

            var clone = new Purple_4_Group_DTO(group);
            SerializeAny(clone);
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);

            var clone = new Purple_5_Report_DTO(report);
            SerializeAny(clone);
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);

            if (typeof(T) == typeof(Purple_1.Participant))
            {
                var obj = DeserializeAny<Purple_1_Participant_DTO>();

                var name = obj.Name;
                var surname = obj.Surname;
                var coefs = obj.Coefs;
                var marks = ToMatrix(obj.Marks);

                Purple_1.Participant participant = new Purple_1.Participant(name, surname);
                participant.SetCriterias(coefs);

                for (int i = 0; i < 4; i++)
                {
                    int[] jumpMarks = new int[7];

                    for (int j = 0; j < 7; j++) jumpMarks[j] = marks[i, j];

                    participant.Jump(jumpMarks);
                }

                return participant as T;
            } else if (typeof(T) == typeof(Purple_1.Judge))
            {
                var obj = DeserializeAny<Purple_1_Judge_DTO>();

                var name = obj.Name;
                var marks = obj.Marks;

                return (new Purple_1.Judge(name, marks)) as T;
            } else // Purple_1.Competition
            {
                var obj = DeserializeAny<Purple_1_Competition_DTO>();

                var realJudges = obj.Judges.Select(fake => new Purple_1.Judge(fake.Name, fake.Marks)).ToArray();
                var realParticipants = obj.Participants.Select(fake =>
                {
                    var realParticipant = new Purple_1.Participant(fake.Name, fake.Surname);
                    realParticipant.SetCriterias(fake.Coefs);

                    var marks = ToMatrix(fake.Marks);

                    for (int i = 0; i < 4; i++)
                    {
                        int[] jumpMarks = new int[7];

                        for (int j = 0; j < 7; j++) jumpMarks[j] = marks[i, j];

                        realParticipant.Jump(jumpMarks);
                    }

                    return realParticipant;
                }).ToArray();

                Purple_1.Competition competition = new Purple_1.Competition(realJudges);
                competition.Add(realParticipants);

                return competition as T;
            }
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);

            var obj = DeserializeAny<Purple_2_SkiJumping_DTO>();

            var typeName = Type.GetType(obj.Type)?.Name;
            Purple_2.SkiJumping jumping;

            if (typeName == "JuniorSkiJumping")
            {
                jumping = new Purple_2.JuniorSkiJumping();
            } else // ProSkiJumping
            {
                jumping = new Purple_2.ProSkiJumping();
            }

            var realParticipants = obj.Participants.Select(fake =>
            {
                var realParticipant = new Purple_2.Participant(fake.Name, fake.Surname);
                realParticipant.Jump(fake.Distance, fake.Marks, obj.Standard);

                return realParticipant;
            }).ToArray();

            jumping.Add(realParticipants);

            return (T)jumping;
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);

            var obj = DeserializeAny<Purple_3_Skating_DTO>();

            var typeName = Type.GetType(obj.Type)?.Name;
            Purple_3.Skating skating;

            if (typeName == "FigureSkating")
            {
                skating = new Purple_3.FigureSkating(obj.Moods, false);
            } else // IceSkating
            {
                skating = new Purple_3.IceSkating(obj.Moods, false);
            }

            var realParticipants = obj.Participants.Select(fake =>
            {
                var realParticipant = new Purple_3.Participant(fake.Name, fake.Surname);

                foreach (var mark in fake.Marks) realParticipant.Evaluate(mark);

                return realParticipant;
            }).ToArray();

            Purple_3.Participant.SetPlaces(realParticipants);
            skating.Add(realParticipants);

            return (T)skating;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);

            var obj = DeserializeAny<Purple_4_Group_DTO>();

            var name = obj.Name;
            Purple_4.Group group = new Purple_4.Group(name);

            var realSportsmen = obj.Sportsmen.Select(fake =>
            {
                var realSportsman = new Purple_4.Sportsman(fake.Name, fake.Surname);
                realSportsman.Run(fake.Time);

                return realSportsman;
            }).ToArray();

            group.Add(realSportsmen);

            return group;
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);

            var obj = DeserializeAny<Purple_5_Report_DTO>();

            Purple_5.Report report = new Purple_5.Report();

            var realResearches = obj.Researches.Select(fake =>
            {
                var realResearch = new Purple_5.Research(fake.Name);

                foreach (var resp in fake.Responses) realResearch.Add(new string[] { resp.Animal, resp.CharacterTrait, resp.Concept });

                return realResearch;
            }).ToArray();

            report.AddResearch(realResearches);

            return report;
        }
    }
}
