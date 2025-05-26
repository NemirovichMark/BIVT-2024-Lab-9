using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
namespace Lab_7
{
    public class Purple_4
    {
        public class Sportsman
        {
            //поля
            private string _name;
            private string _surname;
            private double _time;
            private bool _need;
            //свойства
            public string Name => _name;
            public string Surname => _surname;
            public double Time => _time;
            //конструктор
            public Sportsman(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _time = 0;
                _need = false;
            }
            //методы
            public void Run(double time)
            {
                if (_need) return;
                _time = time;
                _need = true;
            }

            public void Print()
            {
                System.Console.WriteLine($" {_name} {_surname} {_time}");
            }
            public static void Sort(Sportsman[] array)
            {
                if (array == null) return;
                var copy = array.OrderBy(x => x.Time).ToArray();
                Array.Copy(copy, array, copy.Length);
            }
        }
        public class Group
        {
            //поля
            private string _name;
            private Sportsman[] _sportsmen;
            //свойства
            public string Name => _name;
            public Sportsman[] Sportsmen => _sportsmen;

            //конструктор
            public Group(string name)
            {
                _name = name;
                _sportsmen = new Sportsman[0];
            }
            public Group(Group s)
            {
                _name = s.Name;
                if (s.Sportsmen != null)
                {
                    _sportsmen = new Sportsman[s.Sportsmen.Length];
                    Array.Copy(s.Sportsmen, _sportsmen, s.Sportsmen.Length);
                }
                else _sportsmen = new Sportsman[0];

            }


            public void Add(Sportsman sportm)
            {
                if (_sportsmen == null) return;
                var sport1 = new Sportsman[_sportsmen.Length + 1];
                Array.Copy(_sportsmen, sport1, _sportsmen.Length);
                sport1[sport1.Length - 1] = sportm;
                _sportsmen = sport1;
            }
            public void Add(Sportsman[] sportms)
            {
                if (_sportsmen == null || sportms == null) return;
                int l = _sportsmen.Length;
                var sport2 = new Sportsman[l + sportms.Length];
                Array.Copy(_sportsmen, sport2, l);
                Array.ConstrainedCopy(sportms, 0, sport2, l, sportms.Length);
                _sportsmen = sport2;
            }
            public void Add(Group s)
            {
                if (_sportsmen == null) return;
                Add(s.Sportsmen);

            }
            public void Sort()
            {
                if (_sportsmen == null) return;
                Array.Sort(_sportsmen, (x, y) =>
                {
                    if (x.Time < y.Time) return -1;
                    else if (x.Time > y.Time) return 1;
                    else return 0;
                });

            }

            public static Group Merge(Group group1, Group group2)
            {
                var group = new Group("Финалисты");
                var g1 = group1._sportsmen;
                var g2 = group2._sportsmen;
                if (group1._sportsmen == null || group2._sportsmen == null) return group;
                if (g1 == null) g1 = new Sportsman[0];
                if (g2 == null) g2 = new Sportsman[0];
                group._sportsmen = new Sportsman[g1.Length + g2.Length];
                int ind1 = 0, ind2 = 0, ind = 0;
                while (ind1 < g1.Length && ind2 < g2.Length)
                {
                    if (g1[ind1].Time <= g2[ind2].Time)
                    {
                        group._sportsmen[ind++] = g1[ind1++];
                    }
                    else
                    {
                        group._sportsmen[ind++] = g2[ind2++];
                    }
                }
                while (ind1 < g1.Length)
                {
                    group._sportsmen[ind++] = g1[ind1++];
                }
                while (ind2 < g2.Length)
                {
                    group._sportsmen[ind++] = g2[ind2++];
                }
                return group;
            }
            public void Print() { }

            public void Split(out Sportsman[] men, out Sportsman[] women)
            {
                if (_sportsmen == null) { men = null; women = null; }
                men = _sportsmen.Where(x => (x is SkiMan)).ToArray();
                women = _sportsmen.Where(x => (x is SkiWoman)).ToArray();
            }
            public void Shuffle()
            {
                if (_sportsmen == null) return;
                Sort();
                Split(out Sportsman[] men, out Sportsman[] women);
                if (men.Length == 0 || women.Length == 0) return;
                if (men.Length != 0 && women.Length != 0 && men[0].Time > women[0].Time) { (men, women) = (women, men); }
                int ind = 0;
                int i;
                for (i = 0; i < men.Length && i < women.Length; i++)
                {

                    _sportsmen[ind] = men[i];
                    ind++;
                    _sportsmen[ind] = women[i];
                    ind++;

                }

                for (int j = i; j < men.Length; j++) _sportsmen[ind++] = men[j];
                for (int j = i; j < women.Length; j++) _sportsmen[ind++] = women[j];
            }
        }
        public class SkiMan : Sportsman
        {
            public SkiMan(string name, string surname) : base(name, surname) { }
            public SkiMan(string name, string surname, int time) : base(name, surname)
            {
                Run(time);
            }

        }
        public class SkiWoman : Sportsman
        {
            public SkiWoman(string name, string surname) : base(name, surname) { }
            public SkiWoman(string name, string surname, int time) : base(name, surname) { Run(time); }

        }
    }
}