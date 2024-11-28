
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
            return View(SelectedTeacher);
            ////Teacher resul;

            //foreach(Teacher t in _api.ListTeachers())
            //{
            //    if (t.teacherid == id)
            //        SelectedTeacher = t;
            //    else View("Not found");


            //}

            //return View(SelectedTeacher);
        }
        [HttpGet]
        public IActionResult New() {
            return View();
        }
        // POST: TeacherPage/New
        [HttpPost]
        public IActionResult Create(Teacher NewTeacher)
        {
            int TeacherId = _api.AddTeacher(NewTeacher);

            return RedirectToAction("Show", new { id = TeacherId });
            
        }

  

        // GET : TeacherPage/DeleteConfirm/{id}
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);
            return View(SelectedTeacher);
        }

        // POST: TeacherPage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int TeacherId = _api.DeleteConfirm(id);
            // redirects to list action
            return RedirectToAction("List");
        }

    

    }
}
