using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        private string _folderPath;
        private string _fileName;
        public string FolderPath => _folderPath;
        public string FilePath => _fileName;
        public abstract string Extension { get; }
        public void SelectFolder(string path)
        {
            if (path == null) return;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            _folderPath = path;
        }
        public void SelectFile(string name)
        {
            if (name == null) return;
            string path = Path.Combine(_folderPath, name + "." + Extension);
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            _fileName = path;
        }



    }
}
