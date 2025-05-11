using System;
using System.IO;
using System.Linq;
using System.Text;
using Lab_7;
using Newtonsoft.Json;


namespace Lab_9{
    public class BlueJSONSerializer : BlueSerializer{
        public override string Extension{
            get{
                return "json";
            }
        }
         private class ResponseDTO // для сохранения принципов ООП, чтобы не было доступа к private полям
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
        private static int[][] ChangeMatrix (int[,] matrix) { // Преобразует обычную матрицу (int[,]) в "рваный" массив (int[][]), где каждая строка — это отдельный массив.
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int[][] changed = new int[rows][];
            for (int i = 0; i < rows; i++) {
                changed[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                    changed[i][j] = matrix[i, j];
            }
            return changed;
        }

        private static int[,] MatrixToOriginal (int[][] arr) {
            int rows = arr.Length;
            int cols = arr[0].Length;
            int[,] original = new int[rows, cols];
            for (int i = 0; i < rows; i++){
                for (int j = 0; j < cols; j++){
                    original[i, j] = arr[i][j];
                }
            }

            return original;
        }
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            string path = Path.Combine(FolderPath, fileName);
            var participantDTO = new ResponseDTO(participant);
            var json = JsonConvert.SerializeObject(participantDTO, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        private class WaterJumpDTO {

            public string Type {get; set;}

            public string Name {get; set;}

            public int Bank {get; set;}

            public WaterJumpDTO() {}

            public ParticipantB2DTO[] Participants {get; set;}

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
                Marks = ChangeMatrix(participant.Marks);
            }

        }
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            var jumper_dto = new WaterJumpDTO(participant);
            var json = JsonConvert.SerializeObject(jumper_dto);
            string path = Path.Combine(FolderPath, fileName);
            File.WriteAllText(path, json);
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
            if (student == null || String.IsNullOrEmpty(fileName)) return;
            var dto = new ParticipantB3DTO(student);
            var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            string path = Path.Combine(FolderPath, fileName);
            File.WriteAllText(path, json);
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
            var obj = new GroupDTO(participant);
            var json = JsonConvert.SerializeObject(obj);
            string path = Path.Combine(FolderPath, fileName);
            File.WriteAllText(path, json);
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
            object objDTO = null;
            if (group.GetType().Name == nameof(Blue_5.ManTeam) || group.GetType().Name == nameof(Blue_5.WomanTeam))
            {
                objDTO = new TeamB5DTO(group);
            }
            else if (group is Blue_5.Sportsman sportsman){
                objDTO = new SportsmanDTO(sportsman);
            }
            string json = JsonConvert.SerializeObject(objDTO);
            string path = Path.Combine(FolderPath, fileName);
            File.WriteAllText(path, json);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            string path = Path.Combine(FolderPath, fileName);
            string json = File.ReadAllText(path);
            var obj = JsonConvert.DeserializeObject<ResponseDTO>(json);
            if (obj.Type == "HumanResponse") {
                return new Blue_1.HumanResponse(obj.Name, obj.Surname, obj.Votes);
            }
            else {
                return new Blue_1.Response(obj.Name, obj.Votes);
            }
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            string path = Path.Combine(FolderPath, fileName);
            Blue_2.WaterJump jumper = default(Blue_2.WaterJump);
            string json = File.ReadAllText(path);
            var obj = JsonConvert.DeserializeObject<WaterJumpDTO>(json);
            if (obj.Type == "WaterJump5m")
                jumper = new Blue_2.WaterJump5m(obj.Name, obj.Bank);
            else
                jumper = new Blue_2.WaterJump3m(obj.Name, obj.Bank);

            for (int k = 0; k < obj.Participants.Length; k++) {
                Blue_2.Participant p = new Blue_2.Participant(obj.Participants[k].Name, obj.Participants[k].Surname);
                int [,] marks = MatrixToOriginal(obj.Participants[k].Marks);
                for (int i = 0; i < 2; i++){
                    int[] res = new int[5];
                    for (int j = 0; j < 5; j++) {
                        res[j] = marks[i, j];
                    }
                    p.Jump(res);
                }
                jumper.Add(p);
            }
            return jumper;
        }
        public override T DeserializeBlue3Participant<T>(string fileName) where T: class
        {
            string path = Path.Combine(FolderPath, fileName);
            Blue_3.Participant deserializedParticipant = default(Blue_3.Participant);
            string json = File.ReadAllText(path);
            var obj = JsonConvert.DeserializeObject<ParticipantB3DTO>(json);
            if (obj.Type == "Participant")
                deserializedParticipant = new Blue_3.Participant(obj.Name, obj.Surname);
            else if (obj.Type == "HockeyPlayer")
                deserializedParticipant = new Blue_3.HockeyPlayer(obj.Name, obj.Surname);
            else
                deserializedParticipant = new Blue_3.BasketballPlayer(obj.Name, obj.Surname);
            foreach (int p in obj.Penalties)
                deserializedParticipant.PlayMatch(p);
            return (T)deserializedParticipant;
        }
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            string path = Path.Combine(FolderPath, fileName);
            string jsonContent = File.ReadAllText(path);
            var groupDto = JsonConvert.DeserializeObject<GroupDTO>(jsonContent);

