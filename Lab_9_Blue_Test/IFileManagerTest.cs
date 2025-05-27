using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_9;

namespace Lab_9_Blue_Test
{
    internal class IFileManagerTest
    {
        private static readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static string GeneralPath => path;
        internal static bool Check_CreateFolder(IFileManager manager, string folderName)
        {
            if (Directory.Exists(Path.Combine(path, folderName)))
            {
                Directory.Delete(Path.Combine(path, folderName), true);
            }
            manager.SelectFolder(Path.Combine(path, folderName));
            return Directory.Exists(Path.Combine(path, folderName));
        }
        internal static bool Check_CreateFile(IFileManager manager, string folderName, string fileName, string extension)
        {
            if (File.Exists(Path.Combine(path, folderName, fileName)))
            {
                Directory.Delete(Path.Combine(path, folderName));
            }
            manager.SelectFolder(Path.Combine(path, folderName));
            manager.SelectFile(fileName);
            return Directory.Exists(Path.Combine(path, folderName)) && File.Exists(Path.Combine(path, folderName, String.Concat(fileName, ".", extension)));
        }
        internal static (string folder, string file) Check_Properties(IFileManager manager)
        {
            return (manager.FolderPath, manager.FilePath);
        }
    }
}
