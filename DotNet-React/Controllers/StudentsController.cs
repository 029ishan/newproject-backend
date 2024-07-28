using DotNet_React.StudentData;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Collections.Generic;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNet_React.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly string _context;

        public StudentsController(IConfiguration config)
        {
            _context = config.GetConnectionString("DefaultConnection");
        }

        

        // GET: api/<StudentsController>
        [HttpGet]
        public JsonResult Get()
        {

            List < DetailStudents > studentdetails= new List< DetailStudents > ();
            string query = "SELECT id, name, age, address, faculty FROM \"student_info\"";
            DataTable table = new DataTable();
         //   string sqlDataSource = _context.GetConnectionString("DefaultConnection");
            
            using (NpgsqlConnection mycon = new NpgsqlConnection(_context))
            {
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, mycon))
                {
                    mycon.Open();
                    using (NpgsqlDataReader myReader = myCommand.ExecuteReader())
                    {
                    while (myReader.Read())
                        {
                            DetailStudents student = new DetailStudents
                            {
                            Id = myReader.GetInt32(0),
                            Name = myReader.GetString(1),
                            Age = myReader.GetInt32(2),
                            Address = myReader.GetString(3),
                           Faculty = myReader.GetString(4),
                            };
                            studentdetails.Add (student);
                        }
                    myReader.Close();

                    }
                    mycon.Close();
               
                }
            }
            return new JsonResult(studentdetails);
        }
        //POST: api/<studentController>
        [HttpPost]
    public JsonResult Post(DetailStudents studentdetails)
        {
            string query = @"
            insert into""student_info"" (name, age, address, faculty)
            values(@Name, @Age, @Address, @Faculty)
            ";

            using (NpgsqlConnection mycon = new NpgsqlConnection(_context))
            {
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query,mycon))
                {
                    myCommand.Parameters.AddWithValue("@Name", studentdetails.Name);
                    myCommand.Parameters.AddWithValue("@Age", studentdetails.Age);
                    myCommand.Parameters.AddWithValue("@Address", studentdetails.Address);
                    myCommand.Parameters.AddWithValue("@Faculty", studentdetails.Faculty);

                    mycon.Open();
                    myCommand.ExecuteNonQuery();
                    mycon.Close();
                }

            }
            return new JsonResult("Added Successfully");

        }
        //PUT: api/<studentController>
        [HttpPut("{id}")]
        public JsonResult Put( int id, DetailStudents studentdetails)
        {
            string query = @"
            UPDATE ""student_info""
            SET name =@Name,
            age = @Age,
            address = @Address,
            faculty = @Faculty
            WHERE id = @ID
            ";
            using (NpgsqlConnection mycon = new NpgsqlConnection(_context))

            {
               using (NpgsqlCommand mycommand = new NpgsqlCommand(query,mycon))
                {
                    mycommand.Parameters.AddWithValue("id", id);
                    mycommand.Parameters.AddWithValue("@Name", studentdetails.Name);
                    mycommand.Parameters.AddWithValue("@Age", studentdetails.Age);
                    mycommand.Parameters.AddWithValue("@Address", studentdetails.Address);
                    mycommand.Parameters.AddWithValue("@Faculty", studentdetails.Faculty);

                    mycon.Open();
                    mycommand.ExecuteNonQuery();
                    mycon.Close();

                }

            }
            return new JsonResult("Update Successfully");
        }
        //DELETE: api/<StudentController>
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
Delete From ""student_info""
where id = @Id
        ";
            using (NpgsqlConnection mycon = new NpgsqlConnection(_context))
            {
                using (NpgsqlCommand mycommand = new NpgsqlCommand(query, mycon))
                {
                    mycommand.Parameters.AddWithValue("@Id", id);

                    mycon.Open() ;
                    mycommand.ExecuteNonQuery();
                    mycon.Close();
                }
            }
            return new JsonResult("delete successful");
        }
    }
}
