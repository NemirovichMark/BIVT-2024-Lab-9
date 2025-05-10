using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_9;
using Lab_7;
using Newtonsoft.Json;

namespace Lab_9 {
    public class WhiteJSONSerializer : WhiteSerializer {
        public override string Extension => "json";

        private class White1ParticipantDTO {
            public string Type { get; set; }
            public string Surname { get; set; }
            public string Club { get; set; }
            public double FirstJump { get; set; }
            public double SecondJump { get; set; }
        }

        public override void SerializeWhite1Participant(White_1.Participant participant, string fileName) {
            var dto = new White1ParticipantDTO {
                Type = participant.GetType().Name,
                Surname = participant.Surname,
                Club = participant.Club,
                FirstJump = participant.FirstJump,
                SecondJump = participant.SecondJump
            };
            SelectFile(fileName);
            string json = JsonConvert.SerializeObject(dto);
            File.WriteAllText(FilePath, json);
        }

        private class White2ParticipantDTO {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public double FirstJump { get; set; }
            public double SecondJump { get; set; }
        }

        public override void SerializeWhite2Participant(White_2.Participant participant, string fileName) {
            var dto = new White2ParticipantDTO{
                Type = participant.GetType().Name,
                Name = participant.Name,
                Surname = participant.Surname,
                FirstJump = participant.FirstJump,
                SecondJump = participant.SecondJump
            };
            SelectFile(fileName);
            string json = JsonConvert.SerializeObject(dto);
            File.WriteAllText(FilePath, json);
        }

        private class White3StudentDTO {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Marks { get; set; }
            public int Skipped { get; set; }
        }

        public override void SerializeWhite3Student(White_3.Student student, string fileName) {
            var dto = new White3StudentDTO{
                Type = student.GetType().Name,
                Name = student.Name,
                Surname = student.Surname,
                Marks = student.Marks,
                Skipped = student.Skipped,
            };
            SelectFile(fileName);
            string json = JsonConvert.SerializeObject(dto);
            File.WriteAllText(FilePath, json);
        }

        private class White4HumanDTO {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public double[] Scores { get; set; }
        }

        public override void SerializeWhite4Human(White_4.Human human, string fileName) {
            var dto = new White4HumanDTO{
                Type = human.GetType().Name,
                Name = human.Name,
                Surname = human.Surname,
                Scores = (human is White_4.Participant participant) ? participant.Scores : null
            };
            SelectFile(fileName);
            string json = JsonConvert.SerializeObject(dto);
            File.WriteAllText(FilePath, json);
        }

        private class White5TeamDTO {
            public string Type { get; set; }
            public string Name { get; set; }
            public White5MatchDTO[] Matches { get; set; }
        }

        private class White5MatchDTO {
            public int Goals { get; set; }
            public int Misses { get; set; }
        }

        public override void SerializeWhite5Team(White_5.Team team, string fileName) {
            var dto_matches = new White5MatchDTO[team.Matches.Length];
            for (int i = 0; i < team.Matches.Length; i++) {
                dto_matches[i] = new White5MatchDTO {
                    Goals = team.Matches[i].Goals,
                    Misses = team.Matches[i].Misses
                };
            };
            var dto_team = new White5TeamDTO {
                Type = team.GetType().Name,
                Name = team.Name,
                Matches = dto_matches,
            };
            SelectFile(fileName);
            string json = JsonConvert.SerializeObject(dto_team);
            File.WriteAllText(FilePath, json);
        }

        public override White_1.Participant DeserializeWhite1Participant(string fileName) {
            SelectFile(fileName);
            var json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<White1ParticipantDTO>(json);

            var deserialized = new White_1.Participant(dto.Surname, dto.Club);
            deserialized.Jump(dto.FirstJump);
            deserialized.Jump(dto.SecondJump);
            return deserialized;
        }

        public override White_2.Participant DeserializeWhite2Participant(string fileName) {
            SelectFile(fileName);
            var json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<White2ParticipantDTO>(json);
            
            var deserialized = new White_2.Participant(dto.Name, dto.Surname, dto.FirstJump, dto.SecondJump);
            return deserialized;
        }

        public override White_3.Student DeserializeWhite3Student(string fileName) {
            SelectFile(fileName);
            var json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<White3StudentDTO>(json);

            White_3.Student deserialized;
            if (dto.Type == "Undergraduate") {
                deserialized = new White_3.Undergraduate(dto.Name, dto.Surname);
            } else {
                deserialized = new White_3.Student(dto.Name, dto.Surname);
            }
            for (int i = 0; i < dto.Skipped; ++i) {
                deserialized.Lesson(0);
            }
            foreach (var mark in dto.Marks) {
                deserialized.Lesson(mark);
            }
            return deserialized;
        }

        public override White_4.Human DeserializeWhite4Human(string fileName) {
            SelectFile(fileName);
            var json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<White4HumanDTO>(json);

            White_4.Human deserialized;
            if (dto.Type == "Participant") {
                deserialized = new White_4.Participant(dto.Name, dto.Surname);
            } else {
                deserialized = new White_4.Human(dto.Name, dto.Surname);
            }
            if (dto.Scores != null) {
                foreach (var score in dto.Scores) {
                    ((White_4.Participant)deserialized).PlayMatch(score);
                }
            }

            return deserialized;
        }

        public override White_5.Team DeserializeWhite5Team(string fileName) {
            SelectFile(fileName);
            var json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<White5TeamDTO>(json);
            White_5.Team deserialized;
            if (dto.Type == "ManTeam") {
                deserialized = new White_5.ManTeam(dto.Name);
                foreach (var match in dto.Matches) {
                    ((White_5.ManTeam)deserialized).PlayMatch(match.Goals, match.Misses);
                }
            } else {
                deserialized = new White_5.WomanTeam(dto.Name);
                foreach (var match in dto.Matches) {
                    ((White_5.WomanTeam)deserialized).PlayMatch(match.Goals, match.Misses);
                }
            }
            return deserialized;
        }
    }
}