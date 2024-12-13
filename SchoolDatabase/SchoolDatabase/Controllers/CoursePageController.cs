
using Microsoft.AspNetCore.Mvc;
using SchoolDatabase.Models;


namespace SchoolDatabase.Controllers
{
    public class CoursePageController : Controller
    {

        private readonly CourseAPIController _api;

        public CoursePageController(CourseAPIController api)
        {
            _api = api;
        }
        public IActionResult List()
        {
            List<Course> Courses = _api.ListCourses();
            return View(Courses);
        }
        public IActionResult Show(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);

            return View(SelectedCourse);
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }
        // POST:CoursePage/Create

        [HttpPost]
        public IActionResult Create(Course NewCourse)
        {
            int CourseId = _api.AddCourse(NewCourse);

            return RedirectToAction("Show", new { id = CourseId });

        }

        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }

        // POST:CoursePage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int CourseId = _api.DeleteCourse(id);
            // redirects to list action
            return RedirectToAction("List");
        }

       
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }

        // POST: CoursePage/Update/{id}
        [HttpPost]
        public IActionResult Update(int id, string CourseName, string CourseCode, DateTime StartDate, DateTime FinishDate, int TeacherId, int CourseId)
        {
            Course UpdatedCourse = new Course();
            UpdatedCourse.coursename = CourseName;
            UpdatedCourse.coursecode = CourseCode;
            UpdatedCourse.startdate = StartDate;
            UpdatedCourse.finishdate = FinishDate;
            UpdatedCourse.teacherid = TeacherId;
            UpdatedCourse.courseid = CourseId;




            _api.UpdatedCourse(id, UpdatedCourse);
            return RedirectToAction("Show", new { id = id });
        }

    }
}
