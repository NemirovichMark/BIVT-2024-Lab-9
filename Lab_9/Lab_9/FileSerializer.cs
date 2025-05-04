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
            FolderPath = path;
            Directory.CreateDirectory(FolderPath);
        }
        public void SelectFile(string name) {
            string fileName = String.Concat(name, ".", Extension);
            FilePath = Path.Combine(FolderPath, fileName);
            string[] file = Directory.GetFiles(FolderPath, fileName);
            if (file == null || file.Length == 0)
                using (File.Create(FilePath)) ;
        }
        
    }
}
