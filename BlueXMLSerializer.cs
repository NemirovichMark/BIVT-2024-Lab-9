using Lab_7;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static Lab_7.Blue_2;

namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        public override string Extension => "xml";
        public class ResponseDTO
        {
            public string Name { get; set; }
            public int Votes { get; set; }
            public string Surname { get; set; }
            public ResponseDTO() { }
            public ResponseDTO(Blue_1.Response response)
            {
                Name = response.Name;
                Votes = response.Votes;
                if (response is Blue_1.HumanResponse human) Surname = human.Surname;
            }
        }
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var ResponseDTO = new ResponseDTO(participant);
            var serializer = new XmlSerializer(typeof(ResponseDTO));
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, ResponseDTO);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);
            var serializer = new XmlSerializer(typeof(ResponseDTO));
            var dto = (ResponseDTO)serializer.Deserialize(reader);
            if (dto.Surname != null)
            {
                return new Blue_1.HumanResponse(dto.Name, dto.Surname, dto.Votes);
            }
            else
            {
                return new Blue_1.Response(dto.Name, dto.Votes);
            }
        }

        public class ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[][] Marks { get; set; }
            public ParticipantDTO() { }
            public ParticipantDTO(Blue_2.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = new int[participant.Marks.GetLength(0)][];
                for (int i = 0; i < participant.Marks.GetLength(0); i++)
                {
                    Marks[i] = new int[participant.Marks.GetLength(1)];
                    for (int j = 0; j < participant.Marks.GetLength(1); j++)
                    {
                        Marks[i][j] = participant.Marks[i, j];
                    }
                }
            }
        }
        public class WaterJumpDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Bank { get; set; }
            public ParticipantDTO[] Participants { get; set; }
            public WaterJumpDTO() { }
            public WaterJumpDTO(Blue_2.WaterJump waterJump)
            {
                Type = waterJump.GetType().Name;
                Name = waterJump.Name;
                Bank = waterJump.Bank;
                Participants = new ParticipantDTO[waterJump.Participants.Length];
                for (int i = 0; i < waterJump.Participants.Length; i++)
                {
                    Participants[i] = new ParticipantDTO(waterJump.Participants[i]);
                }
            }
        }
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var WaterJumpDTO = new WaterJumpDTO(participant);
            var serializer = new XmlSerializer(typeof(WaterJumpDTO));
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, WaterJumpDTO);
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);
            var serializer = new XmlSerializer(typeof(WaterJumpDTO));
            var dto = (WaterJumpDTO)serializer.Deserialize(reader);
            Blue_2.WaterJump waterjump;
            if (dto.Type == "WaterJump3m") waterjump = new Blue_2.WaterJump3m(dto.Name, dto.Bank);
            else if (dto.Type == "WaterJump5m") waterjump = new Blue_2.WaterJump5m(dto.Name, dto.Bank);
            else return null;
            foreach (var p in dto.Participants)
            {
                string pName = p.Name.ToString();
                string pSurname = p.Surname.ToString();
                var participant = new Blue_2.Participant(pName, pSurname);
                for (int i = 0; i < p.Marks.Length; i++)
                {
                    participant.Jump(p.Marks[i]);
                }
                waterjump.Add(participant);
            }
            return waterjump;
        }

        public class ParticipantBlue_3DTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Penalties { get; set; }
            public ParticipantBlue_3DTO() { }
            public ParticipantBlue_3DTO(Blue_3.Participant participant)
            {
                Type = participant.GetType().Name;
                Name = participant.Name;
                Surname = participant.Surname;
                Penalties = participant.Penalties;
            }
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName) where T : class
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var ParticipantBlue_3DTO = new ParticipantBlue_3DTO(student);
            var serializer = new XmlSerializer(typeof(ParticipantBlue_3DTO));
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, ParticipantBlue_3DTO);
        }

        public override T DeserializeBlue3Participant<T>(string fileName) where T : class
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);
            var serializer = new XmlSerializer(typeof(ParticipantBlue_3DTO));
            var dto = (ParticipantBlue_3DTO)serializer.Deserialize(reader);
            Blue_3.Participant participant;
            if (dto.Type == "Participant") participant = new Blue_3.Participant(dto.Name, dto.Surname);
            else if (dto.Type == "BasketballPlayer") participant = new Blue_3.BasketballPlayer(dto.Name, dto.Surname);
            else if (dto.Type == "HockeyPlayer") participant = new Blue_3.HockeyPlayer(dto.Name, dto.Surname);
            else return null;
            for (int i = 0; i < dto.Penalties.Length; i++)
            {
                participant.PlayMatch(dto.Penalties[i]);
            }
            return participant as T;
        }

        public class TeamDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int[] Scores {  get; set; }
            public TeamDTO() { }
            public TeamDTO(Blue_4.Team team)
            {
                Type = team.GetType().Name;
                Name = team.Name;
                Scores = team.Scores;
            }
        }
        public class GroupDTO
        {
            public string Name { get; set;}
            public TeamDTO[] ManTeams { get; set; }
            public TeamDTO[] WomanTeams { get; set; }
            public GroupDTO() { }
            public GroupDTO(Blue_4.Group group)
            {
                Name = group.Name;
                ManTeams = new TeamDTO[group.ManTeams.Length];
                for (int i = 0; i < group.ManTeams.Length; i++)
                {
                    if (group.ManTeams[i] != null) ManTeams[i] = new TeamDTO(group.ManTeams[i]);
                    else ManTeams[i] = null;
                }
                WomanTeams = new TeamDTO[group.WomanTeams.Length];
                for (int j = 0; j < group.WomanTeams.Length; j++)
                {
                    if (group.WomanTeams[j] != null) WomanTeams[j] = new TeamDTO(group.WomanTeams[j]);
                    else WomanTeams[j] = null;
                }
            }
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var GroupDTO = new GroupDTO(participant);
            var serializer = new XmlSerializer(typeof(GroupDTO));
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, GroupDTO);
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);
            var serializer = new XmlSerializer(typeof(GroupDTO));
            var dto = (GroupDTO)serializer.Deserialize(reader);
            Blue_4.Group group = new Blue_4.Group(dto.Name);
            for (int i = 0; i < dto.ManTeams.Length; i++)
            {
                if (dto.ManTeams[i] == null) continue;
                if (dto.ManTeams[i].Type == "ManTeam")
                {
                    Blue_4.ManTeam team = new Blue_4.ManTeam(dto.ManTeams[i].Name);
                    for (int j = 0; j < dto.ManTeams[i].Scores.Length; j++)
                    {
                        team.PlayMatch(dto.ManTeams[i].Scores[j]);
                    }
                    group.Add(team);
                }
            }
            for (int i = 0; i < dto.WomanTeams.Length; i++)
            {
                if (dto.WomanTeams[i] == null) continue;
                if (dto.WomanTeams[i].Type == "WomanTeam")
                {
                    Blue_4.WomanTeam team = new Blue_4.WomanTeam(dto.WomanTeams[i].Name);
                    for (int j = 0; j < dto.WomanTeams[i].Scores.Length; j++)
                    {
                        team.PlayMatch(dto.WomanTeams[i].Scores[j]);
                    }
                    group.Add(team);
                }
            }
            return group;
        }

        public class SportsmanDTO
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
        }
        public class TeamBlue_5DTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public SportsmanDTO[] Sportsmen { get; set; }
            public TeamBlue_5DTO() { }
            public TeamBlue_5DTO(Blue_5.Team team)
            {
                Type = team.GetType().Name;
                Name = team.Name;
                Sportsmen = new SportsmanDTO[team.Sportsmen.Length];
                for (int i = 0; i < team.Sportsmen.Length; i++)
                {
                    if (team.Sportsmen[i] != null)
                    {
                        Sportsmen[i] = new SportsmanDTO(team.Sportsmen[i]);
                    }
                }
            }
        }
        public override void SerializeBlue5Team<T>(T group, string fileName) where T : class
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var TeamBlue_5DTO = new TeamBlue_5DTO(group);
            var serializer = new XmlSerializer(typeof(TeamBlue_5DTO));
            using var writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, TeamBlue_5DTO);
        }

        public override T DeserializeBlue5Team<T>(string fileName) where T : class
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            using var reader = new StreamReader(FilePath);
            var serializer = new XmlSerializer(typeof(TeamBlue_5DTO));
            var dto = (TeamBlue_5DTO)serializer.Deserialize(reader);
            Blue_5.Team team;
            if (dto.Type == "ManTeam") team = new Blue_5.ManTeam(dto.Name);
            else if (dto.Type == "WomanTeam") team = new Blue_5.WomanTeam(dto.Name);
            else return null;
            for (int i = 0; i < dto.Sportsmen.Length; i++)
            {
                if (dto.Sportsmen[i] == null) continue;
                var sportsman = new Blue_5.Sportsman(dto.Sportsmen[i].Name, dto.Sportsmen[i].Surname);
                sportsman.SetPlace(dto.Sportsmen[i].Place);
                team.Add(sportsman);
            }
            return team as T;
        }
    }
}