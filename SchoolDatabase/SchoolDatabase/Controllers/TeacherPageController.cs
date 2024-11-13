
using Microsoft.AspNetCore.Mvc;

namespace SchoolDatabase.Controllers
{
    public class TeacherPageController : Controller
    {
        public IActionResult Show()
        {
            return View(); 
        }
        public IActionResult List()
        {
         
            return View();
        }
    }
}
