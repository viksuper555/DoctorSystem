using DoctorSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoctorSystem.Controllers
{
    public class PostingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Post()
        {
            return View("CreatePost");
        }
        [HttpPost]
        public IActionResult Post(Posts post)
        {
            return View("Created", post);
        }

    }
}
