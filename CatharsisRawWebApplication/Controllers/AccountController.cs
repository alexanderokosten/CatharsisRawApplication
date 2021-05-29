using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CatharsisRawWebApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CatharsisRawWebApplication.Domain;
using System;
using CatharsisRawWebApplication.Service;
using CatharsisRawWebApplication.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CatharsisRawWebApplication.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IHostingEnvironment Enviroment;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly AppDbContext context;
        private readonly DataManager dataManager;

        public AccountController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signinMgr, AppDbContext context, DataManager dataManager, IHostingEnvironment Enviroment)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            this.context = context;
            this.dataManager = dataManager;
            this.Enviroment = Enviroment;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Registration()
        {
          
            return View(new RegisterViewModel());
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Registration(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    IdentityUser user = new IdentityUser { UserName = model.UserName, Email = model.Email, NormalizedUserName = model.UserName.ToUpper(), NormalizedEmail = model.Email.ToUpper(), EmailConfirmed = true, PhoneNumber = model.PhoneNumber };

                    RegenerationStringMethod regenerationStringMethod = new RegenerationStringMethod();

                    user.Id = $"{regenerationStringMethod.RegenerationString()}-{regenerationStringMethod.RegenerationString()}-{regenerationStringMethod.RegenerationString()}";
                    IdentityUserRole<string> userRole = new IdentityUserRole<string> { RoleId = "44546e06-8719-4ad8-b88a-5557e12qrtyu", UserId = user.Id };
                    // добавляем пользователя
                    var result = await userManager.CreateAsync(user, model.Password);
                    context.UserRoles.Add(userRole);
                    context.SaveChanges();
                    if (result.Succeeded)
                    {


                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                return View(model);
            }
            catch
            {
                return RedirectToAction("ErrorsView", "Home");
            }

        }



        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginViewModel());
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(LoginViewModel.UserName), "Неверный логин или пароль");
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        SelectList selectListItems;
        [Authorize]
        public IActionResult OrderPersonalAccount()
        {
            selectListItems = new SelectList(context.ServiceItems.ToList(), "Title", "Description");
            ViewBag.selectListItems = selectListItems;
            return View();
        }
      
        [HttpPost]
        public IActionResult OrderPersonalAccount(UserOrder userOrder)
        {
            if (userOrder.PointDate == Convert.ToDateTime("0001-01-01"))
            {
                //доделать null
                ViewData["Message"] = "Выберите дату съёмки.";
                selectListItems = new SelectList(context.ServiceItems.ToList(), "Title", "Description");
                ViewBag.selectListItems = selectListItems;
                return View();
            }
            else
            {

                selectListItems = new SelectList(context.ServiceItems.ToList(), "Title", "Description");
                ViewBag.selectListItems = selectListItems;
                var id = this.User.Claims.First().Value;
                userOrder.idClient = context.Users.Find(id).Id.ToString();
                userOrder.PhoneNumber = context.Users.Find(id).PhoneNumber;

                List<ServiceItem> a = new List<ServiceItem>();
                a.AddRange(context.ServiceItems);
                for (int i = 0; i < a.Count; i++)
                {
                    if (userOrder.service.Title == a[i].Title)
                    {
                        userOrder.service = a[i];
                    }
                }
           
                int statusId = context.statusOrders.Find(1).StatusId;
                userOrder.statusId = statusId;
                context.UserOrders.Add(userOrder);
                context.SaveChanges();

                return RedirectToAction("PersonalAccount", "Home");
            }
        
        }

        public IActionResult OrderListPersonalAccount()
        {
          
            var model = context.UserOrders;
            return View(model);
        }

        public ActionResult Download(int id)
        {
            UserOrder userOrderCurrent = context.UserOrders.FirstOrDefault(p => p.IdUserOrder == id);
            
            if (userOrderCurrent.idClient == User.Claims.First().Value)
            {
                string wwwPath = this.Enviroment.WebRootPath;
                string contentPath = this.Enviroment.ContentRootPath;
                string path = Path.Combine(this.Enviroment.WebRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //СКАЧИВАНИЕ ФАЙЛА
                string filename = userOrderCurrent.FileRARName;
                string fullPath = Path.Combine(path, filename);
                return PhysicalFile(fullPath, "archive/rar", userOrderCurrent.FileRARName);


            }
            else
            {
                return RedirectToAction("OrderListPersonalAccount","Account");
            }

        }
        public async Task<IActionResult> MyProfilePage()
        {
          
                IdentityUser user = await context.Users.FirstOrDefaultAsync(p => p.Id == User.Claims.First().Value);
          
                return View(user);
            
          
        }
        public async Task<IActionResult> EditProfile()
        {
            IdentityUser user = await userManager.FindByIdAsync(User.Claims.First().Value);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, UserName = user.UserName,PhoneNumber=user.PhoneNumber };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.PhoneNumber = model.PhoneNumber;
                    user.NormalizedUserName = model.UserName.ToUpper();
                    user.Email = model.Email;
                    user.NormalizedEmail = model.Email.ToUpper();
                    user.UserName = model.UserName;
               
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("MyProfilePage","Account");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }


        public async Task<IActionResult> EditPasswordUser()
        {
            IdentityUser user = await userManager.FindByIdAsync(User.Claims.First().Value);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email, UserName = user.UserName, PhoneNumber = user.PhoneNumber };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPasswordUser(ChangePasswordViewModel model, string oldPassword, string newPassword)
        {
            if (oldPassword == null || newPassword == null)
            {
                ViewData["Message"] = "Заполните все поля";
                return View(model);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    IdentityUser user = await userManager.FindByIdAsync(model.Id);
                    if (user != null)
                    {
                        IdentityResult result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("MyProfilePage", "Account");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Пользователь не найден");
                    }
                }
                return View(model);
            }
          
        }


    }
}