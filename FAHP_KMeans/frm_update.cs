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
    public partial class frm_update : Form
    {
        public static bool ObjCreated = false;
     
        ArrayList ArrayCases = new ArrayList();
        List<Case> ListCases = new List<Case>();
        BestChoice choice = null;
        public frm_update()
        {
            InitializeComponent();
            ObjCreated = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
          



            
          
            
        }

        public bool add_file()
        {

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("حدد حالة للتحديث من فضلك");
                return false;
            }
            string url = "";
            string CaseName = "";
            string TableName = "";

           // dataGridView_updated.Columns.Clear();
           // dataGridView_updated.DataSource = null;
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
                IOCBRFile myfile = OCBRFileFactory.newInstance(url, true);
                ArrayCases = myfile.GetCases();
                ListCases = myfile.GetListCases();

                CaseName = myfile.GetCaseName();
                TableName = dataGridView1.SelectedRows[0].Cells["tbl_name"].Value.ToString();
                if (CaseName != dataGridView1.SelectedRows[0].Cells["case_name"].Value.ToString())
                {
                    MessageBox.Show("عدم تطابق بين الحالة المدخلة والمحددة");
                    return false;
                }

              /*  if (ArrayCases.Count != 0)
                {

                    for (int j = 0; j < ListCases[0].GetFeatures().Count; j++)
                    {
                        Feature feature = (Feature)ListCases[0].GetFeatures()[j];
                        dataGridView_updated.Columns.Add(feature.GetFeatureName().ToString(), feature.GetFeatureName().ToString());
                    }
                    dataGridView_updated.Rows.Add(ArrayCases.Count);
                }
                for (int i = 0; i < ArrayCases.Count; i++)
                {
                    Case c = (Case)ArrayCases[i];
                    for (int j = 0; j < c.GetFeatures().Count; j++)
                    {
                        Feature f = (Feature)c.GetFeatures()[j];
                        dataGridView_updated.Rows[i].Cells[j].Value = f.GetFeatureValue();
                    }
                }*/

            }
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void frm_update_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            List<string> ids = new List<string>();
            List<Case> standrize_array = new List<Case>();

            //get ids od cases to delete
            for (int i = 0; i < dataGridView_old.SelectedRows.Count; i++)
                ids.Add(dataGridView_old.SelectedRows[i].Cells["id"].Value.ToString());
            // delete cases from tables
            Db.del_case_id(choice.TableName, ids);
            Db.del_case_id(choice.Tablename_Standardization, ids);
            // re_standrize all tables
            ArrayList array = Db.get_cases_as_arraylist_condition(choice.Tablename_Standardization, choice.CaseName);
            standrize_array = choice.StandardizeData(array);
             Db.del_data_table(choice.Tablename_Standardization);
             standrize_array.Add(choice.Case_Max_Mean);
             standrize_array.Add(choice.Case_Min_SD);
            int num = Convert.ToInt32(choice.Clusters_Num);
              for(int i=0; i < num; i++) // recalc centroids
                    {
                        
                       
                        List<Case> cases_cluster = standrize_array.FindAll(
                            delegate(Case ca)
                            {
                                return ca.GetFeature("cluster").GetFeatureValue().ToString() == i.ToString();
                            }
                        );
                       Case centroid= Kmeans.ReCalcCentroid(cases_cluster, i);
                       standrize_array.Add(centroid);
              }
            // add all after restandrize
             Db.insert_data_to_standrize_caseTable_noID(choice.Tablename_Standardization,choice.get_rows_as_string(standrize_array));
             dataGridView_old.DataSource = Db.get_table_contetnt(choice.TableName);

        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ArrayList myarr = Db.get_cases_as_arraylist_condition(dataGridView1.SelectedRows[0].Cells["tbl_name"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["case_name"].Value.ToString());
            choice = new BestChoice(myarr, dataGridView1.SelectedRows[0].Cells["standrize_type"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["kmeans_method"].Value.ToString(), dataGridView1.SelectedRows[0].Cells["clusters_num"].Value.ToString());
            dataGridView_old.DataSource = Db.get_table_contetnt(dataGridView1.SelectedRows[0].Cells["tbl_name"].Value.ToString());
            dataGridView1.AutoSizeColumnsMode =DataGridViewAutoSizeColumnsMode.AllCells; 
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            if (add_file())
            {
                List<Case> centroids = new List<Case>();
                centroids = Db.get_Centroids(choice.Tablename_Standardization, choice.CaseName);
                List<string> Ids = new List<string>();
                for (int i = 0; i < ListCases.Count; i++)
                {
                    //Case s_case = new Case(0, ListCases[0].GetCaseName(), ListCases[0].GetCaseDescription());
                    // تقييس الداتا سيت بناءا على العنصر الجديد
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    
                    List<Case> restandrize_array = choice.Re_standrize(ListCases[i]);
                    // العنصر الجديد بعد التقييس
                    Case s_case = restandrize_array.Last();

                    // نرى إلى أي عنقود ستمم اضافته
                    Case result_centroid = choice.Updated_cluster(s_case, centroids, restandrize_array);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    label2.Text = "with clustring time  " + elapsedMs.ToString();
                    // سيتم انضمامه لعنقود موجود
                    if (result_centroid != null)
                    {
                        
                        Feature f = (Feature)result_centroid.GetFeatures()[0];
                        s_case.GetFeature("cluster").SetFeatureValue(f.GetFeatureValue()); // اسناد العنقود المناسب للعنصر الجديد
                        Db.update_cluster(choice.Tablename_Standardization, s_case.GetFeature("id").GetFeatureValue().ToString(), Convert.ToInt32(f.GetFeatureValue()));
                        Db.update_cluster(choice.TableName, s_case.GetFeature("id").GetFeatureValue().ToString(), Convert.ToInt32(f.GetFeatureValue()));
                        Ids.Add(f.GetFeatureValue().ToString());
                   
                    }// end if
                    else // عنقود جديد
                    {
                        int k = Convert.ToInt32(choice.Clusters_Num) + 1;
                        // اضافة عنقود جديد
                        Db.update_cluster(choice.Tablename_Standardization, s_case.GetFeature("id").GetFeatureValue().ToString(), k);
                        Db.update_cluster(choice.TableName, s_case.GetFeature("id").GetFeatureValue().ToString(), k);
                        Case new_centroid = s_case;
                        new_centroid.GetFeature("cluster").SetFeatureValue(-1);
                        Db.insert_case_to_table(new_centroid, choice.Tablename_Standardization);
                        choice.Clusters_Num = k.ToString();
                        Db.update_clusterNum(choice.CaseName, k);
                    }
                } // end for all new cases
                if (Ids != null)
                {
                    Db.del_centroids(choice.Tablename_Standardization, Ids);
                    double ess = 0;
                    //   من ضمن الداتا سيت المقييسة يتم حساب المراكز لجميع العناقيد
                    foreach (string id in Ids)
                    {
                        List<Case> S_Arr_Cluster = Db.get_cases_condition_cluster(choice.Tablename_Standardization, choice.CaseName, "cluster", id);
                        Feature cen = S_Arr_Cluster[0].GetFeature("cluster");
                        Case new_centroid = Kmeans.ReCalcCentroid(S_Arr_Cluster, Convert.ToInt32(cen.GetFeatureValue()));
                        EuclideanSimilarity es = new EuclideanSimilarity();
                        double d = 0;
                        foreach (Case c in S_Arr_Cluster)
                            d += es.Dissimilarity(c, new_centroid);

                        ess += d;

                        Db.insert_case_to_table(new_centroid, choice.Tablename_Standardization);
                    }
                    //  حساب distorion error
                    MessageBox.Show(ess.ToString());
                }
                dataGridView_old.DataSource = Db.get_table_contetnt(choice.TableName);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (add_file())
            {
                List<Case> centroids = new List<Case>();
                centroids = Db.get_Centroids(choice.Tablename_Standardization, choice.CaseName);
                for (int i = 0; i < ListCases.Count; i++)
                {
                    // Case s_case = new Case(0, ListCases[0].GetCaseName(), ListCases[0].GetCaseDescription());
                    List<Case> restandrize_array = choice.Re_standrize(ListCases[i]);//, out s_case);
                    Case s_case = restandrize_array.Last();
                    Case result_centroid = choice.Updated_cluster_Min_Centroid(s_case, centroids, restandrize_array);
                    if (result_centroid != null)
                    {
                        Feature f = (Feature)result_centroid.GetFeatures()[0];
                        s_case.GetFeature("cluster").SetFeatureValue(f.GetFeatureValue()); // اسناد العنقود المناسب للعنصر الجديد
                        Db.update_cluster(choice.Tablename_Standardization, s_case.GetFeature("id").GetFeatureValue().ToString(), Convert.ToInt32(f.GetFeatureValue()));
                        Db.update_cluster(choice.TableName, s_case.GetFeature("id").GetFeatureValue().ToString(), Convert.ToInt32(f.GetFeatureValue()));
                        double ess = 0;
                        //   من ضمن الداتا سيت المقييسة يتم حساب المراكز لجميع العناقيد
                        for (int k = 0; k < Convert.ToInt32(choice.Clusters_Num); k++)
                        {
                            List<Case> S_Arr_Cluster = Db.get_cases_condition_cluster(choice.Tablename_Standardization, choice.CaseName, "cluster", k.ToString());
                            Feature cen = S_Arr_Cluster[0].GetFeature("cluster");
                            Case new_centroid = Kmeans.ReCalcCentroid(S_Arr_Cluster, Convert.ToInt32(cen.GetFeatureValue()));
                            Db.insert_case_to_table(new_centroid, choice.Tablename_Standardization);
                            EuclideanSimilarity es = new EuclideanSimilarity();
                            double d = 0;
                            foreach (Case c in S_Arr_Cluster)
                                d += es.Dissimilarity(c, new_centroid);

                            ess += d;
                        }
                        MessageBox.Show(ess.ToString());
                    }
                }
                dataGridView_old.DataSource = Db.get_table_contetnt(choice.TableName);

            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {

        }
    }
}
