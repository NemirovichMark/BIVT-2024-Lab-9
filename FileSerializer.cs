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
        public void SelectFolder(string path)
        {
            if (path == null) { return; }
            FolderPath = path;
            Directory.CreateDirectory(FolderPath);
        }
        public void SelectFile(string name)
        {
            if (name == null) { return; }
            string NameofFile = String.Concat(name, '.', Extension);
            FilePath = Path.Combine(FolderPath, NameofFile);
            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Close();
            }
        }
    }
}
