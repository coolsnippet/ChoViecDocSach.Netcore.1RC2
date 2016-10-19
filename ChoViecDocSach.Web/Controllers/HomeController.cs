using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Onha.Kiet;

namespace ChoViecDocSach.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {  
            return View();
        }

        //http://stackoverflow.com/questions/5826649/returning-a-file-to-view-download-in-asp-net-mvc

        // I have file not found issue
        // here is a work around
        // https://github.com/aspnet/Mvc/issues/5053
        public IActionResult GetKindleFile()
        {
            var thuvienhoasen = new ThuVienHoaSen();         
            var bookHelper = new BookHelper(thuvienhoasen);
            bookHelper.DownloadFolder = AppContext.BaseDirectory; // in case web app cannot access to a file outside of its folder
            var firstUrlPath = @"/a17221/ban-do-tu-phat";
            // var firstUrlPath = @"/p27a10044/1/bai-van-khuyen-phat-tam-bo-de";            
            var kindleFile = bookHelper.CreateKindleFiles(firstUrlPath);
            var fileContent = new System.IO.FileStream(kindleFile, System.IO.FileMode.Open);
            return File(fileContent, "application/octet-stream", System.IO.Path.GetFileName(kindleFile));          
            
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
