using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XML_Converter.Data.Models;

namespace XML_Converter.Data.Interfaces
{
    public interface IFiles
    {
        IEnumerable<FileModel> Files { get; }
        FileModel GetObjectFile(int fileId);

        void AddFile(FileModel file);
    }
}
