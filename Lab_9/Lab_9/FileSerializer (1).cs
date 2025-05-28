using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_7;

namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        public string FolderPath { get; private set; }//путь к папке
        public string FilePath { get; private set; }//путь к файлу

        public abstract string Extension { get; } //расширение файла

        public void SelectFolder(string path)
        {
            if (path == null) return;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path); //создаем папку
            }
            FolderPath = path;
        }
        public void SelectFile(string name)
        {
            if (name == null || FolderPath == null) return;
            string FileName = $"{name}.{Extension}"; //полное имя файла
            string FullPath = Path.Combine(FolderPath, FileName); //добавляем имя папки к пути, чтобы создать полное имя файла
            if (!File.Exists(FullPath))
            {
                var p = File.Create(FullPath);
                p.Close();//создание файла. важно его закрыть!!!
            }
            FilePath = FullPath;
        }
    }
}
