using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using static Lab_7.Blue_1;
using static Lab_7.Blue_2;
using static Lab_7.Blue_3;
using static Lab_9.BlueXMLSerializer;

namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        public override string Extension => "xml";
       
        public class Blue_1ResponseDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Votes { get; set; }
            public string Surname { get; set; }
            public Blue_1ResponseDTO() { } // Для десериализации
            public Blue_1ResponseDTO(Blue_1.Response response) // Для сериализации
            {
                Type = response.GetType().Name;
                Name = response.Name;
                Votes = response.Votes;
                if (response is Blue_1.HumanResponse h)
                    Surname = h.Surname;
            }
        }

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {

            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var dto = new Blue_1ResponseDTO(participant);
            var serializer = new XmlSerializer(typeof(Blue_1ResponseDTO));
            using (var writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, dto);
            }

        }
        public class Blue_2ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[][] Marks { get; set; }
            public Blue_2ParticipantDTO() { }
            public Blue_2ParticipantDTO(Blue_2.Participant p)
            {
                Name = p.Name;
                Surname = p.Surname;
                Marks = ConvertMarks(p.Marks);
            }
        }
        public class Blue_2WaterJumpDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Bank { get; set; }
            public Blue_2ParticipantDTO[] Participants { get; set; }
            public Blue_2WaterJumpDTO() { }
            public Blue_2WaterJumpDTO(Blue_2.WaterJump water)
            {
                Type = water.GetType().Name;
                Name = water.Name;
                Bank = water.Bank;
                Participants = water.Participants.Select(t => new Blue_2ParticipantDTO(t)).ToArray();
            }
        }
        private static int[][] ConvertMarks(int[,] marks)
        {
            if (marks == null) return null;
            //Создается массив массивов с известным количеством строк
            int[][] result = new int[marks.GetLength(0)][];

            for (int i = 0; i < marks.GetLength(0); i++)
            {
                result[i] = new int[marks.GetLength(1)];
                for (int j = 0; j < marks.GetLength(1); j++)
                {
                    result[i][j] = marks[i, j];
                }
            }
            return result;
        }
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var dto = new Blue_2WaterJumpDTO(participant);
            var serializer = new XmlSerializer(typeof(Blue_2WaterJumpDTO));
            using (var writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, dto);
            }
        }

        public class Blue_3ParticipantDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Penalties { get; set; }
            public Blue_3ParticipantDTO() { }
            public Blue_3ParticipantDTO(Blue_3.Participant p)
            {
                Type = p.GetType().Name;
                Name = p.Name;
                Surname = p.Surname;
                Penalties = p.Penalties;
            }
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName) //where T : Blue_3.Participant
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var dto = new Blue_3ParticipantDTO(student);
            var serializer = new XmlSerializer(typeof(Blue_3ParticipantDTO));
            using (var writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, dto);
            }
        }


        public class Blue_4TeamDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int[] Scores { get; set; }
            public Blue_4TeamDTO() { }
            public Blue_4TeamDTO(Blue_4.Team p)
            {
                Type = p.GetType().Name;
                Name = p.Name;
                Scores = p.Scores;
            }
        }
        public class Blue_4GroupDTO
        {
            public string Name { get; set; }
            public Blue_4TeamDTO[] ManTeams { get; set; }
            public Blue_4TeamDTO[] WomanTeams { get; set; }

            public Blue_4GroupDTO() { }
            public Blue_4GroupDTO(Blue_4.Group p)
            {
                Name = p.Name;
                ManTeams = p.ManTeams.Select(x => x == null ? null : new Blue_4TeamDTO(x)).ToArray();
                WomanTeams = p.WomanTeams.Select(x => x == null ? null : new Blue_4TeamDTO(x)).ToArray();
            }
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var dto = new Blue_4GroupDTO(participant);
            var serializer = new XmlSerializer(typeof(Blue_4GroupDTO));
            using (var writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, dto);
            }
        }


        public class Blue_5SportsmanDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Place { get; set; }

            public Blue_5SportsmanDTO() { }
            public Blue_5SportsmanDTO(Blue_5.Sportsman p)
            {
                Name = p.Name;
                Surname = p.Surname;
                Place = p.Place;
            }
        }
        public class Blue_5TeamDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public Blue_5SportsmanDTO[] Sportsmen { get; set; }
            public Blue_5TeamDTO() { }
            public Blue_5TeamDTO(Blue_5.Team p)
            {
                Type = p.GetType().Name;
                Name = p.Name;
                Sportsmen = p.Sportsmen.Select(p => p == null ? null : new Blue_5SportsmanDTO(p)).ToArray();
            }
        }
        public override void SerializeBlue5Team<T>(T group, string fileName) //where T : Blue_5.Team
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var dto = new Blue_5TeamDTO(group);
            var serializer = new XmlSerializer(typeof(Blue_5TeamDTO));
            using (var writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, dto);
            }
        }

       
        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            var serializer = new XmlSerializer(typeof(Blue_1ResponseDTO));
            using (var reader = new StreamReader(FilePath))
            {
                var dto = (Blue_1ResponseDTO)serializer.Deserialize(reader);
                if (dto.Surname != null) return new Blue_1.HumanResponse(dto.Name, dto.Surname, dto.Votes);
                else return new Blue_1.Response(dto.Name, dto.Votes);
                
            }
            
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            var serializer = new XmlSerializer(typeof(Blue_2WaterJumpDTO));
            Blue_2.WaterJump water;
            using (var reader = new StreamReader(FilePath))
            {
                var dto = (Blue_2WaterJumpDTO)serializer.Deserialize(reader);

                if (dto.Type == "WaterJump5m") water = new Blue_2.WaterJump5m(dto.Name, dto.Bank);
                else if (dto.Type == "WaterJump3m") water = new Blue_2.WaterJump3m(dto.Name, dto.Bank);
                else return null;
                foreach (var p in dto.Participants)
                {
                    var participant = new Blue_2.Participant(p.Name, p.Surname);
                    foreach (var marks in p.Marks)
                    {
                        if (marks != null && marks.Length == 5) // Проверяем ровно 5 оценок
                        {
                            participant.Jump(marks);
                        }
                    }
                    water.Add(participant);
                }
            }
            return water;
        }
        public override T DeserializeBlue3Participant<T>(string fileName) //where T : Blue_3.Participant
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            var serializer = new XmlSerializer(typeof(Blue_3ParticipantDTO));
            Blue_3.Participant participants;
            using (var reader = new StreamReader(FilePath))
            {
                var dto = (Blue_3ParticipantDTO)serializer.Deserialize(reader);
                if (dto.Type == "BasketballPlayer") participants = new Blue_3.BasketballPlayer(dto.Name, dto.Surname);
                else if (dto.Type == "HockeyPlayer") participants = new Blue_3.HockeyPlayer(dto.Name, dto.Surname);
                else participants = new Blue_3.Participant(dto.Name, dto.Surname);
                if (dto.Penalties != null)
                {
                    foreach (int penalty in dto.Penalties)
                    {
                        participants.PlayMatch(penalty);
                    }
                }
                return (T)participants;
            }
        }
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            var serializer = new XmlSerializer(typeof(Blue_4GroupDTO));
            Blue_4.Group group;
            using (var reader = new StreamReader(FilePath))
            {
                var dto = (Blue_4GroupDTO)serializer.Deserialize(reader);
                group = new Blue_4.Group(dto.Name);
                if (dto.ManTeams != null)
                {
                    foreach (var t in dto.ManTeams)
                    {
                        if (t != null)
                        {
                            var men = new Blue_4.ManTeam(t.Name);

                            foreach (var score in t.Scores)
                            {
                                men.PlayMatch(score);
                            }
                            group.Add(men);
                        }
                    }
                }
                if (dto.WomanTeams != null)
                {
                    foreach (var t in dto.WomanTeams)
                    {
                        if (t != null)
                        {
                            var women = new Blue_4.WomanTeam(t.Name);

                            foreach (var score in t.Scores)
                            {
                                women.PlayMatch(score);
                            }
                            group.Add(women);
                        }
                    }
                }
            }
            return group;
        }
        public override T DeserializeBlue5Team<T>(string fileName) //where T : Blue_5.Team
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            var serializer = new XmlSerializer(typeof(Blue_5TeamDTO));
            Blue_5.Team teams;
            using (var reader = new StreamReader(FilePath))
            {
                var dto = (Blue_5TeamDTO)serializer.Deserialize(reader);
                if(dto.Type == "WomanTeam") teams = new Blue_5.WomanTeam(dto.Name);
                else if (dto.Type == "ManTeam") teams = new Blue_5.ManTeam(dto.Name);
                else return default(T);
                if (dto.Sportsmen != null)
                {
                    foreach (var s in dto.Sportsmen)
                    {
                        if (s != null)
                        {
                            var participant = new Blue_5.Sportsman(s.Name, s.Surname);
                            participant.SetPlace(s.Place);
                            teams.Add(participant);
                        }
                    }
                }
                return (T)teams;
            }
        }
    }
}
