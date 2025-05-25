using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
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

            public string Name => _name;
            public string Surname => _surname;
            public double Time => _time;

            public Sportsman(string name, string surname)
            {
                _name = name;
                _surname = surname;
            }
            public void Run(double time)
            {
                _time = time;
            }


            public void Print()
            {
                Console.WriteLine($"{_printItem(Name)} {_printItem(Surname)} {_printItem(Time.ToString())}");
            }
            private static string _printItem(string item)
            {
                return item + new string(' ', 15 - item.Length);
            }
            public static void Sort(Sportsman[] array)
            {
                if (array == null || array.Length == 0) return;
                Sportsman[] sortedArray = array.Where(s => s != null).Where(s => s.Time != 0).OrderBy(s => s.Time).ToArray(); // Нужны ли Where
                Array.Resize(ref array, sortedArray.Length);
                Array.Copy(sortedArray, array, sortedArray.Length);
                //Console.WriteLine(array.Length);
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
            public Group(Group group)
            {
                _name = group.Name;

                _sportsmen = new Sportsman[group.Sportsmen.Length];
                Array.Copy(group.Sportsmen, _sportsmen, group.Sportsmen.Length);

            }
            public void Add(Sportsman sportsman)
            {
                if (sportsman == null) return;

                Array.Resize(ref _sportsmen, _sportsmen.Length + 1);
                _sportsmen[_sportsmen.Length - 1] = sportsman;
            }
            public void Add(Sportsman[] sportsmen)
            {
                if (sportsmen == null) return;

                _sportsmen = (Sportsman[])_sportsmen.Concat(sportsmen.Where(s => s != null)).ToArray().Clone();
            }
            public void Add(Group group)
            {
                Add(group.Sportsmen);
            }

            public void Sort()
            {
                _sportsmen = _sportsmen.Where(s => s.Time != 0).OrderBy(x => x.Time).ToArray();
            }
            public static Group Merge(Group group1, Group group2)
            {
                Group group = new Group("Финалисты");
                group1.Sort();
                group2.Sort();

                int n1 = group1.Sportsmen.Length, n2 = group2.Sportsmen.Length;
                int i = 0, j = 0;
                while (i < n1 && j < n2)
                {
                    if (group1.Sportsmen[i].Time < group2.Sportsmen[j].Time)
                    {
                        group.Add(group1.Sportsmen[i]);
                        i++;
                    }
                    else
                    {
                        group.Add(group2.Sportsmen[j]);
                        j++;
                    }
                }
                while (i < n1)
                {
                    group.Add(group1.Sportsmen[i]);
                    i++;
                }
                while (j < n2)
                {
                    group.Add(group2.Sportsmen[j]);
                    j++;
                }
                return group;
            }
            public void Split(out Sportsman[] men, out Sportsman[] women)
            {
                men = _sportsmen.Where(s => s is SkiMan).ToArray();
                women = _sportsmen.Where(s => s is SkiWoman).ToArray();
            }
            public void Shuffle()
            {
                Sort();
                Sportsman[] men;
                Sportsman[] women;

                Split(out men, out women);

                int menLength = men.Length;
                int womenLength = women.Length;
                bool manFirst = men[0].Time <= women[0].Time;
                
                int i = 0, j = 0;
                while (i < Math.Min(menLength, womenLength))
                {
                    if (manFirst)
                    {
                        _sportsmen[2 * i] = men[i];
                        _sportsmen[2 * i + 1] = women[i];
                    } else
                    {
                        _sportsmen[2 * i] = women[i];
                        _sportsmen[2 * i + 1] = men[i];
                    }
                    i++;
                }
                while (i + j < menLength)
                {
                    _sportsmen[2 * i + j] = men[i + j];
                    j++;
                }
                while (i + j < womenLength)
                {
                    _sportsmen[2 * i + j] = women[i + j];
                    j++;
                }
            }
            private static void _printHead()
            {
                Console.WriteLine(_printItem("Name") + _printItem("Surname") + _printItem("Time"));
            }
            public void Print()
            {
                Console.Write(new string(' ', 15));
                Console.WriteLine(_printItem(Name));
                _printHead();
                for (int i = 0; i < _sportsmen.Length; i++)
                {
                    _sportsmen[i].Print();
                }
            }
            private static string _printItem(string item)
            {
                return item + new string(' ', 15 - item.Length);
            }
        }
    }
}
