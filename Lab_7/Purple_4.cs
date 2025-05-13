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
            private bool _timeAlreadySet;

            public string Name => _name;
            public string Surname => _surname;
            public double Time => _time;

            public Sportsman(string name, string surname)
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

            }
            public static void Sort(Sportsman[] array)
            {
                if (array == null) return;
                Array.Copy(array.OrderBy(x => x.Time).ToArray(), array, array.Length);
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

            public string Name => _name;
            public Sportsman[] Sportsmen => _sportsmen;

            public Group(string name)
            {
                _name = name;
                _sportsmen = new Sportsman[0];
            }

            public Group(Group g)
            {
                _name = g.Name;
                if (g.Sportsmen != null)
                {
                    _sportsmen = new Sportsman[g.Sportsmen.Length];
                    Array.Copy(g.Sportsmen, _sportsmen, g.Sportsmen.Length);
                }
                else _sportsmen = new Sportsman[0];
            }

            public void Add(Sportsman sm)
            {
                if (_sportsmen == null) return;

                var sportsmen = new Sportsman[_sportsmen.Length + 1];
                Array.Copy(_sportsmen, sportsmen, _sportsmen.Length);
                sportsmen[sportsmen.Length - 1] = sm;
                _sportsmen = sportsmen;
            }

            public void Add(Sportsman[] sms)
            {
                if (_sportsmen == null || sms == null) return;
                var sportsmen = new Sportsman[_sportsmen.Length + sms.Length];
                Array.Copy(_sportsmen, sportsmen, _sportsmen.Length);
                Array.ConstrainedCopy(sms, 0, sportsmen, _sportsmen.Length, sms.Length);
                _sportsmen = sportsmen;
            }

            public void Add(Group g)
            {
                if (_sportsmen == null || g.Sportsmen == null) return;
                Add(g.Sportsmen);
            }

            public void Sort()
            {
                if (_sportsmen == null) return;

                Array.Sort(_sportsmen, (a, b) =>
                {
                    double x = a.Time - b.Time;
                    if (x < 0) return -1;
                    else if (x > 0) return 1;
                    else return 0;
                });
            }

            public static Group Merge(Group group1, Group group2)
            {
                var g = new Group("Финалисты");
                var s1 = group1._sportsmen;
                var s2 = group2._sportsmen;
                if (s1 == null) s1 = new Sportsman[0];
                if (s2 == null) s2 = new Sportsman[0];
                g._sportsmen = new Sportsman[s1.Length + s2.Length];
                int i1 = 0, i2 = 0, ind = 0;
                while (i1 < s1.Length && i2 < s2.Length)
                {
                    if (s1[i1].Time <= s2[i2].Time)
                    {
                        g._sportsmen[ind++] = s1[i1++];
                    }
                    else
                    {
                        g._sportsmen[ind++] = s2[i2++];
                    }
                }
                while (i1 < s1.Length)
                {
                    g._sportsmen[ind++] = s1[i1++];
                }
                while (i2 < s2.Length)
                {
                    g._sportsmen[ind++] = s2[i2++];
                }
                return g;
            }

            public void Print()
            {

            }

            public void Split(out Sportsman[] men, out Sportsman[] women)
            {
                men = null; women = null;
                if (_sportsmen == null) return;
                men = _sportsmen.Where(s => (s is SkiMan)).ToArray();
                women = _sportsmen.Where(s => (s is SkiWoman)).ToArray();
            }

            public void Shuffle()
            {
                if (_sportsmen == null) return;
                Sort();
                Sportsman[] men, women;
                Split(out men, out women);
                if (men.Length != 0 && women.Length != 0 && men[0].Time > women[0].Time)
                    (men, women) = (women, men);
                int ind = 0;
                int i;
                for (i = 0; i < men.Length && i < women.Length; i++)
                {
                    _sportsmen[ind++] = men[i];
                    _sportsmen[ind++] = women[i];
                }
                for (int j = i; j < men.Length; j++) _sportsmen[ind++] = men[j];
                for (int j = i; j < women.Length; j++) _sportsmen[ind++] = women[j];
            }
        }
    }
}
