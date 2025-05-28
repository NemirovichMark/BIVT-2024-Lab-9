using System;
using System.IO;

namespace Lab_9{
public abstract class FileSerializer : IFileManager
{
    public string FolderPath { get; private set; }
    public string FilePath { get; private set; }
    public abstract string Extension { get; }

    public void SelectFolder(string path){
    if (!Directory.Exists(path))
    {
        Directory.CreateDirectory(path);
    }
    FolderPath = Path.GetFullPath(path);
    }

    public void SelectFile(string n){
    string fn = $"{n}.{Extension}";
    FilePath = Path.Combine(FolderPath, fn);
    string[] f = Directory.GetFiles(FolderPath, fn);
    if (f == null || f.Length == 0)
        using (File.Create(FilePath)) ;
    }

}
}
