using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_7;

namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        public string FolderPath { get; private set; }
        public string FilePath { get; private set; }
        public abstract string Extension { get; }

        public void SelectFolder(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            FolderPath = path;
        }
        
        public void SelectFile(string name)
        {
            string path = Path.Combine(FolderPath, name + "." + Extension);
            if (!File.Exists(path)) File.Create(path).Close();
            FilePath = path;
        }
    }
}
