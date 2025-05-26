using System.Text.Json;
using Lab_7;

namespace Lab_9
{
    public class PurpleJSONSerializer : PurpleSerializer
    {
        public override string Extension => "json";
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            string json = File.ReadAllText(FilePath);
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;

            if (root.TryGetProperty("Coefs", out _))
            {
                Console.WriteLine("Deserialize Participant");
                var dao = JsonSerializer.Deserialize<Purple_1_Participant_DAO>(json);
                return (T)(Object)dao.ToObject();
            }
            if (root.TryGetProperty("FavMarks", out _))
            {
                Console.WriteLine("Deserialize Judge");
                var dao = JsonSerializer.Deserialize<Purple_1_Judge_DAO>(json);
                return (T)(Object)dao.ToObject();
            }
            if (root.TryGetProperty("Participants", out _))
            {
                Console.WriteLine("Deserialize Competition");
                var dao = JsonSerializer.Deserialize<Purple_1_Competition_DAO>(json);
                return (T)(Object)dao.ToObject();
            }

            return default;
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            string json = File.ReadAllText(FilePath);

            var dao = JsonSerializer.Deserialize<Purple_2_Ski_Jumping_DAO>(json);
            return (T)(Object)dao.ToObject();
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            string json = File.ReadAllText(FilePath);
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            string skatingKind = root.GetProperty("SkatingKind").GetString();

            if (skatingKind == nameof(Purple_3.IceSkating))
            {
                var dao = JsonSerializer.Deserialize<Purple_3_Scating_DAO<Purple_3.IceSkating>>(json);
                return (T)(Object)dao.ToObject();
            }
            else if (skatingKind == nameof(Purple_3.FigureSkating))
            {
                var dao = JsonSerializer.Deserialize<Purple_3_Scating_DAO<Purple_3.FigureSkating>>(json);
                return (T)(Object)dao.ToObject();
            }

            return default;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            string json = File.ReadAllText(FilePath);
            var dao = JsonSerializer.Deserialize<Purple_4_Group_DTO>(json);
            return dao.ToObject();
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            string json = File.ReadAllText(FilePath);
            var dao = JsonSerializer.Deserialize<Purple_5_Report_DTO>(json);
            return dao.ToObject();
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            string json = "";
            if (obj is Purple_1.Participant p)
            {
                var dao = new Purple_1_Participant_DAO(p);
                json = JsonSerializer.Serialize(dao, JsonSerializerOptions.Default);
            }
            if (obj is Purple_1.Judge j)
            {
                var dao = new Purple_1_Judge_DAO(j);
                json = JsonSerializer.Serialize(dao, JsonSerializerOptions.Default);
            }
            if (obj is Purple_1.Competition c)
            {
                var dao = new Purple_1_Competition_DAO(c);
                json = JsonSerializer.Serialize(dao, JsonSerializerOptions.Default);
            }
            if (string.IsNullOrEmpty(json)) return;
            SelectFile(fileName);
            File.WriteAllText(FilePath, json);
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            var dao = new Purple_2_Ski_Jumping_DAO(jumping.Name, jumping.Standard, jumping.Participants.Select(p => new Purple_2_Participant_DAO(p)).ToArray());
            string json = JsonSerializer.Serialize(dao, JsonSerializerOptions.Default);

            SelectFile(fileName);
            File.WriteAllText(FilePath, json);
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            var dao = new Purple_3_Scating_DAO<T>(skating);
            string json = JsonSerializer.Serialize(dao, JsonSerializerOptions.Default);

            SelectFile(fileName);
            File.WriteAllText(FilePath, json);
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            var dao = new Purple_4_Group_DTO(group);
            string json = JsonSerializer.Serialize(dao, JsonSerializerOptions.Default);

            SelectFile(fileName);
            File.WriteAllText(FilePath, json);
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            var dao = new Purple_5_Report_DTO(report);
            string json = JsonSerializer.Serialize(dao, JsonSerializerOptions.Default);

            SelectFile(fileName);
            File.WriteAllText(FilePath, json);
        }
    }
}
