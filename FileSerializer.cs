using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    //path - путь, folder - папка
    public abstract class FileSerializer : IFileManager
    {
        private string _folderPath;
        private string _filePath;
        public string FolderPath { get { return _folderPath; } }
        public string FilePath { get { return _filePath; } }
        public abstract string Extension { get; }
        public void SelectFolder(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path); // создаю папку, если её нет
            _folderPath = path;
        }
        public void SelectFile(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return;
            if (string.IsNullOrWhiteSpace(_folderPath)) return;
            string fullName = $"{name}.{Extension}";
            _filePath = Path.Combine(_folderPath, fullName);
            if (!File.Exists(_filePath)) File.Create(_filePath).Close(); // создаю файл, если его нет
        }
    }
}
