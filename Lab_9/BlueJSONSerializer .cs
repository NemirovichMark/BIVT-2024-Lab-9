using Lab_7;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override string Extension
        {
            get
            {
                return "json";
            }
        }

        // Blue_1
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);

            var man = new
            {
                Type =  participant.GetType().Name,
                participant.Name,
                participant.Votes,
                Surname = participant is Blue_1.HumanResponse human ? human.Surname : null
            };
            string str = JsonConvert.SerializeObject(man);
            File.WriteAllText(fileName, str);

        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return default(Blue_1.Response);
            SelectFile(fileName);

            string text = File.ReadAllText(fileName);


            var str = JsonConvert.DeserializeObject<dynamic>(text);

            string type = (string)str.Type;
            string name = (string)str.Name;
            int votes = (int)str.Votes;

            if (type == "HumanResponse")
            {
                string surname = (string)str.Surname;
                return new Blue_1.HumanResponse(name, surname, votes);
            }
            else
            {
                return new Blue_1.Response(name, votes);
            }

        }
   


        // Blue_2
        private static int[][] ToJaggedArray(int[,] arr)
        {
            if (arr == null)
                return null;
            int[][] newArr = new int[arr.GetLength(0)][];
            for (int i = 0; i < newArr.Length; i++)
            {
                newArr[i] = new int[arr.GetLength(1)];
                for (int j = 0; j < newArr[i].Length; j++)
                    newArr[i][j] = arr[i, j];
            }
            return newArr;
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {

            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            var man = new
            {
                Type = participant is Blue_2.WaterJump3m ? "3m" : "5m",
                participant.Name,
                participant.Bank,
                Participants = participant.Participants?.Select(p => new
                {
                    p.Name,
                    p.Surname,
                    Marks = InfoMarks(p.Marks)
                }).ToArray()

            };

            string text = JsonConvert.SerializeObject(man);
            File.WriteAllText(fileName, text);
        }

        //private string[] Meth(Blue_2.WaterJump participant)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (var p in participant.Participants)
        //    {
        //        sb.AppendLine($"{p.Name} {p.Surname} {InfoMarks(p.Marks).ToString()}");
        //    }
        //    return sb.ToString().Split(' ');
        //}

        private int[][] InfoMarks(int[,] arr)
        {
            if (arr == null) return null;

            int rows = arr.GetLength(0);
            int cols = arr.GetLength(1);

            int[][] arr1 = new int[rows][];

            for (int i = 0; i < rows; i++)
            {
                arr1[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                    arr1[i][j] = arr[i, j];
            }

            return arr1;

        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {

            if (String.IsNullOrEmpty(fileName)) return default(Blue_2.WaterJump);
            SelectFile(fileName);
            string text = File.ReadAllText(fileName);

            var str = JsonConvert.DeserializeObject<dynamic>(text);
            if (str == null) return null;

            string type = str.Type;
            string name = str.Name;
            int bank = str.Bank;
            Blue_2.WaterJump jump = null;
            if (str.Type == "3m")
                jump = new Blue_2.WaterJump3m(name, bank);
            else
                jump = new Blue_2.WaterJump5m(name, bank);


            var lines = str.Participants;
            if (lines == null)
            {
                jump.Add(Array.Empty<Blue_2.Participant>());
                return jump;
            }

            foreach (var part in str.Participants)
            {
                Blue_2.Participant participant = new Blue_2.Participant((string)part.Name, (string)part.Surname);
                int[][] jumps = part.Marks.ToObject<int[][]>(); // JArray в зубчатый массив
                for (int i = 0; i < 2; i++)
                {
                    if (jumps[i].Length == 5)
                        participant.Jump(jumps[i]);
                }
                jump.Add(participant);
            }
            return jump;
            //private int[,] MatrixMarks(int[][] arr1)
            //{
            //    if (arr1 == null || arr1.Length != 2 || arr1.Any(a => a == null || a.Length != 5)) return null;
            //    int[,] arr = new int[2, 5];
            //    for (int i = 0; i < 2; i++)
            //    {
            //        for (int j = 0; j < 5; j++)
            //            arr[i, j] = arr1[i][j];
            //    }
            //    return arr;
            //}
        }



        // Blue_3
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
            // string type = (string)student.Type;
            //string name = (string)student.Name;
            //string surname = (string)student.Surname;
            //int[] penalties = (int[])student.Penalties;

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

        //public override T DeserializeBlue3Participant<T>(string fileName)
        //{
        //    if (string.IsNullOrEmpty(fileName) || !File.Exists(FilePath))
        //        return default(T);

        //    string text = File.ReadAllText(fileName);
        //    var parsing = JsonConvert.DeserializeObject<dynamic>(text);
        //    Blue_3.Participant player;
        //    switch ((string)parsing.Type)
        //    {
        //        case "BasketballPlayer":
        //            player = new Blue_3.BasketballPlayer((string)parsing.Name, (string)parsing.Surname);
        //            break;
        //        case "HockeyPlayer":
        //            player = new Blue_3.HockeyPlayer((string)parsing.Name, (string)parsing.Surname);
        //            break;
        //        default:
        //            player = new Blue_3.Participant((string)parsing.Name, (string)parsing.Surname);
        //            break;
        //    }

        //    foreach (var penalty in parsing.Penalties)
        //        player.PlayMatch(int.Parse(penalty.ToString()));

        //    return (T)(object)player;
        //}


        // Blue_4
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            // string Name
            // Manteam[] ManTeams : Name  Scores
            // WomanTeams[] WomanTeams 

            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            var part = new
            {
                Type = participant.GetType().Name,
                participant.Name,

                Women = participant.WomanTeams
                .Where(team => team != null)
                .Select(team => new
                {
                    team.Name,
                    team.Scores
                })
                .ToArray(),

                Men = participant.ManTeams
                .Where(team => team != null)
                .Select(team => new
                {
                    team.Name,
                    team.Scores
                }).ToArray(),

            };
            string text = JsonConvert.SerializeObject(part);
            File.WriteAllText(fileName, text);
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            string text = File.ReadAllText(fileName);

            var part = JsonConvert.DeserializeObject<dynamic>(text);

            string groupName = part.Name;

            var group = new Blue_4.Group(groupName);

            foreach (var woman in part.Women)
            {
                string name = woman.Name;

                var team = new Blue_4.WomanTeam(name);

                foreach (var score in woman.Scores)
                {
                    team.PlayMatch(int.Parse(score.ToString()));
                }
                group.Add(team);
            }

            foreach (var man in part.Men)
            {
                string name = man.Name;

                var team = new Blue_4.ManTeam(name);
                foreach (int score in man.Scores)
                {
                    team.PlayMatch(int.Parse(score.ToString()));
                }
                group.Add(team);
            }

            return group;

        }



        // Blue_5
        public override void SerializeBlue5Team<T>(T group, string fileName) // where T : Blue_5.Team;
        {
            var team = group as Blue_5.Team;
            if (team == null || String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            var obj = new
            {
                Type = team.GetType().Name, // Сохраняем точный тип команды (ManTeam или WomanTeam)
                team.Name,  // Имя команды

                // Формируем массив спортсменов, исключая null-элементы
                Sportsman = team.Sportsmen.Where(p => p != null).Select(p => new
                {
                    p.Name,
                    p.Surname,
                    p.Place
                }).ToArray()
            };
            // Сериализуем объект в JSON-строку с помощью Newtonsoft.Json
            string text = JsonConvert.SerializeObject(obj);
            // Записываем JSON в файл
            File.WriteAllText(fileName, text);

        }


        public override T DeserializeBlue5Team<T>(string fileName) // where T : Blue_5.Team
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            string text = File.ReadAllText(fileName);

            // Десериализуем JSON в динамический объект (неопределённый тип)
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(text);

            // Извлекаем тип и имя команды из десериализованного объекта
            string type = obj.Type;
            string name = obj.Name;

            Blue_5.Team team = null;

            // В зависимости от типа создаём соответствующий объект команды
            if (type == nameof(Blue_5.ManTeam))
            {
                team = new Blue_5.ManTeam(name);
            }
            else if (type == nameof(Blue_5.WomanTeam))
            {
                team = new Blue_5.WomanTeam(name);
            }
            else
            {
                return null;
            }

            // Проходим по каждому спортсмену в массиве Sportsman
            foreach (var s in obj.Sportsman)
            {
                string sName = s.Name;
                string sSurname = s.Surname;
                int sPlace = (int)s.Place;

                var sportsman = new Blue_5.Sportsman(sName, sSurname);
                sportsman.SetPlace(sPlace);
                team.Add(sportsman);
            }

            // Возвращаем объект команды, приведенный к типу T
            return (T)(object)team;


        }

    }
}
