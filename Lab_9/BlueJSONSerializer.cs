using Lab_7;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override string Extension => "json";

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);

            var anon = new // анонимный объект
            {
                Type = participant.GetType().Name,
                Name = participant.Name,
                Votes = participant.Votes,
                Surname = (participant as Blue_1.HumanResponse)?.Surname
            };

            string text = JsonConvert.SerializeObject(anon);
            File.WriteAllText(fileName, text);
        }

        private static int[][] ArrayToJagged(int[,] array) // массив в зубчатый
        {
            if (array == null) return null;

            int[][] jagged = new int[array.GetLength(0)][];

            for (int i = 0; i < jagged.Length; i++)
            {
                jagged[i] = new int[array.GetLength(1)];

                for (int j = 0; j < jagged[i].Length; j++)
                    jagged[i][j] = array[i, j];
            }

            return jagged;
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);

            var anon = new
            {
                Type = participant.GetType().Name,
                Name = participant.Name,
                Bank = participant.Bank,
                Participants = participant.Participants.Select(part => new
                {
                    part.Name,
                    part.Surname,
                    Marks = ArrayToJagged(part.Marks) 
                }).ToArray()
            };

            string text = JsonConvert.SerializeObject(anon);
            File.WriteAllText(fileName, text);
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);

            var anon = new
            {
                Type = student.GetType().Name,
                Name = student.Name,
                Surname = student.Surname,
                Penalties = student.Penalties
            };

            string text = JsonConvert.SerializeObject(anon);
            File.WriteAllText(fileName, text);
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);

            var anon = new
            {
                Type = participant.GetType().Name,
                Name = participant.Name,
                ManParticipant = participant.ManTeams.Where(t => t != null).Select(p => new
                {
                    p.Name,
                    p.Scores
                }).ToArray(),
                WomanParticipant = participant.WomanTeams.Where(t => t != null).Select(p => new
                {
                    p.Name,
                    p.Scores
                }).ToArray()
            };

            string text = JsonConvert.SerializeObject(anon);
            File.WriteAllText(fileName, text);
        }

        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);

            var anon = new
            {
                Type = group.GetType().Name,
                Name = group.Name,
                Sportsmen = group.Sportsmen.Where(t => t != null).Select(p => new
                {
                    p.Name,
                    p.Surname,
                    p.Place
                }).ToArray()
            };

            string text = JsonConvert.SerializeObject(anon);
            File.WriteAllText(fileName, text);
        }




        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            string content = File.ReadAllText(fileName);
            var json = JsonConvert.DeserializeObject<dynamic>(content); // dynamic, чтобы удобно обращаться к полям

            if ((string)json.Type == "HumanResponse") // наследник
            {
                return new Blue_1.HumanResponse(
                    (string)json.Name,
                    (string)json.Surname,
                    (int)json.Votes
                );
            }
            else // Response (родитель)
            {
                return new Blue_1.Response(
                    (string)json.Name,
                    (int)json.Votes
                );
            }
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            string content = File.ReadAllText(fileName);
            var json = JsonConvert.DeserializeObject<dynamic>(content);

            Blue_2.WaterJump waterJump; // родитель абстрактный 

            if ((string)json.Type == "WaterJump3m") // если наследник 3m
                waterJump = new Blue_2.WaterJump3m((string)json.Name, (int)json.Bank);

            else waterJump = new Blue_2.WaterJump5m((string)json.Name, (int)json.Bank); // если наследник 5m

            // приведение к типам string, int обязательно, так как dynamic не даёт строгой типизации


            // прыгуны
            foreach (dynamic DataParticipant in json.Participants)
            {
                if (DataParticipant == null) continue;

                var participant = new Blue_2.Participant(
                    (string)DataParticipant.Name,
                    (string)DataParticipant.Surname
                );

                // оценки прыжков
                int[][] marks = DataParticipant.Marks.ToObject<int[][]>();

                for (int i = 0; i < 2; i++)
                    if (marks[i].Length == 5) participant.Jump(marks[i]);

                waterJump.Add(participant);
            }

            return waterJump;
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            string content = File.ReadAllText(fileName);
            var json = JsonConvert.DeserializeObject<dynamic>(content);

            Blue_3.Participant participant;

            if ((string)json.Type == "HockeyPlayer")
                participant = new Blue_3.HockeyPlayer((string)json.Name, (string)json.Surname); // наследник хоккеист

            else if ((string)json.Type == "BasketballPlayer")
                participant = new Blue_3.BasketballPlayer((string)json.Name, (string)json.Surname); // наследник баскетболист

            else
                participant = new Blue_3.Participant((string)json.Name, (string)json.Surname); // родитель

            // пенальти
            int[] penalties = json.Penalties.ToObject<int[]>();

            foreach (var time in penalties)
            {
                participant.PlayMatch(time);
            }
                
            return (T)participant;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            string content = File.ReadAllText(fileName);
            var json = JsonConvert.DeserializeObject<dynamic>(content);

            Blue_4.Group group = new Blue_4.Group((string)json.Name);

            // man team
            foreach (var man in json.ManParticipant)
            {
                var manTeam = new Blue_4.ManTeam((string)man.Name);

                int[] scores = man.Scores.ToObject<int[]>();
                foreach (int score in scores)
                {
                    manTeam.PlayMatch(score);
                }

                group.Add(manTeam);
            }

            // woman team
            foreach (var woman in json.WomanParticipant)
            {
                var womanTeam = new Blue_4.WomanTeam((string)woman.Name);

                int[] scores = woman.Scores.ToObject<int[]>();
                foreach (int score in scores)
                {
                    womanTeam.PlayMatch(score);
                }

                group.Add(womanTeam);
            }

            return group;
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            string content = File.ReadAllText(fileName);
            var json = JsonConvert.DeserializeObject<dynamic>(content);

            Blue_5.Team team; // родитель абстрактный

            if ((string)json.Type == "ManTeam")
                team = new Blue_5.ManTeam((string)json.Name); // man team
            else 
                team = new Blue_5.WomanTeam((string)json.Name); // woman team

            // сами спортсмены
            foreach (var DataSportsman in json.Sportsmen)
            {
                if (DataSportsman.Name == null || DataSportsman.Surname == null) continue;

                var sportsman = new Blue_5.Sportsman((string)DataSportsman.Name, (string)DataSportsman.Surname);

                sportsman.SetPlace((int)DataSportsman.Place);
                team.Add(sportsman);

            }

            return (T)team;
            // приведение типа (cast) переменной team к универсальному (обобщённому) типу T
        }
    }
}
