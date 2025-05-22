using Lab_7;

namespace Lab_9
{
    public class GreenJSONSerializer : GreenSerializer // Публичный класс для сериализации и десирал. json.
    {
        public override string Extension => ".json"; // Переопределяем расширение на .json.
        public override void SerializeGreen1Participant(Green_1.Participant participant, string fileIdentifier) // Метод сериализации участниц.
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла.
            var Document = new Newtonsoft.Json.Linq.JObject // Создаём новый json.
            {
                ["Surname"] = participant.Surname, // Заполняем фамилию.
                ["Group"] = participant.Group, // Заполняем группу.
                ["Trainer"] = participant.Trainer, // Заполняем тренера. 
                ["Result"] = participant.Result, // Заполняем резултат.
                ["Discipline"] = participant is Green_1.Participant100M ? "100M" : "500M" // Заполняем дисциплину в зависимости от её типа.
            };
            try
            {
                File.WriteAllText(fullPath, Document.ToString(Newtonsoft.Json.Formatting.Indented)); // Записываем json с нужным оформлением.
            }
            catch (Exception ex) // Если что-то не получилось, то
            {
                Console.WriteLine($"Ошибка при записи файла: {ex.Message}"); // пишем об этом.
            }
        }
        public override Green_1.Participant DeserializeGreen1Participant(string fileIdentifier) // Метод десериал. участниц.
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла. 
            string jsonContent = File.ReadAllText(fullPath); // Считывем весь текст.
            var document = Newtonsoft.Json.Linq.JObject.Parse(jsonContent); // Парсим.
            string surname = document.Value<string>("Surname") ?? throw new InvalidOperationException("Missing Surname"); // Считываем фамилию.
            string group = document.Value<string>("Group") ?? throw new InvalidOperationException("Missing Group"); // Считываем группу.
            string trainer = document.Value<string>("Trainer") ?? throw new InvalidOperationException("Missing Trainer"); // Считываем тренера.
            double result = document.Value<double?>("Result") ?? throw new InvalidOperationException("Missing Result"); // Считываем результат.
            string discipline = document.Value<string>("Discipline") ?? throw new InvalidOperationException("Missing Discipline"); // Считываем дисциплину.
            Green_1.Participant participant = discipline == "100M" ? new Green_1.Participant100M(surname, group, trainer) : new Green_1.Participant500M(surname, group, trainer); 
            participant.Run(result); // Записываем результат (пробегаем).
            return participant; // Возвращаем участницу.
        }
        public override void SerializeGreen2Human(Green_2.Human human, string fileIdentifier) // Метод сериализации людей. 
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла.
            var Document = new Newtonsoft.Json.Linq.JObject // Создаём новый json.
            {
                ["Name"] = human.Name, // Заполняем имя.
                ["Surname"] = human.Surname, // Заполняем фамилию.
            };
            if (human is Green_2.Student student) // Если человек является студентом, то
            {
                Document["Marks"] = new Newtonsoft.Json.Linq.JArray(student.Marks); // записываем его оценки.
            }
            try
            {
                File.WriteAllText(fullPath, Document.ToString(Newtonsoft.Json.Formatting.Indented)); // Записываем json с нужным оформлением.
            }
            catch (Exception ex) // Если что-то не получилось, то
            {
                Console.WriteLine($"Ошибка при записи файла: {ex.Message}"); // пишем об этом.
            }
        }
        public override Green_2.Human DeserializeGreen2Human(string fileIdentifier) // Метод десериал. людей. 
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла. 
            string jsonContent = File.ReadAllText(fullPath); // Считывем весь текст.
            var document = Newtonsoft.Json.Linq.JObject.Parse(jsonContent); // Парсим.
            string name = document.Value<string>("Name") ?? throw new InvalidOperationException("Missing Name"); // Считываем имя.
            string surname = document.Value<string>("Surname") ?? throw new InvalidOperationException("Missing Surname"); // Считываем фамилию.
            Green_2.Human human; // Создаём человека.
            if (document.TryGetValue("Marks", out var marks) && marks is Newtonsoft.Json.Linq.JArray marksArray) // Если получены оценки, то
            {
                var student = new Green_2.Student(name, surname); // Создаём студента.
                foreach (var mark in marksArray.Values<int>()) // Заполняем оценки.
                {
                    student.Exam(mark); // Заполнение.
                }
                human = student; // Приравниваем человека к студенту.
            }
            else
            {
                human = new Green_2.Human(name, surname); // Создаём человека с ФИ.
            }
            return human; // Возвращаем человека.
        }
        public override void SerializeGreen3Student(Green_3.Student student, string fileIdentifier) // Метод сериализации студентов.
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла.
            var Document = new Newtonsoft.Json.Linq.JObject // Создаём новый json.
            {
                ["Name"] = student.Name, // Заполняем имя.
                ["Surname"] = student.Surname, // Заполняем фамилию.
                ["ID"] = student.ID, // Заполняем айдишник.
                ["Marks"] = new Newtonsoft.Json.Linq.JArray(student.Marks) // Заполняем оценки.
            };
            try
            {
                File.WriteAllText(fullPath, Document.ToString(Newtonsoft.Json.Formatting.Indented)); // Записываем json с нужным оформлением.
            }
            catch (Exception ex) // Если что-то не получилось, то
            {
                Console.WriteLine($"Ошибка при записи файла: {ex.Message}"); // пишем об этом.
            }
        }
        public override Green_3.Student DeserializeGreen3Student(string fileIdentifier) // Метод десериал. студентов. 
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла. 
            string jsonContent = File.ReadAllText(fullPath); // Считывем весь текст.
            var document = Newtonsoft.Json.Linq.JObject.Parse(jsonContent); // Парсим.
            string name = document.Value<string>("Name") ?? throw new InvalidOperationException("Missing Name"); // Считываем имя.
            string surname = document.Value<string>("Surname") ?? throw new InvalidOperationException("Missing Surname"); // Считываем фамилию.
            int id = document.Value<int?>("ID") ?? throw new InvalidOperationException("Missing ID");  // Считываем айдишник.
            var student = new Green_3.Student(name, surname, id); // Создаём студента.
            if (document.TryGetValue("Marks", out var marksToken) && marksToken is Newtonsoft.Json.Linq.JArray marksArr) // Если получены оценки, то
            {
                foreach (var mark in marksArr.Values<int>())
                {
                    student.Exam(mark); // Заполняем их.
                }
            }
            return student; // Возвращаем студента.
        }
        public override void SerializeGreen4Discipline(Green_4.Discipline discipline, string fileIdentifier) // Метод сериализации дисциплин.
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла.
            var Document = new Newtonsoft.Json.Linq.JObject // Создаём новый json.
            {
                ["Type"] = discipline.GetType().Name, // Заполняем тип.
                ["Participants"] = new Newtonsoft.Json.Linq.JArray( // Заполняем участниц.
                    discipline.Participants.Select(p => new Newtonsoft.Json.Linq.JObject
                    { ["Name"] = p.Name, ["Surname"] = p.Surname, ["Jumps"] = new Newtonsoft.Json.Linq.JArray(p.Jumps)}) // Имя, фамилия и прыжки. Заполняем их.
                )
            };
            try
            {
                File.WriteAllText(fullPath, Document.ToString(Newtonsoft.Json.Formatting.Indented)); // Записываем json с нужным оформлением.
            }
            catch (Exception ex) // Если что-то не получилось, то
            {
                Console.WriteLine($"Ошибка при записи файла: {ex.Message}"); // пишем об этом.
            }
        }
        public override Green_4.Discipline DeserializeGreen4Discipline(string fileIdentifier) // Метод десериал. дисциплин. 
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла. 
            string jsonContent = File.ReadAllText(fullPath); // Считывем весь текст.
            var document = Newtonsoft.Json.Linq.JObject.Parse(jsonContent); // Парсим.
            string typeStr = document.Value<string>("Type") ?? throw new InvalidOperationException("Missing Type"); // Считываем тип.
            Green_4.Discipline discipline = typeStr // Создаём дисциплину.
            switch
            {
                nameof(Green_4.LongJump) => new Green_4.LongJump(),
                nameof(Green_4.HighJump) => new Green_4.HighJump(),
                _ => throw new InvalidOperationException($"Unknown discipline {typeStr}")
            };
            if (document.TryGetValue("Participants", out var partsToken) && partsToken is Newtonsoft.Json.Linq.JArray participantsArr) // Считываем всех участниц.
            {
                foreach (var part in participantsArr.Children<Newtonsoft.Json.Linq.JObject>())
                {
                    string n = part.Value<string>("Name") ?? string.Empty; // Считываем имя.
                    string s = part.Value<string>("Surname") ?? string.Empty; // Считываем фамилию.
                    var p = new Green_4.Participant(n, s); // Создаём участницу.
                    foreach (var jump in part["Jumps"]!.Values<double>()) p.Jump(jump); // Считываем прыжки и записываем их.
                    discipline.Add(p); // Добавляем в дисциплину участницу.
                }
            }
            return discipline; // Возвращаем дисциплину.
        }
        public override void SerializeGreen5Group<T>(T group, string fileIdentifier) // Метод сериализации групп.
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла.
            var typedGroup = group; // Группа.
            var Document = new Newtonsoft.Json.Linq.JObject // Создаём новый json.
            {
                ["Type"] = typedGroup.GetType().Name, // Записываем тип.
                ["Name"] = typedGroup.Name, // Записываем имя.
                ["Students"] = new Newtonsoft.Json.Linq.JArray( // Записываем студентов.
                    typedGroup.Students.Select(s => new Newtonsoft.Json.Linq.JObject
                    {
                        ["Name"] = s.Name, // Их имя.
                        ["Surname"] = s.Surname, // Их фамилии.
                        ["Marks"] = new Newtonsoft.Json.Linq.JArray(s.Marks) // Их оценки.
                    })
                )
            };
            try
            {
                File.WriteAllText(fullPath, Document.ToString(Newtonsoft.Json.Formatting.Indented)); // Записываем json с нужным оформлением.
            }
            catch (Exception ex) // Если что-то не получилось, то
            {
                Console.WriteLine($"Ошибка при записи файла: {ex.Message}"); // пишем об этом.
            }
        }
        public override T DeserializeGreen5Group<T>(string fileIdentifier) // Метод десериал. групп. 
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла. 
            string jsonContent = File.ReadAllText(fullPath); // Считывем весь текст.
            var document = Newtonsoft.Json.Linq.JObject.Parse(jsonContent); // Парсим.
            string typeStr = document.Value<string>("Type") ?? throw new InvalidOperationException("Missing Type"); // Считываем тип.
            string name = document.Value<string>("Name") ?? throw new InvalidOperationException("Missing Name"); // Считываем имя.
            Green_5.Group typedGroup = typeStr switch
            {
                nameof(Green_5.EliteGroup) => new Green_5.EliteGroup(name),
                nameof(Green_5.SpecialGroup) => new Green_5.SpecialGroup(name),
                _ => new Green_5.Group(name)
            };
            if (document.TryGetValue("Students", out var studsToken) && studsToken is Newtonsoft.Json.Linq.JArray studentsArr) // Считываем студентов.
            {
                foreach (var stud in studentsArr.Children<Newtonsoft.Json.Linq.JObject>())
                {
                    string nm = stud.Value<string>("Name") ?? string.Empty; // Их имена.
                    string sr = stud.Value<string>("Surname") ?? string.Empty; // Их фамилии.
                    var sObj = new Green_5.Student(nm, sr); // Создаём студента.
                    foreach (var m in stud["Marks"]!.Values<int>()) sObj.Exam(m); // Заполняем оценки.
                    typedGroup.Add(sObj); // Добавляем его в группу.
                }
            }
            return (T)typedGroup; // Возвращаем тип группы.
        }
    }
}