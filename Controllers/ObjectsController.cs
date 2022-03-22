using HomeFinder.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeFinder.Controllers
{

    public class ObjectsController : Controller
    {
        private readonly UserManager<HomeFinderUser> userManager;

        public ObjectsController(UserManager<HomeFinderUser> userManager)
        {
            this.userManager = userManager;
        }




        public async Task<IActionResult> IndexUserSavedObjects(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(GetObjects(user));

        }

        private List<PropertyObject> GetObjects(HomeFinderUser user)
        {
            List<PropertyObject> objects = new();

            foreach (var noticeOfInterest in user.NoticeOfInterests)
            {
                objects.Add(noticeOfInterest.PropertyObject);
            }

            return objects;
        }
    }
}
