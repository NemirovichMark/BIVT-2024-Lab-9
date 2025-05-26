namespace Lab_9
{
    public abstract class FileSerializer : IFileManager
    {
        public string FilePath { get; private set; }
        public string FolderPath { get; private set; }

        public abstract string Extension { get; }

        public void SelectFile(string name)
        {
            string path = Path.Combine(FolderPath, $"{name}.{Extension}");
            if (!File.Exists(path))
            {
                var myFile = File.Create(path);
                myFile.Close();
            }
            this.FilePath = path;
        }

        public void SelectFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            this.FolderPath = path;
        }


    }
}
