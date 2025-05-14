using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        protected FileSerializer() { }

        public abstract string Extension { get; }

        public string FolderPath { get; private set; }
        public string FilePath { get; private set; }

        public void SelectFolder(string folderPath) {

            if(string.IsNullOrWhiteSpace(folderPath))
                throw new ArgumentNullException("wrong folder path!");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            FolderPath = folderPath;
            FilePath = null;

        }
        public void SelectFile(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name was null or white space");
            if (string.IsNullOrWhiteSpace(FolderPath))
                throw new InvalidOperationException("Folder must be selected");
            string fullName = $"{name}.{Extension}";
            string fullPathToFile = Path.Combine(FolderPath, fullName);

            if (!File.Exists(fullPathToFile))
                File.Create(fullPathToFile).Close();
            FilePath = fullPathToFile;

        }
    }
}
