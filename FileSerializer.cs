using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        //Создать абстрактный класс FileSerializer, который
        //реализует интерфейс IFileManager, в файле FileSerializer.cs.
        //Свойства FolderPath и FilePath должны возвращать путь к выбранной
        //пользователем текущей папке и файлу соответственно.
        //Создать абстрактное свойство только для чтения Extension.
        //Создавать публичные конструкторы запрещено.
        //Реализовать метод void SelectFolder(string path), который получает
        //путь к папке (включая папку) и сохраняет его в FolderPath. Если такой папки
        //нет, то создать её.
        //Реализовать метод void SelectFile(string name), который получает имя файла
        //(без расширения) и ищет его в папке по пути FolderPath с расширением Extension
        //и устанавливает в FilePath путь к данному файлу. Если такого файла нет, то создать его.
        public string FolderPath { get; private set;} //получаем значение извне,
                                                     //изменять его можем только внутри класса
        public string FilePath { get; private set;}
        public abstract string Extension { get;}
        public void SelectFolder(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            if (!Directory.Exists(path)) //проверяем существует ли папка
            {
                Directory.CreateDirectory(path); //если папка не существует создаем ее
            }
            FolderPath = path;
        }

        public void SelectFile(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            if (string.IsNullOrEmpty(FolderPath))
            {
                return;
            }
            string FullPath = Path.Combine(FolderPath, $"{name}.{Extension}"); //объединяет строки в корректный
                                                                               //путь, правильно проставляя слеши
            if (!File.Exists(FullPath))
            {
                var newf= File.Create(FullPath); //создаем новый файл
                newf.Close(); //моментально закрываем файл чтобы можно было с ним работать
            }
            FilePath = FullPath;
        }
    }
}
