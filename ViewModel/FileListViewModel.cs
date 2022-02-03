using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XML_Converter.Data.Models;

namespace XML_Converter.ViewModel
{
    public class FileListViewModel
    {
        public IEnumerable<FileModel> allFiles { get; set; }
    }
}
