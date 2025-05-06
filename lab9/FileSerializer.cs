using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public abstract class FileSerializer : IFileManager{
        private string _folderPath;
        private string _filePath;
        private string _extension;

        public string FolderPath => _folderPath;
        public string FilePath => _filePath;
        public abstract string Extension{
            get;
        }

        public void SelectFile(string name){
            _filePath = Path.Combine(FolderPath, $"{name}.{Extension}");

            Directory.CreateDirectory(FolderPath); //create dir if not exist

            if (!File.Exists(_filePath)){
                using (File.Create(_filePath)){}; //auto-closer
            }
        }

        public void SelectFolder(string path){
            _folderPath = Path.GetFullPath(path); // delete double-slash in link

            Directory.CreateDirectory(_folderPath); // create dir if not exist

            _filePath = null; //change file_path if the folder path have been changed
        }
    }
}
