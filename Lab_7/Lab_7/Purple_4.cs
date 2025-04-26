using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
            
            [JsonProperty]
            public double Time
            {
                get { return _time; }
                private set {_time = value;}
            }

            public Sportsman(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _time = 0;
            }

            public void Run(double time)
            {
                if (_time != 0) return;
                _time = time;
            }

            public static void Sort(Sportsman[] array)
            {
                if (array == null) return;
                Array.Copy(array.OrderBy(s => s._time).ToArray(), array, array.Length);
            }
            
            public void Print()
            {
                Console.WriteLine($"Name: {_name}");
                Console.WriteLine($"Surname: {_surname}");
                Console.WriteLine($"Time: {_time}\n");
            }
        }

        public class SkiMan : Sportsman
        {
            public SkiMan(string name, string surname) : base(name, surname) {}
            public SkiMan(string name, string surname, int time) : base(name, surname) => Run(time);
        }

        public class SkiWoman : Sportsman
        {
            public SkiWoman(string name, string surname) : base(name, surname) {}
            public SkiWoman(string name, string surname, int time) : base(name, surname) => Run(time);
        }
        
        public class Group
        {
            private string _name;
            private Sportsman[] _sportsmen;

            public string Name => _name;
            
            [JsonProperty]
            public Sportsman[] Sportsmen
            {
                get { return _sportsmen; }
                private set {_sportsmen = value; }
            }

            
            [JsonConstructor]
            public Group(string name)
            {
                _name = name;
                _sportsmen = new Sportsman[0];
            }
            
            public Group(Group group)
            {
                if (group._sportsmen == null) return;

                _name = group.Name;
                _sportsmen = new Sportsman[group.Sportsmen.Length];
                Array.Copy(group.Sportsmen, _sportsmen, group.Sportsmen.Length);
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
                Add(group.Sportsmen);
            }

            public void Sort()
            {
                if (_sportsmen == null) return;
                Array.Copy(_sportsmen.OrderBy(s => s.Time).ToArray(), _sportsmen, _sportsmen.Length);
            }

            public static Group Merge(Group x, Group y)
            {
                Group group = new Group("Финалисты");
                if (x._sportsmen == null && y._sportsmen == null) return group;
                if (x._sportsmen == null) {
                    // Array.Copy(y._sportsmen, group._sportsmen, y._sportsmen.Length);
                    group._sportsmen = y._sportsmen;
                    return group;
                }
                if (y._sportsmen == null) {
                    // Array.Copy(x._sportsmen, group._sportsmen, x._sportsmen.Length);
                    group._sportsmen = x._sportsmen;
                    return group;
                }

                Array.Resize(ref group._sportsmen, x._sportsmen.Length + y._sportsmen.Length);
                int i = 0, j = 0, k = 0;
                while (i != x._sportsmen.Length && j != y._sportsmen.Length) {
                    if (x._sportsmen[i].Time <= y._sportsmen[j].Time) group._sportsmen[k++] = x._sportsmen[i++];
                    else group._sportsmen[k++] = y._sportsmen[j++];
                }

                while (i < x._sportsmen.Length) group._sportsmen[k++] = x._sportsmen[i++];
                while (j < y._sportsmen.Length) group._sportsmen[k++] = y._sportsmen[j++];

                return group;
            }

            public void Split(out Sportsman[] men, out Sportsman[] women)
            {
                if (_sportsmen == null)
                {
                    men = null;
                    women = null;
                }
                men = _sportsmen.Where(s => s is SkiMan).ToArray();
                women = _sportsmen.Where(s => s is SkiWoman).ToArray();
            }

            public void Shuffle()
            {
                if (_sportsmen == null) return;
                
                Sort();
                Split(out Sportsman[] men, out Sportsman[] women);

                if (men.Length == 0 || women.Length == 0) return;

                bool startsWithMan = men[0].Time <= women[0].Time;
                int i = 0, j = 0, k = 0;
                
                while (i < men.Length && j < women.Length) {
                    if (startsWithMan && i == j) _sportsmen[k++] = men[i++];
                    else if (i == j) _sportsmen[k++] = women[j++];
                    else if (i < j) _sportsmen[k++] = men[i++];
                    else _sportsmen[k++] = women[j++];
                }
                
                while (i < men.Length) _sportsmen[k++] = men[i++];
                while (j < women.Length) _sportsmen[k++] = women[j++];
            }
            
            public void Print()
            {
                if (_sportsmen == null) return;

                Console.WriteLine($"Name: {_name}");
                Console.WriteLine($"Sportsmen:\n");
                foreach (Sportsman sportsman in _sportsmen) sportsman.Print();
                Console.WriteLine();
            }

            // public static void PrintTable(Group group)
            // {
            //     Console.WriteLine("Name\tSurname\tTime");
            //     foreach (Sportsman sportsman in group.Sportsmen) Console.WriteLine($"{sportsman.Name}\t{sportsman.Surname}\t{sportsman.Time}");
            // }
        }
    }
}