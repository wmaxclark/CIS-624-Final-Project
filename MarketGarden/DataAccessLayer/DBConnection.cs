using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    // A factory to provide connection objects to the data access layer
    internal static class DBConnection
    {
        private static string connectionString =
            @"Data Source=yury-bot\localhost;Initial Catalog=farm_db;Integrated Security=True";
        public static SqlConnection GetDBConnection()
        {
            var conn = new SqlConnection(connectionString);
            return conn;
        }
    }
}

