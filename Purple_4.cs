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
            private string _name, _surname;
            private double _time;
            private int _att; // only 1 time

            public string Name => _name;
            public string Surname => _surname;
            public double Time => _time;
            public Sportsman(string name, string surname)
            {
                _name = name; _surname = surname;
                _time = 0;
                _att = 0;
            }
            public void Run(double time)
            {
                if (time < 0) return;
                if (_att == 0) _time = time; _att++;
            }
            public static void Sort(Sportsman[] array)
            {
                if (array == null) return;
                Sportsman[] helping = array.OrderBy(time => time.Time).ToArray();
                Array.Copy(helping, 0, array, 0, helping.Length);
            }
            public void Print()
            {

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
            public Sportsman[] Sportsmen
            {
                get
                {
                    if (_sportsmen == null) return default;

                    // Sportsman[] copy = new Sportsman[_sportsmen.Length];
                    //Array.Copy(_sportsmen, copy, _sportsmen.Length);

                    return _sportsmen;
                }
            }
            public Group(string naming)
            {
                _name = naming;
                _sportsmen = new Sportsman[0];
            }
            public Group(Group elem)
            {
                _name = elem.Name;
                if (elem.Sportsmen == null)
                {
                    _sportsmen = new Sportsman[0];
                }
                else
                {
                    _sportsmen = new Sportsman[elem.Sportsmen.Length];
                    Array.Copy(elem.Sportsmen, _sportsmen, elem.Sportsmen.Length);
                }

            }
            public void Split(out Sportsman[] men, out Sportsman[] women)
            {
                if (_sportsmen == null)
                {
                    men = null; women = null;
                    return;
                }
                men = new Sportsman[0]; int manind = 0;
                women = new Sportsman[0]; int womanind = 0;

                foreach (var spperson in _sportsmen)
                {
                    if (spperson is SkiMan)
                    {
                        manind++;
                    }
                    else if (spperson is SkiWoman)
                    {
                        womanind++;
                    }
                }

                Array.Resize(ref men, manind); int counterman = 0;
                Array.Resize(ref women, womanind); int counterwomen = 0;

                foreach (var spperson in _sportsmen)
                {
                    if (spperson is SkiMan)
                    {
                        men[counterman] = spperson; counterman++;
                    }
                    else if (spperson is SkiWoman)
                    {
                        women[counterwomen] = spperson; counterwomen++;
                    }
                }
            }
            public void Shuffle()
            {
                if (_sportsmen == null) return;
                Sportsman[] men;
                Sportsman[] women;
                Split(out men, out women);

                if (men.Length == 0 || women.Length == 0) return;
                Sportsman.Sort(men);
                Sportsman.Sort(women);


                bool man_or_woman = men[0].Time <= women[0].Time;
                Sportsman[] result = new Sportsman[men.Length + women.Length];


                int i = 0, jm = 0, jwm = 0;

                if (man_or_woman == true)
                {
                    while (jm < men.Length && jwm < women.Length)
                    {
                        result[i++] = men[jm++];
                        result[i++] = women[jwm++];
                    }
                }
                else
                {
                    while (jm < men.Length && jwm < women.Length)
                    {
                        result[i++] = women[jwm++];
                        result[i++] = men[jm++];

                    }
                }
                while (jm < men.Length)
                {
                    result[i++] = men[jm++];
                }
                while (jwm < women.Length)
                {
                    result[i++] = women[jwm++];
                }

                _sportsmen = result;
            }
            public void Add(Sportsman one_sportsman)
            {
                if (_sportsmen == null) return;

                Sportsman[] copy = new Sportsman[_sportsmen.Length + 1];
                Array.Copy(_sportsmen, copy, _sportsmen.Length);
                copy[copy.Length - 1] = one_sportsman;
                _sportsmen = copy;
            }
            public void Add(Sportsman[] SPORTSmenGroup)
            {
                if (_sportsmen == null || SPORTSmenGroup == null) return;

                Sportsman[] copy = new Sportsman[_sportsmen.Length + SPORTSmenGroup.Length];

                Array.Copy(_sportsmen, copy, _sportsmen.Length);

                Array.ConstrainedCopy(SPORTSmenGroup, 0, copy, _sportsmen.Length, SPORTSmenGroup.Length);

                _sportsmen = copy;
            }
            public void Add(Group elem)
            {
                if (elem.Sportsmen == null || _sportsmen == null) return;
                Add(elem.Sportsmen);
            }

            public void Sort()
            {
                if (_sportsmen == null) return;

                Array.Sort(_sportsmen, (a, b) => {
                    if (a.Time - b.Time > 0) return 1;
                    else if (a.Time - b.Time < 0) return -1;
                    else return 0;
                });
            }
            public static Group Merge(Group group1, Group group2)
            {
                Group copy = new Group("Финалисты");

                Sportsman[] grp1 = group1._sportsmen, grp2 = group2._sportsmen;

                if (grp1 == null) grp1 = new Sportsman[0] { }; if (grp2 == null) grp2 = new Sportsman[0] { };

                int g1 = grp1.Length; int g2 = grp2.Length;
                copy._sportsmen = new Sportsman[g1 + g2];

                int l = 0, k = 0, index_massive = 0;
                while (l < g1 && k < g2)
                {
                    if (grp1[l].Time <= grp2[k].Time)
                    {
                        copy._sportsmen[index_massive++] = grp1[l++];
                    }
                    else
                    {
                        copy._sportsmen[index_massive++] = grp2[k++];
                    }
                }
                while (l < g1)
                {
                    copy._sportsmen[index_massive++] = grp1[l++];
                }
                while (k < g2)
                {
                    copy._sportsmen[index_massive++] = grp2[k++];
                }

                return copy;



            }
            public void Print() { }
        }
    }
}
