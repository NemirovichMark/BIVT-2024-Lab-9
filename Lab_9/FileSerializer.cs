using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        private string folderPath = "123";
        private string filePath;

        public abstract string Extension { get; }

        public string FolderPath => folderPath;
        public string FilePath => filePath;

        protected FileSerializer() { } 

        public void SelectFolder(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return;
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            folderPath = path;
            
        }

        public void SelectFile(string path)
        {
            Console.WriteLine("1");
            if (String.IsNullOrEmpty(Extension) || String.IsNullOrEmpty(path))
            {
                return;
            }
            

            var fileName = $"{path}.{Extension}";
            Console.WriteLine(fileName);
            filePath = Path.Combine(folderPath, fileName);
            Console.WriteLine(filePath);

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            Console.Write("Select File: ");
            Console.WriteLine(filePath);
            this.filePath = filePath;


        }
    }
}
