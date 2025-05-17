using Lab_7;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension => "txt";
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            string txt;
            if (participant is Blue_1.HumanResponse humanResponse)
            {
                txt = $"HumanResponse|{humanResponse.Name}|{humanResponse.Surname}|{humanResponse.Votes}";
            }
            else
            {
                txt = $"Response|{participant.Name}|{participant.Votes}";
            }
            File.WriteAllText(fileName, txt); // записывает текстовую строку txt в файл с именем fileName
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            StringBuilder txt = new StringBuilder();

            //тип название банк
            txt.AppendLine($"{participant.GetType().Name}|{participant.Name}|{participant.Bank}");

            //участники
            foreach (var p in participant.Participants)
            {
                string marks = "";
                for (int i = 0; i < 2; i++)
                {
                    if (i > 0) marks += ";";
                    for (int j = 0; j < 5; j++)
                    {
                        if (j > 0) marks += ",";
                        marks += p.Marks[i, j];
                    }
                }

                txt.AppendLine($"{p.Name}|{p.Surname}|{marks}");
            }
            File.WriteAllText(fileName, txt.ToString());
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName) 
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            StringBuilder txt = new StringBuilder();
            txt.AppendLine($"{student.Name}|{student.Surname}|{student.GetType().Name}|{student.Penalties.Length}|{string.Join(',', student.Penalties)}");

            File.WriteAllText(fileName, txt.ToString());
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            using (var txt = new StreamWriter(fileName)) // поток для записи в файл fileName
            {
                txt.WriteLine($"Name:{participant.Name}");

                txt.WriteLine($"ManTeamsCount:{participant.ManTeams.Length}");
                foreach (var manteam in participant.ManTeams)
                {
                    if (manteam != null)
                    {
                        txt.WriteLine($"{manteam.Name}|{string.Join(',', manteam.Scores)}");
                    }
                }

                txt.WriteLine($"WomanTeamsCount:{participant.WomanTeams.Length}");
                foreach (var womanteam in participant.WomanTeams)
                {
                    if (womanteam != null)
                    {
                        txt.WriteLine($"{womanteam.Name}|{string.Join(',', womanteam.Scores)}");
                    }
                }
            }
        }

        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            using (var txt = new StreamWriter(fileName))
            {
                txt.WriteLine($"Name={group.Name}");
                txt.WriteLine($"Type={group.GetType().Name}");

                int cnt = 0;
                for (int i = 0; i < group.Sportsmen.Length; i++)
                {
                    if (group.Sportsmen[i] != null)
                    {
                        cnt++;
                    }
                }
                txt.WriteLine($"Count={cnt}");

                for (int i = 0; i < group.Sportsmen.Length; i++)
                {
                    if (group.Sportsmen[i] != null)
                    {
                        txt.WriteLine($"{group.Sportsmen[i].Name}|{group.Sportsmen[i].Surname}|{group.Sportsmen[i].Place}");
                    }
                        
                }
            }
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) return null;

            string txt = File.ReadAllText(fileName); // Читаем все строки файла
            string[] parts = txt.Split('|');

            if (parts.Length < 3) return null;

            if (parts[0] == "HumanResponse" && parts.Length >= 4)
            {
                if (int.TryParse(parts[3], out int votes))
                {
                    return new Blue_1.HumanResponse(parts[1], parts[2], votes);
                }
                return new Blue_1.HumanResponse(parts[1], parts[2]);
            }
            else 
            {
                if (int.TryParse(parts[2], out int votes))
                {
                    return new Blue_1.Response(parts[1], votes);
                }
                return new Blue_1.Response(parts[1]);
            }
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) return null;

            string[] txt = File.ReadAllLines(fileName); // Читаем все строки файла
            if (txt.Length == 0) return null;

            // Парсим первую строку с основной информацией
            string[] header = txt[0].Split('|');
            if (header.Length < 3) return null;

            string type = header[0];
            Blue_2.WaterJump result;
            if (type == "WaterJump5m")
                result = new Blue_2.WaterJump5m(header[1], int.Parse(header[2]));
            else
                result = new Blue_2.WaterJump3m(header[1], int.Parse(header[2]));

            for (int i = 1; i < txt.Length; i++)
            {
                string[] parts = txt[i].Split('|');
                if (parts.Length < 3) continue;

                var participant = new Blue_2.Participant(parts[0], parts[1]);

                string[] jumps = parts[2].Split(';');
                for (int j = 0; j < jumps.Length && j < 2; j++)
                {
                    string[] marks = jumps[j].Split(',');
                    if (marks.Length == 5)
                    {
                        int[] marksInt = Array.ConvertAll(marks, int.Parse);
                        participant.Jump(marksInt);
                    }
                }
                result.Add(participant);
            }
            return result;
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) return default(T);

            string[] txt = File.ReadAllLines(fileName); // Читаем все строки файла

            string[] parts = txt[0].Split('|');
            if (parts.Length < 5) return default(T);

            string name = parts[0];
            string surname = parts[1];
            string type = parts[2];
            int[] penalties = new int[0];

            if (!string.IsNullOrEmpty(parts[4]))
            {
                string[] penaltyStrings = parts[4].Split(',');
                int[] tempPenalties = new int[penaltyStrings.Length];
                int count = 0;

                for (int i = 0; i < penaltyStrings.Length; i++)
                {
                    if (int.TryParse(penaltyStrings[i].Trim(), out int value))
                    {
                        tempPenalties[count] = value;
                        count++;
                    }
                }
                penalties = new int[count];
                Array.Copy(tempPenalties, penalties, count);
            }

            Blue_3.Participant participant = new Blue_3.Participant(name, surname);
            if (type == "BasketballPlayer")
            {
                participant = new Blue_3.BasketballPlayer(name, surname);
            }
            else if (type == "HockeyPlayer")
            {
                participant = new Blue_3.HockeyPlayer(name, surname);
            }

            foreach (int penalty in penalties)
            {
                participant.PlayMatch(penalty);
            }

            return (T)participant;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) return null;

            string[] txt = File.ReadAllLines(fileName); // Читаем все строки файла

            string groupName = txt[0].Replace("Name:", "").Trim(); // Первая строка название группы
            var result = new Blue_4.Group(groupName);

            // Предварительный подсчет количества команд каждого типа
            int manTeamsCount = 0, womanTeamsCount = 0;
            bool flagManTeams = false, flagWomanTeams = false;  // Флаг активности блока команд

            foreach (string line in txt) // Обрабатываем каждую строку файла
            {
                if (line.StartsWith("ManTeamsCount:")) // Определяем начало блока мужских команд
                {
                    manTeamsCount = int.Parse(line.Replace("ManTeamsCount:", "").Trim());
                    flagManTeams = true; flagWomanTeams = false;
                }
                else if (line.StartsWith("WomanTeamsCount:")) // Определяем начало блока женских команд
                {
                    womanTeamsCount = int.Parse(line.Replace("WomanTeamsCount:", "").Trim());
                    flagWomanTeams = true; flagManTeams = false;
                }
            }

            // Создание массивов фиксированного размера
            var manTeams = new Blue_4.ManTeam[manTeamsCount];
            var womanTeams = new Blue_4.WomanTeam[womanTeamsCount];
            int manIndex = 0, womanIndex = 0; // Счётчик добавленных команд

            // Обработка данных команд
            flagManTeams = false; // Флаг обработки мужских команд
            flagWomanTeams = false;

            foreach (string line in txt)
            {
                if (line.StartsWith("ManTeamsCount:"))// Определяем начало блока мужских команд
                {
                    flagManTeams = true; flagWomanTeams = false;
                    continue;
                }

                if (line.StartsWith("WomanTeamsCount:"))
                {
                    flagWomanTeams = true; flagManTeams = false;
                    continue;
                }

                if (line.Contains("|")) // Обработка строк с данными команд Name|score1,score2...
                {
                    string[] parts = line.Split('|');

                    string teamName = parts[0].Trim();
                    string[] scoresStr = parts[1].Split(',');

                    if (flagManTeams && manIndex < manTeams.Length)
                    {
                        var team = new Blue_4.ManTeam(teamName);
                        foreach (string scoreStr in scoresStr)
                        {
                            if (int.TryParse(scoreStr.Trim(), out int score))
                            {
                                team.PlayMatch(score);
                            }
                        }
                        manTeams[manIndex++] = team;
                    }
                    else if (flagWomanTeams && womanIndex < womanTeams.Length)
                    {
                        var team = new Blue_4.WomanTeam(teamName);
                        foreach (string scoreStr in scoresStr)
                        {
                            if (int.TryParse(scoreStr.Trim(), out int score))
                            {
                                team.PlayMatch(score);
                            }
                        }
                        womanTeams[womanIndex++] = team;
                    }
                }
            }

            // Добавление команд в группу
            result.Add(manTeams);
            result.Add(womanTeams);

            return result;
        }

        public override T DeserializeBlue5Team<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName)) return null;

            string[] txt = File.ReadAllLines(fileName); // Читаем все строки файла в массив

            string name = txt[0].Replace("Name=", "").Trim(); // Извлекаем название команды из первой строки
            string type = txt[1].Replace("Type=", "").Trim();
            int count = int.Parse(txt[2].Replace("Count=", "").Trim()); // Получаем количество спортсменов из третьей строки

            var sportsmen = new Blue_5.Sportsman[count]; // Создаем массив для хранения спортсменов указанного размера
            int ind = 0;

            for (int i = 3; i < txt.Length && ind < count; i++) // Обрабатываем оставшиеся строки (начиная с 4-й) со спортсменами
            {
                string[] parts = txt[i].Split('|');
                if (parts.Length >= 3)
                {
                    var sportsman = new Blue_5.Sportsman( // Создаем нового спортсмена с именем и фамилией
                        parts[0].Trim(),
                        parts[1].Trim()
                    );
                    sportsman.SetPlace(int.Parse(parts[2].Trim())); // Устанавливаем занятое место
                    sportsmen[ind++] = sportsman; // Добавляем спортсмена в массив
                }
            }
            Blue_5.Team team = new Blue_5.WomanTeam(name);
            if (type == "ManTeam")
            {
                team = new Blue_5.ManTeam(name);
            }

            team.Add(sportsmen);
            return (T)team;
        }
    }
}
