using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public interface IFileManager
    {
        //Создать интерфейс IFileManager в файле IFileManager.cs.
        //В интерфейсе объявить строковые свойства для чтения FolderPath
        //и FilePath.В интерфейсе объявить методы  void SelectFile(string name)
        //и void SelectFolder(string path).
        public string FolderPath { get; }
        public string FilePath { get; }
        public void SelectFile(string name);
        public void SelectFolder(string path); //также можно не указывать паблик
                                               //потому что в интерфейсе он идет по умолчанию

    }
}
