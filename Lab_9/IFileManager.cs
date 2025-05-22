using System;
using Lab_7;

namespace Lab_9
{
    public interface IFileManager // Публичный интерфейс, от которого всё и произойдёт.
    {
        string FolderPath // Свойство пути папки.
        {
            get;
        }
        string FilePath // Свойство пути файла.
        {
            get;
        }
        void SelectFile(string name); // Пустой метод для дальнейшего переопределения.
        void SelectFolder(string name); // Пустой метод для дальнейшего переопределения.
    }
}