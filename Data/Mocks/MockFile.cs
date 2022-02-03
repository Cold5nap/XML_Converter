using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XML_Converter.Data.Interfaces;
using XML_Converter.Data.Models;

namespace XML_Converter.Data.Mocks
{
    public class MockFile : IFiles
    {
        public IEnumerable<FileModel> Files
        {
            get
            {
                return new List<FileModel>
                {
                    new FileModel{id=1,name="first",path="fp"},
                    new FileModel { id = 2, name = "two", path = "sp" }
                };
            }
        }

        public void AddFile(FileModel file)
        {
            throw new NotImplementedException();
        }

        public FileModel GetObjectFile(int fileId)
        {
            throw new NotImplementedException();
        }
    }
}
