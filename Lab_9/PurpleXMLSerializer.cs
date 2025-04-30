using System.Xml.Serialization;
using Lab_7;

namespace Lab_9 {
    public class PurpleXMLSerializer  : PurpleSerializer {
        public override string Extension => "xml";

        public class Purple_1_ParticipantDTO {
            public string Name {get; set; }
            public string Surname {get; set; }
            public double[] Coefs {get; set; }
            public int[][] Marks {get; set; }
            public double TotalScore {get; set; }
            public Purple_1_ParticipantDTO() {}
            public Purple_1_ParticipantDTO(Purple_1.Participant obj) {
                Name = obj.Name;
                Surname = obj.Surname;
                Coefs = obj.Coefs;
                TotalScore = obj.TotalScore;      

                int n = obj.Marks.GetLength(0);
                int m = obj.Marks.GetLength(1);

                Marks = new int[n][];

                for (int i = 0; i < n; i++) {
                    Marks[i] = new int[m];
                    for (int j = 0; j < m; j++) 
                        Marks[i][j] = obj.Marks[i, j];
                    
                }
            }
        }

        public class Purple_1_JudgeDTO {
            public string Name {get; set; }
            public int[] Marks {get; set; }
            public Purple_1_JudgeDTO() {}
            public Purple_1_JudgeDTO(Purple_1.Judge obj) {
                Name = obj.Name;
                Marks = obj.Marks;
            }
        }

        public class Purple_1_CompetitionDTO {
            public Purple_1_JudgeDTO[] Judges {get; set; }
            public Purple_1_ParticipantDTO[] Participants {get; set; }
            public Purple_1_CompetitionDTO() {}
            public Purple_1_CompetitionDTO(Purple_1.Competition obj) {
                int jCount = obj.Judges.Length;
                int pCount = obj.Participants.Length;
                Judges = new Purple_1_JudgeDTO[jCount];
                Participants = new Purple_1_ParticipantDTO[pCount];

                for (int i = 0; i < jCount; i++) 
                    Judges[i] = new Purple_1_JudgeDTO(obj.Judges[i]);
                
                for (int i = 0; i < pCount; i++) 
                    Participants[i] = new Purple_1_ParticipantDTO(obj.Participants[i]);
                
            }
        }

        public class Purple_2_ParticipantDTO {
            public string Name {get; set; }
            public string Surname {get; set; }
            public int Distance {get; set; }
            public int[] Marks {get; set; }
            public int Result { get; set; }
            public Purple_2_ParticipantDTO() {}
            public Purple_2_ParticipantDTO(Purple_2.Participant obj) {
                Name = obj.Name;
                Surname = obj.Surname;
                Distance = obj.Distance;
                Marks = obj.Marks;
                Result = obj.Result;
            }
        }
        public class Purple_2_SkiJumpingDTO {
            public string Type {get; set; }
            public string Name {get; set; }
            public int Standard {get; set; }
            public Purple_2_ParticipantDTO[] Participants {get; set; }
            public Purple_2_SkiJumpingDTO() {}
            public Purple_2_SkiJumpingDTO(Purple_2.SkiJumping obj) {
                Type = obj.GetType().AssemblyQualifiedName;
                Name = obj.Name;
                Standard = obj.Standard;

                int pCount = obj.Participants.Length;
                Participants = new Purple_2_ParticipantDTO[pCount];
                for (int i = 0; i < pCount; i++) 
                    Participants[i] = new Purple_2_ParticipantDTO(obj.Participants[i]);
            }
        }

        public class Purple_3_ParticipantDTO {
            public string Name {get; set; }
            public string Surname {get; set; }
            public double[] Marks {get; set; }
            public int[] Places {get; set; }
            public int Score {get; set; }
            public Purple_3_ParticipantDTO() {}
            public Purple_3_ParticipantDTO(Purple_3.Participant obj) {
                Name = obj.Name;
                Surname = obj.Surname;
                Marks = obj.Marks;
                Places = obj.Places;
                Score = obj.Score;
            }
        }

        public class Purple_3_SkatingDTO {
            public string Type {get; set; }
            public Purple_3_ParticipantDTO[] Participants {get; set; }
            public double[] Moods {get; set; }
            public Purple_3_SkatingDTO() {}
            public Purple_3_SkatingDTO(Purple_3.Skating obj) {
                Type = obj.GetType().AssemblyQualifiedName;
                Moods = obj.Moods;

                int pCount = obj.Participants.Length;
                Participants = new Purple_3_ParticipantDTO[pCount];

                for (int i = 0; i < pCount; i++)   
                    Participants[i] = new Purple_3_ParticipantDTO(obj.Participants[i]);

            }
        }

