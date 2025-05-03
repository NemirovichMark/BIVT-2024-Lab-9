using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            private int _curMark;

            public string Name => _name;
            public string Surname => _surname;

            public double[] Marks => (double[])_marks?.Clone();
            public int[] Places => (int[])_places?.Clone();

            public int Score => _places?.Sum() ?? 0;

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _marks = new double[7];
                _places = new int[7];
                _curMark = 0;
            }

            public void Evaluate(double result)
            {
                if (_curMark >= 7 || _marks == null) return;

                _marks[_curMark++] = result;
            }

            public static void SetPlaces(Participant[] participants)
            {
                if (participants == null || participants.Length == 0) return;

                var sortedParticipants = participants.Where(x => x._marks != null && x._places != null).ToArray();
                int n = sortedParticipants.Length;
                for (int judge = 0; judge < 7; judge++)
                {
                    sortedParticipants = sortedParticipants.OrderByDescending(x => x._marks[judge]).ToArray();

                    for (int i = 0; i < n; i++)
                    {
                        sortedParticipants[i]._places[judge] = i + 1;
                    }
                }

                sortedParticipants = sortedParticipants
                    .Concat(participants.Where(x => x._marks == null || x._places == null)).ToArray();
                Array.Copy(sortedParticipants, participants, sortedParticipants.Length);
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;

                var sortedArray = array
                    .Where(x => x._marks != null && x._places != null)
                    .OrderBy(x => x.Score)
                    .ThenBy(x => x._places.Min())
                    .ThenByDescending(x => x._marks.Sum())
                    .Concat(array.Where(x => x._marks == null || x._places == null))
                    .ToArray();
                Array.Copy(sortedArray, array, sortedArray.Length);
            }

            public void Print()
            {
                Console.WriteLine($"Имя: {_name ?? "не задано"}");
                Console.WriteLine($"Фамилия: {_surname ?? "не задана"}");

                Console.Write("Оценки:\t");
                if (_marks == null) Console.WriteLine("не заданы");
                else
                {
                    foreach (var mark in _marks)
                    {
                        Console.Write($"{Math.Round(mark, 2)}\t");
                    }

                    Console.WriteLine();
                }

                Console.Write("Места:\t");
                if (_places == null) Console.WriteLine("не заданы");
                else
                {
                    foreach (var place in _places)
                    {
                        Console.Write($"{place}\t");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine($"Лучшее место: {_places?.Min() ?? 0}");
                Console.WriteLine($"Сумма оценок: {Math.Round(_marks?.Sum() ?? 0, 2)}");
                Console.WriteLine($"Результат: {Score}");
                Console.WriteLine();
            }
        }

        public abstract class Skating
        {
            protected Participant[] participants;
            protected double[] moods;

            public Participant[] Participants => participants;
            public double[] Moods => moods;

            public Skating(double[] moods, bool needModificate = true)
            {
                participants = new Participant[0];

                if (moods == null) return;

                this.moods = new double[Math.Min(moods.Length, 7)];
                Array.Copy(moods, this.moods, Math.Min(moods.Length, 7));

                if (needModificate)
                {
                    ModificateMood();
                }
            }

            protected abstract void ModificateMood();

            public void Evaluate(double[] marks)
            {
                if (marks == null || moods == null || participants == null) return;
                int index = Array.FindIndex(participants, x => x.Marks != null && x.Marks.All(y => y == 0));
                if (index == -1) return;

                for (int i = 0; i < Math.Min(marks.Length, 7); i++)
                {
                    participants[index].Evaluate(marks[i] * moods[i]);
                }
            }

            public void Add(Participant participant)
            {
                if (participants == null) return;

                Array.Resize(ref participants, participants.Length + 1);
                participants[participants.Length - 1] = participant;
            }

            public void Add(Participant[] participants)
            {
                if (participants == null || this.participants == null) return;

                this.participants = this.participants.Concat(participants).ToArray();
            }
        }

        public class FigureSkating : Skating
        {
            public FigureSkating(double[] moods, bool needModificate = true) : base(moods, needModificate)
            {
            }

            protected override void ModificateMood()
            {
                if (moods == null) return;

                moods = moods.Select((x, i) => x + (double)(i + 1) / 10).ToArray();
            }
        }

        public class IceSkating : Skating
        {
            public IceSkating(double[] moods, bool needModificate = true) : base(moods, needModificate)
            {
            }

            protected override void ModificateMood()
            {
                if (moods == null) return;

                moods = moods.Select((x, i) => x * (i + 101) / 100).ToArray();
            }
        }
    }
}