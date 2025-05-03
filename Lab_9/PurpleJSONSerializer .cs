using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Lab_9
{
    public class PurpleJSONSerializer: PurpleSerializer
    {
        public override string Extension => "json";

        private T[][] ConvertMatrixTo2D<T>(T[,] mat) {
            int rows = mat.GetLength(0), cols = mat.GetLength(1);
            T[][] ans = new T[rows][];
            for (int i = 0; i < rows; i++) {
                ans[i] = new T[cols];
                for (int j = 0; j < cols; j++) {
                    ans[i][j] = mat[i, j];
                }
            }
            return ans;
        } 

        private class Purple1ParticipantDTO {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Coefs { get; set; }
            public int[][] Marks { get; set; }
        }

        private class Purple1JudgeDTO {
            public string Name { get; set; }
            public int[] Marks { get; set; }
        }

        private class Purple1CompetitionDTO {
            public Purple1JudgeDTO[] Judges { get; set; }
            public Purple1ParticipantDTO[] Participants { get; set; }
        }

        private void writePurple1Participant(StreamWriter writer, Purple_1.Participant participant)
        {
            Purple1ParticipantDTO partDTO = new Purple1ParticipantDTO{
                Name = participant.Name,
                Surname = participant.Surname,
                Coefs = participant.Coefs,
                Marks = ConvertMatrixTo2D<int>(participant.Marks)
            };
            string jsonPart = JsonConvert.SerializeObject(partDTO, Formatting.Indented);
            writer.Write(jsonPart);
        }
        private void writePurple1Judge(StreamWriter writer, Purple_1.Judge judge) {
            Purple1JudgeDTO judgeDTO = new Purple1JudgeDTO{
                Name = judge.Name,
                Marks = judge.Marks
            };
            string jsonJudge = JsonConvert.SerializeObject(judgeDTO, Formatting.Indented);
            writer.Write(jsonJudge);
        }

        private void writePurple1Competition(StreamWriter writer, Purple_1.Competition comp)
        {
            Purple1CompetitionDTO compDTO = new Purple1CompetitionDTO{
                Judges = comp.Judges.Select(j => new Purple1JudgeDTO{
                    Name = j.Name,
                    Marks = j.Marks
                }).ToArray(),
                Participants = comp.Participants.Select(p => new Purple1ParticipantDTO{
                    Name = p.Name,
                    Surname = p.Surname,
                    Coefs = p.Coefs,
                    Marks = ConvertMatrixTo2D<int>(p.Marks)
                }).ToArray(),
            };
            string jsonComp = JsonConvert.SerializeObject(compDTO, Formatting.Indented);
            writer.Write(jsonComp);
        }
        public override void SerializePurple1<T>(T obj, string fileName) where T : class
        {
            if (obj == null)
                return;
            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                if (obj.GetType().Name == nameof(Purple_1.Participant))
                {
                    Purple_1.Participant participant = obj as Purple_1.Participant;
                    writePurple1Participant(writer, participant);
                }
                else if (obj.GetType().Name == nameof(Purple_1.Judge))
                {
                    Purple_1.Judge judge = obj as Purple_1.Judge;
                    writePurple1Judge(writer, judge);
                }
                else if (obj.GetType().Name == nameof(Purple_1.Competition))
                {
                    Purple_1.Competition competition = obj as Purple_1.Competition;
                    writePurple1Competition(writer, competition);
                }
            }
        }
        
        private class Purple2ParticipantDTO {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Distance { get; set; }
            public int[] Marks { get; set; }
        }
        
        private class SkiJumpingDto {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Standard { get; set; }
            public Purple2ParticipantDTO[] Participants { get; set; }
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            if (jumping == null) return;

            SelectFile(fileName);


            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                
                SkiJumpingDto skiDto = new SkiJumpingDto{
                    Type = jumping.GetType().Name,
                    Name = jumping.Name,
                    Standard = jumping.Standard,
                    Participants = jumping.Participants.Select(p => new Purple2ParticipantDTO{
                        Name = p.Name,
                        Surname = p.Surname,
                        Distance = p.Distance,
                        Marks = p.Marks
                    }).ToArray()
                };

                string skiJson = JsonConvert.SerializeObject(skiDto, Formatting.Indented);

                writer.Write(skiJson);
            }
        }

        private class Purple3ParticipantDTO {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Marks { get; set; }
        }

        private class SkatingDto {
            public string Type { get; set; }
            public double[] Moods { get; set; }
            public Purple3ParticipantDTO[] Participants { get; set; }
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            if (skating == null) return;

            SelectFile(fileName);


            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                
                SkatingDto skateDto = new SkatingDto{
                    Type = skating.GetType().Name,
                    Moods = skating.Moods,
                    Participants = skating.Participants.Select(p => new Purple3ParticipantDTO{
                        Name = p.Name,
                        Surname = p.Surname,
                        Marks = p.Marks
                    }).ToArray()
                };

                string skateJson = JsonConvert.SerializeObject(skateDto, Formatting.Indented);

                writer.Write(skateJson);
            }
        }
        
        private class SportsmanDTO {
            public string Name { get; set; }
            public string Surname { get; set; }
            public double Time { get; set; }
        }

        private class GroupDto {
            public string Name { get; set; }
            public SportsmanDTO[] Sportsmen { get; set; }
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            if (group == null) return;

            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                
                GroupDto groupDto = new GroupDto{
                    Name  = group.Name,
                    Sportsmen = group.Sportsmen.Select(s => new SportsmanDTO{
                        Name = s.Name,
                        Surname = s.Surname,
                        Time = s.Time
                    }).ToArray()
                };

                string groupJson = JsonConvert.SerializeObject(groupDto, Formatting.Indented);

                writer.Write(groupJson);
            }

        }
        
        private class ResponseDTO {
            public string Animal { get; set; }
            public string CharacterTrait { get; set; }
            public string Concept { get; set; }
        }
        
        private class ResearchDTO {
            public string Name { get; set; }
            public ResponseDTO[] Responses { get; set; }
        }

        private class ReportDTO {
            public ResearchDTO[] Researches { get; set; }
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            if (report == null) return;

            SelectFile(fileName);

            using (StreamWriter writer = File.AppendText(FilePath))
            {
                ReportDTO reportDTO = new ReportDTO {
                    Researches = report.Researches.Select(rsch => new ResearchDTO{
                        Name = rsch.Name,
                        Responses = rsch.Responses.Select(rsp => new ResponseDTO {
                            Animal = rsp.Animal,
                            CharacterTrait = rsp.CharacterTrait,
                            Concept = rsp.Concept
                        }).ToArray()
                    }).ToArray()
                };

                string reportJson = JsonConvert.SerializeObject(reportDTO, Formatting.Indented);

                writer.Write(reportJson);

            }
        }
        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {   
            if (fileName == null) {
                return null;
            }

            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string jsonDTO = reader.ReadToEnd();
                SkiJumpingDto skiDto = JsonConvert.DeserializeObject<SkiJumpingDto>(jsonDTO);
                Purple_2.Participant[] participants = new Purple_2.Participant[skiDto.Participants.Length];
                int iter = 0;
                foreach (Purple2ParticipantDTO partDto in skiDto.Participants) {
                    participants[iter] = new Purple_2.Participant(partDto.Name, partDto.Surname);
                    participants[iter].Jump(partDto.Distance, partDto.Marks, skiDto.Standard);
                    iter++;
                }
                if (skiDto.Type == nameof(Purple_2.ProSkiJumping)) {
                    Purple_2.ProSkiJumping proSkiJumping = new Purple_2.ProSkiJumping();
                    proSkiJumping.Add(participants);
                    return proSkiJumping as T;
                } else {
                    Purple_2.JuniorSkiJumping juniorSkiJumping = new Purple_2.JuniorSkiJumping();
                    juniorSkiJumping.Add(participants);
                    return juniorSkiJumping as T;
                }
            }
        }
        public override T DeserializePurple1<T>(string fileName)
        {
            if (fileName == null) {
                return null;
            }

            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string jsonDTO = reader.ReadToEnd();
                if (typeof(T) == typeof(Purple_1.Participant)) {
                    Purple1ParticipantDTO partDto = JsonConvert.DeserializeObject<Purple1ParticipantDTO>(jsonDTO);

                    Purple_1.Participant participant = new Purple_1.Participant(partDto.Name, partDto.Surname); 

                    participant.SetCriterias(partDto.Coefs);

                    foreach (int[] marks in partDto.Marks)
                        participant.Jump(marks);

                    return participant as T;
                } else if (typeof(T) == typeof(Purple_1.Judge)) {
                    Purple1JudgeDTO judgeDto = JsonConvert.DeserializeObject<Purple1JudgeDTO>(jsonDTO);

                    Purple_1.Judge judge = new Purple_1.Judge(judgeDto.Name, judgeDto.Marks); 

                    return judge as T;
                } else {
                    Purple1CompetitionDTO compDTO = JsonConvert.DeserializeObject<Purple1CompetitionDTO>(jsonDTO);

                    Purple_1.Judge[] judges = compDTO.Judges.Select(j => new Purple_1.Judge(j.Name, j.Marks)).ToArray();
                    Purple_1.Competition competition = new Purple_1.Competition(judges); 

                    foreach(Purple1ParticipantDTO partDto in compDTO.Participants) {
                        Purple_1.Participant participant = new Purple_1.Participant(partDto.Name, partDto.Surname); 

                        participant.SetCriterias(partDto.Coefs);

                        foreach (int[] marks in partDto.Marks) {
                            participant.Jump(marks);
                        }
                        competition.Add(participant);
                    }
                    
                    return competition as T;
                }
            }
        }
        public override T DeserializePurple3Skating<T>(string fileName)
        {   
            if (fileName == null) {
                return null;
            }

            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string jsonDTO = reader.ReadToEnd();
                SkatingDto skateDto = JsonConvert.DeserializeObject<SkatingDto>(jsonDTO);
                Purple_3.Participant[] participants = new Purple_3.Participant[skateDto.Participants.Length];
                int iter = 0;
                foreach (Purple3ParticipantDTO partDto in skateDto.Participants) {
                    participants[iter] = new Purple_3.Participant(partDto.Name, partDto.Surname);
                    foreach (double mark in partDto.Marks)
                        participants[iter].Evaluate(mark);
                    iter++;
                }
                double[] moods = skateDto.Moods;
                if (skateDto.Type == nameof(Purple_3.IceSkating)) {
                    for (int i = 0; i < moods.Length; i++) {
                        moods[i] /= (1 + (i + 1) / 100.0);
                    }

                    Purple_3.IceSkating iceSkating = new Purple_3.IceSkating(moods);
                    iceSkating.Add(participants);
                    return iceSkating as T;
                } else {
                    for (int i = 0; i < moods.Length; i++) {
                        moods[i] -= ((i + 1) / 10.0);
                    }
                    
                    Purple_3.FigureSkating figureSkating = new Purple_3.FigureSkating(moods);
                    figureSkating.Add(participants);
                    return figureSkating as T;
                }
            }
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            if (fileName == null) {
                return null;
            }
            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string jsonDTO = reader.ReadToEnd();
                GroupDto groupDto = JsonConvert.DeserializeObject<GroupDto>(jsonDTO);
                if (groupDto == null)
                    return null;
                Purple_4.Sportsman[] sportsmen = new Purple_4.Sportsman[groupDto.Sportsmen.Length];
                int iter = 0;
                foreach (SportsmanDTO sportDto in groupDto.Sportsmen) {
                    sportsmen[iter] = new Purple_4.Sportsman(sportDto.Name, sportDto.Surname);
                    sportsmen[iter].Run(sportDto.Time);
                    iter++;
                }
                Purple_4.Group group = new Purple_4.Group(groupDto.Name);
                group.Add(sportsmen);
                return group;
            }
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            if (fileName == null) {
                return null;
            }
            SelectFile(fileName);

            using (StreamReader reader = new StreamReader(FilePath))
            {
                string jsonDTO = reader.ReadToEnd();
                ReportDTO reportDto = JsonConvert.DeserializeObject<ReportDTO>(jsonDTO);
                if (reportDto == null)
                    return null;
                Purple_5.Research[] researches = new Purple_5.Research[reportDto.Researches.Length];

                Purple_5.Report report = new Purple_5.Report();
                int iter = 0;
                foreach (ResearchDTO rschDto in reportDto.Researches) {
                    researches[iter] = new Purple_5.Research(rschDto.Name);
                    foreach(ResponseDTO rsp in rschDto.Responses) {
                        researches[iter].Add(new string[] {rsp.Animal, rsp.CharacterTrait, rsp.Concept});
                    }
                    iter++;
                }
                report.AddResearch(researches);
                return report;
            }
        }
    }
}

