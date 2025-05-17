using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab_9
{
    abstract public class FileSerializer : IFileManager
    {
        public string FolderPath { get; private set; }

        public string FilePath { get; private set; }

        abstract public string Extension { get; }

        public void SelectFolder(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            FolderPath = path;
        }

        public void SelectFile(string name)
        {
            if ((string.IsNullOrWhiteSpace(name)) || (string.IsNullOrWhiteSpace(FolderPath))) return;
            name = name + "." + Extension;
            if (!File.Exists(FolderPath + name)) File.Create(name);
            FilePath = FolderPath + name;
        }
    }
}
