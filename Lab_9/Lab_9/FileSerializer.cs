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
        public string FolderPath { get; private set; }
        public string FilePath { get; private set; }
        public abstract string Extension { get; }
        public void SelectFile(string name)
        {
            string filePath = Path.Combine(FolderPath, $"{name}.{Extension}");
            if (!File.Exists(filePath))
                using (File.Create(filePath)) { }
            FilePath = filePath;
        }
        public void SelectFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FolderPath = path;
        }

    }
}
