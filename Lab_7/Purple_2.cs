using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

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

            public string Name => _name;
            public string Surname => _surname;
            public int Distance => _distance;
            public int[] Marks
            {
                get
                {
                    if (_marks == null) return null;

                    int[] makrsCopy = new int[_marks.Length];
                    Array.Copy(_marks, makrsCopy, _marks.Length);
                    return makrsCopy;
                }
            }
            public int Result { get; private set; }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _marks = new int[5];
            }
            
            public void Jump(int distance, int[] marks, int target)
            {
                if (marks == null || _marks == null) return;
                _distance = distance;
                Array.Copy(marks, _marks, _marks.Length);

                int distancePoints = 60 + (distance - target) * 2;
                Result = distancePoints + marks.Sum() - marks.Min() - marks.Max();
                if (Result < 0) Result = 0;
            }

            public static void Sort(Participant[] array)
            {
                if (array == null || array.Length == 0) return;

                var sortedArray = array.OrderByDescending(x => x.Result).ToArray();
                Array.Copy(sortedArray, array, array.Length);
            }

            public void Print()
            {
                Console.WriteLine($"{_printItem(_name)} {_printItem(_surname)} {_printItem(Result.ToString())}");
            }
            private static string _printItem(string item)
            {
                return item + new string(' ', 15 - item.Length);
            }
        }
        public abstract class SkiJumping
        {
            private string _name;
            private int _standard;
            private Participant[] _participants;

            public string Name => _name;
            public int Standard => _standard;
            public Participant[] Participants => _participants == null ? null : (Participant[])_participants.Clone();

            public SkiJumping(string name, int standart)
            {
                _name = name;
                _standard = standart;
                _participants = new Participant[0];
            }

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
            public void Jump(int distance, int[] marks)
            {
                int jumperIndex = Array.FindIndex(_participants, participant => participant.Marks.All(x => x == 0));
                if (jumperIndex == -1) return;

                _participants[jumperIndex].Jump(distance, marks, Standard); ;
            }
            public void Print()
            {
                string[] items = new string[] { "Name", "Surname", "Result" };
                Console.WriteLine(_printItem(_name, (int)((60 - _name.Length) / 2), false));
                _printHead(items);

                foreach (Participant participant in _participants)
                {
                    participant.Print();
                }
            }
            private static void _printHead(string[] items)
            {
                foreach (string item in items)
                {
                    Console.Write(_printItem(item));
                }
                Console.WriteLine();
            }
            private static string _printItem(string item, int len = 16, bool flag = true)
            {
                if (flag) return item + new string(' ', len - item.Length);
                else return new string(' ', len - item.Length) + item;
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