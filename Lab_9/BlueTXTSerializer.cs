using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Lab_7.Blue_2;
using static Lab_7.Blue_3;
using static Lab_7.Blue_4;
using static Lab_7.Blue_5;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension => "txt";
        //Реализовать абстрактные методы класса-родителя
        //так, чтобы они сериализовывали/десериализовывали
        //объекты в формате txt.
        // При сериализации сохранять только публичные
        // нестатические свойства объекта! При десериализации
        // использовать имеющийся в классе конструктор и
        // методы для заполнения данных объекта аналогично
        // созданию нового объекта.Значения свойств
        // десериализованного объекта должны полностью
        // совпадать со значениями свойств базового объекта.





        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName))
                return;

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                if (participant is Blue_1.HumanResponse human)
                {
                    writer.WriteLine("HumanResponse");
                    writer.WriteLine(human.Name);
                    writer.WriteLine(human.Surname);
                    writer.WriteLine(human.Votes);
                }
                else
                {
                    writer.WriteLine("Response");
                    writer.WriteLine(participant.Name);
                    writer.WriteLine(participant.Votes);
                }
            }
        }
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine($"Type={participant.GetType().Name}");
                writer.WriteLine($"Name={participant.Name}");
                writer.WriteLine($"Bank={participant.Bank}");
                foreach (var player in participant.Participants)
                {
                    writer.WriteLine("ParticipantStart");
                    writer.WriteLine($"Name={player.Name}");
                    writer.WriteLine($"Surname={player.Surname}");
                    string firstJump = "";
                    for (int i = 0; i < 5; i++)
                        firstJump += (i > 0 ? "," : "") + player.Marks[0, i];
                    writer.WriteLine("firstJump");
                    writer.WriteLine(firstJump);
                    string secondJump = "";
                    for (int i = 0; i < 5; i++)
                        secondJump += (i > 0 ? "," : "") + player.Marks[1, i];
                    writer.WriteLine("secondJump");
                    writer.WriteLine(secondJump);
                    writer.WriteLine("ParticipantEnd");
                }
            }
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;

            using (StreamWriter writer =new StreamWriter(fileName))
            {
                writer.WriteLine(student.GetType().Name);
                writer.WriteLine(student.Name);
                writer.WriteLine(student.Surname);
                string penalty = "";
                for (int i = 0; i < student.Penalties.Length; i++)
                {
                    penalty += (i > 0 ? "," : "") + student.Penalties[i];
                }
                writer.WriteLine(penalty);
            }
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine(participant.Name);

                writer.WriteLine("Мужланы");
                foreach (var manteam in participant.ManTeams)
                {
                    if (manteam == null) continue;    //null reference exception
                    writer.WriteLine(manteam.Name);
                    string score = "";
                    for (int i = 0; i < manteam.Scores.Length; i++)
                    {
                        score += (i > 0 ? "," : "") + manteam.Scores[i];
                    }
                    writer.WriteLine(score);
                }
                writer.WriteLine("Девушки");
                foreach (var woman in participant.WomanTeams)
                {
                    if (woman == null) continue;
                    writer.WriteLine(woman.Name);
                    string score = "";
                    for (int i = 0; i < woman.Scores.Length; i++)
                    {
                        score += (i > 0 ? "," : "") + woman.Scores[i];
                    }
                    writer.WriteLine(score);
                }
            }
        }
        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine(group.GetType().Name); //ManTeam or WomanTeam
                writer.WriteLine(group.Name);
                writer.WriteLine(group.Sportsmen.Length);
                foreach (var player in group.Sportsmen)
                {
                    if (player == null) continue;
                    writer.WriteLine(player.Name);
                    writer.WriteLine(player.Surname);
                    writer.WriteLine(player.Place);
                }
            }
        }




        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string type = reader.ReadLine();
                if (type == "HumanResponse")
                {
                    string name = reader.ReadLine();
                    string surname = reader.ReadLine();
                    int votes = int.Parse(reader.ReadLine());
                    return new Blue_1.HumanResponse(name, surname, votes);
                }
                else 
                {
                    string name = reader.ReadLine();
                    int votes = int.Parse(reader.ReadLine());
                    return new Blue_1.Response(name, votes);
                }
            }
            //string[] lines = File.ReadAllLines(fileName);
            //Dictionary<string, string> properties = new Dictionary<string, string>();
            //foreach (string line in lines)
            //{
            //    if (line.Contains('='))
            //    {
            //        var parts = line.Split('=');
            //        properties[parts[0].Trim()] = parts[1].Trim();
            //    }
            //}
            //if (properties.ContainsKey("Surname"))
            //{
            //    return new Blue_1.HumanResponse(
            //        properties["Name"],
            //        properties["Surname"],
            //        int.Parse(properties["Votes"]));
            //}
            //else
            //{
            //    return new Blue_1.Response(
            //        properties["Name"],
            //        int.Parse(properties["Votes"]));
            //}
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string typeLine = reader.ReadLine();
                string nameLine = reader.ReadLine();
                string bankLine = reader.ReadLine();

                string type = typeLine.Split('=')[1];
                string name = nameLine.Split('=')[1];
                int.TryParse(bankLine.Split('=')[1], out int bank);


                Blue_2.WaterJump waterJump;
                if (type == "WaterJump3m")
                {
                    waterJump = new Blue_2.WaterJump3m(name, bank);
                }
                else
                    waterJump = new Blue_2.WaterJump5m(name, bank);

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line != "ParticipantStart") continue;
                    string pName = reader.ReadLine()?.Split('=')[1];
                    string pSurname = reader.ReadLine()?.Split('=')[1];

                    if (reader.ReadLine() != "firstJump") continue;
                    string firstJumpString = reader.ReadLine();

                    if (reader.ReadLine() != "secondJump") continue;
                    string secondJumpString = reader.ReadLine();

                    if (reader.ReadLine() != "ParticipantEnd") continue;

                    //участники
                    var participant = new Blue_2.Participant(pName, pSurname);
                    int[] firstJump = Array.ConvertAll(firstJumpString.Split(','), int.Parse);
                    int[] secondJump = Array.ConvertAll(secondJumpString.Split(','), int.Parse);
                    participant.Jump(firstJump);
                    participant.Jump(secondJump);

                    waterJump.Add(participant);


                }
                return waterJump;
                //int.TryParse(reader.ReadLine(), out int countParticipants);
                //for (int i = 0; i < countParticipants; i++)
                //{
                //    string participantName = reader.ReadLine();
                //    string participantSurname = reader.ReadLine();
                //    var participant = new Blue_2.Participant(participantName, participantSurname);
                //    int[] firstJump = new int[5];
                //    for (int j = 0; j < 5; j++)
                //    {
                //        int.TryParse(reader.ReadLine(), out firstJump[j]);
                //    }
                //    int[] secondJump = new int[5];
                //    for (int j = 0; j < 5; j++)
                //    {
                //        int.TryParse(reader.ReadLine(), out secondJump[j]);
                //    }
                //    participant.Jump(firstJump);
                //    participant.Jump(secondJump);
                //    waterJump.Add(participant);
                //}
                //return waterJump;
            }
            //if (!File.Exists(fileName)) return null;
            //string[] lines = File.ReadAllLines(fileName);
            //string type = "";
            //string name = "";
            //int bank = 0;
            //var participants = new List<Blue_2.Participant>();

            //foreach (var line in lines)
            //{
            //    if (line.StartsWith("Type=")) //(line?.StartsWith("Type=") == true)
            //    {
            //        type = line.Substring("Type=".Length);
            //    }
            //    else if (line.StartsWith("Name="))
            //    {
            //        name = line.Substring("Name=".Length);
            //    }
            //    else if (line.StartsWith("Bank="))
            //    {
            //        bank = int.Parse(line.Substring("Bank=".Length));
            //    }
            //    else if (line == "ParticipantStart")
            //    {
            //        break; // Переходим к обработке участников
            //    }
            //}

            //Blue_2.WaterJump waterjump;
            //if (type == "WaterJump3m")
            //    waterjump = new Blue_2.WaterJump3m(name, bank);
            //else
            //    waterjump = new Blue_2.WaterJump5m(name, bank);
            ////обработка участников
            //for (int i = 0; i < lines.Length; i++)
            //{
            //    if (lines[i] == "ParticipantStart")
            //    {
            //        // данные участника
            //        string partisName = lines[++i].Substring("Name=".Length);
            //        string partisSurname = lines[++i].Substring("Surname=".Length);
            //        var participant = new Blue_2.Participant(partisName, partisSurname);
            //        //marks
            //        int[,] marks = new int[2, 5];
            //        for (int jump = 0; jump < 2; jump++)
            //        {
            //            string[] value = lines[++i].Substring($"Jump{jump + 1}=".Length).Split(',');
            //            //1 or 2
            //            for (int j = 0; j < 5; j++)
            //                marks[jump, j] = int.Parse(value[j]);
            //        }
            //        participant.Jump(new[] { marks[0, 0], marks[0, 1], marks[0, 2], marks[0, 3], marks[0, 4] });
            //        participant.Jump(new[] { marks[1, 0], marks[1, 1], marks[1, 2], marks[1, 3], marks[1, 4] });
            //        waterjump.Add(participant);
            //    }
            //}
            //return waterjump;
        }

        public override T DeserializeBlue3Participant<T>(string fileName)// where T : Blue_3.Participant
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string type = reader.ReadLine();
                string name = reader.ReadLine();
                string surname = reader.ReadLine();

                Blue_3.Participant participant;
                if (type == "HockeyPlayer")
                    participant = new Blue_3.HockeyPlayer(name, surname);
                else if (type == "BasketballPlayer")
                    participant = new Blue_3.BasketballPlayer(name, surname);
                else 
                    participant = new Blue_3.Participant(name, surname);

                //penalty
                string penaltyString = reader.ReadLine();
                int[] penalty = Array.ConvertAll(penaltyString.Split(','), int.Parse);
                foreach (var penalt in penalty)
                {
                    participant.PlayMatch(penalt);
                }
                return (T)(object)participant;

            }


            //var lines = File.ReadAllLines(fileName);
            //Dictionary<string, string> properties = new Dictionary<string, string>();
            //string type = "";
            //string name = "";
            //string surname = "";
            //List<int> penalties = new List<int>();
            //bool readingPenalties = false;

            //foreach (var line in lines)
            //{
            //    if (line.StartsWith("Type="))
            //    {
            //        type = line.Substring("Type=".Length);
            //    }
            //    else if (line.StartsWith("Name="))
            //    {
            //        name = line.Substring("Name=".Length);
            //    }
            //    else if (line.StartsWith("Surname="))
            //    {
            //        surname = line.Substring("Surname=".Length);
            //    }
            //    else if (line == "Penalties:")
            //    {
            //        readingPenalties = true;
            //    }
            //    else if (readingPenalties)
            //    {
            //        if (int.TryParse(line, out int penalty))
            //        {
            //            penalties.Add(penalty);
            //        }
            //        else
            //        {
            //            readingPenalties = false; // Завершаем чтение штрафов при ошибке
            //        }
            //    }
            //}

            //Blue_3.Participant participant;
            //if (type == "BasketballPlayer")
            //    participant = new Blue_3.BasketballPlayer(name, surname);
            //else if (type == "HockeyPlayer")
            //    participant = new Blue_3.HockeyPlayer(name, surname);
            //else
            //    participant = new Blue_3.Participant(name, surname);
            ////penalty
            //foreach (var penalty in penalties)
            //{
            //    participant.PlayMatch(penalty);
            //}
            //return (T)(object)participant;
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string nameGroup = reader.ReadLine();
                //if (nameGroup == null) return null;
                Blue_4.Group group = new Blue_4.Group(nameGroup);
                bool readingMen = false;
                bool readingWomen = false;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line == "Мужланы")
                    {
                        readingMen = true;
                        readingWomen = false;
                        continue;
                    }
                    else if (line == "Девушки")
                    {
                        readingMen = false;
                        readingWomen = true;
                        continue;
                    }

                    if (readingMen)
                    {
                        string nameMan = line;
                        string scoreString = reader.ReadLine();
                        if (scoreString == null) continue;

                        int[] scores = Array.ConvertAll(scoreString.Split(','), int.Parse);
                        Blue_4.ManTeam manTeam = new Blue_4.ManTeam(nameMan);
                        foreach (var score in scores)
                        {
                            manTeam.PlayMatch(score);
                        }
                        group.Add(manTeam);
                    }
                    else if (readingWomen)
                    {
                        string nameWoman = line;
                        string scoreString = reader.ReadLine();
                        if (scoreString == null) continue;

                        int[] scores = Array.ConvertAll(scoreString.Split(','), int.Parse);
                        Blue_4.WomanTeam womanTeam = new Blue_4.WomanTeam(nameWoman);
                        foreach (var score in scores)
                        {
                            womanTeam.PlayMatch(score);
                        }
                        group.Add(womanTeam);
                    }
                }
                return group;
            }
        }

        public override T DeserializeBlue5Team<T>(string fileName) //where T : Blue_5.Team
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                Blue_5.Team team;
                string type = reader.ReadLine();
                string name = reader.ReadLine();
                if (type == "ManTeam")
                {
                    team = new Blue_5.ManTeam(name);
                }
                else 
                {
                    team = new Blue_5.WomanTeam(name);
                }

                int.TryParse(reader.ReadLine(), out int countParticipant);
                for (int i = 0; i < countParticipant; i++) //[6] их
                {
                    if (reader.EndOfStream)
                        break;
                    //Использование EndOfStream гарантирует, что даже если файл обрезан (например, содержит только 3 спортсмена), код не упадет с ошибкой, а корректно обработает те данные, которые есть.
                    string namePlayer = reader.ReadLine();
                    string surnamePlayer = reader.ReadLine();
                    int.TryParse(reader.ReadLine(), out int place);
                    Blue_5.Sportsman player = new Blue_5.Sportsman(namePlayer, surnamePlayer);
                    player.SetPlace(place);
                    team.Add(player);
                }
                return (T)team;    
            }
        }
    }
}
