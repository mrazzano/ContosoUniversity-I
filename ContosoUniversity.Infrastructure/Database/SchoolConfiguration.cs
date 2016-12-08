using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace ContosoUniversity.Infrastructure.Database
{
    public class SchoolConfiguration : DbConfiguration
    {
        public SchoolConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}