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

namespace Fahp_cbr_app
{
    public partial class frm_result_fuzzy : Form
    {
        public static bool ObjCreated = false;
        Statistics statistics;
        BestChoice mybestchoice;
        List<int[]> diff_fuzzy_bigger;
        List<int[]> diff_fuzzy_equal;
        List<int[]> diff_fuzzy_smaller;
        List<double[]> diff_fuzzy_bigger_sim;
        List<double[]> diff_fuzzy_smaller_sim;
        List<double[]> diff_fuzzy_equal_sim;
       
       
        public frm_result_fuzzy()
        {
            InitializeComponent();
        }

        private void frm_result_fuzzy_Load(object sender, EventArgs e)
        {

        }

        public void read_files()
        {

            if (comboBox1.SelectedIndex == 0)
            {
                listBox1.Items.Add("خطوة1:ادخال حالات التجربة-ادخالات مستخدم");
                statistics.GetRandomCases_File();
            } // end if combobox
            else if (comboBox1.SelectedIndex == 1) // generate cases
            {
                listBox1.Items.Add("خطوة1:توليد حالات التجربة-ادخالات مستخدم");
                statistics.GenerateRandomCases();

            }

            //display
            dg_randomcases.Columns.Clear();
            dg_randomcases.DataSource = null;
            if (statistics.exp_random_cases_nonstandrize.Count != 0)
            {
                dg_randomcases.Columns.Add("Criteria", "Alternative/Criteria");
                for (int j = 0; j < statistics.exp_random_cases.Count; j++)
                {
                    Case c = (Case)statistics.exp_random_cases_nonstandrize[j];
                    dg_randomcases.Columns.Add(c.GetFeature("id").GetFeatureValue().ToString(), c.GetFeature("id").GetFeatureValue().ToString());
                }
                dg_randomcases.Rows.Add(statistics.exp_random_cases_nonstandrize[0].GetFeatures().Count);

                for (int j = 0; j < statistics.exp_random_cases_nonstandrize[0].GetFeatures().Count; j++)
                {
                    Feature feature = (Feature)statistics.exp_random_cases_nonstandrize[0].GetFeatures()[j];
                    dg_randomcases.Rows[j].Cells[0].Value = feature.GetFeatureName().ToString();
                    for (int i = 0; i < statistics.exp_random_cases_nonstandrize.Count; i++)
                    {
                        Case c = statistics.exp_random_cases_nonstandrize[i];
                        Feature f = (Feature)c.GetFeatures()[j];
                        dg_randomcases.Rows[j].Cells[i + 1].Value = f.GetFeatureValue();
                    }

                }
            }
            dg_randomcases.AutoResizeColumns();
            dg_randomcases.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }
        //------------------------------------------------------------------------------------------
        public void implement_ahp()
        {
            

            // call method
            statistics.FindSolutions_FuzzyAHP();


            // display
            int dG_R = 0;
            
            int DG_cri = 0;
            dg_With_stat.Columns.Clear();
            dg_With_stat.Columns.Add("Porblem No  ", "Porblem No  ");
            dg_With_stat.Columns.Add("SolNum", "Sol No ");
            dg_With_stat.Columns.Add("Weight", "Weight");
            dg_With_stat.Columns.Add("Sol sim with problem  ", "Sol sim with problem  ");

            dg_Non_stat.Columns.Clear();
            dg_Non_stat.Columns.Add("Porblem No ", "Porblem No  ");
            dg_Non_stat.Columns.Add("SolNum", "Sol Num  ");
            dg_Non_stat.Columns.Add("Weight", "Weight");
            dg_Non_stat.Columns.Add("Sol sim with problem  ", "Sol sim with problem  ");

            dg_criteria.Columns.Clear();
            dg_criteria.Columns.Add("Porblem No", "Porblem No  ");
            for (int i = 1; i < statistics.exp_random_cases[0].GetFeatures().Count - 1; i++)
            {
                Feature f = (Feature)statistics.exp_random_cases[0].GetFeatures()[i];
                dg_criteria.Columns.Add(f.GetFeatureName(), f.GetFeatureName());

            }

            
            listBox1.Items.Add("خطوة2: إيجاد حلول لكل حالة دراسة");
            listBox1.Items.Add("الأنسب ايجاد بدائل العنقود");
            listBox1.Items.Add("AHP خطوة 3: تطبيق");
            listBox1.Items.Add("خطوة 1.3 توليد المقارنات الزوجية للمعاير وللبدائل");
           
            foreach (Case c in statistics.exp_random_cases)
            {
                 dg_Non_stat.Rows.Add();
                 dg_With_stat.Rows.Add();
                 
                // ahp
                 dg_Non_stat.Rows[dG_R].Cells[0].Value = c.GetFeature("id").GetFeatureValue().ToString();
                 dg_Non_stat.Rows[dG_R].Cells[1].Value = statistics.AllAhpResults[c][0, 0];// first sol
                 dg_Non_stat.Rows[dG_R].Cells[2].Value = Math.Round(statistics.AllAhpResults[c][0, 1] , 2).ToString();
                 int sol_id = Convert.ToInt32(statistics.AllAhpResults[c][0, 0]);
                 EuclideanSimilarity s = new EuclideanSimilarity();
                 dg_Non_stat.Rows[dG_R].Cells[3].Value = Math.Round(s.Similarity(c, mybestchoice.Cases[sol_id]) * 100, 2).ToString();
                 // fuzzy ahp
                 dg_With_stat.Rows[dG_R].Cells[0].Value = c.GetFeature("id").GetFeatureValue().ToString();
                 dg_With_stat.Rows[dG_R].Cells[1].Value = statistics.AllFuzzyAhpResults[c][0, 0]; // first sol
                 sol_id = Convert.ToInt32(statistics.AllFuzzyAhpResults[c][0, 0]);
                 dg_With_stat.Rows[dG_R].Cells[2].Value = Math.Round(statistics.AllFuzzyAhpResults[c][0, 1],2);
                 s = new EuclideanSimilarity();
                 dg_With_stat.Rows[dG_R].Cells[3].Value = Math.Round(s.Similarity(c, mybestchoice.Cases[sol_id]) * 100, 2).ToString();
                 dG_R++;
                //criteria
                 dg_criteria.Rows.Add(2);
                 dg_criteria.Rows[DG_cri].Cells[0].Value = c.GetFeature("id").GetFeatureValue().ToString();
                 dg_criteria.Rows[DG_cri+1].Cells[0].Value = c.GetFeature("id").GetFeatureValue().ToString();
                 for (int i = 0; i < statistics.exp_random_cases[0].GetFeatures().Count - 2; i++)
                 {
                     dg_criteria.Rows[DG_cri].Cells[i+1].Value = Math.Round(statistics.AllAhpCriteria[c][i]*100,2).ToString() + "%";
                     dg_criteria.Rows[DG_cri + 1].Cells[i + 1].Value = Math.Round(statistics.AllFAhpCriteria[c][i] * 100, 2).ToString() + "%";
                 }
                 DG_cri += 2;

            }

            dg_With_stat.AutoResizeColumns();
            dg_Non_stat.AutoResizeColumns();

            dg_With_stat.AutoSizeColumnsMode =
              DataGridViewAutoSizeColumnsMode.AllCells;
            dg_Non_stat.AutoSizeColumnsMode =
              DataGridViewAutoSizeColumnsMode.AllCells;

        }

