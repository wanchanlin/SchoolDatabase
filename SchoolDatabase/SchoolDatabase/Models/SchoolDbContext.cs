using MySql.Data.MySqlClient;
namespace SchoolDatabase.Models
{
    public class SchoolDbContext
    {
            //These are readonly "secret" properties.
            //Only the BlogDbContext class can use them.
            //Change these to match your own local blog database!
            private static string User { get { return "root"; } }
            private static string Password { get { return "root"; } }
            private static string Database { get { return "school"; } }
            private static string Server { get { return "localhost"; } }
            private static string Port { get { return "3306"; } }

            //ConnectionString is a series of credentials used to connect to the database.
            protected static string ConnectionString
            {
                get
                {
                    //convert zero datetime is a db connection setting which returns NULL if the date is 0000-00-00
                    //this can allow C# to have an easier interpretation of the date (no date instead of 0 BCE)

                    return "server = " + Server
                        + "; user = " + User
                        + "; database = " + Database
                        + "; port = " + Port
                        + "; password = " + Password
                        + "; convert zero datetime = True";
                }
            }

            public MySqlConnection AccessDatabase()
            {
     
                return new MySqlConnection(ConnectionString);
            }



    }
}
