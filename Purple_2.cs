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
            private string _name, _surname;
            private int _distance;
            private int[] _marks; // 5 marks
            private int _result;
            private bool _jumping;

            public string Name => _name;
            public string Surname => _surname;

            public int Distance => _distance;

            public int[] Marks
            {
                get
                {
                    if (_marks == null || _marks.Length != 5) return default(int[]);
                    int[] scores = new int[_marks.Length];
                    Array.Copy(_marks, scores, _marks.Length);
                    return scores;
                }
            }

            public int Result { get { return _result; } }


            public Participant(string name, string surname)
            {
                _name = name; _surname = surname; _distance = -500;
                _marks = new int[] { 0, 0, 0, 0, 0 };
                _result = 0; _jumping = false;
            }

            public void Jump(int distance, int[] marks, int target)
            {
                if (marks == null || _marks == null || distance < 0 || marks.Length != _marks.Length || _jumping == true) return;
                _distance = distance; // (1)
                _jumping = true;
                for (int i = 0; i < marks.Length; i++)
                {
                    _marks[i] = marks[i];
                }

                int result = 0; int imin = 0; int imax = 0;  // int copy = _distance; (1)
                for (int i = 0; i < _marks.Length; i++)
                {
                    result += _marks[i];
                    if (_marks[i] > _marks[imax]) { imax = i; }
                    if (_marks[i] < _marks[imin]) { imin = i; }
                }
                result -= _marks[imin]; result -= _marks[imax];

                int x = 60 + 2 * (_distance - target);
                result += x;
                result = Math.Max(0, result);
                _result = result;
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;
                Array.Sort(array, (a, b) => { return b.Result - a.Result; });
            }
            public void Print() { }
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

            public void Add(Participant participant1)
            {
                if (_participants == null) return;

                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[_participants.Length - 1] = participant1;

            }
            public void Add(Participant[] participants)
            {
                if (_participants == null || participants == null) return;
                foreach (Participant participant in participants) { Add(participant); }
            }
            public void Jump(int distance, int[] marks)
            {
                if (_participants == null || marks == null) return;

                for (int i = 0; i < _participants.Length; i++)
                {
                    if (_participants[i].Distance == -500)
                    {
                        _participants[i].Jump(distance, marks, _standard);
                        break;
                    }
                }
            }
            public void Print() { }
        }
        public class JuniorSkiJumping : SkiJumping
        {
            public JuniorSkiJumping() : base("100m", 100) { } // sending to father-class
        }
        public class ProSkiJumping : SkiJumping
        {
            public ProSkiJumping() : base("150m", 150) { } // sending to father-class
        }

    }
}
