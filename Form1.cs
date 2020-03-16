using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class Form1 : Form
    {
        DataSet ds;
        BindingSource bs1 = new BindingSource();
        BindingSource bs2 = new BindingSource();
        SqlDataAdapter da1, da2;
        SqlConnection con;
        public Form1()
        {
            InitializeComponent();
  
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            bs1.EndEdit();

            da1.Update(ds.Tables[0]);
            ds.Tables[0].AcceptChanges();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            bs2.EndEdit();
            da2.Update(ds.Tables[1]);
            ds.Tables[0].AcceptChanges();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT Max(id)+1 From Address", con);
            con.Open();
            int i = (int)cmd.ExecuteScalar();
            con.Close();
            cmd.CommandText = "INSERT INTO Address VALUES (@i, @c, @p, @ci)";
            cmd.Parameters.AddWithValue("@i", i);
            cmd.Parameters.AddWithValue("@c", textBox2.Text);
            cmd.Parameters.AddWithValue("@p", textBox3.Text);
            cmd.Parameters.AddWithValue("@ci", comboBox1.SelectedValue);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            DataRow r = ds.Tables["Address"].Rows.Add(i, textBox2.Text, textBox3.Text, comboBox1.SelectedValue);
           ds.Tables["Address"].AcceptChanges();
            //SqlCommand cmd = new SqlCommand( "INSERT INTO Customers")
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bs1.EndEdit();
            da1.Update(ds.Tables["Customer"]);
            ds.Tables["Customer"].AcceptChanges();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bs2.EndEdit();
            da2.Update(ds.Tables["Address"]);
            ds.Tables["Address"].AcceptChanges();

        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RptForm().Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\CustomerInfo.mdf;Integrated Security=True");

            da1 = new SqlDataAdapter("SELECT * FROM Customer", con);
            SqlCommandBuilder cb1 = new SqlCommandBuilder(da1);
            ds = new DataSet();
            da1.Fill(ds, "Customer");
            da2 = new SqlDataAdapter("SELECT * FROM Address", con);
            da2.Fill(ds, "Address");
            SqlCommandBuilder cb2 = new SqlCommandBuilder(da2);
            ds.Tables["Customer"].PrimaryKey = new DataColumn[] { ds.Tables["Customer"].Columns["CustomerId"] };
            ds.Tables["Address"].PrimaryKey = new DataColumn[] { ds.Tables["Address"].Columns["Id"] };
            DataRelation rel = new DataRelation("FK_C_T", ds.Tables["Customer"].Columns["CustomerId"], ds.Tables["Address"].Columns["CustomerId"]);
            ds.Relations.Add(rel);
            bs1.DataSource = ds;
            bs1.DataMember = "Customer";
            bs2.DataSource = bs1;
            bs2.DataMember = "FK_C_T";
            dataGridView1.DataSource = bs1;
            dataGridView2.DataSource = bs2;
            bindingNavigator2.BindingSource = bs1;
            bindingNavigator3.BindingSource = bs2;
            comboBox1.DataSource = ds.Tables["Customer"];


        }
    }
}
