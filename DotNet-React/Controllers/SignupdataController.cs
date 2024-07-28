using Dapper;
using DotNet_React.StudentData;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
using Npgsql;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

namespace DotNet_React.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignupdataController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly ILogger<SignupdataController> _logger;

        public SignupdataController(IConfiguration config, ILogger<SignupdataController> logger)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

//private string GenerateJwtToken(string email, string role)
//{
//    try
//    {
//        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("012345678910111213141516"));
//        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//                var claims = new[]
//                {
//            new Claim(JwtRegisteredClaimNames.Sub, email),
//            new Claim(ClaimTypes.Role, role) // This should be a string
//        };
//                    Console.WriteLine($"Claims: {string.Join(",", claims.Select(c => $"{c.Type}:{c.Value}"))}");

//        var token = new JwtSecurityToken(
//            claims: claims,
//            expires: DateTime.Now.AddMinutes(120),
//            signingCredentials: credentials
//            );

//        return new JwtSecurityTokenHandler().WriteToken(token);
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError($"Error generating JWT token: {ex.Message} - {ex.StackTrace}");
//        throw;
//    }
//}

[HttpPost("login")]
public IActionResult Login(Logindetail logindetail)
{
    string query = @"
        SELECT id, name, email, password, role
        FROM signup_data
        WHERE email = @Email AND password = @Password";

    using (var mycon = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    var user = mycon.QuerySingleOrDefault<dynamic>(query, new { email = logindetail.Email, password = logindetail.Password });

                    if (user != null)
                    {
                        var response = new
                        {
                            UserId = user.id,
                            Role = user.role ? "Admin" : "User"
                        };
                        return Ok(response);
                    }
                    else
                    {
                        return Unauthorized("Invalid email or password");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in Login method: {ex.Message} - {ex.StackTrace}");
                    return StatusCode(500, $"Internal server error: {ex.Message} - {ex.StackTrace}");
                }
            }
        }



        [HttpGet]
        public JsonResult Get()
        {
            string query = "SELECT id, name, email, password, confirmpassword FROM signup_data";

            using (var mycon = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    var signupdata = mycon.Query<SignupDetail>(query).AsList();
                    return new JsonResult(signupdata);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in Get method: {ex.Message} - {ex.StackTrace}");
                    return new JsonResult("Error retrieving data");
                }
            }
        }



        [HttpPost]
        public JsonResult Signup(SignupDetail signupdata)
        {
            bool defaultRole = false; 

            string query = @"
        INSERT INTO ""signup_data"" (name, email, password, confirmpassword, role)
        VALUES(@Name, @Email, @Password, @Confirmpassword, @Role)";

            using (var mycon = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    mycon.Execute(query, new
                    {
                        name = signupdata.Name,
                        email = signupdata.Email,
                        password = signupdata.Password,
                        Confirmpassword = signupdata.ConfirmPassword,
                        Role = defaultRole
                    });
                    return new JsonResult("Added Successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in Signup method: {ex.Message} - {ex.StackTrace}");
                    return new JsonResult("Error adding data");
                }
            }
        }


        [HttpPut]
        public JsonResult Put(int id, SignupDetail signupData)
        {
            string query = @"
                UPDATE ""signup_data""
                SET name = @Name,
                    email = @Email,
                    password = @Password,
                    confirmpassword = @Confirmpassword,
                    role = @Role
                WHERE id = @ID";

            using (var mycon = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    mycon.Execute(query, new
                    {
                        Id = id,
                        Name = signupData.Name,
                        Email = signupData.Email,
                        Password = signupData.Password,
                        Confirmpassword = signupData.ConfirmPassword,
                        Role = signupData.Role
                    });
                    return new JsonResult("Update Successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in Put method: {ex.Message} - {ex.StackTrace}");
                    return new JsonResult("Error updating data");
                }
            }
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                DELETE FROM signup_data
                WHERE id = @Id";

            using (var mycon = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    mycon.Execute(query, new { Id = id });
                    return new JsonResult("Delete successful");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in Delete method: {ex.Message} - {ex.StackTrace}");
                    return new JsonResult("Error deleting data");
                }
            }
        }
    }
}
