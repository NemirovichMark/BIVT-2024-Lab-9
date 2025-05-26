using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using Lab_7;
using Newtonsoft.Json;

namespace Lab_9;
public class PurpleJSONSerializer : PurpleSerializer
{

    public override string Extension => "json";

    public class ParticipantDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        //public double TotalScore { get; set; }
        public double[] Coefs { get; set; }
        public int[][] Marks { get; set; }
    }

    public class JudgeDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public int[] Marks { get; set; }
    }

    public class CompetitionDTO
    {
        public string Type { get; set; }
        public JudgeDTO[] Judges { get; set; }
        public ParticipantDTO[] Participants { get; set; }
    }
    public class Purple2_ParticipantDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Distance { get; set; }
        public int Result { get; set; }
        public int[] Marks { get; set; }
    }
    public class SkiJumpingDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public int Standard { get; set; }

        public Purple2_ParticipantDTO[] Participants { get; set; }
    }

    public class Purple3_ParticipantDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int[] Places { get; set; }
        public int Score { get; set; }
        public double[] Marks { get; set; }
    }

    public class SkatingDTO
    {
        public string Type { get; set; }
        public Purple3_ParticipantDTO[] Participants { get; set; }
        public double[] Moods { get; set; }
    }

    public class SportsmanDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public double Time { get; set; }
    }

    public class GroupDTO
    {
        public string Name { get; set; }
        public SportsmanDTO[] Sportsmen { get; set; }
    }

    public class ResponseDTO
    {
        public string Animal { get; set; }
        public string CharacterTrait { get; set; }
        public string Concept { get; set; }
    }

    public class ResearchDTO
    {
        public string Name { get; set; }
        public ResponseDTO[] Responses { get; set; }
    }

    public class ReportDTO
    {
        public ResearchDTO[] Researches { get; set; }
    }


    public override void SerializePurple1<T>(T obj, string fileName)
    {
        var allowedTypes = new[]
        {
        typeof(Purple_1.Participant),
        typeof(Purple_1.Competition),
        typeof(Purple_1.Judge)
        };

        if (!allowedTypes.Contains(obj?.GetType()))
        {
            return;
        }

        SelectFile(fileName);
        var jsonSettings = CreateJsonSettings();
        var jsonContent = JsonConvert.SerializeObject(obj, jsonSettings);
        File.WriteAllText(FilePath, jsonContent);
    }

    public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
    {
        if (jumping == null) { return; }

        SelectFile(fileName);
        var jsonSettings = CreateJsonSettings();
        var jsonContent = JsonConvert.SerializeObject(jumping, jsonSettings);
        File.WriteAllText(FilePath, jsonContent);
    }

    public override void SerializePurple3Skating<T>(T skating, string fileName)
    {
        if (skating == null) { return; }

        SelectFile(fileName);
        var jsonSettings = CreateJsonSettings();
        var jsonContent = JsonConvert.SerializeObject(skating, jsonSettings);
        File.WriteAllText(FilePath, jsonContent);
    }


    public override void SerializePurple4Group(Purple_4.Group group, string fileName)
    {
        if (group == null) { return; }

        SelectFile(fileName);
        var jsonContent = JsonConvert.SerializeObject(group, Formatting.Indented);
        File.WriteAllText(FilePath, jsonContent);
    }

    public override void SerializePurple5Report(Purple_5.Report report, string fileName)
    {
        if (report == null) { return; }

        SelectFile(fileName);
        var jsonContent = JsonConvert.SerializeObject(report, Formatting.Indented);
        File.WriteAllText(FilePath, jsonContent);
    }



    public override T DeserializePurple1<T>(string fileName)
    {
        SelectFile(fileName);
        var jsonContent = File.ReadAllText(FilePath);
        var jsonObject = JObject.Parse(jsonContent);
        var objectType = (string)jsonObject["$type"]; //$ обозначение служебных данных в библиотеке

        switch (objectType)
        {
            case "Lab_7.Purple_1+Judge, Lab_7":
                var judgeName = (string)jsonObject["Name"];
                var judgeMarks = jsonObject["Marks"].Select(mark => (int)mark).ToArray();
                return new Purple_1.Judge(judgeName, judgeMarks) as T;

            case "Lab_7.Purple_1+Participant, Lab_7":
                var participantName = (string)jsonObject["Name"];
                var participantSurname = (string)jsonObject["Surname"];
                var participant = new Purple_1.Participant(participantName, participantSurname);

                var coefs = jsonObject["Coefs"].Select(coef => (double)coef).ToArray();
                participant.SetCriterias(coefs);

                var marksArray = ((JArray)jsonObject["Marks"])
                    .Select(row => row.Values<int>().ToArray())
                    .ToArray();
                var rectangularMarks = ConvertToMatrix(marksArray);

                for (var round = 0; round < rectangularMarks.GetLength(0); round++)
                {
                    var roundMarks = new int[rectangularMarks.GetLength(1)];
                    for (var judge = 0; judge < roundMarks.Length; judge++)
                    {
                        roundMarks[judge] = rectangularMarks[round, judge];
                    }
                    participant.Jump(roundMarks);
                }
                return participant as T;

            case "Lab_7.Purple_1+Competition, Lab_7":
                var judgesData = ((JArray)jsonObject["Judges"]).ToObject<JudgeDTO[]>();
                var judges = judgesData
                    .Select(judgeDto => new Purple_1.Judge(judgeDto.Name, judgeDto.Marks))
                    .ToArray();

                var participantsData = ((JArray)jsonObject["Participants"]).ToObject<ParticipantDTO[]>();
                var participants = participantsData
                    .Select(participantDto =>
                    {
                        var participant = new Purple_1.Participant(participantDto.Name, participantDto.Surname);
                        participant.SetCriterias(participantDto.Coefs);

                        for (var jumpIndex = 0; jumpIndex < participantDto.Marks.Length; jumpIndex++)
                        {
                            participant.Jump(participantDto.Marks[jumpIndex]);
                        }
                        return participant;
                    })
                    .ToArray();

                var competition = new Purple_1.Competition(judges);
                foreach (var p in participants)
                {
                    competition.Add(p);
                }
                return competition as T;

            default:
                return null as T;
        }
    }

    public override T DeserializePurple2SkiJumping<T>(string fileName)
    {
        SelectFile(fileName);
        var json = File.ReadAllText(FilePath);
        var jsonObject = JObject.Parse(json);

        var participants = ((JArray)jsonObject["Participants"]).ToObject<Purple2_ParticipantDTO[]>();

        Purple_2.SkiJumping jumping = (string)jsonObject["$type"] == "Lab_7.Purple_2+JuniorSkiJumping, Lab_7" ? new Purple_2.JuniorSkiJumping() : new Purple_2.ProSkiJumping();
        foreach (var p in participants)
        {
            var participant = new Purple_2.Participant(p.Name, p.Surname);
            participant.Jump(p.Distance, p.Marks, jumping.Standard);
            jumping.Add(participant);
        }


        return jumping as T;
    }



    public override T DeserializePurple3Skating<T>(string fileName)
    {
        SelectFile(fileName);
        var json = File.ReadAllText(FilePath);
        var jsonObject = JObject.Parse(json);

        var dtoArr = jsonObject["Participants"]
            .ToObject<Purple3_ParticipantDTO[]>();

        var moods = jsonObject["Moods"]
            .Select(token => (double)token)
            .ToArray();

        var type = (string)jsonObject["$type"];
        Purple_3.Skating skatingEvent = type.Contains("IceSkating")
            ? new Purple_3.IceSkating(moods, needModificate: false)
            : new Purple_3.FigureSkating(moods, needModificate: false);


        foreach (var dto in dtoArr)
        {
            var skater = new Purple_3.Participant(dto.Name, dto.Surname);

            foreach (var mark in dto.Marks)
            {
                skater.Evaluate(mark);
            }

            skatingEvent.Add(skater);
        }

        return skatingEvent as T;
    }

    public override Purple_4.Group DeserializePurple4Group(string fileName)
    {
        SelectFile(fileName);
        string json = File.ReadAllText(FilePath);
        var jsonObj = JObject.Parse(json);

        var sportsmenDtos = ((JArray)jsonObj["Sportsmen"]).ToObject<SportsmanDTO[]>();
        var sportsmen = sportsmenDtos
            .Select(dto => {
                var s = new Purple_4.Sportsman(dto.Name, dto.Surname);
                s.Run(dto.Time);
                return s;
            })
            .ToArray();

        var group = new Purple_4.Group((string)jsonObj["Name"]);
        group.Add(sportsmen);
        return group;
    }


    public override Purple_5.Report DeserializePurple5Report(string fileName)
    {
        SelectFile(fileName);
        string json = File.ReadAllText(FilePath);
        var jsonObj = JObject.Parse(json);
        var report = new Purple_5.Report();

        var researches = ((JArray)jsonObj["Researches"])
            .ToObject<ResearchDTO[]>()
            .Select(dto =>
            {
                var research = new Purple_5.Research(dto.Name);
                foreach (var resp in dto.Responses)
                {
                    research.Add([resp.Animal, resp.CharacterTrait, resp.Concept]);
                }
                return research;
            })
            .ToArray();

        foreach (var r in researches)
        {
            report.AddResearch(r);
        }

        return report;
    }


    private JsonSerializerSettings CreateJsonSettings()
    {
        return new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Objects
        };
    }

    private T[,] ConvertToMatrix<T>(T[][] jagged)
    {
        var rowCount = jagged.Length;
        var colCount = jagged[0].Length;
        var result = new T[rowCount, colCount];

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                result[row, col] = jagged[row][col];
            }
        }

        return result;
    }

    private T[][] ConvertToJagged<T>(T[,] matrix)
    {
        var rows = matrix.GetLength(0);
        var cols = matrix.GetLength(1);
        var jagged = new T[rows][];

        for (int i = 0; i < rows; i++)
        {
            jagged[i] = new T[cols];
            for (int j = 0; j < cols; j++)
            { 
                jagged[i][j] = matrix[i, j];
            }
        }

        return jagged;
    }

}
