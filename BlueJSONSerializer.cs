using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Xml.Linq;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override string Extension => "json";

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName))
            {
                return;
            }
            SelectFile(fileName);
            var data = new
            {
                Type = participant.GetType().Name,
                participant.Name,
                participant.Votes,
                Surname = participant is Blue_1.HumanResponse human ? human.Surname : null
            };
            File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
        }
        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            var j = File.ReadAllText(fileName);
            var data = JsonConvert.DeserializeObject<dynamic>(j); //объект операции котрого будут
                                                                  //решаться во время выполнения
            if (data == null) return null;
            string type = (string)data.Type;
            string name = (string)data.Name;
            int votes = (int)data.Votes;
            if (type == "HumanResponse")
            {
                return new Blue_1.HumanResponse((string)data.Name, (string)data.Surname, (int)data.Votes);
            }
            else
            {
                return new Blue_1.Response((string)data.Name, (int)data.Votes);
            }
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName))
            {
                return;
            }
            SelectFile(fileName);
            var data = new
            {
                Type = participant.GetType().Name,
                participant.Name,
                participant.Bank,
                Participants = participant.Participants.Select(part => new
                {
                    part.Name,
                    part.Surname,
                    Marks = Mark(part.Marks)
                })
                .ToArray()
            };
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(fileName, json);
        }
        private static int[][] Mark(int[,] mark)
        {
            if (mark == null)
            {
                return null;
            }
            int[][] New = new int[mark.GetLength(0)][];
            for (int i = 0; i < mark.GetLength(0); i++)
            {
                New[i] = new int[mark.GetLength(1)];
                for (int j = 0; j < mark.GetLength(1); j++)
                    New[i][j] = mark[i, j];
            }
            return New;
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            var json = File.ReadAllText(fileName);
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            if (data == null) return null;
            string type = (string)data.Type;
            string name = (string)data.Name;
            int bank = data.Bank;

            Blue_2.WaterJump waterJump;
            if (data.Type == "WaterJump3m")
                waterJump = new Blue_2.WaterJump3m(name, bank);
            else
                waterJump = new Blue_2.WaterJump5m(name, bank);

            foreach (var part in data.Participants)
            {
                if (part == null) continue;
                Blue_2.Participant participant = new Blue_2.Participant((string)part.Name, (string)part.Surname);
                int[][] jumps = part.Marks.ToObject<int[][]>();//ToObject<>
                                                               //метод Newtonsoft.Json для
                                                               //преобразования в нужный тип
                for (int i = 0; i < 2; i++)
                {
                    if (jumps[i].Length == 5)
                    {
                        participant.Jump(jumps[i]);
                    }
                }
                waterJump.Add(participant);
            }
            return waterJump;
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || string.IsNullOrEmpty(fileName))
            {
                return;
            }
            SelectFile(fileName);
            var data = new
            {
                Type = student.GetType().Name,
                student.Name,
                student.Surname,
                student.Penalties
            };
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(fileName, json);
        }

        public override T DeserializeBlue3Participant<T>(string fileName) //T: Blue_3.Participant
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }
            SelectFile(fileName);
            string json = File.ReadAllText(FilePath);
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            Blue_3.Participant participants;
            switch ((string)data["Type"]) //вернет значение "BasketballPlayer"
            {
                case "BasketballPlayer":
                    participants = new Blue_3.BasketballPlayer(
                        (string)data.Name,
                        (string)data.Surname);
                    break;
                case "HockeyPlayer":
                    participants = new Blue_3.HockeyPlayer(
                        (string)data.Name,
                        (string)data.Surname);
                    break;
                default:
                    participants = new Blue_3.Participant(
                        (string)data.Name,
                        (string)data.Surname);
                    break;
            }
            if (data.Penalties != null)
            {
                foreach (var penalty in data.Penalties)
                {
                    participants.PlayMatch((int)penalty);
                }
            }
            return (T)participants;
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) 
            { 
                return; 
            }
            SelectFile(fileName);
            var data = new
            {
                type = participant.GetType().Name,
                participant.Name,
                manTeam = participant.ManTeams.Where(team => team != null).Select
                (team => new
                {
                    Type = team.GetType().Name,
                    team.Name,
                    team.Scores

                }).ToArray(),
                womanTeam = participant.WomanTeams.Where(team => team != null).Select
                (team => new
                {
                    Type = team.GetType().Name,
                    team.Name,
                    team.Scores

                }).ToArray()
            };
            File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
        }
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) 
            { 
                return null; 
            }
            SelectFile(fileName);
            var json = File.ReadAllText(fileName);
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            if (data == null) 
            { 
                return null; 
            }
            var group = new Blue_4.Group(data.Name);
            foreach (var woman in data.Women)
            {
                var team = new Blue_4.WomanTeam(woman.Name);
                foreach (var score in woman.Scores)
                {
                    team.PlayMatch(int.Parse(score.ToString()));
                }
                if (team != null)
                {
                    group.Add(team);
                }
            }
            foreach (var man in data.Men)
            {
                var team = new Blue_4.ManTeam(man.Name);
                foreach (int score in man.Scores)
                {
                    team.PlayMatch(int.Parse(score.ToString()));
                }
                if (team != null)
                {
                    group.Add(team);
                }
                
            }
            return group;

        }
        public override void SerializeBlue5Team<T>(T group, string fileName) //T: Blue_5.Team
        {
            if (group == null || string.IsNullOrEmpty(fileName)) 
            { 
                return; 
            }
            SelectFile(fileName);

            var data = new
            {
                Type = group.GetType().Name,
                group.Name,
                Sportsmen = group.Sportsmen.Where(team => team != null).Select
                (team => new
                {
                    team.Name,
                    team.Surname, 
                    team.Place

                }).ToArray(),
            };

            File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
        }
        public override T DeserializeBlue5Team<T>(string fileName) //T: Blue_5.Team
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }
            var json = File.ReadAllText(fileName);
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            if (data == null) 
            { 
                return null; 
            }
            Blue_5.Team team;
            if (typeof(T) == typeof(Blue_5.ManTeam))
            {
                team = (T)(object)new Blue_5.ManTeam(data.Name);
            }
            else
            {
                team = (T)(object)new Blue_5.WomanTeam(data.Name);
            }
            foreach (var p in data.sportsman)
            {
                var sportsman = new Blue_5.Sportsman((string)p.Name, (string)p.Surname);
                sportsman.SetPlace((int)p.Place);
                team.Add(sportsman);
            }
            return (T)team;
        }
    }
}
