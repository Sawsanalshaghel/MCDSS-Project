using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
namespace Fahp_cbr_app
{
    public partial class frm_AHP : Form
    {
        SqlConnection conn = new SqlConnection(@" Data Source=.;Initial Catalog=PhoneDB;Integrated Security=True");
       // public Dictionary<string, Dictionary<double, double>> simarr = new Dictionary<string, Dictionary<double, double>>();

      
        frm_Alternative frm_alternative = new frm_Alternative();
       
        string sols = "";
        int clusterid;
       Statistics statistics;
       BestChoice mybestchoice;
       MyAHPModel ahp_sol_clustering;
       MyAHPModel ahp_sol_no_clustering;
       KmeansPlus mykp;
       List<Case> BestCluster;
       double[,] CriteriaArr;
       Dictionary<int, double[,]> ChoicesArr_With;
        Dictionary<int, double[,]>  ChoicesArr_Non;
        public frm_AHP()
        {
            InitializeComponent();
        }


        private void cmbox_case_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGV_CaseEntries.Columns.Clear();
           

            if ((conn.State == ConnectionState.Closed) && (cmbox_case.SelectedValue.ToString() != "System.Data.DataRowView"))
            {
                ArrayList myarr = Db.get_cases_as_arraylist(cmbox_case.SelectedValue.ToString(), cmbox_case.Text);
                DataTable mydt = Db.get_table_contetnt_condition("tbl_cases", "case_name", cmbox_case.Text);
                conn.Open();

                statistics = new Statistics(cmbox_case.SelectedValue.ToString(), cmbox_case.Text);
                string select_name = "select * from tbl_dic where caseid ='" + cmbox_case.Text + "'";
                //   string select_name = " SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.Columns where TABLE_NAME = '" + comboBox1.SelectedValue + "'";
                SqlCommand cmd = new SqlCommand(select_name, conn);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                int p = sd.Fill(dt);

                if (p != 0)
                {
                    dataGV_CaseEntries.DataSource = dt;
                   
                    for (int i = 0; i < dataGV_CaseEntries.Columns.Count; i++)
                    {
                        dataGV_CaseEntries.Columns[i].Visible = false;
                       
                    }

                    dataGV_CaseEntries.Columns["FeatureName"].Visible = true;
                    dataGV_CaseEntries.Columns["FeatureName"].HeaderText = "المعيار";
                    dataGV_CaseEntries.Columns["FeatureUnit"].Visible = true;
                    dataGV_CaseEntries.Columns["FeatureUnit"].HeaderText = "وحدة البيانات";
                    dataGV_CaseEntries.Columns.Add("UserEntry", "تفضيلات المستخدم");
                    dataGV_CaseEntries.Columns.Add("ComEntry", " عامل المقارنة");
              
                }
                
                conn.Close();
            }

        }

   
        private void dG_Non_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           

             
       
        }

        private void dG_Non_SelectionChanged(object sender, EventArgs e)
        {
            sols = "";
          /*  foreach (DataGridViewRow row in dG_Non.SelectedRows)
               
            {
                
                for (int i = 2; i < dG_Non.Columns.Count; i++)
                    if (row.Cells[i].Value != null)
                        sols += row.Cells[i].Value.ToString() + ",";
                sols = "id in (" + sols.Substring(0, sols.Length - 1) + ")";

                frm_showdatabase fs = new frm_showdatabase();
                fs.dataGridView1.DataSource = Db.get_cases_customized_condition(statistics.tablename, sols);
                fs.Show();

            }*/
               
            foreach (DataGridViewRow row in dG_Non.SelectedRows)
               
            {
                    if (row.Cells[2].Value != null)
                        sols = "id  ="+row.Cells[2].Value.ToString() ;

                frm_showdatabase fs = new frm_showdatabase();
                fs.dataGridView1.DataSource = Db.get_cases_customized_condition(statistics.tablename, sols);
                fs.Show();

            }
       
        }

        private void dG_Prob_SelectionChanged(object sender, EventArgs e)
        {
            sols = "";
            /*  foreach (DataGridViewRow row in dG_Non.SelectedRows)
               
              {
                
                  for (int i = 2; i < dG_Non.Columns.Count; i++)
                      if (row.Cells[i].Value != null)
                          sols += row.Cells[i].Value.ToString() + ",";
                  sols = "id in (" + sols.Substring(0, sols.Length - 1) + ")";

                  frm_showdatabase fs = new frm_showdatabase();
                  fs.dataGridView1.DataSource = Db.get_cases_customized_condition(statistics.tablename, sols);
                  fs.Show();

              }*/

            foreach (DataGridViewRow row in dG_data.SelectedRows)
            {
                if (row.Cells[1].Value != null)
                    sols = "id  =" + row.Cells[1].Value.ToString();

                frm_showdatabase fs = new frm_showdatabase();
                fs.dataGridView1.DataSource = Db.get_cases_customized_condition(statistics.tablename, sols);
                fs.Show();

            }
        }

        private void dG_With_SelectionChanged(object sender, EventArgs e)
        {
            sols = "";

            foreach (DataGridViewRow row in dG_With.SelectedRows)
            {
                if (row.Cells[2].Value != null)
                    sols = "id  =" + row.Cells[2].Value.ToString();

                frm_showdatabase fs = new frm_showdatabase();
                fs.dataGridView1.DataSource = Db.get_cases_customized_condition(statistics.tablename, sols);
                fs.Show();

            }
       
        }

       

        private void frm_statistical_Load(object sender, EventArgs e)
        {

        }


  
      
        private void dG_data_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {

                dG_data.DataSource = Db.get_all_orderd(cmbox_case.SelectedValue.ToString(), "cluster");
                dg_clusters.DataSource = Db.get_count_cluster(cmbox_case.SelectedValue.ToString());
                return;
            }
            lstBox_Notes.Items.Add("Normalizing Data using MIn-Max Scaling");

            List<Case> standardize_current_cases = mybestchoice.StandardizeData(mybestchoice.ArrayCases);
            mykp = new KmeansPlus();
            KmeansPlus.PointClusters myclusters;
            List<KmeansPlus.PointClusters> Allmyclusters = new List<KmeansPlus.PointClusters>();
            // myclusters = mykp.GetKMeansPP(standardize_current_cases,int.Parse( Clusters_num));
            double min_fk = Double.MaxValue;
            int min_i = 0;
            for (int j = 0; j < 10; j++)
            {
                
                myclusters = new KmeansPlus.PointClusters();
                myclusters = mykp.GetKMeansPP_K(standardize_current_cases);
                Allmyclusters.Add(myclusters);
                if (myclusters.Fk < min_fk)
                    min_i = j;
            }
            myclusters = Allmyclusters[min_i];

           
            lstBox_Notes.Items.Add("Best Clusters Number = " + myclusters.PC.Count.ToString());

           
            // intialize db grids
            dG_data.Rows.Clear();
            dG_data.Columns.Clear();
            for (int j = 0; j < mybestchoice.Cases[0].GetFeatures().Count; j++)
            {
                Feature feature = (Feature)mybestchoice.Cases[0].GetFeatures()[j];
                dG_data.Columns.Add(feature.GetFeatureName().ToString(), feature.GetFeatureName().ToString());
            }
            dG_data.Rows.Add(mybestchoice.Cases.Count);

            
            int countcluster = myclusters.PC.Count;
            dg_clusters.Rows.Clear();
            dg_clusters.Columns.Clear();  
            dg_clusters.Columns.Add("cluster", "cluster");
            dg_clusters.Columns.Add("count", "count");
            dg_clusters.Rows.Add(countcluster+1);
            dg_clusters.Rows[0].Cells[0].Value = "عدد العناقيد الكلي";
            dg_clusters.Rows[0].Cells[1].Value = countcluster;
            int t = 1;
            int i = 0;
            foreach (List<Case> cluster in myclusters.PC.Values)
            {
                dg_clusters.Rows[t].Cells[0].Value = t.ToString();
                dg_clusters.Rows[t].Cells[1].Value = cluster.Count;
                foreach (Case c in cluster)
                {
                    Case realcase = mybestchoice.Cases.Find(
                        delegate(Case ca)
                        {
                            return ca.GetFeature("id").GetFeatureValue() == c.GetFeature("id").GetFeatureValue();
                        }
                    );
                    realcase.GetFeature("cluster").SetFeatureValue(t);
                    c.GetFeature("cluster").SetFeatureValue(t);
                    for (int j = 0; j < c.GetFeatures().Count; j++)
                    {
                        Feature f = (Feature)realcase.GetFeatures()[j];
                        dG_data.Rows[i].Cells[j].Value = f.GetFeatureValue();
                    }
                    i++;
                } // end feautres
                t++;
            } // end clusters

            dG_data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            mybestchoice.Clusters = myclusters;
        }

        private void radioButton2_Click_1(object sender, EventArgs e)
        {
            //cmbox_case.Items.Clear();

            dG_data.DataSource = null;
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = true;

            // Call the ShowDialog method to show the dialog box.
            DialogResult userClickedOK = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK.Equals(DialogResult.OK))
            {
                string url = openFileDialog1.FileName;
                // Open the selected file to read.
                System.IO.Stream fileStream = openFileDialog1.OpenFile();
                // تحميل الى datagrid
                IOCBRFile myfile = OCBRFileFactory.newInstance(url, true);
                ArrayList ArrayCases = myfile.GetCases();
                string CaseName = myfile.GetCaseName();
                List<Case> ListCases = myfile.GetListCases();
                mybestchoice = new BestChoice(ArrayCases, ListCases,"MN","KPP");
                if (ArrayCases.Count != 0)
                {
                    for (int j = 0; j < ListCases[0].GetFeatures().Count; j++)
                    {
                        Feature feature = (Feature)ListCases[0].GetFeatures()[j];
                        dG_data.Columns.Add(feature.GetFeatureName().ToString(), feature.GetFeatureName().ToString());
                    }

                    dG_data.Rows.Add(ArrayCases.Count);
                }
                for (int i = 0; i < ArrayCases.Count; i++)
                {
                    Case c = (Case)ArrayCases[i];
                    for (int j = 0; j < c.GetFeatures().Count; j++)
                    {
                        Feature f = (Feature)c.GetFeatures()[j];
                        dG_data.Rows[i].Cells[j].Value = f.GetFeatureValue();
                    }
                }

            }
            
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void dg_clusters_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ( dg_clusters.CurrentRow.Index == 0)
                for (int i = 0; i < dG_data.Rows.Count - 1; i++)
                        dG_data.Rows[i].Visible = true;
            else
            {
                string c = dg_clusters.CurrentRow.Cells["cluster"].Value.ToString();

                for (int i = 0; i < dG_data.Rows.Count - 1; i++)
                    if (dG_data.Rows[i].Cells["cluster"].Value.ToString() != c)
                        dG_data.Rows[i].Visible = false;
                    else
                        dG_data.Rows[i].Visible = true;
            }
            
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            cmbox_case.DataSource = Db.get_table_contetnt("tbl_cases");
            cmbox_case.DisplayMember = "case_name";
            cmbox_case.ValueMember = "tbl_name";
            dG_data.DataSource = null;
        }

        private void cmbox_case_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            if ((conn.State == ConnectionState.Closed) && (cmbox_case.SelectedValue.ToString() != "System.Data.DataRowView"))
            {
                DataTable mydt = Db.get_table_contetnt_condition("tbl_cases", "case_name", cmbox_case.Text);
                ArrayList myarr=Db.get_cases_as_arraylist(cmbox_case.SelectedValue.ToString(), cmbox_case.Text);
                mybestchoice = new BestChoice(myarr, mydt.Rows[0]["standrize_type"].ToString(), mydt.Rows[0]["kmeans_method"].ToString(), mydt.Rows[0]["clusters_num"].ToString());
                mybestchoice.Cases = Db.get_cases_condition(mybestchoice.Tablename_Standardization, mybestchoice.CaseName);
                statistics = new Statistics(mybestchoice.TableName, mybestchoice.CaseName);
                lstBox_Notes.Items.Add("Fetching data from database");
                dG_data.DataSource = Db.get_all_cases(cmbox_case.SelectedValue.ToString());
                DataTable countcluster = Db.get_count_cluster(mybestchoice.TableName);
                dg_clusters.Rows.Clear();
                dg_clusters.Columns.Clear();
                dg_clusters.Columns.Add("cluster", "cluster");
                dg_clusters.Columns.Add("count", "count");
                dg_clusters.Rows.Add(countcluster.Rows.Count + 1);

                dg_clusters.Rows[0].Cells[0].Value = "عدد العناقيد الكلي";
                dg_clusters.Rows[0].Cells[1].Value = countcluster.Rows.Count;
                for (int i=0; i<countcluster.Rows.Count;i++)
                {
                    dg_clusters.Rows[i+1].Cells[0].Value = countcluster.Rows[i][1];
                    dg_clusters.Rows[i+1].Cells[1].Value = countcluster.Rows[i][0];

                }
                List<Case>  centroids = Db.get_Centroids(mybestchoice.Tablename_Standardization,mybestchoice.CaseName);
                mybestchoice.Clusters = new KmeansPlus.PointClusters();
                for (int i = 0; i < centroids.Count; i++)
                    mybestchoice.Clusters.PC.Add(centroids[i], Db.get_cases_condition_cluster(mybestchoice.TableName, mybestchoice.CaseName, "cluster", centroids[i].GetFeature("id").GetFeatureValue().ToString()));

            }
            conn.Close();
               
           
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            List<string> criteria = Db.get_criteria(mybestchoice.CaseName);
           if (criteria.Count==0)
           {
               MessageBox.Show("توجد مشكلة -لايوجد معايير");
               return;
           }
            lbl_goal.Text += cmbox_case.Text;
            for (int i = 0; i < criteria.Count; i++)
               richTextBox1.Text += "معيار "+ (i+1).ToString() + "."+criteria[i]+"\n"  ;
            richTextBox2.Text += " عناصر قاعدة البيانات كاملة " + "\n";
            richTextBox2.Text += "الطريقة المقترحة :عناصر العنقود الأفضل عند استخدام IK-means++ ";
            

            

            string select_name = "select * from tbl_dic where caseid ='" + mybestchoice.CaseName + "'";
            SqlCommand cmd = new SqlCommand(select_name, conn);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            int p = sd.Fill(dt);

            dataGV_CaseEntries.Columns.Clear();
            DG_comparing.Columns.Clear();
            if (p != 0)
            {
                dataGV_CaseEntries.DataSource = dt;
                for (int i = 0; i < dataGV_CaseEntries.Columns.Count; i++)
                    dataGV_CaseEntries.Columns[i].Visible = false;

                dataGV_CaseEntries.Columns["FeatureName"].Visible = true;
                dataGV_CaseEntries.Columns["FeatureName"].HeaderText = "المعيار";
                dataGV_CaseEntries.Columns["FeatureUnit"].Visible = true;
                dataGV_CaseEntries.Columns["FeatureUnit"].HeaderText = "وحدة البيانات";
                dataGV_CaseEntries.Columns.Add("UserEntry", "تفضيلات المستخدم");
                if (dataGV_CaseEntries.Rows[0].Cells["FeatureName"].Value.ToString().ToUpper().Trim() == "id".ToUpper())
                {
                    dataGV_CaseEntries.Rows[0].Visible = false;
                    dataGV_CaseEntries.Rows[0].Cells["UserEntry"].Value = 0;
                }


            }

            for (int k = 0; k < criteria.Count + 1; k++)
                DG_comparing.Columns.Add("", "");
            DG_comparing.Rows.Add();
            DG_comparing.Rows[0].DefaultCellStyle.BackColor = Color.RoyalBlue;
            DG_comparing.Columns[0].DefaultCellStyle.BackColor = Color.RoyalBlue;

            frm_alternative.dg_bestcluster.Columns.Add("id", "id");
            frm_alternative.dg_allcases.Columns.Add("id", "id");
            for (int k = 0; k < criteria.Count; k++)
            {
                DG_comparing.Rows[0].Cells[k + 1].Value = criteria[k].ToString();
                dG_Prob.Columns.Add(criteria[k].ToString(), criteria[k].ToString());
                frm_alternative.dg_bestcluster.Columns.Add(criteria[k].ToString(), criteria[k].ToString());
                frm_alternative.dg_allcases.Columns.Add(criteria[k].ToString(), criteria[k].ToString());
            }

            frm_alternative.dg_bestcluster.Columns.Add("cluster", "cluster");
            frm_alternative.dg_allcases.Columns.Add("cluster", "cluster");
            int c = 0;
            for (int k = 0; k < criteria.Count; k++)
            {
                DG_comparing.Rows.Add();
                c++;
                DG_comparing.Rows[c].Cells[0].Value = criteria[k].ToString();
            }

            dG_Prob.Rows.Add();

            tabControl1.SelectTab(tabPage2);
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            statistics = new Statistics(mybestchoice.TableName, mybestchoice.CaseName);
            Case Max_Mean = Db.get_Case_data(statistics.standrize_table, -2);
            Case Min_Sd = Db.get_Case_data(statistics.standrize_table, -3);
            string url = "";
            System.IO.StreamReader reader = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            ArrayList inputs = new ArrayList();
            // Set filter options and filter index.
            openFileDialog1.Filter = "Text Files (.key)|*.key|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = true;

            // Call the ShowDialog method to show the dialog box.
            DialogResult userClickedOK = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK.Equals(DialogResult.OK))
            {
                url = openFileDialog1.FileName;
                reader = File.OpenText(url);
                if (reader == null)
                {
                    System.Console.WriteLine("file open failed " + url);
                    return;
                }

                for (int i = 0; i < dataGV_CaseEntries.Rows.Count; i++)
                {
                    string s = reader.ReadLine();
                    dataGV_CaseEntries.Rows[i].Cells["UserEntry"].Value = s;
                    inputs.Add(s);

                }
                reader.Close();
            }



            Case _problem = new Case(0, mybestchoice.CaseName, "");
            _problem.AddFeature("id", FeatureType.TYPE_FEATURE_INT, 0, 1.0, false, false, "num");


            for (int i = 0; i < dataGV_CaseEntries.Rows.Count - 1; i++)
                if (dataGV_CaseEntries.Rows[i].Cells["UserEntry"].Value.ToString() != "".ToString())
                {
                    _problem.AddFeature(
                        dataGV_CaseEntries.Rows[i].Cells["FeatureName"].Value.ToString().Trim(),
                        Token.GetType(dataGV_CaseEntries.Rows[i].Cells["FeatureType"].Value.ToString().Trim()),
                        dataGV_CaseEntries.Rows[i].Cells["UserEntry"].Value.ToString().Trim(),
                        Convert.ToDouble(dataGV_CaseEntries.Rows[i].Cells["FeatureWeight"].Value.ToString().Trim()),
                        Convert.ToBoolean(dataGV_CaseEntries.Rows[i].Cells["FeatureKey"].Value.ToString().Trim()),
                        Convert.ToBoolean(dataGV_CaseEntries.Rows[i].Cells["FeatureIndex"].Value.ToString().Trim()),
                        dataGV_CaseEntries.Rows[i].Cells["FeatureUnit"].Value.ToString().Trim());
                      
                }
           
            _problem.AddFeature("cluster", FeatureType.TYPE_FEATURE_INT, 0, 1.0, false, false, "num");
            statistics.exp_random_cases.Add(BestChoice.StandardizeCase(_problem, Max_Mean, Min_Sd, statistics.standrize_type));
            statistics.exp_random_cases_nonstandrize.Add(_problem);
            mybestchoice.Problem = statistics.exp_random_cases_nonstandrize[0];
            mybestchoice.StandrizeProblem = statistics.exp_random_cases[0];
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            //criteria-------------------------------------------------------------------------------------------------------

            string url = "";
            System.IO.StreamReader reader = null;
            System.IO.FileStream fs = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            // Set filter options and filter index.
            openFileDialog1.Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = true;

            // Call the ShowDialog method to show the dialog box.
            DialogResult userClickedOK = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK.Equals(DialogResult.OK))
            {
                url = openFileDialog1.FileName;
                fs = File.OpenRead(url);
                reader = new StreamReader(fs);
                if (reader == null)
                {
                    System.Console.WriteLine("file open failed " + url);
                    return;
                }
                
                int linecount = 1;
                CriteriaArr = new double[DG_comparing.Columns.Count, DG_comparing.Columns.Count];
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(' ');
                    for (int i = 0; i < DG_comparing.Columns.Count - 1; i++)
                    {
                        DG_comparing.Rows[linecount].Cells[i + 1].Value = values[i];
                         CriteriaArr[linecount, i] = Convert.ToDouble(values[i]);
                    }
                    linecount++;
                    
                }
                fs.Close();
                
            }
        }

        private void radioButton4_Click(object sender, EventArgs e)
        {
            GenerateComparison.Create_Criteria_Comparison_Array(mybestchoice.Cases[0].GetFeatures().Count - 2);
            for (int i = 0; i < DG_comparing.Columns.Count - 1; i++)
            { 
                for (int j = 0; j < DG_comparing.Columns.Count - 1; j++)
                    DG_comparing.Rows[i+1].Cells[ j + 1].Value = GenerateComparison.CriteriaComparisonMatrix[i,j];
             }
            CriteriaArr = GenerateComparison.CriteriaComparisonMatrix;
            
            }

        private void button19_Click_1(object sender, EventArgs e)
        {
            double[] CriteriaWeights;
            if (MyAHPModel._CalculatedCriteria(CriteriaArr, out CriteriaWeights))
              for (int i = 0; i <  dG_Prob.Columns.Count;i++)
              {
                dG_Prob.Rows[0].Cells[i].Value = Math.Round(CriteriaWeights[i] * 100, 2);
              }
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
             
            List<Case> Centroids = new List<Case>();
             foreach (Case centroid in mybestchoice.Clusters.PC.Keys)
                 Centroids.Add(centroid);
             mykp = new KmeansPlus();
             clusterid = mykp.FindCluster_Customize(mybestchoice.StandrizeProblem, Centroids);
            BestCluster= mybestchoice.Clusters.PC.ElementAt(clusterid).Value;
            frm_alternative.dg_bestcluster.Rows.Add(BestCluster.Count);
             for (int i = 0; i < BestCluster.Count; i++)
             {
                 Case mycase = BestCluster[i];
                  for (int j = 0; j < mycase.GetFeatures().Count; j++)
                  {
                      Feature f = (Feature)mycase.GetFeatures()[j];
                      frm_alternative.dg_bestcluster.Rows[i].Cells[j].Value = f.GetFeatureValue();
                  }
             }

             listBox1.Items.Add("عدد عناصر العنقود المناسب =" + BestCluster.Count);
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click_3(object sender, EventArgs e)
        {
            frm_alternative.dg_allcases.Rows.Add(mybestchoice.Cases.Count);
            for (int i = 0; i < mybestchoice.Cases.Count; i++)
            {
                Case mycase = mybestchoice.Cases[i];
                for (int j = 0; j < mycase.GetFeatures().Count; j++)
                {
                    Feature f = (Feature)mycase.GetFeatures()[j];
                    frm_alternative.dg_allcases.Rows[i].Cells[j].Value = f.GetFeatureValue();
                }
            }
            listBox1.Items.Add("عدد عناصر قاعدة البيانات =" + mybestchoice.Cases.Count);
        }

      


        private void radioButton7_Click(object sender, EventArgs e)
        {
             GenerateComparison.Create_All_Criteria_Choice_Comparison_Array(BestCluster);
             ChoicesArr_With = GenerateComparison.ChoicesComparisonMatrix;
             int count_solution = ChoicesArr_With[0].GetLength(0);
            for (int j = 0; j < count_solution+1; j++)
                dG_With.Columns.Add("","");

            dG_With.Columns[0].DefaultCellStyle.BackColor = Color.RoyalBlue;
            
            List<string> criteria= Db.get_criteria(mybestchoice.CaseName);
            int rowcount=0;
            for (int i = 0; i < criteria.Count; i++)
            {
                dG_With.Rows.Add();
                dG_With.Rows[rowcount].DefaultCellStyle.BackColor = Color.RoyalBlue;
                dG_With.Rows[rowcount].Cells[0].Value = criteria[i];
                for (int j = 0; j < BestCluster.Count; j++)
                    dG_With.Rows[rowcount].Cells[j + 1].Value = BestCluster[j].GetFeature("id").GetFeatureValue().ToString();

                for (int j = 0; j < BestCluster.Count; j++)
                {
                    dG_With.Rows.Add();
                    rowcount++;
                    
                    for (int k = 0; k < BestCluster.Count; k++)
                    {
                        dG_With.Rows[rowcount].Cells[0].Value = BestCluster[j].GetFeature("id").GetFeatureValue().ToString();
                        dG_With.Rows[rowcount].Cells[k + 1].Value = ChoicesArr_With[i][j, k];
                    }
                }
            }
            
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton5_Click(object sender, EventArgs e)
        {
            GenerateComparison.Create_All_Criteria_Choice_Comparison_Array(mybestchoice.Cases);
            ChoicesArr_Non=  GenerateComparison.ChoicesComparisonMatrix;
            int count_solution = ChoicesArr_Non[0].GetLength(0);
            for (int j = 0; j < count_solution + 1; j++)
                dG_Non.Columns.Add("", "");

            dG_Non.Columns[0].DefaultCellStyle.BackColor = Color.RoyalBlue;

            List<string> criteria = Db.get_criteria(mybestchoice.CaseName);
            int rowcount = 0;
            for (int i = 0; i < criteria.Count; i++)
            {
                dG_Non.Rows.Add();
                dG_Non.Rows[rowcount].DefaultCellStyle.BackColor = Color.RoyalBlue;
                dG_Non.Rows[rowcount].Cells[0].Value = criteria[i];
                for (int j = 0; j < mybestchoice.Cases.Count; j++)
                    dG_Non.Rows[rowcount].Cells[j + 1].Value = mybestchoice.Cases[j].GetFeature("id").GetFeatureValue().ToString();

                for (int j = 0; j < mybestchoice.Cases.Count; j++)
                {
                    dG_Non.Rows.Add();
                    rowcount++;

                    for (int k = 0; k < mybestchoice.Cases.Count; k++)
                    {
                        dG_Non.Rows[rowcount].Cells[0].Value = mybestchoice.Cases[j].GetFeature("id").GetFeatureValue().ToString();
                        dG_Non.Rows[rowcount].Cells[k + 1].Value = ChoicesArr_Non[i][j, k];
                    }
                }
            }
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            int c;
            int dG_WithN = 0;
            double[] ranks;

            ahp_sol_clustering = new MyAHPModel(CriteriaArr.GetLength(0), BestCluster.Count);
            ahp_sol_clustering.AddCriteria(CriteriaArr);
            ahp_sol_clustering.AddCriterionRatedChoices(ChoicesArr_With);
            if (ahp_sol_clustering.CalculatedCriteria())
            { 
                ranks = ahp_sol_clustering.CalculatedChoices(out c);
                if (c!=-1)
                {
                    MessageBox.Show("عدم تناسق في تفضيلات البدائل المدخلة للمعيار رقم"+c.ToString());
                    return;
                }
            }

            else
            {
                MessageBox.Show("عدم تناسق في تفضيلات المعايير المدخلة");
                return;
            }
            dg_results_clustering.Columns.Add("SolutionID", "Solution ID");
            dg_results_clustering.Columns.Add("SolutionWeight", "Solution Weight");
            dg_results_clustering.Columns.Add("SolutionSim", "Solution Similarity");
            for (int i = 0; i < ranks.Length; i++)
            {

                dg_results_clustering.Rows.Add();
                dg_results_clustering.Rows[dG_WithN].Cells[0].Value = BestCluster[i].GetFeature("id").GetFeatureValue().ToString();
                dg_results_clustering.Rows[dG_WithN].Cells[1].Value = Math.Round(ranks[i] * 100, 2);
                EuclideanSimilarity s = new EuclideanSimilarity();
                List<Case> sols = Db.get_cases_condition2(mybestchoice.Tablename_Standardization, mybestchoice.CaseName, "cluster", clusterid.ToString());
                dg_results_clustering.Rows[dG_WithN].Cells[2].Value = Math.Round(s.Similarity(sols[i], mybestchoice.StandrizeProblem) * 100, 2);
                dG_WithN++;
            }
            dg_results_clustering.Sort(dg_results_clustering.Columns["SolutionWeight"], ListSortDirection.Descending);

        }

        private void button13_Click_2(object sender, EventArgs e)
        {
            int c;
            double[] ranks ;
            int dG_With = 0;
            ahp_sol_no_clustering = new MyAHPModel(CriteriaArr.GetLength(0), mybestchoice.Cases.Count);
            ahp_sol_no_clustering.AddCriteria(CriteriaArr);
            ahp_sol_no_clustering.AddCriterionRatedChoices(ChoicesArr_Non);
            if (ahp_sol_no_clustering.CalculatedCriteria())
            {
                ranks = ahp_sol_no_clustering.CalculatedChoices(out c);
                if (c != -1)
                {
                    MessageBox.Show("عدم تناسق في تفضيلات البدائل المدخلة للمعيار رقم" + c.ToString());
                    return;
                }
            }

            else
            {
                MessageBox.Show("عدم تناسق في تفضيلات المعايير المدخلة");
                return;
            }
           
           
             
           

            dg_results_no_clustering.Columns.Add("SolutionID", "Solution ID");
            dg_results_no_clustering.Columns.Add("SolutionWeight", "Solution Weight");
            dg_results_no_clustering.Columns.Add("SolutionSim", "Solution Similarity");
            for (int i = 0; i < ranks.Length; i++)
            {

                dg_results_no_clustering.Rows.Add();
                dg_results_no_clustering.Rows[dG_With].Cells[0].Value = mybestchoice.Cases[i].GetFeature("id").GetFeatureValue().ToString();
                dg_results_no_clustering.Rows[dG_With].Cells[1].Value = Math.Round(ranks[i] * 100, 2);
                EuclideanSimilarity s = new EuclideanSimilarity();
                List<Case> sols = Db.get_cases_condition(mybestchoice.Tablename_Standardization, mybestchoice.CaseName);
                dg_results_no_clustering.Rows[dG_With].Cells[2].Value = Math.Round(s.Similarity(sols[i], mybestchoice.StandrizeProblem) * 100, 2);
                dG_With++;
            }

            dg_results_no_clustering.Sort(dg_results_no_clustering.Columns["SolutionWeight"], ListSortDirection.Descending);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frm_alternative.Show();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tabPage3);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tabPage4);
        }



       
        
    }
}

 
