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

            private int _target;   //доп. поле

            public string Name => _name;
            public string Surname
            {
                get
                {
                    return _surname;
                }
            }
            public int Distance => _distance;
            public int[] Marks
            {
                get
                {
                    if (_marks == null) return null;
                    int[] newarr = new int[_marks.Length];
                    Array.Copy(_marks, newarr, _marks.Length);
                    return newarr;
                }
            }

            public int Result
            {
                get
                {
                    if (_marks == null || _marks.Length == 0 || _distance == 0) return 0;
                    int answer = 0;
                    int maxres = 0, minres = int.MaxValue;
                    for (int i = 0; i < _marks.Length; i++)
                    {
                        if (_marks[i] > maxres) maxres = _marks[i];
                        if (_marks[i] < minres) minres = _marks[i];
                        answer += _marks[i];
                    }
                    answer = answer - maxres - minres;
                    int razn = _distance - _target;
                    answer += razn * 2 + 60;

                    return Math.Max(answer, 0);



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
                if (_marks == null || marks == null || _marks.Length != _marks.Length) return;
                _distance = distance;
                for (int i = 0; i < 5; i++) _marks[i] = marks[i];
                _target = target;
            }
            public static void Sort(Participant[] array)
            {
                if (array == null) return;
                for (int i = 1, j = 2; i < array.Length;)
                {
                    if (i == 0 || array[i].Result <= array[i - 1].Result)
                    {
                        i = j; j++;
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
                Console.WriteLine($"{Surname,-12}  -  {Result}");
            }
        }

        public abstract class SkiJumping
        {
            private string _name;
            private int _standard;
            private Participant[] _participants;

            public string Name => _name;
            public Participant[] Participants => _participants;
            public int Standard => _standard;
            public SkiJumping(string name, int standart)
            {
                _name = name;
                _standard = standart;
                _participants = new Participant[0];
            }
            public void Add(Participant participant)
            {
                if (_participants == null) return;
                Participant[] arr = new Participant[_participants.Length + 1];
                Array.Copy(_participants, arr, _participants.Length);
                arr[arr.Length - 1] = participant;
                _participants = arr;
            }
            public void Add(Participant[] array)
            {
                if (array != null)
                {
                    foreach (var x in array) Add(x);
                }
            }
            public void Jump(int distance, int[] marks)
            {
                if (marks == null || _participants == null) return;
                foreach (var x in _participants)
                {
                    if (x.Distance == 0)
                    {
                        x.Jump(distance, marks, _standard);
                        break;
                    }
                }
            }
            public void Print()
            {
                Console.WriteLine($"{Name}");
                foreach (var x in _participants) x.Print();
            }


        }

        // наследники
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

        // Purple_2.JuniorSkiJumping a = new Purple_2.JuniorSkiJumping(); 

    }
}

