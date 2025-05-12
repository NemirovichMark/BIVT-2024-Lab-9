using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class Purple_3
    {
        const int JUDGES_COUNT = 7;
        public struct Participant
        {
            private string _name;
            private string _surname;
            private double[] _marks;
            private int[] _places;
            private int _currentJudge;

            public string Name => _name;
            public string Surname => _surname;
            public double[] Marks
            {
                get
                {
                    if (_marks == null) return null;
                    return _marks.ToArray();
                }
            }
            public int[] Places
            {
                get
                {
                    if (_places == null) return null;
                    return _places.ToArray();
                }
            }

            public int Score
            {
                get
                {
                    if (_places == null) return 0;
                    int score = 0;
                    foreach (int x in _places)
                        score += x;

                    return score;
                }
            }

            private double MarksSum
            {
                get
                {
                    if (_marks == null) return 0;
                    double sum = 0;
                    foreach (double m in _marks)
                        sum += m;
                    return sum;
                }
            }

            private int TopPlace
            {
                get
                {
                    if (_places == null) return 0;
                    int imin = 0;
                    for (int i = 0; i < _places.Length; i++)
                        if (_places[i] < _places[imin])
                            imin = i;
                    return _places[imin];
                }
            }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _marks = new double[JUDGES_COUNT];
                _places = new int[JUDGES_COUNT];
                _currentJudge = 0;
                for (int i = 0; i < JUDGES_COUNT; i++)
                {
                    _marks[i] = 0;
                    _places[i] = 0;
                }
            }

            public void Evaluate(double result)
            {
                if (_marks == null || _currentJudge >= _marks.Length) return;

                _marks[_currentJudge] = result;
                _currentJudge++;
            }

            private void SetPlace(int judge, int place)
            {
                if (_places == null || judge < 0 || judge >= _places.Length) return;

                _places[judge] = place;
            }

            public static void SetPlaces(Participant[] participants)
            {
                if (participants == null) return;
                for (int i = 0; i < JUDGES_COUNT; i++)
                {
                    var temp = participants.OrderByDescending(x => x.Marks != null ? x.Marks[i] : 0).ToArray();
                    Array.Copy(temp, participants, temp.Length);
                    for (int j = 0; j < participants.Length; j++)
                        participants[j].SetPlace(i, j + 1);
                }
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;

                Array.Sort(array, (a, b) =>
                {
                    if (a.Score == b.Score)
                    {
                        if (a.TopPlace == b.TopPlace)
                        {
                            double x = a.MarksSum - b.MarksSum;
                            if (x < 0) return 1;
                            else if (x > 0) return -1;
                            else return 0;
                        }
                        return a.TopPlace - b.TopPlace;
                    }
                    return a.Score - b.Score;
                });
            }

            public void Print()
            {

            }
        }

        public abstract class Skating
        {
            protected Participant[] participants;
            protected double[] moods;
            public Participant[] Participants => participants;
            public double[] Moods => moods;
            public Skating(double[] moods, bool needModificate = true)
            {
                participants = new Participant[0];
                this.moods = new double[7];
                if (moods == null) return;
                for (int i = 0; i < Math.Min(moods.Length, 7); i++)
                {
                    this.moods[i] = moods[i];
                }
                if (needModificate) ModificateMood();
            }
            protected abstract void ModificateMood();
            public void Evaluate(double[] marks)
            {
                if (marks == null || participants == null || moods == null) return;
                for (int i = 0; i < participants.Length; i++)
                {
                    if (participants[i].Score == 0)
                    {
                        for (int j = 0; j < marks.Length; j++)
                        {
                            participants[i].Evaluate(marks[j] * moods[j]);
                        }
                        return;
                    }
                }
            }
            public void Add(Participant participant)
            {
                this.participants = Participants.Append(participant).ToArray();
            }
            public void Add(Participant[] participants)
            {
                if (participants == null) return;
                this.participants = Participants.Concat(participants).ToArray();
            }
        }

        public class FigureSkating : Skating
        {
            public FigureSkating(double[] mood, bool needModificate = true) : base(mood, needModificate) { }
            protected override void ModificateMood()
            {
                if (moods == null) return;
                for (int i = 0; i < moods.Length; i++)
                {
                    moods[i] += (i + 1) / 10.0;
                }
            }
        }

        public class IceSkating : Skating
        {
            public IceSkating(double[] mood, bool needModificate = true) : base(mood, needModificate) { }

            protected override void ModificateMood()
            {
                if (moods == null) return;
                for (int i = 0; i < moods.Length; i++)
                {
                    moods[i] *= 1 + (i + 1) / 100.0;
                }
            }
        }
    }
}
