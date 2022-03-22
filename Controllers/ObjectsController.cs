using HomeFinder.Data;
using HomeFinder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> IndexUserSavedObjects()

        {
          
            var user = await userManager.GetUserAsync(User);

            return View(GetObjects(user));
        }

        private List<PropertyObject> GetObjects(HomeFinderUser user)
        {
            NoticeOfInterest noticeOfInterest = new()
            {
                PropertyObject = new PropertyObject { Address = new Address { City = "Gävle", Country = "SWEDEN", StreetAddress = "Hejsangatan 13" }, ListPrice = 6000000 },
                User = user
            };
            
            context.NoticeOfInterests.Add(noticeOfInterest);
            context.SaveChanges();

           var tempObjects = context.NoticeOfInterests.Where(obj => obj.User == user).Include(obj => obj.PropertyObject).ThenInclude(prop=>prop.Images).ToList();
           
            List<PropertyObject> objects = new List<PropertyObject>();   

            foreach (var obj in tempObjects)
            {

                objects.Add(obj.PropertyObject);

            }

            objects[0].Images.Add(new HomeFinderImages { AltText = "blabla", Url = "https://cdn.pixabay.com/photo/2017/09/07/21/35/stilt-houses-2726812_960_720.jpg" });
            context.NoticeOfInterests.Remove(noticeOfInterest);
            context.SaveChanges();

            return objects;
        }
    }
}
