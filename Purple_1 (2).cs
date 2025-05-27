using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            private int _k;
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
                    double rez = 0.0;
                    for (int i = 0; i < 4; i++)
                    {
                        int mi = int.MaxValue;
                        int ma = int.MinValue;
                        int sum = 0;
                        for (int j = 0; j < 7; j++)
                        {
                            int A = Marks[i, j];
                            if (A > ma) ma = A;
                            if (A < mi) mi = A;
                            sum += A;
                        }
                        sum -= mi;
                        sum -= ma;
                        rez += sum * _coefs[i];
                    }
                    return rez;
                }
            }

            //конструктор
            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _coefs = new double[] { 2.5, 2.5, 2.5, 2.5 };
                _marks = new int[,] { { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 } };
                _k = 0;
            }
            public void SetCriterias(double[] coefs)
            {
                if (coefs == null || coefs.Length != 4) return;
                foreach (double x in coefs)
                {
                    if (x < 2.5 || x > 3.5) return;
                }
                Array.Copy(coefs, _coefs, 4);
            }
            public void Jump(int[] marks)
            {
                if (marks == null || marks.Length != 7 || _marks == null || _k >= _marks.GetLength(0)) return;
                for (int i = 0; i < marks.Length; i++)
                {
                    _marks[_k, i] = marks[i];
                }
                _k++;
            }
            public void Print()
            {

                Console.WriteLine($"Имя: {Name}. Фамилия: {Surname}. Результат: {TotalScore}");

            }
            public static void Sort(Participant[] array)
            {
                if (array == null) return;
                var array1 = array.OrderByDescending(x => x.TotalScore).ToArray();
                Array.Copy(array1, array, array.Length);
            }
        }
        public class Judge
        {
            private string _name;
            private int[] _marks;
            private int _k;
            public int[] Marks
            {
                get
                {
                    return _marks;
                }
            }
            public string Name
            {
                get
                {
                    return _name;
                }
            }

            public Judge(string name, int[] marks)
            {
                _name = name;
                _marks = marks.ToArray();
                _k = 0;
            }
            public int CreateMark()
            {
                if (_marks == null || _marks.Length == 0) return 0;
                if (_k == _marks.Length) _k = 0;
                int a = _marks[_k];
                _k++;
                return a;
            }
            public void Print()
            {
                Console.WriteLine($"Имя : {_name}, Оценки : {_marks}");
            }
        }
        public class Competition
        {
            private Judge[] _judges;
            private Participant[] _participants;

            public Judge[] Judges
            {
                get
                {
                    if (_judges == null) return null;

                    return _judges;
                }
            }

            public Participant[] Participants
            {
                get
                {
                    if (_participants == null) return null;
                    return _participants;
                }
            }
            public Competition(Judge[] judges)
            {
                _participants = new Participant[0];
                _judges = judges;
            }
            public void Evaluate(Participant jumper)
            {
                if (_judges == null || _participants == null) return;
                int[] a = new int[7];
                for (int i = 0; i < 7; i++)
                {
                    if (_judges[i] == null) return;
                    a[i] = _judges[i].CreateMark();
                }
                jumper.Jump(a);
            }
            public void Add(Participant jumper)
            {
                if (_participants == null) return;
                Evaluate(jumper);
                var a = new Participant[_participants.Length];
                Array.Copy(_participants, a, _participants.Length);
                a = a.Append(jumper).ToArray();
                _participants = a;
            }
            public void Add(Participant[] jumpers)
            {
                if (_participants == null || jumpers == null) return;
                for (int i = 0; i < jumpers.Length; i++)
                {
                    if (jumpers[i] == null) return;
                    Evaluate(jumpers[i]);
                }
                var a = new Participant[_participants.Length];
                Array.Copy(_participants, a, _participants.Length);
                a = a.Concat(jumpers).ToArray();
                _participants = a;
            }
            public void Sort() 
            { 
                Participant.Sort(_participants);
            }
        }
    }
}
