namespace Lab_9;

public abstract class FileSerializer : IFileManager
{
    private string _folderPath;
    private string _filePath;
    
    public string FolderPath => _folderPath;
    public string FilePath  => _filePath;
    public abstract string Extension { get; }
    
    public void SelectFolder(string path)
    {
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        _folderPath = path;
    }
    
    public void SelectFile(string name)
    {
        string fullPath = Path.Combine(FolderPath, $"{name}.{Extension}");
        if (!File.Exists(fullPath)) using (File.Create(fullPath)) { }
        _filePath = fullPath;
    }
    
    
}