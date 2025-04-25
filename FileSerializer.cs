namespace Lab_9
{
    public abstract class FileSerializer : IFileManager{
        public string FolderPath { get; private set; }
        public string FilePath { get; private set; }
        public abstract string Extension { get; }

        public void SelectFolder(string path){
            if (path == null) return;
             
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            
            FolderPath = path;
        }

        public void SelectFile(string name){
            if (name == null || FolderPath == null) return;
            
            var fullPath = Path.Combine(FolderPath, $"{name}.{Extension}");
            
            if (!File.Exists(fullPath)) File.Create(fullPath).Close();
            
            FilePath = fullPath;
        }
    }
}