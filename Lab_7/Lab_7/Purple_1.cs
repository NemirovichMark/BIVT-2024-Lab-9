using System;
using System.Linq;

namespace Lab_7 {
    public class Purple_1
    {
        public class Participant {
            public Participant(string name, string surname) {
                _name = name;
                _surname = surname;
                _jumpCounter = 0;
                _coefs = new double[4] {2.5, 2.5, 2.5, 2.5};
                _marks = new int[4, 7];
                _totalScore = 0;
            }

            private string _name;
            private string _surname;
            private double[] _coefs;
            private int[,] _marks;
            private int _jumpCounter;
            private double _totalScore;

            public string Name => _name;
            public string Surname => _surname;
            public double[] Coefs {
                get {
                    if (_coefs == null) return null;

                    var copy = new double[_coefs.Length];
                    Array.Copy(_coefs, copy, _coefs.Length);
                    return copy;
                }
            }

            public int[,] Marks {
                get {
                    if (_marks == null) return null;

                    var copy = new int[4, 7];
                    for (int i = 0; i < _marks.GetLength(0); i++) {
                        for (int j = 0; j < _marks.GetLength(1); j++) {
                            copy[i, j] = _marks[i, j];
                        }
                    }
                    return copy;
                }
            }
            
            public double TotalScore => _totalScore;

            public void SetCriterias(double[] coefs) {
                if (coefs == null || coefs.Length != 4 || _coefs == null) return;

                for (int i = 0; i < _coefs.Length; i++) {
                    _coefs[i] = coefs[i];
                }
            }

            public void Jump(int[] marks) {
                if (marks == null || _marks == null || _coefs == null || _jumpCounter >= 4 || marks.Length != 7) return;

                double currentScore = (marks.Sum() - marks.Min() - marks.Max()) * _coefs[_jumpCounter];
                _totalScore += currentScore;

                for (int i = 0; i < _marks.GetLength(1); i++) {
                    _marks[_jumpCounter, i] = marks[i];
                }
                _jumpCounter++;
            }

            public static void Sort(Participant[] array) { // insertion sort
                if (array == null) return;

                for (int i = 1; i < array.Length; i++) {
                    Participant key = array[i];
                    int j = i - 1;

                    while (j >= 0 && array[j].TotalScore <= key.TotalScore) {
                        array[j + 1] = array[j];
                        j--;
                    }

                    array[j + 1] = key;
                }
            }

            public void Print() {
                Console.WriteLine($"{_name} {_surname} {_totalScore}");

            }
        }

        public class Judge {
            public Judge(string name, int[] marks) {
                _name = name;

                if (marks != null) {
                    _marks = new int[marks.Length];
                    Array.Copy(marks, _marks, marks.Length);
                }
            }
            private string _name;
            private int[] _marks;
            private int _counter;

            public string Name => _name;
            public int[] Marks => _marks;

            public int CreateMark() {
                if (_marks == null || _marks.Length == 0) return 0;

                int res = _marks[_counter++];
                _counter %= _marks.Length;

                return res;
            }

            public void Print() {
                Console.Write($"{_name}\t");

                foreach (var mark in _marks)
                    Console.Write($"{mark} ");

                Console.WriteLine();
            }
        }
    
        public class Competition {
            public Competition(Judge[] judges) {
                if (judges != null) {
                    _judges = new Judge[judges.Length];
                    Array.Copy(judges, _judges, judges.Length);
                }

                _participants = new Participant[0];
            }

            private Judge[] _judges;
            private Participant[] _participants;

            public Judge[] Judges => _judges;
            public Participant[] Participants => _participants;

            public void Evaluate(Participant jumper) {
                if (_judges == null) return;

                int[] jumperMarks = new int[7];

                int counter = 0;
                foreach (var judge in _judges) {
                    if (counter >= 7) break;

                    if (judge != null)
                        jumperMarks[counter++] = judge.CreateMark();
                }

                jumper.Jump(jumperMarks);
            }

            public void Add(Participant participant) {
                if (participant == null) return;

                if (_participants == null) _participants = new Participant[0];

                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[_participants.Length - 1] = participant;

                this.Evaluate(_participants[_participants.Length - 1]);
            }

            public void Add(Participant[] participants) {
                if (participants == null) return;

                foreach (var participant in participants)
                    this.Add(participant);
            }

            public void Sort() {
                if (_participants == null) return;
                for (int i = 1; i < _participants.Length; i++) {
                    Participant key = _participants[i];
                    int j = i - 1;

                    while (j >= 0 && _participants[j].TotalScore < key.TotalScore)
                    {
                        _participants[j + 1] = _participants[j];
                        j--;
                    }

                    _participants[j + 1] = key;
                }
            }
        }
    }
}