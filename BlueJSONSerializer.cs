using Lab_7;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override string Extension => "json";

        private void WriteToFile(object data, string fileName)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(fileName, json);
        }

        private T ReadFromFile<T>(string fileName)
        {
            var json = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<T>(json)!;
        }

        private int[][]? ConvertToJaggedArray(int[,]? array)
        {
            if (array == null || array.Length == 0) return null;

            int rows = array.GetLength(0);
            int cols = array.GetLength(1);
            var jagged = new int[rows][];
            for (int i = 0; i < rows; i++)
            {
                jagged[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                    jagged[i][j] = array[i, j];
            }
            return jagged;
        }

        private int[,]? ConvertTo2DArray(int[][]? jagged)
        {
            if (jagged == null || jagged.Length == 0) return null;

            int rows = jagged.Length;
            int cols = jagged[0].Length;
            int[,] array = new int[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    array[i, j] = jagged[i][j];

            return array;
        }

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            var temp = new
            {
                ResponseType = participant.GetType().Name,
                participant.Name,
                participant.Votes,
                Surname = (participant as Blue_1.HumanResponse)?.Surname
            };
            WriteToFile(temp, fileName);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            dynamic obj = ReadFromFile<dynamic>(fileName);
            return obj.ResponseType == "HumanResponse"
                ? new Blue_1.HumanResponse((string)obj.Name, (string)obj.Surname, (int)obj.Votes)
                : new Blue_1.Response((string)obj.Name, (int)obj.Votes);
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            var temp = new
            {
                ParticipantType = participant.GetType().Name,
                participant.Name,
                participant.Bank,
                Participants = participant.Participants?.Select(p => new
                {
                    p.Name,
                    p.Surname,
                    Marks = ConvertToJaggedArray(p.Marks)
                }).ToArray()
            };
            WriteToFile(temp, fileName);
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            dynamic obj = ReadFromFile<dynamic>(fileName);

            Blue_2.WaterJump jump = obj.ParticipantType == "WaterJump5m"
                ? new Blue_2.WaterJump5m((string)obj.Name, (int)obj.Bank)
                : new Blue_2.WaterJump3m((string)obj.Name, (int)obj.Bank);

            if (obj.Participants != null)
            {
                foreach (var p in obj.Participants)
                {
                    var part = new Blue_2.Participant((string)p.Name, (string)p.Surname);
                    int[][]? marks = p.Marks?.ToObject<int[][]>();

                    if (marks != null)
                    {
                        foreach (var mark in marks)
                        {                            
                            if (mark != null && mark.Length == 5)
                                part.Jump(mark);
                        }
                    }

                    jump.Add(part);
                }
            }

            return jump;
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            var temp = new
            {
                ParticipantType = student.GetType().AssemblyQualifiedName,
                student.Name,
                student.Surname,
                student.Penalties
            };
            WriteToFile(temp, fileName);
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            dynamic obj = ReadFromFile<dynamic>(fileName);

            Type type = Type.GetType((string)obj.ParticipantType!)!;
            object p;

            if (type == typeof(Blue_3.HockeyPlayer))
                p = new Blue_3.HockeyPlayer((string)obj.Name, (string)obj.Surname);
            else if (type == typeof(Blue_3.BasketballPlayer))
                p = new Blue_3.BasketballPlayer((string)obj.Name, (string)obj.Surname);
            else
                p = new Blue_3.Participant((string)obj.Name, (string)obj.Surname);

            int[] penalties = obj.Penalties?.ToObject<int[]>() ?? Array.Empty<int>();
            foreach (var time in penalties)
                ((dynamic)p).PlayMatch(time);

            return (T)p;
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            var temp = new
            {
                GroupType = participant.GetType().Name,
                participant.Name,
                womanParticipant = participant.WomanTeams?.Where(t => t != null).Select(t => new
                {
                    TeamType = t!.GetType().Name,
                    t.Name,
                    t.Scores
                }).ToArray(),
                manParticipant = participant.ManTeams?.Where(t => t != null).Select(t => new
                {
                    TeamType = t!.GetType().Name,
                    t.Name,
                    t.Scores
                }).ToArray()
            };
            WriteToFile(temp, fileName);
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            dynamic obj = ReadFromFile<dynamic>(fileName);
            var group = new Blue_4.Group((string)obj.Name);

            if (obj.womanParticipant != null)
            {
                foreach (var team in obj.womanParticipant)
                {
                    var t = new Blue_4.WomanTeam((string)team.Name);
                    int[] scores = team.Scores?.ToObject<int[]>() ?? Array.Empty<int>();
                    foreach (int s in scores) t.PlayMatch(s);
                    group.Add(t);
                }
            }

            if (obj.manParticipant != null)
            {
                foreach (var team in obj.manParticipant)
                {
                    var t = new Blue_4.ManTeam((string)team.Name);
                    int[] scores = team.Scores?.ToObject<int[]>() ?? Array.Empty<int>();
                    foreach (int s in scores) t.PlayMatch(s);
                    group.Add(t);
                }
            }

            return group;
        }

        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            var temp = new
            {
                TeamType = group.GetType().Name,
                group.Name,
                sportsman = group.Sportsmen?.Where(p => p != null).Select(p => new
                {
                    p!.Name,
                    p.Surname,
                    p.Place
                }).ToArray()
            };
            WriteToFile(temp, fileName);
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            dynamic obj = ReadFromFile<dynamic>(fileName);
            Blue_5.Team team = obj.TeamType == "ManTeam"
                ? new Blue_5.ManTeam((string)obj.Name)
                : new Blue_5.WomanTeam((string)obj.Name);

            if (obj.sportsman != null)
            {
                foreach (var s in obj.sportsman)
                {
                    var sp = new Blue_5.Sportsman((string)s.Name, (string)s.Surname);
                    sp.SetPlace((int)s.Place);
                    team.Add(sp);
                }
            }

            return (T)(object)team;
        }
    }
}