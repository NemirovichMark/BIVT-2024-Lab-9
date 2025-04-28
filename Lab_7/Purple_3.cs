using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7 {
    public class Purple_3 {
        public struct Participant {
            private string _name;
            private string _surname;
            private double[] _marks;
            private int[] _places;
            private int _markCount;

            public string Name => _name;
            public string Surname => _surname;
            public double[] Marks => (_marks == null) ? _marks : (double[])_marks.Clone(); 
            public int[] Places => (_places == null) ? _places : (int[])_places.Clone();
            public int Score => (_places == null) ? 0 : _places.Sum(); 

            

            public Participant(string name, string surname) { 
                _name = name;  
                _surname = surname;

                _marks = new double[7];
                _places = new int[7];
            }

            public void Evaluate(double result) {
                if (_markCount >= 7 || _marks == null) return;

                _marks[_markCount++] = result;
            }

            public static void SetPlaces(Participant[] participants) {
                if (participants == null || participants.Length == 0) return;
                
                var validParticipants = participants.Where(x => x.Marks != null && x.Places != null).ToArray();
                var invalidParticipants = participants.Where(x => x.Marks == null || x.Places == null).ToArray();

                for (int judge = 0; judge < 7; judge++) {
                    validParticipants = validParticipants.OrderByDescending(x => x.Marks[judge]).ToArray(); 

                    for (int p = 0; p < validParticipants.Length; p++) 
                        validParticipants[p]._places[judge] = p + 1;
                }

                validParticipants = validParticipants.Concat(invalidParticipants).ToArray();
                Array.Copy(validParticipants, participants, participants.Length);
            }

            public static void Sort(Participant[] array) {
                if (array == null) return;

                var sortedArray = array.Where(x => x.Marks != null && x.Places != null)
                                    .OrderBy(x => x.Score).ThenBy(x => x.Places.Min()).ThenByDescending(x => x.Marks.Sum()).ToArray();
                    
                sortedArray = sortedArray.Concat(
                                array.Where(x => x.Marks == null || x.Places == null)
                                ).ToArray();

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
            
            public void Print() {
                Console.WriteLine($"Name: {_name ?? "N/A"}");
                Console.WriteLine($"Surname: {_surname ?? "N/A"}");
                PrintArray(_marks, "Marks:");
                PrintArray(_places, "Places:");
                Console.WriteLine($"Score: {Score}");
            }

        }

        public abstract class Skating {
            private Participant[] _participants;
            protected double[] _moods;
            public Participant[] Participants => _participants;
            public double[] Moods 
                => (_moods == null) ? _moods : (double[])_moods.Clone();

            public Skating(double[] moods, bool needModificate = true) {
                _moods = new double[7];
                
                if (moods != null)
                    Array.Copy(moods, _moods, _moods.Length);

                if (needModificate) ModificateMood();
                
                _participants = new Participant[0]; 
            }

            protected abstract void ModificateMood();

            public void Evaluate(double[] marks) {
                if (marks == null) return;
                
                var targetParticipantIndex = Array.FindIndex(_participants, p => p.Marks == null || p.Marks.All(m => m == 0));
                if (targetParticipantIndex == -1) return;

                var targetParticipant = _participants[targetParticipantIndex];
                for (int i = 0; i < marks.Length; i++) 
                    targetParticipant.Evaluate(marks[i] * _moods[i]);
                
                _participants[targetParticipantIndex] = targetParticipant;
            }

            public void Add(Participant participant) {
                if (_participants == null)
                    _participants = new Participant[0];

                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[^1] = participant;
            }

            public void Add(Participant[] participants) {
                if (participants == null) return;

                if (_participants == null)
                    _participants = new Participant[0];

                int n = _participants.Length;
                Array.Resize(ref _participants, n + participants.Length);
                Array.Copy(participants, 0, _participants, n, participants.Length);
            }
        }

        public class FigureSkating : Skating {
            public FigureSkating(double[] moods, bool needModificate = true) : base(moods, needModificate) {}
            protected override void ModificateMood() {
                if (_moods == null) return;
                
                _moods = _moods.Select((m, i) => m + (double)(i + 1) / 10).ToArray();
            }
        }

        public class IceSkating : Skating {
            public IceSkating(double[] moods, bool needModificate = true) : base(moods, needModificate) {}

            protected override void ModificateMood() {
                if (_moods == null) return;
                
               _moods = _moods.Select((m, i) => m * (100 + i + 1) / 100).ToArray();
            }
        } 
    }
}