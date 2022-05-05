
using System.Data.SqlClient;

namespace UptimeR.ML.Trainer.Interfaces;

public interface ISQLConn
{
    SqlConnection CreateConnection();
    bool TestConn(SqlConnection conn);
}
