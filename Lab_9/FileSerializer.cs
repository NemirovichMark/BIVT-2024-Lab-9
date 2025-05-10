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
        public abstract string Extension { get; }
        //private FileSerializer() : base() { } 
        public string FolderPath { get; private set; }
        public string FilePath { get; private set; }
        //который получает путь к папке (включая папку) и
        //сохраняет его в FolderPath. Если такой папки нет,
        //то создать её.

        public void SelectFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            Directory.CreateDirectory(path); //if exists no error

            //FolderPath = Path.GetFullPath(path);
            //Сохраняет абсолютный путь к папке в FolderPath
            FolderPath = path;
        }

        public void SelectFile(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(FolderPath)) return; //no need
            
            string fileName = $"{name}.{Extension.Trim('.')}";
            string FullFilePath = Path.Combine(FolderPath, fileName);

            if (!File.Exists(FullFilePath))
            {
                var fs = File.Create(FullFilePath);
                fs.Close();
            }

            FilePath = FullFilePath;
            //string FullFileName = $"{name}.{Extension}";
            //string FullPathToFile = Path.Combine(FolderPath, FullFileName);
            //if (!Directory.Exists(FullPathToFile))
            //    File.Create(FullPathToFile).Close();
            //FilePath = FullPathToFile;
        }

    }
}
