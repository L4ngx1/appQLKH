using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace appQLKH
{
    internal class Connection
    {
        public static string strConnection = @"Data Source=DESKTOP-75SKCA4;Initial Catalog=QLKhachHang;Integrated Security=True";
        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(strConnection);
        }
    }
}
