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
        public abstract string Extension {  get; }
        public void SelectFolder(string path) //получает путь к папке (включая папку) и сохраняет его в FolderPath.
                                              //Если такой папки нет, то создать её.

        {
            if (string.IsNullOrEmpty(path)) return;
            if (Directory.Exists(path)==false) Directory.CreateDirectory(path);
            FolderPath = path;
        }
        public void SelectFile(string name) //получает имя файла (без расширения) и ищет его в папке по пути FolderPath
                                            //с расширением Extension и устанавливает в FilePath путь к данному файлу.
                                            //Если такого файла нет, то создать его.

        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(FolderPath)) return;

            string filePath=Path.Combine(FolderPath, $"{name}.{Extension}");

            if (File.Exists(filePath) == false) File.Create(filePath).Dispose();
            FilePath = filePath;
        }
    }
}
