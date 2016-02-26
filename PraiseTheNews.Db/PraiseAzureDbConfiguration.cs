using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace PraiseTheNews.Db
{
    public class PraiseAzureDbConfiguration : DbConfiguration
    {
        public PraiseAzureDbConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}