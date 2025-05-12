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
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            //Создание анонимного объекта для сериализации
            var set = new
            {
                Type = participant.GetType().Name, //Response or HumanResponse
                participant.Name,// Эквивалентно Name = participant.Name
                participant.Votes,
                Surname = (participant as Blue_1.HumanResponse)?.Surname
                //Будет null, если не HumanResponse
            };
            string json = JsonConvert.SerializeObject(set);//преобразует анонимный объект в строку JSON

            File.WriteAllText(FilePath, json);//сохраняет JSON-строку в указанный файл

        }
        // Вспомогательный метод для преобразования двумерного массива оценок
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
            var set = new
            {
                Type = participant.GetType().Name,
                participant.Name,
                participant.Bank,
                Part = participant.Participants.Select(pr => new
                {
                    pr.Name,
                    pr.Surname,
                    Marks = ConvertMarks(pr.Marks)
                }).ToArray()
            };
            string json = JsonConvert.SerializeObject(set);
            File.WriteAllText(FilePath, json);
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName) //where T : Blue_3.Participant
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var set = new
            {
                Type = student.GetType().Name, 
                student.Name,
                student.Surname,
                student.Penalties
            };
            string json = JsonConvert.SerializeObject(set);
            File.WriteAllText(FilePath, json);
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var set = new
            {
                gType = participant.GetType().Name,
                participant.Name,
                manTeam = participant.ManTeams?.Where(team => team != null).Select(team => new
                {
                    tType = team.GetType().Name,
                    team.Name,
                    team.Scores

                }).ToArray() ?? Array.Empty<object>(),
                womanTeam = participant.WomanTeams?.Where(team => team != null).Select(team => new
                {
                    tType = team.GetType().Name,
                    team.Name,
                    Scores = team.Scores

                }).ToArray() ?? Array.Empty<object>()
            };

            string json = JsonConvert.SerializeObject(set);
            File.WriteAllText(FilePath, json);
        }
        public override void SerializeBlue5Team<T>(T group, string fileName) //where T : Blue_5.Team
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            var set = new
            {
                Type = group.GetType().Name,
                group.Name,
                Sportsmen = group.Sportsmen?.Where(s => s != null).Select(s => new
                {
                    s.Name,
                    s.Surname,
                    s.Place
                }).ToArray() ?? Array.Empty<object>()
            };
            string json = JsonConvert.SerializeObject(set);
            File.WriteAllText(FilePath, json);
        }

        
        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            string file = File.ReadAllText(FilePath);
            var toJson = JsonConvert.DeserializeObject<dynamic>(file);
            if ((string)toJson.Type == "HumanResponse")
            {
                return new Blue_1.HumanResponse(
                   (string)toJson.Name,
                   (string)toJson.Surname,
                   (int)toJson.Votes);
            }
            else
            {
                return new Blue_1.Response(
                    (string)toJson.Name,
                    (int)toJson.Votes);
            }
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            string file = File.ReadAllText(FilePath);
            var toJson = JsonConvert.DeserializeObject<dynamic>(file);
            Blue_2.WaterJump waterJump;
            if ((string)toJson.Type == "WaterJump3m")
                waterJump = new Blue_2.WaterJump3m((string)toJson.Name, (int)toJson.Bank);
            else if ((string)toJson.Type == "WaterJump5m")
                waterJump = new Blue_2.WaterJump5m((string)toJson.Name, (int)toJson.Bank);
            else
                return null;

            //Десериализация участников
            foreach (var p in toJson.Part)
            {
                var participant = new Blue_2.Participant(
                    (string)p.Name,
                    (string)p.Surname);

                // Восстанавливаем оценки
                if (p.Marks != null)
                {
                    var marksArray = (JArray)p.Marks;
                    int rows = marksArray.Count;
                    int cols = marksArray[0].Count();
                    var marks = new int[rows, cols];

                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            marks[i, j] = (int)marksArray[i][j];
                        }
                        // Добавляем прыжок
                        int[] jump = new int[cols];
                        for (int j = 0; j < cols; j++)
                        {
                            jump[j] = marks[i, j];
                        }
                        participant.Jump(jump);
                    }
                }
                //if (p.Marks != null)
                //{
                //    var marksArray = (JArray)p.Marks;
                //    foreach (var row in marksArray)
                //    {
                //        int[] jump = row.Select(x => (int)x).ToArray();
                //        participant.Jump(jump);
                //    }
                //}
                waterJump.Add(participant);
            }

            return waterJump;
        }
        public override T DeserializeBlue3Participant<T>(string fileName) //where T : Blue_3.Participant
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            string file = File.ReadAllText(FilePath);
            
            var toJson = JsonConvert.DeserializeObject<dynamic>(file);
           
            Blue_3.Participant participants;
            if ((string)toJson.Type == "BasketballPlayer") participants = new Blue_3.BasketballPlayer((string)toJson.Name, (string)toJson.Surname);
            else if ((string) toJson.Type == "HockeyPlayer") participants = new Blue_3.HockeyPlayer((string)toJson.Name, (string)toJson.Surname);
            else participants = new Blue_3.Participant((string)toJson.Name, (string)toJson.Surname);
            if (toJson.Penalties != null)
            {
                foreach (var penalty in toJson.Penalties)
                {
                    participants.PlayMatch((int)penalty);
                }
            }
            return (T)participants;
        }
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            string file = File.ReadAllText(FilePath);
            var toJson = JsonConvert.DeserializeObject<dynamic>(file);
            Blue_4.Group group = new Blue_4.Group((string)toJson.Name);
            if (toJson.manTeam != null)
            {
                foreach (var t in toJson.manTeam)
                {
                    var team = new Blue_4.ManTeam((string)t.Name);
                    if (t.Scores != null)
                    {
                        foreach (var score in t.Scores)
                        {
                            team.PlayMatch((int)score); // Явное приведение к int
                        }
                    }
                    group.Add(team);
                }
            }

            if (toJson.womanTeam != null)
            {
                foreach (var t in toJson.womanTeam)
                {
                    var team = new Blue_4.WomanTeam((string)t.Name);
                    if (t.Scores != null)
                    {
                        foreach (var score in t.Scores) //var = dynamic
                        {
                            team.PlayMatch((int)score); // Явное приведение к int
                        }
                    }
                    group.Add(team);
                }
            }

            return group;
        }
        public override T DeserializeBlue5Team<T>(string fileName) //where T : Blue_5.Team
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            string file = File.ReadAllText(FilePath);
            var toJson = JsonConvert.DeserializeObject<dynamic>(file);
            Blue_5.Team team;
            if ((string)toJson.Type == "WomanTeam") team = new Blue_5.WomanTeam((string)toJson.Name);
            else if ((string)toJson.Type == "ManTeam") team = new Blue_5.ManTeam((string)toJson.Name);
            else return default(T);
            // Добавляем спортсменов
            if (toJson.Sportsmen != null)
            {
                foreach (var s in toJson.Sportsmen)
                {
                    var participant = new Blue_5.Sportsman((string)s.Name, (string)s.Surname);
                    participant.SetPlace((int)s.Place);
                    team.Add(participant);
                }
            }
            return (T)team;
        }
    }
}
