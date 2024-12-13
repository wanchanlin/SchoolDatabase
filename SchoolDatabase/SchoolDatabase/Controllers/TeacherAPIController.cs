using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolDatabase.Models;
using System;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using Mysqlx.Datatypes;


namespace SchoolDatabase.Controllers
{
    // API controller to manage teacher data in the school database
    [Route("api/Teacher")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        // Dependency injection of the database context
        private readonly SchoolDbContext _context;

        public TeacherAPIController(SchoolDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Retrieves a list of teachers from the database. Optionally filters teachers based on their hire date.
        /// </summary>
        /// <param name="SearchKey">An optional parameter used to filter teachers by hire date. If provided, only teachers
        /// whose hire dates contain this search term will be returned.</param>
        /// <returns>A list of <see cref="Teacher"/> objects representing the teachers in the database.</returns>
        [HttpGet]
        [Route(template: "ListTeachers")]
        public List<Teacher> ListTeachers(string SearchKey = null)
        {
            // Initialize an empty list to hold teacher data
            List<Teacher> Teachers = new List<Teacher>();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                // SQL query to select all teachers
                MySqlCommand Command = Connection.CreateCommand();
                string query = "select * from teachers";

                if (SearchKey != null)
                {
                    query += " where hiredate like @key ";
                    Command.Parameters.AddWithValue("@key", $"%{SearchKey}%");
                }

                //SQL QUERY
                Command.CommandText = query;
                Command.Prepare();

                // Execute the query and read each teacher record
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // Map each database column to the Teacher object's properties
                    while (ResultSet.Read())
                    {
                        // Map each database column to the Teacher object's properties
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string EmployeeNumber = ResultSet["employeenumber"].ToString();
                        DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);


                        // Create and populate a Teacher object with the data
                        Teacher CurrentTeacher = new Teacher()
                        {
                            teacherid = Id,
                            teacherfname = FirstName,
                            teacherlname = LastName,
                            employeenumber = EmployeeNumber,
                            hiredate = HireDate,
                            salary = Salary
                        };
                        // Add the teacher to the list
                        Teachers.Add(CurrentTeacher);

                    }
                }
            }
            // Return the full list of teachers
            return Teachers;
        }
        /// <summary>
        /// Retrieves a teacher's details from the database based on the specified teacher ID.
        /// </summary>
        /// <param name="id">The unique identifier of the teacher to retrieve.</param>
        /// <returns>A <see cref="Teacher"/> object representing the teacher with the specified ID, or an empty <see cref="Teacher"/> object if no match is found.</returns>
        [HttpGet]
        [Route(template: "FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            // Initialize an empty Teacher object to hold the result
            Teacher SelectedTeacher = new Teacher();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                // Initialize an empty Teacher object to hold the result
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT * FROM teachers where teacherid = " + id;
                Command.Parameters.AddWithValue("@id", id);

                // Open the database connection to retrieve the teacher by ID
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {

                    while (ResultSet.Read())
                    {
                        // Map database columns to the SelectedTeacher object's properties
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string EmployeeNumber = ResultSet["employeenumber"].ToString();
                        DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);


                        // Assign data to the SelectedTeacher object
                        SelectedTeacher.teacherid = Id;
                        SelectedTeacher.teacherfname = FirstName;
                        SelectedTeacher.teacherlname = LastName;
                        SelectedTeacher.employeenumber = EmployeeNumber;
                        SelectedTeacher.hiredate = HireDate;
                        SelectedTeacher.salary = Salary;
                    }
                }
            } // Return the found teacher or an empty object if not found
            return SelectedTeacher;
        }


        /// <summary>
        /// Adds a new teacher to the database.
        /// </summary>
        /// <param name="TeacherData">A teacher object containing the details to insert.</param>
        /// <example>
        /// POST: api/TeacherAPI/AddTeacher
        /// Headers: Content-Type: application/json
        /// Body:
        /// {
        ///     "teacherfname": "John",
        ///     "teacherlname": "Doe",
        ///     "employeenumber": "12345",
        ///     "hiredate": "2024-01-01",
        ///     "salary": 60000
        /// }
        /// </example>
        /// <returns>The ID of the newly added teacher, or 0 if unsuccessful.</returns>

        [HttpPost(template: "AddTeacher")]
        public int AddTeacher([FromBody] Teacher TeacherData)
        {
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@teacherfname, @teacherlname, @employeenumber, @hiredate, @salary)";
                Command.Parameters.AddWithValue("@teacherfname", TeacherData.teacherfname);
                Command.Parameters.AddWithValue("@teacherlname", TeacherData.teacherlname);
                Command.Parameters.AddWithValue("@employeenumber", TeacherData.employeenumber);
                Command.Parameters.AddWithValue("@hiredate", TeacherData.hiredate);
                Command.Parameters.AddWithValue("@salary", TeacherData.salary);

                Command.ExecuteNonQuery();

                int lastID = Convert.ToInt32(Command.LastInsertedId);
                //Debug.WriteLine(lastID);
                return Convert.ToInt32(Command.LastInsertedId);
            }
            return 0;
        }

        /// <summary>
        /// Deletes a teacher from the database by ID.
        /// </summary>
        /// <param name="TeacherId">The ID of the teacher to delete.</param>
        /// <example>
        /// DELETE: api/TeacherAPI/DeleteConfirm/{TeacherId}
        /// </example>
        /// <returns>The number of rows affected by the operation.</returns>
        [HttpDelete(template: "DeleteConfirm/{TeacherId}")]
        public int DeleteConfirm(int TeacherId)
        {
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "delete from teachers where teacherid=@id";
                Command.Parameters.AddWithValue("@id", TeacherId);
                return Command.ExecuteNonQuery();
            }
            return 0;
        }

        [HttpPut(template: "UpdatedTeacher/{TeacherId}")]
        public Teacher UpdatedTeacher(int TeacherId, [FromBody] Teacher TeacherData)
        {
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "update teachers set teacherfname=@teacherfname, teacherlname=@teacherlname, employeenumber=@employeenumber, hiredate=@hiredate,salary=@salary where teacherid=@id";
                Command.Parameters.AddWithValue("@teacherfname", TeacherData.teacherfname);
                Command.Parameters.AddWithValue("@teacherlname", TeacherData.teacherlname);
                Command.Parameters.AddWithValue("@employeenumber", TeacherData.employeenumber);
                Command.Parameters.AddWithValue("@hiredate", TeacherData.hiredate);
                Command.Parameters.AddWithValue("@salary", TeacherData.salary);

                Command.Parameters.AddWithValue("@id", TeacherId);

                Command.ExecuteNonQuery();
            }

            return FindTeacher(TeacherId);
        }


    }
}