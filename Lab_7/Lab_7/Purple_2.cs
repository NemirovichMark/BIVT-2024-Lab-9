using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class Purple_2
    {
        public struct Participant
        {
            private string _name;
            private string _surname;
            private int _distance;
            private int[] _marks;
            private int _target;

            public string Name => _name;
            public string Surname => _surname;
            public int Distance => _distance;
            public int[] Marks
            {
                get
                {
                    if (_marks == null) return null;

                    int[] marks = new int[_marks.Length];
                    Array.Copy(_marks, marks, _marks.Length);
                    return marks;
                }
            }

            public int Result
            {
                get
                {
                    if (_marks == null || _distance == 0) return 0;

                    int result = 0;
                    int minMark = int.MaxValue;
                    int maxMark = int.MinValue;

                    for (int judge = 0; judge < _marks.Length; judge++)
                    {
                        int currentMark = Marks[judge];

                        if (currentMark > maxMark)
                            maxMark = currentMark;

                        if (currentMark < minMark)
                            minMark = currentMark;

                        result += currentMark;
                    }

                    result -= minMark + maxMark;
                    result += 60;
                    result += (_distance - _target) * 2;

                    return Math.Max(result, 0);
                }
            }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _distance = 0;
                _marks = new int[5] { 0, 0, 0, 0, 0 };
                _target = 0;
            }

            public void Jump(int distance, int[] marks, int target)
            {
                if (marks == null || _marks == null || marks.Length != _marks.Length || _distance != 0) return;

                _distance = distance;
                _target = target;
                Array.Copy(marks, _marks, marks.Length);
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;

                for (int i = 0; i < array.Length; i++)
                {
                    Participant key = array[i];
                    int j = i - 1;

                    while (j >= 0 && array[j].Result < key.Result)
                    {
                        array[j + 1] = array[j];
                        j = j - 1;
                    }
                    array[j + 1] = key;
                }
            }

            public void Print()
            {
                Console.WriteLine($"{Name} {Surname} - Итоговый балл: {Result:F2}");
            }
        }

        public abstract class SkiJumping
        {
            private string _name;
            private int _standard;
            private Participant[] _participants;

            public string Name => _name;
            public int Standard => _standard;
            public Participant[] Participants => _participants;

            public SkiJumping(string name, int standard)
            {
                _name = name;
                _standard = standard;
                _participants = new Participant[0];
            }

            public void Add(Participant participant)
            {
                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[_participants.Length - 1] = participant;
            }

            public void Add(Participant[] participants)
            {
                Array.Resize(ref _participants, _participants.Length + participants.Length);
                Array.Copy(participants, _participants, participants.Length);
            }

            public void Jump(int distance, int[] marks)
            {
                if (_participants == null || marks == null) return;

                for (int i = 0; i < _participants.Length; i++)
                {
                    if (_participants[i].Distance == 0)
                    {
                        _participants[i].Jump(distance, marks, Standard);
                        break;
                    }
                }
            }

            public void Print()
            {
                Console.WriteLine($"Соревнование: {Name}, Норматив дистанции: {Standard}м");
                Console.WriteLine("Участники:");
                for (int i = 0; i < _participants.Length; i++)
                {
                    _participants[i].Print();
                }
            }
        }

        public class JuniorSkiJumping : SkiJumping
        {
            public JuniorSkiJumping() : base("100m", 100)
            {
            }
        }

        public class ProSkiJumping : SkiJumping
        {
            public ProSkiJumping() : base("150m", 150)
            {
            }
        }
    }
}

