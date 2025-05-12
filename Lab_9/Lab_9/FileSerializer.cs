using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9 
{
    public abstract class FileSerializer : IFileManager
    {
        public virtual string FolderPath { get; private set; } 

        public virtual string FilePath { get; private set; }

        public abstract string Extension { get; }

        public virtual void SelectFile(string name)
        {
            if (name == null || name.Length == 0)
                Console.WriteLine("Имя файла не может быть пустым или длины 0");

            string fileName = $"{name}.{Extension}"; // Extension - расширение, поэтому в имени идет через символ "."
            string pathName = Path.Combine(FolderPath, fileName); // Combine объединяет 2 строки в путь через "/"

            FilePath = pathName;
            var f = File.Create(pathName);
            f.Close();
            //Console.WriteLine(Path.Combine(pathName));
        }

        public virtual void SelectFolder(string path)
        {
            if (path.Length == 0 || path == null) Console.WriteLine("Путь до папки не может быть пустым или длины 0");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path); // CreateDirectory(path) создает каталоги
                                                                          // и подкатологи по данному пути (path)
            FolderPath = path; 
        }
    }
}

//▒▒▒▒▒█▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█
//▒▒▒▒▒█░▒▒▒▒▒▒▒▓▒▒▓▒▒▒▒▒▒▒░█
//▒▒▒▒▒█░▒▒▓▒▒▒▒▒▒▒▒▒▄▄▒▓▒▒░█░▄▄
//▄▀▀▄▄█░▒▒▒▒▒▒▓▒▒▒▒█░░▀▄▄▄▄▄▀░░█
//█░░░░█░▒▒▒▒▒▒▒▒▒▒▒█░░░░░░░░░░░█      
//▒▀▀▄▄█░▒▒▒▒▓▒▒▒▓▒█░░░█▒░░░░█▒░░█    
//▒▒▒▒▒█░▒▓▒▒▒▒▓▒▒▒█░░░░░░░▀░░░░░█
//▒▒▒▄▄█░▒▒▒▓▒▒▒▒▒▒▒█░░█▄▄█▄▄█░░█
//▒▒▒█░░░█▄▄▄▄▄▄▄▄▄▄█░█▄▄▄▄▄▄▄▄▄█
//▒▒▒█▄▄█░░█▄▄█░░░░░░█▄▄█░░█▄▄█


