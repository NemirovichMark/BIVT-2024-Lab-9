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
        public void SelectFile(string name)
        {
            if (name == null || FolderPath == null) return;
            var path = Path.Combine(FolderPath, $"{name}.{Extension}");
            if (File.Exists(path) == false)
            {
                File.Create(path).Dispose();
            }
            FilePath = path;
        }
        public void SelectFolder(string path)
        {
            if (path == null) return;
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            FolderPath = path;
        }
    }
}
