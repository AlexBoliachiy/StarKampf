using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Sorry for temporary lack of comments, this wil get fixed soon;
namespace StarKampf_server
{
    class DBCon
    {
        private string SQL_string;
        private string str_con;

        System.Data.SqlClient.SqlDataAdapter da_1;

        public string SQL
        { set { SQL_string = value; } }

        public string connection_string
        { set { str_con = value; } }

        public System.Data.DataSet GetConnection
        { get { return MyDataSet(); } }

        private System.Data.DataSet MyDataSet()
        {
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(str_con);
            con.Open();
            da_1 = new System.Data.SqlClient.SqlDataAdapter(SQL_string, con);
            System.Data.DataSet data_set = new System.Data.DataSet();
            da_1.Fill(data_set, "Test");
            con.Close();
            return data_set; }
    }
}
