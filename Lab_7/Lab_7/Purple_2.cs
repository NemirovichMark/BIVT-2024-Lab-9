using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _marks = new int[5];
                _distance = 0;
            }

            public string Name => _name;
            public string Surname => _surname;
            public int Distance => _distance;
            public int[] Marks
            {
                get
                {
                    if (_marks == null) return null;

                    int[] copy = new int[_marks.Length];
                    Array.Copy(_marks, copy, _marks.Length);
                    return copy;
                }
            }

            public int Result { get; private set; }

            public void Jump(int distance, int[] marks, int target)
            {
                if (_marks == null || marks == null || marks.Length != 5) return;
                _distance = distance;
                Array.Copy(marks, _marks, marks.Length);
                int points = 60 + (_distance - target) * 2;
                //if (points <= 0) points = 0;
                Result += points + marks.Sum() - (marks.Max() + marks.Min());
                if (Result < 0) Result = 0; 
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) { return; }

                for (int i = 1; i < array.Length;)
                {
                    if (i == 0 || array[i - 1].Result >= array[i].Result)
                    {
                        i++;
                    }
                    else
                    {
                        Participant tmp = array[i - 1];
                        array[i - 1] = array[i];
                        array[i] = tmp;
                        i--;
                    }
                }
            }

            public void Print()
            {
                Console.WriteLine($"Name: {_name}\nSurname: {_surname}\nDistance: {_distance}");
                Console.WriteLine("\nMarks:");
                foreach (int mark in _marks) Console.Write($"{mark} ");
                Console.WriteLine();
                Console.WriteLine($"Result: {Result}\n");
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
            {
                get
                {
                    if (_participants == null) return null;
                    Participant[] participants = new Participant[_participants.Length];
                    Array.Copy(_participants, participants, _participants.Length);
                    return participants;
                }
            }


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
                if (_participants == null || participants == null) return;
                foreach (Participant participant in participants)
                {
                    Add(participant);
                }

            }


            public void Jump(int distance, int[] marks)
            {
                if (_participants == null || marks == null) return;

                for (int i = 0; i < _participants.Length; i++)
                {
                    bool flag = false;
                    for (int j = 0; j < _participants[i].Marks.Length; j++)
                    {
                        if (_participants[i].Marks[j] > 0) { flag = true; break; }
                    }

                    if (!flag)
                    {
                        _participants[i].Jump(distance, marks, _standard);
                        break;
                    }
                }


            }


            public void Print()
            {
                Console.WriteLine($"Name: {_name}");
                Console.WriteLine($"Standard: {_standard}\n");
                Console.WriteLine("Participants:");
                foreach (Participant p in _participants)
                {
                    p.Print();
                }

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
