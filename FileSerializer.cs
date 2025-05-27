
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        private string _file_path;
        private string _folder_path;

        public string FolderPath => _folder_path;
        public string FilePath => _file_path;
        public abstract string Extension { get; }
        public void SelectFolder(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            Directory.CreateDirectory(path);
            _folder_path = path;
        }

        public void SelectFile(string name)
        {
            if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(_folder_path))
            {
                return;
            }
            string filename = $"{name}.{Extension.Trim('.')}";
            string current_path = Path.Combine(_folder_path, filename);

            if (!File.Exists(current_path))
            {
                var newfl = File.Create(current_path);
                newfl.Close();
            }

            _file_path = current_path;
        }
    }
}
