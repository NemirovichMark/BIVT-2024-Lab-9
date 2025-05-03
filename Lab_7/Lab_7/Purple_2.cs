using System;
using System.Linq;

namespace Lab_7 {
    public class Purple_2
    {
        public struct Participant {
            public Participant(string name, string surname) {
                _name = name;
                _surname = surname;
                _marks = new int[5];
                _jumpDistance = 0;
                Result = 0;
            }

            private string _name;
            private string _surname;
            private int _jumpDistance;
            private int[] _marks;

            public string Name => _name;
            public string Surname => _surname;
            public int Distance => _jumpDistance;
            public int[] Marks {
                get {
                    if (_marks == null) return null;

                    var copy = new int[_marks.Length];
                    Array.Copy(_marks, copy, _marks.Length);
                    return copy;
                }
            }

            public int Result {
                get; private set;
            }

            public void Jump(int distance, int[] marks, int target) {
                if (_marks == null || marks == null || marks.Length != 5 || distance < 0) return;
                _jumpDistance = distance;
                Array.Copy(marks, _marks, marks.Length);

                int tempResult = _marks.Sum() - (_marks.Min() + _marks.Max()) + 60 + (distance - target) * 2;

                if (tempResult < 0)
                    Result = 0;
                else
                    Result = tempResult;
            }

            public static void Sort(Participant[] array) { // insertion sort
                if (array == null) return;

                for (int i = 1; i < array.Length; i++) {
                    Participant key = array[i];
                    int j = i - 1;

                    while (j >= 0 && array[j].Result < key.Result) {
                        array[j + 1] = array[j];
                        j--;
                    }

                    array[j + 1] = key;
                }
            }

            public void Print() {
                Console.WriteLine($"{_name, 15} {_surname, 15} {Result, 15}");
            }
        }
    
        public abstract class SkiJumping {
            public SkiJumping(string name, int standard) {
                _name = name;
                _standard = standard;
                _participants = new Participant[0];
            }

            private string _name;
            private int _standard;
            private Participant[] _participants;

            public string Name => _name;
            public int Standard => _standard;
            public Participant[] Participants {
                get {
                    if (_participants == null) return null;

                    var copy = new Participant[_participants.Length];
                    Array.Copy(_participants, copy, _participants.Length);
                    return copy;
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

            public void Jump(int distance, int[] marks) {
                for (int i = 0; i < _participants.Length; i++) {
                    ref Participant participant = ref _participants[i];
                    if (participant.Marks == null || participant.Marks.All(mark => mark == 0)) {
                        participant.Jump(distance, marks, _standard);
                        break;
                    }
                }
            }

            public void Print() {
                Console.WriteLine($"{_name}\t{_standard}");
                foreach (var participant in _participants)
                    participant.Print();
            }

        }

        public class JuniorSkiJumping : SkiJumping {
            public JuniorSkiJumping() : base("100m", 100) { }
        }

        public class ProSkiJumping : SkiJumping {
            public ProSkiJumping() : base("150m", 150) { }
        }
    }
}