        public void calc_matching()
        {
            listBox1.Items.Add("خطوة 4: اظهر نتائج تطابق الطريقتين");
            diff_fuzzy_bigger = new List<int[]>();
            diff_fuzzy_smaller = new List<int[]>();
            diff_fuzzy_equal = new List<int[]>();
            diff_fuzzy_bigger_sim = new List<double[]>();
            diff_fuzzy_smaller_sim = new List<double[]>();
            diff_fuzzy_equal_sim = new List<double[]> ();
            
            // display
            dg_result.Columns.Clear();
            dg_result.Columns.Add("Cases_numbers", "Cases_numbers");
            dg_result.Columns.Add("Similarity", "Similarity");
            double[,] fuzzyAhp_pair, ahp_pair;
            //---------------------------------------------------------------------------------
           
            // display
            foreach (Case c in statistics.exp_random_cases)
            {
                ahp_pair = statistics.AllAhpResults[c];
                fuzzyAhp_pair = statistics.AllFuzzyAhpResults[c];
                EuclideanSimilarity s = new EuclideanSimilarity();
               
                int num1=Convert.ToInt32(fuzzyAhp_pair[0, 0]);
                int num2 = Convert.ToInt32(ahp_pair[0, 0]);
                int sol_id = Convert.ToInt32(c.GetFeature("id").GetFeatureValue());

                double sim1 = Math.Round(s.Similarity(mybestchoice.Cases[num1],c)*100,2); // fuzzy
                double sim2 = Math.Round(s.Similarity(mybestchoice.Cases[num2],c)*100,2); // ahp
                int[] te = new int [3];
                double[] tee = new double[2];

                if (ahp_pair[0, 0] == fuzzyAhp_pair[0, 0])
                { 
                    te[0] = sol_id; 
                    te[1] =num1;
                    te[2] = num2;
                    tee[0] = sim1;
                    tee[1] = sim2;
                    diff_fuzzy_equal.Add(te);
                    diff_fuzzy_equal_sim.Add(tee);
                }
                else
                {
                    
                    // fuzzy > ahp
                    if (sim1 > sim2)
                    {
                        te[0] = sol_id; 
                        te[1] =num1;
                        te[2] = num2;
                        tee[0] = sim1;
                        tee[1] = sim2;
                        diff_fuzzy_bigger.Add(te);
                        diff_fuzzy_bigger_sim.Add(tee);

                    }
                    else // fuzz  < ahp
                    {
                        te[0] = sol_id;
                        te[1] = num1;
                        te[2] = num2;
                        tee[0] = sim1;
                        tee[1] = sim2;
                        diff_fuzzy_smaller.Add(te);
                        diff_fuzzy_smaller_sim.Add(tee);

                    }
                }
                    
             }


            int h = 0;
            dg_result.Rows.Add();
            dg_result.Rows[h].Cells[0].Value = "Fuzzy AHP = AHP";
            dg_result.Rows[h].Cells[1].Value = diff_fuzzy_equal.Count;
            h++;
            for (int i = 0; i < diff_fuzzy_equal.Count; i++)
            {
                dg_result.Rows.Add();
                dg_result.Rows[h].Cells[0].Value = diff_fuzzy_equal[i][0] + "[ " + diff_fuzzy_equal[i][1] + "," + diff_fuzzy_equal[i][2]+" ]";
                dg_result.Rows[h].Cells[1].Value = diff_fuzzy_equal_sim[i][0] + " , " + diff_fuzzy_equal_sim[i][1];
                h++;
            }
            
            dg_result.Rows.Add();
            dg_result.Rows[h].Cells[0].Value = "Fuzzy AHP Sim> AHP Sim";
            dg_result.Rows[h].Cells[1].Value = diff_fuzzy_bigger.Count;
            h++;

            for (int i = 0; i < diff_fuzzy_bigger.Count;i++ )
            {
                dg_result.Rows.Add();
                dg_result.Rows[h].Cells[0].Value = diff_fuzzy_bigger[i][0]+ " [ "+ diff_fuzzy_bigger[i][1] + " , "+ diff_fuzzy_bigger[i][2] + " ] ";
                dg_result.Rows[h].Cells[1].Value = diff_fuzzy_bigger_sim[i][0] + ", " + diff_fuzzy_bigger_sim[i][1];
                h++;
            }

            dg_result.Rows.Add();
            dg_result.Rows[h].Cells[0].Value = "Fuzzy AHP Sim  < AHP Sim";
            dg_result.Rows[h].Cells[1].Value = diff_fuzzy_smaller.Count;
            h++;

            for (int i = 0; i < diff_fuzzy_smaller.Count; i++)
            {
                dg_result.Rows.Add();
                dg_result.Rows[h].Cells[0].Value = diff_fuzzy_smaller[i][0] +" [ "+diff_fuzzy_smaller[i][1] + " , " + diff_fuzzy_smaller[i][2] + " ] ";
                dg_result.Rows[h].Cells[1].Value = diff_fuzzy_smaller_sim[i][0] + ", " + diff_fuzzy_smaller_sim[i][1];
                h++;
            }
            dg_result.AutoResizeColumns();
            dg_result.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
//-----------------------------------------------------------------------------------------
        private void cmbox_case_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbox_case.SelectedValue.ToString() != "System.Data.DataRowView")
            {
                ArrayList myarr = Db.get_cases_as_arraylist(cmbox_case.SelectedValue.ToString(), cmbox_case.Text);
                DataTable mydt = Db.get_table_contetnt_condition("tbl_cases", "case_name", cmbox_case.Text);
                mybestchoice = new BestChoice(myarr, mydt.Rows[0]["standrize_type"].ToString(), mydt.Rows[0]["kmeans_method"].ToString(), mydt.Rows[0]["clusters_num"].ToString());
                mybestchoice.Cases = Db.get_cases_condition(mybestchoice.Tablename_Standardization, mybestchoice.CaseName);
                statistics = new Statistics(mybestchoice.TableName, mybestchoice.CaseName);

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            read_files();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            implement_ahp();
            calc_matching();
        }

