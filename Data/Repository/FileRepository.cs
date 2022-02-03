using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XML_Converter.Data.Interfaces;
using XML_Converter.Data.Models;

namespace XML_Converter.Data.Repository
{
    public class FileRepository : IFiles
    {
        private readonly AppDbContent appDbContent;
        public FileRepository(AppDbContent appDbContent)
        {
            this.appDbContent = appDbContent;
        }
        public IEnumerable<FileModel> Files => appDbContent.file;

        public void AddFile(FileModel file)
        {
            appDbContent.file.Add(file);
            appDbContent.SaveChanges();
        }

        public FileModel GetObjectFile(int fileId) => appDbContent.file.FirstOrDefault(p => p.id == fileId);
    }
}
