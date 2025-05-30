namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        public string FolderPath { get; private set; }
        public string FilePath { get; private set; }
        public abstract string Extension { get; }
        public void SelectFile(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(FolderPath)) return;
            string file = $"{name}.{Extension}";
            string filePath = Path.Combine(FolderPath, file);
            if (!File.Exists(filePath)) {
                var file_stream = File.Create(filePath);
                file_stream.Close();
            }
            FilePath = filePath;
        }
        public void SelectFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            Directory.CreateDirectory(path);
            FolderPath = path;
        }
    }
}