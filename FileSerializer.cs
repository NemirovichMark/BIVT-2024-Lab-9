using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Lab_7;

namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        public string FolderPath { get; private set; }
        public string FilePath { get; private set; }
        public void SelectFile(string name)
        {
            if (string.IsNullOrWhiteSpace(FolderPath) || string.IsNullOrWhiteSpace(name)) return;

            string fileName = String.Concat(name, ".", Extension);
            FilePath = Path.Combine(FolderPath, fileName);
            string[] file = Directory.GetFiles(FolderPath, fileName);
            if (file == null || file.Length == 0)
            {
                using (File.Create(FilePath)) { };
            }    
        }

        public void SelectFolder(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;
            FolderPath = path;
            Directory.CreateDirectory(FolderPath);
        }

        public abstract string Extension { get; }
    }
}
