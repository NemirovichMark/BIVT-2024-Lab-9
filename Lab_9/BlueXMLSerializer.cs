using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lab_9
{
    public class BlueXMLSerializer : BlueSerializer
    {
        public override string Extension => "xml";

        /* создать дополнительные вложенные в класс BlueXMLSerializer 
         * публичные классы, содержащие публичные нестатические свойства 
         * исходного класса! При десериализации использовать имеющиеся в 
         * этом классе автосвойства. На основе десериализованного объекта 
         * временного класса создать необходимый по заданию объект. 
         * Значения свойств десериализованного объекта должны полностью 
         * совпадать со значениями свойств базового объекта.*/

        public class Blue_1_ResponseDTO
        {
            public string Name { get; set; }
            public int Votes { get; set; }
            public string Surname { get; set; }
        }

        public class Blue_2_ParticipantDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[][] Marks { get; set; }
        }

        public class Blue_2_WaterJumpDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Bank { get; set; }
            public Blue_2_ParticipantDTO[] Participants { get; set; }
        }

        public class Blue_3_ParticipantDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int[] Penalties { get; set; }
        }


        public class Blue_4_TeamDTO
        {
            public string Name { get; set; }
            public int[] Scores { get; set; }
        }

        public class Blue_4_GroupDTO
        {
            public string Name { get; set; }
            public Blue_4_TeamDTO[] ManTeams { get; set; }
            public Blue_4_TeamDTO[] WomanTeams { get; set; }
        }

        public class Blue_5_SportsmanDTO
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Place { get; set; }
        }

        public class Blue_5_TeamDTO
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public Blue_5_SportsmanDTO[] Sportsmen { get; set; }
        }


        private void DataSerializer<T>(T data, string fileName)
        {
            SelectFile(fileName);

            if (string.IsNullOrEmpty(FolderPath) || string.IsNullOrEmpty(FilePath) || data == null) return;

            var serializer = new XmlSerializer(typeof(T));

            using (var writer = new StreamWriter(FilePath))
            {
                serializer.Serialize(writer, data);
            }
        }



        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);

            var data = new Blue_1_ResponseDTO
            {
                Name = participant.Name,
                Votes = participant.Votes,
                Surname = (participant as Blue_1.HumanResponse)?.Surname
            };

            DataSerializer(data, fileName);
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            var data = new Blue_2_WaterJumpDTO
            {
                Type = participant.GetType().AssemblyQualifiedName,
                Name = participant.Name,
                Bank = participant.Bank,
                Participants = participant.Participants.Select(p =>
                {
                    var marks = new int[p.Marks.GetLength(0)][];

                    for (int i = 0; i < p.Marks.GetLength(0); i++)
                    {
                        marks[i] = new int[p.Marks.GetLength(1)];
                        for (int j = 0; j < p.Marks.GetLength(1); j++)
                        {
                            marks[i][j] = p.Marks[i, j];
                        }
                    }

                    return new Blue_2_ParticipantDTO
                    {
                        Name = p.Name,
                        Surname = p.Surname,
                        Marks = marks
                    };

                }).ToArray()
            };

            DataSerializer(data, fileName);
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName) 
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;

            var data = new Blue_3_ParticipantDTO
            {
                Type = student.GetType().AssemblyQualifiedName,
                Name = student.Name,
                Surname = student.Surname,
                Penalties = student.Penalties.ToArray()
            };

            DataSerializer(data, fileName);
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            var data = new Blue_4_GroupDTO
            {
                Name = participant.Name,

                // man
                ManTeams = participant.ManTeams == null ? Array.Empty<Blue_4_TeamDTO>() :
                    participant.ManTeams.Where(m => m != null).Select(x => new Blue_4_TeamDTO
                    {
                        Name = x.Name,
                        Scores = x.Scores?.ToArray() ?? Array.Empty<int>()
                    }).ToArray(),

                //woman
                WomanTeams = participant.WomanTeams == null ? Array.Empty<Blue_4_TeamDTO>() :
                    participant.WomanTeams.Where(w => w != null).Select(x => new Blue_4_TeamDTO
                    {
                        Name = x.Name,
                        Scores = x.Scores?.ToArray() ?? Array.Empty<int>()
                    }).ToArray()
            };

            DataSerializer(data, fileName);
        }

        public override void SerializeBlue5Team<T>(T group, string fileName) 
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;

            var data = new Blue_5_TeamDTO
            {
                Type = group.GetType().AssemblyQualifiedName,
                Name = group.Name,
                Sportsmen = group.Sportsmen?.Where(t => t != null).Select(t => new Blue_5_SportsmanDTO
                {
                    Name = t.Name,
                    Surname = t.Surname,
                    Place = t.Place
                }).ToArray() ?? Array.Empty<Blue_5_SportsmanDTO>()
            };

            DataSerializer(data, fileName);
        }



        private T DataDeserializer<T>(string fileName) where T : class
        {
            SelectFile(fileName);

            if (string.IsNullOrEmpty(FolderPath) || string.IsNullOrEmpty(FilePath) || !File.Exists(FilePath)) return null;

            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StreamReader(FilePath))
            {
                return serializer.Deserialize(reader) as T;
            }
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            var data = DataDeserializer<Blue_1_ResponseDTO>(fileName);

            if (data == null) return null;

            if (data.Surname != null)
                return new Blue_1.HumanResponse(data.Name, data.Surname, data.Votes);
            else
                return new Blue_1.Response(data.Name, data.Votes);
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            var data = DataDeserializer<Blue_2_WaterJumpDTO>(fileName);

            if (data == null) return null;

            Blue_2.WaterJump waterJump = null;

            if (Type.GetType(data.Type) == typeof(Blue_2.WaterJump3m))
                waterJump = new Blue_2.WaterJump3m(data.Name, data.Bank);

            else if (Type.GetType(data.Type) == typeof(Blue_2.WaterJump5m))
                waterJump = new Blue_2.WaterJump5m(data.Name, data.Bank);

            else return null;


            foreach (var p in data.Participants)
            {
                var participant = new Blue_2.Participant(p.Name, p.Surname);

                foreach (var marks in p.Marks)
                {
                    participant.Jump(marks);
                }
                waterJump.Add(participant);
            }

            return waterJump;
        }

        public override T DeserializeBlue3Participant<T>(string fileName) 
        {
            var data = DataDeserializer<Blue_3_ParticipantDTO>(fileName);

            if (data == null) return null;

            var type = Type.GetType(data.Type);
            if (type == null) return null;

            var participant = (Blue_3.Participant)Activator.CreateInstance(type, data.Name, data.Surname);

            foreach (var penalty in data.Penalties)
            {
                participant.PlayMatch(penalty);
            }

            return (T)participant;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            var data = DataDeserializer<Blue_4_GroupDTO>(fileName);
            if (data == null) return null;

            var group = new Blue_4.Group(data.Name);

            if (data.ManTeams != null)
            {
                foreach (var team in data.ManTeams)
                {
                    if (team == null) continue;

                    var t = new Blue_4.ManTeam(team.Name);

                    if (team.Scores != null)
                    {
                        foreach (var score in team.Scores)
                        {
                            t.PlayMatch(score);
                        }
                    }
                    group.Add(t);
                }
            }

            if (data.WomanTeams != null)
            {
                foreach (var team in data.WomanTeams)
                {
                    if (team == null) continue;

                    var t = new Blue_4.WomanTeam(team.Name);

                    if (team.Scores != null)
                    {
                        foreach (var score in team.Scores)
                        {
                            t.PlayMatch(score);
                        }
                    }
                    group.Add(t);
                }
            }

            return group;
        }
        public override T DeserializeBlue5Team<T>(string fileName) 
        {
            var data = DataDeserializer<Blue_5_TeamDTO>(fileName);

            if (data == null) return null;

            var type = Type.GetType(data.Type);
            if (type == null) return null;

            Blue_5.Team team;

            if (type == typeof(Blue_5.ManTeam))
                team = new Blue_5.ManTeam(data.Name);

            else if (type == typeof(Blue_5.WomanTeam))
                team = new Blue_5.WomanTeam(data.Name);

            else team = null;


            if (team == null) return null;

            if (data.Sportsmen != null)
            {
                foreach (var s in data.Sportsmen)
                {
                    if (s == null) continue;

                    var sportsman = new Blue_5.Sportsman(s.Name, s.Surname);
                    sportsman.SetPlace(s.Place);
                    team.Add(sportsman);
                }
            }

            return (T)(object)team;
        }
    }
}
