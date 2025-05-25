using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;

namespace Lab_7
{
    public class Purple_1
    {
        public class Participant
        {
            //поля
            private string _name;
            private string _surname;
            private double[] _coefs;
            private int[,] _marks;
            private int _ind;
            //свойства
            public string Name => _name;
            public string Surname => _surname;
            public double[] Coefs
            {
                get
                {
                    if (_coefs == null) return null;
                    double[] copy = new double[_coefs.Length];
                    Array.Copy(_coefs, copy, _coefs.Length);
                    return copy;
                }
            }
            public int[,] Marks
            {
                get
                {
                    if (_marks == null) return null;
                    int[,] copy_m = new int[_marks.GetLength(0), _marks.GetLength(1)];
                    Array.Copy(_marks, copy_m, _marks.Length);
                    return copy_m;
                }
            }
            public double TotalScore
            {
                get
                {
                    if (_marks == null || _coefs == null) return 0;
                    double score = 0.0;
                    for (int i = 0; i < 4; i++)
                    {
                        int worst = int.MaxValue;
                        int best = int.MinValue;
                        int ans = 0;
                        for (int j = 0; j < 7; j++)
                        {
                            if (_marks[i, j] > best)
                                best = _marks[i, j];
                            if (_marks[i, j] < worst)
                                worst = _marks[i, j];
                            ans += _marks[i, j];
                        }
                        ans -= worst;
                        ans -= best;
                        score += ans * _coefs[i];
                    }
                    return score;
                }
            }

            //конструктор
            [JsonConstructor]
            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _coefs = new double[4] { 2.5, 2.5, 2.5, 2.5 };
                _marks = new int[4, 7];
                _ind = 0;
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        _marks[i, j] = 0;
                    }
                }
            }
            //main методы
            public void SetCriterias(double[] coefs)
            {
                if (coefs == null || _coefs == null || coefs.Length != _coefs.Length) return;

                {
                    Array.Copy(coefs, _coefs, 4);
                }
            }
            public void Jump(int[] marks)
            {
                if (marks == null || _marks == null || _ind >= _marks.GetLength(0) || marks.Length != _marks.GetLength(1)) return;
                for (int i = 0; i < marks.Length; i++)
                {
                    _marks[_ind, i] = marks[i];

                }
                _ind++;
            }
            public static void Sort(Participant[] array)
            {
                if (array == null) return;

                var a = array.OrderByDescending((x) => x.TotalScore).ToArray();
                Array.Copy(a, array, array.Length);
            }

            public void Print()
            {
                System.Console.WriteLine($"{_name} {_surname} {TotalScore}");
            }
        }
        public class Judge
        {
            private string _name;
            private int[] _marks;
            private int _ind;
            public string Name => _name;
            public int[] Marks => _marks;
            [JsonConstructor]
            public Judge(string name, int[] marks)
            {
                _name = name;
                _marks =marks.ToArray();
            }
            public int CreateMark()
            {
                if (_marks == null) return 0;
                int ans = _marks[_ind];
                _ind++;
                _ind %= _marks.Length;
                return ans;
            }
            public void Print()
            {
                Console.WriteLine(_name);
                for (int i = 0; i < _marks.Length; i++) Console.Write($"{_marks[i]} ");
            }
        }
        public class Competition
        {
            private Judge[] _judges;
            private Participant[] _participants;

            public Judge[] Judges => _judges;

            public Participant[] Participants => _participants;
            [JsonConstructor]
            public Competition(Judge[] judges)
            {
                _participants = new Participant[0];
                _judges = judges;
            }
            public void Evaluate(Participant jumper)
            {
                if (_judges == null || _participants == null) return;
                int[] ans = new int[7];
                for (int i = 0; i < 7; i++)
                {
                    if (_judges[i] == null) return;
                    ans[i] = _judges[i].CreateMark();
                }
                jumper.Jump(ans);
            }
            public void Add(Participant participant)
            {
                if (_participants == null || _judges==null) return;
                Evaluate(participant);
                var part1 = new Participant[_participants.Length + 1];
                Array.Copy(_participants, part1, _participants.Length);
                part1[part1.Length - 1] = participant;
                _participants = part1;
            }
            public void Add(Participant[] participants)
            {
                foreach (var participant in participants)
                {
                    Add(participant);
                }
            }
            public void Sort() { Participant.Sort(_participants); }
        }
    }
}
