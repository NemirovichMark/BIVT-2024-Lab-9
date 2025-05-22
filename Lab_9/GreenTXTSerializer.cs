using Lab_7;

namespace Lab_9
{
    public class GreenTXTSerializer : GreenSerializer // Публичный класс для сериализации и десирал. txt.
    {
        public override string Extension => ".txt"; // Переопределяем расширение на .txt.
        public override void SerializeGreen1Participant(Green_1.Participant participant, string fileIdentifier)  // Метод сериализации участниц.
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла.
            var Vocabulary = new List<string> // Создаём лист строк. 
            {
                $"Surname:    {participant.Surname}",  // Записываем фамилию.
                $"Group:      {participant.Group}",    // Записываем группу.
                $"Trainer:    {participant.Trainer}",  // Записываем тренера.
                $"Result:     {participant.Result}",   // Записываем результат.
                $"Discipline: {(participant is Green_1.Participant100M ? "100M" : "500M")}" // Заполняем дисциплину в зависимости от её типа.
            };
            try
            {
                File.WriteAllLines(fullPath, Vocabulary); // Записываем всё в файл.
            }
            catch (Exception ex) // Если что-то не получилось, то
            {
                Console.WriteLine($"Ошибка при записи файла: {ex.Message}"); // пишем об этом.
            }
        }
        public override Green_1.Participant DeserializeGreen1Participant(string fileIdentifier)
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла.
            var Text = File.ReadAllLines(fullPath); // Считывем весь текст.
            var Vocabulary = Text // Записываем всё в словарь.
                .Select(line => line.Split(new[] { ':' }, 2)) // Выбираем разбиением все строки.
                .ToDictionary( // Преобразовываем в настоящий словарь.
                    parts => parts[0].Trim(), // Первый элемент с обрезанием пробелов.
                    parts => parts.Length > 1 ? parts[1].Trim() : string.Empty); // Если есть часть после ":", то она обрезается от пробелов, иначе пустая строка.
            string surname = Vocabulary.GetValueOrDefault("Surname") ?? throw new InvalidOperationException("Missing Surname"); // Считываем фамилию.
            string group = Vocabulary.GetValueOrDefault("Group") ?? throw new InvalidOperationException("Missing Group"); // Считываем группу.
            string trainer = Vocabulary.GetValueOrDefault("Trainer") ?? throw new InvalidOperationException("Missing Trainer"); // Считываем тренера.
            string Result = Vocabulary.GetValueOrDefault("Result") ?? throw new InvalidOperationException("Missing Result"); // Считываем Результат.
            string discipline = Vocabulary.GetValueOrDefault("Discipline") ?? throw new InvalidOperationException("Missing Discipline"); // Считывем дисциплину.
            if (!double.TryParse(Result, out double result)) // Если Результат не преобразовывается в действительное число, то
            {
                throw new FormatException($"Cannot parse Result '{result}' as double."); // исключаем это.
            }
            Green_1.Participant participant = discipline switch // Рассматриваем дисциплину.
            {
                "100M" => new Green_1.Participant100M(surname, group, trainer), // Если забег на 100 метров, то соот. тип.
                "500M" => new Green_1.Participant500M(surname, group, trainer), // Если забег на 500 метров, то соот. тип.
                _ => throw new InvalidOperationException($"Unknown discipline '{discipline}'") // Другие исключаем.
            };
            participant.Run(result); // Записываем результат (пробегаем).
            return participant; // Возвращаем участницу.
        }
        public override void SerializeGreen2Human(Green_2.Human human, string fileIdentifier) // Метод сериализации людей. 
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла.
            var Vocabulary = new List<string> // Создаём лист строк. 
            {
                $"Name:    {human.Name}", // Записываем имя.
                $"Surname: {human.Surname}", // Записываем фамилию.
                $"Type:    {human.GetType().Name}" // Записываем тип.
            };
            if (human is Green_2.Student student) // Если человек является студентом, то
            {
                Vocabulary.Add($"Marks: {string.Join(",", student.Marks)}"); // Записываем и его оценки.
            }
            try
            {
                File.WriteAllLines(fullPath, Vocabulary); // Записываем всё в файл.
            }
            catch (Exception ex) // Если что-то не получилось, то
            {
                Console.WriteLine($"Ошибка при записи файла: {ex.Message}"); // пишем об этом.
            }
        }
        public override Green_2.Human DeserializeGreen2Human(string fileIdentifier)  // Метод десериал. людей. 
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла.
            var Text = File.ReadAllLines(fullPath); // Считывем весь текст.
            var Vocabulary = Text // Записываем всё в словарь.
                .Select(line => line.Split(new[] { ':' }, 2)) // Выбираем разбиением все строки.
                .ToDictionary( // Преобразовываем в настоящий словарь.
                    parts => parts[0].Trim(), // Первый элемент с обрезанием пробелов.
                    parts => parts.Length > 1 ? parts[1].Trim() : string.Empty); // Если есть часть после ":", то она обрезается от пробелов, иначе пустая строка.
            string type = Vocabulary.GetValueOrDefault("Type") ?? throw new InvalidOperationException("Missing Type"); // Считываем тип.
            string name = Vocabulary.GetValueOrDefault("Name") ?? throw new InvalidOperationException("Missing Name"); // Считываем имя.
            string surname = Vocabulary.GetValueOrDefault("Surname") ?? throw new InvalidOperationException("Missing Surname"); // Считываем фамилию.
            Green_2.Human human; // Создаём человека.
            if (type == nameof(Green_2.Student) && Vocabulary.TryGetValue("Marks", out var marksTXT) && marksTXT.Length > 0) // Если студенческий тип и считываются его оценки, то 
            {
                var student = new Green_2.Student(name, surname); // Создаём студента с такими ФИ.
                var marks = marksTXT // Создаём его оценки.
                    .Split(',', StringSplitOptions.RemoveEmptyEntries) // Объединяя по запятым, убирая пустые символы.
                    .Select(int.Parse); // И преобразовываем в целый тип.
                foreach (var mark in marks)
                {
                    student.Exam(mark); // Заставляем проходить экзамен.
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
            var Vocabulary = new List<string> // Создаём лист строк. 
            {
                $"Name:    {student.Name}", // Записываем имя.
                $"Surname: {student.Surname}", // Записываем фамилию.
                $"ID:      {student.ID}", // Записываем айдишник.
                $"Marks:   {string.Join(",", student.Marks)}" // Записываем оценки.
            };
            try
            {
                File.WriteAllLines(fullPath, Vocabulary); // Записываем всё в файл.
            }
            catch (Exception ex) // Если что-то не получилось, то
            {
                Console.WriteLine($"Ошибка при записи файла: {ex.Message}"); // пишем об этом.
            }
        }
        public override Green_3.Student DeserializeGreen3Student(string fileIdentifier) // Метод десериал. студентов. 
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла.
            var Text = File.ReadAllLines(fullPath); // Считывем весь текст.
            var Vocabulary = Text // Записываем всё в словарь.
                .Select(line => line.Split(new[] { ':' }, 2)) // Выбираем разбиением все строки.
                .ToDictionary( // Преобразовываем в настоящий словарь.
                    parts => parts[0].Trim(), // Первый элемент с обрезанием пробелов.
                    parts => parts.Length > 1 ? parts[1].Trim() : string.Empty); // Если есть часть после ":", то она обрезается от пробелов, иначе пустая строка.
            string name = Vocabulary.GetValueOrDefault("Name") ?? throw new InvalidOperationException("Missing Name"); // Считываем имя.
            string surname = Vocabulary.GetValueOrDefault("Surname") ?? throw new InvalidOperationException("Missing Surname"); // Считываем фамилию.
            string idTXT = Vocabulary.GetValueOrDefault("ID") ?? throw new InvalidOperationException("Missing ID"); // Считываем айдишник.
            if (!int.TryParse(idTXT, out int id)) // Если не получается преобразовать в целый вид.
            {
                throw new FormatException($"Cannot parse ID '{idTXT}'"); // Исключаем такой вариант.
            }
            var student = new Green_3.Student(name, surname, id); // Создаём студента.
            if (Vocabulary.TryGetValue("Marks", out var marksTXT) && marksTXT.Length > 0) // Если получается считать оценки, то
            {
                var marks = marksTXT // Создаём его оценки.
                    .Split(',', StringSplitOptions.RemoveEmptyEntries) // Объединяя по запятым, убирая пустые символы.
                    .Select(int.Parse); // И преобразовываем в целый тип.
                foreach (var mark in marks)
                {
                    student.Exam(mark); // Заставляем проходить экзамен.
                }
            }
            return student;
        }
        public override void SerializeGreen4Discipline(Green_4.Discipline discipline, string fileIdentifier) // Метод сериализации дисциплин.
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла.
            var Vocabulary = new List<string> // Создаём лист строк. 
            {
                $"Name:       {discipline.Name}", // Записываем имя.
                $"Discipline: {(discipline is Green_4.LongJump ? "LongJump" : "HighJump")}", // Записываем дисциплину.
                $"Count:      {discipline.Participants.Length}", // Записываем кол-во участниц.
                "Participants:"
            };
            foreach (var participant in discipline.Participants) // Для каждой участницы.
            {
                string jumps = string.
                    Join(",", participant.Jumps.Select(j => j.ToString(System.Globalization.CultureInfo.InvariantCulture))); // Объединям прыжки.
                Vocabulary.Add($"{participant.Name}|{participant.Surname}|{jumps}"); // Добавляем ФИ и прыжки в словарь.
            }
            try
            {
                File.WriteAllLines(fullPath, Vocabulary); // Записываем всё в файл.
            }
            catch (Exception ex) // Если что-то не получилось, то
            {
                Console.WriteLine($"Ошибка при записи файла: {ex.Message}"); // пишем об этом.
            }
        }
        public override Green_4.Discipline DeserializeGreen4Discipline(string fileIdentifier) // Метод десериал. дисциплин.
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла.
            var Text = File.ReadAllLines(fullPath); // Считывем весь текст.
            var headerLines = Text // Собираем весь текст.
                .TakeWhile(line => !line.StartsWith("Participants", StringComparison.OrdinalIgnoreCase)) // Покуда не дошли до участниц,
                .Select(line => line.Split(new[] { ':' }, 2)) // Выбираем разбиением все строки.
                .ToDictionary( // Преобразовываем в настоящий словарь.
                    parts => parts[0].Trim(), // Первый элемент с обрезанием пробелов.
                    parts => parts.Length > 1 ? parts[1].Trim() : string.Empty); // Если есть часть после ":", то она обрезается от пробелов, иначе пустая строка.
            string name = headerLines.GetValueOrDefault("Name") ?? throw new InvalidOperationException("Missing Name"); // Считываем имя.
            string type = headerLines.GetValueOrDefault("Discipline") ?? throw new InvalidOperationException("Missing Discipline"); // Считываем дисциплину.
            string countText = headerLines.GetValueOrDefault("Count") ?? throw new InvalidOperationException("Missing Count"); // Считываем кол-во.
            if (!int.TryParse(countText, out int count)) // Если кол-во нецелое, то
            {
                throw new FormatException($"Cannot parse Count '{countText}' as integer."); // исключаем.
            }
            Green_4.Discipline discipline = type switch // Рассматриваем дисциплину.
            {
                "LongJump" => new Green_4.LongJump(), // Если прыжок в длину, то соот. тип.
                "HighJump" => new Green_4.HighJump(), // Если прыжок в высоту, то соот. тип.
                _ => throw new InvalidOperationException($"Unknown discipline '{type}'") // Другие исключаем.
            };
            int participantsStart = Array.FindIndex(Text, line => line.StartsWith("Participants", StringComparison.OrdinalIgnoreCase)) + 1; // Ищем где начинается список участниц.
            for (int i = 0; i < count; i++)
            {
                var parts = Text[participantsStart + i].Split('|'); // Разделяем по |.
                var p = new Green_4.Participant(parts[0].Trim(), parts[1].Trim()); // Выделяем участницу.
                var jumps = parts[2] // Записываем прыжки.
                    .Split(',', StringSplitOptions.RemoveEmptyEntries) // Разделяем по запятым.
                    .Select(x => double.Parse(x, System.Globalization.CultureInfo.InvariantCulture)); // Переводим в действительные числа.
                foreach (var j in jumps)
                {
                    p.Jump(j); // Пропрыгиваем каждый прыжок.
                }
                discipline.Add(p); // Добавляем участницу.
            }
            return discipline; // Возвращаем дисциплину.
        }
        public override void SerializeGreen5Group<T>(T group, string fileIdentifier)
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла.
            var students = group.Students.Where(s => s.Marks?.Any() == true).ToArray(); // Выделяем студентов.
            double avg = students.Length > 0 ? students.Average(s => s.AvgMark) : 0; // Рассчитываем среднее.
            var Vocabulary = new List<string> // Создаём лист строк. 
            {
                $"Type:        {group.GetType().Name}", // Записываем тип.
                $"Name:        {group.Name}", // Записываем имя.
                $"AverageMark: {avg}", // Записываем среднее.
                $"Count:       {students.Length}", // Записываем кол-во.
                "Students:"
            };
            foreach (var student in students)
            {
                Vocabulary.Add($"{student.Name}|{student.Surname}|{string.Join(",", student.Marks)}"); // Добавляем студента.
            }
            try
            {
                File.WriteAllLines(fullPath, Vocabulary); // Записываем всё в файл.
            }
            catch (Exception ex) // Если что-то не получилось, то
            {
                Console.WriteLine($"Ошибка при записи файла: {ex.Message}"); // пишем об этом.
            }
        }
        public override T DeserializeGreen5Group<T>(string fileIdentifier)
        {
            string fullPath = Path.Combine(FolderPath, $"{fileIdentifier}{Extension}"); // Совмещаем путь с именем файла.
            var Text = File.ReadAllLines(fullPath); // Считывем весь текст.
            var headerLines = Text // Собираем весь текст.
               .TakeWhile(line => !line.StartsWith("Participants", StringComparison.OrdinalIgnoreCase)) // Покуда не дошли до участниц,
               .Select(line => line.Split(new[] { ':' }, 2)) // Выбираем разбиением все строки.
               .ToDictionary( // Преобразовываем в настоящий словарь.
                   parts => parts[0].Trim(), // Первый элемент с обрезанием пробелов.
                   parts => parts.Length > 1 ? parts[1].Trim() : string.Empty); // Если есть часть после ":", то она обрезается от пробелов, иначе пустая строка.
            string typeName = headerLines.GetValueOrDefault("Type") ?? throw new InvalidOperationException("Missing Type"); // Считываем тип.
            string groupName = headerLines.GetValueOrDefault("Name") ?? throw new InvalidOperationException("Missing Name"); // Считываем название.
            string averageTXT = headerLines.GetValueOrDefault("AverageMark") ?? throw new InvalidOperationException("Missing AverageMark"); // Считываем среднее.
            string countText = headerLines.GetValueOrDefault("Count") ?? throw new InvalidOperationException("Missing Count"); // Считываем кол-во.
            if (!double.TryParse(averageTXT, out double averageMark)) // Если среднее недействительное, то
            {
                throw new FormatException($"Cannot parse AverageMark '{averageTXT}' as double."); // исключаем.
            }
            if (!int.TryParse(countText, out int count)) // Если кол-во нецелое, то
            {
                throw new FormatException($"Cannot parse Count '{countText}' as integer."); // исключаем.
            }
            Green_5.Group baseGroup = typeName switch // Рассматриваем типы.
            {
                nameof(Green_5.EliteGroup) => new Green_5.EliteGroup(groupName), // Если элита, то соот. тип.
                nameof(Green_5.SpecialGroup) => new Green_5.SpecialGroup(groupName), // Если спецгруппа, то соот. тип.
                _ => new Green_5.Group(groupName) // Другие исключаем.
            };
            int studentsStart = Array.FindIndex(Text, line => line.StartsWith("Students")) + 1; // Ищем где начинается список студентов.
            for (int i = 0; i < count; i++)
            {
                var parts = Text[studentsStart + i].Split('|'); // Разделяем по |.
                var student = new Green_5.Student(parts[0].Trim(), parts[1].Trim()); // Создаём новго студента.
                foreach (var markTXT in parts[2].Split(',', StringSplitOptions.RemoveEmptyEntries)) // Пробегаем по каждой оценке.
                {
                    if (int.TryParse(markTXT.Trim(), out int mark)) // Если оценка целая, то
                    {
                        student.Exam(mark); // типо прошёл экзамен.
                    }
                    else
                    {
                        throw new FormatException($"Cannot parse exam mark '{markTXT}' as integer."); // исключаем.
                    }
                }
                baseGroup.Add(student); // Добавляем студента в группу.
            }
            return (T)baseGroup; // Возвращаем группу.
        }
    }
}