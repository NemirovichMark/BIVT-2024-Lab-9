using System.Xml.Serialization;
using Lab_7;

namespace Lab_9
{
    public class PurpleXMLSerializer : PurpleSerializer
    {
        public override string Extension => "xml";
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            var stream = new StreamReader(FilePath);
            Console.WriteLine("БЛЯТЬ Я ЗАЕБАЛСЯ ТЕБЯ ЧИНИТЬ");

            if (typeof(T) == typeof(Purple_1.Participant))
            {
                var serializer = new XmlSerializer(typeof(Purple_1_Participant_DAO));
                var dao = (Purple_1_Participant_DAO)serializer.Deserialize(stream);
                stream.Close();
                return (T)(Object)dao.ToObject();
            }
            if (typeof(T) == typeof(Purple_1.Judge))
            {
                var serializer = new XmlSerializer(typeof(Purple_1_Judge_DAO));
                var dao = (Purple_1_Judge_DAO)serializer.Deserialize(stream);
                stream.Close();
                return (T)(Object)dao.ToObject();
            }
            if (typeof(T) == typeof(Purple_1.Competition))
            {
                Console.WriteLine("ХУЕСОС БЛЯТЬ РАБОТАЙ");
                var serializer = new XmlSerializer(typeof(Purple_1_Competition_DAO));
                var dao = (Purple_1_Competition_DAO)serializer.Deserialize(stream);
                stream.Close();
                Console.WriteLine(string.Join(' ', dao.Participants.AsEnumerable()));
                return (T)(Object)dao.ToObject();
            }

            return default;
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            SelectFile(fileName);
            var stream = new StreamReader(FilePath);
            var serializer = new XmlSerializer(typeof(Purple_2_Ski_Jumping_DAO));
            var dao = (Purple_2_Ski_Jumping_DAO)serializer.Deserialize(stream);
            stream.Close();
            return (T)(Object)dao.ToObject();
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            SelectFile(fileName);
            var stream = new StreamReader(FilePath);

            if (typeof(T) == typeof(Purple_3.IceSkating))
            {
                var serializer = new XmlSerializer(typeof(Purple_3_Scating_DAO<Purple_3.IceSkating>));
                var dao = (Purple_3_Scating_DAO<Purple_3.IceSkating>)serializer.Deserialize(stream);
                stream.Close();
                return (T)(Object)dao.ToObject();
            }
            else if (typeof(T) == typeof(Purple_3.FigureSkating))
            {
                var serializer = new XmlSerializer(typeof(Purple_3_Scating_DAO<Purple_3.FigureSkating>));
                var dao = (Purple_3_Scating_DAO<Purple_3.FigureSkating>)serializer.Deserialize(stream);
                stream.Close();
                return (T)(Object)dao.ToObject();
            }
            else if (typeof(T) == typeof(Purple_3.Skating))
            {

                var serializer = new XmlSerializer(typeof(Purple_3_Scating_DAO<Purple_3.Skating>));
                var dao = (Purple_3_Scating_DAO<Purple_3.Skating>)serializer.Deserialize(stream);
                stream.Close();
                return (T)(Object)dao.ToObject();
            }

            return default;
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            SelectFile(fileName);
            var stream = new StreamReader(FilePath);
            var serializer = new XmlSerializer(typeof(Purple_4_Group_DTO));
            var dao = (Purple_4_Group_DTO)serializer.Deserialize(stream);
            stream.Close();
            return dao.ToObject();
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            SelectFile(fileName);
            var stream = new StreamReader(FilePath);
            var serializer = new XmlSerializer(typeof(Purple_5_Report_DTO));
            var dao = (Purple_5_Report_DTO)serializer.Deserialize(stream);
            stream.Close();
            return dao.ToObject();
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            SelectFile(fileName);
            var stream = new StreamWriter(FilePath);
            if (obj is Purple_1.Participant p)
            {
                var serializer = new XmlSerializer(typeof(Purple_1_Participant_DAO));
                var dao = new Purple_1_Participant_DAO(p);
                serializer.Serialize(stream, dao);
                stream.Close();
            }
            if (obj is Purple_1.Judge j)
            {
                var serializer = new XmlSerializer(typeof(Purple_1_Judge_DAO));
                var dao = new Purple_1_Judge_DAO(j);
                serializer.Serialize(stream, dao);
                stream.Close();
            }
            if (obj is Purple_1.Competition c)
            {
                var serializer = new XmlSerializer(typeof(Purple_1_Competition_DAO));
                var dao = new Purple_1_Competition_DAO(c);
                serializer.Serialize(stream, dao);
                stream.Close();
            }
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            SelectFile(fileName);
            var stream = new StreamWriter(FilePath);
            var serializer = new XmlSerializer(typeof(Purple_2_Ski_Jumping_DAO));
            var dao = new Purple_2_Ski_Jumping_DAO(jumping.Name, jumping.Standard, jumping.Participants.Select(p => new Purple_2_Participant_DAO(p)).ToArray());
            serializer.Serialize(stream, dao);
            stream.Close();
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            SelectFile(fileName);
            var stream = new StreamWriter(FilePath);
            var serializer = new XmlSerializer(typeof(Purple_3_Scating_DAO<T>));
            var dao = new Purple_3_Scating_DAO<T>(skating);
            serializer.Serialize(stream, dao);
            stream.Close();
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            SelectFile(fileName);
            var stream = new StreamWriter(FilePath);
            var serializer = new XmlSerializer(typeof(Purple_4_Group_DTO));
            var dao = new Purple_4_Group_DTO(group);
            serializer.Serialize(stream, dao);
            stream.Close();
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            SelectFile(fileName);
            var stream = new StreamWriter(FilePath);
            var serializer = new XmlSerializer(typeof(Purple_5_Report_DTO));
            var dao = new Purple_5_Report_DTO(report);
            serializer.Serialize(stream, dao);
            stream.Close();
        }
    }
}
