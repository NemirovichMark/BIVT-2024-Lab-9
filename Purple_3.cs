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
            private string _name;
            private string _surname;
            private double[] _marks;
            private int[] _places;

            private int _marksCount;
            public string Name => _name;
            public string Surname => _surname;
            public double[] Marks
            {
                get
                {
                    if (_marks == null) return default(double[]);

                    var newArray = new double[_marks.Length];
                    Array.Copy(_marks, newArray, _marks.Length);
                    return newArray;
                }
            }
            public int[] Places
            {
                get
                {
                    if (_places == null) return default(int[]);

                    var newArray = new int[_places.Length];
                    Array.Copy(_places, newArray, _places.Length);
                    return newArray;
                }
            }

            public int Score => _places == null ? 0 : _places.Sum();
            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _marks = new double[7];
                _places = new int[7];
                _marksCount = 0;
            }

            public void Evaluate(double result)
            {
                if (_marksCount >= 7 || _marks == null) return;
                _marks[_marksCount++] = result;
            }

            public static void SetPlaces(Participant[] participants)
            {
                if (participants == null) return;
                for (int i = 0; i < 7; i++)
                {
                    SortJudge(participants, i);
                    for (int j = 0; j < participants.Length; j++)
                    {
                        participants[j].Method(i, j + 1);
                    }
                }
            }
            private void Method(int i, int j)
            {
                if (_places == null || i < 0 || i >= _places.Length) return;
                _places[i] = j;
            }

            private static void SortJudge(Participant[] array, int judgeIndex)
            {
                foreach (var part in array)
                {
                    if (part.Marks == null) return;
                }
                for (int i = 0; i < array.Length; i++)
                {
                    Participant key = array[i];
                    int j = i - 1;

                    while (j >= 0 && array[j].Marks[judgeIndex] < key.Marks[judgeIndex])
                    {
                        array[j + 1] = array[j];
                        j = j - 1;
                    }
                    array[j + 1] = key;
                }
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;
                foreach (var part in array)
                {
                    if (part.Places == null) return;
                }
                for (int i = 0; i < array.Length; i++)
                {
                    Participant key = array[i];
                    int j = i - 1;

                    while (j >= 0 && Compare(array[j], key))
                    {
                        array[j + 1] = array[j];
                        j = j - 1;
                    }
                    array[j + 1] = key;
                }
            }
            private static bool Compare(Participant p1, Participant p2)
            {

                if (p1.Score != p2.Score) return p1.Score > p2.Score;
                if (p1.Places.Min() != p2.Places.Min()) return p1.Places.Min() > p2.Places.Min(); //Parity by sum of places
                return p1.Marks.Sum() < p2.Marks.Sum(); //Parity by max judge place
            }
            public void Print()
            {
                Console.WriteLine(_name + " " + _surname);
                Console.Write("Marks: ");
                foreach (double var in _marks)
                {
                    Console.Write(var + "  ");
                }
                Console.Write("Places: ");
                foreach (double var in _places)
                {
                    Console.Write(var + "  ");
                }
                Console.WriteLine($"Score: {Score}");
                Console.WriteLine();
            }
        }

        public abstract class Skating
        {
            protected Participant[] _participants;
            protected double[] _moods;

            public Participant[] Participants => _participants;
            public double[] Moods => _moods;


            public Skating(double[] moods, bool needModificate = true)
            {
                if (moods == null || moods.Length < 7) return;
                Array.Resize(ref moods, 7);
                _moods = (double[])moods.Clone();
                if (needModificate) ModificateMood();
                _participants = new Participant[0];
            }

            protected abstract void ModificateMood();


            public void Evaluate(double[] marks)
            {
                if (_participants == null || marks == null) return;

                foreach (var participant in _participants)
                {
                    if (participant.Score == 0)
                    {
                        for (int i = 0; i < marks.Length; i++)
                        {
                            participant.Evaluate(marks[i] * Moods[i]);
                        }
                        break;
                    }
                }
            }
            public void Add(Participant skater)
            {
                if (_participants == null) _participants = new Participant[0];
                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[_participants.Length - 1] = skater;
            }
            public void Add(Participant[] skaters)
            {
                if (skaters == null) return;
                foreach (var skater in skaters) Add(skater);
            }
        }

        public class FigureSkating : Skating
        {
            public FigureSkating(double[] moods, bool needModificate = true) : base(moods, needModificate) { }

            protected override void ModificateMood()
            {
                if (_moods == null) return;
                for (int i = 0; i < _moods.Length; i++)
                {
                    _moods[i] += (i + 1) / 10.0;
                }
            }
        }

        public class IceSkating : Skating
        {
            public IceSkating(double[] moods, bool needModificate = true) : base(moods, needModificate) { }

            protected override void ModificateMood()
            {
                if (_moods == null) return;
                for (int i = 0; i < _moods.Length; i++)
                {
                    _moods[i] *= 1 + (i + 1) / 100.0;
                }
            }
        }
    }
}
