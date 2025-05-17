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
using static Lab_9.ClassDTO;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        public override string Extension => "json";
        
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            // сериализация объекта в JSON строку
            SelectFile(fileName);
            ResponseSD part = new ResponseSD(participant);
            string json = System.Text.Json.JsonSerializer.Serialize(part); //Сериализация объекта в JSON
            File.WriteAllText(fileName, json);// записывает строку json в файл с именем fileName
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            // сериализация объекта в JSON строку
            SelectFile(fileName);
            WaterJumpSD part = new WaterJumpSD(participant);
            string json = System.Text.Json.JsonSerializer.Serialize(part);
            File.WriteAllText(fileName, json);
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName) 
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;
            // сериализация объекта в JSON строку
            SelectFile(fileName);
            Participant3SD part = new Participant3SD(student);
            string json = System.Text.Json.JsonSerializer.Serialize(part);
            File.WriteAllText(fileName, json);
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            // сериализация объекта в JSON строку
            SelectFile(fileName);
            GroupSD part = new GroupSD(participant);
            string json = System.Text.Json.JsonSerializer.Serialize(part);
            File.WriteAllText(fileName, json);
        }

        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;
            // сериализация объекта в JSON строку
            SelectFile(fileName);
            int count = 0;  // Подсчёт ненулевых спортсменов
            foreach (var i in group.Sportsmen)
            {
                if (i != null) count++;
            }
            var sportsmenArray = new object[count];// Создание массива для сериализации
            int index = 0;
            foreach (var i in group.Sportsmen)// Заполнение массива объектами
            {
                if (i != null)
                {
                    sportsmenArray[index] = new
                    {
                        i.Name,
                        i.Surname,
                        i.Place
                    };
                    index++;
                }
            }
            var part = new// Формирование итогового объекта для сериализации
            {
                Type = group.GetType().Name,
                group.Name,
                Sportsmen = sportsmenArray
            };
            string json = System.Text.Json.JsonSerializer.Serialize(part);
            File.WriteAllText(fileName, json);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            string json = File.ReadAllText(fileName); //Чтение файла в строку

            var part = JsonConvert.DeserializeObject<dynamic>(json);//превращает JSON-строку в динамический объект, объект позволяет обращаться к любому полю JSON как к свойству
            string type = part.Type;

            if (type == "HumanResponse")
            {
                string name = part.Name;
                string surname = part.Surname;
                int votes = part.Votes;
                Blue_1.HumanResponse result = new Blue_1.HumanResponse(name, surname, votes);
                return result;
            }
            else
            {
                string name = part.Name;
                int votes = part.Votes;
                Blue_1.Response result = new Blue_1.Response(name, votes);
                return result;
            }
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            string json = File.ReadAllText(fileName);
            var part = JsonConvert.DeserializeObject<dynamic>(json);

            string type = part.Type;
            string name = part.Name;
            int bank = part.Bank;

            Blue_2.WaterJump result = new Blue_2.WaterJump3m(name, bank);
            if (type == "WaterJump5m")
            {
                result = new Blue_2.WaterJump5m(name, bank);
            }

            if (part.Participants != null)
            {
                foreach (var part2 in part.Participants) //Для каждого создаём объект Participant, передавая имя и фамилию
                {
                    var participant = new Blue_2.Participant((string)part2.Name, (string)part2.Surname);

                    if (part2.Marks != null) // Если у участника есть оценки, перебираем каждую
                    {
                        foreach (var jump in part2.Marks)
                        {
                            if (jump != null)
                            {
                                participant.Jump(((JArray)jump).ToObject<int[]>());//Преобразуем jump из JArray в массив int[] ызываем метод Jump у участника, передавая массив оценок   
                            }
                        }
                    }
                    result.Add(participant);
                }
            }

            return result;
        }

        public override T DeserializeBlue3Participant<T>(string fileName) 
        {
            if (string.IsNullOrEmpty(fileName)) return default(T);
            SelectFile(fileName);
            string json = File.ReadAllText(fileName);
            var part = JsonConvert.DeserializeObject<dynamic>(json);

            string type = part.Type;

            Blue_3.Participant result;
            if (type == "BasketballPlayer")
            {
                result = new Blue_3.BasketballPlayer((string)part.Name, (string)part.Surname);
            }
            else if (type == "HockeyPlayer")
            {
                result = new Blue_3.HockeyPlayer((string)part.Name, (string)part.Surname);
            }
            else
            {
                result = new Blue_3.Participant((string)part.Name, (string)part.Surname);
            }

            foreach(var penalties in part.Penalties)
            {
                result.PlayMatch(int.Parse(penalties.ToString()));//каждое значение штрафных очков в целое число и в метод PlayMatch
            }

            return (T)result;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            string json = File.ReadAllText(fileName);
            var part = JsonConvert.DeserializeObject<dynamic>(json);

            Blue_4.Group result = new Blue_4.Group((string)part.Name);
            if (part.ManTeams != null)
            {
                foreach (var team in part.ManTeams) // Проходим по каждой мужской команде
                {
                    if (team == null) continue;

                    Blue_4.ManTeam manteam = new Blue_4.ManTeam((string)team.Name);
                    if (team.Scores != null) // Если у команды есть результаты матчей (Scores)
                    {
                        foreach (var score in team.Scores)
                        {
                            manteam.PlayMatch((int)score); // Для каждого результата вызываем метод PlayMatch для учёта очков
                        }
                    }
                    result.Add(manteam);
                }
            }
            if (part.WomanTeams != null)
            {
                foreach (var team in part.WomanTeams)
                {
                    if (team == null) continue;

                    Blue_4.WomanTeam womanteam = new Blue_4.WomanTeam((string)team.Name);
                    if (team.Scores != null)
                    {
                        foreach (var score in team.Scores)
                        {
                            womanteam.PlayMatch((int)score);
                        }
                    }
                    result.Add(womanteam);
                }
            }

            return result;
        }

        public override T DeserializeBlue5Team<T>(string fileName) 
        {
            if (string.IsNullOrEmpty(fileName)) return default(T);
            SelectFile(fileName);
            string json = File.ReadAllText(fileName);
            var part = JsonConvert.DeserializeObject<dynamic>(json);

            Blue_5.Team result;
            if ((string)part.Type == "ManTeam")
            {
                result = new Blue_5.ManTeam((string)part.Name);
            }
            else
            {
                result = new Blue_5.WomanTeam((string)part.Name);
            }

            if (part.Sportsmen != null) // Если в JSON есть список спортсменов
            {
                foreach (var sportsman in part.Sportsmen)
                {
                    if (sportsman == null) continue;

                    var sport = new Blue_5.Sportsman((string)sportsman.Name, (string)sportsman.Surname);
                    sport.SetPlace((int)sportsman.Place); // Устанавливаем место спортсмена
                    result.Add(sport);
                }
            }

            return (T)result;
        }
    }
}