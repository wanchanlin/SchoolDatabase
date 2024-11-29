using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolDatabase.Models;
using System;
using MySql.Data.MySqlClient;

namespace SchoolDatabase.Controllers
{
    /// <summary>
    /// API controller to manage course data in the school database.
    /// Provides endpoints for listing, retrieving, adding, and deleting course records.
    /// </summary>
    [Route("api/Course")]
    [ApiController]
    public class CourseAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public CourseAPIController(SchoolDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Retrieves a list of all courses in the database.
        /// </summary>
        /// <returns>A list of <see cref="Course"/> objects representing all courses.</returns>
        [HttpGet]
        [Route(template: "ListCourses")]
        public List<Course> ListCourses()
        {
            List<Course> Courses = new List<Course>();
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                string query = "SELECT * FROM courses";
                Command.CommandText = query;
                Command.Prepare();

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        Courses.Add(new Course
                        {
                            courseid = Convert.ToInt32(ResultSet["courseid"]),
                            coursecode = ResultSet["coursecode"].ToString(),
                            teacherid = Convert.ToInt64(ResultSet["teacherid"]),
                            startdate = Convert.ToDateTime(ResultSet["startdate"]),
                            finishdate = Convert.ToDateTime(ResultSet["finishdate"]),
                            coursename = ResultSet["coursename"].ToString()
                        });
                    }
                }
            }
            return Courses;
        }
        /// <summary>
        /// Retrieves details of a specific course by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the course to retrieve.</param>
        /// <returns>A <see cref="Course"/> object representing the course, or null if not found.</returns>
       
        [HttpGet]
        [Route(template: "FindCourse/{id}")]
        public Course FindCourse(int id)
        {
            Course SelectedCourse = new Course();
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT * FROM courses WHERE courseid = @id";
                Command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    if (ResultSet.Read())
                    {
                        SelectedCourse.courseid = Convert.ToInt32(ResultSet["courseid"]);
                        SelectedCourse.coursecode = ResultSet["coursecode"].ToString();
                        SelectedCourse.teacherid = Convert.ToInt64(ResultSet["teacherid"]);
                        SelectedCourse.startdate = Convert.ToDateTime(ResultSet["startdate"]);
                        SelectedCourse.finishdate = Convert.ToDateTime(ResultSet["finishdate"]);
                        SelectedCourse.coursename = ResultSet["coursename"].ToString();
                    }
                }
            }
            return SelectedCourse;
        }
        /// <summary>
        /// Adds a new course record to the database.
        /// </summary>
        /// <param name="CourseData">A <see cref="Course"/> object containing the details of the new course.</param>
        /// <example>
        /// POST: api/Course/AddCourse
        /// Body:
        /// {
        ///     "coursecode": "CSE101",
        ///     "teacherid": 123,
        ///     "startdate": "2024-01-01",
        ///     "finishdate": "2024-06-01",
        ///     "coursename": "Introduction to Programming"
        /// }
        /// </example>
        /// <returns>The ID of the newly added course, or 0 if the operation fails.</returns>

        [HttpPost(template: "AddCourse")]
        public int AddCourse([FromBody] Course CourseData)
        {
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "INSERT INTO courses (coursecode, teacherid, startdate, finishdate, coursename) VALUES (@coursecode, @teacherid, @startdate, @finishdate, @coursename)";
                Command.Parameters.AddWithValue("@coursecode", CourseData.coursecode);
                Command.Parameters.AddWithValue("@teacherid", CourseData.teacherid);
                Command.Parameters.AddWithValue("@startdate", CourseData.startdate);
                Command.Parameters.AddWithValue("@finishdate", CourseData.finishdate);
                Command.Parameters.AddWithValue("@coursename", CourseData.coursename);

                Command.ExecuteNonQuery();
                return Convert.ToInt32(Command.LastInsertedId);
            }
        }
        /// <summary>
        /// Deletes a course record from the database by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the course to delete.</param>
        /// <example>
        /// DELETE: api/Course/DeleteCourse/{id}
        /// </example>
        /// <returns>The number of rows affected by the delete operation.</returns>

        [HttpDelete(template: "DeleteCourse/{id}")]
        public int DeleteCourse(int id)
        {
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "DELETE FROM courses WHERE courseid = @id";
                Command.Parameters.AddWithValue("@id", id);
                return Command.ExecuteNonQuery();
            }
        }
    }
}
