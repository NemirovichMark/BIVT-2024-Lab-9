using System;
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

            public int[] Marks => (int[])_marks?.Clone();

            public int Result { get; private set; }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _distance = 0;
                _marks = new int[5];
                Result = 0;
            }

            public void Jump(int distance, int[] marks, int target)
            {
                if (marks == null || _marks == null || marks.Length != 5 || distance < 0) return;

                _distance = distance;
                Array.Copy(marks, _marks, marks.Length);

                Result = Math.Max(marks.Sum() - marks.Max() - marks.Min() + 60 + (_distance - target) * 2, 0);
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;

                var sortedArray = array.OrderByDescending(x => x.Result).ToArray();
                Array.Copy(sortedArray, array, sortedArray.Length);
            }

            public void Print()
            {
                Console.WriteLine($"Имя: {_name ?? "не задано"}");
                Console.WriteLine($"Фамилия: {_surname ?? "не задана"}");
                Console.WriteLine($"Расстояние: {_distance}");

                Console.Write($"Оценки:\t");
                if (_marks == null) Console.WriteLine("не заданы");
                else
                {
                    foreach (var mark in _marks)
                    {
                        Console.Write($"{mark}\t");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine($"Результат: {Result}");
                Console.WriteLine();
            }
        }

        public abstract class SkiJumping
        {
            private string _name;
            private int _standard;
            private Participant[] _participants;

            public string Name => _name;
            public int Standard => _standard;
            public Participant[] Participants => (Participant[])_participants?.Clone();

            public SkiJumping(string name, int standard)
            {
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

            public void Add(Participant[] participants)
            {
                if (participants == null || _participants == null) return;

                _participants = _participants.Concat(participants).ToArray();
            }

            public void Jump(int distance, int[] marks)
            {
                if (_participants == null) return;

                int index = Array.FindIndex(_participants, x => x.Marks != null && x.Marks.All(y => y == 0));
                if (index == -1) return;

                _participants[index].Jump(distance, marks, _standard);
            }

            public void Print()
            {
                Console.WriteLine($"Название: {_name ?? "не задано"}");
                Console.WriteLine($"Норматив: {_standard}");
                Console.WriteLine($"Участники:");
                if (_participants == null) Console.WriteLine("Не заданы");
                else
                {
                    Console.WriteLine();
                    foreach (var participant in _participants)
                    {
                        participant.Print();
                    }
                }

                Console.WriteLine();
            }
        }

        public class JuniorSkiJumping : SkiJumping
        {
            public JuniorSkiJumping() : base("100m", 100)
            {
            }
        }

        public class ProSkiJumping : SkiJumping
        {
            public ProSkiJumping() : base("150m", 150)
            {
            }
        }
    }
}