using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public abstract class FileSerializer : iFileManager{
        private string _folderPath;
        private string _filePath;
        private string _extension;

        public string FolderPath => _folderPath;
        public string FilePath => _filePath;
        public abstract string Extension{
            get;
        }

        public void SelectFolder(string path){
            _folderPath = path;
            if (!Directory.Exists(_folderPath)){
                Directory.CreateDirectory(_folderPath);
            }
        }

        void SelectFile(string name){
            _filePath = name;
            string full_path = Path.Combine(_folderPath, _filePath);
            if (!File.Exists(_filePath)){
                File.Create(_filePath);
            }
        }
    }
}