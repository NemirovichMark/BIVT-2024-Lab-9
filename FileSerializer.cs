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
        //поля
        protected string _folderPath;
        protected string _filePath;

        protected string _extension;

        //реализация свойств интерфейса
        public string FolderPath { get { return _folderPath; } }
        public string FilePath { get { return _filePath; } }

        //свойства 
        public abstract string Extension { get; }

        //создавать публичные конструкторы запрещено

        //реализация методов
        public void SelectFolder(string path)
        {
            if (path == null) return;

            _folderPath = path;
            //если папка НЕ существует 
            if (!Directory.Exists(path)) //Directory находится в пространстве имен System.IO
            {
                Directory.CreateDirectory(path);
            }

        }
        public void SelectFile(string name)
        {
            if (name == null || _folderPath == null) return;

            //создание директории (если она существует, то ничего не произойдет)
            //а-ля проверка
            Directory.CreateDirectory(_folderPath); //по идее она точно есть, но на всякий случай


            _filePath = Path.Combine(_folderPath, name + "." + Extension);

            //если такого файла нет 
            if (!File.Exists(_filePath))
            {
                using (FileStream fs = File.Create(_filePath))
                { }
                //поток закроется

            }
        }

    }
}
