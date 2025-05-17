using Lab_7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab_9
{
    public class BlueJSONSerializer : BlueSerializer
    {
        override public string Extension => "json";

        override public void SerializeBlue1Response(Blue_1.Response participant, string fileName)
        {
            if ((participant == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            string json = JsonSerializer.Serialize(participant);
            File.WriteAllText(fileName, json);
        }

        override public void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName)
        {
            if ((participant == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            string json = JsonSerializer.Serialize(participant);
            File.WriteAllText(fileName, json);
        }
        override public void SerializeBlue3Participant<T>(T student, string fileName)
        {
            if ((student == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            string json = JsonSerializer.Serialize(student);
            File.WriteAllText(fileName, json);
        }
        override public void SerializeBlue4Group(Blue_4.Group participant, string fileName)
        {
            if ((participant == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            string json = JsonSerializer.Serialize(participant);
            File.WriteAllText(fileName, json);
        }
        override public void SerializeBlue5Team<T>(T group, string fileName)
        {
            if ((group == null) || (string.IsNullOrWhiteSpace(fileName))) return;
            string json = JsonSerializer.Serialize(group);
            File.WriteAllText(fileName, json);
        }

        override public Blue_1.Response DeserializeBlue1Response(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            string text = File.ReadAllText(fileName);
            Blue_1.Response response = JsonSerializer.Deserialize<Blue_1.Response>(text);
            return response;
        }
        override public Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            string text = File.ReadAllText(fileName);
            Blue_2.WaterJump waterjump = JsonSerializer.Deserialize<Blue_2.WaterJump>(text);
            return waterjump;
        }
        override public T DeserializeBlue3Participant<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            string text = File.ReadAllText(fileName);
            Blue_3.Participant participant = JsonSerializer.Deserialize<Blue_3.Participant>(text);
            return (T)participant;
        }
        override public Blue_4.Group DeserializeBlue4Group(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            string text = File.ReadAllText(fileName);
            Blue_4.Group group = JsonSerializer.Deserialize<Blue_4.Group>(text);
            return group;
        }
        override public T DeserializeBlue5Team<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            string text = File.ReadAllText(fileName);
            Blue_5.Team team = JsonSerializer.Deserialize<Blue_5.Team>(text);
            return (T)team;
        }
    }
}
