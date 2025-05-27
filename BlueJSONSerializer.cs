using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lab_7;
using Lab_7.Blue_1_Models;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override string Extension => "json";

        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            SelectFile(fileName);
            var dto = new ResponseDTO(participant);
            string json = JsonSerializer.Serialize(dto, _options);
            File.WriteAllText(fileName, json);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            string json = File.ReadAllText(fileName);
            var dto = JsonSerializer.Deserialize<ResponseDTO>(json, _options);
            return dto.ToResponse();
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            SelectFile(fileName);
            var dto = new WaterJumpDTO(participant);
            string json = JsonSerializer.Serialize(dto, _options);
            File.WriteAllText(fileName, json);
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            string json = File.ReadAllText(fileName);
            var dto = JsonSerializer.Deserialize<WaterJumpDTO>(json, _options);
            return dto.ToWaterJump();
        }

        public override void SerializeBlue3Participant<T>(T participant, string fileName)
        {
            SelectFile(fileName);
            var dto = new ParticipantDTO(participant);
            string json = JsonSerializer.Serialize(dto, _options);
            File.WriteAllText(fileName, json);
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            SelectFile(fileName);
            string json = File.ReadAllText(fileName);
            var dto = JsonSerializer.Deserialize<ParticipantDTO>(json, _options);
            return (T)dto.ToParticipant();
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            SelectFile(fileName);
            var dto = new GroupDTO(participant);
            string json = JsonSerializer.Serialize(dto, _options);
            File.WriteAllText(fileName, json);
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);
            string json = File.ReadAllText(fileName);
            var dto = JsonSerializer.Deserialize<GroupDTO>(json, _options);
            return dto.ToGroup();
        }

        public override void SerializeBlue5Team<T>(T team, string fileName)
        {
            SelectFile(fileName);
            var dto = new TeamDTO(team);
            string json = JsonSerializer.Serialize(dto, _options);
            File.WriteAllText(fileName, json);
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            SelectFile(fileName);
            string json = File.ReadAllText(fileName);
            var dto = JsonSerializer.Deserialize<TeamDTO>(json, _options);
            return (T)dto.ToTeam();
        }

        #region DTO Classes
        private class ResponseDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Votes { get; set; }
            public string Surname { get; set; }

            public ResponseDTO() { }

            public ResponseDTO(Blue_1.Response response)
            {
                Type = response.GetType().Name;
                Name = response.Name;
                Votes = response.Votes;

                if (response is Blue_1.HumanResponse humanResponse)
                {
                    Surname = humanResponse.Surname;
                }
            }

            public Blue_1.Response ToResponse()
            {
                if (Type == nameof(Blue_1.HumanResponse))
                {
                    return new Blue_1.HumanResponse(Name, Surname, Votes);
                }
                return new Blue_1.Response(Name, Votes);
            }
        }

        private class ParticipantDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Penalties { get; set; }

            public ParticipantDTO() { }

            public ParticipantDTO(Blue_3.Participant participant)
            {
                Type = participant.GetType().Name;
                Name = participant.Name;
                Surname = participant.Surname;
                Penalties = participant.Penalties;
            }

            public Blue_3.Participant ToParticipant()
            {
                Blue_3.Participant participant;

                switch (Type)
                {
                    case nameof(Blue_3.BasketballPlayer):
                        participant = new Blue_3.BasketballPlayer(Name, Surname);
                        break;
                    case nameof(Blue_3.HockeyPlayer):
                        participant = new Blue_3.HockeyPlayer(Name, Surname);
                        break;
                    default:
                        participant = new Blue_3.Participant(Name, Surname);
                        break;
                }

                foreach (var penalty in Penalties)
                {
                    participant.PlayMatch(penalty);
                }

                return participant;
            }
        }

        private class WaterJumpDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Bank { get; set; }
            public ParticipantJumpDTO[] Participants { get; set; }

            public WaterJumpDTO() { }

            public WaterJumpDTO(Blue_2.WaterJump waterJump)
            {
                Type = waterJump.GetType().Name;
                Name = waterJump.Name;
                Bank = waterJump.Bank;

                if (waterJump.Participants != null)
                {
                    Participants = waterJump.Participants
                        .Where(p => !p.Equals(default(Blue_2.Participant)))
                        .Select(p => new ParticipantJumpDTO(p))
                        .ToArray();
                }
            }

            public Blue_2.WaterJump ToWaterJump()
            {
                Blue_2.WaterJump waterJump = Type == nameof(Blue_2.WaterJump5m)
                    ? new Blue_2.WaterJump5m(Name, Bank)
                    : new Blue_2.WaterJump3m(Name, Bank);

                if (Participants != null)
                {
                    foreach (var participantDto in Participants)
                    {
                        var participant = participantDto.ToParticipant();
                        waterJump.Add(participant);
                    }
                }

                return waterJump;
            }
        }

        private class ParticipantJumpDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[][] Marks { get; set; }

            public ParticipantJumpDTO() { }

            public ParticipantJumpDTO(Blue_2.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;

                if (participant.Marks != null)
                {
                    Marks = new int[participant.Marks.GetLength(0)][];
                    for (int i = 0; i < participant.Marks.GetLength(0); i++)
                    {
                        Marks[i] = new int[participant.Marks.GetLength(1)]; for (int j = 0; j < participant.Marks.GetLength(1); j++)
                        {
                            Marks[i][j] = participant.Marks[i, j];
                        }
                    }
                }
            }

            public Blue_2.Participant ToParticipant()
            {
                var participant = new Blue_2.Participant(Name, Surname);

                if (Marks != null)
                {
                    foreach (var mark in Marks)
                    {
                        participant.Jump(mark);
                    }
                }

                return participant;
            }
        }

        private class GroupDTO
        {
            public string Name { get; set; }
            public TeamScoreDTO[] ManTeams { get; set; }
            public TeamScoreDTO[] WomanTeams { get; set; }

            public GroupDTO() { }

            public GroupDTO(Blue_4.Group group)
            {
                Name = group.Name;

                if (group.ManTeams != null)
                {
                    ManTeams = group.ManTeams
                        .Where(t => t != null)
                        .Select(t => new TeamScoreDTO(t))
                        .ToArray();
                }

                if (group.WomanTeams != null)
                {
                    WomanTeams = group.WomanTeams
                        .Where(t => t != null)
                        .Select(t => new TeamScoreDTO(t))
                        .ToArray();
                }
            }

            public Blue_4.Group ToGroup()
            {
                var group = new Blue_4.Group(Name);

                if (ManTeams != null)
                {
                    foreach (var teamDto in ManTeams)
                    {
                        var team = teamDto.ToTeam();
                        group.Add(team);
                    }
                }

                if (WomanTeams != null)
                {
                    foreach (var teamDto in WomanTeams)
                    {
                        var team = teamDto.ToTeam();
                        group.Add(team);
                    }
                }

                return group;
            }
        }

        private class TeamScoreDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int[] Scores { get; set; }

            public TeamScoreDTO() { }

            public TeamScoreDTO(Blue_4.Team team)
            {
                Type = team.GetType().Name;
                Name = team.Name;
                Scores = team.Scores;
            }

            public Blue_4.Team ToTeam()
            {
                Blue_4.Team team = Type == nameof(Blue_4.WomanTeam)
                    ? new Blue_4.WomanTeam(Name)
                    : new Blue_4.ManTeam(Name);

                if (Scores != null)
                {
                    foreach (var score in Scores)
                    {
                        team.PlayMatch(score);
                    }
                }

                return team;
            }
        }

        private class TeamDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public SportsmanDTO[] Sportsmen { get; set; }

            public TeamDTO() { }

            public TeamDTO(Blue_5.Team team)
            {
                Type = team.GetType().Name;
                Name = team.Name;

                if (team.Sportsmen != null)
                {
                    Sportsmen = team.Sportsmen
                        .Where(s => s != null)
                        .Select(s => new SportsmanDTO(s))
                        .ToArray();
                }
            }

            public Blue_5.Team ToTeam()
            {
                Blue_5.Team team = Type == nameof(Blue_5.WomanTeam)
                    ? new Blue_5.WomanTeam(Name)
                    : new Blue_5.ManTeam(Name);

                if (Sportsmen != null)
                {
                    foreach (var sportsmanDto in Sportsmen)
                    {
                        var sportsman = sportsmanDto.ToSportsman();
                        team.Add(sportsman);
                    }
                }

                return team;
            }
        }

        private class SportsmanDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Place { get; set; }

            public SportsmanDTO() { }

            public SportsmanDTO(Blue_5.Sportsman sportsman)
            {
                Name = sportsman.Name;
                Surname = sportsman.Surname;
                Place = sportsman.Place;
            }

            public Blue_5.Sportsman ToSportsman()
            {
                var sportsman = new Blue_5.Sportsman(Name, Surname);
                sportsman.SetPlace(Place);
                return sportsman;
            }
        }
    }
}
