using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lab_7
{
    public class Purple_1
    {
        public class Participant
        {
            private string _name;
            private string _surname;
            
            
            private double[] _coefs;
            private int[,] _marks;
            
            private int _jumpIndex;
            
            public string Name => _name;
            public string Surname => _surname;
            
            public double TotalScore { get; private set; }
            
            [JsonProperty]
            public double[] Coefs
            {
                get
                {
                    if (_coefs == null) return null;
                    double[] coefs = new double[_coefs.Length];
                    Array.Copy(_coefs, coefs, _coefs.Length);
                    return coefs;
                }
                private set
                {
                    if (value == null) _coefs = null;
                    else _coefs = (double[])value.Clone();
                }
            }
            
            [JsonProperty]
            public int[,] Marks
            {
                get
                {
                    if (_marks == null) return null;
                    int[,] marks = new int[_marks.GetLength(0), _marks.GetLength(1)];
                    Array.Copy(_marks, marks, _marks.Length);
                    return marks;
                }
                private set
                {
                    if (value == null) _marks = null;
                    else _marks = (int[,])value.Clone();
                }
            }
            
            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                
                _coefs = new double[] { 2.5, 2.5, 2.5, 2.5 };
                _marks = new int[4, 7];
                TotalScore = 0;

                _jumpIndex= 0;
            }
            
            public void SetCriterias(double[] coefs)
            {
                if (coefs == null || coefs.Length != 4) return;
                Array.Copy(coefs, _coefs, coefs.Length);
            }

            public void Jump(int[] marks)
            {
                if (_jumpIndex >= 4 || marks == null || marks.Length != 7) return;
                for (int i = 0; i < marks.Length; i++) _marks[_jumpIndex, i] = marks[i];
                TotalScore += _coefs[_jumpIndex++] * (marks.Sum() - (marks.Max() + marks.Min()));
            }

            public void Print()
            {
                Console.WriteLine($"Name: {_name}");
                Console.WriteLine($"Surname: {_surname}");
                Console.WriteLine("Coefs:");
                foreach(double coef in _coefs) Console.Write($"{coef}\t");
                Console.WriteLine("\nMarks:");
                for (int i = 0; i < _marks.GetLength(0); i++)
                {
                    for (int j = 0; j < _marks.GetLength(1); j++) Console.Write($"{_marks[i, j]}\t");
                    Console.WriteLine();
                }
                Console.WriteLine($"Total score: {Math.Round(TotalScore, 2)}\n");
            }

            /* public static void PrintTable(Participant[] array)
            {
                Console.WriteLine("Name\tSurname\tTotalScore");
                foreach (Participant p in array) Console.WriteLine($"{p.Name}\t{p.Surname}\t{Math.Round(p.TotalScore, 2)}");
            } */
            
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
                    if (array[i].TotalScore >= array[j].TotalScore) buffer[k++] = array[i++];
                    else buffer[k++] = array[j++];
                }
                while (i <= mid) buffer[k++] = array[i++];
                while (j <= right) buffer[k++] = array[j++];
                for (i = left; i <= right; i++) array[i] = buffer[i];
            }
        }

        public class Judge
        {
            private string _name;
            private int[] _marks;
            private int _markIndex;
            
            public string Name => _name;
            
            public int[] Marks
            {
                get
                {
                    if (_marks == null) return null;
                    int[] marks = new int[_marks.Length];
                    Array.Copy(_marks, marks, _marks.Length);
                    return marks;
                }
            } 
            
            public Judge(string name, int[] marks)
            {
                _name = name;
                if (marks != null)
                {
                    _marks = new int[marks.Length];
                    Array.Copy(marks, _marks, marks.Length);
                }
            }

            public int CreateMark()
            {
                if (_marks == null || _marks.Length == 0) return 0;
                _markIndex %= _marks.Length;
                return _marks[_markIndex++];
            }
            
            public void Print()
            {
                Console.WriteLine($"Name: {_name}");
                Console.WriteLine("Marks:");
                foreach(double mark in _marks) Console.Write($"{mark}\t");
                Console.WriteLine();
            }
        }

        public class Competition
        {
            private Judge[] _judges;
            private Participant[] _participants;

            [JsonProperty]
            public Judge[] Judges
            {
                get { return _judges; }
                private set { _judges = value; }
            }

            [JsonProperty]
            public Participant[] Participants
            {
                get { return _participants; }
                private set { _participants = value; }
            }


            public Competition(Judge[] judges) {
                if (judges != null) {
                    _judges = new Judge[judges.Length];
                    Array.Copy(judges, _judges, judges.Length);
                }
                _participants = new Participant[0];
            }
            
            public void Evaluate(Participant jumper) {
                if (_judges == null) return;
                int[] marks = new int[7];
                int i = 0;
                foreach (Judge judge in _judges) {
                    if (judge != null)
                    {
                        marks[i++] = judge.CreateMark();
                        if (i == 7) break;
                    }
                }
                jumper.Jump(marks);
            }
            
            public void Add(Participant participant) {
                if (participant == null) return;
                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[_participants.Length - 1] = participant;
                Evaluate(_participants[_participants.Length - 1]);
            }
            
            public void Add(Participant[] participants) {
                if (participants == null) return;
                int n = _participants.Length;
                Array.Resize(ref _participants, n + participants.Length);
                Array.Copy(participants, 0, _participants, n, participants.Length);
                for (int i = n; i < _participants.Length; ++i) Evaluate(_participants[i]);
            }

            public void Sort() => Participant.Sort(_participants);
        }
    }
}