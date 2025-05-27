using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using static Lab_7.Purple_4;

namespace Lab_7
{
    public class Purple_4
    {
        public class Sportsman
        {
            private string _name;
            private string _surname;
            private double _time;
            private bool _timeAlreadySet;
            public string Name
            {
                get
                {
                    return _name;
                }
            }
            public string Surname
            {
                get
                {
                    return _surname;
                }
            }
            public double Time
            {
                get
                {

                    return _time;
                }
            }

            public Sportsman(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _time = 0;
                _timeAlreadySet = false;
            }

            public Sportsman(string name, string surname, double time)
            {
                _name = name;
                _surname = surname;
                _time = 0;
                _timeAlreadySet = false;
            }
            public void Run(double time)
            {
                if (_timeAlreadySet) return;
                _time = time;
                _timeAlreadySet = true;
            }
            public void Print()
            {
                Console.WriteLine($"Name: {Name}, Surname: {Surname}, Time: {Time}");
            }

            public static void Sort(Sportsman[] array)
            {
                if (array == null) return;
                Array.Copy(array.OrderBy(s => s.Time).ToArray(), array, array.Length);
            }
        }
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
            public string Name
            {
                get
                {
                    return _name;
                }
            }
            public Sportsman[] Sportsmen
            {
                get
                {
                    return _sportsmen;
                }
            }

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
                    Array.Copy(group.Sportsmen, _sportsmen, group.Sportsmen.Length);
                }
                else _sportsmen = new Sportsman[0];
            }
            public void Add(Sportsman sportsman)
            {
                if (_sportsmen == null) return;
                _sportsmen = _sportsmen.Append(sportsman).ToArray();

            }
            public void Add(Sportsman[] sportsmen)
            {
                if (_sportsmen == null || sportsmen == null) return;
                _sportsmen = _sportsmen.Concat(sportsmen).ToArray();

            }
            public void Add(Group group)
            {
                if (_sportsmen == null || group.Sportsmen == null) return;
                Add(group.Sportsmen);
            }
            public void Sort()
            {
                if (_sportsmen == null) return;
                Array.Sort(_sportsmen, (x, y) => { if (x.Time < y.Time) return -1; else if (x.Time > y.Time) return 1; else return 0; });

            }
            public static Group Merge(Group group1, Group group2)
            {
                Group finalGroup = new Group("Финалисты");

                finalGroup.Add(group1);
                finalGroup.Add(group2);

                finalGroup.Sort();

                return finalGroup;

            }
            public void Print()
            {
                for (int i = 0; i < _sportsmen.Length; i++)
                {
                    _sportsmen[i].Print();
                }

                Console.WriteLine($"Name: {Name}.");
            }

            public void Split(out Sportsman[] men, out Sportsman[] women)
            {
                men = null;
                women = null;
                if (_sportsmen == null) return;
                men = _sportsmen.Where(s => (s is SkiMan)).ToArray();
                women = _sportsmen.Where(s => (s is SkiWoman)).ToArray();
            }

            public void Shuffle()
            {
                if (_sportsmen == null) return;
                //int m=0, w=0;
                Sort();
                //foreach (Sportsman x in _sportsmen) 
                //{
                //    if (x is SkiMan) m++;
                //    if (x is SkiWoman) w++;
                //}
                //var copy= new Sportsman[m];
                //int z = 0;
                //foreach (Sportsman x in _sportsmen)
                //{
                //    if (x is SkiMan)
                //    {
                //        copy[z] = x;
                //        z++;

                //    }
                //}
                //var copy2 = new Sportsman[w];
                //z = 0;
                //foreach (Sportsman x in _sportsmen)
                //{
                //    if (x is SkiWoman)
                //    {
                //        copy2[z] = x;
                //        z++;

                //    }
                //}

                Sportsman[] men, women;
                Split(out men, out women);
                if (men == null && women == null) return;
                if (men[0].Time > women[0].Time) (men, women) = (women, men);

                int z = 0;
                int k = 0;
                for (int i = 0; i < men.Length && i < women.Length; i++)
                {
                    _sportsmen[z++] = men[i];
                    _sportsmen[z++] = women[i];
                    k++;
                }

                for (int j = k; j < men.Length; j++)
                {
                    _sportsmen[z++] = men[j];
                }
                for (int j = k; j < women.Length; j++)
                {
                    _sportsmen[z++] = women[j];
                }
            }
        }
    }
}
