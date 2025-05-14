using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Lab_7;

namespace Lab_9
{
  public abstract class GreenSerializer : FileSerializer
  {
    public abstract void SerializeGreen1Participant(Green_1.Participant participant, string fileName);
    public abstract void SerializeGreen2Human(Green_2.Human human, string fileName);
    public abstract void SerializeGreen3Student(Green_3.Student student, string fileName);
    public abstract void SerializeGreen4Discipline(Green_4.Discipline participant, string fileName);
    public abstract void SerializeGreen5Group<T>(T group, string fileName) where T : Green_5.Group;

    public abstract Green_1.Participant DeserializeGreen1Participant(string fileName);
    public abstract Green_2.Human DeserializeGreen2Human(string fileName);
    public abstract Green_3.Student DeserializeGreen3Student(string fileName);
    public abstract Green_4.Discipline DeserializeGreen4Discipline(string fileName);
    public abstract T DeserializeGreen5Group<T>(string fileName) where T : Green_5.Group;

  }
}