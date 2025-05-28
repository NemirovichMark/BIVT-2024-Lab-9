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
        private string _folderpath;
        private string _filepath;
        public string FolderPath => _folderpath;
        public string FilePath => _filepath;
        public abstract string Extension { get; }

        public void SelectFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            _folderpath = path;
        }

        public void SelectFile(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(FolderPath) || string.IsNullOrEmpty(Extension)) return;

            _filepath = Path.Combine(_folderpath, name + "." + Extension);

            if (!(File.Exists(_filepath)))
            {
                File.Create(_filepath).Close();
            }
        }
    }
}
