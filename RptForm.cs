using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class RptForm : Form
    {
        public RptForm()
        {
            InitializeComponent();
        }

        private void RptForm_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\CustomerInfo.mdf;Integrated Security=True");

            SqlDataAdapter da1 = new SqlDataAdapter("SELECT * FROM Customer", con);

            DataSet ds = new DataSet();
            da1.Fill(ds, "Customer");
            SqlDataAdapter da2 = new SqlDataAdapter("SELECT * FROM Address", con);
            da2.Fill(ds, "Address");
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(Path.GetFullPath("..\\..\\") + "CrystalReport1.rpt");
            cryRpt.SetDataSource(ds);

            crystalReportViewer1.ReportSource = cryRpt;
            crystalReportViewer1.Refresh();
        }
    }
}
