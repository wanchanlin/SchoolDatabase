
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using SchoolDatabase.Models;

namespace SchoolDatabase.Controllers
{

    public class TeacherPageController : Controller
    {
        TeacherAPIController _api;

        public TeacherPageController(TeacherAPIController api)
        {
            _api = api;
        }
        public IActionResult Show()
        {
            List<Teacher> Teachers = _api.ListTeachers();
            return View(Teachers); 
        }
        public IActionResult List(int id)
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);
            return View(SelectedTeacher);
        }
    }
}
