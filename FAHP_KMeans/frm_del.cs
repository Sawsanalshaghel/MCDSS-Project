using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fahp_cbr_app
{
    public partial class frm_del : Form
    {
        public frm_del()
        {
            InitializeComponent();
        }

        private void frm_del_Load(object sender, EventArgs e)
        {
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "System.Data.DataRowView" && comboBox1.Text != "")
            dataGridView1.DataSource = Db.get_table_contetnt(comboBox1.SelectedValue.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> ids = new List<string>();
            List<string> clusters = new List<string>();
            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                ids.Add(dataGridView1.SelectedRows[i].Cells["id"].Value.ToString());
                clusters.Add(dataGridView1.SelectedRows[i].Cells["cluster"].Value.ToString());

            }
            Db.del_case_id(comboBox1.SelectedValue.ToString(), ids);
            for (int i = 0; i < clusters.Count; i++)
            {
                List<Case> array = Db.get_cases_condition_cluster(comboBox1.SelectedValue.ToString(), comboBox1.Text, "cluster", clusters[i]);
            }

        }
    }
}
