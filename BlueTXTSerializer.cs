using Lab_7;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;
using static Lab_7.Blue_2;
using static System.Formats.Asn1.AsnWriter;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension => "txt";

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName))
            {
                return;
            }
            SelectFile(fileName); //устанавливает путь к файлу, выбирает подходящий
            using (StreamWriter writer = new StreamWriter(fileName)) //класс, который записывает
                                                                     //текстовые данные в поток, открывает файл
            {
                if (participant is Blue_1.HumanResponse human)
                {
                    writer.WriteLine("HumanResponse"); //тип объекта
                    writer.WriteLine(human.Name);    //данные объекта
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

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(fileName))
            {
                string Type = reader.ReadLine();
                if (Type == "HumanResponse")
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
        }
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);

            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(participant.GetType().Name); //получаем тип 5м или 3м
                writer.WriteLine(participant.Name); //имя объекта
                writer.WriteLine(participant.Bank);
                foreach (var part in participant.Participants) //для каждого участника
                {
                    writer.WriteLine(part.Name);
                    writer.WriteLine(part.Surname);
                    int[,] marks = part.Marks;
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            writer.Write(marks[i, j]);
                            if (j < 4) writer.Write(" ");
                        }
                        writer.WriteLine(); //на новую строку 
                    }
                }
            }
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string Type = reader.ReadLine();
                string name = reader.ReadLine();
                int bank = int.Parse(reader.ReadLine());
                Blue_2.WaterJump waterJump;
                if (Type == nameof(Blue_2.WaterJump3m))
                {
                    waterJump = new Blue_2.WaterJump3m(name, bank);
                }
                else
                {
                    waterJump = new Blue_2.WaterJump5m(name, bank);
                }
                while (!reader.EndOfStream)
                {
                    string Pname = reader.ReadLine();
                    if (string.IsNullOrEmpty(Pname)) break;
                    string Psurname = reader.ReadLine();
                    var participant = new Blue_2.Participant(Pname, Psurname);

                    ReadAndProcessJumps(reader, participant);
                    waterJump.Add(participant);
                }
                return waterJump;
            }

        }
        private void ReadAndProcessJumps(StreamReader reader, Blue_2.Participant participant)
        {
            string[] firstJumpMarks = reader.ReadLine().Split(' ');
            int[] firstJump = new int[5];
            for (int i = 0; i < 5; i++)
            {
                firstJump[i] = int.Parse(firstJumpMarks[i]);
            }
            string[] secondJumpMarks = reader.ReadLine().Split(' ');
            int[] secondJump = new int[5];
            for (int i = 0; i < 5; i++)
            {
                secondJump[i] = int.Parse(secondJumpMarks[i]);
            }
            participant.Jump(firstJump);
            participant.Jump(secondJump);
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName) //T:Blue_3.Participant
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(student.GetType().Name);
                writer.WriteLine(student.Name);
                writer.WriteLine(student.Surname);
                string penalties = student.Penalties != null && student.Penalties.Length > 0 ?
                    string.Join(" ", student.Penalties) : string.Empty;
                writer.WriteLine(penalties);
            }
        }

        public override T DeserializeBlue3Participant<T>(string fileName) //T: Blue_3.Participant
        {
            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string Type = reader.ReadLine();
                string name = reader.ReadLine();
                string surname = reader.ReadLine();
                Blue_3.Participant participants;
                if (Type == "BasketballPlayer") participants = new Blue_3.BasketballPlayer(name, surname);
                else if (Type == "HockeyPlayer") participants = new Blue_3.HockeyPlayer(name, surname);
                else participants = new Blue_3.Participant(name, surname);
                ReadPenalties(reader, participants);
                return (T)participants;
            }
        }
        private void ReadPenalties(StreamReader reader, Blue_3.Participant participant)
        {
            var penalties = reader.ReadLine();
            if (!string.IsNullOrEmpty(penalties))
            {
                foreach (var penalty in penalties.Split(' '))
                {
                    if (int.TryParse(penalty, out var result))
                    {
                        participant.PlayMatch(result);
                    }
                }
            }
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(participant.Name);
                SerializeTeams(writer, participant.ManTeams, "ManTeam");
                SerializeTeams(writer, participant.WomanTeams, "WomanTeam");
            }
        }
        private void SerializeTeams(StreamWriter writer, Blue_4.Team[] teams, string teamType)
        {
            foreach (var team in teams)
            {
                if (team != null)
                {
                    writer.WriteLine(teamType);
                    writer.WriteLine(team.Name);
                    string scores = team.Scores != null ? string.Join(" ", team.Scores) : String.Empty;
                    writer.WriteLine(scores);
                }
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
                    if (type == "ManTeam")
                    {
                        team = new Blue_4.ManTeam(teamName);
                    }
                    else if (type == "WomanTeam")
                    {
                        team = new Blue_4.WomanTeam(teamName);
                    }
                    else
                    {
                        continue;
                    }
                    if (!string.IsNullOrEmpty(scores))
                    {
                        foreach (var sc in scores.Split(' '))
                        {
                            if (int.TryParse(sc, out int score))
                            {
                                team.PlayMatch(score);
                            }
                        }
                    }
                    group.Add(team);
                }
                return group;
            }
        }

        public override void SerializeBlue5Team<T>(T group, string fileName) //T: Blue_5.Team
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(group.GetType().Name);
                writer.WriteLine(group.Name);
                foreach (var spor in group.Sportsmen)
                {
                    if (spor != null)
                    {
                        writer.WriteLine(spor.Name);
                        writer.WriteLine(spor.Surname);
                        writer.WriteLine(spor.Place);
                    }
                }
            }
        }
        public override T DeserializeBlue5Team<T>(string fileName) //T: Blue_5.Team
        {
            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(fileName))
            {
                Blue_5.Team team;
                string type = reader.ReadLine();
                string name = reader.ReadLine();
                if (type == "ManTeam")
                {
                    team = new Blue_5.ManTeam(name);
                }
                else if (type == "WomanTeam")
                {
                    team = new Blue_5.WomanTeam(name);
                }
                else { return default(T); }
                int.TryParse(reader.ReadLine(), out int countParticipant);
                for (int i = 0; i < countParticipant; i++)
                {
                    if (reader.EndOfStream)
                    {
                        break;
                    }
                    string Pname = reader.ReadLine();
                    string Psurname = reader.ReadLine();
                    int.TryParse(reader.ReadLine(), out int place);
                    Blue_5.Sportsman player = new Blue_5.Sportsman(Pname, Psurname);
                    player.SetPlace(place);
                    team.Add(player);
                }
                return (T)team;
            }

        }
    }
}
