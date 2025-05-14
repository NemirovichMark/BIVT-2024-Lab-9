namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        private string _folderPath, _filePath;

        public abstract string Extension { get; }

        string IFileManager.FolderPath => _folderPath;

        string IFileManager.FilePath => _filePath;

        void IFileManager.SelectFile(string name)
        {
            string path = (this as IFileManager).FilePath + name + '.' + this.Extension;
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            this._filePath = path;
        }

        void IFileManager.SelectFolder(string path)
        {
            this._folderPath = path;
            Directory.CreateDirectory(path);
        }
    }
}
