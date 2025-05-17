using Lab_7;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        override public string Extension => "xml";

        public class Blue_1_ResponseDTO
        {
            public string Name { get; set; }
            public int Votes { get; set; }
            public string Surname { get; set; }
            public Blue_1_ResponseDTO() { }
            public Blue_1_ResponseDTO(Blue_1.Response response)
            {
                Name = response.Name;
                Votes = response.Votes;
                Surname = null;
                if (response is Blue_1.HumanResponse humanresponse) Surname = humanresponse.Surname;
            }
        }

        public class Blue_2_ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[,] Marks { get; set; }
            public Blue_2_ParticipantDTO() { }

            public Blue_2_ParticipantDTO(Blue_2.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = participant.Marks;
            }
        }

        public class Blue_2_WaterJumpDTO
        {
            public int Type { get; set; }
            public string Name { get; set; }
            public int Bank { get; set; }
            public Blue_2_ParticipantDTO[] Participants { get; set; }
            public Blue_2_WaterJumpDTO() { }

            public Blue_2_WaterJumpDTO(Blue_2.WaterJump jump)
            {
                if (jump is Blue_2.WaterJump3m) Type = 3;
                if (jump is Blue_2.WaterJump5m) Type = 5;
                Name = jump.Name;
                Bank = jump.Bank;
                Blue_2_ParticipantDTO[] pdto = new Blue_2_ParticipantDTO[jump.Participants.Length];
                for (int i = 0; i < jump.Participants.Length; i++) pdto[i] = new Blue_2_ParticipantDTO(jump.Participants[i]);
                Participants = pdto;
            }
        }

        public class Blue_3_ParticipantDTO
        {
            public char Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Penalties { get; set; }
            public Blue_3_ParticipantDTO() { }

            public Blue_3_ParticipantDTO(Blue_3.Participant participant)
            {
                Type = 'P';
                if (participant is Blue_3.BasketballPlayer) Type = 'B';
                if (participant is Blue_3.HockeyPlayer) Type = 'H';
                Name = participant.Name;
                Surname = participant.Surname;
                Penalties = participant.Penalties;
            }
        }

        public class Blue_4_TeamDTO
        {
            public string Name { get; set; }
            public int[] Scores { get; set; }
            public Blue_4_TeamDTO() { }

            public Blue_4_TeamDTO(Blue_4.Team team)
            {
                Name = team.Name;
                Scores = team.Scores;
            }
        }

        public class Blue_4_GroupDTO
        {
            public string Name { get; set; }
            public Blue_4_TeamDTO[] ManTeams { get; set; }
            public Blue_4_TeamDTO[] WomanTeams { get; set; }
            public Blue_4_GroupDTO() { }

            public Blue_4_GroupDTO(Blue_4.Group group)
            {
                Name = group.Name;
                ManTeams = new Blue_4_TeamDTO[group.ManTeams.Length];
                for (int i = 0; i < group.ManTeams.Length; i++) ManTeams[i] = new Blue_4_TeamDTO(group.ManTeams[i]);
                WomanTeams = new Blue_4_TeamDTO[group.WomanTeams.Length];
                for (int i = 0; i < group.WomanTeams.Length; i++) WomanTeams[i] = new Blue_4_TeamDTO(group.WomanTeams[i]);

            }
        }

        public class Blue_5_SportsmanDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Place { get; set; }
            public Blue_5_SportsmanDTO() { }

            public Blue_5_SportsmanDTO(Blue_5.Sportsman sportsman)
            {
                Name = sportsman.Name;
                Surname = sportsman.Surname;
                Place = sportsman.Place;
            }
        }

        public class Blue_5_TeamDTO
        {
            public char Type { get; set; }
            public string Name { get; set; }
            public Blue_5_SportsmanDTO[] Sportsmen { get; set; }
            public Blue_5_TeamDTO() { }

            public Blue_5_TeamDTO(Blue_5.Team team)
            {
                if (team is Blue_5.ManTeam) Type = 'M';
                if (team is Blue_5.WomanTeam) Type = 'W';
                Sportsmen = new Blue_5_SportsmanDTO[team.Sportsmen.Length];
                for (int i = 0; i < team.Sportsmen.Length; i++) Sportsmen[i] = new Blue_5_SportsmanDTO(team.Sportsmen[i]);
            }
        }

        override public void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if ((participant == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            Blue_1_ResponseDTO responseDTO = new Blue_1_ResponseDTO(participant);
            XmlSerializer serializer = new XmlSerializer(typeof(Blue_1_ResponseDTO));
            StreamWriter writer = new StreamWriter(fileName);
            serializer.Serialize(writer, responseDTO);
            writer.Close();
        }
        override public void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if ((participant == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            Blue_2_WaterJumpDTO waterJumpDTO = new Blue_2_WaterJumpDTO(participant);
            XmlSerializer serializer = new XmlSerializer(typeof(Blue_2_WaterJumpDTO));
            StreamWriter writer = new StreamWriter(fileName);
            serializer.Serialize(writer, waterJumpDTO);
            writer.Close();
        }
        override public void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if ((student == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            Blue_3_ParticipantDTO participantDTO = new Blue_3_ParticipantDTO(student);
            XmlSerializer serializer = new XmlSerializer(typeof(Blue_3_ParticipantDTO));
            StreamWriter writer = new StreamWriter(fileName);
            serializer.Serialize(writer, participantDTO);
            writer.Close();
        }
        override public void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if ((participant == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            Blue_4_GroupDTO groupDTO = new Blue_4_GroupDTO(participant);
            XmlSerializer serializer = new XmlSerializer(typeof(Blue_4_GroupDTO));
            StreamWriter writer = new StreamWriter(fileName);
            serializer.Serialize(writer, groupDTO);
            writer.Close();
        }
        override public void SerializeBlue5Team<T>(T group, string fileName)
        {
            if ((group == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            Blue_5_TeamDTO teamDTO = new Blue_5_TeamDTO(group);
            XmlSerializer serializer = new XmlSerializer(typeof(Blue_5_TeamDTO));
            StreamWriter writer = new StreamWriter(fileName);
            serializer.Serialize(writer, teamDTO);
            writer.Close();
        }

        override public Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            StreamReader reader = new StreamReader(fileName);
            XmlSerializer serializer = new XmlSerializer(typeof(Blue_1_ResponseDTO));
            Blue_1_ResponseDTO dto = (Blue_1_ResponseDTO)serializer.Deserialize(reader);
            Blue_1.Response response;
            if (dto.Surname == null)
            {
                response = new Blue_1.Response(dto.Name, dto.Votes);
            }
            else
            {
                response = new Blue_1.HumanResponse(dto.Name, dto.Surname, dto.Votes);
            }
            return response;
        }
        override public Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            StreamReader reader = new StreamReader(fileName);
            XmlSerializer serializer = new XmlSerializer(typeof(Blue_2_WaterJumpDTO));
            Blue_2_WaterJumpDTO dto = (Blue_2_WaterJumpDTO)serializer.Deserialize(reader);
            Blue_2.WaterJump waterJump;
            if (dto.Type == 3)
            {
                waterJump = new Blue_2.WaterJump3m(dto.Name, dto.Bank);
            }
            else
            {
                waterJump = new Blue_2.WaterJump5m(dto.Name, dto.Bank);
            }
            for (int i = 0; i < dto.Participants.Length; i++)
            {
                Blue_2.Participant participant = new Blue_2.Participant(dto.Participants[i].Name, dto.Participants[i].Surname);
                int[] jump = new int[5];
                for (int j = 0; j < 5; j++)
                {
                    jump[j] = dto.Participants[i].Marks[0, j];
                }
                participant.Jump(jump);
                for (int j = 0; j < 5; j++)
                {
                    jump[j] = dto.Participants[i].Marks[1, j];
                }
                participant.Jump(jump);
                waterJump.Add(participant);
            }
            return waterJump;
        }
        override public T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            StreamReader reader = new StreamReader(fileName);
            XmlSerializer serializer = new XmlSerializer(typeof(Blue_3_ParticipantDTO));
            Blue_3_ParticipantDTO dto = (Blue_3_ParticipantDTO)serializer.Deserialize(reader);
            Blue_3.Participant participant;
            if (dto.Type == 'B')
            {
                participant = new Blue_3.BasketballPlayer(dto.Name, dto.Surname);
            }
            else if (dto.Type == 'H')
            {
                participant = new Blue_3.HockeyPlayer(dto.Name, dto.Surname);
            }
            else
            {
                participant = new Blue_3.Participant(dto.Name, dto.Surname);
            }
            for (int i = 0; i < dto.Penalties.Length; i++) participant.PlayMatch(dto.Penalties[i]);
            return (T)participant;
        }
        override public Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            StreamReader reader = new StreamReader(fileName);
            XmlSerializer serializer = new XmlSerializer(typeof(Blue_4_GroupDTO));
            Blue_4_GroupDTO dto = (Blue_4_GroupDTO)serializer.Deserialize(reader);
            Blue_4.Group group = new Blue_4.Group(dto.Name);
            for (int i = 0; i < dto.ManTeams.Length; i++)
            {
                Blue_4.Team team = new Blue_4.ManTeam(dto.ManTeams[i].Name);
                for (int j = 0; j < dto.ManTeams[i].Scores.Length; j++)
                {
                    team.PlayMatch(dto.ManTeams[i].Scores[j]);
                }
                group.Add(team);
            }
            for (int i = 0; i < dto.WomanTeams.Length; i++)
            {
                Blue_4.Team team = new Blue_4.WomanTeam(dto.WomanTeams[i].Name);
                for (int j = 0; j < dto.WomanTeams[i].Scores.Length; j++)
                {
                    team.PlayMatch(dto.WomanTeams[i].Scores[j]);
                }
                group.Add(team);
            }
            return group;
        }
        override public T DeserializeBlue5Team<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            StreamReader reader = new StreamReader(fileName);
            XmlSerializer serializer = new XmlSerializer(typeof(Blue_5_TeamDTO));
            Blue_5_TeamDTO dto = (Blue_5_TeamDTO)serializer.Deserialize(reader);
            Blue_5.Team team;
            if (dto.Type == 'M')
            {
                team = new Blue_5.ManTeam(dto.Name);
            }
            else
            {
                team = new Blue_5.WomanTeam(dto.Name);
            }
            for (int i = 0; i < team.Sportsmen.Length; i++)
            {
                Blue_5.Sportsman sportsman = new Blue_5.Sportsman(dto.Sportsmen[i].Name, dto.Sportsmen[i].Surname);
                sportsman.SetPlace(dto.Sportsmen[i].Place);
                team.Add(sportsman);
            }
            return (T)team;
        }
    }
}
