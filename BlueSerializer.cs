using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    abstract public class BlueSerializer : FileSerializer
    {
        abstract public void SerializeBlue1Response(Blue_1.Response participant, string fileName);
        abstract public void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName);
        abstract public void SerializeBlue3Participant<T>(T student, string fileName) where T : Blue_3.Participant;
        abstract public void SerializeBlue4Group(Blue_4.Group participant, string fileName);
        abstract public void SerializeBlue5Team<T>(T group, string fileName) where T : Blue_5.Team;

        abstract public Blue_1.Response DeserializeBlue1Response(string fileName);
        abstract public Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName);
        abstract public T DeserializeBlue3Participant<T>(string fileName) where T : Blue_3.Participant;
        abstract public Blue_4.Group DeserializeBlue4Group(string fileName);
        abstract public T DeserializeBlue5Team<T>(string fileName) where T : Blue_5.Team;
    }
}