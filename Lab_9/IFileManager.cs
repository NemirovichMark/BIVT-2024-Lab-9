using System;
using System.IO;

namespace Lab_9{
public interface IFileManager
{
    string FolderPath { get; }
    string FilePath { get; }

    void SelectFolder(string path);
    void SelectFile(string name);
}
}
