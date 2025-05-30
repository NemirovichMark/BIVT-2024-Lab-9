using Lab_7;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab_7.Purple_1;
using static Lab_7.Purple_3;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";
        //public override void SerializePurple1<T>(T obj, string fileName) => throw new NotImplementedException();
        //public override T DeserializePurple1<T>(string fileName) => throw new NotImplementedException();
        public override void SerializePurple1<T>(T obj, string fileName)
        {
            if (obj == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            File.WriteAllText(FilePath, SerializeObject(obj));
        }

        private string SerializeObject<T>(T obj)
        {
            var res = new StringBuilder();

            switch (obj)
            {
                case Purple_1.Participant p:
                    res.AppendLine($"Name: {p.Name}");
                    res.AppendLine($"Surname: {p.Surname}");
                    res.Append("Coefs: ").AppendJoin(" ", p.Coefs ?? Array.Empty<double>()).AppendLine();
                    res.AppendLine($"TotalScore: {p.TotalScore}");

                    if (p.Marks is int[,] marks)
                    {
                        for (int i = 0; i < marks.GetLength(0); i++)
                        {
                            res.Append($"Jump{i}: ");
                            for (int j = 0; j < marks.GetLength(1); j++)
                                res.Append($"{marks[i, j]} ");
                            res.AppendLine();
                        }
                    }
                    break;

                case Purple_1.Judge j:
                    res.AppendLine($"Name: {j.Name}");
                    res.Append("Marks: ").AppendJoin(" ", j.Marks ?? Array.Empty<int>()).AppendLine();
                    break;

                case Purple_1.Competition c:
                    res.AppendLine("Judges:");
                    if (c.Judges is Judge[] judges)
                    {
                        foreach (var j in judges)
                        {
                            res.AppendLine($"Judge:");
                            res.AppendLine(SerializeObject(j));
                        }
                    }

                    res.AppendLine("Participants:");
                    if (c.Participants is Purple_1.Participant[] participants)
                    {
                        foreach (var p in participants)
                        {
                            res.AppendLine($"Participant:");
                            res.AppendLine(SerializeObject(p));
                        }
                    }
                    break;
            }

            return res.ToString();
        }
        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            if (jumping == null || string.IsNullOrEmpty(fileName)) return;

            SelectFile(fileName);
            var res = new StringBuilder();
            if (jumping is Purple_2.JuniorSkiJumping)
                res.AppendLine("JuniorSkiJumping");
            else if (jumping is Purple_2.ProSkiJumping)
                res.AppendLine("ProSkiJumping");

            // Основная информация о соревновании
            res.AppendLine($"Name: {jumping.Name}");
            res.AppendLine($"Standard: {jumping.Standard}");
            res.AppendLine($"ParticipantsCount: {jumping.Participants.Length}");

            // Участники
            for (int i = 0; i < jumping.Participants.Length; i++)
            {
                var p = jumping.Participants[i];
                res.AppendLine("Participant:");
                res.AppendLine($"Name: {p.Name}");
                res.AppendLine($"Surname: {p.Surname}");
                res.AppendLine($"Distance: {p.Distance}");
                res.Append("Marks: ");
                if (p.Marks != null)
                {
                    for (int j = 0; j < p.Marks.Length; j++)
                    {
                        res.Append(p.Marks[j]);
                        if (j < p.Marks.Length - 1) res.Append(" ");
                    }
                }
                res.AppendLine();
            }

            File.WriteAllText(FilePath, res.ToString());
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            var output = new StringBuilder();

            output.AppendLine(skating switch
            {
                FigureSkating _ => "FigureSkating",
                IceSkating _ => "IceSkating",
                _ => "UnknownSkating"
            });

            output.Append("moods:").AppendLine(string.Join(" ", skating.Moods));
            output.Append("participants:").AppendLine(skating.Participants.Length.ToString());

            foreach (var p in skating.Participants)
            {
                var data = new Dictionary<string, string>(4)
                {
                    ["name"] = p.Name,
                    ["surname"] = p.Surname,
                    ["marks"] = string.Join(" ", p.Marks),
                    ["places"] = string.Join(" ", p.Places)
                };
                output.Append(DictToString(data));
            }

            SelectFile(fileName);
            File.WriteAllText(FilePath, output.ToString());
        }
        private static string DictToString(Dictionary<string, string> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var k in dict.Keys)
            {
                sb.Append(k);
                sb.Append(':');
                sb.AppendLine(dict[k]);
            }
            return sb.ToString();
        }
        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            if (group == null || string.IsNullOrEmpty(fileName))
                return;
            SelectFile(fileName);
            var content = new StringBuilder();

            // Название группы
            content.AppendLine($"GroupName: {group.Name}");
            content.AppendLine($"ParticipantsCount: {group.Sportsmen?.Length ?? 0}");


            // Участники и их результаты
            if (group.Sportsmen != null)
            {
                foreach (var sportsman in group.Sportsmen)
                {
                    content.AppendLine("Participant:");
                    if (sportsman is Purple_4.SkiMan)
                    {
                        content.AppendLine("Type: SkiMan");
                    }
                    else if (sportsman is Purple_4.SkiWoman)
                    {
                        content.AppendLine("Type: SkiWoman");
                    }
                    else
                    {
                        content.AppendLine("Type: Sportsman");
                    }
                    content.AppendLine($"Name: {sportsman.Name}");
                    content.AppendLine($"Surname: {sportsman.Surname}");
                    content.AppendLine($"Time: {sportsman.Time.ToString("G17", CultureInfo.InvariantCulture)}");
                }
            }
            File.WriteAllText(FilePath, content.ToString());
        }
        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            if (report == null || string.IsNullOrEmpty(fileName))
                return;

            var content = new StringBuilder();

            // Сериализация каждого исследования
            foreach (var research in report.Researches)
            {
                // Имя исследования
                content.Append(research.Name);

                // Сериализация каждого ответа
                foreach (var response in research.Responses)
                {
                    content.Append("; ");
                    content.Append(response.Animal ?? "NULL");
                    content.Append(", ");
                    content.Append(response.CharacterTrait ?? "NULL");
                    content.Append(", ");
                    content.Append(response.Concept ?? "NULL");
                }

                content.AppendLine();
            }

            SelectFile(fileName);
            File.WriteAllText(FilePath, content.ToString());
        }
        private Dictionary<string, string> ParseKeyValuePairs(string[] lines)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split( ':' , 2);
                if (parts.Length != 2) continue;
                var key = parts[0].Trim().ToLower();
                var value = parts[1].Trim();
                dict[key] = value;
            }
            return dict;
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            if (!File.Exists(FilePath))
                throw new FileNotFoundException($"File {fileName} not found");

            string[] lines = File.ReadAllLines(FilePath);
            if (lines.Length == 0)
                throw new InvalidDataException("Empty file");

            if (typeof(T) == typeof(Purple_1.Participant))
                return (T)(object)DeserializeParticipant(lines);
            if (typeof(T) == typeof(Purple_1.Judge))
                return (T)(object)DeserializeJudge(lines);
            if (typeof(T) == typeof(Purple_1.Competition))
                return (T)(object)DeserializeCompetition(lines);

            throw new NotSupportedException($"Type {typeof(T)} not supported");
        }

        private Purple_1.Participant DeserializeParticipant(string[] lines)
        {
            var data = ParseKeyValuePairs(lines);

            var participant = new Purple_1.Participant(data["Name"], data["Surname"]);

            if (data.TryGetValue("Coefs", out var coefsStr))
            {
                var coefParts = coefsStr.Split(' ');
                var coefs = new double[coefParts.Length];
                for (int i = 0; i < coefParts.Length; i++)
                    coefs[i] = double.Parse(coefParts[i]);
                participant.SetCriterias(coefs);
            }

            for (int i = 0; i < 4; i++)
            {
                if (data.TryGetValue($"Jump{i}", out var marksStr))
                {
                    var markParts = marksStr.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var marks = new int[markParts.Length];
                    for (int j = 0; j < markParts.Length; j++)
                        marks[j] = int.Parse(markParts[j]);
                    participant.Jump(marks);
                }
            }

            return participant;
        }

        private Purple_1.Judge DeserializeJudge(string[] lines)
        {
            var data = ParseKeyValuePairs(lines);
            var markParts = data["Marks"].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var marks = new int[markParts.Length];
            for (int i = 0; i < markParts.Length; i++)
                marks[i] = int.Parse(markParts[i]);

            return new Purple_1.Judge(data["Name"], marks);
        }

        private Purple_1.Competition DeserializeCompetition(string[] allLines)
        {
            // Подсчет количества судей и участников
            int judgeCount = 0, participantCount = 0;
            foreach (var line in allLines)
            {
                if (line == "Judge:") judgeCount++;
                else if (line == "Participant:") participantCount++;
            }

            // Инициализация массивов
            var judges = new Purple_1.Judge[judgeCount];
            var participants = new Purple_1.Participant[participantCount];
            int jIdx = 0, pIdx = 0;

            // Обработка данных
            var blockStarts = new int[judgeCount + participantCount];
            var blockTypes = new bool[judgeCount + participantCount]; // true = judge, false = participant
            int blockIndex = 0;

            for (int i = 0; i < allLines.Length; i++)
            {
                if (allLines[i] == "Judge:" || allLines[i] == "Participant:")
                {
                    blockStarts[blockIndex] = i;
                    if (allLines[i] == "Judge:")
                        blockTypes[blockIndex] = true;
                    else
                        blockTypes[blockIndex] = false;
                    blockIndex++;
                }
            }
            for (int b = 0; b < blockStarts.Length; b++)
            {
                int start = blockStarts[b];
                int end = (b < blockStarts.Length - 1) ? blockStarts[b + 1] : allLines.Length;
                int blockSize = end - start - 1; // -1 чтобы исключить строку с Judge:/Participant:

                if (blockSize <= 0) continue;

                var blockLines = new string[blockSize];
                Array.Copy(allLines, start + 1, blockLines, 0, blockSize);

                if (blockTypes[b]) // Judge
                {
                    if (jIdx < judges.Length)
                    {
                        judges[jIdx] = DeserializeJudge(blockLines);
                        jIdx++;
                    }
                }
                else // Participant
                {
                    if (pIdx < participants.Length)
                    {
                        participants[pIdx] = DeserializeParticipant(blockLines);
                        pIdx++;
                    }
                }
            }

            var competition = new Purple_1.Competition(judges);
            competition.Add(participants);
            return competition;
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            if (!File.Exists(FilePath))
                throw new FileNotFoundException($"File {fileName} not found");

            string[] lines = File.ReadAllLines(FilePath);
            if (lines.Length == 0)
                throw new InvalidDataException("Empty file");

            Purple_2.SkiJumping jumping;
            if (lines[0] == "JuniorSkiJumping")
                jumping = new Purple_2.JuniorSkiJumping();
            else if (lines[0] == "ProSkiJumping")
                jumping = new Purple_2.ProSkiJumping();
            else
                throw new InvalidDataException("Unknown competition type");

            // Читаем основные параметры
            int participantsCount = 0;
            for (int i = 1; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("ParticipantsCount:"))
                {
                    participantsCount = int.Parse(lines[i].Substring("ParticipantsCount:".Length).Trim());
                    break;
                }
            }

            // Подготавливаем массив участников
            var participants = new Purple_2.Participant[participantsCount];
            int currentParticipant = -1;
            int[] currentMarks = null;
            int currentDistance = -1;

            // Обрабатываем участников
            for (int i = 1; i < lines.Length; i++)
            {
                if (lines[i] == "Participant:")
                {
                    currentParticipant++;
                    if (currentParticipant >= participantsCount) break;
                }
                else if (currentParticipant >= 0 && currentParticipant < participantsCount)
                {
                    if (lines[i].StartsWith("Name:"))
                    {
                        string name = lines[i].Substring("Name:".Length).Trim();
                        string surname = lines[i + 1].Substring("Surname:".Length).Trim();
                        participants[currentParticipant] = new Purple_2.Participant(name, surname);
                        i++; // Пропускаем следующую строку (Surname)
                    }
                    else if (lines[i].StartsWith("Distance:"))
                    {
                        currentDistance = int.Parse(lines[i].Substring("Distance:".Length).Trim());
                    }
                    else if (lines[i].StartsWith("Marks:"))
                    {
                        string marksStr = lines[i].Substring("Marks:".Length).Trim();
                        if (!string.IsNullOrEmpty(marksStr))
                        {
                            string[] marks = marksStr.Split(' ');
                            currentMarks = new int[marks.Length];
                            for (int j = 0; j < marks.Length; j++)
                                currentMarks[j] = int.Parse(marks[j]);
                        }

                        // Сохраняем прыжок
                        if (currentDistance != -1 && currentMarks != null)
                        {
                            participants[currentParticipant].Jump(currentDistance, currentMarks, jumping.Standard);
                            currentDistance = -1;
                            currentMarks = null;
                        }
                    }
                }
            }

            // Добавляем участников
            for (int i = 0; i < participants.Length; i++)
                jumping.Add(participants[i]);

            return (T)(object)jumping;
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var content = File.ReadAllText(FilePath);
            var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            var type = lines[0].Trim();
            double[] moods = lines[1].Split(':')[1].Trim().Split(' ').Select(x => double.Parse(x)).ToArray();
            Purple_3.Skating skating;
            if (type == "IceSkating") skating = new Purple_3.IceSkating(moods, false);
            else if (type == "FigureSkating") skating = new Purple_3.FigureSkating(moods, false);
            else return null;

            var pCount = int.Parse(lines[2].Split(':')[1]);
            var participants = Enumerable.Empty<Purple_3.Participant>();

            for (int i = 0; i < pCount; i++)
            {
                var start = 3 + i * 4;
                var dict = new Dictionary<string, string>(4);

                for (int j = start; j < start + 4; j++)
                {
                    var parts = lines[j].Split(':');
                    dict[parts[0].Trim()] = parts[1].Trim();
                }

                var p = new Purple_3.Participant(dict["name"], dict["surname"]);
                var marks = Array.ConvertAll(dict["marks"].Split(), double.Parse);

                foreach (var mark in marks) p.Evaluate(mark);

                participants = participants.Append(p);
            }

            skating.Add(participants.ToArray());
            Purple_3.Participant.SetPlaces(skating.Participants);
            return (T)(object)skating;
        }

        private string SafeGetValue(string[] lines, int index, string prefix)
        {
            if (index >= lines.Length) return string.Empty;
            string line = lines[index];
            return line.StartsWith(prefix) ? line.Substring(prefix.Length).Trim() : string.Empty;
        }
        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);

            string[] lines = File.ReadAllLines(FilePath);
            if (lines.Length == 0)
                throw new InvalidDataException("Empty file");

            // Читаем название группы
            string groupName = lines[0].Substring("GroupName:".Length).Trim();
            var group = new Purple_4.Group(groupName);

            // Читаем количество участников
            int participantsCount = int.Parse(lines[1].Substring("ParticipantsCount:".Length).Trim());

            // Обрабатываем участников
            int currentLine = 2;
            int processedParticipants = 0;

            while (processedParticipants < participantsCount && currentLine < lines.Length)
            {
                if (lines[currentLine] == "Participant:")
                {
                    currentLine++;
                    processedParticipants++;

                    // Тип участника
                    string type = lines[currentLine++].Substring("Type:".Length).Trim();

                    // Основные данные
                    string name = lines[currentLine++].Substring("Name:".Length).Trim();
                    string surname = lines[currentLine++].Substring("Surname:".Length).Trim();
                    double time = double.Parse(lines[currentLine++].Substring("Time:".Length).Trim(), NumberStyles.Any, CultureInfo.InvariantCulture);

                    // Создаем участника нужного типа
                    Purple_4.Sportsman sportsman;
                    switch (type)
                    {
                        case "SkiMan":
                            sportsman = new Purple_4.SkiMan(name, surname);
                            break;
                        case "SkiWoman":
                            sportsman = new Purple_4.SkiWoman(name, surname);
                            break;
                        case "Sportsman":
                            sportsman = new Purple_4.Sportsman(name, surname);
                            break;
                        default:
                            throw new InvalidDataException($"Unknown participant type: {type}");
                    }

                    sportsman.Run(time);
                    group.Add(sportsman);
                }
                else
                {
                    currentLine++;
                }
            }

            return group;
        }
        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var lines = File.ReadAllLines(FilePath);

            var report = new Purple_5.Report();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) continue;

                var researchName = parts[0].Trim();
                var research = new Purple_5.Research(researchName);

                // Обрабатываем ответы (каждый ответ состоит из 3 частей: animal;trait;concept)
                for (int i = 1; i < parts.Length; i++)
                {
                    var responseParts = parts[i].Split(',');
                    if (responseParts.Length != 3) continue;

                    var answers = new string[3];
                    answers[0] = responseParts[0].Trim() == "NULL" ? null : responseParts[0].Trim();
                    answers[1] = responseParts[1].Trim() == "NULL" ? null : responseParts[1].Trim();
                    answers[2] = responseParts[2].Trim() == "NULL" ? null : responseParts[2].Trim();

                    research.Add(answers);
                }

                report.AddResearch(research);
            }

            return report;
        }
    }
}
