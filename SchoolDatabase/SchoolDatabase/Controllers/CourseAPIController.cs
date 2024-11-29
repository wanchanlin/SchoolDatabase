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
        private readonly SchoolDbContext _context;

        public CourseAPIController(SchoolDbContext context)
        {
            _context = context;
        }

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
