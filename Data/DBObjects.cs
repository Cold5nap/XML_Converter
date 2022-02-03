using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XML_Converter.Data.Models;

namespace XML_Converter.Data
{
    public class DBObjects
    {
        public static void Initial(AppDbContent content)
        {
            //content.SaveChanges();
        }
    }
}
