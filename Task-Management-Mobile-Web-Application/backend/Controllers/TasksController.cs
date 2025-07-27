using Microsoft.AspNetCore.Mvc;
using System.Data;
using TaskFlowManager.Api.DAL;
using TaskFlowManager.Api.Models;

namespace TaskFlowManager.Api.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly IDbConnectionFactory _dbFactory;
        private readonly IQueryProvider _queryProvider;

        public TasksController(IDbConnectionFactory dbFactory, IQueryProvider queryProvider)
        {
            _dbFactory = dbFactory;
            _queryProvider = queryProvider;
        }

        [HttpGet("{userId}")]
        public IActionResult GetTasksForUser(int userId)
        {
            var tasks = new List<TaskDto>();
            try
            {
                using var conn = _dbFactory.CreateConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = _queryProvider.GetQuery("FetchTasksByUser");
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@UserId", userId));
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tasks.Add(new TaskDto
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Status = reader.GetString(3),
                        CreatedDate = reader.GetDateTime(4),
                        DueDate = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                        UserId = userId
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            return Ok(tasks);
        }

        [HttpPost]
        public IActionResult CreateTask([FromBody] TaskDto task)
        {
            if (task == null || string.IsNullOrWhiteSpace(task.Title) || task.UserId == 0)
                return BadRequest(new { error = "Title and UserId are required." });
            try
            {
                using var conn = _dbFactory.CreateConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = _queryProvider.GetQuery("InsertTask");
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Title", task.Title));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Description", (object?)task.Description ?? DBNull.Value));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Status", string.IsNullOrEmpty(task.Status) ? "Pending" : task.Status));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@UserId", task.UserId));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CreatedDate", DateTime.UtcNow));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DueDate", (object?)task.DueDate ?? DBNull.Value));
                var result = cmd.ExecuteNonQuery();
                if (result > 0)
                    return StatusCode(201);
                return StatusCode(500, new { error = "Failed to create task." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTaskStatus(int id, [FromBody] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return BadRequest(new { error = "Status is required." });
            try
            {
                using var conn = _dbFactory.CreateConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = _queryProvider.GetQuery("UpdateTaskStatus");
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Status", status));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Id", id));
                var result = cmd.ExecuteNonQuery();
                if (result > 0)
                    return Ok();
                return NotFound(new { error = "Task not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = _queryProvider.GetQuery("RemoveTask");
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Id", id));
                var result = cmd.ExecuteNonQuery();
                if (result > 0)
                    return Ok();
                return NotFound(new { error = "Task not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
