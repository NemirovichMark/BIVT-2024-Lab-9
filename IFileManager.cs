using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public interface IFileManager
    { 
        //всё паблик abstract не нужны модификаторы доступа

        //Свойства по умолчанию абстаракт паблик 
        string FolderPath { get;}
        string FilePath { get; }

        //методы (только объявление) 
        void SelectFile(string name);
        void SelectFolder(string path);
    }
}
