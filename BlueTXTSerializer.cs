using Lab_7;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension
        {
            get { return "txt"; }
        }
        //Blue_1
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string text;
            if (participant.GetType() == typeof(Blue_1.HumanResponse))
            {
                var human = participant as Blue_1.HumanResponse;
                text = $"{human.Name} {human.Surname} {human.Votes}";
            }
            else
            {
                text = $"{participant.Name} {participant.Votes}";
            }
            File.WriteAllText(fullPath, text);
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (!File.Exists(fullPath)) return null;
            var text = File.ReadAllText(fullPath);
            if (String.IsNullOrEmpty(text)) return null;
            var temp = text.Split(' ');
            if (temp.Length == 2)
            {
                return new Blue_1.Response(temp[0], Int32.Parse(temp[1]));
            }
            else if (temp.Length == 3)
            {
                return new Blue_1.HumanResponse(temp[0], temp[1], Int32.Parse(temp[2]));
            }
            return null;
        }
        //Blue_2
        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            string text;
            if (participant.GetType() == typeof(Blue_2.WaterJump3m)) text = $"water3m{Environment.NewLine}";
            else if (participant.GetType() == typeof(Blue_2.WaterJump5m)) text = $"water5m{Environment.NewLine}";
            else return;
            text += $"{participant.Name} {participant.Bank}{Environment.NewLine}";
            if (participant.Participants != null && participant.Participants.Length > 0)
            {
                for (int i = 0; i < participant.Participants.Length; i++)
                {
                    Blue_2.Participant participantI = participant.Participants[i];
                    if (participantI.Marks == null) continue;
                    text += $"{participantI.Name} {participantI.Surname}";
                    int jumps = Math.Min(2, participantI.Marks.GetLength(0));
                    int judges = Math.Min(5, participantI.Marks.GetLength(1));
                    for (int j = 0; j < jumps; j++)
                    {
                        for (int k = 0; k < judges; k++)
                        {
                            text += " " + participantI.Marks[j, k];
                        }
                    }
                    text += Environment.NewLine;
                }
            }
            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(text)) return null;
            var temp = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length < 2) return null;
            var tempNameBank = temp[1].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (tempNameBank.Length < 2) return null;
            Blue_2.WaterJump result;
            if (temp[0] == "water3m") result = new Blue_2.WaterJump3m(tempNameBank[0], Int32.Parse(tempNameBank[1]));
            else if (temp[0] == "water5m") result = new Blue_2.WaterJump5m(tempNameBank[0], Int32.Parse(tempNameBank[1]));
            else return null;
            for (int i = 2; i < temp.Length; i++)
            {
                var tempParticipant = temp[i].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (tempParticipant.Length < 12) continue;
                bool allScoresValid = true;
                for (int k = 2; k < 12; k++)
                {
                    if (!int.TryParse(tempParticipant[k], out _))
                    {
                        allScoresValid = false;
                        break;
                    }
                }
                if (!allScoresValid) continue;
                Blue_2.Participant participant = new Blue_2.Participant(tempParticipant[0], tempParticipant[1]);
                int[] jump1 = new int[5];
                int[] jump2 = new int[5];
                for (int k = 0; k < 5; k++)
                {
                    jump1[k] = Int32.Parse(tempParticipant[2 + k]);
                }
                for (int k = 0; k < 5; k++)
                {
                    jump2[k] = Int32.Parse(tempParticipant[7 + k]);
                }
                participant.Jump(jump1);
                participant.Jump(jump2);
                result.Add(participant);
            }

            return result;
        }
        //Blue_3
        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            Console.WriteLine("CCCC");
            Console.WriteLine(student.Name, student.Surname, string.Join(" ", student.Penalties));
            if (student == null || String.IsNullOrEmpty(fileName)) return;
            string text;
            if (student.GetType() == typeof(Blue_3.BasketballPlayer))
            {
                text = "Basketball";
            }
            else if (student.GetType() == typeof(Blue_3.HockeyPlayer))
            {
                text = "Hockey";
            }
            else if (student.GetType() == typeof(Blue_3.Participant))
            {
                text = "Participant";
            }
            else return;
            if (student.Surname != null && student.Name != null)
            {
                text += $"{Environment.NewLine}{student.Name} {student.Surname}";
            }
            else
            {
                return;
            }
            if (student.Penalties != null && student.Penalties.Length > 0)
            {
                text += Environment.NewLine;
                for (int i = 0; i < student.Penalties.Length; i++)
                {
                    text += $"{student.Penalties[i]} ";
                }
            }
            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }

        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            if (!File.Exists(FilePath)) return null;
            string text = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(text)) return null;
            var temp = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length < 2) return null;
            var tempNameSurname = temp[1].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (tempNameSurname.Length < 2) return null;
            Blue_3.Participant t;
            if (temp[0] == "Basketball") t = new Blue_3.BasketballPlayer(tempNameSurname[0], tempNameSurname[1]);
            else if (temp[0] == "Hockey") t = new Blue_3.HockeyPlayer(tempNameSurname[0], tempNameSurname[1]);
            else if (temp[0] == "Participant") t = new Blue_3.Participant(tempNameSurname[0], tempNameSurname[1]);
            else
            {
                return null;
            }
            if (temp.Length > 2 && t != null)
            {
                var tempPenalties = temp[2].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var penalty in tempPenalties)
                {
                    if (int.TryParse(penalty, out int penaltyValue))
                    {
                        t.PlayMatch(penaltyValue);
                    }
                }
            }
            return (T)t;
        }

        //Blue_4
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) return;
            string text = participant.Name + Environment.NewLine;
            if (participant.ManTeams != null)
            {
                for (int i = 0; i < participant.ManTeams.Length; i++)
                {
                    if (participant.ManTeams[i] == null) continue;
                    text += $"{participant.ManTeams[i].Name} ";
                    for (int j = 0; j < participant.ManTeams[i].Scores.Length; j++)
                    {
                        text += $"{participant.ManTeams[i].Scores[j]} ";
                    }
                    text += "man";
                }
            }
            text += Environment.NewLine;
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
            string text = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(text)) return null;
            var temp = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length < 1) return null;  // проверка минимальной длины
            Blue_4.Group result = new Blue_4.Group(temp[0]);
            // Обработка мужских команд
            if (temp.Length > 1)
            {
                string[] manTeams = temp[1].Split(new[] { "man" }, StringSplitOptions.RemoveEmptyEntries);
                Blue_4.ManTeam[] ManTeams = new Blue_4.ManTeam[manTeams.Length];
                for (int i = 0; i < ManTeams.Length; i++)
                {
                    var tempNameScores = manTeams[i].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tempNameScores.Length == 0) continue;  // Пропускаю пустые записи
                    ManTeams[i] = new Blue_4.ManTeam(tempNameScores[0]);
                    for (int j = 1; j < tempNameScores.Length; j++)
                    {
                        ManTeams[i].PlayMatch(Int32.Parse(tempNameScores[j]));
                    }
                }
                result.Add(ManTeams);
            }
            // Обработка женских команд
            if (temp.Length > 2)
            {
                string[] womanTeams = temp[2].Split(new[] { "woman" }, StringSplitOptions.RemoveEmptyEntries);
                Blue_4.WomanTeam[] WomanTeams = new Blue_4.WomanTeam[womanTeams.Length];
                for (int i = 0; i < WomanTeams.Length; i++)
                {
                    var tempNameScores = womanTeams[i].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tempNameScores.Length == 0) continue;
                    WomanTeams[i] = new Blue_4.WomanTeam(tempNameScores[0]);
                    for (int j = 1; j < tempNameScores.Length; j++)
                    {
                        WomanTeams[i].PlayMatch(Int32.Parse(tempNameScores[j]));
                    }
                }
                result.Add(WomanTeams);
            }
            return result;
        }
        //Blue_5
        public override void SerializeBlue5Team<T>(T group, string fileName) // where T : Blue_5.Team 
        {
            if (group == null || String.IsNullOrEmpty(fileName)) return;
            string text;
            if (group.GetType() == typeof(Blue_5.ManTeam))
            {
                text = "man";
            }
            else if (group.GetType() == typeof(Blue_5.WomanTeam))
            {
                text = "woman";
            }
            else return;
            text += Environment.NewLine + group.Name + Environment.NewLine;
            if (group.Sportsmen != null)
            {
                for (int i = 0; i < group.Sportsmen.Length; i++)
                {
                    if (group.Sportsmen[i] == null) continue;
                    text += $"{group.Sportsmen[i].Name} {group.Sportsmen[i].Surname} {group.Sportsmen[i].Place}{Environment.NewLine}";
                }
            }
            SelectFile(fileName);
            File.WriteAllText(FilePath, text);
        }
        public override T DeserializeBlue5Team<T>(string fileName) //where T : Blue_5.Team
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            if (String.IsNullOrEmpty(text)) return null;
            var temp = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length < 2) return null;
            Blue_5.Team tempTeam;
            T result;
            if (temp[0] == "man") tempTeam = new Blue_5.ManTeam(temp[1]);
            else if (temp[0] == "woman") tempTeam = new Blue_5.WomanTeam(temp[1]);
            else return null;
            result = tempTeam as T;
            if (result == null) return null;
            if (temp.Length == 2) return result;
            for (int i = 2; i < temp.Length; i++)
            {
                var tempS = temp[i].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (tempS.Length < 2) continue; 
                var tempSportsman = new Blue_5.Sportsman(tempS[0], tempS[1]);
                if (tempS.Length == 2) continue;
                for (int j = 2; j < tempS.Length; j++)
                {
                    tempSportsman.SetPlace(Int32.Parse(tempS[j]));
                }
                result.Add(tempSportsman);
            }
            return result;
        }
    }
}
