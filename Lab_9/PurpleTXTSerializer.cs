using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab_7.Purple_4;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";


        private void DictToFile(Dictionary<string, string> dict)
        {
            var lines = dict
                .Select(kv => $"{kv.Key}={kv.Value}")
                .ToArray();

            File.WriteAllLines(FilePath, lines);
        }

        private Dictionary<string, string> FileToDict()
        {
            var result = new Dictionary<string, string>();
            var lines = File.ReadAllLines(FilePath);

            foreach (var line in lines)
            {
                var parts = line.Split(new[] { '=' }, 2);

                if (parts.Length == 2 && parts[0] != "")
                {
                    var key = parts[0];
                    var value = parts[1];
                    result[key] = value;
                }
            }

            return result;
        }

        private string ArrayToString<T>(T[] items)
        {
            if (items == null || items.Length == 0)
            {
                return string.Empty;
            }

            string[] strings = new string[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                strings[i] = items[i]?.ToString() ?? "";
            }

            return string.Join(";", strings);
        }

        private T[] StringToArray<T>(string data, Func<string, T> parser)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return Array.Empty<T>();
            }

            var parts = data.Split(';');
            var result = new T[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                result[i] = parser(parts[i]);
            }

            return result;
        }

        private string MatrixToString<T>(T[,] matrix)
        {
            if (matrix == null) { return string.Empty; }


            int rowCount = matrix.GetLength(0);
            int colCount = matrix.GetLength(1);

            var rowStrings = new string[rowCount];
            for (int r = 0; r < rowCount; r++)
            {
                var cells = new string[colCount];
                for (int c = 0; c < colCount; c++)
                    cells[c] = matrix[r, c]?.ToString() ?? "";

                rowStrings[r] = string.Join(";", cells);
            }

            return string.Join("|", rowStrings); // другой разделитель, чтобы получать 1;2;|3;4
        }

        private T[,] StringToMatrix<T>(string input, Func<string, T> parser)
        {
            if (string.IsNullOrWhiteSpace(input)) { return new T[0, 0]; }


            var rowStrings = input.Split('|');
            int numRows = rowStrings.Length;

            int numCols = rowStrings[0].Split(';').Length;

            var result = new T[numRows, numCols];

            for (int r = 0; r < numRows; r++)
            {
                var cells = rowStrings[r].Split(';');
                for (int c = 0; c < numCols; c++)
                    result[r, c] = parser(cells[c]);
            }

            return result;
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            var dict = new Dictionary<string, string>(); //

            if (obj is Purple_1.Participant participant) // определяем обьект и заполняем для него ключ-значение
            {
                dict["Type"] = "Participant";
                dict["Name"] = participant.Name;
                dict["Surname"] = participant.Surname;
                dict["TotalScore"] = participant.TotalScore.ToString();
                dict["Coefs"] = ArrayToString(participant.Coefs);
                dict["Marks"] = MatrixToString(participant.Marks);
            }
            else if (obj is Purple_1.Judge judge)
            {
                dict["Type"] = "Judge";
                dict["Name"] = judge.Name;
                dict["Marks"] = ArrayToString(judge.Marks);
            }
            else if (obj is Purple_1.Competition comp)
            {
                dict["Type"] = "Competition";
                dict["JudgesCount"] = comp.Judges.Length.ToString();
                for (int i = 0; i < comp.Judges.Length; i++)
                {
                    var j = comp.Judges[i];
                    dict[$"Judge_{i}_Name"] = j.Name;
                    dict[$"Judge_{i}_Marks"] = ArrayToString(j.Marks);
                }
                dict["ParticipantsCount"] = comp.Participants.Length.ToString();
                for (int k = 0; k < comp.Participants.Length; k++)
                {
                    var p = comp.Participants[k];
                    dict[$"Part_{k}_Name"] = p.Name;
                    dict[$"Part_{k}_Surname"] = p.Surname;
                    dict[$"Part_{k}_Coefs"] = ArrayToString(p.Coefs);
                    dict[$"Part_{k}_Marks"] = MatrixToString(p.Marks);
                }
            }
            DictToFile(dict);
        }

        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var data = FileToDict();
            var type = data.TryGetValue("Type", out var t) ? t : string.Empty; // определяем тип

            if (type == "Participant")
            {
                var name = data["Name"];
                var surname = data["Surname"];
                var coefs = StringToArray(data["Coefs"], double.Parse);
                var marks = StringToMatrix(data["Marks"], int.Parse);

                var p = new Purple_1.Participant(name, surname); // создаем нужный обьект и через внутр методы заполняем
                p.SetCriterias(coefs);
                for (int i = 0; i < marks.GetLength(0); i++)
                {
                    var row = new int[marks.GetLength(1)];
                    for (int j = 0; j < row.Length; j++) { row[j] = marks[i, j]; }
                    p.Jump(row);
                }

                return p as T;
            }
            else if (type == "Judge")
            {
                var name = data["Name"];
                var marks = StringToArray(data["Marks"], int.Parse);
                return new Purple_1.Judge(name, marks) as T;
            }
            else if (type == "Competition")
            {
                int judgesCount = int.Parse(data["JudgesCount"]);
                var judges = new Purple_1.Judge[judgesCount];
                for (int i = 0; i < judgesCount; i++)
                {
                    var jName = data[$"Judge_{i}_Name"];
                    var jMarks = StringToArray(data[$"Judge_{i}_Marks"], int.Parse);
                    judges[i] = new Purple_1.Judge(jName, jMarks);
                }

                var comp = new Purple_1.Competition(judges);
                int partCount = int.Parse(data["ParticipantsCount"]);
                for (int k = 0; k < partCount; k++)
                {
                    var pName = data[$"Part_{k}_Name"];
                    var pSurname = data[$"Part_{k}_Surname"];
                    var pCoefs = StringToArray(data[$"Part_{k}_Coefs"], double.Parse);
                    var pMarks = StringToMatrix(data[$"Part_{k}_Marks"], int.Parse);

                    var participant = new Purple_1.Participant(pName, pSurname);
                    participant.SetCriterias(pCoefs);
                    for (int i = 0; i < pMarks.GetLength(0); i++)
                    {
                        var row = new int[pMarks.GetLength(1)];
                        for (int j = 0; j < row.Length; j++) { row[j] = pMarks[i, j]; }
                        participant.Jump(row);
                    }
                    comp.Add(participant);
                }

                return comp as T;
            }

            return null;
        }

        public override void SerializePurple2SkiJumping<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            var dict = new Dictionary<string, string>
            {
                ["Type"] = obj is Purple_2.JuniorSkiJumping ? "JuniorSkiJumping" : "ProSkiJumping",
                ["Name"] = obj.Name,
                ["Standard"] = obj.Standard.ToString(),
                ["ParticipantsCount"] = obj.Participants.Length.ToString()
            };

            for (int i = 0; i < obj.Participants.Length; i++)
            {
                var p = obj.Participants[i];
                dict[$"P_{i}_Name"] = p.Name;
                dict[$"P_{i}_Surname"] = p.Surname;
                dict[$"P_{i}_Distance"] = p.Distance.ToString();
                dict[$"P_{i}_Result"] = p.Result.ToString();
                dict[$"P_{i}_Marks"] = ArrayToString(p.Marks);
            }

            DictToFile(dict);
        }



        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var data = FileToDict();

            Purple_2.SkiJumping jumpEvent = data["Type"] == "JuniorSkiJumping"
                ? new Purple_2.JuniorSkiJumping()
                : new Purple_2.ProSkiJumping();

            int count = int.Parse(data["ParticipantsCount"]);
            for (int i = 0; i < count; i++)
            {
                string name = data[$"P_{i}_Name"];
                string surname = data[$"P_{i}_Surname"];
                int distance = int.Parse(data[$"P_{i}_Distance"]);
                var scores = StringToArray(data[$"P_{i}_Marks"], int.Parse);

                var p = new Purple_2.Participant(name, surname);
                p.Jump(distance, scores, jumpEvent.Standard);
                jumpEvent.Add(p);
            }

            return jumpEvent as T;
        }

        public override void SerializePurple3Skating<T>(T obj, string fileName)
        {
            SelectFile(fileName);

            var dict = new Dictionary<string, string>
            {
                ["Type"] = obj is Purple_3.IceSkating ? "IceSkating" : "FigureSkating",
                ["Moods"] = ArrayToString(obj.Moods),
                ["ParticipantsCount"] = obj.Participants.Length.ToString()
            };

            for (int i = 0; i < obj.Participants.Length; i++)
            {
                var participant = obj.Participants[i];
                dict[$"P_{i}_Name"] = participant.Name;
                dict[$"P_{i}_Surname"] = participant.Surname;
                dict[$"P_{i}_Marks"] = ArrayToString(participant.Marks);
            }

            DictToFile(dict);
        }



        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var data = FileToDict();

            var moodValues = StringToArray(data["Moods"], double.Parse);

            Purple_3.Skating skating = data["Type"] == "IceSkating"
                ? new Purple_3.IceSkating(moodValues, false)
                : new Purple_3.FigureSkating(moodValues, false);

            int count = int.Parse(data["ParticipantsCount"]);
            for (int i = 0; i < count; i++)
            {
                var firstName = data[$"P_{i}_Name"];
                var lastName = data[$"P_{i}_Surname"];
                var markArray = StringToArray(data[$"P_{i}_Marks"], double.Parse);

                var person = new Purple_3.Participant(firstName, lastName);
                foreach (var mark in markArray)
                {
                    person.Evaluate(mark);
                }

                skating.Add(person);
            }

            return skating as T;
        }

        public override void SerializePurple4Group(Purple_4.Group obj, string fileName)
        {
            SelectFile(fileName);

            var dict = new Dictionary<string, string>
            {
                ["Type"] = "Group",
                ["Name"] = obj.Name,
                ["Count"] = obj.Sportsmen.Length.ToString()
            };

            for (int i = 0; i < obj.Sportsmen.Length; i++)
            {
                var sportsman = obj.Sportsmen[i];
                dict[$"S_{i}_Name"] = sportsman.Name;
                dict[$"S_{i}_Surname"] = sportsman.Surname;
                dict[$"S_{i}_Time"] = sportsman.Time.ToString();
            }

            DictToFile(dict);
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var data = FileToDict();

            var group = new Purple_4.Group(data["Name"]);
            int count = int.Parse(data["Count"]);

            for (int i = 0; i < count; i++)
            {
                string first = data[$"S_{i}_Name"];
                string last = data[$"S_{i}_Surname"];
                double time = double.Parse(data[$"S_{i}_Time"]);

                var sportsman = new Purple_4.Sportsman(first, last);
                sportsman.Run(time);

                group.Add(sportsman);
            }

            return group;
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            var dict = new Dictionary<string, string>
            {
                ["Type"] = "Report",
                ["ResearchCount"] = report.Researches.Length.ToString()
            };

            for (int i = 0; i < report.Researches.Length; i++)
            {
                var research = report.Researches[i];
                dict[$"Research_{i}_Name"] = research.Name;
                dict[$"Research_{i}_ResponseCount"] = research.Responses.Length.ToString();

                for (int j = 0; j < research.Responses.Length; j++)
                {
                    var response = research.Responses[j];
                    dict[$"Research_{i}_Response_{j}_Animal"] = response.Animal ?? "null";
                    dict[$"Research_{i}_Response_{j}_Trait"] = response.CharacterTrait ?? "null";
                    dict[$"Research_{i}_Response_{j}_Concept"] = response.Concept ?? "null";
                }
            }

            DictToFile(dict);
        }



        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var dict = FileToDict();
            var report = new Purple_5.Report();

            int researchCount = int.Parse(dict["ResearchCount"]);
            for (int i = 0; i < researchCount; i++)
            {
                var researchName = dict[$"Research_{i}_Name"];
                var research = new Purple_5.Research(researchName);

                int responseCount = int.Parse(dict[$"Research_{i}_ResponseCount"]);
                for (int j = 0; j < responseCount; j++)
                {
                    var animal = dict[$"Research_{i}_Response_{j}_Animal"] == "null"
                        ? null
                        : dict[$"Research_{i}_Response_{j}_Animal"];

                    var trait = dict[$"Research_{i}_Response_{j}_Trait"] == "null"
                        ? null
                        : dict[$"Research_{i}_Response_{j}_Trait"];

                    var concept = dict[$"Research_{i}_Response_{j}_Concept"] == "null"
                        ? null
                        : dict[$"Research_{i}_Response_{j}_Concept"];

                    research.Add(new[] { animal, trait, concept });
                }

                report.AddResearch(research);
            }

            return report;
        }




    }
}