using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static Lab_7.Blue_2;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        override public string Extension => "txt";

        override public void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if ((participant == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            string lain;
            if (participant is Blue_1.HumanResponse human)
            {
                lain = $"{human.Name} {human.Surname} {human.Votes}";
            }
            else
            {
                lain = $"{participant.Name} {participant.Votes}";
            }
            SelectFile(fileName);
            StreamWriter writer = new StreamWriter(FilePath);
            writer.Write(lain);
            writer.Close();
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if ((participant == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            string[] lain = [$"{participant.Name} {participant.Bank}"];
            if (participant is Blue_2.WaterJump3m)
            {
                lain[0] += " 3";
            }
            else
            {
                lain[0] += " 5";
            }
            for (int i = 0; i < participant.Participants.Length; i++)
            {
                Array.Resize(ref lain, lain.Length + 1);
                lain[lain.Length - 1] = $"{participant.Participants[i].Name} {participant.Participants[i].Surname}";
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        lain[lain.Length - 1] += $" {participant.Participants[i].Marks[j, k]}";
                    }
                }
            }
            SelectFile(fileName);
            StreamWriter writer = new StreamWriter(FilePath);
            for (int i = 0; i < lain.Length; i++) writer.WriteLine(lain[i]);
            writer.Close();
        }

        override public void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if ((student == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            string lain = $"{student.Name} {student.Surname}";
            if (student is Blue_3.BasketballPlayer)
            {
                lain += " B";
            }
            else if (student is Blue_3.HockeyPlayer)
            {
                lain += " H";
            }
            else
            {
                lain += " P";
            }
            for (int i = 0; i < student.Penalties.Length; i++)
            {
                lain += $" {student.Penalties[i]}";
            }
            SelectFile(fileName);
            StreamWriter writer = new StreamWriter(FilePath);
            writer.Write(lain);
            writer.Close();
        }

        override public void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if ((participant == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            string[] lain = [$"{participant.Name}"];
            for (int i = 0; i < participant.ManTeams.Length; i++)
            {
                if (participant.ManTeams[i] == null) break;
                Array.Resize(ref lain, lain.Length + 1);
                lain[lain.Length - 1] = $"{participant.ManTeams[i].Name + " M"}";
                for (int j = 0; j < participant.ManTeams[i].Scores.Length; j++) lain[lain.Length - 1] += $" {participant.ManTeams[i].Scores[j]}";
            }
            for (int i = 0; i < participant.WomanTeams.Length; i++)
            {
                if (participant.WomanTeams[i] == null) break;
                Array.Resize(ref lain, lain.Length + 1);
                lain[lain.Length - 1] = $"{participant.WomanTeams[i].Name + " W"}";
                for (int j = 0; j < participant.WomanTeams[i].Scores.Length; j++) lain[lain.Length - 1] += $" {participant.WomanTeams[i].Scores[j]}";
            }
            SelectFile(fileName);
            StreamWriter writer = new StreamWriter(FilePath);
            for (int i = 0; i < lain.Length; i++) writer.WriteLine(lain[i]);
            writer.Close();
        }

        override public void SerializeBlue5Team<T>(T group, string fileName)
        {
            if ((group == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            string[] lain = [$"{group.Name}"];
            if (group is Blue_5.ManTeam)
            {
                lain[0] += " M";
            }
            else
            {
                lain[0] += " W";
            }
            for (int i = 0; i < group.Sportsmen.Length; i++)
            {
                if (group.Sportsmen[i] == null) break;
                Array.Resize(ref lain, lain.Length + 1);
                lain[lain.Length - 1] = $"{group.Sportsmen[i].Name} {group.Sportsmen[i].Surname} {group.Sportsmen[i].Place}";
            }
            SelectFile(fileName);
            StreamWriter writer = new StreamWriter(FilePath);
            for (int i = 0; i < lain.Length; i++) writer.WriteLine(lain[i]);
            writer.Close();
        }

        override public Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            SelectFile(fileName);
            StreamReader reader = new StreamReader(FilePath);
            string lain = reader.ReadToEnd();
            reader.Close();
            string[] lains = lain.Split(' ');
            Blue_1.Response response;
            if (lains.Length == 2)
            {
                response = new Blue_1.Response(lains[0], int.Parse(lains[1]));
            }
            else
            {
                response = new Blue_1.HumanResponse(lains[0], lains[1], int.Parse(lains[2]));
            }
            return response;
        }

        override public Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            SelectFile(fileName);
            StreamReader reader = new StreamReader(FilePath);
            string lain = reader.ReadLine();
            string[] lains = lain.Split(' ');
            Blue_2.WaterJump waterJump;
            if (lains[2] == "3")
            {
                waterJump = new Blue_2.WaterJump3m(lains[0], int.Parse(lains[1]));
            }
            else
            {
                waterJump = new Blue_2.WaterJump5m(lains[0], int.Parse(lains[1]));
            }
            while ((lain = reader.ReadLine()) != null)
            {
                lains = lain.Split(' ');
                Blue_2.Participant participant = new Blue_2.Participant(lains[0], lains[1]);
                int[] result = new int[5];
                for (int i = 2; i < 7; i++) result[i - 2] = int.Parse(lains[i]);
                participant.Jump(result);
                for (int i = 7; i < 12; i++) result[i - 7] = int.Parse(lains[i]);
                participant.Jump(result);
                waterJump.Add(participant);
            }
            reader.Close();
            return waterJump;
        }

        override public T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            SelectFile(fileName);
            StreamReader reader = new StreamReader(FilePath);
            string lain = reader.ReadToEnd();
            string[] lains = lain.Split(' ');
            Blue_3.Participant participant;
            if (lains[2] == "B")
            {
                participant = new Blue_3.BasketballPlayer(lains[0], lains[1]);
            }
            else if (lains[2] == "H")
            {
                participant = new Blue_3.HockeyPlayer(lains[0], lains[1]);
            }
            else
            {
                participant = new Blue_3.Participant(lains[0], lains[1]);
            }
            for (int i = 3; i < lains.Length; i++) participant.PlayMatch(int.Parse(lains[i]));
            reader.Close();
            return (T)participant;
        }

        override public Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            SelectFile(fileName);
            StreamReader reader = new StreamReader(FilePath);
            string lain = reader.ReadLine();
            string[] lains;
            Blue_4.Group group = new Blue_4.Group(lain);
            while ((lain = reader.ReadLine()) != null)
            {
                lains = lain.Split(' ');
                Blue_4.Team team;
                if (lains[1] == "M")
                {
                    team = new Blue_4.ManTeam(lains[0]);
                    for (int j = 2; j < lains.Length; j++) team.PlayMatch(int.Parse(lains[j]));
                }
                else
                {
                    team = new Blue_4.WomanTeam(lains[0]);
                    for (int j = 2; j < lains.Length; j++) team.PlayMatch(int.Parse(lains[j]));
                }
                group.Add(team);
            }
            reader.Close();
            return group;
        }

        override public T DeserializeBlue5Team<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            SelectFile(fileName);
            StreamReader reader = new StreamReader(FilePath);
            string lain = reader.ReadLine();
            string[] lains = lain.Split(' ');
            Blue_5.Team team;
            if (lains[1] == "M")
            {
                team = new Blue_5.ManTeam(lains[0]);
            }
            else
            {
                team = new Blue_5.WomanTeam(lains[0]);
            }
            while ((lain = reader.ReadLine()) != null)
            {
                lains = lain.Split(' ');
                Blue_5.Sportsman sportsman = new Blue_5.Sportsman(lains[0], lains[1]);
                sportsman.SetPlace(int.Parse(lains[2]));
                team.Add(sportsman);
            }
            reader.Close();
            return (T)team;
        }
    }
}
