using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolDatabase.Models;
using System;
using MySql.Data.MySqlClient;


namespace SchoolDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        // dependency injection of database context
        public TeacherAPIController(SchoolDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Returns a list of Authors in the system. If a search key is included, search for authors with a first or last name matching.
        /// </summary>
        /// <example>
        /// GET api/Author/ListAuthors?SearchKey=Sam -> [{"AuthorId":1,"AuthorFname":"Sam", "AuthorLName":"Smith"},{"AuthorId":2,"AuthorFname":"Jillian", "AuthorLName":"Samuel"},..]
        /// </example>
        /// <returns>
        /// A list of author objects 
        /// </returns>
        [HttpGet]
        [Route(template: "ListTeachers")]
        public List<Teacher> ListTeachers(string SearchKey = null)
        {

            // Create an empty list of Authors
            List<Teacher> Teachers = new List<Teacher>();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();


                string query = "select * from Teachers";

                // search criteria, first, last or first + last
                if (SearchKey != null)
                {
                    query += " where lower(teacherfname) like @key or lower(authorlname) like @key or lower(concat(teacherfname,' ',teacherlname)) like @key";
                    Command.Parameters.AddWithValue("@key", $"%{SearchKey}%");
                }
                //SQL QUERY
                Command.CommandText = query;
                Command.Prepare();

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Setteacherfname
                    while (ResultSet.Read())
                    {
                        //Access Column information by the DB column name as an index
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string EmployeeNumber = ResultSet["employeenumber"].ToString();
                        DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);


                        //short form for setting all properties while creating the object
                        Teacher CurrentTeacher = new Teacher()
                        {
                            teacherid = Id,
                            teacherfname = FirstName,
                            teacherlname = LastName,
                            employeenumber = EmployeeNumber,
                            hiredate = HireDate,
                            salary = Salary
                        };

                        Teachers.Add(CurrentTeacher);

                    }
                }
            }


            //Return the final list of authors
            return Teachers;
        }

        /// <summary>
        /// Returns an author in the database by their ID
        /// </summary>
        /// <example>
        /// GET api/Author/FindAuthor/3 -> {"AuthorId":3,"AuthorFname":"Sam","AuthorLName":"Cooper","AuthorJoinDate":"2020-10-11", "AuthorBio":"Fun Guy", "NumArticles":1}
        /// </example>
        /// <returns>
        /// A matching author object by its ID. Empty object if Author not found
        /// </returns>
        [HttpGet]
        [Route(template: "FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {

            //Empty Author
            Teacher SelectedTeacher = new Teacher();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // @id is replaced with a sanitized id
                // 'how many' 'articles' <> count(articleid)
                // 'for each' 'author' <> group by (authorid)
                Command.CommandText = "select teacher.*, count(.teacherid) as numarticles from authors left join articles on (articles.authorid=authors.authorid) where authors.authorid=@id group by authors.authorid";
                Command.Parameters.AddWithValue("@id", id);

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {
                        //Access Column information by the DB column name as an index
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string EmployeeNumber = ResultSet["employeenumber"].ToString();
                        DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);



                        SelectedTeacher.teacherid = Id;
                        SelectedTeacher.teacherfname = FirstName;
                        SelectedTeacher.teacherlname = LastName;
                        SelectedTeacher.employeenumber = EmployeeNumber;
                        SelectedTeacher.hiredate = HireDate;
                        SelectedTeacher.salary = Salary;

                       

                    }
                    return SelectedTeacher;
                }
            }
        }
    }
}