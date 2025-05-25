using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Lab_7;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override string Extension => "json";

        #region Вспомогательные классы для сериализации в JSON

        
        public class ResponseDTO
        {
            public string Type { get; set; } = "Response";
            public string Name { get; set; } = string.Empty;
            public int Votes { get; set; }

            public ResponseDTO() { }

            public ResponseDTO(Blue_1.Response response)
            {
                Name = response.Name;
                Votes = response.Votes;
            }
        }

       
        public class HumanResponseDTO : ResponseDTO
        {
            public string Surname { get; set; } = string.Empty;

            public HumanResponseDTO()
            {
                Type = "HumanResponse";
            }

            public HumanResponseDTO(Blue_1.HumanResponse response) : base(response)
            {
                Type = "HumanResponse";
                Surname = response.Surname;
            }
        }

        
        public class ParticipantDTO
        {
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public int[,] Marks { get; set; }
            public int FilledJumps { get; set; }

            public ParticipantDTO() { }

            public ParticipantDTO(Blue_2.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = participant.Marks;
                FilledJumps = participant.Marks?.GetLength(0) ?? 0;
            }
        }

       
        public class WaterJumpDTO
        {
            public string Name { get; set; } = string.Empty;
            public int Bank { get; set; }
            public List<ParticipantDTO> Participants { get; set; } = new List<ParticipantDTO>();
            public string Type { get; set; } = "WaterJump";

            public WaterJumpDTO() { }

            public WaterJumpDTO(Blue_2.WaterJump waterJump)
            {
                Name = waterJump.Name;
                Bank = waterJump.Bank;
                if (waterJump.Participants != null)
                {
                    foreach (var participant in waterJump.Participants)
                    {
                        Participants.Add(new ParticipantDTO(participant));
                    }
                }
                Type = waterJump.GetType().Name;
            }
        }

        
        public class Participant3DTO
        {
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public int[] Penalties { get; set; } = Array.Empty<int>();
            public string Type { get; set; } = "Participant";

            public Participant3DTO() { }

            public Participant3DTO(Blue_3.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Penalties = participant.Penalties;
                Type = participant.GetType().Name;
            }
        }

        
        public class GroupDTO
        {
            public string Name { get; set; } = string.Empty;
            public List<TeamDTO> ManTeams { get; set; } = new List<TeamDTO>();
            public List<TeamDTO> WomanTeams { get; set; } = new List<TeamDTO>();

            public GroupDTO() { }

            public GroupDTO(Blue_4.Group group)
            {
                Name = group.Name;
                if (group.ManTeams != null)
                {
                    foreach (var team in group.ManTeams)
                    {
                        if (team != null)
                            ManTeams.Add(new TeamDTO(team));
                    }
                }
                if (group.WomanTeams != null)
                {
                    foreach (var team in group.WomanTeams)
                    {
                        if (team != null)
                            WomanTeams.Add(new TeamDTO(team));
                    }
                }
            }
        }

        
        public class TeamDTO
        {
            public string Name { get; set; } = string.Empty;
            public int[] Scores { get; set; } = Array.Empty<int>();
            public string Type { get; set; } = "Team";

            public TeamDTO() { }

            public TeamDTO(Blue_4.Team team)
            {
                Name = team.Name;
                Scores = team.Scores;
                Type = team.GetType().Name;
            }
        }

        
        public class Team5DTO
        {
            public string Name { get; set; } = string.Empty;
            public List<SportsmanDTO> Sportsmen { get; set; } = new List<SportsmanDTO>();
            public string Type { get; set; } = "Team";

            public Team5DTO() { }

            public Team5DTO(Blue_5.Team team)
            {
                Name = team.Name;
                if (team.Sportsmen != null)
                {
                    foreach (var sportsman in team.Sportsmen)
                    {
                        if (sportsman != null)
                            Sportsmen.Add(new SportsmanDTO(sportsman));
                    }
                }
                Type = team.GetType().Name;
            }
        }

        
        public class SportsmanDTO
        {
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public int Place { get; set; }

            public SportsmanDTO() { }

            public SportsmanDTO(Blue_5.Sportsman sportsman)
            {
                Name = sportsman.Name;
                Surname = sportsman.Surname;
                Place = sportsman.Place;
            }
        }

        #endregion

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName))
            {
                return;
            }

            SelectFile(fileName);

            if (participant is Blue_1.HumanResponse humanResponse)
            {
                var dto = new HumanResponseDTO(humanResponse);
                string json = JsonConvert.SerializeObject(dto, Formatting.Indented);
                File.WriteAllText(FilePath, json);
            }
            else
            {
                var dto = new ResponseDTO(participant);
                string json = JsonConvert.SerializeObject(dto, Formatting.Indented);
                File.WriteAllText(FilePath, json);
            }
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            SelectFile(fileName);
            Blue_1.Response response = null;
            int votes = 0;

            try
            {
                string jsonString = File.ReadAllText(FilePath);
                JObject jObject = JObject.Parse(jsonString);

                string typeFieldValue = jObject["Type"]?.Value<string>();

                if (typeFieldValue == "HumanResponse")
                {
                    var dto = jObject.ToObject<HumanResponseDTO>();
                    if (dto != null)
                    {
                        response = new Blue_1.HumanResponse(dto.Name, dto.Surname);
                        votes = dto.Votes;
                    }
                }
                else 
                {
                    var dto = jObject.ToObject<ResponseDTO>();
                    if (dto != null)
                    {
                        response = new Blue_1.Response(dto.Name);
                        votes = dto.Votes;
                    }
                }

                if (response != null)
                {
                    var field = typeof(Blue_1.Response).GetField("_votes", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (field != null)
                    { 
                        field.SetValue(response, votes);
                    }
                }
                return response;
            }
            catch
            {
                return null;
            }
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName))
            {
                return;
            }

            SelectFile(fileName);

            var dto = new WaterJumpDTO(participant);
            string json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            SelectFile(fileName);

            try
            {
                string json = File.ReadAllText(FilePath);
                var dto = JsonConvert.DeserializeObject<WaterJumpDTO>(json);
                
                Blue_2.WaterJump waterJump;
                if (dto.Type == "WaterJump3m")
                {
                    waterJump = new Blue_2.WaterJump3m(dto.Name, dto.Bank);
                }
                else if (dto.Type == "WaterJump5m")
                {
                    waterJump = new Blue_2.WaterJump5m(dto.Name, dto.Bank);
                }
                else
                {
                    return null;
                }

                foreach (var participantDTO in dto.Participants)
                {
                    var participant = new Blue_2.Participant(participantDTO.Name, participantDTO.Surname);
                    if (participantDTO.Marks != null)
                    {
                        for (int i = 0; i < participantDTO.FilledJumps; i++)
                        {
                            int[] jumpMarks = new int[5];
                            for (int j = 0; j < 5; j++)
                            {
                                jumpMarks[j] = participantDTO.Marks[i, j];
                            }
                            participant.Jump(jumpMarks);
                        }
                    }
                    waterJump.Add(participant);
                }

                return waterJump;
            }
            catch
            {
                return null;
            }
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || string.IsNullOrEmpty(fileName))
            {
                return;
            }

            SelectFile(fileName);

            var dto = new Participant3DTO(student as Blue_3.Participant);
            string json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return default;
            }

            SelectFile(fileName);

            try
            {
                string json = File.ReadAllText(FilePath);
                var dto = JsonConvert.DeserializeObject<Participant3DTO>(json);
                
                Blue_3.Participant participant;
                if (dto.Type == "BasketballPlayer")
                {
                    participant = new Blue_3.BasketballPlayer(dto.Name, dto.Surname);
                }
                else if (dto.Type == "HockeyPlayer")
                {
                    participant = new Blue_3.HockeyPlayer(dto.Name, dto.Surname);
                }
                else
                {
                    participant = new Blue_3.Participant(dto.Name, dto.Surname);
                }

                foreach (var penalty in dto.Penalties)
                {
                    participant.PlayMatch(penalty);
                }

                return (T)participant;
            }
            catch
            {
                return default;
            }
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName))
            {
                return;
            }

            SelectFile(fileName);

            var dto = new GroupDTO(participant);
            string json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            SelectFile(fileName);

            try
            {
                string json = File.ReadAllText(FilePath);
                var dto = JsonConvert.DeserializeObject<GroupDTO>(json);
                var group = new Blue_4.Group(dto.Name);

                foreach (var teamDTO in dto.ManTeams)
                {
                    var team = new Blue_4.ManTeam(teamDTO.Name);
                    foreach (var score in teamDTO.Scores)
                    {
                        team.PlayMatch(score);
                    }
                    group.Add(team);
                }

                foreach (var teamDTO in dto.WomanTeams)
                {
                    var team = new Blue_4.WomanTeam(teamDTO.Name);
                    foreach (var score in teamDTO.Scores)
                    {
                        team.PlayMatch(score);
                    }
                    group.Add(team);
                }

                return group;
            }
            catch
            {
                return null;
            }
        }

        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName))
            {
                return;
            }

            SelectFile(fileName);

            var dto = new Team5DTO(group as Blue_5.Team);
            string json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return default;
            }

            SelectFile(fileName);

            try
            {
                string json = File.ReadAllText(FilePath);
                var dto = JsonConvert.DeserializeObject<Team5DTO>(json);
                
                Blue_5.Team team;
                if (dto.Type == "ManTeam")
                {
                    team = new Blue_5.ManTeam(dto.Name);
                }
                else if (dto.Type == "WomanTeam")
                {
                    team = new Blue_5.WomanTeam(dto.Name);
                }
                else
                {
                    return default;
                }

                foreach (var sportsmanDTO in dto.Sportsmen)
                {
                    var sportsman = new Blue_5.Sportsman(sportsmanDTO.Name, sportsmanDTO.Surname);
                    sportsman.SetPlace(sportsmanDTO.Place);
                    team.Add(sportsman);
                }

                return (T)team;
            }
            catch
            {
                return default;
            }
        }
    }
}