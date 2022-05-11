using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UptimeR.ML.Trainer.Interfaces;

namespace Uptimer.ML.Reader;


public class SQLConn : ISQLConn
{
    private readonly IConfiguration _config;
    public SQLConn(IConfiguration config)
    {
        _config = config;
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_config.GetConnectionString("DataConnection"));
    }

    public bool TestConn(SqlConnection conn)
    {
        conn.Open();
        if (conn.State == System.Data.ConnectionState.Open)
        {
            conn.Close();
            return true;
        }
        conn.Close();
        return false;
    }
}
