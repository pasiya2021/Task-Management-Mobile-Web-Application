using System.Data;

namespace TaskFlowManager.Api.DAL
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
