using Lab_7;

namespace Lab_9
{
    public class PurpleTXTSerializer : PurpleSerializer
    {
        public override string Extension => "txt";
        public override T DeserializePurple1<T>(string fileName)
        {
            SelectFile(fileName);
            string[] lines = File.ReadAllLines(FilePath);
            if (lines[0] == $"type={nameof(Purple_1.Participant)}")
            {
                Purple_1_Participant_DAO dao = new Purple_1_Participant_DAO(lines);
                Purple_1.Participant p = new Purple_1.Participant(dao.Name, dao.Surname);
                p.SetCriterias(dao.Coefs);
                for (int i = 0; i < 7; i++)
                {
                    p.Jump(new int[] { dao.Marks[i, 0], dao.Marks[i, 1], dao.Marks[i, 2], dao.Marks[i, 3] });
                }

                return (T)(Object)p;
            }
            if (lines[0] == $"type={nameof(Purple_1.Judge)}")
            {

                Purple_1_Judge_DAO dao = new Purple_1_Judge_DAO(lines);
                Purple_1.Judge j = new Purple_1.Judge(dao.Name, dao.Marks);
                return (T)(Object)j;
            }
            if (lines[0] == $"type={nameof(Purple_1.Competition)}")
            {
                Purple_1_Competition_DAO comp_dao = new Purple_1_Competition_DAO(lines);
                Purple_1.Judge[] judges = comp_dao.Judges.Select(dao => new Purple_1.Judge(dao.Name, dao.Marks)).ToArray();
                Purple_1.Participant[] participants = comp_dao.Participants.Select(dao => new Purple_1.Participant(dao.Name, dao.Surname)).ToArray();
                Purple_1.Competition comp = new Purple_1.Competition(judges);
                comp.Add(participants);
                foreach (var p in comp.Participants)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        comp.Evaluate(p);
                    }
                }

                return (T)(Object)comp;
            }

            return null;
        }

        public override T DeserializePurple2SkiJumping<T>(string fileName)
        {
            throw new NotImplementedException();
        }

        public override T DeserializePurple3Skating<T>(string fileName)
        {
            throw new NotImplementedException();
        }

        public override Purple_4.Group DeserializePurple4Group(string fileName)
        {
            throw new NotImplementedException();
        }

        public override Purple_5.Report DeserializePurple5Report(string fileName)
        {
            throw new NotImplementedException();
        }

        public override void SerializePurple1<T>(T obj, string fileName)
        {
            if (obj is Purple_1.Participant p)
            {
                Purple_1_Participant_DAO dao = new Purple_1_Participant_DAO(p.Name, p.Surname, p.Coefs, p.Marks);
                string[] lines = dao.SerializeToTXT();

                SelectFile(fileName);
                File.WriteAllLines(FilePath, lines);
            }
        }

        public override void SerializePurple2SkiJumping<T>(T jumping, string fileName)
        {
            throw new NotImplementedException();
        }

        public override void SerializePurple3Skating<T>(T skating, string fileName)
        {
            throw new NotImplementedException();
        }

        public override void SerializePurple4Group(Purple_4.Group group, string fileName)
        {
            throw new NotImplementedException();
        }

        public override void SerializePurple5Report(Purple_5.Report report, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
