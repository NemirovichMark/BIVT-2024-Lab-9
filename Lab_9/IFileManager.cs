using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public interface IFileManager
    {
        //свойства
        string FolderPath { get; }
        string FilePath { get; }

        //методы
        void SelectFolder(string path);
        void SelectFile(string name);
    }
}
