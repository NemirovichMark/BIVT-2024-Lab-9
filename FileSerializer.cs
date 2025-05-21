using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {

   
        public string FolderPath { get; private set; }
        public string FilePath { get; private set; }
        public abstract string Extension { get; }


        public void SelectFile(string name)
        {
           

            var folder = FolderPath;
            var file_name = $"{name}.{Extension}";

            var path = Path.Combine(FolderPath, file_name);

            if (!File.Exists(path))
            {
                using (File.Create(path)) { }
            }
            FilePath = path;
        }
        public void SelectFolder(string path)
        {
            

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FolderPath = path;
        }
    }
}