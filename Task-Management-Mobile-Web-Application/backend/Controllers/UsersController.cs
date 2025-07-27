using Microsoft.AspNetCore.Mvc;
using System.Data;
using TaskFlowManager.Api.DAL;
using TaskFlowManager.Api.Models;

namespace TaskFlowManager.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IDbConnectionFactory _dbFactory;
        private readonly IQueryProvider _queryProvider;

        public UsersController(IDbConnectionFactory dbFactory, IQueryProvider queryProvider)
        {
            _dbFactory = dbFactory;
            _queryProvider = queryProvider;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = new List<UserDto>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = _queryProvider.GetQuery("FetchUsers");
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new UserDto
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Email = reader.GetString(2),
                        CreatedDate = reader.GetDateTime(3)
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            return Ok(users);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDto user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
                return BadRequest(new { error = "Name and Email are required." });
            try
            {
                using var conn = _dbFactory.CreateConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = _queryProvider.GetQuery("InsertUser");
                var now = DateTime.UtcNow;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Name", user.Name));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Email", user.Email));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CreatedDate", now));
                var result = cmd.ExecuteNonQuery();
                if (result > 0)
                    return StatusCode(201);
                return StatusCode(500, new { error = "Failed to create user." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
