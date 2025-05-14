using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9 {
    public abstract class FileSerializer : IFileManager {
        private string _folderPath;
        private string _filePath;
        private string _extension;
        public string FolderPath => _folderPath;
        public string FilePath => _filePath;
        public abstract string Extension {
            get;
        }
        public void SelectFolder(string path) {
            if (String.IsNullOrEmpty(path)) {return;}
            if (!(Directory.Exists(path))){
                Directory.CreateDirectory(path);
            }
            _folderPath = path;
        }
        public void SelectFile(string name) {
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(Extension)) {return;}
            string file_with_ext = name + "." + Extension;
            string full_path = Path.Combine(FolderPath, file_with_ext);
            if (!(File.Exists(full_path))){
                FileStream f = File.Create(full_path);
                f.Close();
            }
            _filePath = full_path;
        }
    }
}