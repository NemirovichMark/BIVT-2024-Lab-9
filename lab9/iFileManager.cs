using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_9
{
    public interface iFileManager
    {
        public string FolderPath{
            get;
        }
        public string FilePath{
            get;
        }

        void SelectFile(string name){}
        void SelectFolder(string path){}
    }
}