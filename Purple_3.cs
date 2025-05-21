using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class Purple_3
    {
        public struct Participant
        {
            private string _name, _surname;
            private double[] _marks;
            private int[] _places_judges;
            private int _amount;

            public string Name => _name;
            public string Surname => _surname;

            public double[] Marks
            {
                get
                {
                    if (_marks == null) return default(double[]);
                    double[] copy = new double[_marks.Length];
                    Array.Copy(_marks, copy, _marks.Length);
                    return copy;
                }
            }

            public int[] Places
            {
                get
                {
                    if (_places_judges == null) return default(int[]);
                    int[] copy = new int[_places_judges.Length];
                    Array.Copy(_places_judges, copy, _places_judges.Length);
                    return copy;
                }
            }

            public int Score // total_marks_judges 
            {
                get
                {
                    if (_places_judges == null) return default;
                    int copy = 0;
                    for (int i = 0; i < _places_judges.Length; i++)
                    {
                        copy += _places_judges[i];
                    }
                    return copy;
                }
            }
            private int Total_place // [34,5,23,1,32,4,89] ==> 1 = the best position at all
            {
                get
                {
                    if (_places_judges == null) return default(int);
                    int unnes = 10000000; int ind_top_mesta = -1;
                    for (int i = 0; i < _places_judges.Length; i++)
                    {
                        if (_places_judges[i] < unnes)
                        {
                            {
                                unnes = _places_judges[i]; ind_top_mesta = i;
                            }
                        }
                    }
                    return _places_judges[ind_top_mesta];
                }
            }
            private double Total_Sum_Mark // last stolbetc in test
            {
                get
                {
                    if (_marks == null) return default;
                    double sum_copy = 0;
                    for (int i = 0; i < _marks.Length; i++)
                    {
                        sum_copy += _marks[i];

                    }
                    return sum_copy;
                }
            }
            public Participant(string name, string surname)
            {
                _name = name; _surname = surname; _amount = 0;
                _places_judges = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
                _marks = new double[7] { 0, 0, 0, 0, 0, 0, 0 };
            }

            public void Evaluate(double result)
            {
                if (_marks == null || _amount >= 7 || result < 0 || result > 6) return;
                _marks[_amount] = result; _amount += 1;
            }
            public static void SetPlaces(Participant[] participants)
            {
                if (participants == null) return;
                for (int i = 0; i < 7; i++)
                {
                    Array.Sort(participants, (a, b) =>
                    {
                        double A1 = 0, B1 = 0;

                        if (a.Marks == null) A1 = 0; else { A1 = a.Marks[i]; }
                        if (b.Marks == null) B1 = 0; else { B1 = b.Marks[i]; }


                        if (A1 - B1 < 0) return 1;
                        else if (A1 - B1 > 0) return -1;
                        else return 0;
                    });
                    for (int j = 0; j < participants.Length; j++)
                    {
                        participants[j].Link_for_SetPlaces(i, j + 1);
                    }
                }
            }
            private void Link_for_SetPlaces(int i, int j)
            {
                if (_places_judges == null || i < 0 || i >= 7) return;
                _places_judges[i] = j;
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;
                Array.Sort(array, (a, b) =>
                {
                    if (a.Score == b.Score)
                    {
                        if (a.Total_place == b.Total_place)
                        {
                            if (a.Total_Sum_Mark - b.Total_Sum_Mark > 0) return -1;
                            else if (a.Total_Sum_Mark - b.Total_Sum_Mark < 0) return 1;
                            else return 0;
                        }
                        return a.Total_place - b.Total_place;
                    }
                    return a.Score - b.Score;
                }
                );
            }
            public void Print() { }
        }
        public abstract class Skating
        {
            protected Participant[] _participants;
            protected double[] _moods;

            public Participant[] Participants => _participants;
            public double[] Moods => _moods;

            public Skating(double[] moods, bool needModificate = true) // ADDING BOOL PARAMETR
            {
                _participants = new Participant[0];
                if (moods == null) { _moods = new double[7]; }
                else if (moods != null)
                {
                    int length_of_moods = Math.Min(moods.Length, 7);
                    _moods = new double[length_of_moods];
                    Array.Copy(moods, _moods, length_of_moods);
                }
                if (needModificate == true) // New logic of method using
                {
                    ModificateMood();
                }
            }
            protected abstract void ModificateMood();


            public void Evaluate(double[] marks)
            {
                if (marks == null || _participants == null || _moods == null || marks.Length == 0 || _moods.Length == 0) return;

                for (int p = 0; p < _participants.Length; p++)
                {
                    if (_participants[p].Score == 0)
                    {
                        for (int m = 0; m < marks.Length; m++)
                        {
                            _participants[p].Evaluate(marks[m] * _moods[m]);
                        }
                    }
                }
            }
            public void Add(Participant participant1)
            {
                if (_participants == null) return;

                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[_participants.Length - 1] = participant1;

            }
            public void Add(Participant[] participants)
            {
                if (_participants == null || participants == null) return;
                foreach (Participant participant in participants) { Add(participant); }
            }

        }
        public class FigureSkating : Skating
        {
            public FigureSkating(double[] moods, bool needModificate = true) : base(moods, needModificate) { } // NEW BOOL PAR
            protected override void ModificateMood()
            {
                if (_moods == null) return;
                for (int j = 0; j < _moods.Length; j++)
                {
                    _moods[j] += (double)(j + 1.0) / 10;
                }
            }
        }

        public class IceSkating : Skating
        {
            public IceSkating(double[] moods, bool needModificate = true) : base(moods, needModificate) { } // NEW BOOL PAR
            protected override void ModificateMood()
            {
                if (_moods == null) return;
                for (int j = 0; j < _moods.Length; j++)
                {
                    _moods[j] += _moods[j] * (double)(j + 1.0) / 100;
                }
            }
        }
    }
}
