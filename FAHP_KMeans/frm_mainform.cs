using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
namespace Fahp_cbr_app
{
    public partial class frm_mainform : Form
    {
       
        public static frm_db frm_db = null;
        public static frm_processing frm_pro = null;
        public static frm_input frm_input = null;
        public static frm_update frm_update = null;
        public static frm_results frm_reslut = null;
        public static frm_Alternative frm_alternative = null;
        public static frm_FuzzyDB frm_fuzzyDB = null;
        public static frm_processing frm_process = null;
        public static frm_result_fuzzy frm_resultfuzzy = null;
        public static List<int> Ids = new List<int>();
        SqlConnection conn = new SqlConnection(@" Data Source=.;Initial Catalog=PhoneDB;Integrated Security=True");
        public frm_mainform()
        {
            InitializeComponent();
        }

        private void frm_mainform_Load(object sender, EventArgs e)
        {

        }

  

        private void toolStripButton4_Click_1(object sender, EventArgs e)
        {
           
             if (frm_update.ObjCreated == false)
              {
                  frm_update = new frm_update();
                  frm_update.MdiParent = this;
                  frm_update.ClientSize = new System.Drawing.Size(2000, 800);
                  this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                  frm_update.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                  frm_update.dataGridView1.DataSource = Db.get_table_contetnt("tbl_cases");
                  frm_update.Dock = DockStyle.Fill;
                  frm_update.Show();
                  frm_update.ObjCreated = true;
              }
              else
              {
                  frm_update.WindowState = FormWindowState.Normal;
                  frm_update.Focus();
              }
        }



        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            //if ( frm_FuzzyDB.ObjCreated == false)
            //{
                frm_fuzzyDB = new frm_FuzzyDB();
             //   frm_fuzzyDB.MdiParent = this;
              //  frm_fuzzyDB.ClientSize = new System.Drawing.Size(2000, 800);
                frm_fuzzyDB.WindowState = System.Windows.Forms.FormWindowState.Maximized;
               // frm_fuzzyDB.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
               // frm_fuzzyDB.Dock = DockStyle.Fill;
                frm_fuzzyDB.cmbox_case.DataSource = Db.get_table_contetnt("tbl_cases");
                frm_fuzzyDB.cmbox_case.DisplayMember = "case_name";
                frm_fuzzyDB.cmbox_case.ValueMember = "tbl_name";
                frm_fuzzyDB.Show();
               // frm_Alternative.ObjCreated = true;

               
          //  }
          //  else
          //  {
             //   frm_fuzzyDB.WindowState = FormWindowState.Normal;
             //   frm_fuzzyDB.Focus();

           // }
           /* if ( frm_Alternative.ObjCreated == false)
            {
                frm_alternative = new frm_Alternative();
                frm_alternative.MdiParent = this;
                frm_alternative.ClientSize = new System.Drawing.Size(2000, 800);
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                frm_alternative.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                frm_alternative.Dock = DockStyle.Fill;
                frm_alternative.cmbox_case.DataSource = Db.get_table_contetnt("tbl_cases");
                frm_alternative.cmbox_case.DisplayMember = "case_name";
                frm_alternative.cmbox_case.ValueMember = "tbl_name";
                frm_alternative.Show();
                frm_Alternative.ObjCreated = true;

               
            }
            else
            {
                frm_alternative.WindowState = FormWindowState.Normal;
                frm_alternative.Focus();

            }*/
               
        }



        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (frm_db.ObjCreated == false)
            {
                frm_db = new frm_db();
                frm_db.MdiParent = this;
                frm_db.ClientSize = new System.Drawing.Size(2000, 800);
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                frm_db.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                frm_db.Dock = DockStyle.Fill;
                frm_db.Show();
                frm_db.ObjCreated = true;
            }
            else
            {
                frm_db.WindowState = FormWindowState.Normal;
                frm_db.Focus();
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            frm_AHP frm_ahp = new frm_AHP();

            frm_ahp.Show();
           
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {

            if (frm_input.ObjCreated == false)
            {
                frm_input = new frm_input();
                frm_input.MdiParent = this;
                frm_input.ClientSize = new System.Drawing.Size(2000, 800);
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                frm_input.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                frm_input.Dock = DockStyle.Fill;
                frm_input.Show();
                frm_input.ObjCreated = true;
            }
            else
            {
                frm_input.WindowState = FormWindowState.Normal;
                frm_input.Focus();
            }

            
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            frm_update fu = new frm_update();
            fu.dataGridView1.DataSource = Db.get_table_contetnt("tbl_cases");
            fu.Show();
        }

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            

        }

        private void toolStripButton7_Click_1(object sender, EventArgs e)
        {
            if (frm_results.ObjCreated == false)
            {
                frm_reslut = new frm_results();
            //    frm_reslut.MdiParent = this;
              //  frm_reslut.ClientSize = new System.Drawing.Size(2000, 800);
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
               // frm_reslut.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
               // frm_reslut.Dock = DockStyle.Fill;
                frm_reslut.cmbox_case.DataSource = Db.get_table_contetnt("tbl_cases");
                frm_reslut.cmbox_case.DisplayMember = "case_name";
                frm_reslut.cmbox_case.ValueMember = "tbl_name";
                frm_reslut.cmbox_case.DataSource = Db.get_table_contetnt("tbl_cases");
                frm_reslut.cmbox_case.DisplayMember = "case_name";
                frm_reslut.cmbox_case.ValueMember = "tbl_name";
                frm_reslut.Show();
             //   frm_results.ObjCreated = true;


            }
            else
            {
                frm_reslut.WindowState = FormWindowState.Normal;
                frm_reslut.Focus();

            }
               
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            frm_resultfuzzy = new frm_result_fuzzy();
            frm_resultfuzzy.cmbox_case.DataSource = Db.get_table_contetnt("tbl_cases");
            frm_resultfuzzy.cmbox_case.DisplayMember = "case_name";
            frm_resultfuzzy.cmbox_case.ValueMember = "tbl_name";
            frm_resultfuzzy.cmbox_case.DataSource = Db.get_table_contetnt("tbl_cases");
            frm_resultfuzzy.cmbox_case.DisplayMember = "case_name";
            frm_resultfuzzy.cmbox_case.ValueMember = "tbl_name";
            frm_resultfuzzy.Show();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            if (frm_input.ObjCreated == false)
            {
                frm_process = new frm_processing();
                frm_process.MdiParent = this;
                frm_process.ClientSize = new System.Drawing.Size(2000, 800);
                //this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                frm_process.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                //frm_process.Dock = DockStyle.Fill;

                frm_process.comboBox1.DataSource = Db.get_table_contetnt("tbl_cases");
                frm_process.comboBox1.DisplayMember = "case_name";
                frm_process.comboBox1.ValueMember = "tbl_name";
                frm_process.comboBox1.DataSource = Db.get_table_contetnt("tbl_cases");
                frm_process.comboBox1.DisplayMember = "case_name";
                frm_process.comboBox1.ValueMember = "tbl_name";
                frm_process.Show();
                frm_processing.ObjCreated = true;
            }
            else
            {
                frm_process.WindowState = FormWindowState.Normal;
                frm_process.Focus();
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
