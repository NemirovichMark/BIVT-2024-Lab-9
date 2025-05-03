using System;
using System.Linq;

namespace Lab_7 {
    public class Purple_3 {
        public struct Participant {
            public Participant(string name, string surname) {
                _name = name;
                _surname = surname;

                _marks = new double[7];
                _places = new int[7];

                _marksCounter = 0;
            }
            private string _name;
            private string _surname;
            private double[] _marks;
            private int[] _places;

            private int _marksCounter;

            public string Name => _name;
            public string Surname => _surname;
            public double[] Marks {
                get {
                    if (_marks == null) return null;

                    var copy = new double[_marks.Length];
                    Array.Copy(_marks, copy, _marks.Length);

                    return copy;
                }
            }

            public int[] Places {
                get {
                    if (_places == null) return null;
                    var copy = new int[_places.Length];
                    Array.Copy(_places, copy, _places.Length);

                    return copy;
                }
            }

            public int Score {
                get {
                    if (_places == null) return 0;

                    return _places.Sum();
                }
            }

            public void Evaluate(double result) {
                if (_marks == null || _marksCounter >= _marks.Length) return;

                _marks[_marksCounter++] = result;
            }

            public static void SetPlaces(Participant[] participants) {
                if (participants == null || participants.Any(participant => participant._marks == null)) return;

                for (int judge = 0; judge < 7; judge++) {
                    for (int i = 1; i < participants.Length; i++) { // insertion sort
                        Participant key = participants[i];
                        int j = i - 1;
                        while (j >= 0 && participants[j].Marks[judge] < key.Marks[judge]) {
                            participants[j + 1] = participants[j];
                            j--;
                        }
                        participants[j + 1] = key;
                    }

                    for (int i = 0; i < participants.Length; i++) {
                        participants[i]._places[judge] = i + 1;
                    }
                }
            }

            public static void Sort(Participant[] array) {
                if (array == null) return;
                for (int i = 0; i < array.Length - 1; i++) {
                    for (int j = 0; j < array.Length - (i + 1); j++) {
                        Participant first = array[j], second = array[j+1];
                        if (first._places == null || first._marks == null || second._places == null || second._marks == null) continue;

                        if ((first.Score > second.Score) || 
                            (first.Score == second.Score && first._places.Min() > second._places.Min()) || 
                            (first.Score == second.Score && first._places.Min() == second._places.Min() && first._marks.Sum() < second._marks.Sum())) {
                                array[j] = second;
                                array[j + 1] = first;
                            }
                        
                    }
                }
            }

            public void Print() {
                Console.Write($"{_name, 15} {_surname, 15}\t");
                if (_places != null)
                    Console.Write($"{_places.Sum()}\t");
                    Console.Write($"{_places.Min()}\t");
                if (_marks != null)
                    Console.Write($"{Math.Round(_marks.Sum(), 2)}\t");
                Console.WriteLine();
            }


        }
    
        public abstract class Skating {
            public Skating(double[] moods, bool needModificate = true) {
                _participants = new Participant[0];

                if (moods == null) { 
                    _moods = new double[7];
                }
                else {
                    int len = (moods.Length > 7) ? 7 : moods.Length; // <= 7
                    _moods = new double[len];
                    Array.Copy(moods, _moods, len);
                }
                if (needModificate)
                    this.ModificateMood();
            }

            protected abstract void ModificateMood();

            protected double[] _moods;
            private Participant[] _participants;

            public double[] Moods => _moods;

            public Participant[] Participants => _participants;

            public void Evaluate(double[] marks) {
                if (marks == null) return;

                for (int i = 0; i < _participants.Length; i++) {
                    ref Participant participant = ref _participants[i];
                    if (participant.Marks == null || participant.Marks.All(mark => mark == 0)) {
                        for (int j = 0; j < marks.Length; j++) {
                            participant.Evaluate(marks[j] * _moods[j]);
                        }
                        break;
                    }
                }
            }

            public void Add(Participant participant) {
                if (_participants == null) _participants = new Participant[0];

                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[_participants.Length - 1] = participant;
            }

            public void Add(Participant[] participants) {
                if (participants == null) return;

                foreach (var participant in participants)
                    this.Add(participant);
            }
        }

        public class FigureSkating : Skating {
            public FigureSkating(double[] moods, bool needModificate = true) : base(moods, needModificate) {}

            protected override void ModificateMood()
            {
                if (_moods == null) return;

                for (int i = 0; i < _moods.Length; i++) {
                    _moods[i] += (double)(i + 1.0) / 10.0;
                }
            }
        }

        public class IceSkating : Skating {
            public IceSkating(double[] moods, bool needModificate = true) : base(moods, needModificate) {}

            protected override void ModificateMood()
            {
                if (_moods == null) return;

                for (int i = 0; i < _moods.Length; i++) {
                    _moods[i] *= 1.0 + (double)(i + 1.0) / 100.0;
                }
            }
        }
    }
}
