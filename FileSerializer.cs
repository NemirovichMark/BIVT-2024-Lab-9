using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        private string _folderpath;
        private string _filepath;
        public string FolderPath => _folderpath;
        public string FilePath => _filepath;
        public abstract string Extension { get; }
        public void SelectFolder(string path)
        {
            if (String.IsNullOrEmpty(path)) return;
            if (Directory.Exists(path) == false) Directory.CreateDirectory(path);
            _folderpath = path;
        }
        public void SelectFile(string name)
        {
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(Extension)) return;
            string _path = Path.Combine(FolderPath, $"{name}.{Extension}");
            if (File.Exists(_path) == false)
            {
                using (File.Create(_path)) { }
            }
            _filepath = _path;
        }
    }
}
