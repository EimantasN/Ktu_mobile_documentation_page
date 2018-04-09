using KTU_Mobile_Data;
using Ktu_Mobile_Docs.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Ktu_Mobile_Docs.Controllers
{
    public class HomeController : Controller
    {
        private readonly Data _context;

        public HomeController(Data context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            Main_page info = new Main_page();
            info = _context.Main_page.FirstOrDefault();
            var model = new MainPage
            {
                Git_hub_link = info.Git_hub_link,
                Gmail = info.Gmail,
                Google_store_app = info.Google_store_app,
                Memebers = _context.Members
            };
            return View(model);
        }

        public IActionResult Doc()
        {
            var model = new Docs
            {
                info = _context.Categorys
            };
            return View(model);
        }
    }
}