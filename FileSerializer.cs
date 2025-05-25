namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        protected string _folderPath;
        protected string _filePath;

        public abstract string Extension { get; }

        public virtual string FolderPath => _folderPath;
        public virtual string FilePath => _filePath;

        public virtual void SelectFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            _folderPath = path;
            _filePath = null;
        }

        public virtual void SelectFile(string name)
        {
            string fullPath = Path.Combine(FolderPath, $"{name}.{Extension}");
            _filePath = fullPath;

            if (!File.Exists(fullPath))
            {
                File.Create(fullPath).Close();
            }
        }
    }
}