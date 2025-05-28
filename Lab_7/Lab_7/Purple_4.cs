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
            private bool _timeSet;

            public string Name => _name;
            public string Surname => _surname;
            public double Time => _time;

            public Sportsman(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _time = 0;
                _timeSet = false;
            }

            public void Run(double time)
            {
                if (_timeSet) return;

                _time = time;
                _timeSet = true;
            }

            public void Print()
            {
                Console.WriteLine($"{Name} {Surname} - Итоговое время: {Time}");
            }

            public static void Sort(Sportsman[] array)
            {
                int i = 1, j = 2; 

                while (i < array.Length)
                {
                    if (i == 0 || array[i].Time >= array[i - 1].Time)
                    {
                        i = j;
                        j++;
                    }

                    else
                    {
                        Sportsman temp = array[i];
                        array[i] = array[i - 1];
                        array[i - 1] = temp;
                        i--; 
                    }
                }
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

                if (group.Sportsmen != null)
                {
                    _sportsmen = new Sportsman[group.Sportsmen.Length];
                    Array.Copy(group.Sportsmen, _sportsmen, group.Sportsmen.Length);
                }
                else
                {
                    _sportsmen = new Sportsman[0];
                }
            }

            public void Add(Sportsman newSportsman)
            {
                if (_sportsmen == null) return;

                var sportsmen = new Sportsman[_sportsmen.Length + 1];
                Array.Copy(_sportsmen, sportsmen, _sportsmen.Length);
                sportsmen[sportsmen.Length - 1] = newSportsman;

                _sportsmen = sportsmen;
            }

            public void Add(Sportsman[] newSportsmen)
            {
                if (_sportsmen == null || newSportsmen == null) return;

                var sportsmen = new Sportsman[_sportsmen.Length + newSportsmen.Length];
                Array.Copy(_sportsmen, sportsmen, _sportsmen.Length);
                Array.Copy(newSportsmen, 0, sportsmen, _sportsmen.Length, newSportsmen.Length);

                _sportsmen = sportsmen;
            }

            public void Add(Group otherGroup)
            {
                if (_sportsmen == null || otherGroup.Sportsmen == null) return;

                Add(otherGroup.Sportsmen);
            }

            //практика защиты
            private void RemoveLast()
            {
                if (_sportsmen == null || _sportsmen.Length == 0) return;

                var sportsmen = new Sportsman[_sportsmen.Length - 1];
                Array.Copy(_sportsmen, sportsmen, _sportsmen.Length - 1);
                _sportsmen = sportsmen;
            }

            private void RemoveLast(int count)
            {
                if (_sportsmen == null || _sportsmen.Length == 0 || count <= 0) return;

                int newLength = _sportsmen.Length - count;
                if (newLength < 0) newLength = 0;

                var sportsmen = new Sportsman[newLength];
                Array.Copy(_sportsmen, sportsmen, newLength);
                _sportsmen = sportsmen;
            }


            public void Sort()
            {
                if (_sportsmen == null || _sportsmen.Length == 0) return;

                int index = 0;

                while (index < _sportsmen.Length)
                {
                    if (index == 0 || _sportsmen[index - 1].Time <= _sportsmen[index].Time)
                    {
                        index++;
                    }
                    else
                    {
                        Sportsman temp = _sportsmen[index];
                        _sportsmen[index] = _sportsmen[index - 1];
                        _sportsmen[index - 1] = temp;
                        index--;
                    }
                }
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
                if (Sportsmen == null)
                {
                    men = null;
                    women = null;
                    return;
                }
                
                int menCount = Sportsmen.Count(s => s is SkiMan);
                int womenCount = Sportsmen.Count(s => s is SkiWoman);

                men = new Sportsman[menCount];
                women = new Sportsman[womenCount];

                int m = 0, w = 0;

                foreach (var s in Sportsmen)
                {
                    if (s is SkiMan)
                    {
                        men[m++] = s;
                    }
                    else if (s is SkiWoman)
                    {
                        women[w++] = s;
                    }
                }
            }
            public void Shuffle()
            {
                Sort();
                Sportsman[] men, women;
                Split(out men, out women);

                if (men == null || women == null || men.Length == 0 || women.Length == 0)
                {
                    return;
                }

                int matching = Math.Min(men.Length, women.Length);
                int remaining = men.Length - women.Length;

                int i = 0, w = 0, m = 0;

                if (men[0].Time < women[0].Time)
                {
                    while (i < matching * 2)
                    {
                        _sportsmen[i++] = men[m++];
                        _sportsmen[i++] = women[w++];
                    }
                }

                else
                {
                    while (i < matching * 2)
                    {
                        _sportsmen[i++] = women[w++];
                        _sportsmen[i++] = men[m++];
                    }
                }

                while (m < men.Length)
                {
                    _sportsmen[i++] = men[m++];
                }

                while (w < women.Length)
                {
                    _sportsmen[i++] = women[w++];
                }
            }
            public void Print()
            {
                Console.WriteLine($"Группа: {Name}");

                foreach (var sportsman in _sportsmen)
                {
                    if (sportsman.Time > 0)
                    {
                        sportsman.Print();
                    }
                }
            }
        }
    }
}
