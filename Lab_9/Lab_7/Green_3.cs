using System;
using System.Linq;


namespace Lab_7
{
    public class Green_3
    {
        public class Student
        {
            private static int _studentID;
            private int _id;
            private string _name;
            private string _surname;
            private int[] _marks;
            private int _count_exams;
            private bool _session_outcome;

            static Student()
            {
                _studentID = 1;
            }

            public Student(string name, string surname)
            {
                _id = _studentID++;
                _name = name;
                _surname = surname;
                _marks = new int[3]; // инициализируем массив нулями
                _session_outcome = false;
                _count_exams = 0;
            }

            public Student(string name, string surname, int id) // добавлен второй констуктор к Lab_9 с параметром id
            {
                _id = id;
                if(id >= _studentID) _studentID = id + 1;
                _name = name;
                _surname = surname;
                _marks = new int[3]; // инициализируем массив нулями
                _session_outcome = false;
                _count_exams = 0;
            }

            public string Name
            {
                get
                {
                    return _name;
                }
            }
            public string Surname
            {
                get
                {
                    return _surname;
                }
            }

            public int ID
            {
                get
                {
                    return _id;
                }
            }

            private static int CurrentID // сделано приватным
            {
                get
                {
                    return _studentID;
                }
            }

            public int[] Marks
            {
                get
                {
                    return (int[])_marks?.Clone() ?? Array.Empty<int>(); // ? предотвращает ошибку, тк вмето вызова Clone возвращается null
                }
            }

            public double AvgMark
            {
                get
                {
                    if (_marks.Length == 0 || _marks == null)
                    {
                        return 0;
                    }

                    double sum = 0;
                    foreach (int mark in _marks)
                    {
                        sum += mark;
                    }

                    return _count_exams != 0 ? sum / (double)_count_exams : 0;
                }
            }

            public bool IsExpelled
            {
                get
                {
                    if (_count_exams == 0)
                    {
                        return false;
                    }
                    for (int i = 0; i < _count_exams; i++)
                    {
                        if (_marks[i] <= 2)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public void Restore()
            {
                // Проверка: студент не должен быть "новым"
                if (_id >= CurrentID)
                {
                    throw new Exception("Студента ранее не было в списке. Восстановление невозможно");
                }

                _session_outcome = false;
            }

            public void Exam(int mark)
            {

                if (_marks.Length == 0 || _marks == null) return;

                if (_count_exams >= 3) return;

                if (!_session_outcome)
                {
                    _marks[_count_exams] = mark;
                    _count_exams++;

                    if (mark <= 2)
                    {
                        _session_outcome = true;
                    }
                }
            }

            // исправление на гномью сортировку
            public static void SortByAvgMark(Student[] array)
            {

                if (array == null) return;
                for (int i = 1, j = 2; i < array.Length;)
                {
                    if (i == 0 || array[i].AvgMark <= array[i - 1].AvgMark)
                    {
                        i = j;
                        j++;
                    }

                    else 
                    {
                        Student temp = array[i];
                        array[i] = array[i-1];
                        array[i-1] = temp;
                        i--;
                    }
                }
            }

            public void Print()
            {
                Console.WriteLine($"{ID} {Name} {Surname} {AvgMark} {IsExpelled}");
            }
        }

        public class Commission
        {
            public static void Sort(Student[] students)
            {
                if (students == null) return;
                for (int i = 0; i < students.Length - 1; i++)
                {
                    for (int j = i + 1; j < students.Length; j++)
                    {
                        if (students[i].ID > students[j].ID) // смена знака
                        {
                            Student temp = students[i];
                            students[i] = students[j];
                            students[j] = temp;
                        }
                    }
                }

            }
            public static Student[] Expel(ref Student[] students)
            {
                var expelled = students.Where(s => s.IsExpelled).ToArray();
                students = students.Where(s => !s.IsExpelled).ToArray();

                return expelled;
            }


            public static void Restore(ref Student[] students, Student restored)
            {

                //if (students.Any(s => s.ID == restored.ID))
                //{
                //    //Console.WriteLine("Студент уже есть в списке. Дублирования не произошло");
                //    //return;
                //    throw new Exception("Студент уже есть в списке. Дублирования не произошло");
                //}
                ////if (restored.IsExpelled == true) restored.Restore();  один из способов //Вызов Restore из Student (с проверкой ID и сбросом статуса)
                //try // второй способ
                //{
                //    if (restored.IsExpelled)
                //    { // можем зачислять только отчисленных
                //        restored.Restore();
                //    }
                //}
                //catch 
                //{
                //    Console.WriteLine("Произошло исключение, студент не зачислен"); // если ошибка, то ничего не делаем и не зачисляем
                //    return;
                //}

                //if (restored.IsExpelled)
                //{
                //    return;
                //}

                if (restored != null && students != null && students.Any(s => s.ID == restored.ID) && restored.IsExpelled == true)
                {
                    // Теперь вставляем восстановленного студента в массив так, чтобы он оказался на правильном месте
                    int n = students.Length;
                    // Определим индекс для вставки: ищем первое место, где restored.ID меньше ID текущего студента.
                    // по умолчанию – добавляем в конец
                    int insertIndex = n;

                    for (int i = 0; i < n; i++)
                    {
                        if (restored.ID < students[i].ID)
                        {
                            insertIndex = i;
                            break;
                        }
                    }

                    // Изменяем размер массива: увеличиваем на 1 (Array.Resize создаёт новый массив внутри - встроенная функция)
                    Array.Resize(ref students, n + 1);

                    // Сдвигаем элементы вправо, начиная с insertIndex
                    for (int i = n; i > insertIndex; i--)
                    {
                        students[i] = students[i - 1];
                    }

                    // Вставляем восстановленного студента в найденную позицию
                    students[insertIndex] = restored;
                }          
            }
        }
    }
}