            var group = new Blue_4.Group(groupDto.Name);

            foreach (var teamDto in groupDto.ManTeams)
            {
                if (teamDto == null)
                {
                    group.Add(null as Blue_4.ManTeam);
                    continue;
                }

                if (teamDto.Type == "ManTeam")
                {
                    var team = new Blue_4.ManTeam(teamDto.Name);
                    foreach (var score in teamDto.Scores)
                        team.PlayMatch(score);
                    group.Add(team);
                }
                else
                {
                    group.Add(null as Blue_4.ManTeam);
                }
            }

            foreach (var teamDto in groupDto.WomanTeams)
            {
                if (teamDto == null)
                {
                    group.Add(null as Blue_4.WomanTeam);
                    continue;
                }

                if (teamDto.Type == "WomanTeam")
                {
                    var team = new Blue_4.WomanTeam(teamDto.Name);
                    foreach (var score in teamDto.Scores)
                        team.PlayMatch(score);
                    group.Add(team);
                }
                else
                {
                    group.Add(null as Blue_4.WomanTeam);
                }
            }

            return group;
        }

        private Blue_5.Sportsman ConvertDtoToSportsman(SportsmanDTO dto)
        {
            var sportsman = new Blue_5.Sportsman(dto.Name, dto.Surname);
            sportsman.SetPlace(dto.Place);
            return sportsman;
        }
        public override T DeserializeBlue5Team<T>(string fileName) where T : class
        {
            string path = Path.Combine(FolderPath, fileName);
            string rawJson = File.ReadAllText(path);

            if (typeof(T) == typeof(Blue_5.Sportsman))
            {
                var sportsmanDto = JsonConvert.DeserializeObject<SportsmanDTO>(rawJson);
                var sportsman = new Blue_5.Sportsman(sportsmanDto.Name, sportsmanDto.Surname);
                sportsman.SetPlace(sportsmanDto.Place);
                return sportsman as T;
            }

            var teamDto = JsonConvert.DeserializeObject<TeamB5DTO>(rawJson);

            if (teamDto.Type == "WomanTeam")
            {
                var womanTeam = new Blue_5.WomanTeam(teamDto.Name);
                foreach (var athleteDto in teamDto.Sportsmen)
                {
                    if (athleteDto == null) continue;
                    womanTeam.Add(ConvertDtoToSportsman(athleteDto));
                }
                return womanTeam as T;
            }
            else
            {
                var manTeam = new Blue_5.ManTeam(teamDto.Name);
                foreach (var athleteDto in teamDto.Sportsmen)
                {
                    if (athleteDto == null) continue;
                    manTeam.Add(ConvertDtoToSportsman(athleteDto));
                }
                return manTeam as T;
            }
        }
    }
}