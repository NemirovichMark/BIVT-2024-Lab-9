using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab_7.Purple_1;

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
            private int _currentJump;

            public string Name => _name;
            public string Surname => _surname;
            public double[] Coefs
            {
                get
                {
                    if (_coefs == null) return default(double[]);

                    double[] coefs = new double[_coefs.Length];
                    Array.Copy(_coefs, coefs, _coefs.Length);
                    return coefs;
                }
            }
            public int[,] Marks
            {
                get
                {
                    if (_marks == null) return null;

                    int[,] marks = new int[_marks.GetLength(0), _marks.GetLength(1)];
                    Array.Copy(_marks, marks, _marks.Length);
                    return marks;
                }
            }

            public double TotalScore
            {
                get
                {
                    if (_marks == null || _coefs == null) return 0;

                    double totalScore = 0;

                    for (int jump = 0; jump < 4; jump++)
                    {
                        int minMark = int.MaxValue;
                        int maxMark = int.MinValue;
                        int score = 0;

                        for (int judge = 0; judge < 7; judge++)
                        {
                            int currentMark = _marks[jump, judge];

                            if (currentMark > maxMark)
                                maxMark = currentMark;

                            if (currentMark < minMark)
                                minMark = currentMark;

                            score += currentMark;
                        }

                        score -= minMark + maxMark;
                        totalScore += score * _coefs[jump];
                    }

                    return totalScore;
                }
            }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _coefs = new double[4] { 2.5, 2.5, 2.5, 2.5 };
                _marks = new int[4, 7];
                _currentJump = 0;

                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 7; j++)
                        _marks[i, j] = 0;
            }

            public void SetCriterias(double[] coefs)
            {
                if (coefs == null || _coefs == null || coefs.Length != _coefs.Length) return;

                for (int i = 0; i < coefs.Length; i++)
                    _coefs[i] = coefs[i];
            }

            public void Jump(int[] marks)
            {
                if (marks == null || _currentJump >= 4 || marks.Length != 7) return;

                for (int i = 0; i < marks.Length; i++)
                    _marks[_currentJump, i] = marks[i];

                _currentJump++;
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;

                int index = 0;

                while (index < array.Length)
                {
                    if (index == 0 || array[index].TotalScore <= array[index - 1].TotalScore)
                    {
                        index++;
                    }
                    else
                    {
                        Participant temp = array[index];
                        array[index] = array[index - 1];
                        array[index - 1] = temp;
                        index--;
                    }
                }
            }
            public void Print()
            {
                Console.WriteLine($"{Name} {Surname} - Итоговый балл: {TotalScore:F2}");
            }
        }

        public class Judge
        {
            private string _name;
            private int[] _marks;
            private int _currentMarkIndex;
            public int[] Marks
            {
                get
                {
                    return _marks == null ? null : (int[])_marks.Clone();
                }
            }

            public string Name => _name;

            public Judge(string name, int[] marks)
            {
                _name = name;

                if (marks != null)
                {
                    _marks = new int[marks.Length];
                    Array.Copy(marks, _marks, marks.Length);
                }

                _currentMarkIndex = 0;
            }
            public int CreateMark()
            {
                if (_marks == null || _marks.Length == 0) return 0;

                int mark = _marks[_currentMarkIndex];
                _currentMarkIndex = (_currentMarkIndex + 1) % _marks.Length;

                return mark;
            }

            public void Print()
            {
                Console.WriteLine($"Судья: {Name}");

                foreach (var num in _marks)
                {
                    Console.Write(num + " ");
                }

                Console.WriteLine();
            }
        }
        public class Competition
        {
            private Judge[] _judges;
            private Participant[] _participants;
           
            public Judge[] Judges => _judges;
            public Participant[] Participants => _participants;

            public Competition(Judge[] judges)
            {
                if (judges != null)
                {
                    _judges = new Judge[judges.Length];
                    Array.Copy(judges, _judges, judges.Length);
                }

                _participants = new Participant[0];
            }

            public void Evaluate(Participant jumper)
            {
                if (jumper == null || _judges == null) return;

                int[] marks = new int[_judges.Length];

                for (int i = 0; i < _judges.Length; i++)
                {
                    marks[i] = _judges[i].CreateMark();
                }

                jumper.Jump(marks);
            }

            public void Add(Participant participant)
            {
                if (participant == null) return;

                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[_participants.Length - 1] = participant;

                Evaluate(participant);
            }
            public void Add(Participant[] participants)
            {
                if (participants == null || participants.Length == 0) return;

                var updatedParticipants = new Participant[_participants.Length + participants.Length];
                Array.Copy(_participants, updatedParticipants, _participants.Length);

                for (int i = 0; i < participants.Length; i++)
                {
                    if (participants[i] != null)
                    {
                        updatedParticipants[_participants.Length + i] = participants[i];
                        Evaluate(participants[i]);
                    }
                }

                _participants = updatedParticipants;
            }
            public void Sort()
            {
                if (_participants == null) return;

                for (int index = 1; index < _participants.Length;)
                {
                    if (index == 0 || _participants[index - 1].TotalScore > _participants[index].TotalScore)
                    {
                        index++;
                    }
                    else
                    {
                        Participant temp = _participants[index];
                        _participants[index] = _participants[index - 1];
                        _participants[index - 1] = temp;
                        index--;
                    }
                }
            }
        }
    }
}
