using System;
using System.IO;

namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        private string _folderPath;
        private string _filePath;

        public string FolderPath
        {
            get { return _folderPath; }
        }

        public string FilePath
        {
            get { return _filePath; }
        }

        public abstract string Extension { get; }

        public void SelectFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) { return; }
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            _folderPath = Path.GetFullPath(path);
        }

        public void SelectFile(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(_folderPath)) { return; }
            string file = Path.Combine(_folderPath, name + "." + Extension);
            if (!File.Exists(file)) { File.Create(file).Close(); }
            _filePath = Path.GetFullPath(file);
        }
    }
}