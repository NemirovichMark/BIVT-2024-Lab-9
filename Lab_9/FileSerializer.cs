using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Enumeration;

namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        public string FolderPath { get; private set; }
        public string FilePath { get; private set; }
        public abstract string Extension { get; }

        public void SelectFile(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(FolderPath)) return;

            string extension = Extension.Replace(".", "").Trim();
            string nameExt = Path.GetExtension(name);

            string fileName = !string.IsNullOrEmpty(nameExt) ? name : $"{name}.{extension}";
            string fullPath = Path.Combine(FolderPath, fileName);

            if (!File.Exists(fullPath))
                using (File.Create(fullPath)) { }

            FilePath = fullPath;
        }
        public void SelectFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            FolderPath = Path.GetFullPath(path);
        }

        /* Свойства FolderPath и FilePath должны возвращать путь к выбранной пользователем текущей 
        папке и файлу соответственно.
        
        Создавать публичные конструкторы запрещено.

        Реализовать метод void SelectFolder(string path), который получает 
        путь к папке (включая папку) и сохраняет его в FolderPath. Если такой папки нет, то создать её.

        Реализовать метод void SelectFile(string name), который получает имя файла (без расширения) 
        и ищет его в папке по пути FolderPath с расширением Extension и устанавливает в FilePath 
        путь к данному файлу. Если такого файла нет, то создать его.
        */
    }
}
