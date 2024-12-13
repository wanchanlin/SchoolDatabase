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
        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        // POST: StudentPage/Create
        [HttpPost]
        public IActionResult Create(Student NewStudent)
        {
            int StudentId = _api.AddStudent(NewStudent);

            return RedirectToAction("Show", new { id = StudentId });

        }
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);
        }

        // POST: StudentPage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int StudentId = _api.DeleteConfirm(id);
            // redirects to list action
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);
        }

        // POST: StudentPage/Update/{id}
        [HttpPost]
        public IActionResult Update(int id, string StudentFName, string StudentLName, string StudentNumber, DateTime EnrolDate)
        {
            Student UpdatedStudent = new Student();
            UpdatedStudent.studentfname = StudentFName;
            UpdatedStudent.studentlname = StudentLName;
            UpdatedStudent.studentnumber = StudentNumber;
            UpdatedStudent.enroldate = EnrolDate;

            _api.UpdatedStudent(id, UpdatedStudent);
            return RedirectToAction("Show", new { id = id });
        }
    }
}
