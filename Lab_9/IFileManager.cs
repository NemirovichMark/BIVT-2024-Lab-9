using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public interface IFileManager
    {
        string FolderPath // свойство
        {
            get;
        }
        string FilePath
        {
            get;
        }
        void SelectFile(string name); // методы
        void SelectFolder(string path);
    }
}
