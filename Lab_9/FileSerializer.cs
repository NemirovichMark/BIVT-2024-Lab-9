using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public abstract class FileSerializer: IFileManager
    {
        private string _folderPath;
        private string _filePath;
        private string _extension;
        public string FolderPath => _folderPath;
        public string FilePath => _filePath;

        public abstract string Extension {
            get;
        }

        public void SelectFolder(string folderPath)
        {
            _folderPath = folderPath;
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
        }
        public void SelectFile(string filePath)
        {
            _filePath = filePath;

            string path = $"{FolderPath}{FilePath}.{Extension}";
            if (!File.Exists(path)) {
                File.Create(path);
            }
        }
    }
}
