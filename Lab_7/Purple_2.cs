using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class Purple_2
    {
        const int JUDGES_COUNT = 5;
        public struct Participant
        {
            private string _name;
            private string _surname;
            private int _distance;
            private int[] _marks;
            private bool _jumped;
            private int _result;

            public string Name => _name;
            public string Surname => _surname;
            public int Distance => _distance;
            public int[] Marks
            {
                get
                {
                    if (_marks == null) return null;

                    int[] marks = new int[_marks.Length];
                    Array.Copy(_marks, marks, marks.Length);
                    return marks;
                }
            }
            public int Result => _result;

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _distance = -1;
                _marks = new int[JUDGES_COUNT];
                _jumped = false;
                _result = 0;
                for (int i = 0; i < JUDGES_COUNT; i++) _marks[i] = 0;
            }

            public void Jump(int distance, int[] marks, int target)
            {
                if (marks == null || _marks == null || marks.Length != _marks.Length || _jumped) return;

                _jumped = true;
                _distance = distance;
                Array.Copy(marks, _marks, marks.Length);

                int result = 0;
                int imax = 0, imin = 0;
                for (int i = 0; i < _marks.Length; i++)
                {
                    result += _marks[i];
                    if (_marks[i] > _marks[imax]) imax = i;
                    if (_marks[i] < _marks[imin]) imin = i;
                }
                result -= _marks[imax] + _marks[imin];
                result += 60;
                result += (_distance - target) * 2;
                result = Math.Max(result, 0);
                _result = result;
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;

                Array.Sort(array, (a, b) => {
                    return b.Result - a.Result;
                });
            }

            public void Print()
            {

            }
        }

        public abstract class SkiJumping
        {
            public string Name { get; private set; }
            public int Standard { get; private set; }
            public Participant[] Participants { get; private set; }

            public SkiJumping(string name, int standard)
            {
                Name = name;
                Standard = standard;
                Participants = new Participant[0];
            }

            public void Add(Participant participant)
            {
                Participants = Participants.Append(participant).ToArray();
            }
            public void Add(Participant[] participants)
            {
                if (participants == null) return;
                Participants = Participants.Concat(participants).ToArray();
            }

            public void Jump(int distance, int[] marks)
            {
                if (Participants == null || marks == null) return;
                for (int i = 0; i < Participants.Length; i++)
                {
                    if (Participants[i].Distance == -1)
                    {
                        Participants[i].Jump(distance, marks, Standard);
                        break;
                    }
                }
            }

            public void Print() { }
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
