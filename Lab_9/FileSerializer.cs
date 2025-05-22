using Lab_9;

public abstract class FileSerializer : IFileManager // Публичный абстрактный класс-наследник.
{
    private string? _folderPath; // Приватное поле пути папки.
    private string? _filePath; // Приватное поле пути файла.
    public string? FolderPath => _folderPath; // Свойство пути папки.
    public string? FilePath => _filePath; // Свойство пути файла.
    public abstract string Extension { get; } // Абстрактное свойство расширения.
    public void SelectFolder(string name) // Метод, который получает путь к папке (включая папку) и сохраняет его.
    {
        if (string.IsNullOrWhiteSpace(name)) // Если путь пустой или состоит из символов разделителей, то
        {
            throw new ArgumentException("Путь не может быть пустым.", nameof(name)); // исключаем.
        }
        if (!Directory.Exists(name)) // Если папки с заданным именем не существует, то
        {
            Directory.CreateDirectory(name); // создаём её.
        }
        _folderPath = name; // Запоминаем путь папки.
    }
    public void SelectFile(string name) // Метод, который получает имя файла (без расширения) и ищет его в папке по пути FolderPath с расширением Extension и устанавливает в FilePath путь к данному файлу.
    {
        if (string.IsNullOrWhiteSpace(_folderPath)) // Если путь пустой или состоит из символов разделителей, то
        {
            throw new InvalidOperationException("Сначала необходимо выбрать папку."); // исключаем.
        }
        if (string.IsNullOrWhiteSpace(name)) // Если название пустое или состоит из символов разделителей, то
        {
            throw new ArgumentException("Имя файла не может быть пустым.", nameof(name)); // исключаем.
        }
        string fileName = name + Extension; // Запоминаем полное имя файла.
        string fullPath = Path.Combine(_folderPath, fileName); // Объединяем путь с названием файла.
        if (!File.Exists(fullPath))  // Если такого файла нет, то
        {
            File.Create(fullPath).Dispose(); // создаём файл по такому пути и закрываем этот файл.
        }
        _filePath = fullPath; // Сохраняем путь.
    }
}