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
            private int _k;
            public string Name { get { return _name; } }
            public string Surname { get { return _surname; } }
            public int Distance { get { return _distance; } }
            public int[] Marks
            {
                get
                {
                    if (_marks == null) return null;
                    int[] copy = new int[_marks.Length];
                    Array.Copy(_marks, copy, _marks.Length);
                    return copy;
                }
            }
            public int Result
            {
                get
                {
                    int rez = 0;
                    int mi = int.MaxValue;
                    int ma = int.MinValue;
                    if (_distance < 0 || _marks == null) return 0;
                    for (int i = 0; i < Marks.Length; i++)
                    {
                        if (Marks[i] > ma) ma = Marks[i];
                        if (Marks[i] < mi) mi = Marks[i];
                        rez += Marks[i];
                    }
                    rez -= ma;
                    rez -= mi;
                    rez += _k;
                    if (rez < 0) return 0;
                    else return rez;
                }
            }

            //конструктор
            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _distance = -1;
                _marks = new int[5];
                _k = 0;
            }
            public void Jump(int distance, int[] marks, int target)
            {
                if (distance < 0) return;
                if (marks == null || _marks == null || distance < 0) return;
                _distance = distance;
                _k = 60 + (_distance - target) * 2;
                for (int i = 0; i < _marks.Length; i++)
                {
                    _marks[i] = marks[i];
                }
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;
                var array1 = array.OrderByDescending(x => x.Result).ToArray();
                Array.Copy(array1, array, array.Length);

            }
            public void Print()
            {
                Console.WriteLine($"Имя: {Name}. Фамилия: {Surname}. Результат: {Result}");
            }
        }
        public abstract class SkiJumping
        {
            private string _name;
            private int _standart;
            private Participant[] _participant;


            public string Name
            {
                get
                {
                    return _name;
                }
            }
            public int Standard
            {
                get
                {
                    return _standart;
                }
            }

            public Participant[] Participants
            {
                get
                {
                    if (_participant == null) return null;

                    return _participant;
                }
            }
            public SkiJumping(string name, int standart)
            {
                _name = name;
                _standart = standart;
                _participant = new Participant[0];
            }



            public void Add(Participant skijumper)
            {
                if (_participant == null) return;

                _participant = _participant.Append(skijumper).ToArray();

            }

            public void Add(Participant[] skijumpers)
            {
                if (_participant == null || skijumpers == null) return;

                _participant = _participant.Concat(skijumpers).ToArray();

            }

            public void Jump(int distance, int[] marks)
            {
                if (_participant == null || marks == null) return;
                for (int i = 0; i < _participant.Length; i++)
                {
                    if (_participant[i].Distance == -1)
                    {
                        _participant[i].Jump(distance, marks, _standart);
                        break;
                    }
                }

            }
            public void Print()
            {
                Console.WriteLine($"Название события : {Name}, Стандарт : {Standard}, Участники : {Participants}");
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