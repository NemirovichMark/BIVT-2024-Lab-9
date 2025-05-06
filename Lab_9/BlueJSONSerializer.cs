using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Lab_7;
using System.Diagnostics;
using System.Dynamic;
using System.Security.Cryptography;

namespace Lab_9 {
    public class BlueJSONSerializer : BlueSerializer {
        public override string Extension => "json";

        private class ResponseDTO
        {
            public string Type { get; set; } 
            public string Name { get; set; }
            public int Votes { get; set; }
            public string? Surname { get; set; } 

            public ResponseDTO() { }

            public ResponseDTO(Blue_1.Response response) {
                Type = response.GetType().Name;
                Name = response.Name;
                Votes = response.Votes;
                if (response is Blue_1.HumanResponse r)
                    Surname = r.Surname;
            }
        }

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) {return;}
            SelectFile(fileName);
            var participantDTO = new ResponseDTO(participant);
            var json = JsonConvert.SerializeObject(participantDTO, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        private static int[][] MatrixToJaggedArr(int[,] matrix) {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int[][] jagged = new int[rows][];
            for (int i = 0; i < rows; i++) {
                jagged[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                    jagged[i][j] = matrix[i, j];
            }
            return jagged;
        }

        private static int[,] JaggedArrToMatrix(int[][] arr) {
            int rows = arr.Length;
            int cols = arr[0].Length;
            int[,] matrix = new int[rows, cols];
            for (int i = 0; i < rows; i++){
                for (int j = 0; j < cols; j++){
                    matrix[i, j] = arr[i][j];
                }
            }

            return matrix;
        }

        private class WaterJumpDTO {

            public string Type {get; set;}

            public string Name {get; set;}

            public int Bank {get; set;}

            public ParticipantB2DTO[] Participants {get; set;}

            public WaterJumpDTO() {}

            public WaterJumpDTO(Blue_2.WaterJump wj) {
                Type = wj.GetType().Name;
                Name = wj.Name;
                Bank = wj.Bank;
                Participants = wj.Participants.Select(p => new ParticipantB2DTO(p)).ToArray();
            }
        }

        private class ParticipantB2DTO {
            public string Name {get; set;}
            public string Surname {get; set;}

            public int[][] Marks {get; set;}

            public ParticipantB2DTO() {}

            public ParticipantB2DTO(Blue_2.Participant participant) {
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = MatrixToJaggedArr(participant.Marks);
            }

        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) {return;}
            SelectFile(fileName);
            var wj_dto = new WaterJumpDTO(participant);
            var json = JsonConvert.SerializeObject(wj_dto);
            File.WriteAllText(FilePath, json);
        }

        private class ParticipantB3DTO {
            public string Type {get; set;}
            public string Name {get; set;}
            public string Surname {get; set;}
            public int[] Penalties {get; set;}

            public ParticipantB3DTO() {}

            public ParticipantB3DTO(Blue_3.Participant p) {
                Type = p.GetType().Name;
                Name = p.Name;
                Surname = p.Surname;
                Penalties = p.Penalties;
            }
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName) where T: class
        {
            if (student == null || String.IsNullOrEmpty(fileName)) {return;}
            SelectFile(fileName);
            var dto = new ParticipantB3DTO(student);
            var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        private class TeamDTO {
            public string Type {get; set;}
            public string Name {get; set;}
            public int[] Scores {get; set;}
            public TeamDTO() {}
            public TeamDTO(Blue_4.Team t) {
                Type = t.GetType().Name;
                Name = t.Name;
                Scores = t.Scores;
            } 
        }

        private class GroupDTO {
            public string Name {get; set;}
            public TeamDTO[] ManTeams {get; set;}
            public TeamDTO[] WomanTeams {get; set;}
            public GroupDTO() {}
            public GroupDTO(Blue_4.Group g) {
                Name = g.Name;
                ManTeams = g.ManTeams.Select(t => t == null ? null : new TeamDTO(t)).ToArray();
                WomanTeams = g.WomanTeams.Select(t => t == null ? null : new TeamDTO(t)).ToArray();
            }
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) {return;}
            SelectFile(fileName);
            var dto = new GroupDTO(participant);
            var json = JsonConvert.SerializeObject(dto);
            File.WriteAllText(FilePath, json);
        }

        private class SportsmanDTO {
            public string Name {get; set;}
            public string Surname {get; set;}
            public int Place {get; set;}
            public SportsmanDTO() {}
            public SportsmanDTO(Blue_5.Sportsman s) {
                Name = s.Name;
                Surname = s.Surname;
                Place = s.Place;
            }
        }

        private class TeamB5DTO {
            public string Type {get; set;}
            public string Name {get; set;}
            public SportsmanDTO[] Sportsmen {get; set;}

            public TeamB5DTO() {}
            public TeamB5DTO(Blue_5.Team t) {
                Type = t.GetType().Name;
                Name = t.Name;
                Sportsmen = t.Sportsmen.Select(s => s != null ? new SportsmanDTO(s) : null).ToArray();
            }
        }

        public override void SerializeBlue5Team<T>(T group, string fileName) where T: class
        {
            if (group == null || String.IsNullOrEmpty(fileName)) {return;}
            SelectFile(fileName);
            object dto = null;
            if (group.GetType().Name == nameof(Blue_5.ManTeam) || group.GetType().Name == nameof(Blue_5.WomanTeam))
            {
                dto = new TeamB5DTO(group);
            }
            else if (group is Blue_5.Sportsman sportsman){
                dto = new SportsmanDTO(sportsman);
            }
            string json = JsonConvert.SerializeObject(dto);
            File.WriteAllText(FilePath, json);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            Blue_1.Response deserialize1d = default(Blue_1.Response);
            string json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<ResponseDTO>(json);
            if (dto.Type == "HumanResponse") {
                deserialize1d = new Blue_1.HumanResponse(dto.Name, dto.Surname, dto.Votes);
            }
            else {
                deserialize1d = new Blue_1.Response(dto.Name, dto.Votes);
            }
            return deserialize1d;
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            Blue_2.WaterJump wj = default(Blue_2.WaterJump);
            string json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<WaterJumpDTO>(json);
            if (dto.Type == "WaterJump5m")
                wj = new Blue_2.WaterJump5m(dto.Name, dto.Bank);
            else
                wj = new Blue_2.WaterJump3m(dto.Name, dto.Bank);

            for (int k = 0; k < dto.Participants.Length; k++) {
                Blue_2.Participant p = new Blue_2.Participant(dto.Participants[k].Name, dto.Participants[k].Surname);
                int [,] marks = JaggedArrToMatrix(dto.Participants[k].Marks);
                for (int i = 0; i < 2; i++){
                    int[] res = new int[5];
                    for (int j = 0; j < 5; j++) {
                        res[j] = marks[i, j];
                    }
                    p.Jump(res);
                }
                wj.Add(p);
            }
            return wj;
        }

        public override T DeserializeBlue3Participant<T>(string fileName) where T: class
        {
            SelectFile(fileName);
            Blue_3.Participant deserialized = default(Blue_3.Participant);
            string json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<ParticipantB3DTO>(json);
            if (dto.Type == "Participant")
                deserialized = new Blue_3.Participant(dto.Name, dto.Surname);
            else if (dto.Type == "HockeyPlayer")
                deserialized = new Blue_3.HockeyPlayer(dto.Name, dto.Surname);
            else
                deserialized = new Blue_3.BasketballPlayer(dto.Name, dto.Surname);
            foreach (int p in dto.Penalties)
                deserialized.PlayMatch(p);
            return (T)deserialized;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);
            Blue_4.Group deserialized = default(Blue_4.Group);
            string json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<GroupDTO>(json);
            deserialized = new Blue_4.Group(dto.Name);
            foreach (var dto_team in dto.ManTeams) {
                if (dto_team == null) {
                    deserialized.Add(default(Blue_4.ManTeam));
                    continue;
                }
                Blue_4.ManTeam mt = default(Blue_4.ManTeam);
                if (dto_team.Type == "ManTeam") {
                    mt = new Blue_4.ManTeam(dto_team.Name);
                    foreach (int s in dto_team.Scores) {
                        mt.PlayMatch(s);
                    }
                }
                deserialized.Add(mt);
            }
            foreach (var dto_team in dto.WomanTeams) {
                if (dto_team == null) {
                    deserialized.Add(default(Blue_4.WomanTeam));
                    continue;
                }
                Blue_4.WomanTeam wt = default(Blue_4.WomanTeam);
                if (dto_team.Type == "WomanTeam") {
                    wt = new Blue_4.WomanTeam(dto_team.Name);
                    foreach (int s in dto_team.Scores) {
                        wt.PlayMatch(s);
                    }
                }
                deserialized.Add(wt);
            }
            return deserialized;
        }

