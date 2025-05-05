using System;
using System.IO;

namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        public string FolderPath {get; private set; }
        public string FilePath {get; private set; }
        public abstract string Extension { get; }

        // методы
        public void SelectFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            FolderPath = Path.GetFullPath(path);
        }

        public void SelectFile(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(FolderPath)) return;

            string cleanExtension = Extension.Replace(".", "").Trim();

            string fileName;
            string nameExtension = Path.GetExtension(name);
            if (!string.IsNullOrEmpty(nameExtension))
            {
                fileName = name;
            }
            else
            {
                fileName = $"{name}.{cleanExtension}";
            }
            string fullPath = Path.Combine(FolderPath, fileName);

            if (!File.Exists(fullPath))
            {
                using (File.Create(fullPath)) { }
            }

            FilePath = fullPath;
        }
    }
}
