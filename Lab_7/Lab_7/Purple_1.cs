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

            private int _jump;

            public string Name => _name;
            public string Surname => _surname;
            public double[] Coefs
            {
                get
                {
                    if (_coefs == null) return null;
                    double[] newarray = new double[_coefs.Length];
                    Array.Copy(_coefs, newarray, _coefs.Length);
                    return newarray;

                }
            }

            public int[,] Marks
            {

                get
                {
                    if (_marks == null) return null;
                    int[,] newmatrix = new int[_marks.GetLength(0), _marks.GetLength(1)];
                    Array.Copy(_marks, newmatrix, _marks.Length);
                    return newmatrix;

                }
            }
            public double TotalScore
            {
                get
                {
                    if (_marks == null || _coefs == null) return 0;
                    double answer = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        double sum = 0;
                        int minind = 0, maxind = 0;
                        for (int j = 0; j < 7; j++)
                        {
                            if (_marks[i, j] > _marks[i, maxind]) maxind = j;
                            if (_marks[i, j] < _marks[i, minind]) minind = j;
                        }
                        for (int j = 0; j < 7; j++)
                        {
                            if (j != maxind && j != minind) sum += _marks[i, j];
                        }
                        answer += sum * _coefs[i];
                    }
                    return answer;
                }
            }
            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _coefs = new double[4];
                _marks = new int[4, 7];

                _jump = 0;
                for (int i = 0; i < 4; i++)
                {
                    _coefs[i] = 2.5;
                    for (int j = 0; j < 7; j++)
                    {
                        _marks[i, j] = 0;
                    }
                }
            }
            //методы
            public void SetCriterias(double[] coefs)
            {
                if (_coefs == null || coefs == null || _coefs.Length != 4 || coefs.Length != 4) return;
                for (int i = 0; i < 4; i++) _coefs[i] = coefs[i];
            }

            public void Jump(int[] marks)
            {
                if (marks == null || _marks == null || _jump > 3 || marks.Length != 7) return;
                for (int i = 0; i < marks.Length; i++)
                {
                    _marks[_jump, i] = marks[i];
                }
                _jump++;

            }


            public static void Sort(Participant[] array)
            {
                if (array == null) return;
                double[] temp1 = new double[array.Length];
                for (int i = 0; i < array.Length; i++)
                    temp1[i] = array[i].TotalScore;
                Participant temp2;
                for (int i = 1, j = 2; i < array.Length;)
                {
                    if (i == 0 || temp1[i] < temp1[i - 1])
                    {
                        i = j;
                        j++;
                    }
                    else
                    {
                        double temp = temp1[i];
                        temp1[i] = temp1[i - 1];
                        temp1[i - 1] = temp;

                        temp2 = array[i];
                        array[i] = array[i - 1];
                        array[i - 1] = temp2;
                        i--;
                    }
                }
            }
            public void Print()
            {
                Console.WriteLine($"{Name}   {Surname}   {TotalScore}");
            }
        }



        public class Judge
        {
            private string _name;
            private int[] _marks;
            private int _counter;

            public int[] Marks => _marks;
            public string Name => _name;

            public Judge(string name, int[] marks)
            {
                _name = name;
                if (marks != null)
                {
                    _marks = new int[marks.Length];
                    Array.Copy(marks, _marks, marks.Length);
                }
                else _marks = new int[0];
                _counter = 0;

            }
            // методы
            public int CreateMark()
            {
                if (_marks == null || _marks.Length == 0) return 0;
                return _marks[_counter++ % _marks.Length];
            }
            public void Print()
            {

                Console.Write($"{_name,7} - ");
                foreach (int x in _marks) Console.Write($"{x} ");
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
                _judges = judges;
                _participants = new Participant[0];
            }
            // методы
            public void Evaluate(Participant jumper)
            {
                if (_judges != null && jumper != null)
                {
                    int[] arr_marks = new int[_judges.Length];
                    for (int i = 0; i < _judges.Length; i++)
                    {
                        arr_marks[i] = _judges[i].CreateMark();
                    }
                    jumper.Jump(arr_marks);
                }
            }
            public void Add(Participant jumper)
            {
                if (jumper != null)
                {
                    Evaluate(jumper);
                    Participant[] arr = new Participant[_participants.Length + 1];
                    Array.Copy(_participants, arr, _participants.Length);
                    arr[arr.Length - 1] = jumper;
                    _participants = arr;
                }

            }
            public void Add(Participant[] jumpers)
            {
                if (jumpers != null)
                {
                    foreach (Participant jumper in jumpers)
                    {

                        Add(jumper);
                    }

                }

            }
            public void Sort()
            {

                Participant.Sort(_participants);
            }

        }
    }
}
