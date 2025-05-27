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
            private int _count;
            private double _total;


            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _coefs = new double[] { 2.5, 2.5, 2.5, 2.5 };
                _marks = new int[4, 7];
                _count = 0;
                _total = 0;
            }

            public string Name { get { return _name; } }
            public string Surname { get { return _surname; } }
            public double[] Coefs
            {
                get
                {
                    if (_coefs == null) return null;
                    double[] temp = new double[4];
                    for (int i = 0; i < 4; i++)
                    {
                        temp[i] = _coefs[i];
                    }
                    return temp;
                }
            }
            public double TotalScore { get { return _total; } }
            public int[,] Marks
            {
                get
                {
                    if (_marks == null) return null;
                    int[,] temp = new int[4, 7];
                    for (int i = 0; i < _marks.GetLength(0); i++)
                    {
                        for (int j = 0; j < _marks.GetLength(1); j++)
                        {
                            temp[i, j] = _marks[i, j];
                        }
                    }

                    return temp;
                }
            }

            public void SetCriterias(double[] coefs)
            {
                if (coefs == null || _coefs == null || coefs.Length != 4) { return; }
                for (int i = 0; i < 4; i++)
                {
                    _coefs[i] = coefs[i];
                }
            }

            public void Jump(int[] marks)
            {
                if (marks == null || _marks == null || _coefs == null || _count >= 4 || marks.Length != 7) return;
                for (int i = 0; i < 7; i++)
                {
                    _marks[_count, i] = marks[i];
                }
                int[] arr = new int[7];
                for (int i = 0; i < 7; i++) { arr[i] = marks[i]; }
                Array.Sort(arr);
                for (int i = 1; i < 6; i++)
                {
                    _total += arr[i] * _coefs[_count];
                }
                _count++;
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) { return; }

                for (int i = 1; i < array.Length;)
                {
                    if (i == 0 || array[i - 1].TotalScore >= array[i].TotalScore)
                    {
                        i++;
                    }
                    else
                    {
                        Participant tmp = array[i - 1];
                        array[i - 1] = array[i];
                        array[i] = tmp;
                        i--;
                    }
                }

            }

            public void Print()
            {
                Console.WriteLine($"Name: {Name}\nSurname: {Surname}\nTotal: {TotalScore}");

            }

        }

        public class Judge
        {
            private string _name;
            private int[] _favMarks;
            private int _currentMark;

            public string Name => _name;
            public int[] Marks { get
                {
                    if (_favMarks == null) { return null; }
                    var copy = new int[ _favMarks.Length ];
                    Array.Copy(_favMarks, copy, _favMarks.Length );
                    return copy;
                } }

            public Judge(string name, int[] marks)
            {
                _name = name;
                if (marks != null)
                {
                    _favMarks = new int[marks.Length];
                    Array.Copy(marks, _favMarks, marks.Length);
                }
            }

            public int CreateMark()
            {
                if (_favMarks == null || _favMarks.Length == 0) return 0;
                if (_currentMark >= _favMarks.Length) _currentMark = 0;
                return _favMarks[_currentMark++];
            }
            

            public void Print()
            {
                Console.WriteLine($"Name: {_name}");
                Console.WriteLine("Marks: ");
                foreach (int mark in _favMarks) Console.Write($"{mark} ");
                Console.WriteLine();
            }

        }

        public class Competition
        {
            private Participant[] _participants;
            private Judge[] _judges;

            public Judge[] Judges => _judges;
            public Participant[] Participants => _participants;

            public Competition(Judge[] judges)
            {
                _participants = new Participant[0];

                if (judges != null)
                {
                    _judges = new Judge[judges.Length];
                    Array.Copy(judges, _judges, judges.Length);
                }

            }


            public void Evaluate(Participant jumper)
            {
                if (_judges == null) return;
                int[] marks = new int[7];
                int i = 0;
                foreach (Judge j in _judges)
                {
                    if (j != null)
                    {
                        marks[i++] = j.CreateMark();
                        if (i == 7) break;
                    }
                }
                jumper.Jump(marks);
            }


            public void Add(Participant participant) {
                if (participant == null) {return;}
                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[_participants.Length - 1] = participant;
                Evaluate(_participants[_participants.Length - 1]);
            }
            
            public void Add(Participant[] participants) {
                if (participants == null) {return;}
                int n = _participants.Length;
                Array.Resize(ref _participants, n + participants.Length);
                Array.Copy(participants, 0, _participants, n, participants.Length);
                for (int i = n; i < _participants.Length; ++i) Evaluate(_participants[i]);
            }



            public void Sort()
            {
                Participant.Sort(_participants);
            }
        }
    }

}

