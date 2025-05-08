using System;
using System.Linq;


namespace Lab_7
{
    public class Green_5
    {
        public struct Student
        {
            private string _name;
            private string _surname;
            private int[] _marks;


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

            public int[] Marks
            {
                get
                {
                    return _marks;
                }
            }

            public double AvgMark
            {
                get
                {
                    if (_marks == null || _marks.Length == 0) return 0;

                    double sum = 0;
                    for (int i = 0; i < _marks.Length; i++)
                    {
                        sum += _marks[i];
                    }
                    return sum / (double)_marks.Length;
                }
            }

            public Student(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _marks = new int[5];

            }

            public void Exam(int mark)
            {
                if (_marks == null) return;
                if (mark < 2 || mark > 5) return;
                for (int i = 0; i < _marks.Length; i++)
                {
                    if (_marks[i] == 0)
                    {
                        _marks[i] = mark;
                        break;
                    }
                }
            }

            public void Print()
            {
                Console.WriteLine($"{Name} {Surname} {AvgMark}");
            }
        }

        public class Group
        {
            private string _name;
            private Student[] _students;
            private int _studentCount;
            private int _capacity;

            public string Name
            {
                get
                {
                    return _name;
                }
            }

            public Student[] Students
            {
                get
                {
                    // если нет ни одного добавленного студента — возвращаем просто пустой массив
                    if (_students == null || _studentCount == 0)
                        return Array.Empty<Student>();

                    // иначе — возвращаем только ту часть _students, в которой реально лежат добавленные студенты
                    return _students.Take(_studentCount).ToArray();
                }
            }

            public virtual double AvgMark
            {
                get
                {
                    if (_students == null || _studentCount == 0)
                    {
                        return 0;
                    }
                    double totalMark = 0;

                    for (int i = 0; i < _studentCount; i++)
                    {
                        totalMark += _students[i].AvgMark;
                    }
                    double averageMark = totalMark / (double)_studentCount;

                    return averageMark;
                }
            }

            public Group(string name)
            {
                _name = name;
                _capacity = 100;
                _students = new Student[_capacity];
                _studentCount = 0;
            }

            private void ResizeArray()
            {
                _capacity *= 2;
                if (_capacity <= 0) _capacity = 30;

                Student[] newStudents = new Student[_capacity];

                Array.Copy(_students, newStudents, _studentCount);

                _students = newStudents;
            }

            public void Add(Student student)
            {
                if (_studentCount == _capacity)
                {
                    ResizeArray();
                }

                _students[_studentCount] = student;
                _studentCount++;
            }

            public void Add(params Student[] students)
            {
                foreach (Student student in students)
                {
                    Add(student);
                }
            }

            public static void SortByAvgMark(Group[] array)
            {
                for (int i = 0; i < array.Length - 1; i++)
                {
                    for (int j = 0; j < array.Length - 1 - i; j++)
                    {
                        if (array[j].AvgMark < array[j + 1].AvgMark)
                        {
                            Group temp = array[j];
                            array[j] = array[j + 1];
                            array[j + 1] = temp;
                        }
                    }
                }
            }

            public void Print()
            {
                Console.WriteLine($"{_name} {AvgMark}");
                foreach (var student in _students)
                {
                    student.Print();
                }
            }
        }

        public class EliteGroup : Group
        {
            public EliteGroup(string name) : base(name)
            {
            }

            public override double AvgMark
            {
                get 
                {
                    double total = 0;
                    int count = 0;

                    foreach (var student in Students)
                    {
                        int[] marks = student.Marks;
                        bool hasMarks = false;

                        foreach (var mark in marks)
                        {
                            if (mark != 0)
                            {
                                hasMarks = true;
                                break;
                            }
                        }
                        if (!hasMarks) continue;

                        double sum = 0;
                        int markCount = 0;

                        foreach (var mark in marks)
                        {
                            if (mark == 0) continue;
                            switch (mark)
                            {
                                case 5:
                                    sum += mark * 1.0;
                                    break;
                                case 4:
                                    sum += mark * 1.5;
                                    break;
                                case 3:
                                    sum += mark * 2;
                                    break;
                                case 2:
                                    sum += mark * 2.5;
                                    break;
                            }
                            markCount++;
                        }

                        if (markCount > 0)
                        {
                            double weightedAvg = sum / markCount;
                            total += weightedAvg;
                            count++;
                        }
                    }

                    return count > 0 ? total / count : 0;    
                } 
            }
        }

        public class SpecialGroup : Group
        {
            public SpecialGroup(string name) : base(name)
            {
            }

            public override double AvgMark
            {
                get
                {
                    double total = 0;
                    int count = 0;

                    foreach (var student in Students)
                    {
                        int[] marks = student.Marks;
                        bool hasMarks = false;

                        foreach (var mark in marks)
                        {
                            if (mark != 0)
                            {
                                hasMarks = true;
                                break;
                            }
                        }

                        if (!hasMarks) continue;

                        double sum = 0;
                        int markCount = 0;

                        foreach (var mark in marks)
                        {
                            if (mark == 0) continue;
                            switch (mark)
                            {
                                case 5:
                                    sum += mark * 1.0;
                                    break;
                                case 4:
                                    sum += mark * 0.75;
                                    break;
                                case 3:
                                    sum += mark * 0.5;
                                    break;
                                case 2:
                                    sum += mark * 0.25;
                                    break;
                            }
                            markCount++;
                        }

                        if (markCount > 0)
                        {
                            double weightedAvg = sum / markCount;
                            total += weightedAvg;
                            count++;
                        }
                    }

                    return count > 0 ? total / count : 0;
                }
            }
        } 
    }
}