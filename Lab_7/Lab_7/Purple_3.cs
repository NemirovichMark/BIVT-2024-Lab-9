using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lab_7
{
    public class Purple_3
    {
        public struct Participant
        {
            private string _name;
            private string _surname;
            private double[] _marks;
            private int[] _places;

            private int _markIndex;

            public string Name => _name;
            public string Surname => _surname;
            
            public int Score => (_places == null) ? 0 : _places.Sum();

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _marks = new double[7];
                _places = new int[7];
                _markIndex = 0;
            }

            // [JsonProperty]
            public double[] Marks
            {
                get
                {
                    if (_marks == null) return null;
                    double[] marks = new double[_marks.Length];
                    Array.Copy(_marks, marks, _marks.Length);
                    return marks;
                }
                private set
                {
                    if (value == null) _marks = null;
                    else _marks = (double[])value.Clone();
                }
            }

            // [JsonProperty]
            public int[] Places
            {
                get
                {
                    if (_places == null) return null;
                    int[] places = new int[_places.Length];
                    Array.Copy(_places, places, _places.Length);
                    return places;
                }
                private set
                {
                    if (value == null) _places = null;
                    else _places = (int[])value.Clone();
                }
            }

            public void Evaluate(double result)
            {
                if (_marks == null || _markIndex >= 7) return;
                _marks[_markIndex++] = result;
            }

            public static void SetPlaces(Participant[] participants)
            {
                if (participants == null || participants.Length == 0) return;
                Participant[] temp = participants.Where(p => p._marks != null && p._places != null).ToArray();
                int size = temp.Length;
                for (int i = 0; i < 7; ++i) // i-й судья
                {
                    temp = temp.OrderByDescending(p => p._marks[i]).ToArray();
                    for (int j = 0; j < size; ++j) temp[j]._places[i] = j + 1;
                }

                Array.Copy(temp.Concat(participants.Where(p => p._marks == null || p._places == null)).ToArray(),
                    participants, participants.Length);
            }

            // linq-реализация
            public static void Sort(Participant[] array)
            {
                if (array == null || array.Length <= 1) return;
                Participant[] buffer = new Participant[array.Length];
                buffer = array.Where(p => p._marks != null && p._places != null).OrderBy(p => p.Score)
                    .ThenBy(p => p._places.Min()).ThenByDescending(p => p._marks.Sum()).ToArray();
                Array.Copy(buffer.Concat(array.Where(p => p._marks == null || p._places == null)).ToArray(),
                    array, array.Length);
            }

            public void Print()
            {
                Console.WriteLine($"Name: {_name}");
                Console.WriteLine($"Surname: {_surname}");

                Console.WriteLine("Marks:");
                foreach (double mark in _marks) Console.Write($"{mark}\t");
                Console.WriteLine();

                Console.WriteLine("Places:");
                foreach (double mark in _marks) Console.Write($"{mark}\t");
                Console.WriteLine();

                Console.WriteLine($"Top place: {_places.Min()}");
                Console.WriteLine($"TotalMark: {Math.Round(_marks.Sum(), 2)}");
                Console.WriteLine($"Result: {Score}");
                Console.WriteLine();
            }

            /*public static void PrintTable(Participant[] array)
            {
                Console.WriteLine("Name\tSurname\tScore\tTopPlace\tTotalMark");
                foreach (Participant p in array) Console.WriteLine($"{p.Name}\t{p.Surname}\t{p.Score}\t{p.Places.Min()}\t{Math.Round(p.Marks.Sum(), 2)}");
            }*/
        }

        public abstract class Skating
        {
            private Participant[] _participants;
            protected double[] _moods;

            [JsonProperty]
            public Participant[] Participants
            {
                get { return _participants; }
                private set {_participants = value;}
            }
            public double[] Moods => _moods;

            public Skating(double[] moods, bool needModificate = true)
            {
                _participants = new Participant[0];
                if (moods == null) return;
                int size = moods.Length > 7 ? 7 : moods.Length;
                _moods = new double[size];
                Array.Copy(moods, _moods, size);
                if(needModificate) ModificateMood();
            }

            protected abstract void ModificateMood();

            public void Evaluate(double[] marks)
            {
                if (marks == null) return;
                int ind = Array.FindIndex(_participants, p => p.Marks != null && p.Marks.All(m => m == 0));
                if (ind == -1) return;
                int size = marks.Length >= 7 ? 7 : marks.Length;
                for (int i = 0; i < size; i++) _participants[ind].Evaluate(marks[i] * _moods[i]);
            }
            
            public void Add(Participant participant)
            {
                if (_participants == null) return;
                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[_participants.Length - 1] = participant;
            }

            public void Add(Participant[] participants) {
                if (_participants == null || participants == null) return;
                foreach (var participant in participants) Add(participant);
            }
        }
        
        public class FigureSkating : Skating
        {
            public FigureSkating(double[] moods, bool needModificate = true) : base(moods, needModificate) {}
            
            protected override void ModificateMood() {
                for (int i = 0; i < _moods.Length; i++) _moods[i] += (i + 1.0) / 10;
            }
        }
        
        public class IceSkating : Skating
        {
            public IceSkating(double[] moods, bool needModificate = true) : base(moods, needModificate) {}

            protected override void ModificateMood()
            {
                for (int i = 0; i < _moods.Length; i++) _moods[i] *= (i + 1.0 + 100) / 100;
            }
        }
    }
}