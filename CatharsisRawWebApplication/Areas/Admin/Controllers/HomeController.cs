using Microsoft.AspNetCore.Mvc;
using CatharsisRawWebApplication.Domain;
using CatharsisRawWebApplication.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System;
using Microsoft.Extensions.Configuration;
using CatharsisRawWebApplication.Service;

namespace CatharsisRawWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        [System.Obsolete]
        private IHostingEnvironment Enviroment;
        private readonly DataManager dataManager;
        private readonly AppDbContext context;
        private readonly IConfiguration _configuration;

        [System.Obsolete]
        public HomeController(DataManager dataManager, AppDbContext context, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            this.dataManager = dataManager;
            this.context = context;
            this.Enviroment = hostingEnvironment;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View(dataManager.ServiceItems.GetServiceItems());
        }
        public IActionResult OrderListPersonalAccount()
        {
            var model = context.UserOrders;
            return View(model);
        }
        public IActionResult ClosedOrderList()
        {       
            var model = context.UserOrders;
            return View(model);
        }
        public IActionResult PendingOrdersList()
        { 
            var model = context.UserOrders;
            return View(model);
        }

        private string filename;
        [HttpPost]
        [DisableRequestSizeLimit]
        [System.Obsolete]
        public IActionResult OrderListPersonalAccount(int id, string accept, string closing, string photoInTreatment, string CloseOrder,string DownLoadFile, List<IFormFile> postedFiles, string currentData)
        {
            string wwwPath = this.Enviroment.WebRootPath;
            string contentPath = this.Enviroment.ContentRootPath;
            string path = Path.Combine(this.Enviroment.WebRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (IFormFile postedFile in postedFiles)
            {

                string extension = Path.GetExtension(postedFile.FileName);
                filename = Guid.NewGuid().ToString() + extension;
                using (FileStream stream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                {
                  
                    postedFile.CopyTo(stream);
                }
            }
            if (!string.IsNullOrEmpty(accept))
            {
                UserOrder userOrderCurrent = context.UserOrders.FirstOrDefault(p => p.IdUserOrder == id);
                userOrderCurrent.statusId = 3; //Принял

                if (currentData != null)
                {
                    userOrderCurrent.PointDate = Convert.ToDateTime(currentData);
                }
               
                context.SaveChanges();
                return View();
            }
            if (!string.IsNullOrEmpty(closing))
            {
                UserOrder userOrderCurrent = context.UserOrders.FirstOrDefault(p => p.IdUserOrder == id);
                userOrderCurrent.statusId = 2; //Отклонил
                context.SaveChanges();
                return View();
            }
            if (!string.IsNullOrEmpty(photoInTreatment))
            {

                UserOrder userOrderCurrent = context.UserOrders.FirstOrDefault(p => p.IdUserOrder == id);
                userOrderCurrent.statusId = 4; //Фото в обработке
              
                
                context.SaveChanges();
                return View();
            }
            if (!string.IsNullOrEmpty(CloseOrder))
            {
                if (postedFiles.Count == 0)
                {
                    ViewBag.Message += string.Format("Загрузите файл.");
                }
                else
                {
                    UserOrder userOrderCurrent = context.UserOrders.FirstOrDefault(p => p.IdUserOrder == id);
                    userOrderCurrent.statusId = 5; //Завершено
                    foreach (IFormFile postedFile in postedFiles)
                    {
                        
                        userOrderCurrent.FileRARName = filename;
                    }

                    context.SaveChanges();
                    return View();
                }

            
            }
            if (!string.IsNullOrEmpty(DownLoadFile))
            {


                UserOrder userOrderCurrent = context.UserOrders.FirstOrDefault(p => p.IdUserOrder == id);

                Download(userOrderCurrent.IdUserOrder);
               
             
            }
            return View();
           
        }
        [DisableRequestSizeLimit]
        public ActionResult Download(int id)
        {
            UserOrder userOrderCurrent = context.UserOrders.FirstOrDefault(p => p.IdUserOrder == id);
          
                string wwwPath = this.Enviroment.WebRootPath;
                string contentPath = this.Enviroment.ContentRootPath;
                string path = Path.Combine(this.Enviroment.WebRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //СКАЧИВАНИЕ ФАЙЛА

                filename = userOrderCurrent.FileRARName;
                string fullPath = Path.Combine(path, filename);
                return PhysicalFile(fullPath, "archive/rar", userOrderCurrent.FileRARName);


         

        }
    
    }
}