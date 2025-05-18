using Lab_7;
using System;

namespace Lab_9
{
    public abstract class GreenSerializer : FileSerializer
    {
        public abstract override string Extension { get; } // Переопределяем метод расширений без каких-либо изменений.
        public abstract void SerializeGreen1Participant(Green_1.Participant participant, string fileName); // Публично-абстрактный метод сериализации участниц.
        public abstract void SerializeGreen2Human(Green_2.Human human, string fileName); // Публично-абстрактный метод сериализации людей.
        public abstract void SerializeGreen3Student(Green_3.Student student, string fileName); // Публично-абстрактный метод сериализации студентов.
        public abstract void SerializeGreen4Discipline(Green_4.Discipline participant, string fileName); // Публично-абстрактный метод сериализации дисциплин.
        public abstract void SerializeGreen5Group<T>(T group, string fileName) where T : Green_5.Group; // Публично-абстрактный метод сериализации различных групп.
        public abstract Green_1.Participant DeserializeGreen1Participant(string fileName);  // Публично-абстрактный метод десериализации участниц.
        public abstract Green_2.Human DeserializeGreen2Human(string fileName); // Публично-абстрактный метод сериализации людей.
        public abstract Green_3.Student DeserializeGreen3Student(string fileName); // Публично-абстрактный метод сериализации студентов.
        public abstract Green_4.Discipline DeserializeGreen4Discipline(string fileName); // Публично-абстрактный метод сериализации дисциплин.
        public abstract T DeserializeGreen5Group<T>(string fileName) where T : Green_5.Group; // Публично-абстрактный метод сериализации различных групп.
    }
}