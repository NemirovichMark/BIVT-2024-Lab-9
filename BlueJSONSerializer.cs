using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override string Extension => "json";

        private void CheckArguments(object obj, string fileName)
        {
            if (obj == null || string.IsNullOrWhiteSpace(fileName)) return;
        }

        // Сериализация
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            CheckArguments(participant, fileName);
            var data = new //анонимный объект
            {
                Type = participant.GetType().Name, //свойства объекта
                Name=participant.Name,
                Votes=participant.Votes,
                Surname = (participant as Blue_1.HumanResponse)?.Surname //проверить, является ли participant объектом типа Blue_1.HumanResponse
                                                                         //если да - получить значение его свойства Surname
                                                                         //если нет - вернуть null
            };
            string json = System.Text.Json.JsonSerializer.Serialize(data);
            File.WriteAllText(fileName, json);
        }
        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (!File.Exists(fileName)) return null;

            var json = File.ReadAllText(fileName);
            var data = JsonConvert.DeserializeObject<dynamic>(json);

            return data.Type == "HumanResponse"
                ? new Blue_1.HumanResponse((string)data.Name, (string)data.Surname, (int)data.Votes)
                : new Blue_1.Response((string)data.Name, (int)data.Votes);
        }


        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            CheckArguments(participant, fileName);
            // сериализация объекта в JSON строку
            var participants = new object[participant.Participants.Length];
            for (int i = 0; i < participant.Participants.Length; i++) //обрабатываем массив уч и созд для каждого анон объект
            {
                Blue_2.Participant j=participant.Participants[i];
                participants[i] = new
                {
                    Type = "Participant",
                    Name = j.Name,
                    Surname = j.Surname,
                    Marks = j.Marks
                };
            }
            var data = new
            {
                Type = participant.GetType().Name,
                Name=participant.Name,
                Bank=participant.Bank,
                Participants = participants
            };
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(fileName, json);
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (!File.Exists(fileName)) return null;

            var json = File.ReadAllText(fileName);
            var data = JsonConvert.DeserializeObject<dynamic>(json);

            Blue_2.WaterJump jumper;
            if (data.Type == "WaterJump3m")
            {
                jumper = new Blue_2.WaterJump3m((string)data.Name, (int)data.Bank);
            }
            else
            {
                jumper = new Blue_2.WaterJump5m((string)data.Name, (int)data.Bank);
            }

            foreach (var p in data.Participants)
            {
                var participant = new Blue_2.Participant((string)p.Name, (string)p.Surname);
                var marks = JsonConvert.DeserializeObject<int[][]>(p.Marks.ToString());
                foreach (var jump in marks)
                    participant.Jump(jump);
                jumper.Add(participant);
            }

            return jumper;
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName) //where T : Blue_3.Participant
        {
            if (student == null || string.IsNullOrEmpty(fileName))
                return;

            SelectFile(fileName);
            var ex = new
            {
                Type = student.GetType().Name,
                student.Name,
                student.Surname,
                student.Penalties
            };
            string text = JsonConvert.SerializeObject(ex);
            File.WriteAllText(fileName, text);
        }
        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(FilePath))
                return default(T);

            string json = File.ReadAllText(fileName);
            dynamic data = JsonConvert.DeserializeObject(json);
            Blue_3.Participant player;
            switch ((string)data.Type)
            {
                case "BasketballPlayer":
                    player = new Blue_3.BasketballPlayer((string)data.Name, (string)data.Surname);
                    break;

                case "HockeyPlayer":
                    player = new Blue_3.HockeyPlayer((string)data.Name, (string)data.Surname);
                    break;
                default:
                    player = new Blue_3.Participant((string)data.Name, (string)data.Surname);
                    break;
            };

            if (data.Penalties != null)
            {
                foreach (var penalty in data.Penalties)
                    player.PlayMatch((int)penalty);
            }

            return (T)(object)player;
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            CheckArguments(participant, fileName);
            var manTeamsList = new List<object>();
            if (participant.ManTeams != null)
            {
                foreach (var t in participant.ManTeams)
                {
                    if (t != null)
                    {
                        manTeamsList.Add(new { Type = "ManTeam", t.Name, t.Scores });
                    }
                }
            }

            var womanTeamsList = new List<object>();
            if (participant.WomanTeams != null)
            {
                foreach (var t in participant.WomanTeams)
                {
                    if (t != null)
                    {
                        womanTeamsList.Add(new { Type = "WomanTeam", t.Name, t.Scores });
                    }
                }
            }

            var data = new
            {
                participant.Name,
                ManTeams = manTeamsList,
                WomanTeams = womanTeamsList
            };
            File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (!File.Exists(fileName)) return null;

            var json = File.ReadAllText(fileName);
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            var group = new Blue_4.Group((string)data.Name);

            if (data.ManTeams != null)
                foreach (var team in data.ManTeams)
                {
                    var t = new Blue_4.ManTeam((string)team.Name);
                    if (team.Scores != null)
                        foreach (var score in team.Scores)
                            t.PlayMatch((int)score);
                    group.Add(t);
                }

            if (data.WomanTeams != null)
                foreach (var team in data.WomanTeams)
                {
                    var t = new Blue_4.WomanTeam((string)team.Name);
                    if (team.Scores != null)
                        foreach (var score in team.Scores)
                            t.PlayMatch((int)score);
                    group.Add(t);
                }

            return group;
        }
        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            CheckArguments(group, fileName);
            var data = new
            {
                Type = group.GetType().Name,
                group.Name,
                Sportsmen = group.Sportsmen?.Where(s => s != null).Select(s => new
                {
                    s.Name,
                    s.Surname,
                    s.Place
                })
            };
            File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            if (!File.Exists(fileName)) return default;

            var json = File.ReadAllText(fileName);
            var data = JsonConvert.DeserializeObject<dynamic>(json);

            Blue_5.Team team;
            if (data.Type == "ManTeam")
            {
                team = new Blue_5.ManTeam((string)data.Name);
            }
            else
            {
                team = new Blue_5.WomanTeam((string)data.Name);
            }

            if (data.Sportsmen != null)
                foreach (var s in data.Sportsmen)
                {
                    var sportsman = new Blue_5.Sportsman((string)s.Name, (string)s.Surname);
                    sportsman.SetPlace((int)s.Place);
                    team.Add(sportsman);
                }

            return (T)team;
        }
    }
}
