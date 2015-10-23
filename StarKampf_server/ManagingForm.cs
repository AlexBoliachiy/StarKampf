using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Sorry for temporary lack of comments, they will be added soon;
//This is an attempt at managing Dataset as a Form;
//DBCon class is situated in the respective .cs file;
//The actual working code is situated in mmiprojDataSet.xsd->mmiprojDataSet.Designer.cs , it is auto-generated, and contains all the necessery tools;
//This is attempt to shorten that giant piece of code, but surely is unnecessary;
namespace StarKampf_server
{
    public partial class ManagingForm : Form
    {
        public ManagingForm()
        {
            InitializeComponent();
        }

        DBCon objConnect;
        string conString;
        DataSet dset;
        DataRow drow;
        int Max_rows;
        int inc = 0;

        private void ManagingForm_Load(object sender, EventArgs e)
        {
            try
            {
                objConnect = new DBCon();
                conString = Properties.Settings.Default.MMIPROJConnectionString;
                objConnect.connection_string = conString;
                objConnect.SQL = Properties.Settings.Default.; //it should've been Properties.Settings.Default.Sql; , but ".Sql" does not exist; trying to figure out what's wrong with it; needs fixing
                dset = objConnect.GetConnection;
                Max_rows = dset.Tables[0].Rows.Count;
                NavigateRecords();
            }
            catch (Exception Error) { MessageBox.Show(Error.Message); }
        }
        private void NavigateRecords()
        {
            drow = dset.Tables[0].Rows[inc];
            txtID.Text = drow.ItemArray.GetValue(1).ToString();
            txtName.Text = drow.ItemArray.GetValue(2).ToString();
        }
    }
}
