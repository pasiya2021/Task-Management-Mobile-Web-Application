using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Xml.Linq;

namespace TaskFlowManager.Api.DAL
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;
        public DbConnectionFactory()
        {
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "Config", "db.config");
            var xml = XDocument.Load(configPath);
            _connectionString = xml.Descendants("add")
                .FirstOrDefault(x => (string)x.Attribute("name") == "TaskManagerDb")?
                .Attribute("connectionString")?.Value ?? "";
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
