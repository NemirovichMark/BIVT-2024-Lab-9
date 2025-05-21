using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension
        {
            get
            {
                return "txt";
            }
        }

        // Blue1
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            var man = participant as Blue_1.HumanResponse;
            string text;
            if (man == null)
            {
                text = $"{participant.Name} {participant.Votes}";
            }
            else
            {
                text = $"{man.Name} {man.Surname} {man.Votes}";
            }
            SelectFile(fileName);
            File.WriteAllText(fileName, text);

        }
        //public override Blue_1.Response DeserializeBlue1Response(string fileName)
        //{
        //    if (String.IsNullOrEmpty(fileName)) return null;

        //    SelectFile(fileName);
        //    string content = File.ReadAllText(FilePath);
        //    string[] parts = content.Split(' ');

        //    if (parts.Length == 2)
        //    {
        //        string name = parts[0];
        //        if (int.TryParse(parts[1], out int votes))
        //        {
        //            return new Blue_1.Response(name, votes);
        //        }
        //    }
        //    else if (parts.Length == 3)
        //    {
        //        string name = parts[0];
        //        string surname = parts[1];
        //        if (int.TryParse(parts[2], out int votes))
        //        {
        //            return new Blue_1.HumanResponse(name, surname, votes);
        //        }
        //    }

        //    return null;
        //}
        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            string content = File.ReadAllText(fileName);

            string[] parts = content.Split(' ');

            Blue_1.Response response;
          
            if (parts.Length == 2)
            {
                response = new Blue_1.Response(parts[0], int.Parse(parts[1]));
            }
            else
            {
                response = new Blue_1.HumanResponse(parts[0], parts[1], int.Parse(parts[2]));
            }
            return response;    
        }

        // Blue2
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
          
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            StringBuilder sb = new StringBuilder();

            
            if (participant is Blue_2.WaterJump3m)
            {
                sb.Append("3m ");
            }
            else if (participant is Blue_2.WaterJump5m)
            {
                sb.Append("5m ");
            }

           
            sb.AppendLine($"{participant.Name} {participant.Bank}");

            // Если есть участники, записываем их данные
            if (participant.Participants != null && participant.Participants.Length > 0)
            {
                foreach (var p in participant.Participants)
                {
                    // Имя и фамилия участника в отдельной строке
                    sb.AppendLine($"{p.Name} {p.Surname}");

                    var marks = p.Marks;
                    if (marks != null)
                    {
                        // Записываем оценки для каждого из двух прыжков (2 строки по 5 оценок)
                        for (int i = 0; i < 2; i++)
                        {
                            for (int j = 0; j < 5; j++)
                                sb.Append(marks[i, j] + " ");
                            sb.AppendLine();
                        }
                    }
                }
            }

            // Устанавливаем путь для записи файла
            SelectFile(fileName);

            // Записываем всё содержимое в файл
            File.WriteAllText(fileName, sb.ToString());
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
           
            if (String.IsNullOrEmpty(fileName)) return null;

            // Устанавливаем путь к файлу
            SelectFile(fileName);

            // Читаем все строки из файла
            var lines = File.ReadAllLines(fileName);
            if (lines.Length == 0) return null;

            // Первая строка содержит тип прыжка, имя и призовой фонд
            string[] header = lines[0].Split();
            if (header.Length < 3) return null;

            string type = header[0];   // "3m" или "5m"
            string name = header[1];   // имя соревнования
            int bank = int.Parse(header[2]);  // призовой фонд

            Blue_2.WaterJump jump = null;
            if (type == "3m")
                jump = new Blue_2.WaterJump3m(name, bank);
            else if (type == "5m")
                jump = new Blue_2.WaterJump5m(name, bank);
            else
                return null; 

            int i = 1; // индекс для прохода по строкам файла

            while (i < lines.Length)
            {
               
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    i++;
                    continue;
                }

                // Следующая строка должна содержать имя и фамилию участника
                string[] parts = lines[i].Split();
                if (parts.Length < 2) break; 

                string pname = parts[0];      // имя участника
                string psurname = parts[1];   // фамилия участника
                var participant = new Blue_2.Participant(pname, psurname);
                i++;

                // Читаем оценки с 2 строк по 5 чисел (матрица 2x5)
                int[,] marks = new int[2, 5];
                for (int row = 0; row < 2; row++)
                {
                    if (i >= lines.Length) break;
                    string[] markLine = lines[i].Split();
                    for (int col = 0; col < 5; col++)
                    {
                        marks[row, col] = int.Parse(markLine[col]);
                    }
                    i++;
                }

                // Добавляем прыжки с оценками участнику
                for (int jumpIndex = 0; jumpIndex < 2; jumpIndex++)
                {
                    int[] jumpMarks = new int[5];
                    for (int j = 0; j < 5; j++)
                    {
                        jumpMarks[j] = marks[jumpIndex, j];
                    }
                    participant.Jump(jumpMarks);  // регистрируем оценки прыжка
                }

                // Добавляем участника в соревнование
                jump.Add(participant);
            }

            // Возвращаем объект с загруженными данными
            return jump;
        }

        // Blue3
        public override void SerializeBlue3Participant<T>(T student, string fileName)  //where T : Blue_3.Participant
        {
            var participant = student as Blue_3.Participant;
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            StringBuilder sb = new StringBuilder();

            if (participant is Blue_3.BasketballPlayer)
                sb.AppendLine("BasketballPlayer");
            else if (participant is Blue_3.HockeyPlayer)
                sb.AppendLine("HockeyPlayer");
            else
                sb.AppendLine("Participant");

            sb.AppendLine($"{participant.Name} {participant.Surname}");

            if (participant.Penalties != null)
            {
                foreach (int p in participant.Penalties)
                    sb.Append($"{p} ");
            }

            SelectFile(fileName);
            File.WriteAllText(fileName, sb.ToString());

        }


        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;

            SelectFile(fileName);
            var lines = File.ReadAllLines(fileName);
            if (lines.Length < 2) return null;

            Blue_3.Participant participant = null;

            string typeLine = lines[0];
            string[] nameParts = lines[1].Split(' ');
            if (nameParts.Length < 2) return null;

            string name = nameParts[0];
            string surname = nameParts[1];

            switch (typeLine.Trim())
            {
                case "BasketballPlayer":
                    participant = new Blue_3.BasketballPlayer(name, surname);
                    break;
                case "HockeyPlayer":
                    participant = new Blue_3.HockeyPlayer(name, surname);
                    break;
                case "Participant":
                    participant = new Blue_3.Participant(name, surname);
                    break;
                default:
                    return null;
            }

            if (lines.Length >= 3)
            {
                string[] penaltyParts = lines[2].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string part in penaltyParts)
                {
                    if (int.TryParse(part, out int p))
                        participant.PlayMatch(p);
                }
            }

            return (T)(object)participant;
        }





        // Blue4
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            
            if (participant == null || String.IsNullOrEmpty(fileName)) return;

            
            string text = participant.Name + Environment.NewLine;

            // Если есть мужские команды, проходим по каждой
            if (participant.ManTeams != null)
            {
                for (int i = 0; i < participant.ManTeams.Length; i++)
                {
                    if (participant.ManTeams[i] == null) continue; // пропускаем пустые команды

                    // Записываем имя команды и все ее очки через пробел
                    text += $"{participant.ManTeams[i].Name} ";
                    for (int j = 0; j < participant.ManTeams[i].Scores.Length; j++)
                    {
                        text += $"{participant.ManTeams[i].Scores[j]} ";
                    }
                    // Добавляем "man" как разделитель команд мужского пола
                    text += "man";
                }
            }

            // Переход на новую строку перед женскими командами
            text += Environment.NewLine;

            // Аналогично для женских команд
            if (participant.WomanTeams != null)
            {
                for (int i = 0; i < participant.WomanTeams.Length; i++)
                {
                    if (participant.WomanTeams[i] == null) continue;

                    text += $"{participant.WomanTeams[i].Name} ";
                    for (int j = 0; j < participant.WomanTeams[i].Scores.Length; j++)
                    {
                        text += $"{participant.WomanTeams[i].Scores[j]} ";
                    }
                    // Добавляем "woman" как разделитель команд женского пола
                    text += "woman";
                }
            }

            
            SelectFile(fileName);

            
            File.WriteAllText(FilePath, text);
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
           
            if (String.IsNullOrEmpty(fileName)) return null;

            
            SelectFile(fileName);

            // Читаем весь текст из файла
            string text = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(text)) return null;

            // Разбиваем текст на строки, удаляя пустые
            var temp = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length < 1) return null; 

            // Создаем объект группы с именем из первой строки
            Blue_4.Group result = new Blue_4.Group(temp[0]);

            // Обработка мужских команд - данные во второй строке temp[1]
            if (temp.Length > 1)
            {
                // Разделяем строку по ключевому слову "man", чтобы выделить каждую команду
                string[] manTeams = temp[1].Split(new[] { "man" }, StringSplitOptions.RemoveEmptyEntries);
                Blue_4.ManTeam[] ManTeams = new Blue_4.ManTeam[manTeams.Length];

                for (int i = 0; i < ManTeams.Length; i++)
                {
                    // Убираем лишние пробелы и разделяем имя и очки команды
                    var tempNS = manTeams[i].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tempNS.Length == 0) continue;  // если пусто, пропускаем

                    // Первый элемент - имя команды
                    ManTeams[i] = new Blue_4.ManTeam(tempNS[0]);

                    // Остальные элементы - очки, которые добавляем через PlayMatch
                    for (int j = 1; j < tempNS.Length; j++)
                    {
                        ManTeams[i].PlayMatch(int.Parse(tempNS[j]));
                    }
                }
                // Добавляем все мужские команды в группу
                result.Add(ManTeams);
            }

            // Обработка женских команд - данные в третьей строке temp[2]
            if (temp.Length > 2)
            {
                // Аналогично, разделяем по "woman" для выделения команд
                string[] womanTeams = temp[2].Split(new[] { "woman" }, StringSplitOptions.RemoveEmptyEntries);
                Blue_4.WomanTeam[] WomanTeams = new Blue_4.WomanTeam[womanTeams.Length];

                for (int i = 0; i < WomanTeams.Length; i++)
                {
                    var tempNS = womanTeams[i].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tempNS.Length == 0) continue;

                    WomanTeams[i] = new Blue_4.WomanTeam(tempNS[0]);

                    for (int j = 1; j < tempNS.Length; j++)
                    {
                        WomanTeams[i].PlayMatch(int.Parse(tempNS[j]));
                    }
                }
                // Добавляем женские команды в группу
                result.Add(WomanTeams);
            }

            // Возвращаем восстановленную группу
            return result;
        }



        // Blue5
        public override void SerializeBlue5Team<T>(T group, string fileName)  // where T : Blue_5.Team;
        {
            // string Name
            // Sportsman[] Sportsmen   Name, Surname, Place
            // manteam
            // womanteam
            var team = group as Blue_5.Team;
            if (team == null || String.IsNullOrEmpty(fileName)) return;
            string text = "";
            if (team is Blue_5.WomanTeam) text += "WomanTeam";
            else if (team is Blue_5.ManTeam) text += "ManTeam";

            text += $"{Environment.NewLine}{team.Name}{Environment.NewLine}";
            foreach (var sportsman in team.Sportsmen)
            {
                if (sportsman == null) break;
                text += $"{sportsman.Name} {sportsman.Surname} {sportsman.Place} {Environment.NewLine}";
            }
            SelectFile(fileName);
            File.WriteAllText(fileName, text);

        }


        public override T DeserializeBlue5Team<T>(string fileName) // where T : Blue_5.Team;
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            SelectFile(fileName); // предполагается, что FilePath задаётся в этом методе
            if (!File.Exists(FilePath)) return null;

            string[] lines = File.ReadAllLines(fileName);
            if (lines.Length < 2) return null;

            string typeLine = lines[0].Trim();
            string teamName = lines[1].Trim();

            Blue_5.Team team;

            // определение типа команды
            if (typeLine == "WomanTeam")
                team = new Blue_5.WomanTeam(teamName);
            else if (typeLine == "ManTeam")
                team = new Blue_5.ManTeam(teamName);
            else
                return null;

            // считываем спортсменов начиная с 3-й строки
            for (int i = 2; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 3) continue;

                string name = parts[0];
                string surname = parts[1];
                int place;

                if (!int.TryParse(parts[2], out place)) continue;

                var sportsman = new Blue_5.Sportsman(name, surname);
                sportsman.SetPlace(place);
                team.Add(sportsman);
            }

            return team as T;

        }



    }
}
