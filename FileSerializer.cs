using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_7;
namespace Lab_9
{
    
        public abstract class FileSerializer : IFileManager
        {
        public string FolderPath { get; private set; }
        public string FilePath { get; private set; }

        public abstract string Extension { get; }



        public void SelectFolder(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            FolderPath = path;
        }
        public void SelectFile(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(FolderPath))
                return;
            string fp = Path.Combine(FolderPath, $"{name}.{Extension}");
            if (!File.Exists(fp))
                File.Create(fp).Dispose();
            FilePath = fp;
        }
    }

    
}
