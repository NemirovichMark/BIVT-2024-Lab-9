using System;
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

            private int _curJump;

            public string Name => _name;
            public string Surname => _surname;

            public double[] Coefs => (double[])_coefs?.Clone();
            public int[,] Marks => (int[,])_marks?.Clone();

            public double TotalScore { get; private set; }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _coefs = new double[] { 2.5, 2.5, 2.5, 2.5 };
                _marks = new int[4, 7];
                TotalScore = 0;

                _curJump = 0;
            }

            public void SetCriterias(double[] coefs)
            {
                if (coefs == null || coefs.Length != 4 || _coefs == null) return;

                _coefs = (double[])coefs.Clone();
            }

            public void Jump(int[] marks)
            {
                if (_curJump >= 4 || marks == null || marks.Length != 7 || _marks == null || _coefs == null) return;

                for (int j = 0; j < marks.Length; j++)
                {
                    _marks[_curJump, j] = marks[j];
                }

                TotalScore += (marks.Sum() - marks.Max() - marks.Min()) * _coefs[_curJump];
                _curJump++;
            }

            public void Print()
            {
                Console.WriteLine($"Имя: {_name ?? "не задано"}");
                Console.WriteLine($"Фамилия: {_surname ?? "не задана"}");

                Console.Write("Коэффициенты:\t");
                if (_coefs == null) Console.WriteLine("не заданы");
                else
                {
                    foreach (var coef in _coefs)
                    {
                        Console.Write($"{coef}\t");
                    }

                    Console.WriteLine();
                }

                Console.Write("Оценки:");
                if (_marks == null) Console.WriteLine("не заданы");
                else
                {
                    for (int i = 0; i < _marks.GetLength(0); i++)
                    {
                        Console.Write("\t");
                        for (int j = 0; j < _marks.GetLength(1); j++)
                        {
                            Console.Write($"{_marks[i, j]}\t");
                        }

                        Console.WriteLine();
                    }
                }

                Console.WriteLine($"Результат: {Math.Round(TotalScore, 2)}");
                Console.WriteLine();
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;

                var sortedArray = array
                    .Where(x => x != null)
                    .OrderByDescending(x => x.TotalScore)
                    .Concat(array.Where(x => x == null))
                    .ToArray();
                Array.Copy(sortedArray, array, sortedArray.Length);
            }
        }

        public class Judge
        {
            private string _name;
            private int[] _marks;
            private int _curMark;

            public string Name => _name;
            public int[] Marks => (int[])_marks?.Clone();

            public Judge(string name, int[] favouriteMarks)
            {
                _name = name;
                _marks = (int[])favouriteMarks?.Clone();

                _curMark = 0;
            }

            public int CreateMark()
            {
                if (_marks == null || _marks.Length == 0) return 0;

                if (_curMark == _marks.Length) _curMark = 0;
                return _marks[_curMark++];
            }

            public void Print()
            {
                Console.WriteLine($"Имя: {_name ?? "не задано"}");

                Console.Write("Любимые оценки:\t");
                if (_marks == null) Console.WriteLine("не заданы");
                else
                {
                    foreach (var mark in _marks)
                    {
                        Console.Write($"{mark}\t");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
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
                _judges = (Judge[])judges?.Clone();
                _participants = new Participant[0];
            }

            public void Evaluate(Participant jumper)
            {
                if (_judges == null || jumper == null) return;

                jumper.Jump(_judges.Select(x => x == null ? 0 : x.CreateMark()).ToArray());
            }

            public void Add(Participant participant)
            {
                if (_participants == null) return;

                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[_participants.Length - 1] = participant;
                Evaluate(_participants[_participants.Length - 1]);
            }

            public void Add(Participant[] participants)
            {
                if (participants == null || _participants == null) return;

                int n = _participants.Length;
                _participants = _participants.Concat(participants).ToArray();

                for (int i = n; i < _participants.Length; i++)
                {
                    Evaluate(_participants[i]);
                }
            }

            public void Sort()
            {
                if (_participants == null) return;

                Participant.Sort(_participants);
            }
        }
    }
}