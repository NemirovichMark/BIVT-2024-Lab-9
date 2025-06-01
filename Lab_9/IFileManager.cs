namespace Lab_9
{
    public interface IFileManager
    {
        public string FolderPath { get; }
        public string FilePath { get; }
        void SelectFile(string name) { }
        void SelectFolder(string path) { }
    }
}