        public class Purple_4_SportsmanDTO {
            public string Name {get; set; }
            public string Surname {get; set; }
            public double Time {get; set; }
            public Purple_4_SportsmanDTO() {}
            public Purple_4_SportsmanDTO(Purple_4.Sportsman obj) {
                Name = obj.Name;
                Surname = obj.Surname;
                Time = obj.Time;
            } 
        }
        
        public class Purple_4_GroupDTO {
            public string Name {get; set; }
            public Purple_4_SportsmanDTO[] Sportsmen {get; set; }
            public Purple_4_GroupDTO() {}
            public Purple_4_GroupDTO(Purple_4.Group obj) {
                Name = obj.Name;
                int sCount = obj.Sportsmen.Length;
                Sportsmen = new Purple_4_SportsmanDTO[sCount];
                for (int i = 0; i < sCount; i++)
                    Sportsmen[i] = new Purple_4_SportsmanDTO(obj.Sportsmen[i]);
            }
        }

        public class Purple_5_ResponseDTO {
            public string Animal {get; set; }
            public string CharacterTrait {get; set; }
            public string Concept {get; set; }
            public Purple_5_ResponseDTO() {}
            public Purple_5_ResponseDTO(Purple_5.Response obj) {
                Animal = obj.Animal;
                CharacterTrait = obj.CharacterTrait;
                Concept = obj.Concept;
            }

        }
        public class Purple_5_ResearchDTO {
            public string Name {get; set; }
            public Purple_5_ResponseDTO[] Responses {get; set; }
            public Purple_5_ResearchDTO() {}
            public Purple_5_ResearchDTO(Purple_5.Research obj) {
                Name = obj.Name;
                int rCount = obj.Responses.Length;
                Responses = new Purple_5_ResponseDTO[rCount];
                for (int i = 0; i < rCount; i++)
                    Responses[i] = new Purple_5_ResponseDTO(obj.Responses[i]);
            }
        }
        public class Purple_5_ReportDTO {
            public Purple_5_ResearchDTO[] Researches {get; set; }
            public Purple_5_ReportDTO() {}
            public Purple_5_ReportDTO(Purple_5.Report obj) {
                int rCount = obj.Researches.Length;
                Researches = new Purple_5_ResearchDTO[rCount];
                for (int i = 0; i < rCount; i++)
                    Researches[i] = new Purple_5_ResearchDTO(obj.Researches[i]);
            }

        }
        private void SerializeObject<T>(T obj) {
            
            if (FolderPath == null || FilePath  == null) return;

            string targetPath = $"{Path.Combine(FolderPath, FilePath)}.{Extension}";

            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StreamWriter(targetPath)) {
                serializer.Serialize(writer, obj);
            }

        }
        private T GetFileData<T>() {
            if (FolderPath == null || FilePath == null) return default!;

            string targetPath = $"{Path.Combine(FolderPath, FilePath)}.{Extension}";
            if (!File.Exists(targetPath)) return default!;

            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StreamReader(targetPath)) {
                return (T)serializer.Deserialize(reader);
            }
        }

        private int[,] ConvertToMatrix(int[][] jaggedArr) {
            int n = jaggedArr.Length;
            int m = jaggedArr[0].Length;
            var matrix = new int[n, m];

            for (int i = 0; i < n; i++) {
                for (int j = 0; j < m; j++) 
                    matrix[i, j] = jaggedArr[i][j];
            }

            return matrix;
        }
        public override void SerializePurple1<T>(T obj, string fileName)  {
            SelectFile(fileName);

            if (obj is Purple_1.Participant pObj) {
                var curObj = new Purple_1_ParticipantDTO(pObj);
                SerializeObject(curObj);
            } else if (obj is Purple_1.Judge jObj) {
                var curObj = new Purple_1_JudgeDTO(jObj);
                SerializeObject(curObj);
            } else if (obj is Purple_1.Competition cObj) {
                var curObj = new Purple_1_CompetitionDTO(cObj);
                SerializeObject(curObj);
            }
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName) {
            SelectFile(fileName);    
            var curObj = new Purple_2_SkiJumpingDTO(jumping);
            SerializeObject(curObj);
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName) {
            SelectFile(fileName);
            var curObj = new Purple_3_SkatingDTO(skating);
            SerializeObject(curObj);
        }

        public override void SerializePurple4Group(Purple_4.Group participant, string fileName) {
            SelectFile(fileName);
            var curObj = new Purple_4_GroupDTO(participant);
            SerializeObject(curObj);
        }

        public override void SerializePurple5Report(Purple_5.Report group, string fileName) {
            SelectFile(fileName);
            var curObj = new Purple_5_ReportDTO(group);
            SerializeObject(curObj);
        }

        public override T DeserializePurple1<T>(string fileName) {
            SelectFile(fileName);

            if (typeof(T) == typeof(Purple_1.Participant)) { 
                var curObj = GetFileData<Purple_1_ParticipantDTO>();
            
                var Name = curObj.Name;
                var Surname = curObj.Surname;
                var Coefs = curObj.Coefs;
                var Marks = ConvertToMatrix(curObj.Marks);

                var resultObj = new Purple_1.Participant(Name, Surname);
                resultObj.SetCriterias(Coefs);
                
                for (int i = 0; i < 4; i++) {
                    int[] curRow = new int[7];

                    for (int j = 0; j < 7; j++) 
                        curRow[j] = Marks[i, j];
                    
                    resultObj.Jump(curRow);
                }

                return resultObj as T;
            } 

            else if (typeof(T) == typeof(Purple_1.Judge)) { 
                var curObj = GetFileData<Purple_1_JudgeDTO>();

                var Name = curObj.Name;
                var Marks = curObj.Marks;

                var resultObj = new Purple_1.Judge(Name, Marks);

                return resultObj as T;
            }
            
            else { // Competition class
                var curObj = GetFileData<Purple_1_CompetitionDTO>();

                var Judges = curObj.Judges.Select(j => new Purple_1.Judge(j.Name, j.Marks)).ToArray();

                var Participants = curObj.Participants.Select(p => {

                    var curP = new Purple_1.Participant(p.Name, p.Surname);
                    curP.SetCriterias(p.Coefs);
                    
                    var Marks = ConvertToMatrix(p.Marks);
                    
                    for (int i = 0; i < 4; i++) {
                        int[] curRow = new int[7];

                        for (int j = 0; j < 7; j++) 
                            curRow[j] = Marks[i, j];
                        
                        curP.Jump(curRow);
                    }

                    return curP;
                }).ToArray();

                var resultObj = new Purple_1.Competition(Judges);

                resultObj.Add(Participants);

                return resultObj as T;
            } 
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName) {
            SelectFile(fileName);

            var curObj = GetFileData<Purple_2_SkiJumpingDTO>();

            dynamic resultObj = Activator.CreateInstance(Type.GetType(curObj.Type));

            var Participants = curObj.Participants.Select(p => {
                var curP = new Purple_2.Participant(p.Name, p.Surname);

                var Distance = p.Distance;
                var Marks = p.Marks;
                var Result = p.Result;
                var SumWithoutMinMax = Marks.Sum() - Marks.Min() - Marks.Max();
                var target = Math.Ceiling((Result == 0) ? (Distance + (SumWithoutMinMax + 60) / 2.0) 
                                           : (Distance - (Result - SumWithoutMinMax - 60) / 2.0));  
                curP.Jump(Distance, Marks, (int)target);

                return curP;
            }).ToArray();

            resultObj.Add(Participants);  

            return resultObj;
        }

        public override T DeserializePurple3Skating<T>(string fileName) {
            SelectFile(fileName);

            var curObj = GetFileData<Purple_3_SkatingDTO>();

            dynamic resultObj = Activator.CreateInstance(Type.GetType(curObj.Type), curObj.Moods, false);

            var Participants = curObj.Participants.Select(p => {
                var curP = new Purple_3.Participant(p.Name, p.Surname);

                foreach (var m in p.Marks)
                    curP.Evaluate(m);
                
                return curP;
            }).ToArray();

            Purple_3.Participant.SetPlaces(Participants);

            resultObj.Add(Participants);

            return resultObj;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName) {
            SelectFile(fileName);

            var curObj = GetFileData<Purple_4_GroupDTO>();

            var Name = curObj.Name;
    
            var resultObj = new Purple_4.Group(Name);
                
            var Sportsmen = curObj.Sportsmen.Select(s => {
                var curS = new Purple_4.Sportsman(s.Name, s.Surname);
                curS.Run(s.Time);
                
                return curS;
            }).ToArray();

            resultObj.Add(Sportsmen); 
            
            return resultObj;
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName) {
            SelectFile(fileName);

            var objDict = GetFileData<Purple_5_ReportDTO>();

            var resultObj = new Purple_5.Report();

            var Researches = objDict.Researches.Select(rsrch => {
                var curRsrch = new Purple_5.Research(rsrch.Name);

                foreach (var rsp in rsrch.Responses)
                    curRsrch.Add(new string[] {rsp.Animal, rsp.CharacterTrait, rsp.Concept});

                return curRsrch;
            }).ToArray();

            resultObj.AddResearch(Researches);  

            return resultObj;
        }
        
    }
}