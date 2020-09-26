using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Xml;

namespace Fahp_cbr_app
{

    public partial class frm_db : Form
    {
        SqlConnection conn = new SqlConnection(@" Data Source=.;Initial Catalog=PhoneDB;Integrated Security=True");
        public static bool ObjCreated = false;
        public frm_db()
        {
            InitializeComponent();
            ObjCreated = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void frm_settings_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       
        private void button4_Click(object sender, EventArgs e)
        {

            
            

           
           
        }

      
        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)

        {

                listBox1.DataSource = Db.get_table_contetnt("tbl_cases");
                listBox1.DisplayMember = "case_name";
                listBox1.ValueMember = "tbl_name";
            
        }

        private void dataGV_casesname_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGV_casesname_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void listBox1_SelectedIndexChanged_2(object sender, EventArgs e)
        {
             comboBox1.Items.Clear();
             textBox1.Clear();
            if (listBox1.ValueMember != "")
            {
                dataGV_details.DataSource = Db.get_all_cases(listBox1.SelectedValue.ToString());
                for (int i = 0; i < dataGV_details.Columns.Count; i++)
                    comboBox1.Items.Add(dataGV_details.Columns[i].HeaderText);
                comboBox1.Text = "cluster";
             //   textBox1.Text = Db.Get_CostFunction(listBox1.SelectedValue.ToString()).ToString();
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Text != "")
            {
                dataGV_details.DataSource = Db.get_all_orderd(listBox1.SelectedValue.ToString(), comboBox1.Text);
                dataGridView1.DataSource = Db.get_count_cluster(listBox1.SelectedValue.ToString());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Db.del_lastcentroid(mybestchoice.TableName);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
           /* string c = dataGridView1.CurrentRow.Cells["cluster"].Value.ToString();
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                if (dataGV_details.Rows[i].Cells["cluster"].Value.ToString() != c)
                    dataGV_details.Rows[i]. = false;
                else
                    dataGV_details.Rows[i].Visible = true;*/
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            textBox1.Text = Db.Get_CostFunction(listBox1.SelectedValue.ToString()).ToString();
            string t= listBox1.SelectedValue.ToString().ToString();
            double sse=0;
            EuclideanSimilarity sim = new EuclideanSimilarity();
            string c=listBox1.Text;
            List < Case > cases = Db.get_cases_condition(Db.get_standriazation(t),c );
            List<Case> centroids = Db.get_Centroids(Db.get_standriazation(t), c);
            foreach (Case a in cases)
                foreach (Case ce in centroids)
                    if (a.GetFeature("cluster").GetFeatureValue().ToString() == ce.GetFeature("id").GetFeatureValue().ToString())
                    { sse += sim.Dissimilarity(a, ce); break; }
            textBox1.Text = sse.ToString();
                
        }
    }
}
