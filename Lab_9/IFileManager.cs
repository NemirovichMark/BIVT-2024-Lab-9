using System.Formats.Asn1;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Lab_9 {
    public interface IFileManager {
        string FolderPath {get; }
        string FilePath {get; }

        void SelectFolder(string path);
        
        void SelectFile(string name);
    }
}