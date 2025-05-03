using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lab_7
{
    public class Purple_2
    {
        public struct Participant
        {
            private string _name;
            private string _surname;
            
            private int _distance;
            private int[] _marks;

            public string Name => _name;
            public string Surname => _surname;
            
            // [JsonProperty]
            public int Distance
            {
                get { return _distance; }
                private set { _distance = value; }
            }

            // [JsonProperty]
            public int Result
            {
                get;
                private set;
            }
            
            // [JsonProperty]
            public int[] Marks
            {
                get
                {
                    if (_marks == null) return null;
                    int[] marks = new int[_marks.Length];
                    Array.Copy(_marks, marks, _marks.Length);
                    return marks;
                }
                private set
                {
                    if (value == null) _marks = null;
                    else _marks = (int[])value.Clone();
                }
            }
            
            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _distance = 0;
                _marks = new int[5];
            }

            public void Jump(int distance, int[] marks, int target)
            {
                if (marks == null || _marks == null || distance < 0 || marks.Length != 5) return;

                _distance = distance;
                Array.Copy(marks, _marks, marks.Length);

                int points = 60 + (_distance - target) * 2;
                Result += points + marks.Sum() - marks.Max() - marks.Min();
                if (Result < 0) Result = 0;
            }

            public static void Sort(Participant[] array)
            {
                if (array == null || array.Length <= 1) return;
                Participant[] buffer = new Participant[array.Length];
                MergeSort(array, buffer, 0, array.Length - 1);
            }
            private static void MergeSort(Participant[] array, Participant[] buffer, int left, int right)
            {
                if (left < right)
                {
                    int mid = left + (right - left) / 2;
                    MergeSort(array, buffer, left, mid);
                    MergeSort(array, buffer, mid + 1, right);
                    Merge(array, buffer, left, mid, right);
                }
            }
            private static void Merge(Participant[] array, Participant[] buffer, int left, int mid, int right)
            {
                int i = left, j = mid + 1, k = left;
                while (i <= mid && j <= right)
                {
                    if (array[i].Result >= array[j].Result) buffer[k++] = array[i++];
                    else buffer[k++] = array[j++];
                }
                while (i <= mid) buffer[k++] = array[i++];
                while (j <= right) buffer[k++] = array[j++];
                for (i = left; i <= right; i++) array[i] = buffer[i];
            }
            
            public void Print()
            {
                Console.WriteLine($"Name: {_name}");
                Console.WriteLine($"Surname: {_surname}");
                Console.WriteLine($"Distance: {_distance}");
                Console.WriteLine("\nMarks:");
                foreach(double mark in _marks) Console.Write($"{mark}\t");
                Console.WriteLine();
                Console.WriteLine($"Result: {Result}\n");
            }
            
            /* public static void PrintTable(Participant[] array)
            {
                Console.WriteLine("Name\tSurname\tResult");
                foreach (Participant p in array) Console.WriteLine($"{p.Name}\t{p.Surname}\t{p.Result}");
            } */
        }

        public abstract class SkiJumping {
            private string _name;
            private int _standard;
            
            private Participant[] _participants;
            
            public string Name => _name;
            public int Standard => _standard;

            [JsonProperty]
            public Participant[] Participants
            {
                get
                {
                    if (_participants == null) return null;
                    Participant[] participants = new Participant[_participants.Length];
                    Array.Copy(_participants, participants, _participants.Length);
                    return participants;
                }
                private set
                {
                    if (value == null) _participants = null;
                    else _participants = (Participant[])value.Clone();
                }
            }
            
            public SkiJumping(string name, int standard) {
                _name = name;
                _standard = standard;
                _participants = new Participant[0];
            }
            
            public void Add(Participant participant)
            {
                if (_participants == null) return;
                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[_participants.Length - 1] = participant;
            }

            public void Add(Participant[] participants) {
                if (_participants == null || participants == null) return;
                foreach (Participant participant in participants) Add(participant);
            }
            
            public void Jump(int distance, int[] marks) {
                if (_participants == null) return;
                for (int i = 0; i < _participants.Length; i++) {
                    if (_participants[i].Marks.All(m => m == 0)) {
                        _participants[i].Jump(distance, marks, _standard);
                        break;
                    }
                }
            }

            public void Print()
            {
                Console.WriteLine($"Name: {_name}");
                Console.WriteLine($"Standard: {_standard}");
                Console.WriteLine("\nParticipants:");
                foreach (Participant p in _participants) p.Print();
            }
        }
        
        public class JuniorSkiJumping : SkiJumping {
            public JuniorSkiJumping() : base("100m", 100) {}
        }

        public class ProSkiJumping : SkiJumping {
            public ProSkiJumping() : base("150m", 150) {}
        }
    }
}