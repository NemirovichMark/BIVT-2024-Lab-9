using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lab_7;
using System.Threading.Tasks;
using Lab_9;
using static Lab_7.Blue_2;
using System.Text.RegularExpressions;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
       
        public override string Extension => "txt";
        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || fileName==null|| String.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            File.WriteAllText(FilePath, "");
            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine($"Type: {participant.GetType().Name}");
                writer.WriteLine($"Name: {participant.Name}");
                writer.WriteLine($"Votes: {participant.Votes}");
                if (participant is Blue_1.HumanResponse part)
                {
                    writer.WriteLine($"Surname: {part.Surname}");
                }
            }


        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine($"Type: {participant.GetType().Name}");
                writer.WriteLine($"Name: {participant.Name}");
                writer.WriteLine($"Bank: {participant.Bank}");
                writer.Write("Participants:");

                foreach (var part in participant.Participants)
                {
                    writer.Write("(");
                    writer.Write($"Name:{part.Name}");
                    writer.Write($"Surname:{part.Surname}");

                    writer.Write("marks:{");
                    for (int i = 0; i < 2; i++)
                    {
                        writer.Write("[");
                        for (int j = 0; j < 5; j++)
                        {
                            writer.Write(part.Marks[i, j]);
                            if (j < 4) writer.Write(",");
                        }
                        writer.Write("]");
                        if (i < 1) writer.Write(",");
                    }
                    writer.Write("}");

                    writer.Write(");");
                   

                }
            }
        }
        public override void SerializeBlue3Participant<T>(T student, string fileName) where T : class
        {
            if (student == null || String.IsNullOrEmpty(fileName)) { return; }
            
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine($"Type: {student.GetType().Name}");
                writer.WriteLine($"Name: {student.Name}");
                writer.WriteLine($"Surname: {student.Surname}");
                writer.Write("Penalties: [");
                if (student.Penalties != null && student.Penalties.Length > 0)
                {
                    writer.Write(string.Join(",", student.Penalties));
                }
                writer.WriteLine("]");
            }
        }
        public override void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) { return; }
            using (StreamWriter writer = new StreamWriter(fileName, false))
            {
                writer.WriteLine($"Name: {participant.Name}");
                //
                var manteams = participant.ManTeams;
                int manc = 0;
                for (int i = 0; i < manteams.Length; i++)
                {
                    if (manteams[i] != null)
                        manc++;
                }
                for (int i = 0; i < manc; i++)
                {
                    var team = manteams[i];
                    writer.WriteLine("TeamType:ManTeam");
                    writer.WriteLine($"TeamName:{team.Name}");
                    var scores = team.Scores;
                   
                    if (scores != null && scores.Length > 0) writer.WriteLine("Scores:" + string.Join(",", scores));

                }
                //
                var womanteams = participant.WomanTeams;
                int womc = 0;
                for (int i = 0; i < womanteams.Length; i++)
                {
                    if (womanteams[i] != null)
                        womc++;
                }
             
                for (int i = 0; i < womc; i++)
                {
                    var team = womanteams[i];
                    writer.WriteLine("TeamType:WomanTeam");
                    writer.WriteLine($"TeamName:{team.Name}");
                    var scores = team.Scores;
                  
                    if (scores != null && scores.Length > 0) writer.WriteLine("Scores:" + string.Join(",", scores));

                }
            }
        }

        public override void SerializeBlue5Team<T>(T group, string fileName)// where T : class
        {
            if (group == null || String.IsNullOrEmpty(fileName)) { return; }
            using (StreamWriter writer = new StreamWriter(fileName, false))
            {
                writer.WriteLine($"Type: {group.GetType().Name}");
                writer.WriteLine($"Name: {group.Name}");
                writer.WriteLine("Sportsmen:");
                foreach (var sportsman in group.Sportsmen)
                {
                    if (sportsman != null)
                    {
                        writer.WriteLine($"Name:{sportsman.Name}; Surname:{sportsman.Surname}; Place:{sportsman.Place}");
                    }
                }
            }
        }

        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            SelectFile(fileName);
            string text = File.ReadAllText(FilePath);
            Dictionary<string, string> response = new Dictionary<string, string>();
            foreach (var row in text.Split(Environment.NewLine))
            {
                if (row.Contains(':'))
                {
                    var field = row.Split(':');
                    response[field[0].Trim()] = field[1].Trim();
                }
            }
            Blue_1.Response deserialized = default(Blue_1.Response);
            if (response["Type"] == "Response")
            {
                deserialized = new Blue_1.Response(response["Name"], Int32.Parse(response["Votes"]));
            }
            else
            {
                deserialized = new Blue_1.HumanResponse(response["Name"], response["Surname"], Int32.Parse(response["Votes"]));
            }
            return deserialized;
        }
        

        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            SelectFile(fileName);
            using (StreamReader reader = new StreamReader(fileName))
            {
                
                string typestr = reader.ReadLine(); 
                string namestr = reader.ReadLine(); 
                string bankstr = reader.ReadLine(); 
                string participantsstr = reader.ReadLine(); 

                string type = typestr.Substring("Type:".Length).Trim();
                string name = namestr.Substring("Name:".Length).Trim();
                int bank = int.Parse(bankstr.Substring("Bank:".Length).Trim());

                Blue_2.WaterJump jump=null;

                if (type == "WaterJump3m")
                {
                    jump = new Blue_2.WaterJump3m(name, bank);
                }
                else if (type == "WaterJump5m")
                {
                    jump = new Blue_2.WaterJump5m(name, bank);
                }
                

                string participantsData = participantsstr.Substring("Participants:".Length);

                

               
                string[] parts = participantsData.Split(new[] { ");" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string parti in parts)
                {
                    string p = parti.Trim();

                    
                    if (p.StartsWith("("))
                        p = p.Substring(1);

                  
                    int names = p.IndexOf("Name:") + "Name:".Length;
                    int namee = p.IndexOf("Surname:", names);
                    string pname = p.Substring(names, namee - names).Trim();

                   
                    int surnames = p.IndexOf("Surname:") + "Surname:".Length;
                    int markss = p.IndexOf("marks:{", surnames);
                    string psurname = p.Substring(surnames, markss - surnames).Trim();

                    
                    int markscounts = markss + "marks:{".Length;
                    int markscounte= p.IndexOf("}", markscounts);
                    string pmarks = p.Substring(markscounts, markscounte - markscounts);

                    string[] massiv = pmarks.Split(new[] { "],[" }, StringSplitOptions.RemoveEmptyEntries);

                    int[,] marks = new int[2, 5];
                    for (int i = 0; i < massiv.Length; i++)
                    {
                        string mas = massiv[i].Replace("[", "").Replace("]", "");
                        string[] nums = mas.Split(',');

                        for (int j = 0; j < nums.Length; j++)
                        {
                            marks[i, j] = int.Parse(nums[j]);
                        }
                    }

                 
                    Participant part = new Participant(pname, psurname);

                   
                    int[] jump1 = new int[5];
                    int[] jump2 = new int[5];
                    for (int j = 0; j < 5; j++)
                    {
                        jump1[j] = marks[0, j];
                        jump2[j] = marks[1, j];
                    }
                    part.Jump(jump1);
                    part.Jump(jump2);

                    jump.Add(part);
                    
                }


                return jump;
            }



        }

        public override T DeserializeBlue3Participant<T>(string fileName) where T : class
        {
            using (var reader = new StreamReader(fileName))
            {
                string typestr = reader.ReadLine(); 
                string namestr = reader.ReadLine(); 
                string surnamestr = reader.ReadLine(); 
                string penaltiesstr = reader.ReadLine(); 

                string type = typestr.Split(':')[1].Trim();
                string name = namestr.Split(':')[1].Trim();
                string surname = surnamestr.Split(':')[1].Trim();

                
                int start = penaltiesstr.IndexOf('[') + 1;
                int end = penaltiesstr.IndexOf(']');
                string penaltiesStr = penaltiesstr.Substring(start, end - start);

                int[] penalties;

                if (string.IsNullOrEmpty(penaltiesStr))
                {
                    penalties = new int[0];
                }
                else
                {
                    string[] parts = penaltiesStr.Split(',');
                    penalties = new int[parts.Length];
                    for (int i = 0; i < parts.Length; i++)
                    {
                        penalties[i] = int.Parse(parts[i].Trim());
                    }
                }

                Blue_3.Participant participant =null ;
                if (type == "BasketballPlayer")
                {
                    participant = new Blue_3.BasketballPlayer(name, surname);
                }
                else if (type == "HockeyPlayer")
                { 
                    participant = new Blue_3.HockeyPlayer(name, surname);
                }
                else participant = new Blue_3.Participant(name, surname);
               
                foreach (var p in penalties)
                    participant.PlayMatch(p);

                return (T)participant;
            }
        }
        

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                string groupnamestr = reader.ReadLine();
                string  groupname = groupnamestr.Split(':')[1].Trim();
                Blue_4.Group group = new Blue_4.Group(groupname);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!line.StartsWith("TeamType:"))
                        continue;

                    string teamtype = line.Substring("TeamType:".Length).Trim();

                    string teamnamestr = reader.ReadLine();
                    string teamname = teamnamestr.Split(':')[1].Trim();
                    string scoresstr = reader.ReadLine();
                    int[] scores = new int[0];
                    string scorescount = scoresstr.Substring("Scores:".Length).Trim();
                    if (scorescount.Length > 0)
                    {
                        string[] parts = scorescount.Split(',');
                        scores = new int[parts.Length];
                        for (int i = 0; i < parts.Length; i++)
                        {
                            scores[i] = int.Parse(parts[i].Trim());
                        }
                    }
                    Blue_4.Team team = null;
                    if (teamtype == "ManTeam")
                    {
                        team = new Blue_4.ManTeam(teamname);
                    }
                    else if (teamtype == "WomanTeam")
                    {
                        team = new Blue_4.WomanTeam(teamname);
                    }
                    for (int i = 0; i < scores.Length; i++)
                    {
                        team.PlayMatch(scores[i]);
                    }

                    group.Add(team);
                }

                return group;



               
            }
        }

        public override T DeserializeBlue5Team<T>(string fileName) where T : class
        {
            using (var reader = new StreamReader(fileName))
            {

                string typestr = reader.ReadLine();
                string typename = typestr.Substring("Type:".Length).Trim();
                string groupstr = reader.ReadLine();
               string  groupname = groupstr.Substring("Name:".Length).Trim();
                Blue_5.Team team = null;
                if (typename == "ManTeam")
                {
                    team = new Blue_5.ManTeam(groupname);
                }
                   
                else if (typename == "WomanTeam")
                {
                    team = new Blue_5.WomanTeam(groupname);
                }
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts= line.Split(';');
                    if (parts.Length < 3)
                        continue;
                    string name = parts[0].Split(':')[1].Trim();
                    string surname = parts[1].Split(':')[1].Trim();
                    int place = 0;
                    int.TryParse(parts[2].Split(':')[1].Trim(), out place);

                    var sportsman = new Blue_5.Sportsman(name, surname);
                    sportsman.SetPlace(place);
                    team.Add(sportsman);

                }
                return (T)team;
                   

            }
        }
    }
}
