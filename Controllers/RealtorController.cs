using HomeFinder.Data;
using HomeFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeFinder.Controllers
{
    public class RealtorController : Controller
    {
        public IActionResult Index(string id)
        {

            return View();
        }
        
    }
}