        private Blue_5.Sportsman SportsmanDTO_to_Simple(SportsmanDTO s) {
            Blue_5.Sportsman sp = new Blue_5.Sportsman(s.Name, s.Surname);
            sp.SetPlace(s.Place);
            return sp;
        }

        public override T DeserializeBlue5Team<T>(string fileName) where T: class
        {
            SelectFile(fileName);
            string json = File.ReadAllText(FilePath);
            if (typeof(T) == typeof(Blue_5.Sportsman)) {
                SportsmanDTO dto = JsonConvert.DeserializeObject<SportsmanDTO>(json);
                Blue_5.Sportsman deserialized = new Blue_5.Sportsman(dto.Name, dto.Surname);
                deserialized.SetPlace(dto.Place);
                return deserialized as T;
            }
            else {
                TeamB5DTO dto = JsonConvert.DeserializeObject<TeamB5DTO>(json);
                if (dto.Type == "WomanTeam")
                {
                    Blue_5.WomanTeam deserialized = new Blue_5.WomanTeam(dto.Name);
                    foreach (SportsmanDTO s in dto.Sportsmen) {
                        if (s == null) continue;
                        deserialized.Add(SportsmanDTO_to_Simple(s));
                    }
                    return deserialized as T;
                }
                else {
                    Blue_5.ManTeam deserialized = new Blue_5.ManTeam(dto.Name);
                    foreach (SportsmanDTO s in dto.Sportsmen) {
                        if (s == null) continue;
                        deserialized.Add(SportsmanDTO_to_Simple(s));
                    }
                    return deserialized as T;
                }
            }
        }
    }
}