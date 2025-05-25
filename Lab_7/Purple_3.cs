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
            private int _currentJudge;

            public string Name => _name;
            public string Surname => _surname;
            public int Score => _places == null ? 0 :_places.Sum();
            public double[] Marks
            {
                get
                {
                    if (_marks == null) return null;

                    double[] marksCopy = new double[_marks.Length];
                    Array.Copy(_marks, marksCopy, _marks.Length);
                    return marksCopy;
                }
            }
            public int[] Places
            {
                get
                {
                    if (_places == null) return null;

                    int[] placesCopy = new int[_places.Length];
                    Array.Copy(_places, placesCopy, _places.Length);
                    return placesCopy;
                }
            }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _marks = new double[7];
                _places = new int[7];
                _currentJudge = 0;
            }
            public void Evaluate(double result)
            {
                if (_currentJudge >= 7 || _marks == null) return;

                _marks[_currentJudge] = result;
                _currentJudge++;
            }
            public static void SetPlaces(Participant[] participants)
            {
                if (participants == null) return;

                for (int i = 0; i < 7; i++)
                {
                    var sortedParticipants = participants.OrderByDescending(x => x.Marks[i]).ToArray();

                    for (int j = 0; j < participants.Length; j++)
                    {
                        sortedParticipants[j]._places[i] = j + 1;
                    }

                    Array.Copy(sortedParticipants, participants, participants.Length);
                }
            }
            public static void Sort(Participant[] array)
            {
                if (array == null) return;

                var sortedArray = array.OrderBy(x => x.Score).ThenBy(x => x.Places.Min()).ThenByDescending(x => x.Marks.Sum()).ToArray();
                Array.Copy(sortedArray, array, array.Length);
            }

            public void Print()
            {
                Console.WriteLine($"{_printItem(Name)} {_printItem(Surname)} {_printItem(Score.ToString())} {_printItem(Places.Min().ToString())} {_printItem(Marks.Sum().ToString(), 50)}");
            }
            private static string _printItem(string item, int spaces = 15)
            {
                return item + new string(' ', spaces - item.Length);
            }
        }
        public abstract class Skating
        {
            private Participant[] _participants;
            protected double[] _moods;

            public Participant[] Participants => _participants;
            public double[] Moods => _moods;
            public Skating(double[] moods, bool needModificate = true)
            {
                _participants = new Participant[0];
                if (moods == null) return;
                int size = moods.Length < 7 ? moods.Length : 7;
                _moods = new double[size];
                Array.Copy(moods, _moods, size);
                if (needModificate) ModificateMood();
            }
            protected abstract void ModificateMood();

            public void Add(Participant participant)
            {
                int participantsLength = _participants.Length;
                Array.Resize(ref _participants, participantsLength + 1);
                _participants[participantsLength] = participant;
            }
            public void Add(Participant[] participants)
            {
                if (participants == null || participants.Length == 0) return;

                _participants = _participants.Concat(participants).ToArray();
            }
            public void Evaluate(double[] marks)
            {
                if (marks == null || marks.Length == 0) return;

                int targetParticipantIndex = Array.FindIndex(_participants, p => p.Marks.All(x => x == 0));
                if (targetParticipantIndex == -1) return;
                Participant targetParticipant = _participants[targetParticipantIndex];

                for (int i = 0; i < marks.Length; i++)
                    targetParticipant.Evaluate(marks[i] * _moods[i]);

                _participants[targetParticipantIndex] = targetParticipant;
            }
        }
        public class FigureSkating : Skating
        {
            public FigureSkating(double[] moods, bool needModificate = true) : base(moods, needModificate) { }
            protected override void ModificateMood()
            {
                for (int i = 0; i < _moods.Length; i++)
                    _moods[i] += (i + 1) / 10.0;
            }
        }
        public class IceSkating : Skating
        {
            protected override void ModificateMood()
            {
                for (int i = 0; i < _moods.Length; i++)
                {
                    _moods[i] *= 1 + (i + 1) / 100.0;
                }
            }
            public IceSkating(double[] moods, bool needModificate = true) : base(moods, needModificate) { }
        }
    }
}