        private void dg_Non_stat_SelectionChanged(object sender, EventArgs e)
        {
            string sols = "";

            foreach (DataGridViewRow row in dg_Non_stat.SelectedRows)
            {
                if (row.Cells[2].Value != null)
                    sols = "id  =" + row.Cells["SolNum"].Value.ToString();

                frm_showdatabase fs = new frm_showdatabase();
                fs.dataGridView1.DataSource = Db.get_cases_customized_condition(statistics.tablename, sols);
                fs.Show();

            }
        }

        private void dg_With_stat_SelectionChanged(object sender, EventArgs e)
        {
            string sols = "";

            foreach (DataGridViewRow row in dg_With_stat.SelectedRows)
            {
                if (row.Cells[2].Value != null)
                    sols = "id  =" + row.Cells["SolNum"].Value.ToString();

                frm_showdatabase fs = new frm_showdatabase();
                fs.dataGridView1.DataSource = Db.get_cases_customized_condition(statistics.tablename, sols);
                fs.Show();

            }
        }

        private void dg_result_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string sols = "";

            foreach (DataGridViewRow row in dg_result.SelectedRows)
             if (row.Cells[0].Value != null)
            {
                int first = row.Cells["cases_numbers"].Value.ToString().IndexOf('[');
                int comma = row.Cells["cases_numbers"].Value.ToString().IndexOf(',');
                int num1 = Convert.ToInt32(row.Cells["cases_numbers"].Value.ToString().Substring(first+1,first+comma).Trim());

                int second = row.Cells["cases_numbers"].Value.ToString().IndexOf(']');
                int num2 = Convert.ToInt32(row.Cells["cases_numbers"].Value.ToString().Substring(comma + 1, comma + second).Trim());

               
                    sols = "id  in (" + num1+","+num2+")";

                frm_showdatabase fs = new frm_showdatabase();
                fs.dataGridView1.DataSource = Db.get_cases_customized_condition(statistics.tablename, sols);
                fs.Show();

            }
        }

       
    }
}
