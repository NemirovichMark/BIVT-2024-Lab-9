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

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);

            var data = new
            {
                Type = participant.GetType().Name,
                participant.Name,
                participant.Votes,
                Surname = participant is Blue_1.HumanResponse human ? human.Surname : null
            };

            File.WriteAllText(FilePath, JsonConvert.SerializeObject(data));
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);

            dynamic data = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(FilePath));
            return data.Type == "HumanResponse"
                ? new Blue_1.HumanResponse((string)data.Name, (string)data.Surname, (int)data.Votes)
                : new Blue_1.Response((string)data.Name, (int)data.Votes);
        }

        private static int[][] ConvertMarksToJagged(int[,] marks)
        {
            if (marks == null) return null;
            int[][] result = new int[marks.GetLength(0)][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new int[marks.GetLength(1)];
                for (int j = 0; j < result[i].Length; j++)
                    result[i][j] = marks[i, j];
            }
            return result;
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump jump, string fileName)
        {
            if (jump == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            var data = new
            {
                Type = jump is Blue_2.WaterJump3m ? "3m" : "5m",
                jump.Name,
                jump.Bank,
                Participants = jump.Participants?.Select(p => new
                {
                    p.Name,
                    p.Surname,
                    Marks = ConvertMarksToJagged(p.Marks)
                })
            };

            File.WriteAllText(FilePath, JsonConvert.SerializeObject(data));
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);

            dynamic data = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(FilePath));

            Blue_2.WaterJump jump;
            if (data.Type == "3m")
            {
                jump = new Blue_2.WaterJump3m((string)data.Name, (int)data.Bank);
            }
            else
            {
                jump = new Blue_2.WaterJump5m((string)data.Name, (int)data.Bank);
            }

            if (data.Participants != null)
            {
                foreach (var p in data.Participants)
                {
                    var participant = new Blue_2.Participant((string)p.Name, (string)p.Surname);
                    int[][] marks = p.Marks.ToObject<int[][]>();
                    for (int i = 0; i < marks.Length && i < 2; i++)
                        participant.Jump(marks[i]);
                    jump.Add(participant);
                }
            }

            return jump;
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {

            var participant = student as Blue_3.Participant;
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var player = new
            {
                Type =
                participant is Blue_3.BasketballPlayer ? "BasketballPlayer" :
                participant is Blue_3.HockeyPlayer ? "HockeyPlayer" :
                "Participant",
                participant.Name,
                participant.Surname,
                participant.Penalties
            };

            string text = JsonConvert.SerializeObject(player);
            File.WriteAllText(fileName, text);

        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(FilePath)) return default(T);
            SelectFile(fileName);
            string text = File.ReadAllText(fileName);

            var student = JsonConvert.DeserializeObject<dynamic>(text);

            Blue_3.Participant player;
            if ((string)student.Type == "BasketballPlayer")
                player = new Blue_3.BasketballPlayer((string)student.Name, (string)student.Surname);
            else if ((string)student.Type == "HockeyPlayer")
                player = new Blue_3.HockeyPlayer((string)student.Name, (string)student.Surname);
            else
                player = new Blue_3.Participant((string)student.Name, (string)student.Surname);

            foreach (var penalti in student.Penalties)
            {
                player.PlayMatch(int.Parse(penalti.ToString()));
            }


            return (T)(object)player;
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            var part = new
            {
                Type = participant.GetType().Name,
                participant.Name,
                Women = participant.WomanTeams
                    .Where(team => team != null)
                    .Select(team => new { team.Name, team.Scores })
                    .ToArray(),
                Men = participant.ManTeams
                    .Where(team => team != null)
                    .Select(team => new { team.Name, team.Scores })
                    .ToArray()
            };

            File.WriteAllText(fileName, JsonConvert.SerializeObject(part));
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);

            dynamic part = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(fileName));
            var group = new Blue_4.Group((string)part.Name);

            foreach (var woman in part.Women)
            {
                var team = new Blue_4.WomanTeam((string)woman.Name);
                foreach (var score in woman.Scores)
                {
                    team.PlayMatch((int)score);
                }
                group.Add(team);
            }

            foreach (var man in part.Men)
            {
                var team = new Blue_4.ManTeam((string)man.Name);
                foreach (var score in man.Scores)
                {
                    team.PlayMatch((int)score);
                }
                group.Add(team);
            }

            return group;
        }

        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            var team = group as Blue_5.Team;
            if (team == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            var obj = new
            {
                Type = team.GetType().Name,
                team.Name,
                Sportsman = team.Sportsmen
                    .Where(p => p != null)
                    .Select(p => new { p.Name, p.Surname, p.Place })
                    .ToArray()
            };

            File.WriteAllText(fileName, JsonConvert.SerializeObject(obj));
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return default(T);
            SelectFile(fileName);

            dynamic obj = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(fileName));
            Blue_5.Team team = null;

            if (obj.Type == "ManTeam")
            {
                team = new Blue_5.ManTeam((string)obj.Name);
            }
            else if (obj.Type == "WomanTeam")
            {
                team = new Blue_5.WomanTeam((string)obj.Name);
            }
            else
            {
                return default(T);
            }

            if (obj.Sportsman != null)
            {
                foreach (var s in obj.Sportsman)
                {
                    var sportsman = new Blue_5.Sportsman((string)s.Name, (string)s.Surname);
                    sportsman.SetPlace((int)s.Place);
                    team.Add(sportsman);
                }
            }

            return (T)(object)team;
        }
    }
}