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
                    if (_marks == null) return default(int[]);

                    var newArray = new int[_marks.Length];
                    Array.Copy(_marks, newArray, _marks.Length);
                    return newArray;
                }
            }

            public int Result
            {
                get
                {
                    if (_marks == null || _distance == 0) return 0;
                    int sum = Math.Max(0, _marks.Sum() - _marks.Max() - _marks.Min() + 60 + (_distance - _target) * 2);
                    return sum;
                }
            }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _distance = 0;
                _marks = new int[5];
                _target = 0;
            }

            public void Jump(int distance, int[] marks, int target)
            {
                if (_distance != 0 || marks == null || _marks == null || marks.Length != 5) return;
                _distance = distance;
                _target = target;
                Array.Copy(marks, _marks, marks.Length);
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;

                var sortedArray = array.OrderByDescending(x => x.Result).ToArray();
                Array.Copy(sortedArray, array, array.Length);
            }
            public void Print()
            {
                Console.WriteLine(_name + " " + _surname);
                Console.WriteLine(Result);
                Console.WriteLine($"Distance: {_distance}");
                Console.Write("Marks: ");
                foreach (double var in _marks)
                {
                    Console.Write(var + "  ");
                }
                Console.WriteLine();
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

            public void Add(Participant jumper)
            {
                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[_participants.Length - 1] = jumper;
            }
            public void Add(Participant[] jumpers)
            {
                foreach (var jumper in jumpers) Add(jumper);
            }

            public void Jump(int distance, int[] marks)
            {
                for (int i = 0; i < _participants.Length; i++)
                {
                    if (_participants[i].Distance == 0)
                    {
                        var jumper = _participants[i];
                        jumper.Jump(distance, marks, Standard);
                        _participants[i] = jumper;
                        break;
                    }
                }
            }

            public void Print()
            {
                Console.WriteLine(Name);
                Console.WriteLine(Standard);
                foreach (var participant in _participants) participant.Print();
            }
        }

        public class JuniorSkiJumping : SkiJumping
        {
            public JuniorSkiJumping() : base("100m", 100) { }
        }

        public class ProSkiJumping : SkiJumping
        {
            public ProSkiJumping() : base("150m", 150) { }
        }

    }
}