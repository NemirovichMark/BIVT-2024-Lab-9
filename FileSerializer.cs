using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;


namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        private string _path_to_folder;
        private string _path_to_file;
        public string FolderPath => _path_to_folder;
        public string FilePath => _path_to_file;
        public void SelectFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            _path_to_folder = path;
        }
        public void SelectFile(string name)
        {
            if (string.IsNullOrEmpty(_path_to_folder)) return;
            if (string.IsNullOrEmpty(name)) return;
            string path_to_name = $"{name}.{Extension}";
            string path_to_file =
                Path.Combine(_path_to_folder, path_to_name);
            if (!File.Exists(path_to_name))
            {
                FileStream path = File.Create(path_to_file);
                path.Close();
            }
            _path_to_file = path_to_file;
        }
        public abstract string Extension { get; }
    }
}
