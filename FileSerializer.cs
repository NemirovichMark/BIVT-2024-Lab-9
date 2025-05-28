using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_7;

namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        private string _folderPath;
        private string _filePath; 

        public string FolderPath => _folderPath; 
        public string FilePath => _filePath; 

        private string _extension;
        public abstract string Extension { get; }
        public void SelectFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            if (!Directory.Exists(path)) 
            {
                Directory.CreateDirectory(path); 
            }
            _folderPath = path;
        }

        public void SelectFile(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(Extension)) return;
            string fullPath = Path.Combine(FolderPath, name + "." + Extension);

            if (!File.Exists(fullPath))
            {
                FileStream path = File.Create(fullPath);
                path.Close();
            }
            _filePath = fullPath;
        }
    }
}
