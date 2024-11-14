
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using SchoolDatabase.Models;

namespace SchoolDatabase.Controllers
{
    
    public class TeacherPageController : Controller
    {
        
        private readonly TeacherAPIController _api;

        public TeacherPageController(TeacherAPIController api)
        {
            _api = api;
        }
        public IActionResult List()
        {
            List<Teacher> Teachers = _api.ListTeachers();
            return View(Teachers); 
        }
        public IActionResult Show(int id)
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);

            //Teacher resul;

            foreach(Teacher t in _api.ListTeachers())
            {
                if (t.teacherid == id)
                    SelectedTeacher = t;
                else View("Not found");


            }

            return View(SelectedTeacher);
        }
    }
}
