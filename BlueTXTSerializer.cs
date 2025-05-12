using Lab_7;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
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
            SelectFile(fileName); // Устанавливаем путь к файлу (автоматически создает папку/файл, если их нет)
            using (StreamWriter writer = new StreamWriter(FilePath)) //открываем файл для записи
            {
                if (participant is Blue_1.HumanResponse human)
                {
                    // Сериализация HumanResponse
                    writer.WriteLine("HumanResponse"); // Записываем тип объекта
                    writer.WriteLine(human.Name);// Записываем имя
                    writer.WriteLine(human.Surname); // Записываем фамилию
                    writer.WriteLine(human.Votes); // Записываем голоса
                }
                else
                {
                    // Сериализация обычного Response
                    writer.WriteLine("Response");
                    writer.WriteLine(participant.Name);
                    writer.WriteLine(participant.Votes);
                }
            }
        }
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(participant.GetType().Name);
                writer.WriteLine(participant.Name);
                writer.WriteLine(participant.Bank);
                foreach(var p in participant.Participants)
                {
                    writer.WriteLine(p.Name);
                    writer.WriteLine(p.Surname);
                    var marks = p.Marks;
                    for(int i = 0;i < 2; i++)
                    {
                      
                        for(int j = 0;j < 5; j++)
                        {
                            writer.Write(marks[i, j] + " ");
                        }
                        writer.WriteLine();
                    }
                }
            }

        }
        public override void SerializeBlue3Participant<T>(T student, string fileName) //where T : Blue_3.Participant
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer .WriteLine(student.GetType().Name);
                writer .WriteLine(student.Name);
                writer.WriteLine(student.Surname);
                if (student.Penalties != null && student.Penalties.Length > 0)
                {
                    var penalties = student.Penalties; // Получаем массив штрафов 
                    writer.WriteLine(string.Join(" ", penalties)); //все штрафы в одну строку через пробел
                }
                else writer.WriteLine("");
            }
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(participant.Name);
                for (int i = 0; i < participant.ManTeams.Length; i++)
                {
                    if (participant.ManTeams[i] != null)
                    {
                        writer.WriteLine("ManTeam");
                        writer.WriteLine(participant.ManTeams[i].Name);
                        writer.WriteLine(string.Join(" ", participant.ManTeams[i].Scores));
                    }
                }
                for (int i = 0; i < participant.WomanTeams.Length; i++)
                {
                    if (participant.WomanTeams[i] != null)
                    {
                        writer.WriteLine("WomanTeam");
                        writer.WriteLine(participant.WomanTeams[i].Name);
                        writer.WriteLine(string.Join(" ", participant.WomanTeams[i].Scores));
                    }
                }
            }
        }
        public override void SerializeBlue5Team<T>(T group, string fileName) //where T : Blue_5.Team
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer .WriteLine(group.GetType().Name);
                writer.WriteLine(group.Name);
                for (int i = 0; i < group.Sportsmen.Length; i++)
                {
                    if (group.Sportsmen[i] != null)
                    {
                        writer.WriteLine(group.Sportsmen[i].Name);
                        writer.WriteLine(group.Sportsmen[i].Surname);
                        writer.WriteLine(group.Sportsmen[i].Place);
                    }
                }
            }
        }

        // Методы десериализации
        //Методы получают первым параметром имя файла,
        //который находится в папке по пути FolderPath,
        //и возвращают сериализованный объект типа из соответствующего формата,
        //который хранится в файле.

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            // Прочитать файл, разбив его на строки
            using(StreamReader reader = new StreamReader(FilePath))
            {
                string objType = reader.ReadLine();
                if (objType == null) return null;
                if (objType == "HumanResponse")
                {
                    string Name = reader.ReadLine();
                    string Surname = reader.ReadLine();
                    int Votes = int.Parse(reader.ReadLine());
                    return new Blue_1.HumanResponse(Name, Surname, Votes);
                }
                else
                {
                    string Name = reader.ReadLine();
                    int Votes = int.Parse(reader.ReadLine());
                    return new Blue_1.Response(Name, Votes);
                }
            }
        }
    
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string objType = reader.ReadLine();
                string name = reader.ReadLine();
                int bank = int.Parse(reader.ReadLine());
                Blue_2.WaterJump waterJump;
                if(objType == nameof(Blue_2.WaterJump3m))
                {
                    waterJump = new Blue_2.WaterJump3m(name, bank);
                }
                else
                {
                    waterJump = new Blue_2.WaterJump5m(name, bank);
                }
                while (!reader.EndOfStream)
                {
                    
                    string participntName = reader.ReadLine();
                    if (string.IsNullOrEmpty(participntName)) break;
                    string participantSurname = reader.ReadLine();
                    var participant = new Blue_2.Participant(participntName, participantSurname);

                    string[] firstJumpMarks = reader.ReadLine().Split(' ');  // "5 6 7 8 9" → ["5","6","7","8","9"]
                    int[] firstJump = new int[5];
                    for (int i = 0; i < 5; i++)
                        firstJump[i] = int.Parse(firstJumpMarks[i]);// Преобразует строки в числа [5,6,7,8,9]

                    // второй прыжок
                    string[] secondJumpMarks = reader.ReadLine().Split(' ');
                    int[] secondJump = new int[5];
                    for (int i = 0; i < 5; i++)
                        secondJump[i] = int.Parse(secondJumpMarks[i]);

                    participant.Jump(firstJump);
                    participant.Jump(secondJump);

                    waterJump.Add(participant); //метод класса WaterJump, добавляющий участника в список
                }

                return waterJump;
            }
            
        }
        public override T DeserializeBlue3Participant<T>(string fileName) //where T : Blue_3.Participant
        {
            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string objType = reader.ReadLine();
                string name = reader.ReadLine();
                string surname = reader.ReadLine();
                Blue_3.Participant participants;
                if (objType == "BasketballPlayer") participants = new Blue_3.BasketballPlayer(name, surname);
                else if (objType == "HockeyPlayer") participants = new Blue_3.HockeyPlayer(name, surname);
                else participants = new Blue_3.Participant(name, surname);

                string penaltiesLine = reader.ReadLine();
                if (!string.IsNullOrEmpty(penaltiesLine))
                {
                    string[] penalty = penaltiesLine.Split(' ');
                    foreach (var p in penalty)
                    {
                        if (int.TryParse(p, out int result))
                        {
                            participants.PlayMatch(result);
                        }
                    }
                }

                return (T)participants; //Приводит созданный объект к требуемому generic-типу T
            }
        }
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string groupName = reader.ReadLine();
                Blue_4.Group group = new Blue_4.Group(groupName);
                while (!reader.EndOfStream)
                {
                    string type = reader.ReadLine();
                    string teamName = reader.ReadLine();
                    string scores = reader.ReadLine();

                    if (type == null || teamName == null) continue;

                    Blue_4.Team team;

                    // Создаем команду нужного типа
                    if (type == "ManTeam")
                        team = new Blue_4.ManTeam(teamName);
                    else if (type == "WomanTeam")
                        team = new Blue_4.WomanTeam(teamName);
                    else
                        continue; // Пропускаем неизвестные типы

                    // Добавляем очки (если они есть)
                    if (!string.IsNullOrEmpty(scores))
                    {
                        foreach (var s in scores.Split(' '))
                        {
                            if (int.TryParse(s, out int score))
                                team.PlayMatch(score);
                        }
                    }
                    // Добавляем команду в группу
                    group.Add(team);
                }
                return group;
            }
        }
        public override T DeserializeBlue5Team<T>(string fileName) //where T : Blue_5.Team
        {
            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string type = reader.ReadLine();
                string teamName = reader.ReadLine();
                Blue_5.Team team;
                if (type == "ManTeam")
                {
                    team = new Blue_5.ManTeam(teamName);
                }
                else if(type == "WomanTeam")
                    team = new Blue_5.WomanTeam(teamName);
                else
                    return default(T);
                while (!reader.EndOfStream)
                {
                    string name = reader.ReadLine();
                    string surname = reader.ReadLine();
                    string place = reader.ReadLine();

                    if (name == null || surname == null || place == null)
                        continue;

                    if (int.TryParse(place, out int p))
                    {
                        Blue_5.Sportsman sportsman = new Blue_5.Sportsman(name, surname);
                        if (p > 0)
                        {
                            sportsman.SetPlace(p);
                        }
                        team.Add(sportsman);
                    }
                }

                return (T)(object)team;
            }
        }
    }
}
