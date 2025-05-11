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
        public abstract string Extension { get; }
        public string FolderPath { get; private set; }//хранит  путь к папке, можно читать извне, а изменять только внутри класса
        public string FilePath { get; private set; }//хранит путь к файлу
        //Реализовать метод void SelectFolder(string path),
        //который получает путь к папке (включая папку) и сохраняет его в FolderPath.
        //Если такой папки нет, то создать её.

        public void SelectFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FolderPath = path;
        }
        //Реализовать метод void SelectFile(string name), который получает имя файла (без расширения)
        //и ищет его в папке по пути FolderPath с расширением Extension и устанавливает в FilePath путь к данному файлу.
        //Если такого файла нет, то создать его.

        public void SelectFile(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(FolderPath)) return;
            string FileName = $"{name}.{Extension}"; //полное имя файла
            string FullPath = Path.Combine(FolderPath, FileName);
            if (!File.Exists(FullPath))
            {
                var p = File.Create(FullPath);
                p.Close();
            }
            FilePath = FullPath;
        }
    }
}
