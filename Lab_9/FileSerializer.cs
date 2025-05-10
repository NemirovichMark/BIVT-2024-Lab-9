using Lab_7;
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
        //свойства
        public string FolderPath { get; private set; }
        public string FilePath { get; private set; }
        public abstract string Extension { get; }

        //методы
        public void SelectFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            Directory.CreateDirectory(path);
            FolderPath = path;
        }
        public void SelectFile(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(FolderPath)) return;

            string fileName = $"{name}.{Extension.Trim('.')}";
            string fullPath = Path.Combine(FolderPath, fileName);

            if (!File.Exists(fullPath))
            {
                File.Create(fullPath).Close();
            }

            FilePath = fullPath;
        }

    }
}
