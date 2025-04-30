using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7 {
    public class Purple_1 {
        public class Participant {
            private string _name;
            private string _surname;
            private double[] _coefs;
            private int[,] _marks;
            private int _jumpsCount;

            public string Name => _name;
            public string Surname => _surname;
            public double[] Coefs => (_coefs == null) ? _coefs : (double[])_coefs.Clone(); 
            public int[,] Marks => (_marks == null) ? _marks : (int[,])_marks.Clone();
            public double TotalScore {get; private set; }

            public Participant(string name, string surname) {
                _name = name;
                _surname = surname;
                _coefs = new double[4] {2.5, 2.5, 2.5, 2.5};
                _marks = new int[4, 7];
            }

            public void SetCriterias(double[] coefs) {
                if (_coefs == null || coefs == null) 
                    return;
    
                for (int i = 0; i < 4; i++) 
                    _coefs[i] = coefs[i];
            }

            public void Jump(int[] marks) {
                if (_marks == null || _coefs == null || marks == null || _jumpsCount >= 4) 
                    return;

                for (int j = 0; j < 7; j++) 
                    _marks[_jumpsCount, j] = marks[j];
                
                TotalScore += (marks.Sum() - marks.Min() - marks.Max()) * _coefs[_jumpsCount++];
            }

            public static void Sort(Participant[] array) {
                if (array == null) return;

                var sortedArray = array.OrderByDescending(x => x.TotalScore).ToArray();
                Array.Copy(sortedArray, array, array.Length);
            }

            private void PrintArray<T>(T[] array, string label) {
                Console.Write(label + " ");
                if (array == null) {
                    Console.WriteLine("N/A");
                    return;
                }
                
                foreach (var element in array)
                    Console.Write(element + " ");
                Console.WriteLine();
            }

            private void PrintMatrix<T>(T[,] matrix, string label) {
                if (matrix == null) {
                    Console.WriteLine($"{label} N/A");
                    return;
                }

                Console.WriteLine(label);
                for (int i = 0; i < matrix.GetLength(0); i++) {
                    for (int j = 0; j < matrix.GetLength(1); j++) {
                        Console.Write(matrix[i, j] + " ");
                    }
                    Console.WriteLine();
                }
            }
            public void Print() {
                Console.WriteLine($"Name: {_name ?? "N/A"}");
                Console.WriteLine($"Surname: {_surname ?? "N/A"}");
                PrintArray(_coefs, "Coefs:");
                PrintMatrix(_marks, "   :");
                Console.WriteLine($"Total Score: {TotalScore}");
            }
        }

        public class Judge {
            private string _name;
            private int[] _marks;
            public string Name => _name;
            public int[] Marks => (int[])_marks?.Clone();
            private int _curMarkCounter;
            public Judge(string name, int[] marks) {
                _name = name;

                if (marks != null) {
                    _marks = new int[marks.Length];
                    Array.Copy(marks, _marks, marks.Length);
                }
            }

            public int CreateMark() {
                if (_marks == null || _marks.Length == 0) return 0;

                int result = _marks[_curMarkCounter++];
                _curMarkCounter %= _marks.Length;
                
                return result;
            }

            private void PrintArray(int[] array, string label) {
                Console.Write(label + " ");
                if (array == null) {
                    Console.WriteLine("N/A");
                    return;
                }
                
                foreach (var element in array)
                    Console.Write(element + " ");
                Console.WriteLine();
            }

            public void Print() {
                Console.WriteLine($"Name: {_name ?? "N/A"}");
                PrintArray(_marks, "Marks:");
            }

        }

        public class Competition {
            private Judge[] _judges;
            private Participant[] _participants;
            public Judge[] Judges => _judges;
            public Participant[] Participants => _participants;

            public Competition(Judge[] judges) {
                if (judges != null) {
                    _judges = new Judge[judges.Length];
                    Array.Copy(judges, _judges, judges.Length);
                }

                _participants = new Participant[0];
            }

            public void Evaluate(Participant jumper) {
                if (_judges == null) return;

                var achievedMarks = new int[7];
                int k = 0;

                foreach (var judge in _judges) {
                    if (k >= 7) break;
                    if (judge != null) 
                        achievedMarks[k++] = judge.CreateMark();
                }
                
                jumper.Jump(achievedMarks);
            }

            public void Add(Participant participant) {
                if (participant == null) return;

                if (_participants == null) 
                    _participants = new Participant[0];

                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[^1] = participant;
                
                Evaluate(_participants[^1]);
            }

            public void Add(Participant[] participants) {
                if (participants == null) return;
                
                if (_participants == null)
                    _participants = new Participant[0];

                int n = _participants.Length;

                Array.Resize(ref _participants, n + participants.Length);
                Array.Copy(participants, 0, _participants, n, participants.Length);

                for (int i = n; i < _participants.Length; i++) 
                    Evaluate(_participants[i]);
            }

            public void Sort() {
                var sortedParticipants = _participants.OrderByDescending(p => p.TotalScore).ToArray();
                Array.Copy(sortedParticipants, _participants, sortedParticipants.Length);
            }   
        }
    }
}   