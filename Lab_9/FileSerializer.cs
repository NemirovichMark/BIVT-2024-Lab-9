using System;
using System.Collections.Generic;
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
        
        public void SelectFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            Directory.CreateDirectory(path);
            FolderPath = path;
        }
        public void SelectFile(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(FolderPath) || string.IsNullOrEmpty(Extension)) return;
            string fileName = $"{name}.{Extension}";
            string filePath = Path.Combine(FolderPath, fileName);

            if (!File.Exists(filePath))
            {
                var file = File.Create(filePath);
                file.Close();
            }

            FilePath = filePath;
        }
    }
}
