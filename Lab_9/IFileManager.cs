using System;
using Lab_7;


namespace Lab_9
{
    public interface IFileManager
    {
        // свойства
        string FolderPath { get; }
        string FilePath { get; }

        // методы
        void SelectFile(string name);
        void SelectFolder(string path);
    }
}
