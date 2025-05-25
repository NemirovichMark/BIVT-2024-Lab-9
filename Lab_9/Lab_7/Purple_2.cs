using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Lab_7
{
    public class Purple_2
    {
        public struct Participant
        {
            //поля
            private string _name;
            private string _surname;
            private int _distance;
            private int[] _marks;
            private int _needd;
            //свойства
            public string Name => _name;
            public string Surname => _surname;
            public int Distance => _distance;
            public int[] Marks
            {
                get
                {
                    if (_marks == null) return null;

                    int[] copy = new int[_marks.Length];
                    Array.Copy(_marks, copy, copy.Length);
                    return copy;
                }
            }
            public int Result
            {
                get
                {
                    if (_marks == null || _distance == -1) return 0;

                    int worst = 0;
                    int best = 0;
                    int ans = 0;
                    for (int j = 0; j < _marks.Length; j++)
                    {
                        ans += _marks[j];
                        if (_marks[j] > _marks[best])
                            best = j;
                        if (_marks[j] < _marks[worst])
                            worst = j;
                    }
                    ans -= (_marks[best] + _marks[worst]);
                    ans += _needd;
                    if (ans < 0) { return 0; }
                    else
                    {
                        return ans;
                    }
                }
            }
            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _distance = -1;
                _marks = new int[5] { 0, 0, 0, 0, 0 };
                _needd = 0;

            }
            public void Jump(int distance, int[] marks, int target)
            {
                if (marks == null || _marks == null || distance < 0 || marks.Length != _marks.Length) return;
                _distance = distance;
                _needd = 60 + (_distance - target) * 2;
                Array.Copy(marks, _marks, marks.Length);
            }

            public static void Sort(Participant[] array)
            {
                {
                    if (array == null) return;

                    var a = array.OrderByDescending((x) => x.Result).ToArray();
                    Array.Copy(a, array, array.Length);
                }
            }
            public void Print()
            {

                System.Console.WriteLine($"{_name} {_surname} {_marks}");
            }
        }
        public abstract class SkiJumping
        {
            private string _name;
            private int _standard;
            private Participant[] _participants;
            public string Name => _name;
            public int Standard => _standard;
            public Participant[] Participants
            {
                get
                {
                    if (_participants == null) return null;
                    var copy = new Participant[_participants.Length];
                    Array.Copy(_participants, copy, _participants.Length);
                    return copy;
                }
            }
            public SkiJumping(string name, int standard)
            {
                _name = name;
                _standard = standard;
                _participants = new Participant[0];
            }
            public void Add(Participant participant)
            {
                if (_participants == null) return;
                var part1 = new Participant[_participants.Length + 1];
                Array.Copy(_participants, part1, _participants.Length);
                part1[part1.Length - 1] = participant;
                _participants = part1;
            }
            public void Add(Participant[] participants)
            {
                if (_participants == null || participants == null) return;
                int l = _participants.Length;
                var part2 = new Participant[l + participants.Length];
                Array.Copy(_participants, part2, l);
                Array.ConstrainedCopy(participants, 0, part2, l, participants.Length);
                _participants = part2;
            }
            public void Jump(int distance, int[] marks)
            {
                if (_participants == null) return;
                foreach (var x in _participants)
                {
                    if (x.Distance == 0)
                    {
                        x.Jump(distance, marks, _standard);
                    }
                }
            }
            public void Print()
            {
                Console.WriteLine($"{_name} {_standard}");
                if (_participants == null) return;
                for (int i = 0; i < _participants.Length; i++) _participants[i].Print();
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