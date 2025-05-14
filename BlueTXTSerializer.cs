using Lab_7;
using System;
using System.IO;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension => "txt";

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine($"Name={participant.Name}");
                if (participant is Blue_1.HumanResponse human)
                    writer.WriteLine($"Surname={human.Surname}");
                writer.WriteLine($"Votes={participant.Votes}");
            }
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                return null;

            string[] lines = File.ReadAllLines(fileName);
            string name = "";
            string surname = null;
            int votes = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Name="))
                    name = lines[i].Substring(5);
                else if (lines[i].StartsWith("Surname="))
                    surname = lines[i].Substring(8);
                else if (lines[i].StartsWith("Votes="))
                    votes = int.Parse(lines[i].Substring(6));
            }

            return surname != null
                ? new Blue_1.HumanResponse(name, surname, votes)
                : new Blue_1.Response(name, votes);
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                // Записываем основные свойства
                writer.WriteLine($"Type={participant.GetType().Name}");
                writer.WriteLine($"Name={participant.Name}");
                writer.WriteLine($"Bank={participant.Bank}");
                writer.WriteLine($"Participant`s_Count={participant.Participants.Length}"); 

                for (int i = 0; i < participant.Participants.Length; i++)
                {
                    var part = participant.Participants[i];
                    writer.WriteLine($"Participant`s_Name={part.Name}");
                    writer.WriteLine($"Participant`s_Surname={part.Surname}");

                    for (int j = 0; j < 2; j++) 
                    {
                        writer.Write($"Marks{j}=");
                        for (int k = 0; k < 5; k++) 
                        {
                            writer.Write(part.Marks[j, k]);
                            if (k < 4) writer.Write(",");
                        }
                        writer.WriteLine();
                    }
                }
            }
        }

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                return null;

            string[] lines = File.ReadAllLines(fileName);
            string type = "";
            string name = "";
            int bank = 0;
            int participantCount = 0;
            Blue_2.Participant[] participants = Array.Empty<Blue_2.Participant>();

            
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("Type="))
                        type = lines[i].Substring(5);
                    else if (lines[i].StartsWith("Name="))
                        name = lines[i].Substring(5);
                    else if (lines[i].StartsWith("Bank="))
                        bank = int.Parse(lines[i].Substring(5));
                    else if (lines[i].StartsWith("Participant`s_Count="))
                        participantCount = int.Parse(lines[i].Substring(20));
                }

                participants = new Blue_2.Participant[participantCount];
                int currentParticipant = 0;
                string currentName = "";
                string currentSurname = "";
            int[] firstJumpMarks = new int[5];
            int[] secondJumpMarks = new int[5];
            int currentJump = 0;

                for (int i = 0; i < lines.Length && currentParticipant < participantCount; i++)
                {
                    if (lines[i].StartsWith("Participant`s_Name="))
                    {
                        currentName = lines[i].Substring(19);
                    }
                    else if (lines[i].StartsWith("Participant`s_Surname="))
                    {
                        currentSurname = lines[i].Substring(22);
                    }
                else if (lines[i].StartsWith("Marks0="))
                {
                    string[] marks = lines[i].Substring(7).Split(',');
                    for (int j = 0; j < 5 && j < marks.Length; j++)
                    {
                        firstJumpMarks[j] = int.Parse(marks[j]);
                    }
                }
                else if (lines[i].StartsWith("Marks1="))
                {
                    string[] marks = lines[i].Substring(7).Split(',');
                    for (int j = 0; j < 5 && j < marks.Length; j++)
                    {
                        secondJumpMarks[j] = int.Parse(marks[j]);
                    }
                    var participant = new Blue_2.Participant(currentName, currentSurname);
                        participant.Jump(firstJumpMarks);
                    participant.Jump(secondJumpMarks);
                        participants[currentParticipant++] = participant;
                    }
                }

                Blue_2.WaterJump result = type == "WaterJump3m"
                    ? new Blue_2.WaterJump3m(name, bank)
                    : new Blue_2.WaterJump5m(name, bank);

                if (participants.Length > 0)
                {
                    result.Add(participants);
                }

                return result;
            
           
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine($"Name={student.Name}");
                writer.WriteLine($"Surname={student.Surname}");
                writer.WriteLine($"Sport={student.GetType().Name}");
                writer.WriteLine($"Count={student.Penalties.Length}");
                writer.WriteLine(string.Join(",", student.Penalties));
            }
        }

        
        public override T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                return default(T);

            string[] lines = File.ReadAllLines(fileName);
            string name = "";
            string surname = "";
            string type = "";
            int[] penalties = new int[0];

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Name="))
                    name = lines[i].Substring(5);
                else if (lines[i].StartsWith("Surname="))
                    surname = lines[i].Substring(8);
                else if (lines[i].StartsWith("Sport="))
                    type = lines[i].Substring(6);
                else if (lines[i].StartsWith("Count="))
                    penalties = new int[int.Parse(lines[i].Substring(6))];
                else if (lines[i].Contains(","))
                {
                    string[] values = lines[i].Split(',');
                    for (int j = 0; j < penalties.Length; j++)
                        penalties[j] = int.Parse(values[j]);
                }
            }

            object participant;
            switch (type)
            {
                case "BasketballPlayer":
                    participant = new Blue_3.BasketballPlayer(name, surname);
                    break;
                case "HockeyPlayer":
                    participant = new Blue_3.HockeyPlayer(name, surname);
                    break;
                default:
                    participant = new Blue_3.Participant(name, surname);
                    break;
            }

            for (int i = 0; i < penalties.Length; i++)
            {
                if (participant is Blue_3.Participant p)
                    p.PlayMatch(penalties[i]);
            }

            return (T)participant;
        }

        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine($"Name:{participant.Name}");
                writer.WriteLine($"NumberOfMensTeam:{participant.ManTeams.Length}");
                for (int i = 0; i < participant.ManTeams.Length; i++)
                {
                    if (participant.ManTeams[i] != null)
                        writer.WriteLine($"{participant.ManTeams[i].Name}|{string.Join(',', participant.ManTeams[i].Scores)}");
                }

                writer.WriteLine($"NumberOfWomensTeam:{participant.WomanTeams.Length}");
                for (int i = 0; i < participant.WomanTeams.Length; i++)
                {
                    if (participant.WomanTeams[i] != null)
                        writer.WriteLine($"{participant.WomanTeams[i].Name}|{string.Join(',', participant.WomanTeams[i].Scores)}");
                }
            }
        }

      
        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                return null;

            string[] lines = File.ReadAllLines(fileName);
            string name = "";
            Blue_4.ManTeam[] manTeams = new Blue_4.ManTeam[0];
            Blue_4.WomanTeam[] womanTeams = new Blue_4.WomanTeam[0];
            bool readingManTeams = false;
            bool readingWomanTeams = false;
            int manTeamCount = 0;
            int womanTeamCount = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("NumberOfMensTeam:"))
                    manTeamCount = int.Parse(lines[i].Substring(17));
                else if (lines[i].StartsWith("NumberOfWomensTeam:"))
                    womanTeamCount = int.Parse(lines[i].Substring(19));
            }

            manTeams = new Blue_4.ManTeam[manTeamCount];
            womanTeams = new Blue_4.WomanTeam[womanTeamCount];
            int manIndex = 0;
            int womanIndex = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Name:"))
                {
                    name = lines[i].Substring(5).Trim();
                }
                else if (lines[i].StartsWith("NumberOfMensTeam:"))
                {
                    readingManTeams = true;
                    readingWomanTeams = false;
                }
                else if (lines[i].StartsWith("NumberOfWomensTeam:"))
                {
                    readingManTeams = false;
                    readingWomanTeams = true;
                }
                else if (readingManTeams && lines[i].Contains("|") && manIndex < manTeams.Length)
                {
                    string[] parts = lines[i].Split('|');
                    var team = new Blue_4.ManTeam(parts[0]);
                    string[] scores = parts[1].Split(',');
                    for (int j = 0; j < scores.Length; j++)
                        team.PlayMatch(int.Parse(scores[j]));
                    manTeams[manIndex++] = team;
                }
                else if (readingWomanTeams && lines[i].Contains("|") && womanIndex < womanTeams.Length)
                {
                    string[] parts = lines[i].Split('|');
                    var team = new Blue_4.WomanTeam(parts[0]);
                    string[] scores = parts[1].Split(',');
                    for (int j = 0; j < scores.Length; j++)
                        team.PlayMatch(int.Parse(scores[j]));
                    womanTeams[womanIndex++] = team;
                }
            }

            Blue_4.Group group = new Blue_4.Group(name);
            group.Add(manTeams);
            group.Add(womanTeams);
            return group;
        }

       
        public override void SerializeBlue5Team<T>(T group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine($"Name={group.Name}");
                writer.WriteLine($"Type={group.GetType().Name}");

                int count = 0;
                for (int i = 0; i < group.Sportsmen.Length; i++)
                    if (group.Sportsmen[i] != null) count++;

                writer.WriteLine($"Count={count}");

                for (int i = 0; i < group.Sportsmen.Length; i++)
                {
                    if (group.Sportsmen[i] != null)
                        writer.WriteLine($"{group.Sportsmen[i].Name}|{group.Sportsmen[i].Surname}|{group.Sportsmen[i].Place}");
                }
            }
        }

      
        public override T DeserializeBlue5Team<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                return default(T);

            string[] lines = File.ReadAllLines(fileName);
            string name = "";
            string type = "";
            int count = 0;
            Blue_5.Sportsman[] sportsmen = new Blue_5.Sportsman[0];

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Count="))
                {
                    count = int.Parse(lines[i].Substring(6));
                    sportsmen = new Blue_5.Sportsman[count];
                    break;
                }
            }

            int currentIndex = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Name="))
                    name = lines[i].Substring(5);
                else if (lines[i].StartsWith("Type="))
                    type = lines[i].Substring(5);
                else if (lines[i].Contains("|") && currentIndex < sportsmen.Length)
                {
                    string[] parts = lines[i].Split('|');
                    var sportsman = new Blue_5.Sportsman(parts[0], parts[1]);
                    sportsman.SetPlace(int.Parse(parts[2]));
                    sportsmen[currentIndex++] = sportsman;
                }
            }

            Blue_5.Team team = type == "ManTeam"
                ? new Blue_5.ManTeam(name)
                : new Blue_5.WomanTeam(name);

            for (int i = 0; i < sportsmen.Length; i++)
                team.Add(sportsmen[i]);

            return (T)(object)team;
        }
    }
}