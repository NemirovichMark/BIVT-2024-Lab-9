namespace Lab_9 {
    public abstract class FileSerializer : IFileManager
    {
        private string _folderPath;
        private string _filePath;

        public string FolderPath => _folderPath;
        public string FilePath => _filePath;
        public abstract string Extension { get; }

        public void SelectFolder(string path) {
            _folderPath = path;
            if (!Directory.Exists(_folderPath)) {
                Directory.CreateDirectory(_folderPath);
            }
        }

        public void SelectFile(string name) {
            _filePath = Path.Combine(FolderPath, $"{name}.{Extension}");
            if (!File.Exists(_filePath)) {
                File.Create(_filePath).Close();
            }
        }
    }
}
