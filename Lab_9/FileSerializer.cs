using System.Formats.Asn1;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Lab_9 {
    public abstract class FileSerializer : IFileManager {
        public string FolderPath {get; private set; }
        public string FilePath {get; private set; }

        public abstract string Extension {get; }

        public void SelectFolder(string path) {
            if (string.IsNullOrEmpty(path)) 
                return;

            Directory.CreateDirectory(path);
            FolderPath = path;
        }


        public void SelectFile(string name) {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(FolderPath))
                return;

            string fileName = $"{name}.{Extension.Trim('.')}";
            string curFilePath = Path.Combine(FolderPath, fileName);
            
            if (!File.Exists(curFilePath)) {
                var fs = File.Create(curFilePath);
                fs.Close();
            }

            FilePath = curFilePath;
        }
    }
}