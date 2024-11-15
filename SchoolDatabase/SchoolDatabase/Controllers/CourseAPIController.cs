using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolDatabase.Models;
using System;
using MySql.Data.MySqlClient;


namespace SchoolDatabase.Controllers
{
    // API controller to manage course data in the school database
    [Route("api/Course")]
    [ApiController]
    public class CourseAPIController : ControllerBase
    {
        // Dependency injection of the database context
        private readonly SchoolDbContext _context;

        public CourseAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route(template: "ListCourses")]
        public List<Course> ListCourses()
        {
            // Initialize an empty list to hold course data
            List<Course> Courses = new List<Course>();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                // SQL query to select all courses
                MySqlCommand Command = Connection.CreateCommand();
                string query = "select * from courses";

                // SQL QUERY
                Command.CommandText = query;
                Command.Prepare();

                // Execute the query and read each course record
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // Map each database column to the Course object's properties
                    while (ResultSet.Read())
                    {
                        int CourseId = Convert.ToInt32(ResultSet["courseid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        long TeacherId = Convert.ToInt64(ResultSet["teacherid"]);
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);
                        string CourseName = ResultSet["coursename"].ToString();

                        // Create and populate a Course object with the data
                        Course CurrentCourse = new Course()
                        {
                            courseid = CourseId,
                            coursecode = CourseCode,
                            teacherid = TeacherId,
                            startdate = StartDate,
                            finishdate = FinishDate,
                            coursename = CourseName
                        };
                        // Add the course to the list
                        Courses.Add(CurrentCourse);
                    }
                }
            }
            // Return the full list of courses
            return Courses;
        }

        [HttpGet]
        [Route(template: "FindCourse/{id}")]
        public Course FindCourse(int id)
        {
            // Initialize an empty Course object to hold the result
            Course SelectedCourse = new Course();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT * FROM courses where courseid = @id";
                Command.Parameters.AddWithValue("@id", id);

                // Open the database connection to retrieve the course by ID
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        int CourseId = Convert.ToInt32(ResultSet["courseid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        long TeacherId = Convert.ToInt64(ResultSet["teacherid"]);
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);
                        string CourseName = ResultSet["coursename"].ToString();

                        // Assign data to the SelectedCourse object
                        SelectedCourse.courseid = CourseId;
                        SelectedCourse.coursecode = CourseCode;
                        SelectedCourse.teacherid = TeacherId;
                        SelectedCourse.startdate = StartDate;
                        SelectedCourse.finishdate = FinishDate;
                        SelectedCourse.coursename = CourseName;
                    }
                }
            }
            // Return the found course or an empty object if not found
            return SelectedCourse;
        }
    }
}

