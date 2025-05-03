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
        public string FolderPath
        {
            get; private set;
        }

        public string FilePath
        {
            get; private set;
        }

        public void SelectFile(string name)
        {
            if (name == null || FolderPath == null) return;

            var filePath = Path.Combine(FolderPath,
                    String.Concat(name, ".", Extension));
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath)) { }
            }

            FilePath = filePath;
        }

        public void SelectFolder(string path)
        {
            if (path == null) return;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            FolderPath = path;
        }

        public abstract string Extension
        {
            get;
        }
    }
}
