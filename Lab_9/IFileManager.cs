namespace Lab_9
{
    public interface IFileManager
    {
        
        public string FolderPath { get; }
        public string FilePath { get; }
        
        public abstract string Extension { get; }

        public void SelectFile(string name) {}
        public void SelectFolder(string path) {}
    }
}