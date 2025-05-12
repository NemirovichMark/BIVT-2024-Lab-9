using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        public string FolderPath { get; private set; }
        public string FilePath { get; private set; }
        public abstract string Extension { get;} //is readonly needed?
        public void SelectFile(string name)
        {
            string fileName = name + "." + Extension;
            string fullPath = Path.Combine(FolderPath, fileName);
            if (!File.Exists(fullPath))
            {
                File.Create(fullPath).Close();
            }
            FilePath = fullPath;
        }
        public void SelectFolder(string path)
        {
            Directory.CreateDirectory(path);
            FolderPath = path;
        }
    }
}
