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
        private string _folderPath; //поле хранит путь к папке, в которой будут находиться файлы.
        private string _filePath; // хранится полный путь к выбранному файлу

        public string FolderPath => _folderPath; // свойство предоставляет доступ только для чтения к переменной _folderPath
        public string FilePath => _filePath; //свойство предоставляет доступ только для чтения к переменной _filePath

        private string _extension; // хранение расширения файла
        public abstract string Extension { get; }

        // метод для выбора папки
        public void SelectFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            if (!Directory.Exists(path)) // метод позволяет проверить, существует ли указанный каталог (папка) в файловой системе
            {
                Directory.CreateDirectory(path); // создание папки
            }
            _folderPath = path;
        }

        // метод для выбора файла
        public void SelectFile(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(Extension)) return;
            // проверяем наличие файла в заданной папке по заданному имени и расширению
            string fullPath = Path.Combine(FolderPath, name + "." + Extension);

            if (!File.Exists(fullPath))
            {
                // если файл не существует, создаём его
                FileStream path = File.Create(fullPath);
                path.Close(); // закрывается поток, чтобы избежать блокировки файла
            }
            _filePath = fullPath;
        }
    }
}
