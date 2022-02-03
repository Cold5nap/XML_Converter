using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XML_Converter.Data.Models;

namespace XML_Converter.Data
{
    public class AppDbContent : DbContext
    {
        public AppDbContent(DbContextOptions<AppDbContent> options) : base(options)
        {
        }

        public DbSet<FileModel> file { get; set; }
        
    }
}
