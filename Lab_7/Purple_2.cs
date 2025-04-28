using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                public int Distance => _distance;
            public int[] Marks => (_marks == null) ? _marks : (int[])_marks.Clone();
            public int Result { get; private set; }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _marks = new int[5];
            }

            public void Jump(int distance, int[] marks, int target)
            {
                if (_marks == null || marks == null)
                    return;

                for (int i = 0; i < 5; i++)
                    _marks[i] = marks[i];

                Result = Math.Max(0, marks.Sum() - marks.Min() - marks.Max()
                                    + 60 + (distance - target) * 2);

            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;

                var sortedArray = array.OrderByDescending(x => x.Result).ToArray(); 
                Array.Copy(sortedArray, array, array.Length);
            }

            private void PrintArray<T>(T[] array, string label)
            {
                Console.Write(label + " ");
                if (array == null)
                {
                    Console.WriteLine("N/A");
                    return;
                }

                foreach (var element in array)
                    Console.Write(element + " ");
                Console.WriteLine();
            }

            public void Print()
            {
                Console.WriteLine($"Name: {_name ?? "N/A"}");
                Console.WriteLine($"Surname: {_surname ?? "N/A"}");
                Console.WriteLine($"Distance: {_distance}");
                PrintArray(_marks, "Marks:");
            }
        }

        public abstract class SkiJumping
        {
            private string _name;
            private int _standard;
            private Participant[] _participants;
            public string Name => _name;
            public int Standard => _standard;
            public Participant[] Participants
                => (_participants == null) ? _participants : (Participant[])_participants.Clone();

            public SkiJumping(string name, int standard)
            {
                _name = name;
                _standard = standard;
                _participants = new Participant[0]; // what if null needed?or
            }

            public void Add(Participant participant)
            {
                if (_participants == null)
                    _participants = new Participant[0];

                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[^1] = participant;
            }

            public void Add(Participant[] participants)
            {
                if (participants == null) return;

                if (_participants == null)
                    _participants = new Participant[0];

                int n = _participants.Length;
                Array.Resize(ref _participants, n + participants.Length);
                Array.Copy(participants, 0, _participants, n, participants.Length);
            }

            public void Jump(int distance, int[] marks)
            {
                var targetParticipantIndex = Array.FindIndex(_participants, p => p.Marks == null || p.Marks.All(m => m == 0));
                if (targetParticipantIndex == -1) return;

                var targetParticipant = _participants[targetParticipantIndex];
                targetParticipant.Jump(distance, marks, _standard);

                _participants[targetParticipantIndex] = targetParticipant;
            }


            private void PrintArray(Participant[] array, string label)
            {
                Console.Write(label + " ");
                if (array == null)
                {
                    Console.WriteLine("N/A");
                    return;
                }

                for (int i = 0; i < array.Length; i++)
                {
                    Console.WriteLine($"{i + 1}:");
                    array[i].Print();
                }

                Console.WriteLine();
            }

            public void Print()
            {
                Console.WriteLine($"Name: {_name ?? "N/A"}");
                Console.WriteLine($"Standard: {_standard}");
                PrintArray(_participants, "Participants:");
            }

        }

        public class JuniorSkiJumping : SkiJumping
        {
            public JuniorSkiJumping() : base("100m", 100) { }

        }

        public class ProSkiJumping : SkiJumping
        {
            public ProSkiJumping() : base("150m", 150) { }
        }
    }
}