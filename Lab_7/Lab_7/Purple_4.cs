using System;
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
                _time = 0;
            }

            public void Run(double time)
            {
                if (time < 0 || _time != 0) return;

                _time = time;
            }

            public static void Sort(Sportsman[] array)
            {
                if (array == null) return;

                var sortedArray = array.OrderBy(x => x._time).ToArray();
                Array.Copy(sortedArray, array, sortedArray.Length);
            }

            public void Print()
            {
                Console.WriteLine($"Имя: {_name ?? "не задано"}");
                Console.WriteLine($"Фамилия: {_surname ?? "не задана"}");
                Console.WriteLine($"Время: {_time}");
                Console.WriteLine();
            }
        }

        public class SkiMan : Sportsman
        {
            public SkiMan(string name, string surname) : base(name, surname)
            {
            }

            public SkiMan(string name, string surname, int time) : base(name, surname)
            {
                Run(time);
            }
        }

        public class SkiWoman : Sportsman
        {
            public SkiWoman(string name, string surname) : base(name, surname)
            {
            }

            public SkiWoman(string name, string surname, int time) : base(name, surname)
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
                if (group._sportsmen == null) return;

                _name = group._name;
                _sportsmen = (Sportsman[])group._sportsmen.Clone();
            }

            public void Add(Sportsman sportsman)
            {
                if (_sportsmen == null) return;

                Array.Resize(ref _sportsmen, _sportsmen.Length + 1);
                _sportsmen[_sportsmen.Length - 1] = sportsman;
            }

            public void Add(Sportsman[] sportsmen)
            {
                if (sportsmen == null || _sportsmen == null) return;

                int n = _sportsmen.Length;

                Array.Resize(ref _sportsmen, n + sportsmen.Length);
                Array.Copy(sportsmen, 0, _sportsmen, n, sportsmen.Length);
            }

            public void Add(Group group)
            {
                if (_sportsmen == null) return;
                Add(group._sportsmen);
            }

            public void Sort()
            {
                if (_sportsmen == null) return;

                var sortedSportsmen = _sportsmen.OrderBy(x => x.Time).ToArray();
                Array.Copy(sortedSportsmen, _sportsmen, sortedSportsmen.Length);
            }

            public static Group Merge(Group group1, Group group2)
            {
                var group = new Group("Финалисты");
                if (group1._sportsmen == null && group2._sportsmen == null) return group;

                if (group1._sportsmen == null)
                {
                    group._sportsmen = (Sportsman[])group2._sportsmen.Clone();
                    return group;
                }

                if (group2._sportsmen == null)
                {
                    group._sportsmen = (Sportsman[])group1._sportsmen.Clone();
                    return group;
                }

                Array.Resize(ref group._sportsmen, group1._sportsmen.Length + group2._sportsmen.Length);
                int i = 0, j = 0, k = 0;
                while (i != group1._sportsmen.Length && j != group2._sportsmen.Length)
                {
                    if (group1._sportsmen[i].Time < group2._sportsmen[j].Time)
                    {
                        group._sportsmen[k++] = group1._sportsmen[i++];
                    }
                    else
                    {
                        group._sportsmen[k++] = group2._sportsmen[j++];
                    }
                }

                while (i < group1._sportsmen.Length)
                {
                    group._sportsmen[k++] = group1._sportsmen[i++];
                }

                while (j < group2._sportsmen.Length)
                {
                    group._sportsmen[k++] = group2._sportsmen[j++];
                }

                return group;
            }

            public void Split(out Sportsman[] men, out Sportsman[] women)
            {
                men = null;
                women = null;
                if (_sportsmen == null) return;

                men = _sportsmen.Where(x => x is SkiMan).ToArray();
                women = _sportsmen.Where(x => x is SkiWoman).ToArray();
            }

            public void Shuffle()
            {
                if (_sportsmen == null) return;

                Sort();
                var men = new Sportsman[0];
                var women = new Sportsman[0];

                Split(out men, out women);

                if (men.Length == 0 || women.Length == 0) return;

                int i = 0, j = 0, k = 0;
                bool menFirst = men[0].Time <= women[0].Time;
                while (i < men.Length && j < women.Length)
                {
                    if (i == j && menFirst) _sportsmen[k++] = men[i++];
                    else if (i == j && !menFirst) _sportsmen[k++] = women[j++];
                    else if (i < j) _sportsmen[k++] = men[i++];
                    else if (j < i) _sportsmen[k++] = women[j++];
                }

                while (i < men.Length) _sportsmen[k++] = men[i++];
                while (j < women.Length) _sportsmen[k++] = women[j++];
            }

            public void Print()
            {
                Console.WriteLine($"Имя: {_name ?? "не задано"}");

                Console.WriteLine($"Спортсмены:");
                if (_sportsmen == null) Console.WriteLine("Не заданы");
                else
                {
                    Console.WriteLine();
                    foreach (var sportsman in _sportsmen)
                    {
                        sportsman.Print();
                    }
                }

                Console.WriteLine();
            }
        }
    }
}