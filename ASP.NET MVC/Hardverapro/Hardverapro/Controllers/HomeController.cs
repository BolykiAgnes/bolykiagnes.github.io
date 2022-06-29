using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hardverapro.Models;
using Microsoft.AspNetCore.Identity;
using Hardverapro.Data;
using Microsoft.AspNetCore.Authorization;

namespace Hardverapro.Controllers
{
    public class HomeController : Controller
    {
        UserManager<IdentityUser> userManager;
        RoleManager<IdentityRole> roleManager;
        ApplicationDbContext context;

        public HomeController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
        }

        public async Task<ActionResult> Init()
        {
            var first = userManager.Users.First();
            IdentityRole adminRole = new IdentityRole()
            {
                Name = "admin"
            };
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(adminRole);
                await userManager.AddToRoleAsync(first, "admin");
            }
            return RedirectToAction(nameof(Index));
        }

        //public async Task<ActionResult> Init2()
        //{
        //    await userManager.AddToRoleAsync(userManager.GetUserAsync(this.User).Result, "admin");
        //    return RedirectToAction(nameof(Index));
        //}

        public IActionResult Index()
        {
            return View(this.context.Advertisements.Where(x=>x.PictureData != null).AsEnumerable());
        }

        [Authorize(Roles = "admin")]
        public IActionResult Admin()
        {
            return View(this.context.Advertisements.OrderByDescending(a => a.CreationDate).AsEnumerable());
        }

        
        [HttpGet]
        [Authorize]
        public IActionResult NewAd()
        {
            ViewData["email"] = userManager.GetUserAsync(this.User).Result.Email;
            ViewData["shipMethods"] = new string[]
            {
                "By Courier",
                "Foxpost",
                "In Person"
            };
            return View();
        }

        
        [HttpPost]
        [Authorize]
        public IActionResult NewAd(Advertisement newAd, string shipmethod)
        {
            newAd.UID = Guid.NewGuid().ToString();
            var myself = this.User;
            var user_obj = userManager.GetUserAsync(myself);
            newAd.Creator = user_obj.Result;
            newAd.CreationDate = DateTime.Now;
            string contenttype = newAd.PictureFormData?.ContentType;
            if (contenttype != null)
            {
                newAd.PictureData = new byte[newAd.PictureFormData.Length];
                using (var stream = newAd.PictureFormData.OpenReadStream())
                {
                    stream.Read(newAd.PictureData, 0, newAd.PictureData.Length);
                }

            }
            newAd.ContentType = contenttype;



            newAd.ShipMethod = shipmethod;

            string email = user_obj.Result.Email;
          
            newAd.Email = email;
            

            this.context.Advertisements.Add(newAd);
            this.context.SaveChanges();
            return RedirectToAction(nameof(ListAds));
        }

        public IActionResult ListAds()
        {
            if (this.User.Identity != null && this.User.Identity.IsAuthenticated)
            {
                ViewData["isStudent"] = (userManager.GetUserAsync(this.User).Result as SiteUser).IsStudent;
            }
            else
            {
                ViewData["isStudent"] = false;
            }
            return View(this.context.Advertisements.OrderByDescending(a => a.CreationDate).AsEnumerable());
        }

        public IActionResult ListAds2()
        {
            if (this.User.Identity != null && this.User.Identity.IsAuthenticated)
            {
                ViewData["isStudent"] = (userManager.GetUserAsync(this.User).Result as SiteUser).IsStudent;
            }
            else
            {
                ViewData["isStudent"] = false;
            }

            return View(this.context.Advertisements.OrderByDescending(a => a.CreationDate).AsEnumerable());
        }

        [Authorize(Roles = "admin")]
        public IActionResult DeleteAd(string uid)
        {
            var adToDelete = this.context.Advertisements.FirstOrDefault(a => a.UID == uid);
            this.context.Advertisements.Remove(adToDelete);
            this.context.SaveChanges();
            return RedirectToAction(nameof(Admin));
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(string uid)
        {
            ViewData["shipMethods"] = new string[]
            {
                "By Courier",
                "Foxpost",
                "In Person"
            };
            var ad = this.context.Advertisements.Single(a => a.UID == uid);
            return View(ad);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(Advertisement newAd)
        {
            var ad = this.context.Advertisements.Single(a => a.UID == newAd.UID);
            ad.City = newAd.City;
            ad.Email = newAd.Email;
            ad.Name = newAd.Name;
            ad.Price = newAd.Price;
            ad.ShipMethod = newAd.ShipMethod;

            if (newAd.PictureFormData != null)
            {
                string contentType = newAd.PictureFormData?.ContentType;
                if (contentType != null)
                {
                    ad.PictureData = new byte[newAd.PictureFormData.Length];
                    using(var stream = newAd.PictureFormData.OpenReadStream())
                    {
                        stream.Read(ad.PictureData, 0, ad.PictureData.Length);
                    }
                    ad.ContentType = contentType;
                }
            }
            this.context.SaveChanges();
            return RedirectToAction(nameof(Admin));

        }

        public IActionResult Privacy()
        {
            return View();
        }

        public FileContentResult GetPictureData(string uid)
        {
            var ad = this.context.Advertisements.FirstOrDefault(a => a.UID == uid);
            if (ad.PictureData == null || ad.PictureData.Length < 10)
            {
                return null;
            }
            return new FileContentResult(ad.PictureData, ad.ContentType);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
