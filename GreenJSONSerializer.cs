using System;
 using System.IO;
 using Newtonsoft.Json;
 using System.Text.Json;
 using System.Text.Json.Serialization;
 using System.Linq;
 using Lab_7;
 using static Lab_7.Green_4;
 using System.Diagnostics;
 using Newtonsoft.Json.Linq;
using System.Globalization;


namespace Lab_9 {
    public class GreenJSONSerializer : GreenSerializer {
        public override string Extension =>"json";
        private class ResponseDTO
        {
            public string Type { get; set; } 
            public string Surname { get; set; } 
            public string Group { get; set; } 
            public string Trainer { get; set; } 
            public double Result { get; set; } 

            public ResponseDTO() { }

            public ResponseDTO(Green_1.Participant participant) {
                Type = participant.GetType().Name;
                Surname = participant.Surname;
                Group = participant.Group;
                Trainer = participant.Trainer;
                Result = participant.Result;
            }
        }

        public override void SerializeGreen1Participant(Green_1.Participant participant, string fileName)
        {
            if (participant == null || String.IsNullOrEmpty(fileName)) {return;}
            SelectFile(fileName);
            var participantDTO = new ResponseDTO(participant);
            var json = JsonConvert.SerializeObject(participantDTO, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public override Green_1.Participant DeserializeGreen1Participant(string fileName)
        {
            SelectFile(fileName);
            Green_1.Participant deserialized = default(Green_1.Participant);
            string json = File.ReadAllText(FilePath);
            var dto = JsonConvert.DeserializeObject<ResponseDTO>(json);
            if (dto.Type == "Participant100M") {
                deserialized = new Green_1.Participant100M(dto.Surname, dto.Group, dto.Trainer);
            }
            else if (dto.Type == "Participant500M") {
                deserialized = new Green_1.Participant500M(dto.Surname, dto.Group, dto.Trainer);
            }
            if (deserialized != null && dto.Result > 0)
            {
                deserialized.Run(dto.Result);
            }
            return deserialized;
        }
        private void Serializer_json<T>(T obj, string fileName) where T : class
        {
            SelectFile(fileName);
            if (obj == null || FilePath == null) return;
            var json = JObject.FromObject(obj);
            json.Add("Type", obj.GetType().ToString());
            File.WriteAllText(FilePath, json.ToString());
        }
        public override void SerializeGreen2Human(Green_2.Human human, string fileName)
        {
            Serializer_json(human, fileName);
        }
        public override Green_2.Human DeserializeGreen2Human(string fileName)
        {
            SelectFile(fileName);
            string json = File.ReadAllText(FilePath);
            var deserializedPerson = JObject.Parse(json);
            string type = deserializedPerson["Type"].ToString();
            string name = deserializedPerson["Name"].ToString();
            string surname = deserializedPerson["Surname"].ToString();

            if (type == typeof(Green_2.Student).ToString())
            {
                var obj = new Green_2.Student(name, surname);
                int[] marks = deserializedPerson["Marks"].ToObject<int[]>();

                foreach (var m in marks)
                    obj.Exam(m);
                return obj;
            }
            else if (type == typeof(Green_2.Human).ToString())
            {
                var obj = new Green_2.Human(name, deserializedPerson["Surname"].ToString());
                return obj;
            }
            else return null;
        }
        public override void SerializeGreen3Student(Green_3.Student student, string fileName)
        {
            Serializer_json(student, fileName);
        }
        public override Green_3.Student DeserializeGreen3Student(string fileName)
        {
            SelectFile(fileName);
            string json = File.ReadAllText(FilePath);
            var deserializedPerson = JObject.Parse(json);

            string type = deserializedPerson["Type"].ToString();
            string name = deserializedPerson["Name"].ToString();
            string surname = deserializedPerson["Surname"].ToString();
            int[] marks = deserializedPerson["Marks"].ToObject<int[]>();
            int id = (int)deserializedPerson["ID"];

            var obj = new Green_3.Student(name, surname, id);

            foreach (var mark in marks)
            {
                obj.Exam(mark);
            }

            return obj;
        }
        public override void SerializeGreen4Discipline(Green_4.Discipline discipline, string fileName)
        {
            Serializer_json(discipline, fileName);
        }

        public override Green_4.Discipline DeserializeGreen4Discipline(string fileName)
        {
            SelectFile(fileName);
            string json = File.ReadAllText(FilePath);
            var deserializedPerson = JObject.Parse(json);
            string type = deserializedPerson["Type"].ToString();
            string name = deserializedPerson["Name"].ToString();
            var participantsData = deserializedPerson["Participants"].ToObject<List<JObject>>();

            Green_4.Discipline obj = default(Green_4.Discipline);

            if (type == typeof(Green_4.LongJump).ToString())
            {
                obj = new Green_4.LongJump();
            }
            else if (type == typeof(Green_4.HighJump).ToString())
            {
                obj = new Green_4.HighJump();
            }

            foreach (var pData in participantsData)
            {
                var participant = new Green_4.Participant(pData["Name"].ToString(), pData["Surname"].ToString());   
                var jumps = pData["Jumps"].ToObject<List<double>>();
                foreach (var jump in jumps)
                {
                    participant.Jump(jump);
                }
                obj.Add(participant);
            }
            return obj;
        }

        public override void SerializeGreen5Group<T>(T group, string fileName)
        {
            Serializer_json(group, fileName);
        }

        public override T DeserializeGreen5Group<T>(string fileName)
        {
            SelectFile(fileName);
            string json = File.ReadAllText(FilePath);
            var deserializedPerson = JObject.Parse(json);
            string type = deserializedPerson["Type"].ToString();
            string name = deserializedPerson["Name"].ToString();
            var studentsData = deserializedPerson["Students"].ToObject<List<JObject>>();

            // Извлекаем короткое имя типа из полного имени
            string typeName = type.Split('.').Last();
            if (typeName.Contains('+'))
            {
                typeName = typeName.Split('+').Last(); // Обрабатываем вложенные классы
            }

            T group;

            if (typeName == nameof(Green_5.EliteGroup))
            {
                group = (T)(object)new Green_5.EliteGroup(name);
            }
            else if (typeName == nameof(Green_5.SpecialGroup))
            {
                group = (T)(object)new Green_5.SpecialGroup(name);
            }
            else
            {
                group = (T)(object)new Green_5.Group(name);
            }

            foreach (var sData in studentsData)
            {
                var student = new Green_5.Student(sData["Name"].ToString(), sData["Surname"].ToString());
                var marks = sData["Marks"].ToObject<List<int>>();
                foreach (int mark in marks)
                {
                    student.Exam(mark);
                }
                group.Add(student);
            }
            return group;
            return null;
        }
    }
}