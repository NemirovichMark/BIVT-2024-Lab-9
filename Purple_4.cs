﻿using System;
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

            public string Name => _name;
            public string Surname => _surname;
            public double Time => _time;

            public Sportsman(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _time = 0.0;
            }

            public void Run(double time)
            {
                if (_time != 0.0) return;
                _time = time;
            }
            public static void Sort(Sportsman[] array)
            {
                if (array == null) return;

                var sortedSportsmen = array.OrderBy(s => s._time).ToArray();
                Array.Copy(sortedSportsmen, array, sortedSportsmen.Length);
            }

            public void Print()
            {
                Console.WriteLine(_name + " " + _surname);
                Console.WriteLine($"Time: {_time}");
                Console.WriteLine();
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
                if (group.Sportsmen == null)
                {
                    _sportsmen = new Sportsman[0];
                    return;
                }
                _sportsmen = new Sportsman[group.Sportsmen.Length];
                Array.Copy(group.Sportsmen, _sportsmen, group.Sportsmen.Length);
            }

            public void Add(Sportsman newSportsman)
            {
                if (_sportsmen == null) return;
                Array.Resize(ref _sportsmen, _sportsmen.Length + 1);
                _sportsmen[_sportsmen.Length - 1] = newSportsman;
            }

            public void Add(Sportsman[] newSportsmen)
            {
                if (newSportsmen == null || _sportsmen == null) return;
                int oldLength = _sportsmen.Length;  
                Array.Resize(ref _sportsmen, _sportsmen.Length + newSportsmen.Length);
                Array.Copy(newSportsmen, 0, _sportsmen, oldLength, newSportsmen.Length);
            }

            public void Add(Group group)
            {
                Add(group.Sportsmen);
            }

            public void Sort()
            {
                if (_sportsmen == null) return;

                var sortedSportsmen = _sportsmen.OrderBy(x => x.Time).ToArray();
                Array.Copy(sortedSportsmen, _sportsmen, _sportsmen.Length);
            }

            public static Group Merge(Group group1, Group group2)
            {
                if (group1.Sportsmen == null || group2.Sportsmen == null) return default(Group);
                Group finalists = new Group("Финалисты");

                group1.Sort();
                group2.Sort();

                int i = 0, j = 0;
                while (i < group1.Sportsmen.Length && j < group2.Sportsmen.Length)
                {
                    if (group1.Sportsmen[i].Time <= group2.Sportsmen[j].Time)
                        finalists.Add(group1.Sportsmen[i++]);

                    else
                        finalists.Add(group2.Sportsmen[j++]);
                }

                while (i < group1.Sportsmen.Length)
                    finalists.Add(group1.Sportsmen[i++]);

                while (j < group2.Sportsmen.Length)
                    finalists.Add(group2.Sportsmen[j++]);

                return finalists;
            }
            public void Split(out Sportsman[] men, out Sportsman[] women)
            {
                if (_sportsmen == null)
                {
                    men = null;
                    women = null;
                    return;
                }

                men = _sportsmen.Where(s => s is SkiMan).ToArray();
                women = _sportsmen.Where(s => s is SkiWoman).ToArray();
            }

            public void Shuffle()
            {
                Sort(); 
                Sportsman[] men, women;
                Split(out men, out women);  

                if (men.Length == 0 || men == null || women == null || women.Length == 0) 
                {
                    return;
                }

                int pair = Math.Min(men.Length, women.Length);
                int diff = men.Length - women.Length;

                int i = 0, w = 0, m = 0;

                if (men[0].Time < women[0].Time)
                {
                    while (i < pair * 2)
                    {
                        _sportsmen[i++] = men[m++];
                        _sportsmen[i++] = women[w++];
                    }
                }
                else
                {
                    while (i < pair * 2)
                    {
                        _sportsmen[i++] = women[w++];
                        _sportsmen[i++] = men[m++];
                    }
                }

                if (diff > 0 && m < men.Length) 
                {
                    _sportsmen[i++] = men[w++];
                }
                else if (diff < 0 && w < women.Length) 
                {
                    _sportsmen[i++] = women[w++];
                }
            }
            public void Print()
            {
                Console.WriteLine($"Group name: {_name}");
                foreach (Sportsman sportsman in _sportsmen)
                {
                    sportsman.Print();
                }
            }
        }
    }
}