using Microsoft.AspNetCore.Mvc;
using SchoolDatabase.Models;

namespace SchoolDatabase.Controllers
{
    public class StudentPageController : Controller
    {

        private readonly StudentAPIController _api;

        public StudentPageController(StudentAPIController api)
        {
            _api = api;
        }
        public IActionResult List()
        {
            List<Student> Students = _api.ListStudents();
            return View(Students);
        }
        public IActionResult Show(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);

          

            foreach (Student s in _api.ListStudents())
            {
                if (s.studentid == id)
                    SelectedStudent= s;
                else View("Not found");


            }

            return View(SelectedStudent);
        }
    }
}
