using Lab_7;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        public override string Extension => "xml";
        // Реализовать абстрактные методы класса-родителя
        // так, чтобы они сериализовывали/десериализовывали
        // объекты в формате xml.
        // При сериализации сохранять только публичные
        // нестатические свойства объекта! При десериализации
        // использовать имеющийся в классе конструктор и
        // методы для заполнения данных объекта аналогично
        // созданию нового объекта.Значения свойств
        // десериализованного объекта должны полностью
        // совпадать со значениями свойств базового объекта.
        // Для сериализации и десериализации в xml
        // использовать вложенные приватные классы с
        // набором необходимых свойств.

        public class Blue_1_ResponseDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Votes { get; set; }
            public string Surname { get; set; }
            public Blue_1_ResponseDTO() { }
            public Blue_1_ResponseDTO(Blue_1.Response response)
            {
                Type = response.GetType().Name;
                Name = response.Name;
                Votes = response.Votes;
                if (response is Blue_1.HumanResponse human)
                    Surname = human.Surname;
            }

        }

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            
            SelectFile(fileName);
            Blue_1_ResponseDTO dto = new Blue_1_ResponseDTO(participant);
            var serializer = new XmlSerializer(typeof(Blue_1_ResponseDTO));
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, dto);
            }
        }
        private static int[][] ConvertToJaggedArray(int[,] array)
        {
            if (array == null || array.Length == 0) return null;

            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            int[][] jaggedArray = new int[rows][];
            for (int i = 0; i < rows; i++)
            {
                jaggedArray[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                    jaggedArray[i][j] = array[i, j];
            }
            return jaggedArray;
        }

        public class Blue_2_ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[][] Marks { get; set; }
            public Blue_2_ParticipantDTO() { }
            public Blue_2_ParticipantDTO(Blue_2.Participant participant)
            {
                Name = participant.Name;
                Surname = participant.Surname;
                Marks = ConvertToJaggedArray(participant.Marks);
            }
        }

        public class Blue_2_WaterJumpDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Bank { get; set; }
            public Blue_2_ParticipantDTO[] Participants { get; set; }
            public Blue_2_WaterJumpDTO() { }
            public Blue_2_WaterJumpDTO(Blue_2.WaterJump waterjump)
            {
                Type = waterjump.GetType().Name;
                Name = waterjump.Name;
                Bank = waterjump.Bank;
                Participants = waterjump.Participants.Select(p => new 
                Blue_2_ParticipantDTO(p)).ToArray();
            }
        }
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return; 
            
            SelectFile(fileName);
            Blue_2_WaterJumpDTO dto = new Blue_2_WaterJumpDTO(participant);
            var serializer = new XmlSerializer(typeof(Blue_2_WaterJumpDTO));
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, dto);
            }
        }

        public class Blue_3_ParticipantDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Penalties { get; set; }
            public Blue_3_ParticipantDTO() { }
            public Blue_3_ParticipantDTO(Blue_3.Participant participant) 
            {
                Type = participant.GetType().Name;
                Name = participant.Name;
                Surname = participant.Surname;
                Penalties = participant.Penalties;
            }


        }
        public override void SerializeBlue3Participant<T>(T student, string fileName) //where T : Blue_3.Participant
        {
            if (student == null || String.IsNullOrEmpty(fileName)) return;
            
            SelectFile(fileName);
            Blue_3_ParticipantDTO dto = new Blue_3_ParticipantDTO(student);
            var serializer = new XmlSerializer(typeof(Blue_3_ParticipantDTO));
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, dto);
            }

        }

        public class Blue_4_TeamDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int[] Scores { get; set; }
            public Blue_4_TeamDTO() { }
            public Blue_4_TeamDTO(Blue_4.Team team) 
            {
                Type =team.GetType().Name;
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
                ManTeams = group.ManTeams.Select(t => t == null ? null : new Blue_4_TeamDTO(t)).ToArray(); //null reference exception
                WomanTeams = group.WomanTeams.Select(t => t == null ? null : new Blue_4_TeamDTO(t)).ToArray();

            }


        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            Blue_4_GroupDTO dto = new Blue_4_GroupDTO(participant);
            var serializer = new XmlSerializer(typeof(Blue_4_GroupDTO));
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, dto);
            }

        }

        public class Blue_5_ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Place {  get; set; }

            public Blue_5_ParticipantDTO() { }
            public Blue_5_ParticipantDTO(Blue_5.Sportsman sportsman) 
            {
                Name= sportsman.Name;
                Surname= sportsman.Surname;
                Place= sportsman.Place;
            }
        }
        public class Blue_5_TeamDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public Blue_5_ParticipantDTO[] Sporsman { get; set; }
            public Blue_5_TeamDTO() { }
            public Blue_5_TeamDTO(Blue_5.Team team) 
            {
                Type = team.GetType().Name;
                Name = team.Name;
                Sporsman = team.Sportsmen.Select(p => p == null ? null : new Blue_5_ParticipantDTO(p)).ToArray();
            }
        }
        public override void SerializeBlue5Team<T>(T group, string fileName)// where T : Blue_5.Team
        {
            if (group == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            Blue_5_TeamDTO dto = new Blue_5_TeamDTO(group);
            var serializer = new XmlSerializer(typeof(Blue_5_TeamDTO));
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, dto);
            }
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            var serializer = new XmlSerializer(typeof(Blue_1_ResponseDTO));

            Blue_1_ResponseDTO dto;
            Blue_1.Response result;

            using (StreamReader reader = new StreamReader(FilePath))
            {
                dto = (Blue_1_ResponseDTO)serializer.Deserialize(reader);
            }
            if (dto.Surname != null)
                result = new Blue_1.HumanResponse(dto.Name, dto.Surname, dto.Votes);
            else
                result = new Blue_1.Response(dto.Name, dto.Votes);
            return result;
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            var serializer = new XmlSerializer(typeof(Blue_2_WaterJumpDTO));
            
            Blue_2_WaterJumpDTO dto;
            Blue_2.WaterJump waterJump;
            
            using (StreamReader reader = new StreamReader(FilePath))
            {
                dto = (Blue_2_WaterJumpDTO)serializer.Deserialize(reader);
            }

            if (dto.Type == "WaterJump3m")
                waterJump = new Blue_2.WaterJump3m(dto.Name, dto.Bank);
            else
                waterJump = new Blue_2.WaterJump5m(dto.Name, dto.Bank);
            
            //participant[]
            for (int i = 0; i < dto.Participants.Length; i++)
            {
                Blue_2.Participant participant = new Blue_2.Participant(dto.Participants[i].Name, dto.Participants[i].Surname);
                int[][] marks = dto.Participants[i].Marks; 
                for (int j = 0; j < 2; j++)
                {
                    if (marks[j].Length == 5)
                        participant.Jump(marks[j]);
                }
                waterJump.Add(participant);
            }
            return waterJump;
        }

        public override T DeserializeBlue3Participant<T>(string fileName) //where T : Blue_3.Participant
        {
            SelectFile(fileName);
            var serializer = new XmlSerializer(typeof(Blue_3_ParticipantDTO));

            Blue_3_ParticipantDTO dto;
            Blue_3.Participant participant;

            using (StreamReader reader = new StreamReader(FilePath))
            {
                dto = (Blue_3_ParticipantDTO)serializer.Deserialize(reader);
            }

            if (dto.Type == "HockeyPlayer")
                participant = new Blue_3.HockeyPlayer(dto.Name, dto.Surname);
            else if (dto.Type == "BasketballPlayer")
                participant = new Blue_3.BasketballPlayer(dto.Name, dto.Surname);
            else
                participant = new Blue_3.Participant(dto.Name, dto.Surname);

            int[] penalties = dto.Penalties;
            foreach (var time in penalties)
                participant.PlayMatch(time);

            return (T)participant;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName); 
            var serializer = new XmlSerializer(typeof(Blue_4_GroupDTO));

            Blue_4_GroupDTO dto;
            Blue_4.Group group;

            using (StreamReader reader = new StreamReader(FilePath))
            {
                dto = (Blue_4_GroupDTO)serializer.Deserialize(reader);
            }

            group = new Blue_4.Group(dto.Name);
            for (int i = 0; i < dto.ManTeams.Length; i++)
            {
                if (dto.ManTeams[i] != null)
                {
                    Blue_4.ManTeam man = new Blue_4.ManTeam(dto.ManTeams[i].Name);
                    int[] scores = dto.ManTeams[i].Scores;
                    foreach (var time in scores)
                        man.PlayMatch(time);
                    group.Add(man);
                }
            }

            for (int i = 0; i < dto.WomanTeams.Length; i++)
            {
                if (dto.WomanTeams[i] != null) //null reference exception
                {
                    Blue_4.WomanTeam woman = new Blue_4.WomanTeam(dto.WomanTeams[i].Name);
                    int[] scores = dto.WomanTeams[i].Scores;
                    foreach (var time in scores)
                        woman.PlayMatch(time);
                    group.Add(woman);
                }
            }

            return group;

        }

        public override T DeserializeBlue5Team<T>(string fileName) //where T : Blue_5.Team
        {
            SelectFile(fileName);
            var serializer = new XmlSerializer(typeof(Blue_5_TeamDTO));

            Blue_5_TeamDTO dto;
            Blue_5.Team team;

            using (StreamReader reader = new StreamReader(FilePath))
            {
                dto = (Blue_5_TeamDTO)serializer.Deserialize(reader);
            }

            if (dto.Type == "ManTeam")
                team = new Blue_5.ManTeam(dto.Name);
            else
                team = new Blue_5.WomanTeam(dto.Name);

            for (int i = 0; i < dto.Sporsman.Length; i++)
            {
                if (dto.Sporsman[i] != null)
                {
                    Blue_5.Sportsman sportsman = new Blue_5.Sportsman(dto.Sporsman[i].Name, dto.Sporsman[i].Surname);
                    sportsman.SetPlace(dto.Sporsman[i].Place);
                    team.Add(sportsman);
                }
            }

            return (T)team;
        }

    }
}
