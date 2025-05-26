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

            public Sportsman(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _time = 0;
            }

            public string Name => _name;
            public string Surname => _surname;
            public double Time => _time;



            public void Run(double time)
            {
                if (time < 0) { return; }
                if (Time == 0) { _time = time; } else { return; }
            }

            public static void Sort(Sportsman[] array)
            {
                if (array == null) return;
                var sorted = array.OrderBy(p => p.Time).ToArray();
                Array.Copy(sorted, array, array.Length);

            }

            public void Print()
            {
                Console.WriteLine($"{_name,15} {_surname,15} {_time,15}");
            }
        }


        public class SkiMan : Sportsman
        {
            public SkiMan(string name, string surname) : base(name, surname) { }
            public SkiMan(string name, string surname, double time) : base(name, surname) { Run(time); }
        }

        public class SkiWoman : Sportsman
        {
            public SkiWoman(string name, string surname) : base(name, surname) { }
            public SkiWoman(string name, string surname, double time) : base(name, surname) { Run(time);    }
        }




        public class Group
        {
            private string _name;
            private Sportsman[] _sportsmen;

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
                    _sportsmen = null;
                }
                else
                {
                    _sportsmen = new Sportsman[group.Sportsmen.Length];
                    Array.Copy(group.Sportsmen, _sportsmen, group.Sportsmen.Length);
                }
            }


            public string Name => _name;
    
            public Sportsman[] Sportsmen
            {
                get
                {
                    return _sportsmen;
                }
            }



            public void Add(Sportsman sportsman)
            {
                if (_sportsmen == null) return;

                Array.Resize(ref _sportsmen, _sportsmen.Length + 1);
                _sportsmen[_sportsmen.Length - 1] = sportsman;
            }

            //public void Remove(int index)
            //{
            //    if (index < 0 || index >= _sportsmen.Length) return;
            //    Sportsman[] newSportsmen = new Sportsman[_sportsmen.Length - 1];
            //    for (int i = 0; i < index; i++)
            //    {
            //        newSportsmen[i] = _sportsmen[i];
            //    }
            //    for (int i = index; i < _sportsmen.Length - 1; i++)
            //    {
            //        newSportsmen[i] = _sportsmen[i+1];
            //    }

            //    _sportsmen = newSportsmen;

            //}


            public void Add(Sportsman[] sportsmen)
            {
                if (_sportsmen == null || sportsmen == null) return;
                int index = _sportsmen.Length;
                Array.Resize(ref _sportsmen, _sportsmen.Length + sportsmen.Length);
                for (int i = 0; i < sportsmen.Length; i++)
                {
                    _sportsmen[index + i] = sportsmen[i];
                }
            }

            public void Add(Group group)
            {
                if (_sportsmen == null || group.Sportsmen == null) return;
                Add(group.Sportsmen);
            }


            public void Sort()
            {
                if (_sportsmen == null || _sportsmen.Length == 0) return;

                for (int i = 1; i < _sportsmen.Length;)
                {
                    if (i == 0 || _sportsmen[i].Time >= _sportsmen[i-1].Time)
                    {
                        i++;
                    }
                    else
                    {
                        Sportsman temp = _sportsmen[i];
                        _sportsmen[i] = _sportsmen[i - 1];
                        _sportsmen[i-1] = temp;
                        i--;
                    }
                }
            }


            public static Group Merge(Group group1, Group group2)
            {
                Group group = new Group("Финалисты");
                if (group1._sportsmen == null && group2._sportsmen == null) return group;
                if (group1._sportsmen == null ) {
                    group._sportsmen = group2._sportsmen;
                    return group;
                }
                if (group2._sportsmen == null)
                {
                    group._sportsmen = group1._sportsmen;
                    return group;
                }


                Sportsman[] finalists = new Sportsman[group1.Sportsmen.Length + group2.Sportsmen.Length];

                int i = 0, j = 0, k = 0;
                while (i < group1.Sportsmen.Length && j < group2.Sportsmen.Length)
                {
                    if (group1.Sportsmen[i].Time <= group2.Sportsmen[j].Time)
                        finalists[k++] = group1.Sportsmen[i++];
                    else
                        finalists[k++] = group2.Sportsmen[j++];
                }

                while (i < group1.Sportsmen.Length) { 
                    finalists[k++] = group1.Sportsmen[i++];
                }

                while (j < group2.Sportsmen.Length) { 
                    finalists[k++] = group2.Sportsmen[j++];
                }

                group.Add(finalists);
                return group;
            }


            public void Split(out Sportsman[] men, out Sportsman[] women)
            {
                if (_sportsmen == null)
                {
                    men = null;
                    women = null;
                    return;
                }
                men = _sportsmen.Where(s => s != null && s is SkiMan).ToArray();
                women = _sportsmen.Where(s => s != null && s is SkiWoman).ToArray();
            }

            public void Shuffle()
            {
                if (_sportsmen == null) { return; }

                Sort();
                Split(out Sportsman[] men, out Sportsman[] women);

                if (men.Length == 0 || women.Length == 0) { return; }
                bool menFirst = false;

                if (men[0].Time < women[0].Time)
                {
                    menFirst = true;
                }


                int i = 0, j = 0, k = 0;

                while (i < men.Length && j < women.Length)
                {
                    if (menFirst && i == j) { _sportsmen[k++] = men[i++]; }
                    else if (i == j) { _sportsmen[k++] = women[j++]; }
                    else if (i < j) { _sportsmen[k++] = men[i++]; }
                    else { _sportsmen[k++] = women[j++]; }
                }

                while (i < men.Length)
                {
                    _sportsmen[k++] = men[i++];
                }

                while (j < women.Length)
                {
                    _sportsmen[k++] = women[j++];
                }
            }


            public void Print()
            {
                if (_sportsmen == null) return;

                Console.WriteLine($"Name: {_name}");
                Console.WriteLine($"Sportsmen:\n");
                foreach (Sportsman sportsman in _sportsmen)
                {
                    sportsman.Print();
                }
                Console.WriteLine();
            }


        }

    }
}