using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class Purple_1
    {
        public class Participant
        {
            private string _name;
            private string _surname;
            private double[] _coefs;
            private int[,] _marks;
            private int _jumps;

            public string Name => _name;
            public string Surname => _surname;
            public double[] Coefs
            {
                get
                {
                    if (_coefs == null) return default(double[]);
                    var newArray = new double[_coefs.Length];
                    Array.Copy(_coefs, newArray, _coefs.Length);
                    return newArray;
                }
            }
            public int[,] Marks
            {
                get
                {
                    if (_marks == null) return default(int[,]);

                    var newMatrix = new int[_marks.GetLength(0), _marks.GetLength(1)];
                    Array.Copy(_marks, newMatrix, _marks.Length);
                    return newMatrix;
                }
            }

            public double TotalScore
            {
                get
                {
                    if (_marks == null || _coefs == null) return 0;
                    double totalScore = 0;
                    for (int i = 0; i < _marks.GetLength(0); i++)
                    {
                        int[] newArr = new int[7];
                        double sum = 0;
                        for (int j = 0; j < _marks.GetLength(1); j++) newArr[j] = _marks[i, j];
                        for (int j = 0; j < newArr.Length; j++) sum += newArr[j];
                        sum -= (newArr.Min() + newArr.Max());
                        sum *= _coefs[i];
                        totalScore += sum;
                    }
                    return totalScore;
                }
            }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _coefs = new double[] { 2.5, 2.5, 2.5, 2.5 };
                _marks = new int[4, 7];
                _jumps = 0;
            }

            public void SetCriterias(double[] coefs)
            {
                if (coefs == null || _coefs == null) return;
                Array.Copy(coefs, _coefs, coefs.Length);
            }

            public void Jump(int[] marks)
            {
                if (marks == null || _marks == null || _jumps >= _marks.GetLength(0) || marks.Length != _marks.GetLength(1)) return;
                for (int i = 0; i < _marks.GetLength(1); i++)
                {
                    _marks[_jumps, i] = marks[i];
                }
                _jumps++;
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;

                for (int i = 0; i < array.Length; i++)
                {
                    for (int j = 0; j < array.Length - i - 1; j++)
                    {
                        if (array[j].TotalScore < array[j + 1].TotalScore)
                        {
                            var t = array[j];
                            array[j] = array[j + 1];
                            array[j + 1] = t;
                        }

                    }
                }
            }

            public void Print()
            {
                Console.WriteLine(_name + " " + _surname);
                Console.Write("Coefs: ");
                foreach (double var in _coefs)
                {
                    Console.Write(var + "  ");
                }
                Console.WriteLine();
                Console.WriteLine("Marks:");
                for (int i = 0; i < _marks.GetLength(0); i++)
                {
                    for (int j = 0; j < _marks.GetLength(1); j++)
                    {
                        Console.Write(_marks[i, j] + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        public class Judge
        {
            private string _name;
            private int[] _marks;
            private int _indexMark;

            public string Name => _name;
            public int[] Marks => (int[])_marks?.Clone();
            public Judge(string name, int[] marks)
            {
                _name = name;
                if (marks != null)
                {
                    _marks = new int[marks.Length];
                    Array.Copy(marks, _marks, marks.Length);
                }
                _indexMark = 0;
            }

            public int CreateMark()
            {
                if (_marks == null || _marks.Length == 0) return 0;
                _indexMark %= _marks.Length;
                return _marks[_indexMark++];
            }
            public void Print()
            {
                Console.WriteLine(_name);
                Console.WriteLine("Marks:");
                for (int i = 0; i < _marks.Length; i++)
                {
                    Console.Write(_marks[i] + " ");
                }
            }
        }

        public class Competition
        {
            private Judge[] _judges;
            private Participant[] _participants;

            public Judge[] Judges => _judges;
            public Participant[] Participants => _participants;

            public Competition(Judge[] judges)
            {
                if (judges != null)
                {
                    _judges = new Judge[judges.Length];
                    Array.Copy(judges, _judges, judges.Length);
                }
                _participants = new Participant[0];
            }

            public void Evaluate(Participant jumper)
            {
                if (_judges == null) return;

                int[] marks = new int[7];
                int cnt = 0;

                foreach (var judge in _judges)
                {
                    if (cnt >= 7) break;
                    if (judge != null)
                        marks[cnt++] = judge.CreateMark();
                }

                jumper.Jump(marks);
            }

            public void Add(Participant participant)
            {
                if (participant == null) return;

                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[_participants.Length - 1] = participant;

                Evaluate(_participants[_participants.Length - 1]);
            }

            public void Add(Participant[] participants)
            {
                if (participants == null) return;

                int n = _participants.Length;

                Array.Resize(ref _participants, participants.Length + n);
                Array.Copy(participants, 0, _participants, n, participants.Length);

                for (int i = n; i < _participants.Length; i++)
                    Evaluate(_participants[i]);
            }

            public void Sort()
            {
                _participants = _participants.OrderByDescending(p => p.TotalScore).ToArray();
            }
        }





    }
}