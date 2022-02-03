using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XML_Converter.Data.Interfaces;
using XML_Converter.ViewModel;
using Microsoft.AspNetCore.Hosting;
using XML_Converter.Data;
using XML_Converter.Data.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Xsl;
using System.Xml;
using System;
using System.Xml.XPath;

namespace XML_Converter.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFiles _allFiles;
        AppDbContent _context;
        IWebHostEnvironment _appEnvironment;
        public HomeController(IFiles iFiles, AppDbContent dbContent, IWebHostEnvironment webHostEnvironment)
        {
            _allFiles = iFiles;
            _context = dbContent;
            _appEnvironment = webHostEnvironment;

        }

        public ViewResult Index()
        {
            ViewBag.Title = "Страница с файлами.";
            FileListViewModel obj = new FileListViewModel();
            obj.allFiles = _allFiles.Files;
            return View(obj);
        }

        public ViewResult Notification()
        {
            ViewBag.Notification = "Файл с данным названием уже загружен.";
            return View();
        }
        public static void TransformXMLToHTML(string pathToHtml, string inputXml, string xsltString)
        {
            XPathDocument myXPathDoc = new XPathDocument(inputXml);
            XslCompiledTransform myXslTrans = new XslCompiledTransform();
            myXslTrans.Load(xsltString);
            XmlTextWriter myWriter = new XmlTextWriter(pathToHtml, null);
            myXslTrans.Transform(myXPathDoc, null, myWriter);
            myWriter.Close();
        }

        public IActionResult Download(string fileName)
        {
            string xml = _appEnvironment.WebRootPath + $"\\Files\\{fileName}.xml";
            string xslt = _appEnvironment.WebRootPath + "\\Files\\Stylesheets_1.xslt";
            string pathToHtml = $"{_appEnvironment.WebRootPath}\\Files\\Transform\\{fileName}.html";
            TransformXMLToHTML(pathToHtml, xml, xslt);
            var fi = new FileInfo(pathToHtml);
            // Обновляем информацию о запрошенном файле.
            fi.Refresh();
            if (fi.Exists == true)
            {
                var bytes = System.IO.File.ReadAllBytes(pathToHtml);
                return new FileContentResult(bytes, "text/html")
                {
                    FileDownloadName = fileName+".html"
                };
            }
            else
            {
                return new EmptyResult();
            }
        }

        public async Task<ActionResult> Save(string text,string path)
        {
            ViewBag.Title = "Редактирование";
            using (StreamWriter sr = new StreamWriter(path))
            {
                await sr.WriteAsync(text);
                sr.Close();
            }
            return RedirectToAction("Index");
        }

        public async Task<ViewResult> Edit(string filePath)
        {
            ViewBag.Title = "Редактирование";
            ViewBag.Path = filePath;
            using (StreamReader sr = new StreamReader(filePath))
            {
                ViewBag.Raw = await sr.ReadToEndAsync();
                sr.Close();
            }
            return View();
        }

        public async Task<ViewResult> Select(string fileName)
        {
            // чтение из файла
            string xml = _appEnvironment.WebRootPath + $"\\Files\\{fileName}.xml";
            string xslt = _appEnvironment.WebRootPath + "\\Files\\Stylesheets_1.xslt";
            string pathToHtml = $"{_appEnvironment.WebRootPath}\\Files\\Transform\\{fileName}.html";
            ViewBag.Title = "Содержимое файла"; 
            TransformXMLToHTML(pathToHtml, xml, xslt);
            //преобразуем файл если не был преобразован
            if (!System.IO.File.Exists(pathToHtml))
            {
                TransformXMLToHTML(pathToHtml, xml, xslt);
            }
            //считываем файл и отображаем в представлении
            using (StreamReader sr = new StreamReader(pathToHtml))
            {
                ViewBag.Raw = await sr.ReadToEndAsync();
                sr.Close();
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                if (_context.file.Any(f => f.name == uploadedFile.FileName.Replace(".xml", "")))
                {
                    return RedirectToAction("Notification");
                }
                // путь к папке Files
                string path0 = _appEnvironment.WebRootPath + "\\Files\\" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(path0, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                    fileStream.Close();
                }
                FileModel file = new FileModel { name = uploadedFile.FileName.Replace(".xml", ""), path = path0 };
                _context.file.Add(file);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }



        [HttpPost]
        public ActionResult DeleteFile(int id)
        {
            FileModel f = _context.file.Find(id);

            if (System.IO.File.Exists(f.path))
            {
                System.IO.File.Delete(f.path);
            }

            if (f != null)
            {
                _context.file.Remove(f);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }

}
