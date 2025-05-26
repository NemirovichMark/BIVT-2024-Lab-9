using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class Purple_4
    {
        public class Sportsman
        {
            private string _name;
            private string _surname;
            private double _time;

            private bool _test;

            public string Name => _name;
            public string Surname => _surname;
            public double Time => _time;

            public Sportsman(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _time = 0;
                _test = false;
            }

            public void Run(double time)
            {
                if (_test == false)
                {
                    _time = time;
                    _test = true;
                }
            }

            public void Print()
            {
                Console.WriteLine($"{Name,12}  {Surname,12} - {Time}");

            }
            public static void Sort(Sportsman[] array)
            {
                if (array == null) return;
                var arr = array.OrderBy(x => x.Time).ToArray();
                Array.Copy(arr, array, array.Length);
            }
        }

        //наследники
        public class SkiMan : Sportsman
        {
            public SkiMan(string name, string surname) : base(name, surname) { }
            public SkiMan(string name, string surname, double time) : base(name, surname)
            {
                Run(time);
            }
        }
        public class SkiWoman : Sportsman
        {
            public SkiWoman(string name, string surname) : base(name, surname) { }
            public SkiWoman(string name, string surname, double time) : base(name, surname)
            {
                Run(time);
            }
        }

        public class Group
        {
            private string _name;
            private Sportsman[] _sportsmen;



            public string Name => _name;
            public Sportsman[] Sportsmen => _sportsmen;

            public Group(string name)
            {
                _name = name;
                _sportsmen = new Sportsman[0];


            }

            public Group(Group group)
            {
                _name = group.Name;

                if (group.Sportsmen != null)
                {
                    _sportsmen = new Sportsman[group.Sportsmen.Length];
                    Array.Copy(group.Sportsmen, _sportsmen, _sportsmen.Length);

                }
                else _sportsmen = new Sportsman[0];
            }


            //Методы
            public void Add(Sportsman s)
            {
                if (_sportsmen == null) return;
                int i;
                Sportsman[] copy = new Sportsman[_sportsmen.Length + 1];
                for (i = 0; i < _sportsmen.Length; i++) copy[i] = _sportsmen[i];
                copy[i] = s;
                _sportsmen = copy;
            }

            public void Add(Sportsman[] s)
            {
                if (_sportsmen == null || s == null) return;
                Sportsman[] copy = new Sportsman[_sportsmen.Length + s.Length];
                Array.Copy(_sportsmen, copy, _sportsmen.Length);
                for (int i = 0; i < s.Length; i++)
                {
                    copy[_sportsmen.Length + i] = s[i];
                }
                _sportsmen = copy;
            }
            public void Add(Group group)
            {
                if (_sportsmen == null || group._sportsmen == null) return;
                Add(group._sportsmen);
            }

            public void Sort()
            {
                if (_sportsmen == null) return;
                var sp = _sportsmen.OrderBy(x => x.Time).ToArray();
                Array.Copy(sp, _sportsmen, _sportsmen.Length);
            }
            public static Group Merge(Group group1, Group group2)
            {

                Group finalists = new Group("Финалисты");
                if (group1._sportsmen != null && group2._sportsmen != null)
                {
                    int l1 = group1._sportsmen.Length, l2 = group2._sportsmen.Length, l = l1 + l2;
                    finalists._sportsmen = new Sportsman[group1._sportsmen.Length + group2._sportsmen.Length];
                    int i = 0, j = 0, k = 0;
                    while (i < l1 && j < l2)
                    {
                        if (group1._sportsmen[i].Time <= group2._sportsmen[j].Time)
                        {
                            finalists._sportsmen[k++] = group1._sportsmen[i++];
                        }
                        else
                        {
                            finalists._sportsmen[k++] = group2._sportsmen[j++];
                        }
                    }
                    while (i < l1) finalists._sportsmen[k++] = group1._sportsmen[i++];
                    while (j < l2) finalists._sportsmen[k++] = group2._sportsmen[j++];

                }
                else if (group1._sportsmen == null && group2._sportsmen == null) return finalists;
                else if (group1._sportsmen == null)
                {
                    finalists.Add(group2._sportsmen);

                }
                else if (group2._sportsmen == null)
                {
                    finalists.Add(group1._sportsmen);

                }
                return finalists;
            }

            public void Print()
            {
                foreach (Sportsman x in _sportsmen) x.Print();
                Console.WriteLine();
            }
            public void Split(out Sportsman[] men, out Sportsman[] women)
            {
                men = new Sportsman[0];
                women = new Sportsman[0];
                foreach (var x in _sportsmen)
                {
                    if (x is SkiMan)
                    {
                        var copy = new Sportsman[men.Length + 1];
                        Array.Copy(men, copy, men.Length);
                        copy[copy.Length - 1] = x;
                        men = copy;
                    }
                    else if (x is SkiWoman)
                    {
                        var copy = new Sportsman[women.Length + 1];
                        Array.Copy(women, copy, women.Length);
                        copy[copy.Length - 1] = x;
                        women = copy;
                    }
                }
            }
            public void Shuffle()
            {
                Sort();
                Split(out Sportsman[] men, out Sportsman[] women);
                if (women.Length == 0)
                {
                    _sportsmen = men;
                    return;
                }
                if (men.Length == 0)
                {
                    _sportsmen = women;
                    return;
                }
                _sportsmen = new Sportsman[men.Length + women.Length];
                if (women[0].Time < men[0].Time) (women, men) = (men, women);
                int k = 0, m = 0, w = 0;
                while (m < men.Length && w < women.Length && k < _sportsmen.Length)
                {
                    if (k % 2 == 0)
                    {
                        _sportsmen[k++] = men[m++];
                    }
                    else
                    {
                        _sportsmen[k++] = women[w++];
                    }
                }
                while (m < men.Length) _sportsmen[k++] = men[m++];
                while (w < women.Length) _sportsmen[k++] = women[w++];

            }
        }
    }
}

