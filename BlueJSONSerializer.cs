using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Lab_7;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override string Extension => "json";

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrWhiteSpace(fileName)) return;

            SelectFile(fileName);

            var data = new
            {
                Type = participant.GetType().Name,
                participant.Name,
                participant.Votes,
                Surname = (participant as Blue_1.HumanResponse)?.Surname
            };

            File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
        }

        private static int[][] ToJaggedArray(int[,] matrix)
        {
            if (matrix == null) return null;

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int[][] result = new int[rows][];

            for (int i = 0; i < rows; i++)
            {
                result[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                    result[i][j] = matrix[i, j];
            }

            return result;
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrWhiteSpace(fileName)) return;

            SelectFile(fileName);

            var data = new
            {
                Type = participant.GetType().Name,
                participant.Name,
                participant.Bank,
                Participants = participant.Participants?.Select(p => new
                {
                    p.Name,
                    p.Surname,
                    Marks = ToJaggedArray(p.Marks)
                }).ToArray()
            };

            File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || string.IsNullOrWhiteSpace(fileName)) return;

            SelectFile(fileName);

            var data = new
            {
                Type = student.GetType().Name,
                student.Name,
                student.Surname,
                student.Penalties
            };

            File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrWhiteSpace(fileName)) return;

            SelectFile(fileName);

            var data = new
            {
                Type = participant.GetType().Name,
                participant.Name,
                Women = participant.WomanTeams?.Where(t => t != null).Select(t => new
                {
                    t.Name,
                    t.Scores
                }).ToArray(),
                Men = participant.ManTeams?.Where(t => t != null).Select(t => new
                {
                    t.Name,
                    t.Scores
                }).ToArray()
            };

            File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
        }

        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || string.IsNullOrWhiteSpace(fileName)) return;

            SelectFile(fileName);

            var data = new
            {
                Type = group.GetType().Name,
                group.Name,
                Sportsmen = group.Sportsmen?.Where(p => p != null).Select(p => new
                {
                    p.Name,
                    p.Surname,
                    p.Place
                }).ToArray()
            };

            File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;

            var text = File.ReadAllText(fileName);
            dynamic json = JsonConvert.DeserializeObject(text);

            if ((string)json.Type == "HumanResponse")
                return new Blue_1.HumanResponse((string)json.Name, (string)json.Surname, (int)json.Votes);

            return new Blue_1.Response((string)json.Name, (int)json.Votes);
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;

            var text = File.ReadAllText(fileName);
            dynamic json = JsonConvert.DeserializeObject(text);

            Blue_2.WaterJump jump = (string)json.Type == "WaterJump3m"
                ? new Blue_2.WaterJump3m((string)json.Name, (int)json.Bank)
                : new Blue_2.WaterJump5m((string)json.Name, (int)json.Bank);

            foreach (var item in json.Participants)
            {
                var part = new Blue_2.Participant((string)item.Name, (string)item.Surname);
                int[][] marks = item.Marks.ToObject<int[][]>();
                foreach (var m in marks)
                    part.Jump(m);
                jump.Add(part);
            }

            return jump;
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(FilePath))
                return default;

            var text = File.ReadAllText(fileName);
            dynamic json = JsonConvert.DeserializeObject(text);

            Blue_3.Participant player = (string)json.Type switch
            {
                "BasketballPlayer" => new Blue_3.BasketballPlayer((string)json.Name, (string)json.Surname),
                "HockeyPlayer" => new Blue_3.HockeyPlayer((string)json.Name, (string)json.Surname),
                _ => new Blue_3.Participant((string)json.Name, (string)json.Surname)
            };

            foreach (var pen in json.Penalties)
                player.PlayMatch((int)pen);

            return (T)(object)player;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;

            var text = File.ReadAllText(fileName);
            dynamic json = JsonConvert.DeserializeObject(text);

            var group = new Blue_4.Group((string)json.Name);

            foreach (var t in json.Men)
            {
                var team = new Blue_4.ManTeam((string)t.Name);
                foreach (var score in t.Scores)
                    team.PlayMatch((int)score);
                group.Add(team);
            }

            foreach (var t in json.Women)
            {
                var team = new Blue_4.WomanTeam((string)t.Name);
                foreach (var score in t.Scores)
                    team.PlayMatch((int)score);
                group.Add(team);
            }

            return group;
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(FilePath))
                return default;

            var text = File.ReadAllText(fileName);
            dynamic json = JsonConvert.DeserializeObject(text);

            Blue_5.Team team = (string)json.Type == "ManTeam"
                ? new Blue_5.ManTeam((string)json.Name)
                : new Blue_5.WomanTeam((string)json.Name);

            foreach (var p in json.Sportsmen)
            {
                var sport = new Blue_5.Sportsman((string)p.Name, (string)p.Surname);
                sport.SetPlace((int)p.Place);
                team.Add(sport);
            }

            return (T)(object)team;
        }
    }
}
