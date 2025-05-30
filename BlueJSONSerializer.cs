using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab_7.Blue_5;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {

        public override string Extension
        {
            get { return "json"; }
        }

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            var obj = new
            {
                Type = participant.GetType().Name,
                Name = participant.Name,
                Votes = participant.Votes,
                Surname = (participant as Blue_1.HumanResponse)?.Surname
            };

            string json = JsonConvert.SerializeObject(obj);

            File.WriteAllText(fileName, json);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) return null;
            SelectFile(fileName);
            var content = File.ReadAllText(fileName);
            if (string.IsNullOrWhiteSpace(content)) return null;
            dynamic json = JsonConvert.DeserializeObject(content);
            if (json == null) return null;

            if ((string)json.Type == "HumanResponse")
            {
                var answer = new Blue_1.HumanResponse((string)json.Name, (string)json.Surname, (int)json.Votes);
                return answer;
            }
            else if ((string)json.Type == "Response")
            {
                var answer = new Blue_1.Response((string)json.Name, (int)json.Votes);
                return answer;
            }
            else
            {
                return null;
            }
        }

        private int[][] ConvertToJArray(int[,] m)
        {
            if (m == null || m.GetLength(0) == 0 || m.GetLength(1) == 0) return null;

            int[][] JArray = new int[m.GetLength(0)][];
            for (int i = 0; i < m.GetLength(0); i++)
            {
                JArray[i] = new int[m.GetLength(1)];
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    JArray[i][j] = m[i, j];
                }
            }
            return JArray;
        }

        private static int[,] ConvertToMatrix(int[][] arr)
        {
            int rows = arr.Length;
            int cols = arr[0].Length;
            int[,] m = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    m[i, j] = arr[i][j];
                }
            }

            return m;
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            var obj = new
            {
                Type = participant.GetType().Name,
                Name = participant.Name,
                Bank = participant.Bank,
                Participants = participant.Participants?.Select(participant => new
                {
                    participant.Name,
                    participant.Surname,
                    Marks = ConvertToJArray(participant.Marks)
                }).ToArray()
            };

            string json = JsonConvert.SerializeObject(obj);

            File.WriteAllText(fileName, json);

        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) return null;

            string json = File.ReadAllText(fileName);

            var obj = JsonConvert.DeserializeObject<dynamic>(json);

            Blue_2.WaterJump waterJump = null;
            string type = obj.Type;

            if (type == nameof(Blue_2.WaterJump3m))
            {
                waterJump = new Blue_2.WaterJump3m((string)obj.Name, (int)obj.Bank);
            }
            else if (type == nameof(Blue_2.WaterJump5m))
            {
                waterJump = new Blue_2.WaterJump5m((string)obj.Name, (int)obj.Bank);
            }

            if (waterJump != null && obj.Participants != null)
            {
                foreach (var participantObj in obj.Participants)
                {
                    var part = new Blue_2.Participant((string)participantObj.Name, (string)participantObj.Surname);
                    int[][] marks = participantObj.Marks.ToObject<int[][]>();
                    foreach (var m in marks)
                        part.Jump(m);
                    waterJump.Add(part);
                }

                return waterJump;
            }

            return waterJump;
        }



        public override void SerializeBlue3Participant<T>(T participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            var obj = new
            {
                Type = participant.GetType().Name,
                Name = participant.Name,
                Surname = participant.Surname,
                Penalties = participant.Penalties
            };

            string json = JsonConvert.SerializeObject(obj);
            File.WriteAllText(FilePath, json);

        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            if (!File.Exists(FilePath)) return null;

            string json = File.ReadAllText(FilePath);

            var obj = JsonConvert.DeserializeObject<dynamic>(json);

            Blue_3.Participant participant = null;
            string type = obj.Type;
            if (type == "BasketballPlayer")
            {
                participant = new Blue_3.BasketballPlayer((string)obj.Name, (string)obj.Surname);
            }
            else if (type == "HockeyPlayer")
            {
                participant = new Blue_3.HockeyPlayer((string)obj.Name, (string)obj.Surname);
            }
            else
            {
                participant = new Blue_3.Participant((string)obj.Name, (string)obj.Surname);
            }

            foreach (var p in obj.Penalties)
            {
                participant.PlayMatch((int)p);
            }
            return (T)(object)participant;
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            var obj = new
            {
                Name = participant.Name,
                WomanGroup = participant.WomanTeams
                    .Where(t => t != null) 
                    .Select(t => new
                    {
                        Type = t.GetType().Name,
                        Name = t.Name,
                        Scores = t.Scores
                    }).ToArray(),
                ManGroup = participant.ManTeams
                    .Where(t => t != null) 
                    .Select(t => new
                    {
                        Type = t.GetType().Name,
                        Name = t.Name,
                        Scores = t.Scores
                    }).ToArray()
            };

            string json = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(fileName, json);
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            if (!File.Exists(fileName)) return null; 

            string json = File.ReadAllText(fileName);
            var obj = JsonConvert.DeserializeObject<dynamic>(json);

            Blue_4.Group group = new Blue_4.Group((string)obj.Name);

            if (obj.WomanGroup != null)
            {
                foreach (var w in obj.WomanGroup)
                {
                    if (w != null)
                    {
                        var Wteam = new Blue_4.WomanTeam((string)w.Name);
                        int[] scores = w.Scores?.ToObject<int[]>();
                        if (scores != null)
                        {
                            foreach (var s in scores)
                            {
                                Wteam.PlayMatch(s);
                            }
                        }
                        group.Add(Wteam);
                    }
                }
            }

            if (obj.ManGroup != null)
            {
                foreach (var m in obj.ManGroup)
                {
                    if (m != null)
                    {
                        var Mteam = new Blue_4.ManTeam((string)m.Name);
                        int[] scores = m.Scores?.ToObject<int[]>();
                        if (scores != null)
                        {
                            foreach (var s in scores)
                            {
                                Mteam.PlayMatch(s);
                            }
                        }
                        group.Add(Mteam);
                    }
                }
            }
            return group;
        }
        public override void SerializeBlue5Team<T>(T team, string fileName)
        {
            if (team == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            var obj = new
            {
                Type = team.GetType().Name,
                Name = team.Name,
                Sportsman = team.Sportsmen.Where(t => t != null) 
                    .Select(t => new
                    {
                        Type = t.GetType().Name,
                        Name = t.Name,
                        Surname = t.Surname,
                        Place = t.Place
                    }).ToArray()
            };

            string json = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(FilePath, json);

        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            if (!File.Exists(FilePath)) return null;

            string json = File.ReadAllText(FilePath);
            var obj = JsonConvert.DeserializeObject<dynamic>(json);
            Blue_5.Team team;

            if ((string)obj.Type == "ManTeam")
            {
                team = new Blue_5.ManTeam((string)obj.Name);
            }
            else if ((string)obj.Type == "WomanTeam")
            {
                team = new Blue_5.WomanTeam((string)obj.Name);
            }
            else
            {
                return null;
            }

            if (obj.Sportsman != null)
            {
                foreach (var s in obj.Sportsman)
                {
                    if (s != null)
                    {
                        string name = (string)s.Name;
                        string surname = (string)s.Surname;
                        int place = (int)(s.Place); 

                        var sportsman = new Blue_5.Sportsman(name, surname);
                        sportsman.SetPlace(place);
                        team.Add(sportsman);

                    }
                }
            }
            return (T)team;
        }
    }
    }