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
        private string _folderPath;
        private string _filePath;

        public string FolderPath => _folderPath;
        public string FilePath => _filePath;

        public abstract string Extension { get; }

        public void SelectFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            _folderPath = Path.GetFullPath(path);
        }

        public void SelectFile(string name)
        {
            string fileName = $"{name}.{Extension}";
            string fullPath = Path.Combine(_folderPath, fileName);
            if (!File.Exists(fullPath))
            {
                File.Create(fullPath).Dispose();
            }
            _filePath = fullPath;
        }
    }
}
