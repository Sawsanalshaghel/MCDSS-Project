using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fahp_cbr_app
{
    public partial class frm_input : Form
    {
         public static bool ObjCreated = false;
         BestChoice mybestchoice ;
         ArrayList ArrayCases = new ArrayList();
         List<Case>  ListCases = new List<Case>();
         List<Case> standardize_current_cases = new List<Case>();
         string url = "";
         string CaseName = "";
         class Cluster
         {
             public List<List<Case>> clusters = new List<List<Case>>();
             public double fk = 0;
             public double sk = 0;
             public int k = 0;
         }
        public frm_input()
        {
            InitializeComponent();
            ObjCreated = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void frm_input_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
           
            string Standrize_Type = "";
            if (rbtn_normalize.Checked)
                Standrize_Type = "Q";

            else if (rbtn_minmax.Checked)
                Standrize_Type = "MN";
            else
                Standrize_Type = "none";

            string Kmeans_Method = "";
            if (rbtn_kmeansPlus.Checked)
                Kmeans_Method = "KPP";

            else if (rbtn_simpleKmean.Checked)
                Kmeans_Method = "K";
            else
                Kmeans_Method = "none";

            string Clusters_num = txt_cluster.Text;


            mybestchoice = new BestChoice(ArrayCases, Standrize_Type, Kmeans_Method, Clusters_num);
            standardize_current_cases = mybestchoice.StandardizeData(ArrayCases);


            if (rbtn_simpleKmean.Checked)
            {
               // Kmeans kmeans = new Kmeans(int.Parse(txt_cluster.Text), standardize_current_cases);
               // kmeans.run();

                // Number of Attributes
                
                List<Cluster> AllClusters=new List<Cluster>();
                AllClusters.Add(null);
                Cluster myc = new Cluster();
                int Nd = standardize_current_cases[0].GetFeatures().Count - 2;//except id , cluster
                Kmeans kmeans = new Kmeans(1, standardize_current_cases);
                kmeans.run();
                myc.clusters = kmeans.clusters;
                foreach (double sk in kmeans.distances)
                    myc.sk += sk;
                myc.fk = 1;
                myc.k = 1;
                AllClusters.Add(myc);
                for (int ik = 2; ik <= 5; ik++)//19
                {
                    myc = new Cluster();
                    kmeans = new Kmeans(ik, standardize_current_cases);
                    kmeans.run();
                    myc.k = ik;
                    myc.clusters = kmeans.clusters;
                    foreach (double sk in kmeans.distances)
                        myc.sk += sk;
                    if (AllClusters[ik - 1].sk == 0)
                        myc.fk = 1;
                    else
                    {
                        double ak = 1;
                          if (Nd > 1)
                            ak=Convert.ToDouble(kmeans.Ak(ik, Nd));
                          myc.fk = myc.sk / (ak * AllClusters[ik - 1].sk);
                         }
                    AllClusters.Add(myc);
                }

               
            double min=double.MaxValue;
            int kk =0;
            for (int ik = 1; ik <= 19; ik++)
            {
                if (AllClusters[ik].fk < min)
                { kk = AllClusters[ik].k; kmeans.clusters = AllClusters[ik].clusters; min = AllClusters[ik].fk; }
                
            }
                watch.Stop();
                textBox1.Text = min.ToString();
                txt_cluster.Text = kk.ToString();
                var elapsedMs = watch.ElapsedMilliseconds;
                label3.Text = "with clustring time  " + elapsedMs.ToString();
                int t = 0;
                int i = 0;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                for (int j = 0; j < ListCases[0].GetFeatures().Count; j++)
                {
                    Feature feature = (Feature)ListCases[0].GetFeatures()[j];
                    dataGridView1.Columns.Add(feature.GetFeatureName().ToString(), feature.GetFeatureName().ToString());
                }

                dataGridView2.Rows.Clear();
                dataGridView2.Columns.Clear();
                dataGridView1.Rows.Add(ArrayCases.Count);


                dataGridView2.Columns.Add("cluster", "cluster");
                dataGridView2.Columns.Add("count", "count");
                dataGridView2.Rows.Add(kmeans.clusters.Count);
               
                foreach (List<Case> list in kmeans.clusters) // for each cluster, define a new centroid
                {
                    kmeans.centroids[t].GetFeature("id").SetFeatureValue(t);
                    standardize_current_cases.Add(kmeans.centroids[t]);
                    dataGridView2.Rows[t].Cells[0].Value =t.ToString();
                    dataGridView2.Rows[t].Cells[1].Value = list.Count;
                    mybestchoice.Cost_Function += kmeans.distances[t];
                    foreach (Case c in list) // Determine the poit on clusters
                    {
                        
                       
                        Case realcase = ListCases.Find(
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
                            dataGridView1.Rows[i].Cells[j].Value = f.GetFeatureValue();
                        }
                        i++; 
                        
                    }
                    t++;
                }
               
            }
            else if (rbtn_kmeansPlus.Checked)
            {

                KmeansPlus mykp = new KmeansPlus();
                KmeansPlus.PointClusters myclusters;
                List< KmeansPlus.PointClusters> Allmyclusters = new List<KmeansPlus.PointClusters>();
               // myclusters = mykp.GetKMeansPP(standardize_current_cases,int.Parse( Clusters_num));
                double min_fk = Double.MaxValue;
                int min_i=0;
                for (int j = 0; j < 100; j++)
                {
                    myclusters = new KmeansPlus.PointClusters();
                    myclusters = mykp.GetKMeansPP_K(standardize_current_cases);
                    Allmyclusters.Add(myclusters);
                    if (myclusters.Fk < min_fk)
                        min_i = j;
                }
                myclusters = Allmyclusters[min_i];
               // KmeansPlus mykp = new KmeansPlus();
               // KmeansPlus.PointClusters myclusters =new KmeansPlus.PointClusters();;
              //  myclusters = mykp.GetKMeansPP_K(standardize_current_cases);
                textBox1.Text = myclusters.Fk.ToString();
                txt_cluster.Text = myclusters.PC.Count.ToString();
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                label3.Text = "with clustring time  " + elapsedMs.ToString();

                foreach (Case centroid in myclusters.PC.Keys)
                    standardize_current_cases.Add(centroid);


                // intialize db grids

                int t = 0;
                int i = 0;
                int countcluster = myclusters.PC.Count;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                for (int j = 0; j < ListCases[0].GetFeatures().Count; j++)
                {
                    Feature feature = (Feature)ListCases[0].GetFeatures()[j];
                    dataGridView1.Columns.Add(feature.GetFeatureName().ToString(), feature.GetFeatureName().ToString());
                }

                dataGridView2.Rows.Clear();
                dataGridView2.Columns.Clear();
                dataGridView1.Rows.Add(ArrayCases.Count);


                dataGridView2.Columns.Add("cluster", "cluster");
                dataGridView2.Columns.Add("count", "count");
                dataGridView2.Rows.Add(countcluster);

                foreach (List<Case> cluster in myclusters.PC.Values)
                {
                    dataGridView2.Rows[t].Cells[0].Value = t.ToString();
                    dataGridView2.Rows[t].Cells[1].Value = cluster.Count;
                    foreach (Case c in cluster)
                    {
                        Case realcase = ListCases.Find(
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
                            dataGridView1.Rows[i].Cells[j].Value = f.GetFeatureValue();
                        }
                        i++;
                    } // end feautres
                    t++;
                } // end clusters

            } // end if
            else
                if (rbtn_nonekmeans.Checked)
                    dataGridView1.DataSource = Db.get_table_contetnt(mybestchoice.TableName);
            
            // for sorting
            comboBox1.Items.Clear();
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    comboBox1.Items.Add(dataGridView1.Columns[i].HeaderText);
                comboBox1.Text = "id";


                dataGridView1.AutoSizeColumnsMode =
       DataGridViewAutoSizeColumnsMode.AllCells;  
     
        }

        private void button1_Click(object sender, EventArgs e)
        {

        
         
            comboBox1.Items.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = null;
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
                url = openFileDialog1.FileName;
                // Open the selected file to read.
                System.IO.Stream fileStream = openFileDialog1.OpenFile();
              // تحميل الى datagrid
                IOCBRFile myfile = OCBRFileFactory.newInstance(url,true);
                 ArrayCases = myfile.GetCases();
                 CaseName = myfile.GetCaseName();
                 ListCases = myfile.GetListCases();
                 
                 if (ArrayCases.Count != 0)
                 {
                     for (int j = 0; j < ListCases[0].GetFeatures().Count; j++)
                     { 
                         Feature feature = (Feature)ListCases[0].GetFeatures()[j];
                         dataGridView1.Columns.Add(feature.GetFeatureName().ToString(), feature.GetFeatureName().ToString());
                     }

                     dataGridView1.Rows.Add(ArrayCases.Count);
                 }
                for (int i=0;i < ArrayCases.Count;i++)
                {
                    Case c = (Case)ArrayCases[i];
                     for (int j=0;j < c.GetFeatures().Count;j++)
                     {
                         Feature f = (Feature)c.GetFeatures()[j];
                         dataGridView1.Rows[i].Cells[j].Value = f.GetFeatureValue();
                     }
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }



        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {

                //dataGridView1.Sort(dataGridView1.Columns[comboBox1.Text], ListSortDirection.Ascending);
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int result = Db.is_case_exist(CaseName);

            if (result != 0)
            {
                DialogResult result1 = MessageBox.Show("اسم حالة دراسة مكرر، هل تريد حذف بياناتها وادخالها من جديد ",
                "بيانات مكررة",
                MessageBoxButtons.YesNo);
                if (result1 == DialogResult.No)
                    return;
            }


            Db.del_repeated_case(CaseName);
            try
            {
                mybestchoice.LoadCases();
                string[] s = mybestchoice.get_rows_as_string(standardize_current_cases);
                Db.insert_data_to_standrize_caseTable(mybestchoice.Tablename_Standardization, s, mybestchoice.Features_without_Types);
                Db.insert_case_to_table(mybestchoice.Case_Max_Mean, mybestchoice.Tablename_Standardization);
                Db.insert_case_to_table(mybestchoice.Case_Min_SD, mybestchoice.Tablename_Standardization);
              
            }
            catch
            {
                MessageBox.Show("حدث خطأما");
                return;
            }

            MessageBox.Show("تم الحفظ بنجاح");
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {

            
        }

        private void dataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string c = dataGridView2.CurrentRow.Cells["cluster"].Value.ToString();

            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                if (dataGridView1.Rows[i].Cells["cluster"].Value.ToString() != c)
                    dataGridView1.Rows[i].Visible = false;
                else
                    dataGridView1.Rows[i].Visible = true;
        }
    }
}
