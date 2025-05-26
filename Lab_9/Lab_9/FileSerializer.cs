using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public abstract class FileSerializer :  IFileManager  
    {
        public string FolderPath { get; private set; }
        public string FilePath { get; private set; }

        public abstract string Extension { get;  }   
        public void SelectFolder(string path)
        {
            if (path == null) return;
            if (!(Directory.Exists(path))) Directory.CreateDirectory(path );
            FolderPath  = path;
        }
        public void SelectFile(string name)
        {
            if (name == null) return;
            string file = Path.Combine(FolderPath,  String.Concat(name, ".",  Extension));
            if (!File.Exists(file))

                File.Create(file).Close();

            FilePath = file;
        }

    }
}
