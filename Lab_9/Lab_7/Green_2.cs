using System;
using System.Linq;


namespace Lab_7
{
    public class Green_2
    {
        public class Human 
        {
            private string _name;
            private string _surname;

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

            public Human(string name, string surname)
            {
                _name = name;
                _surname = surname;
            }

            public void Print()
            {
                Console.WriteLine($"{Name} {Surname}");
            }
        }
        public class Student : Human
        {
            private static int _countExelent;
            private readonly int[] _marks;
            private int _examsTakenCount;
            private bool _isCountedAsExelent;

            public Student(string name, string surname) : base (name, surname)
            {
                _marks = new int[4]; // инициализаця нулями
                _examsTakenCount = 0;
                _isCountedAsExelent = false;
            }

            public static int ExcellentAmount => _countExelent;
            
            public int[] Marks => (int[])_marks.Clone();

            public double AvgMark
            {
                get
                {
                    if (_examsTakenCount < 4) return 0;

                    double sum = 0;
                    for (int i = 0; i < _examsTakenCount; i++)
                    {
                        sum += _marks[i];
                    }
                    return sum / (double)_examsTakenCount;
                }
            }

            public bool IsExcellent
            {
                get
                {
                    if (_examsTakenCount == 0) return false;
                    for (int i = 0; i < _examsTakenCount; i++)
                    {
                        if (_marks[i] < 4)
                        {
                            return false;
                        }
                    }
                    if (!_isCountedAsExelent)
                    {
                        _countExelent++;
                        _isCountedAsExelent = true;
                    }
                    return true;
                }
            }

            public void Exam(int mark)
            {
                if (mark < 2 || mark > 5) return;

                if (_examsTakenCount < _marks.Length)
                {
                    _marks[_examsTakenCount] = mark;
                    _examsTakenCount++;
                }
            }

            public static void SortByAvgMark(Student[] array)
            {
                if (array == null || array.Length == 0) return;
                for (int i = 0; i < array.Length - 1; i++)
                {
                    for (int j = 0; j < array.Length - 1 - i; j++) 
                    {
                        if (array[j].AvgMark < array[j + 1].AvgMark)
                        {
                            Student temp = array[j];
                            array[j] = array[j + 1];
                            array[j + 1] = temp;
                        }
                    }
                }
            }
            public void Print()
            {
                Console.WriteLine($"Имя: {Name}, Фамилия: {Surname}, Средний балл: {AvgMark} , Отличник: {IsExcellent}");
            }
        }

    }
}