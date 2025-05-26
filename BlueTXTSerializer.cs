using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Lab_7;
using static System.Formats.Asn1.AsnWriter;
using static Lab_7.Blue_2;
using static Lab_7.Blue_3;

namespace Lab_9
{
    public class BlueTXTSerializer : BlueSerializer
    {
        public override string Extension => "txt";

        public override void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if (participant == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Type: {participant.GetType().Name}");
            sb.AppendLine($"Name: {participant.Name}");
            sb.AppendLine($"Votes: {participant.Votes}");

            if (participant is Blue_1.HumanResponse human)
            {
                sb.AppendLine($"Surname: {human.Surname}");
            }

            File.WriteAllText(FilePath, sb.ToString());
        }
        public override Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName)) return null;

            SelectFile(fileName);
            string[] lines = File.ReadAllLines(FilePath);

            string name = lines[1].Split(':')[1].Trim();
            int votes = int.Parse(lines[2].Split(':')[1].Trim());

            if (lines[0].Contains("HumanResponse"))
            {
                string surname = lines[3].Split(':')[1].Trim();
                return new Blue_1.HumanResponse(name, surname, votes);
            }

            return new Blue_1.Response(name, votes);
        }

        public override void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            // Проверка входных данных
            if (participant == null || string.IsNullOrEmpty(fileName)) return;

            // Создаем StringBuilder для эффективного построения строки
            StringBuilder sb = new StringBuilder();

            // Записываем основную информацию о прыжке
            sb.AppendLine($"Type: {participant.GetType().Name}");
            sb.AppendLine($"Name: {participant.Name}");
            sb.AppendLine($"Bank: {participant.Bank}");
            sb.Append("Participants:");
            foreach (var part in participant.Participants)
            {
                sb.Append("(");
                sb.Append($"Name:{part.Name},");
                sb.Append($"Surname:{part.Surname},");
                sb.Append("Marks:{");

                // Проверяем и обрабатываем оценки
                for (int i = 0; i < 2; i++)
                {
                    sb.Append("[");
                    for (int j = 0; j < 5; j++)
                    {
                        sb.Append(part.Marks[i, j]);
                        if (j < 4) sb.Append(",");
                    }
                    sb.Append("]");
                    if (i < 1) sb.Append(",");
                }
                sb.Append("});");
            }
            // Записываем результат в файл
            File.WriteAllText(fileName, sb.ToString());
        }
        public override Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName)) return null;

            SelectFile(fileName);
            using (StreamReader sr = new StreamReader(fileName))
            {
                string Type = sr.ReadLine().Substring("Type:".Length).Trim();
                string Name = sr.ReadLine().Substring("Name:".Length).Trim();
                int Bank=int.Parse(sr.ReadLine().Substring("Bank:".Length).Trim());
                string participantss = sr.ReadLine();
                Blue_2.WaterJump jump;

                if (Type == "WaterJump3m") jump = new Blue_2.WaterJump3m(Name, Bank);
                
                else jump = new Blue_2.WaterJump5m(Name, Bank);
                
                string Participants = participantss.Substring("Participants:".Length);
                string[] parts = Participants.Split(new[] {");"},StringSplitOptions.RemoveEmptyEntries);
                foreach (string entry in parts)
                {
                    string e = entry.Trim();
                    if (e.StartsWith("("))
                        e = e.Substring(1);
                    // Извлекаем имя и фамилию
                    int nameStart = e.IndexOf("Name:") + 5;
                    int nameEnd = e.IndexOf("Surname:", nameStart);
                    string name = e.Substring(nameStart, nameEnd - nameStart).Trim().Trim(','); ;

                    int surnameStart = nameEnd + 8;
                    int marksStart = e.IndexOf("Marks:{",surnameStart);
                    string surname = e.Substring(surnameStart, marksStart - surnameStart).Trim().Trim(',');

                    // Обрабатываем оценки
                    int markscnt=marksStart + 7;
                    int marksEnd=e.IndexOf("}",markscnt);
                    string marksData = e.Substring(markscnt,marksEnd-markscnt);
                    string[] marksPairs = marksData.Split(new[] { "],[" }, StringSplitOptions.RemoveEmptyEntries);

                    int[,] marks = new int[2,5];
                    for (int i = 0; i < marksPairs.Length; i++)
                    {
                        string cleanMarks = marksPairs[i].Replace("[", "").Replace("]", "");
                        string[] scores = cleanMarks.Split(",");
              
                        for (int j = 0; j < scores.Length; j++)
                            marks[i,j] = int.Parse(scores[j]);
                    }

                    // Добавляем участника
                    Blue_2.Participant p = new Blue_2.Participant(name, surname);
                    int[] jump1 = new int[5];
                    int[] jump2 = new int[5];
                    for (int j = 0; j < 5; j++)
                    {
                        jump1[j] = marks[0, j];
                        jump2[j] = marks[1, j];
                    }
                    p.Jump(jump1);
                    p.Jump(jump2);
                    jump.Add(p);
                }
                return jump;
            }
        }

        public override void SerializeBlue3Participant<T>(T student, string fileName) where T:class
        {
            if (student == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);
            string participantData = $"{student.GetType().Name},{student.Surname},{student.Name}," +
            $"{student.Penalties.Length},{string.Join(";",student.Penalties)}";
            File.WriteAllText(fileName, participantData);
        }
    
        public override T DeserializeBlue3Participant<T>(string fileName) where T : class
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) return default(T);

            string fileContent = File.ReadAllText(fileName);
            

            string[] dataParts = fileContent.Split(',');
            if (dataParts.Length < 5) return default(T);

            // Новый порядок полей при чтении
            string participantType = dataParts[0];
            string participantSurname = dataParts[1];
            string participantName = dataParts[2];
            string penaltiesCountStr = dataParts[3];
            string penaltiesData = dataParts[4];

            // Обработка штрафов (теперь с разделителем ;)
            int[] penalties = Array.Empty<int>();
            if (!string.IsNullOrWhiteSpace(penaltiesData))
            {
                string[] penaltyStrings = penaltiesData.Split(';');
                List<int> validPenalties = new List<int>();

                foreach (string penaltyStr in penaltyStrings)
                {
                    if (int.TryParse(penaltyStr.Trim(), out int penalty))
                    {
                        validPenalties.Add(penalty);
                    }
                }
                penalties = validPenalties.ToArray();
            }

            // Создание участника
            Blue_3.Participant participant = new Blue_3.Participant(participantName, participantSurname);
            if (participantType == "BasketballPlayer")
            {
                participant = new Blue_3.BasketballPlayer(participantName, participantSurname);
            }
            else if (participantType == "HockeyPlayer")
            {
                participant = new Blue_3.HockeyPlayer(participantName, participantSurname);
            }

            // Добавление штрафов
            foreach (int penalty in penalties)
            {
                participant.PlayMatch(penalty);
            }

            return (T)participant;

        }

        public override void SerializeBlue4Group(Blue_4.Group group, string fileName)
        {
            if (group == null || string.IsNullOrWhiteSpace(fileName)) return;

            SelectFile(fileName);
            StringBuilder content = new StringBuilder();

            content.AppendLine($"GroupName: {group.Name}");

            void AppendTeamData(Blue_4.Team team, string teamType)
            {
                if (team == null) return;

                content.AppendLine($"TeamType: {teamType}");
                content.AppendLine($"TeamName: {team.Name}");
                content.AppendLine($"Scores: {string.Join(",", team.Scores)}");
            }

            foreach (var team in group.ManTeams)
            {
                AppendTeamData(team, "ManTeam");
            }

            foreach (var team in group.WomanTeams)
            {
                AppendTeamData(team, "WomanTeam");
            }

            File.WriteAllText(fileName, content.ToString());
            
        }

        public override Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName)) return null;

            
            string[] lines = File.ReadAllLines(fileName);
            if (lines.Length == 0) return null;

            string groupNameLine = lines[0];
            if (!groupNameLine.StartsWith("GroupName:")) return null;

            string groupName = groupNameLine.Substring("GroupName:".Length).Trim();
            Blue_4.Group group = new Blue_4.Group(groupName);

            int lineIndex = 1;
            while (lineIndex < lines.Length)
            {
                if (lineIndex >= lines.Length || !lines[lineIndex].StartsWith("TeamType:"))
                {
                    lineIndex++;
                    continue;
                }

                string teamType = lines[lineIndex].Substring("TeamType:".Length).Trim();
                lineIndex++;

                if (lineIndex >= lines.Length || !lines[lineIndex].StartsWith("TeamName:")) continue;

                string teamName = lines[lineIndex].Substring("TeamName:".Length).Trim();
                lineIndex++;

                int[] scores = new int[0];
                if (lineIndex < lines.Length && lines[lineIndex].StartsWith("Scores:"))
                {
                    string scoresStr = lines[lineIndex].Substring("Scores:".Length).Trim();
                    if (!string.IsNullOrWhiteSpace(scoresStr))
                    {
                        string[] scoreValues = scoresStr.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        scores = new int[scoreValues.Length];

                        for (int i = 0; i < scoreValues.Length; i++)
                        {
                           
                            scores[i] = int.Parse(scoreValues[i].Trim());
                            
                        }
                    }
                    lineIndex++;
                }

                Blue_4.Team team=default(Blue_4.Team);

                if (teamType == "ManTeam")
                {
                    team = new Blue_4.ManTeam(teamName);
                }
                else if (teamType == "WomanTeam")
                {
                    team = new Blue_4.WomanTeam(teamName);
                }
                
                if (team != null)
                {
                    foreach (var score in scores)
                    {
                        team.PlayMatch(score);
                    }
                    group.Add(team);
                }
            }

            return group;    
          
        }

        public override void SerializeBlue5Team<T>(T group, string fileName) where T : class 
        {
            if (group == null || string.IsNullOrEmpty(fileName)) return;
            SelectFile(fileName);

            var content = new StringBuilder();
            content.AppendLine($"TeamType: {group.GetType().Name}");
            content.AppendLine($"TeamName: {group.Name}");
            content.AppendLine("Sportsmen:");

            foreach (var sportsman in group.Sportsmen)
            {
                if (sportsman!=null)
                    content.AppendLine($"Name:{sportsman.Name}; Surname:{sportsman.Surname}; Place:{sportsman.Place}");
            }

            File.WriteAllText(fileName, content.ToString());
        }
   
        public override T DeserializeBlue5Team<T>(string fileName) where T : class
        {
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName)) return null;
            
            string[] lines = File.ReadAllLines(fileName);
            if (lines.Length < 3) return null;

            string teamType = lines[0].Substring("TeamType:".Length).Trim();
            string teamName = lines[1].Substring("TeamName:".Length).Trim();

            Blue_5.Team team=default(Blue_5.Team);

            if (teamType=="ManTeam")
            {
                team = new Blue_5.ManTeam(teamName);
            }
            else if(teamType=="WomanTeam")
            {
                team = new Blue_5.WomanTeam(teamName);
            }

            for (int i = 2; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line) || !line.Contains(";"))
                    continue;

                // Парсим данные спортсмена
                string[] parts = line.Split(';');
                string sName = parts[0].Substring("Name:".Length).Trim();
                string sSurname = parts[1].Substring(" Surname:".Length).Trim();
                string placestr = parts[2].Substring(" Place:".Length).Trim();
                if (!int.TryParse(placestr, out int place))
                {
                    place = 0; 
                }
                Blue_5.Sportsman sportsman = new Blue_5.Sportsman(sName, sSurname);
                sportsman.SetPlace(place);
                team.Add(sportsman);
            }

            return (T)team;
        }
    }
}