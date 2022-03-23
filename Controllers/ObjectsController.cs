using HomeFinder.Data;
using HomeFinder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HomeFinder.Controllers
{

    public class ObjectsController : Controller
    {
        private readonly HomeFinderContext context;
        private readonly UserManager<HomeFinderUser> userManager;

        public ObjectsController(HomeFinderContext context,UserManager<HomeFinderUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }


        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);

            //Random r = new Random();
            //string[] Cities = new string[10] { "Skövde", "Stockholm","Malmö","Sandviken","Ystad","Trelleborg","Jönköping","Landskrona","Umeå","Gävle"};
            //string[] Adress = new string[10] { "Miitivägen 1", "Västerlånggatan 45", "Yxgatan 1", "Ulvsundavägen 6", "Peter Bomans gata 8", "Ingen gata 1", "Brovägen 5", "Vasagatan 33", "Kungsgatan 1", "Götgatan 45" };
            

           

            //NoticeOfInterest noticeOfInterest = new()
            //{
            //    PropertyObject = new PropertyObject { Address = new Address { City = Cities[r.Next(0,9)], Country = "SWEDEN", StreetAddress = Adress[r.Next(0,9)] }, ListPrice = r.Next(5000000,99999999) },
            //    User = user
            //};

            //context.NoticeOfInterests.Add(noticeOfInterest);
            //context.SaveChanges();


            //var tempObjects = context.NoticeOfInterests.Where(obj => obj.User == user).Include(obj => obj.PropertyObject).ThenInclude(prop => prop.Images).ToList();

            //List<PropertyObject> objects = new List<PropertyObject>();

            //foreach (var obj in tempObjects)
            //{

            //    objects.Add(obj.PropertyObject);

            //}

            //objects[objects.Count()-1].Images.Add(new HomeFinderImages { AltText = "blabla", Url = "https://cdn.pixabay.com/photo/2017/09/07/21/35/stilt-houses-2726812_960_720.jpg" });
            //context.PropertyFavorited.Add( new PropertyFavoritedByUser { PropertyObject = objects[objects.Count()-1], User = user });
            
            //context.SaveChanges();



            return View(user);
        }

        public async Task<IActionResult> RemoveLikedObject(int id)
        {
            var objToRemove = await context.PropertyFavorited.FirstOrDefaultAsync(lP => lP.PropertyObject.Id == id);
            if (objToRemove == null)
            {
                return NotFound();
            }
            context.PropertyFavorited.Remove(objToRemove);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveObjectOfInterest(int id)
        {
            var objToRemove = await context.NoticeOfInterests.FirstOrDefaultAsync(nI => nI.PropertyObject.Id == id);
            if (objToRemove == null)
            {
                return NotFound();
            }
            context.NoticeOfInterests.Remove(objToRemove);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
