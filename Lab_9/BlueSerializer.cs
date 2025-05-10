using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_9;

namespace Lab_9
{
    public abstract class BlueSerializer : FileSerializer
    {
        //protected string GetFullPath(string fileName)
        //{
        //    return Path.Combine(FolderPath, $"{fileName}.{Extension}");
        //}

        //1 - сериализовать, 2 - имя файла, который должен быть создан в папке по пути FolderPath.
        //Методы сериализуют объект в файл в соответствующем формате
        public abstract void SerializeBlue1Response(Blue_1.Response participant, string fileName);
        public abstract void SerializeBlue2WaterJump(Blue_2.WaterJump participant, string fileName);
        public abstract void SerializeBlue3Participant<T>(T student, string fileName) where T : Blue_3.Participant;
        public abstract void SerializeBlue4Group(Blue_4.Group participant, string fileName);
        public abstract void SerializeBlue5Team<T>(T group, string fileName) where T : Blue_5.Team;


        //1 - имя файла, который находится в папке по пути FolderPath
        //и возвращают сериализованный объект типа из соответствующего формата,
        //который хранится в файле.

        public abstract Blue_1.Response DeserializeBlue1Response(string fileName);

        public abstract Blue_2.WaterJump DeserializeBlue2WaterJump(string fileName);

        public abstract T DeserializeBlue3Participant<T>(string fileName) where T : Blue_3.Participant;

        public abstract Blue_4.Group DeserializeBlue4Group(string fileName);

        public abstract T DeserializeBlue5Team<T>(string fileName) where T : Blue_5.Team;

    }
}
