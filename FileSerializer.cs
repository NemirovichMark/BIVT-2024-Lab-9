using lab_9;
using Microsoft.SqlServer.Server;
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
        { get; private set; }
        public string FilePath 
        { get; private set; }
        public abstract string Extension 
        { get; }

        public void SelectFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FolderPath = path;
        }
        public void SelectFile(string name)
        {
            if (name == null || name.Length == 0)
            {
                return;
            }
            string fileName = $"{name}.{Extension}";
            string FolderPath_fileName = Path.Combine(FolderPath, fileName);
            FilePath = FolderPath_fileName;

            if (!File.Exists(FolderPath_fileName))
            {
                var file = File.Create(FolderPath_fileName);
                file.Close();
            }
        }
    }
}