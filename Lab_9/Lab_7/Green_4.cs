using System;
using System.Linq;
using static Lab_7.Green_1;
using static Lab_7.Green_4;


namespace Lab_7
{
    public class Green_4
    {
        public struct Participant
        {
            private string _name;
            private string _surname;
            private double[] _jumps;

            public string Name => _name;

            public string Surname => _surname;

            public double[] Jumps
            {
                get
                {
                    return (double[])_jumps?.Clone();
                }
            }


            public double BestJump
            {
                get
                {
                    if (_jumps != null && _jumps.Length > 0)
                    {
                        return _jumps.Max();
                    }
                    return 0;
                }
            }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _jumps = new double[3];
            }

            public void Jump(double result)
            {
                if (_jumps == null) return;
                if (_jumps != null)
                {
                    for (int i = 0; i < _jumps.Length; i++)
                    {
                        if (_jumps[i] == 0)
                        {
                            _jumps[i] = result;
                            return;
                        }
                    }
                }

            }

            public static void Sort(Participant[] array)
            {
                for (int i = 1, j = 2; i < array.Length;)
                {
                    if (i == 0 || array[i].BestJump <= array[i - 1].BestJump)
                    {
                        i = j;
                        j++;
                    }
                    else
                    {
                        Participant temp = array[i];
                        array[i] = array[i - 1];
                        array[i - 1] = temp;
                        i--;
                    }
                }
            }

            public void Print()
            {
                Console.WriteLine($" {Name} {Surname} {BestJump}");
            }

        }

        public abstract class Discipline
        {
            private string _name;
            private Participant[] _participants;

            public string Name => _name;
            public Participant[] Participants => _participants;

            public Discipline(string name)
            {
                _name = name;
                _participants = new Participant[0];

            }

            public void Add(Participant participant)
            {
                Participant[] updated = new Participant[_participants.Length + 1];

                for (int i = 0; i < _participants.Length; i++)
                {
                    updated[i] = _participants[i];
                }
                updated[updated.Length - 1] = participant;

                _participants = updated;
            }
            public void Add(Participant[] participants)
            {
                int oldLength = _participants.Length;
                Array.Resize(ref _participants, oldLength + participants.Length);
                Array.Copy(participants, 0, _participants, oldLength, participants.Length);
            }

            public void Sort()
            {
                Participant.Sort(_participants);
            }

            public abstract void Retry(int index);

            public void Print()
            {
                Console.WriteLine($"Дисциплина: {Name}");
                foreach (var p in _participants)
                {
                    p.Print();
                }
            }
        }

        public class LongJump : Discipline
        {
            public LongJump() : base("Long jump")
            {
            }

            public override void Retry(int index)
            {
                if (index < 0 || index > Participants.Length) return;
                Participant p = Participants[index];
                double best = p.BestJump;
                Participant newP = new Participant(p.Name, p.Surname);
                newP.Jump(best);
                newP.Jump(0);
                newP.Jump(0);
                Participants[index] = newP;
            }
        }

        public class HighJump : Discipline
        {
            public HighJump() : base("High jump")
            {
            }

            public override void Retry(int index)
            {
                if (index < 0 || index > Participants.Length) return;

                Participant p = Participants[index];
                double[] jumps = p.Jumps;
                if (jumps.Length == 0) return;

                var lastIndex = -1;
                for (int i = jumps.Length - 1; i >= 0; i--)
                {
                    if (jumps[i] > 0)
                    {
                        lastIndex = i;
                        break;
                    }
                }
                if (lastIndex == -1) return;

                jumps[lastIndex] = 0;
                Participant newP = new Participant(p.Name, p.Surname);
                for (int i = 0; i < jumps.Length; i++)
                {
                    if (jumps[i] > 0)
                    {
                        newP.Jump(jumps[i]);
                    }
                }
                Participants[index] = newP;
            }
        }
    }
}