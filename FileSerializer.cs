using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        private string _filePath;
        private string _folderPath;
        public string FilePath => _filePath;
        public string FolderPath => _folderPath;
        public abstract string Extension{ get; }
        public void SelectFolder(string path)
        {
            if (path == null) return;
            _folderPath = path;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        public void SelectFile(string name)
        {
            if (_folderPath == null || name == null) return;
            var path = Path.Combine(FolderPath, name + "." + Extension);
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            _filePath = path;
        }
    }
}