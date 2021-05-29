using Microsoft.AspNetCore.Mvc;
using CatharsisRawWebApplication.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace CatharsisRawWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataManager dataManager;
        private readonly AppDbContext context;

        public HomeController(DataManager dataManager, AppDbContext context)
        {
            this.dataManager = dataManager;
            this.context = context;
        }

        public IActionResult Index()
        {
            return View(dataManager.TextFields.GetTextFieldByCodeWord("PageIndex"));
        }

        public IActionResult Contacts()
        {
            return View(dataManager.TextFields.GetTextFieldByCodeWord("PageContacts"));
        }
        public IActionResult PersonalAccount()
        {
            return View();
        }
      
        public IActionResult ErrorsView()
        {
            return View();
        }
     
    }
}