using System;
using System.Linq;
using System.Security;


namespace Lab_7
{
    public class Green_1
    {
        public abstract class Participant
        {

            private string _surname;
            private string _group;
            private string _trainer;
            private double _result;


            protected double Standard;
            private static int PassedTheStandardCount;


            static Participant()
            {
                PassedTheStandardCount = 0;
            }


            public Participant(string surname, string group, string trainer)
            {
                _surname = surname;
                _group = group;
                _trainer = trainer;
                _result = 0;
            }


            public string Surname
            {
                get
                {
                    if (_surname == null) return default;
                    return _surname;
                }
            }

            public string Group
            {
                get
                {
                    if (_group == null) return default;
                    return _group;
                }
            }

            public string Trainer
            {
                get
                {
                    if (_trainer == null) return default;
                    return _trainer;
                }
            }

            public double Result
            {
                get
                {
                    return _result;
                }
            }


            public static int PassedTheStandard => PassedTheStandardCount;


            public bool HasPassed => (_result > 0 && _result <= Standard);


            public void Run(double result)
            {
                if (_result == 0)
                {
                    _result = result;
                    if (HasPassed)
                    {
                        PassedTheStandardCount++;
                    }
                }
            }

            public void Print()
            {
                Console.WriteLine(PassedTheStandard);
                Console.WriteLine($"{Surname} {Group} {Trainer} {Result} {HasPassed}");
            }

            public static Participant[] GetTrainerParticipants(Participant[] participants, Type participantType, string trainer)
            {

                return participants.Where(p => p.Trainer == trainer && p.GetType() == participantType).ToArray();  

            }
        }

        public class Participant100M : Participant
        {
            public Participant100M(string surname, string group, string trainer) : base( surname, group, trainer)
            {
                Standard = 12;
            }

        }

        public class Participant500M : Participant
        {
            public Participant500M(string surname, string group, string trainer) : base(surname, group, trainer)
            {
                Standard = 90;
            }
        }
    }
}