namespace Lab_9{
    public interface IFileManager{

        string FolderPath {get;}
        string FilePath {get;}

        public void SelectFile(string name);
        void SelectFolder(string path);
    }
}