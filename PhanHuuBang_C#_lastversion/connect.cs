using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanHuuBang_C__lastversion
{
    public class connect
    {
        public MySqlConnection getConnect()
        {
            MySqlConnection con = new MySqlConnection();
            string str = "Server=localhost;" +
                "Database=tranthuongshop;" +
                "Port=3306;" +
                "User ID =root;" +
                "Password=;" +
                "charset=utf8;" +
                "old guids=true;";
            con.ConnectionString = str;
            return con;
        }
    }
}
