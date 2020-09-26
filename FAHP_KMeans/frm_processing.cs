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
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Xml;
using DotNetMatrix;
using Net.Kniaz.AHP;
using System.IO;


namespace Fahp_cbr_app
{
    public partial class frm_processing : Form
    {
        SqlConnection conn = new SqlConnection(@" Data Source=.;Initial Catalog=PhoneDB;Integrated Security=True");
        public static bool ObjCreated = false;
        bool input_file = false;
        static int real_choices = 0;
        static string[,] arr = new string[7, 2];
        
        static ArrayList Alternatives = new ArrayList();
        private List<Case> current_cases = new List<Case>();
        private List<Case> all_cases = new List<Case>();
        private Dictionary<Case, double> sol_cases;
        private ArrayList current_cases_partial = new ArrayList();
        BestChoice mybestchoice;
        DataTable finalresult;
        int count_solution = 0;
        int count_citeria = 0;
        Dictionary<Case, List<Case>> AllNonClusterdCases = new Dictionary<Case, List<Case>>();
        Dictionary<Case, List<Case>> AllclusterdCases = new Dictionary<Case, List<Case>>();
        Dictionary<Case, double[,]> AllNonClusterdResults = new Dictionary<Case, double[,]>();
        Dictionary<Case, double[,]> AllclusterdResults = new Dictionary<Case, double[,]>();
        List<double[,]> AllCriteriaComp = new List<double[,]>();
        List<Case> exp_random_cases = new List<Case>();
        Case _problem;
        //double[,] choices_arr ;
        public frm_processing()
        {
            InitializeComponent();
            ObjCreated = true;
        }

        private void sort_without_clustering()
        {
           
            // soting Without Clustering
            for (int h = 0; h < AllNonClusterdResults.Count; h++)
                if (AllNonClusterdResults.ElementAt(h).Value.Length > 2)
                    for (int i = 0; i < AllNonClusterdResults.ElementAt(h).Value.GetLength(0) - 1; i++)
                        for (int j = 0; j < AllNonClusterdResults.ElementAt(h).Value.GetLength(0) - i - 1; j++)
                            if (AllNonClusterdResults.ElementAt(h).Value[j, 1] < AllNonClusterdResults.ElementAt(h).Value[j + 1, 1])
                            {
                                double temp = AllNonClusterdResults.ElementAt(h).Value[j, 1];
                                AllNonClusterdResults.ElementAt(h).Value[j, 1] = AllNonClusterdResults.ElementAt(h).Value[j + 1, 1];
                                AllNonClusterdResults.ElementAt(h).Value[j + 1, 1] = temp;
                                temp = AllNonClusterdResults.ElementAt(h).Value[j, 0];
                                AllNonClusterdResults.ElementAt(h).Value[j, 0] = AllNonClusterdResults.ElementAt(h).Value[j + 1, 0];
                                AllNonClusterdResults.ElementAt(h).Value[j + 1, 0] = temp;
                            }


            
        }
        private void sort_with_clustering()
        {
            // sort Clustered
            for (int h = 0; h < AllclusterdResults.Count; h++)
                if (AllclusterdResults.ElementAt(h).Value.Length > 2)
                    for (int i = 0; i < AllclusterdResults.ElementAt(h).Value.GetLength(0) - 1; i++)
                        for (int j = 0; j < AllclusterdResults.ElementAt(h).Value.GetLength(0) - i - 1; j++)
                            if (AllclusterdResults.ElementAt(h).Value[j, 1] < AllclusterdResults.ElementAt(h).Value[j + 1, 1])
                            {
                                double temp = AllclusterdResults.ElementAt(h).Value[j, 1];
                                AllclusterdResults.ElementAt(h).Value[j, 1] = AllclusterdResults.ElementAt(h).Value[j + 1, 1];
                                AllclusterdResults.ElementAt(h).Value[j + 1, 1] = temp;
                                temp = AllclusterdResults.ElementAt(h).Value[j, 0];
                                AllclusterdResults.ElementAt(h).Value[j, 0] = AllclusterdResults.ElementAt(h).Value[j + 1, 0];
                                AllclusterdResults.ElementAt(h).Value[j + 1, 0] = temp;

                            }


          
        }

        void get_comparison_withoutclustering()
        {
            AllCriteriaComp = new List<double[,]>();
            AllNonClusterdResults = new Dictionary<Case, double[,]>();
            List<string> crlist = new List<string>();
            crlist = Db.get_criteria(mybestchoice.CaseName);
            count_citeria = crlist.Count;


            // Criteria Comparsions------------------------
            for (int p = 0; p < AllNonClusterdCases.Count; p++)
            {
            create:
                double[,] cri_arr = new double[crlist.Count, crlist.Count];
                for (int i = 0; i < crlist.Count - 1; i++)
                {
                    Random r = new Random();
                    cri_arr[i, i] = 1;
                    cri_arr[i, i + 1] = r.Next(2, 9);
                    cri_arr[i + 1, i] = 1 / cri_arr[i, i + 1];
                }
                cri_arr[crlist.Count - 1, crlist.Count - 1] = 1;
                int m = 0;
                for (int i = 0; i < crlist.Count; i++)
                    for (int k = i + 2; k < crlist.Count; k++)
                    {
                        Random r = new Random();
                        if (cri_arr[i, k - 1] > cri_arr[k - 1, k])
                            m = (int)cri_arr[i, k - 1];
                        else
                            m = (int)cri_arr[k - 1, k];

                        cri_arr[i, k] = r.Next(m, 9);
                        cri_arr[k, i] = 1 / cri_arr[i, k];

                    }
                // check
                Case _problem = AllNonClusterdCases.Keys.ElementAt(p);
                List<Case> cases = AllNonClusterdCases.Values.ElementAt(p);
                MyAHPModel model = new MyAHPModel(count_citeria, cases.Count);
                if (model.CheckConsistency(cri_arr, crlist.Count))
                {
                    AllCriteriaComp.Add(cri_arr);
                    model.AddCriteria(cri_arr);
                    // Choices Comparsions------------------------
                    Dictionary<int, double[,]> AllChoiceComp = model.Create_All_Criteria_Choice_Comparison_Array(cases);
                    for (int u = 0; u < count_citeria; u++)
                        model.AddCriterionRatedChoices(u, AllChoiceComp[u]);
                    model.CalculatedCriteria();
                    int result = 0;
                    double[] choices = model.CalculatedChoices(out result);
                    int ii = 0;
                    double s = 0, si = 0;
                    double[,] w = new double[cases.Count, 2];
                    foreach (Case c in cases)
                    {

                        EuclideanSimilarity sim = new EuclideanSimilarity();
                        si = sim.Similarity(c, _problem);
                        //  s = System.Math.Round(choices.GetElement(ii, 0) * 100, 0);
                        s = choices[ii] * 100;


                        w[ii, 0] = Convert.ToDouble(c.GetFeature("id").GetFeatureValue());
                        w[ii, 1] = s;
                        // listBox1.Items.Add(" تثقيل الحل " + c.GetFeature("id").GetFeatureValue()+"\t من الحل رقم"+p.ToString() + "\t" + s + "\t" + "Similarity =   " + si);
                        ii++;
                    }
                    AllNonClusterdResults.Add(_problem, w);

                }
                else goto create;

            }
        }




        void get_comparison_withclustering()
        {
            AllCriteriaComp = new List<double[,]>();
            AllclusterdResults = new Dictionary<Case, double[,]>();
            List<string> crlist = new List<string>();
            crlist = Db.get_criteria(mybestchoice.CaseName);
            count_citeria = crlist.Count;
            // Criteria Comparsions------------------------
            for (int p = 0; p < AllclusterdCases.Count; p++)
            {
            create:
                double[,] cri_arr = new double[crlist.Count, crlist.Count];
                for (int i = 0; i < crlist.Count - 1; i++)
                {
                    Random r = new Random();
                    cri_arr[i, i] = 1;
                    cri_arr[i, i + 1] = r.Next(2, 9);
                    cri_arr[i + 1, i] = 1 / cri_arr[i, i + 1];
                }
                cri_arr[crlist.Count - 1, crlist.Count - 1] = 1;
                int m = 0;
                for (int i = 0; i < crlist.Count; i++)
                    for (int k = i + 2; k < crlist.Count; k++)
                    {
                        Random r = new Random();
                        if (cri_arr[i, k - 1] > cri_arr[k - 1, k])
                            m = (int)cri_arr[i, k - 1];
                        else
                            m = (int)cri_arr[k - 1, k];

                        cri_arr[i, k] = r.Next(m, 9);
                        cri_arr[k, i] = 1 / cri_arr[i, k];

                    }
                // check
                Case _problem = AllclusterdCases.Keys.ElementAt(p);
                List<Case> cases = AllclusterdCases.Values.ElementAt(p);
                MyAHPModel model = new MyAHPModel(count_citeria, cases.Count);
                if (model.CheckConsistency(cri_arr, crlist.Count))
                {
                    AllCriteriaComp.Add(cri_arr);
                    model.AddCriteria(cri_arr);
                    // Choices Comparsions------------------------
                    Dictionary<int, double[,]> AllChoiceComp = model.Create_All_Criteria_Choice_Comparison_Array(cases);
                    for (int u = 0; u < count_citeria; u++)
                        model.AddCriterionRatedChoices(u, AllChoiceComp[u]);
                    model.CalculatedCriteria();
                    int result = 0;
                    double[] choices = model.CalculatedChoices(out result);
                    int ii = 0;
                    double s = 0, si = 0;
                    double[,] w = new double[cases.Count, 2];
                    foreach (Case c in cases)
                    {
                        EuclideanSimilarity sim = new EuclideanSimilarity();
                        si = sim.Similarity(c, _problem);
                        s = choices[ii] * 100;
                        w[ii, 0] = Convert.ToDouble(c.GetFeature("id").GetFeatureValue());
                        w[ii, 1] = s;
                      
                        ii++;
                    }
                    AllclusterdResults.Add(_problem, w);
                }
                else goto create;

            }
        }
        public void CreateChoiceComp_Dispaly()
        {
            double[,] comp = new double[count_solution, count_solution];
          
            for (int I = 0; I < count_citeria; I++)
            {
                List<double> sol_value = new List<double>();
                for (int i = 0; i < count_solution; i++)
                    sol_value.Add(Convert.ToDouble(DG_Solution.Rows[I + 1].Cells[i+1].Value));
                for (int i = 0; i < count_solution; i++)
                    comp[i, i] = 1;
                double max = sol_value.Max();
                double min = sol_value.Min();
                double sub = max - min;
                sub = sub / 8;
                bool i_isBigger = false;
                double value = 0;
                for (int i = 0; i < sol_value.Count; i++)
                    for (int j = i + 1; j < sol_value.Count; j++)
                    {
                        if (sol_value[i] == sol_value[j])
                            comp[i, j] = 1;
                        else
                        {
                            if (sol_value[i] > sol_value[j])
                                i_isBigger = true;
                            else
                                i_isBigger = false;
                            value = Math.Abs(sol_value[i] - sol_value[j]);
                            if (value <= sub)
                                comp[i, j] = 2;
                            else
                                if (value > sub && value <= 2 * sub)
                                    comp[i, j] = 3;
                                else
                                    if (value > 2 * sub && value <= 3 * sub)
                                        comp[i, j] = 4;
                                    else
                                        if (value > 3 * sub && value <= 4 * sub)
                                            comp[i, j] = 5;
                                        else
                                            if (value > 4 * sub && value <= 5 * sub)
                                                comp[i, j] = 6;
                                            else
                                                if (value > 5 * sub && value <= 6 * sub)
                                                    comp[i, j] = 7;
                                                else
                                                    if (value > 6 * sub && value <= 7 * sub)
                                                        comp[i, j] = 8;
                                                    else
                                                        if (value > 7 * sub && value <= 8 * sub)
                                                            comp[i, j] = 9;
                            if (!i_isBigger)
                            { comp[j, i] = comp[i, j]; comp[i, j] = 1 / comp[j, i]; }
                            else
                            { comp[j, i] = 1 / comp[i, j]; }

                        } // end else
                    }
                
                // display
                int r_c = 0;
                DG_sol_cri.Columns.Clear();
                for (int ii = 0; ii < count_solution + 1; ii++)
                    DG_sol_cri.Columns.Add("", "");
                DG_sol_cri.Columns[0].DefaultCellStyle.BackColor = Color.RoyalBlue;
                DG_sol_cri.Rows.Add();
                for (int y = 0; y < sol_value.Count; y++)
                    DG_sol_cri.Rows[r_c].Cells[y + 1].Value = DG_Solution.Rows[0].Cells[y + 1].Value;
                DG_sol_cri.Rows[r_c].DefaultCellStyle.BackColor = Color.RoyalBlue;
                for (int y = 0; y < sol_value.Count; y++)
                    DG_sol_cri.Rows.Add();
                r_c++;
                for (int ii = 0; ii < sol_value.Count; ii++)
                {
                    for (int jj = 0; jj < sol_value.Count; jj++)
                    {
                        DG_sol_cri.Rows[r_c].Cells[jj + 1].Value = comp[ii, jj].ToString();
                    }
                    DG_sol_cri.Rows[r_c].Cells[0].Value = DG_Solution.Rows[0].Cells[ii + 1].Value;
                    r_c++;
                }
                
            }
        }

        public Dictionary<int, double[,]> CreateChoiceComp(List<Case> cases)
        {
            Dictionary<int, double[,]> AllChoiceComp = new Dictionary<int, double[,]>();
            

            for (int I = 0; I < count_citeria; I++)// exclude id  cluster
            {
                List<double> sol_value = new List<double>();
                for (int i = 0; i < cases.Count; i++)
                {
                    Case c = cases[i];
                    Feature f = (Feature)c.GetFeatures()[I+1];
                    sol_value.Add(Convert.ToDouble(f.GetFeatureValue()));
                }
                double[,] comp = new double[cases.Count, cases.Count];
                for (int i = 0; i < cases.Count; i++)
                    comp[i, i] = 1;
                double max = sol_value.Max();
                double min = sol_value.Min();
                double sub = max - min;
                sub = sub / 8;
                bool i_isBigger = false;
                double value = 0;
                for (int i = 0; i < sol_value.Count; i++)
                    for (int j = i + 1; j < sol_value.Count; j++)
                    {
                        if (sol_value[i] == sol_value[j])
                            comp[i, j] = 1;
                        else
                        {
                            if (sol_value[i] > sol_value[j])
                                i_isBigger = true;
                            else
                                i_isBigger = false;
                            value = Math.Abs(sol_value[i] - sol_value[j]);
                            if (value <= sub)
                                comp[i, j] = 2;
                            else
                                if (value > sub && value <= 2 * sub)
                                    comp[i, j] = 3;
                                else
                                    if (value > 2 * sub && value <= 3 * sub)
                                        comp[i, j] = 4;
                                    else
                                        if (value > 3 * sub && value <= 4 * sub)
                                            comp[i, j] = 5;
                                        else
                                            if (value > 4 * sub && value <= 5 * sub)
                                                comp[i, j] = 6;
                                            else
                                                if (value > 5 * sub && value <= 6 * sub)
                                                    comp[i, j] = 7;
                                                else
                                                    if (value > 6 * sub && value <= 7 * sub)
                                                        comp[i, j] = 8;
                                                    else
                                                        if (value > 7 * sub && value <= 8 * sub)
                                                            comp[i, j] = 9;
                            if (!i_isBigger)
                            { comp[j, i] = comp[i, j]; comp[i, j] = 1 / comp[j, i]; }
                            else
                            { comp[j, i] = 1 / comp[i, j]; }

                        } // end else
                    }
                AllChoiceComp.Add(I, comp);
            }
            return AllChoiceComp;
        }
        public void set_xml()
        {

            string path = @"E:\D Drive\_SAWSAN MASTER\MobileApplication\Fahp_cbr_app\Fahp_cbr_app\CBR\config.xml";

            XDeclaration _obj = new XDeclaration("1.0", "utf-8", "");
            XNamespace gameSaves = "gameSaves";
            XElement file = new XElement("opencbr-config");
            XElement map = new XElement("mapping");

            XElement interface_impl = new XElement("method");
            XAttribute inter = new XAttribute("interface", "ICaseRetrievalMethod");
            XAttribute impl = new XAttribute("impl", "DefaultCaseRetrievalMethod");
            interface_impl.Add(inter);
            interface_impl.Add(impl);
            map.Add(interface_impl);
            interface_impl = new XElement("method");
            inter = new XAttribute("interface", "ICaseReuseMethod");
            impl = new XAttribute("impl", "DefaultCaseReuseMethod");
            interface_impl.Add(inter);
            interface_impl.Add(impl);
            map.Add(interface_impl);
            interface_impl = new XElement("method");
            inter = new XAttribute("interface", "ICaseReviseMethod");
            impl = new XAttribute("impl", "DefaultCaseReviseMethod");
            interface_impl.Add(inter);
            interface_impl.Add(impl);
            map.Add(interface_impl);
            interface_impl = new XElement("method");
            inter = new XAttribute("interface", "ISimilarity");
            impl = new XAttribute("impl", "EuclideanSimilarity");
            interface_impl.Add(inter);
            interface_impl.Add(impl);
            map.Add(interface_impl);
            interface_impl = new XElement("method");
            inter = new XAttribute("interface", "ICaseReuseStrategy");
            impl = new XAttribute("impl", "MaxCaseReuseStrategy");
            interface_impl.Add(inter);
            interface_impl.Add(impl);
            map.Add(interface_impl);
            interface_impl = new XElement("method");
            inter = new XAttribute("interface", "ICaseBase");
            impl = new XAttribute("impl", "DefaultCaseBase");
            interface_impl.Add(inter);
            interface_impl.Add(impl);
            map.Add(interface_impl);
            interface_impl = new XElement("method");
            inter = new XAttribute("interface", "ICaseRestoreMethod");
            impl = new XAttribute("impl", "DefaultCaseRestoreMethod");
            interface_impl.Add(inter);
            interface_impl.Add(impl);
            map.Add(interface_impl);
            interface_impl = new XElement("method");
            inter = new XAttribute("interface", "ICaseBaseInput");
            impl = new XAttribute("impl", "DefaultCaseBaseInput");
            interface_impl.Add(inter);
            interface_impl.Add(impl);
            map.Add(interface_impl);
            file.Add(map);

            XElement parameters = new XElement("parameters");
            XElement parameters_node = new XElement("parameter");
            XAttribute name = new XAttribute("name", "CaseBaseInputType");
            XAttribute value = new XAttribute("value", "1");
            parameters_node.Add(name);
            parameters_node.Add(value);
            parameters.Add(parameters_node);
            parameters_node = new XElement("parameter");
            name = new XAttribute("name", "SimilarityThrehold");
            value = new XAttribute("value", txt_sim.Text);
            parameters_node.Add(name);
            parameters_node.Add(value);
            parameters.Add(parameters_node);
            parameters_node = new XElement("parameter");
            name = new XAttribute("name", "CaseBaseURL");
            value = new XAttribute("value", "$CaseID=" + comboBox1.Text + "* $DbType=Mssql *  $DataSource=Data Source=.;Initial Catalog=PhoneDB;Integrated Security=True * $DictionaryTable=tbl_dic * $DataTable=" + comboBox1.SelectedValue);
            parameters_node.Add(name);
            parameters_node.Add(value);
            parameters.Add(parameters_node);
            file.Add(parameters);
            file.Save(path);
        }

        public void set_xml_sim(string sim)
        {

            string path = @"E:\D Drive\_SAWSAN MASTER\MobileApplication\Fahp_cbr_app\Fahp_cbr_app\CBR\config.xml";

            XDeclaration _obj = new XDeclaration("1.0", "utf-8", "");
            XNamespace gameSaves = "gameSaves";
            XElement file = new XElement("opencbr-config");
            XElement map = new XElement("mapping");

            XElement interface_impl = new XElement("method");
            XAttribute inter = new XAttribute("interface", "ICaseRetrievalMethod");
            XAttribute impl = new XAttribute("impl", "DefaultCaseRetrievalMethod");
            interface_impl.Add(inter);
            interface_impl.Add(impl);
            map.Add(interface_impl);
            interface_impl = new XElement("method");
            inter = new XAttribute("interface", "ICaseReuseMethod");
            impl = new XAttribute("impl", "DefaultCaseReuseMethod");
            interface_impl.Add(inter);
            interface_impl.Add(impl);
            map.Add(interface_impl);
            interface_impl = new XElement("method");
            inter = new XAttribute("interface", "ICaseReviseMethod");
            impl = new XAttribute("impl", "DefaultCaseReviseMethod");
            interface_impl.Add(inter);
            interface_impl.Add(impl);
            map.Add(interface_impl);
            interface_impl = new XElement("method");
            inter = new XAttribute("interface", "ISimilarity");
            impl = new XAttribute("impl", "EuclideanSimilarity");
            interface_impl.Add(inter);
            interface_impl.Add(impl);
            map.Add(interface_impl);
            interface_impl = new XElement("method");
            inter = new XAttribute("interface", "ICaseReuseStrategy");
            impl = new XAttribute("impl", "MaxCaseReuseStrategy");
            interface_impl.Add(inter);
            interface_impl.Add(impl);
            map.Add(interface_impl);
            interface_impl = new XElement("method");
            inter = new XAttribute("interface", "ICaseBase");
            impl = new XAttribute("impl", "DefaultCaseBase");
            interface_impl.Add(inter);
            interface_impl.Add(impl);
            map.Add(interface_impl);
            interface_impl = new XElement("method");
            inter = new XAttribute("interface", "ICaseRestoreMethod");
            impl = new XAttribute("impl", "DefaultCaseRestoreMethod");
            interface_impl.Add(inter);
            interface_impl.Add(impl);
            map.Add(interface_impl);
            interface_impl = new XElement("method");
            inter = new XAttribute("interface", "ICaseBaseInput");
            impl = new XAttribute("impl", "DefaultCaseBaseInput");
            interface_impl.Add(inter);
            interface_impl.Add(impl);
            map.Add(interface_impl);
            file.Add(map);

            XElement parameters = new XElement("parameters");
            XElement parameters_node = new XElement("parameter");
            XAttribute name = new XAttribute("name", "CaseBaseInputType");
            XAttribute value = new XAttribute("value", "1");
            parameters_node.Add(name);
            parameters_node.Add(value);
            parameters.Add(parameters_node);
            parameters_node = new XElement("parameter");
            name = new XAttribute("name", "SimilarityThrehold");
            value = new XAttribute("value", sim);
            parameters_node.Add(name);
            parameters_node.Add(value);
            parameters.Add(parameters_node);
            parameters_node = new XElement("parameter");
            name = new XAttribute("name", "CaseBaseURL");
            value = new XAttribute("value", "$CaseID=" + comboBox1.Text + "* $DbType=Mssql *  $DataSource=Data Source=.;Initial Catalog=PhoneDB;Integrated Security=True * $DictionaryTable=tbl_dic * $DataTable=" + comboBox1.SelectedValue);
            parameters_node.Add(name);
            parameters_node.Add(value);
            parameters.Add(parameters_node);
            file.Add(parameters);
            file.Save(path);
        }
        private DataTable flip(DataTable dt_data)
        {
            DataTable table = new DataTable();

            for (int i = 0; i <= dt_data.Rows.Count; i++)
            { table.Columns.Add(Convert.ToString(i)); }

            DataRow r;
            for (int k = 0; k < dt_data.Columns.Count; k++)
            {
                r = table.NewRow();
                r[0] = dt_data.Columns[k].ToString();
                for (int j = 1; j <= dt_data.Rows.Count; j++)
                { r[j] = dt_data.Rows[j - 1][k]; }
                table.Rows.Add(r);
            }

            return table;
        }



        private void frm_processing_Load(object sender, EventArgs e)
        {

        }



        private void btn_process_Click(object sender, EventArgs e)
        {




        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_process_with_Click(object sender, EventArgs e)
        {


        }



        private void button2_Click(object sender, EventArgs e)
        {


        }

        private void button1_Click(object sender, EventArgs e)
        {
            DG_Criteria.Columns.Clear();
            DG_sol.DataSource = null;
            DG_sol_cri.Columns.Clear();
            DG_comparing.Columns.Clear();

            DG_Criteria.Columns.Add("FeatureName", "المعيار");
            DG_Criteria.Rows.Add(dataGV_CaseEntries.Rows.Count - 2);
            DataGridViewCheckBoxColumn dgvCmb = new DataGridViewCheckBoxColumn();
            dgvCmb.ValueType = typeof(bool);
            dgvCmb.Name = "CheckBox";
            dgvCmb.HeaderText = "CheckBox";
            DG_Criteria.Columns.Add(dgvCmb);
            for (int i = 0; i < dataGV_CaseEntries.Rows.Count - 1; i++)
                DG_Criteria.Rows[i].Cells[0].Value = dataGV_CaseEntries.Rows[i].Cells["FeatureName"].Value;
            DG_Criteria.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            DG_sol.DataSource = finalresult;
            DG_sol.Columns[0].DefaultCellStyle.BackColor = Color.RoyalBlue;
            DG_sol.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            tabControl1.SelectTab(tabPage2);
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            dataGV_CaseEntries.Columns.Clear();
            label4.Text = " معايير الحالة";
            lbl_solution.Text = "";

            if ((conn.State == ConnectionState.Closed) && (comboBox1.SelectedValue.ToString() != "System.Data.DataRowView"))
            {
                ArrayList myarr = Db.get_cases_as_arraylist(comboBox1.SelectedValue.ToString(), comboBox1.Text);
                DataTable mydt = Db.get_table_contetnt_condition("tbl_cases", "case_name", comboBox1.Text);
                mybestchoice = new BestChoice(myarr, mydt.Rows[0]["standrize_type"].ToString(), mydt.Rows[0]["kmeans_method"].ToString(), mydt.Rows[0]["clusters_num"].ToString());
                conn.Open();

                string select_name = "select * from tbl_dic where caseid ='" + comboBox1.Text + "'";
                //   string select_name = " SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.Columns where TABLE_NAME = '" + comboBox1.SelectedValue + "'";
                SqlCommand cmd = new SqlCommand(select_name, conn);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                int p = sd.Fill(dt);

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
                else
                    label4.Text = "لايوجد معايير  لهذه الحالة في قاعدة البيانات";
                conn.Close();
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void btn_show_Click_1(object sender, EventArgs e)
        {

        }

        private void btn_show_Click(object sender, EventArgs e)
        {
            frm_showdatabase fs = new frm_showdatabase();
            fs.dataGridView1.DataSource = Db.get_table_contetnt(comboBox1.SelectedValue.ToString());
            fs.Show();
        }

        private void btn_process_without_Click(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            // the code that you want to measure comes here

            DG_Solution.Columns.Clear();
            set_xml();
            // with Clustering

            _problem = new Case(0, comboBox1.Text, "");
            _problem.AddFeature("id", FeatureType.TYPE_FEATURE_INT, 0, 1.0, false, false, "num");

            for (int i = 0; i < dataGV_CaseEntries.Rows.Count - 1; i++)
                if (dataGV_CaseEntries.Rows[i].Cells["UserEntry"].Value != null)
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
                else
                    label4.Text = "أدخل كل القيم لو سمحت";


            DataTable dt = Db.get_table_contetnt_condition("tbl_cases", "case_name", comboBox1.Text);
            string standrize_table = dt.Rows[0]["standrize_table"].ToString();
            string standrize_type = dt.Rows[0]["standrize_type"].ToString();
            Case Max_Mean = Db.get_Case(_problem, standrize_table, -2);
            Case Min_Sd = Db.get_Case(_problem, standrize_table, -3);
            //  i have to put general seetings for normalization
            _problem = BestChoice.StandardizeCase(_problem, Max_Mean, Min_Sd, standrize_type);


            // get particular cluster
            ArrayList myarr = Db.get_cases_as_arraylist_condition_NoCluster(standrize_table, mybestchoice.CaseName);
            DefaultEngine _engine = new DefaultEngine();
            string _env = @"E:\D Drive\_SAWSAN MASTER\MobileApplication\Fahp_cbr_app\Fahp_cbr_app\CBR\config.xml";
            _engine.SetEnvironmentVariable(_env);
            _engine.SetProblem(_problem);
            // Run the reasoning engine
            _engine.Startup();
            ArrayList cases = _engine.Run_caseRetrieval_partial(myarr);
            //DG_Solution.Rows.Clear();
            count_solution = cases.Count;
            ArrayList arrcases = new ArrayList();
            if (count_solution != 0)
            {

                Stat ss = null;
                Case cc = null;
                Feature ff = null;
                string ids = "(";
                for (int i = 0; i < cases.Count - 1; i++)
                {

                    ss = (Stat)(cases[i]);
                    cc = ss.GetCBRCase();
                    arrcases.Add(cc);
                    ff = (Feature)cc.GetFeatures()[0];
                    ids += ff.GetFeatureValue() + ",";
                }
                ss = (Stat)(cases[cases.Count - 1]);
                cc = ss.GetCBRCase();
                ff = (Feature)cc.GetFeatures()[0];
                ids += ff.GetFeatureValue() + ")";
                arrcases.Add(cc);
                DataTable dt_data = Db.get_table_contetnt_condition_in(comboBox1.SelectedValue.ToString(), "id", ids);
                finalresult = flip(dt_data);
                DG_Solution.DataSource = finalresult;
                DG_Solution.Columns[0].DefaultCellStyle.BackColor = Color.RoyalBlue;
                lbl_solution.Text = "  لدينا " + count_solution + " حلاً مشابه للمواصفات التي ادخلتها ";
                sol_cases = new Dictionary<Case, double>();
                for (int i = 0; i < cases.Count; i++)
                {
                    Case ca = new Case(_problem.GetCaseID(), _problem.GetCaseName(), _problem.GetCaseDescription());
                    int fc = cc.GetFeatures().Count;
                    for (int j = 0; j < fc; j++)
                    {
                        Feature fe = (Feature)cc.GetFeatures()[j];
                        ca.AddFeature(fe.GetFeatureName(),
                                      fe.GetFeatureType(),
                                      DG_Solution.Rows[j].Cells[i + 1].Value.ToString(),
                                      fe.GetWeight(),
                                      fe.GetIsKey(),
                                      fe.GetIsIndex(),
                                      fe.GetFeatureUnit());
                    }

                    sol_cases.Add(ca, 0);
                   
                }
            }
            else
                lbl_solution.Text = "No solution";



            real_choices = count_solution;
            Alternatives = cases;

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            label2.Text = "without clustring time  " + elapsedMs.ToString();

            listBox2.Items.Clear();
            List<Case> mycases = new List<Case>();
            mycases = mybestchoice.CasesData(arrcases);
            for (int i = 0; i < mycases.Count; i++)
            {
                EuclideanSimilarity sim = new EuclideanSimilarity();
                listBox2.Items.Add(mycases[i].GetFeature("id").GetFeatureValue().ToString() + "\t" + sim.Similarity(_problem, mycases[i]));
            }


        }

        private void btn_process_with_Click_1(object sender, EventArgs e)
        {

            var watch = System.Diagnostics.Stopwatch.StartNew();


            set_xml();
            // with Clustering

            _problem = new Case(0, comboBox1.Text, "");
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
                else
                    label4.Text = "أدخل كل القيم";




            DG_Solution.Columns.Clear();
            current_cases_partial.Clear();

            // get centroids
            DataTable dt = Db.get_table_contetnt_condition("tbl_cases", "case_name", comboBox1.Text);
            string standrize_table = dt.Rows[0]["standrize_table"].ToString();
            string standrize_type = dt.Rows[0]["standrize_type"].ToString();
            dt = Db.get_table_contetnt_condition(standrize_table, "cluster", "-1");

            List<Case> centroids = new List<Case>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Case c = new Case(0, _problem.GetCaseName(), "");
                for (int j = 0; j < _problem.GetFeatures().Count; j++)
                {
                    Feature f = (Feature)_problem.GetFeatures()[j];
                    c.AddFeature(f.GetFeatureName(), f.GetFeatureType(), dt.Rows[i][j].ToString(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                }
                centroids.Add(c);
            }


            Case Max_Mean = Db.get_Case(_problem, standrize_table, -2);
            Case Min_Sd = Db.get_Case(_problem, standrize_table, -3);
            //  i have to put general seetings for normalization
            _problem = BestChoice.StandardizeCase(_problem, Max_Mean, Min_Sd, standrize_type);
            // know which cluster
            Kmeans kmeans = new Kmeans();
            int t = kmeans.findCluster_customize(_problem, centroids);

            // get particular cluster
            dt = Db.get_table_contetnt_condition(standrize_table, "cluster", t.ToString());

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Case c = new Case(0, _problem.GetCaseName(), "");
                for (int j = 0; j < _problem.GetFeatures().Count; j++)
                {
                    Feature f = (Feature)_problem.GetFeatures()[j];
                    c.AddFeature(f.GetFeatureName(), f.GetFeatureType(), dt.Rows[i][j].ToString(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                }
                current_cases_partial.Add(c);
            }



            DefaultEngine _engine = new DefaultEngine();
            string _env = @"E:\D Drive\_SAWSAN MASTER\MobileApplication\Fahp_cbr_app\Fahp_cbr_app\CBR\config.xml";
            _engine.SetEnvironmentVariable(_env);
            _engine.SetProblem(_problem);
            // Run the reasoning engine
            _engine.Startup();
            ArrayList cases = _engine.Run_caseRetrieval_partial(current_cases_partial);
            //DG_Solution.Rows.Clear();
            count_solution = cases.Count;

            if (count_solution != 0)
            {
                Stat ss = null;
                Case cc = null;
                Feature ff = null;
                string ids = "(";
                ArrayList myarr = new ArrayList();
                for (int i = 0; i < cases.Count - 1; i++)
                {
                    ss = (Stat)(cases[i]);
                    cc = ss.GetCBRCase();
                    myarr.Add(cc);
                    ff = (Feature)cc.GetFeatures()[0];
                    ids += ff.GetFeatureValue() + ",";
                }
                ss = (Stat)(cases[cases.Count - 1]);
                cc = ss.GetCBRCase();
                myarr.Add(cc);
                ff = (Feature)cc.GetFeatures()[0];
                ids += ff.GetFeatureValue() + ")";
                DataTable dt_data = Db.get_table_contetnt_condition_in(comboBox1.SelectedValue.ToString(), "id", ids);
                finalresult = flip(dt_data);
                DG_Solution.DataSource = finalresult;
                DG_Solution.Columns[0].DefaultCellStyle.BackColor = Color.RoyalBlue;

                lbl_solution.Text = "  لدينا " + count_solution + " حلاً مشابه للمواصفاتك التي ادخلتها ";


                //---------------------------------------
                List<Case> mycases = new List<Case>();
                mycases = mybestchoice.CasesData(myarr);
                listBox2.Items.Clear();
                listBox2.Items.Add("centroids");
                for (int i = 0; i < centroids.Count; i++)
                {
                    EuclideanSimilarity sim = new EuclideanSimilarity();
                    listBox2.Items.Add(centroids[i].GetFeature("id").GetFeatureValue().ToString() + "\t" + sim.Similarity(_problem, centroids[i]));
                }
                listBox2.Items.Add("cases");
                for (int i = 0; i < mycases.Count; i++)
                {
                    EuclideanSimilarity sim = new EuclideanSimilarity();
                    listBox2.Items.Add(mycases[i].GetFeature("id").GetFeatureValue().ToString() + "\t" + sim.Similarity(_problem, mycases[i]));
                }

                listBox2.Items.Add(t.ToString() + " is cluster");
                //--------------------------------------------
                sol_cases = new Dictionary<Case, double>();
                for (int i = 0; i < cases.Count; i++)
                {
                    Case ca = new Case(_problem.GetCaseID(), _problem.GetCaseName(), _problem.GetCaseDescription());
                    int fc = cc.GetFeatures().Count;
                    for (int j = 0; j < fc; j++)
                    {
                        Feature fe = (Feature)cc.GetFeatures()[j];
                        ca.AddFeature(fe.GetFeatureName(),
                                      fe.GetFeatureType(),
                                      DG_Solution.Rows[j].Cells[i + 1].Value.ToString(),
                                      fe.GetWeight(),
                                      fe.GetIsKey(),
                                      fe.GetIsIndex(),
                                      fe.GetFeatureUnit());
                    }

                    sol_cases.Add(ca, 0);
                }
            }
            else
                lbl_solution.Text = "No solution";

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            label5.Text = "with clustring time  " + elapsedMs.ToString();



        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            // decision matrix for criteria
            double[,] criteria = new double[count_citeria, count_citeria];
            //  for (int i = 0; i < criteria.Length; i++)
            //     criteria[i] = new double[count_citeria];

            for (int i = 0; i < count_citeria; i++)
                for (int j = 0; j < count_citeria; j++)
                    criteria[i, j] = 1;

            for (int i = 0; i < count_citeria - 1; i++)
                for (int j = i + 1; j < count_citeria; j++)
                {
                    DataGridViewComboBoxCell comboCell = (DataGridViewComboBoxCell)DG_comparing.Rows[i].Cells[2];
                    if (comboCell.Items.IndexOf(comboCell.Value) == 0)//(DG_comparing.Rows[i].Cells[2].Value.ToString()=="مهم جداً")
                    {
                        criteria[i, j] = 9;
                        criteria[j, i] = ((double)1 / (double)9);
                    }
                    else
                        if (comboCell.Items.IndexOf(comboCell.Value) == 1)//(DG_comparing.Rows[i].Cells[2].Value.ToString()=="مهم")
                        {
                            criteria[i, j] = 7;
                            criteria[j, i] = ((double)1 / (double)7);
                        }

                        else
                            if (comboCell.Items.IndexOf(comboCell.Value) == 2)//(DG_comparing.Rows[i].Cells[2].Value.ToString()=="مهم إلى حد ما")
                            {
                                criteria[i, j] = 5;
                                criteria[j, i] = ((double)1 / (double)5);
                            }
                            else
                                if (comboCell.Items.IndexOf(comboCell.Value) == 3)//(DG_comparing.Rows[i].Cells[2].Value.ToString() == "مهم قليلا")
                                {
                                    criteria[i, j] = 3;
                                    criteria[j, i] = ((double)1 / (double)3);
                                }
                                else if (comboCell.Items.IndexOf(comboCell.Value) == 4)//(DG_comparing.Rows[i].Cells[2].Value.ToString()=="مهم جداً")
                                {
                                    criteria[i, j] = ((double)1 / (double)9);
                                    criteria[j, i] = 9;
                                }
                                else
                                    if (comboCell.Items.IndexOf(comboCell.Value) == 5)//(DG_comparing.Rows[i].Cells[2].Value.ToString()=="مهم")
                                    {
                                        criteria[i, j] = ((double)1 / (double)7);
                                        criteria[j, i] = 7;
                                    }

                                    else
                                        if (comboCell.Items.IndexOf(comboCell.Value) == 6)//(DG_comparing.Rows[i].Cells[2].Value.ToString()=="مهم إلى حد ما")
                                        {
                                            criteria[i, j] = ((double)1 / (double)5);
                                            criteria[j, i] = 5;
                                        }
                                        else
                                            if (comboCell.Items.IndexOf(comboCell.Value) == 7)//(DG_comparing.Rows[i].Cells[2].Value.ToString() == "مهم قليلا")
                                            {
                                                criteria[i, j] = ((double)1 / (double)3);
                                                criteria[j, i] = 3;
                                            }
                }


            MyAHPModel model = new MyAHPModel(count_citeria, count_solution);
            model.AddCriteria(criteria);
            // decision matrix for choices

            double[,] choices_arr = new double[count_solution, count_solution];
            for (int g = 0; g < count_solution; g++)
                for (int h = 0; h < count_solution; h++)
                    choices_arr[g, h] = 1;

            if (input_file == false)
            {
                int count = 1;
                for (int j = 0; j < count_citeria; j++)
                {
                    for (int i = 0; i < count_solution - 1; i++)
                        for (int t = i + 1; t < count_solution; t++)
                        {
                            DataGridViewComboBoxCell comboCell = (DataGridViewComboBoxCell)DG_sol_cri.Rows[count].Cells[2];
                            // fill it
                            if (comboCell.Items.IndexOf(comboCell.Value) == 0)//(DG_sol_cri.Rows[count].Cells[2].Value.ToString() == "مهم جداً")
                            {
                                choices_arr[i, t] = 9;
                                choices_arr[t, i] = ((double)1 / (double)9);
                            }
                            else
                                if (comboCell.Items.IndexOf(comboCell.Value) == 1)//(DG_sol_cri.Rows[count].Cells[2].Value.ToString() == "مهم")
                                {
                                    choices_arr[i, t] = 7;
                                    choices_arr[t, i] = ((double)1 / (double)7);
                                }

                                else
                                    if (comboCell.Items.IndexOf(comboCell.Value) == 2)//(DG_sol_cri.Rows[count].Cells[2].Value.ToString() == "مهم إلى حد ما")
                                    {
                                        choices_arr[i, t] = 5;
                                        choices_arr[t, i] = ((double)1 / (double)5);
                                    }
                                    else
                                        if (comboCell.Items.IndexOf(comboCell.Value) == 3)//(DG_sol_cri.Rows[count].Cells[2].Value.ToString() == "مهم قليلا")
                                        {
                                            choices_arr[i, t] = 3;
                                            choices_arr[t, i] = ((double)1 / (double)3);
                                        }
                                        else if (comboCell.Items.IndexOf(comboCell.Value) == 4)//(DG_sol_cri.Rows[count].Cells[2].Value.ToString() == "مهم جداً")
                                        {
                                            choices_arr[i, t] = ((double)1 / (double)9);
                                            choices_arr[t, i] = 9;
                                        }
                                        else
                                            if (comboCell.Items.IndexOf(comboCell.Value) == 5)//(DG_sol_cri.Rows[count].Cells[2].Value.ToString() == "مهم")
                                            {
                                                choices_arr[i, t] = ((double)1 / (double)7);
                                                choices_arr[t, i] = 7;
                                            }

                                            else
                                                if (comboCell.Items.IndexOf(comboCell.Value) == 6)//(DG_sol_cri.Rows[count].Cells[2].Value.ToString() == "مهم إلى حد ما")
                                                {
                                                    choices_arr[i, t] = ((double)1 / (double)5);
                                                    choices_arr[t, i] = 5;
                                                }
                                                else
                                                    if (comboCell.Items.IndexOf(comboCell.Value) == 7)//(DG_sol_cri.Rows[count].Cells[2].Value.ToString() == "مهم قليلا")
                                                    {
                                                        choices_arr[i, t] = ((double)1 / (double)3);
                                                        choices_arr[t, i] = 3;
                                                    }
                            count++;
                        }
                    model.AddCriterionRatedChoices(j, choices_arr);
                    for (int g = 0; g < count_solution; g++)
                        for (int h = 0; h < count_solution; h++)
                            choices_arr[g, h] = 1;

                    count++;
                }

            }
            else
            {
                int rcount = 1;
                for (int j = 0; j < count_citeria; j++)
                {
                    for (int i = 0; i < count_solution - 1; i++)
                    {
                        for (int t = i + 1; t < count_solution; t++)
                        {
                            double value = Convert.ToDouble(DG_sol_cri.Rows[rcount].Cells[t + 1].Value);
                            if (value > 1) value = Math.Round(value);
                            choices_arr[i, t] = value;
                            double rvalue = 1 / value;
                            if (rvalue > 1) rvalue = Math.Round(rvalue);
                            choices_arr[t, i] = rvalue;
                        }
                        rcount++;
                    }
                    rcount = rcount + 2;
                    model.AddCriterionRatedChoices(j, choices_arr);
                    for (int g = 0; g < count_solution; g++)
                        for (int h = 0; h < count_solution; h++)
                            choices_arr[g, h] = 1;

                }
            }

            // for (all solutions)
            model.CalculatedCriteria();
            int result=0;
            double[] choices = model.CalculatedChoices(out result);
            int ii = 0;
            double s = 0, si = 0;
            foreach (KeyValuePair<Case, double> pair in sol_cases)
            {
                EuclideanSimilarity sim = new EuclideanSimilarity();
                si = System.Math.Round(sim.Similarity(pair.Key, _problem), 8);
                //  s = System.Math.Round(choices.GetElement(ii, 0) * 100, 0);
                s = System.Math.Round(choices[ii] * 100, 0);
                listBox1.Items.Add(" تثقيل الحل " + DG_sol.Rows[0].Cells[ii + 1].Value.ToString() + "\t" + s + "\t" + "Similarity =   " + si);
                ii++;

            }
            tabControl1.SelectTab(tabPage3);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            input_file = false;
            DG_comparing.Columns.Clear();
            DG_sol_cri.Rows.Clear();
            ArrayList crlist = new ArrayList();

            int count = 0;
            for (int i = 0; i < DG_Criteria.Rows.Count; i++)
                if (Convert.ToBoolean(DG_Criteria.Rows[i].Cells["Checkbox"].Value) == true)
                    crlist.Add(DG_Criteria.Rows[i].Cells["FeatureName"].Value.ToString());

            count_citeria = crlist.Count;

            DG_comparing.Columns.Add("first_cr", "المعيار الأول");
            DG_comparing.Columns.Add("second_cr", "المعيار الثاني");
            DG_comparing.Rows.Add((crlist.Count * (crlist.Count - 1)) / 2);

            DataGridViewComboBoxColumn cmb = new DataGridViewComboBoxColumn();
            cmb.HeaderText = "درجة الأهمية";
            cmb.Name = "cmb";
            cmb.MaxDropDownItems = 5;
            cmb.Items.Add("المعيار الأول مهم جداً بالنسبة للمعيار الثاني");
            cmb.Items.Add("المعيار الأول مهم بالنسبة للمعيار الثاني");
            cmb.Items.Add("المعيار الأول مهم إلى حد ما بالنسبة للمعيار الثاني");
            cmb.Items.Add("المعيار الأول مهم قليلاً بالنسبة للمعيار الثاني");
            cmb.Items.Add("المعيار الثاني مهم جداً بالنسبة للمعيار الأول");
            cmb.Items.Add("المعيار الثاني مهم بالنسبة للمعيار الأول");
            cmb.Items.Add("المعيار الثاني مهم إلى حد ما بالنسبة للمعيار الأول");
            cmb.Items.Add("المعيار الثاني مهم قليلاً بالنسبة للمعيار الأول");
            cmb.Items.Add("أهمية متساوية");
            DG_comparing.Columns.Add(cmb);
            for (int i = 0; i < crlist.Count - 1; i++)
                for (int j = i + 1; j < crlist.Count; j++)
                {
                    DG_comparing.Rows[count].Cells[0].Value = crlist[i].ToString();
                    DG_comparing.Rows[count].Cells[1].Value = crlist[j].ToString();
                    count++;
                }
            DG_comparing.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;




            DG_sol_cri.Columns.Add("first_cr", "الحل الأول");
            DG_sol_cri.Columns.Add("second_cr", "الحل الثاني");

            DataGridViewComboBoxColumn cmbb = new DataGridViewComboBoxColumn();
            cmbb.HeaderText = "درجة الأهمية";
            cmbb.Name = "cmbb";
            cmbb.MaxDropDownItems = 5;
            cmbb.Items.Add(" البديل الأول أفضل بشكل مطلق من البديل الثاني");
            cmbb.Items.Add("البديل الأول أفضل  من البديل الثاني");
            cmbb.Items.Add("البديل الأول أفضل إلى حد ما من البديل الثاني");
            cmbb.Items.Add("البديل الأول أفضل قليلاً من البديل الثاني");
            cmbb.Items.Add(" البديل الثاني أفضل بشكل مطلق من البديل الأول");
            cmbb.Items.Add("البديل الثاني أفضل  من البديل الأول");
            cmbb.Items.Add("البديل الثاني أفضل إلى حد ما من البديل الأول");
            cmbb.Items.Add("البديل الثاني أفضل قليلاً من البديل الأول");
            cmbb.Items.Add("أفضلية متساوية");
            DG_sol_cri.Columns.Add(cmbb);
            int ct = 0;
            for (int i = 0; i < crlist.Count; i++)
            {
                DG_sol_cri.Rows.Add();
                DG_sol_cri.Rows[ct].Cells[0].Value = crlist[i].ToString();
                DG_sol_cri.Rows[ct].Cells[1].Value = crlist[i].ToString();
                DG_sol_cri.Rows[ct].DefaultCellStyle.BackColor = Color.RoyalBlue;
                ct++;
                for (int j = 1; j < count_solution; j++)
                    for (int k = j + 1; k < count_solution + 1; k++)
                    {
                        DG_sol_cri.Rows.Add();

                        DG_sol_cri.Rows[ct].Cells[0].Value = " الحل ذو الرقم " + DG_sol.Rows[0].Cells[j].Value;
                        DG_sol_cri.Rows[ct].Cells[1].Value = " الحل ذو الرقم " + DG_sol.Rows[0].Cells[k].Value;
                        ct++;
                    }
            }

            DG_sol_cri.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }

        private void button4_Click(object sender, EventArgs e)
        {
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
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
           

             StringBuilder sb = new StringBuilder();
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                    sb.Append(listBox1.Items[i].ToString());
                    sb.AppendLine();
                }
                sb.Remove(sb.Length - 1, 1); // Just to avoid copying last empty row
                Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
           
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void DG_sol_cri_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.FillWeight = 10;
        }

        private void DG_Solution_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.FillWeight = 10;
        }

        private void button6_Click_1(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void Add_file_btn_Click(object sender, EventArgs e)
        {
            ArrayList crlist = new ArrayList();
            dataGridView4.Columns.Add("رقم الحل", "رقم الحل");
            dataGridView4.Columns.Add("رقم المجموعة", "رقم المجموعة");
            dataGridView4.Columns.Add("التثقيل", "التثقيل");
            dataGridView4.Columns.Add("التشابه", "التشابه");
            for (int i = 0; i < DG_Criteria.Rows.Count; i++)
                if (Convert.ToBoolean(DG_Criteria.Rows[i].Cells["Checkbox"].Value) == true)
                    crlist.Add(DG_Criteria.Rows[i].Cells["FeatureName"].Value.ToString());

             count_citeria = crlist.Count;
            int count=0;
            
            // Criteria Comparsions------------------------
            for (int p = 0; p < AllNonClusterdCases.Count; p++)
            {
         create:
                double[,] cri_arr = new double[crlist.Count, crlist.Count];
                for (int i = 0; i < crlist.Count-1; i++)
                {
                    Random r = new Random();
                    cri_arr[i, i] = 1;
                    cri_arr[i, i + 1] = r.Next(2, 9);
                    cri_arr[i + 1, i] = 1 / cri_arr[i, i + 1];
                }
                cri_arr[crlist.Count - 1, crlist.Count - 1] = 1;
                int m=0;
                for (int i = 0; i < crlist.Count; i++)
                    for (int k = i + 2; k < crlist.Count; k++)
                    {
                     Random r = new Random();
                    if (cri_arr[i,k-1] > cri_arr[k-1, k])
                            m=(int )cri_arr[i,k-1];
                    else
                            m=(int)cri_arr[k-1, k];

                    cri_arr[i, k] = r.Next(m , 9);
                    cri_arr[k, i] = 1 / cri_arr[i,k];

                    }
                // check
                Case _problem= AllNonClusterdCases.Keys.ElementAt(p);
                List<Case> cases = AllNonClusterdCases.Values.ElementAt(p);
                MyAHPModel model = new MyAHPModel(count_citeria, cases.Count);
                if (model.CheckConsistency(cri_arr, crlist.Count))
                { 
                    AllCriteriaComp.Add(cri_arr);
                    model.AddCriteria(cri_arr);
                    // Choices Comparsions------------------------
                    Dictionary<int, double[,]> AllChoiceComp = CreateChoiceComp(cases);
                    for (int u = 0; u < count_citeria;u++ )
                        model.AddCriterionRatedChoices(u, AllChoiceComp[u]);
                    model.CalculatedCriteria();
                    int result = 0;
                    double[] choices = model.CalculatedChoices(out result);
                    int ii = 0;
                    double s = 0, si = 0;
                    double[,] w = new double[cases.Count, 2];
                    foreach (Case c in cases)
                    {
                       
                        EuclideanSimilarity sim = new EuclideanSimilarity();
                        si = sim.Similarity(c, _problem);
                        //  s = System.Math.Round(choices.GetElement(ii, 0) * 100, 0);
                        s = choices[ii] * 100;
                        dataGridView4.Rows.Add();
                        dataGridView4.Rows[count].Cells[0].Value = c.GetFeature("id").GetFeatureValue();
                        dataGridView4.Rows[count].Cells[1].Value = p.ToString();
                        dataGridView4.Rows[count].Cells[2].Value = s.ToString();
                        dataGridView4.Rows[count].Cells[3].Value = si.ToString();
                             count++;
                        w[ii, 0] = Convert.ToDouble(c.GetFeature("id").GetFeatureValue());
                        w[ii, 1] = s;
                       // listBox1.Items.Add(" تثقيل الحل " + c.GetFeature("id").GetFeatureValue()+"\t من الحل رقم"+p.ToString() + "\t" + s + "\t" + "Similarity =   " + si);
                        ii++;
                    }
                    AllNonClusterdResults.Add(_problem,w);
                     
                }
                else goto create;
            
        }
            
        }

        private void button6_Click_2(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            // decision matrix for criteria
            double[,] criteria = new double[count_citeria, count_citeria];
            for (int i = 0; i < count_citeria; i++)
                for (int j = 0; j < count_citeria; j++)
                    criteria[i, j] = 1;

            for (int i = 0; i < count_citeria - 1; i++)
                for (int j = i + 1; j < count_citeria; j++)
                { 
                    double value = Convert.ToDouble(DG_comparing.Rows[i+1].Cells[j+1].Value);
                    if (value > 1) value = Math.Round(value);
                    criteria[i, j] = value;
                    double rvalue = 1 / value;
                    if (rvalue > 1) rvalue = Math.Round(rvalue);
                    criteria[j, i] = rvalue;
                }
            //Check CI
                MyAHPModel model = new MyAHPModel(count_citeria, count_solution);
                model.AddCriteria(criteria);

                // decision matrix for choices
                double[,] choices_arr = new double[count_solution, count_solution];
                for (int g = 0; g < count_solution; g++)
                    for (int h = 0; h < count_solution; h++)
                        choices_arr[g, h] = 1;

                int rcount = 1;
                for (int j = 0; j < count_citeria; j++)
                {
                    for (int i = 0; i < count_solution - 1; i++)
                    {
                        for (int t = i + 1; t < count_solution; t++)
                        {
                            double value = Convert.ToDouble(DG_sol_cri.Rows[rcount].Cells[t + 1].Value);
                            if (value > 1) value = Math.Round(value);
                            choices_arr[i, t] = value;
                            double rvalue = 1 / value;
                            if (rvalue > 1) rvalue = Math.Round(rvalue);
                            choices_arr[t, i] = rvalue;
                        }
                        rcount++;
                    }
                    rcount = rcount + 2;
                    model.AddCriterionRatedChoices(j, choices_arr);
                    for (int g = 0; g < count_solution; g++)
                        for (int h = 0; h < count_solution; h++)
                            choices_arr[g, h] = 1;

                }

                // for (all solutions)
                if (!model.CalculatedCriteria())
                {
                    MessageBox.Show(" مصفوفة المقارنات الزوجية للمعايير غير ثابتة، تحقق من مقارناتك من فضلك");
                        return;
                }
                int result = 0;
                double[] choices = model.CalculatedChoices(out result);
                if (result!=-1)
                {
                    MessageBox.Show(" مصفوفة المقارنات الزوجية لمصفوفة مقارنات البدائل تبعا للمعيار " +result.ToString()+"غير ثابتة، تحقق من مقارناتك من فضلك");
                    return;
                }
                int ii = 0;
                double s = 0, si = 0;
                foreach (KeyValuePair<Case, double> pair in sol_cases)
                {
                    EuclideanSimilarity sim = new EuclideanSimilarity();
                    si = sim.Similarity(pair.Key, _problem);
                    //  s = System.Math.Round(choices.GetElement(ii, 0) * 100, 0);
                    s = choices[ii] * 100;
                    listBox1.Items.Add(" تثقيل الحل " + DG_sol.Rows[0].Cells[ii + 1].Value.ToString() + "\t" + s + "\t" + "Similarity =   " + si);
                    ii++;

                }
                tabControl1.SelectTab(tabPage3);
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            all_cases = Db.get_all_cases(mybestchoice.TableName, mybestchoice.CaseName);
            DataTable dt = Db.get_table_contetnt_condition("tbl_cases", "case_name", comboBox1.Text);
            string standrize_table = dt.Rows[0]["standrize_table"].ToString();
            string standrize_type = dt.Rows[0]["standrize_type"].ToString();
            dt = Db.get_table_contetnt_condition(standrize_table, "cluster", "-1");

            Case Max_Mean = Db.get_Case_data(standrize_table, -2);
            Case Min_Sd = Db.get_Case_data(standrize_table, -3);
            Random r = new Random();
            List<Case> random_cases = new List<Case>();
            for (int j = 0; j < Max_Mean.GetFeatures().Count; j++)
            {
                Feature max_f = (Feature)Max_Mean.GetFeatures()[j];
                dataGridView3.Columns.Add(max_f.GetFeatureName(), max_f.GetFeatureName());
            }
            for (int i = 0; i < 100; i++)
            {
                double x = 0;
                Case c = new Case(0, Max_Mean.GetCaseName(), "");
                dataGridView3.Rows.Add();
                for (int j = 0; j < Max_Mean.GetFeatures().Count; j++)
                {
                    Feature max_f = (Feature)Max_Mean.GetFeatures()[j];
                    Feature min_f = (Feature)Min_Sd.GetFeatures()[j];
                    if (max_f.GetFeatureName() == "id")
                        x = (double)i;
                    else if (max_f.GetFeatureName() == "cluster")
                        x = -1;
                    else if (max_f.GetFeatureType() == 8)
                    {
                        c.AddFeature(max_f.GetFeatureName(), max_f.GetFeatureType(), "", max_f.GetWeight(), max_f.GetIsKey(), max_f.GetIsIndex(), max_f.GetFeatureUnit());
                        dataGridView3.Rows[i].Cells[j].Value = "";
                        continue;
                    }
                    else
                    {

                        double max = Convert.ToDouble(max_f.GetFeatureValue());
                        double min = Convert.ToDouble(min_f.GetFeatureValue());
                        x = r.NextDouble(max, min);
                        x = Math.Round(x, 1);
                    }

                    c.AddFeature(max_f.GetFeatureName(), max_f.GetFeatureType(), x, max_f.GetWeight(), max_f.GetIsKey(), max_f.GetIsIndex(), max_f.GetFeatureUnit());
                    dataGridView3.Rows[i].Cells[j].Value = x;
                }
                //  i have to put general seetings for normalization 
                c = BestChoice.StandardizeCase(c, Max_Mean, Min_Sd, standrize_type);
                random_cases.Add(c);
            }

            // non clustered cases

            set_xml();
            ArrayList myarr = Db.get_cases_as_arraylist_condition(standrize_table, mybestchoice.CaseName);
            DefaultEngine _engine = new DefaultEngine();
            string _env = @"E:\D Drive\_SAWSAN MASTER\MobileApplication\Fahp_cbr_app\Fahp_cbr_app\CBR\config.xml";
            List<Case> NonClusterdArrCases;
            AllNonClusterdCases = new Dictionary<Case, List<Case>>();
            for (int f = 0; f < random_cases.Count; f++)
            {
                // get particular cluster
                _engine.SetEnvironmentVariable(_env);
                _engine.SetProblem(random_cases[f]);
                _engine.Startup();
                ArrayList cases = _engine.Run_caseRetrieval_partial(myarr);
                count_solution = cases.Count;
                 NonClusterdArrCases = new List<Case>();
                if (count_solution != 0)
                {
                    Stat ss = null;
                    Case cc = null;
                    for (int i = 0; i < cases.Count; i++)
                    {
                        ss = (Stat)(cases[i]);
                        cc = ss.GetCBRCase();
                        NonClusterdArrCases.Add(cc);
                    }
                    AllNonClusterdCases.Add(random_cases[f], NonClusterdArrCases);
                }
            }
         
            // with Clustering
            // get centroids
            dt = Db.get_table_contetnt_condition(standrize_table, "cluster", "-1");

            List<Case> centroids = new List<Case>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Case c = new Case(0, random_cases[0].GetCaseName(), "");
                for (int j = 0; j < random_cases[0].GetFeatures().Count; j++)
                {
                    Feature f = (Feature)random_cases[0].GetFeatures()[j];
                    c.AddFeature(f.GetFeatureName(), f.GetFeatureType(), dt.Rows[i][j].ToString(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                }
                centroids.Add(c);
            }
            AllclusterdCases = new Dictionary<Case, List<Case>>();
            List<Case> ClusterdArrCases;
            // know which cluster
            Kmeans kmeans = new Kmeans();
            for (int u = 0; u < random_cases.Count; u++)
            {
                ClusterdArrCases = new List<Case>();
                current_cases_partial.Clear();
                int t = kmeans.findCluster_customize(random_cases[u], centroids);

                // get particular cluster
                dt = Db.get_table_contetnt_condition(standrize_table, "cluster", t.ToString());

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Case c = new Case(0, random_cases[0].GetCaseName(), "");
                    for (int j = 0; j < random_cases[0].GetFeatures().Count; j++)
                    {
                        Feature f = (Feature)random_cases[0].GetFeatures()[j];
                        c.AddFeature(f.GetFeatureName(), f.GetFeatureType(), dt.Rows[i][j].ToString(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                    }
                    current_cases_partial.Add(c);
                }
                //DefaultEngine _engine = new DefaultEngine();
                // string _env = @"E:\D Drive\_SAWSAN MASTER\MobileApplication\Fahp_cbr_app\Fahp_cbr_app\CBR\config.xml";
                _engine.SetEnvironmentVariable(_env);
                _engine.SetProblem(random_cases[u]);
                // Run the reasoning engine
                _engine.Startup();
                ArrayList mcases = _engine.Run_caseRetrieval_partial(current_cases_partial);
                //DG_Solution.Rows.Clear();
                count_solution = mcases.Count;

                if (count_solution != 0)
                {
                    Stat ss = null;
                    Case cc = null;
                    for (int i = 0; i < mcases.Count; i++)
                    {
                        ss = (Stat)(mcases[i]);
                        cc = ss.GetCBRCase();
                        ClusterdArrCases.Add(cc);
                    }
                    AllclusterdCases.Add(random_cases[u], ClusterdArrCases);
                }

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //Criteria
            input_file = true;

            // criteria
            DG_comparing.Columns.Clear();
            DG_sol_cri.Rows.Clear();
            ArrayList crlist = new ArrayList();


            for (int i = 0; i < DG_Criteria.Rows.Count; i++)
                if (Convert.ToBoolean(DG_Criteria.Rows[i].Cells["Checkbox"].Value) == true)
                    crlist.Add(DG_Criteria.Rows[i].Cells["FeatureName"].Value.ToString());

            count_citeria = crlist.Count;

            /*    DG_comparing.Columns.Add("first_cr", "المعيار الأول");
                DG_comparing.Columns.Add("second_cr", "المعيار الثاني");
                DG_comparing.Rows.Add((crlist.Count * (crlist.Count - 1)) / 2);

                DataGridViewComboBoxColumn cmb = new DataGridViewComboBoxColumn();
                cmb.HeaderText = "درجة الأهمية";
                cmb.Name = "cmb";
                cmb.MaxDropDownItems = 5;
                cmb.Items.Add("المعيار الأول مهم جداً بالنسبة للمعيار الثاني");
                cmb.Items.Add("المعيار الأول مهم بالنسبة للمعيار الثاني");
                cmb.Items.Add("المعيار الأول مهم إلى حد ما بالنسبة للمعيار الثاني");
                cmb.Items.Add("المعيار الأول مهم قليلاً بالنسبة للمعيار الثاني");
                cmb.Items.Add("المعيار الثاني مهم جداً بالنسبة للمعيار الأول");
                cmb.Items.Add("المعيار الثاني مهم بالنسبة للمعيار الأول");
                cmb.Items.Add("المعيار الثاني مهم إلى حد ما بالنسبة للمعيار الأول");
                cmb.Items.Add("المعيار الثاني مهم قليلاً بالنسبة للمعيار الأول");
                cmb.Items.Add("أهمية متساوية");
                DG_comparing.Columns.Add(cmb);
                for (int i = 0; i < crlist.Count - 1; i++)
                    for (int j = i + 1; j < crlist.Count; j++)
                    {
                        DG_comparing.Rows[count].Cells[0].Value = crlist[i].ToString();
                        DG_comparing.Rows[count].Cells[1].Value = crlist[j].ToString();
                        count++;
                    }*/
            DG_comparing.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
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

                for (int k = 0; k < count_citeria + 1; k++)
                    DG_comparing.Columns.Add("", "");
                DG_comparing.Rows.Add();
                DG_comparing.Rows[0].DefaultCellStyle.BackColor = Color.RoyalBlue;
                DG_comparing.Columns[0].DefaultCellStyle.BackColor = Color.RoyalBlue;
                for (int k = 0; k < count_citeria; k++)
                    DG_comparing.Rows[0].Cells[k + 1].Value = crlist[k].ToString();
                int c = 0;
                for (int k = 0; k < count_citeria; k++)
                {
                    DG_comparing.Rows.Add();
                    c++;
                    DG_comparing.Rows[c].Cells[0].Value = crlist[k].ToString();
                }



                int linecount = 1;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    for (int i = 0; i < crlist.Count; i++)
                        DG_comparing.Rows[linecount].Cells[i + 1].Value = values[i];
                    linecount++;

                }
                fs.Close();
            }

            //solutions---------------------------------------------------------------------------------------------------------- 
            CreateChoiceComp_Dispaly();
            /*  url = "";
             //System.IO.StreamReader reader = null;
            // System.IO.FileStream fs = null;
            // OpenFileDialog openFileDialog1 = new OpenFileDialog();
             // Set filter options and filter index.
             openFileDialog1.Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*";
             openFileDialog1.FilterIndex = 1;

             openFileDialog1.Multiselect = true;

             // Call the ShowDialog method to show the dialog box.
            // DialogResult userClickedOK = openFileDialog1.ShowDialog();
             userClickedOK = openFileDialog1.ShowDialog();

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

                 for (int k = 0; k < count_solution + 1; k++)
                     DG_sol_cri.Columns.Add("", "");
                 DG_sol_cri.Columns[0].DefaultCellStyle.BackColor = Color.RoyalBlue;

                 int c = 0;
                 int row = c;
                 DG_sol_cri.Rows.Add();
                 for (int cri_id = 0; cri_id < count_citeria; cri_id++)
                 {
                     DG_sol_cri.Rows[c].Cells[0].Value = " معيار " + crlist[cri_id];
                     DG_sol_cri.Rows[c].DefaultCellStyle.BackColor = Color.RoyalBlue;

                     for (int k = 1; k < count_solution + 1; k++)
                     {
                         DG_sol_cri.Rows[row].Cells[k].Value = DG_sol.Rows[0].Cells[k].Value;
                         DG_sol_cri.Rows.Add();
                         c++;
                         DG_sol_cri.Rows[c].Cells[0].Value = DG_sol.Rows[0].Cells[k].Value;
                     }
                     DG_sol_cri.Rows.Add();
                     c++;
                     row = c;
                 }

                 // from file
                 int linecount = 0;
                 count = 0;
                 int cri_count = 0;
                 while (!reader.EndOfStream)
                 {

                     var line = reader.ReadLine();
                     var values = line.Split(',');

                     if (count % count_solution == 0)
                     {
                         while (values[1].ToUpper().Trim() != crlist[cri_count].ToString().ToUpper().Trim())
                         {
                             if (!reader.EndOfStream)
                             {
                                 line = reader.ReadLine();
                                 values = line.Split(',');
                             }
                             else
                             {
                                 MessageBox.Show("عدم تطابق بين   معايير الملف ومعايير الواجهة");
                                 fs.Close();
                                 return;
                             }
                         }
                         cri_count++;
                         line = reader.ReadLine();
                         values = line.Split(',');
                         if (count != 0)
                             linecount = linecount + 2;
                         else linecount++;
                     }
                     else linecount++;
                     for (int i = 0; i < count_solution; i++)
                         DG_sol_cri.Rows[linecount].Cells[i + 1].Value = values[i];


                     count++;
                 }
               
             }*/
            

        }

        private void button10_Click(object sender, EventArgs e)
        {
            
             StringBuilder sb = new StringBuilder();
             for (int i = 0; i < dataGridView4.Rows.Count; i++)
              
             {
                 string s = dataGridView4.Rows[i].Cells[0].Value.ToString()+","
                    + dataGridView4.Rows[i].Cells[1].Value.ToString() + ","
                   + dataGridView4.Rows[i].Cells[2].Value.ToString() + ","
                   + dataGridView4.Rows[i].Cells[3].Value.ToString();
                 sb.Append(s);

                 sb.AppendLine();
             }
             sb.Remove(sb.Length - 1, 1); // Just to avoid copying last empty row
             Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
            
            /*{ string s = dataGridView4.Rows[i].Cells[0].Value.ToString()
                 +dataGridView4.Rows[i].Cells[1].Value.ToString()
                + dataGridView4.Rows[i].Cells[2].Value.ToString()
                +dataGridView4.Rows[i].Cells[3].Value.ToString();
                 sb.Append(s);
                    
                 sb.AppendLine();
             }
         for (int i = 0; i < listBox1.Items.Count; i++)
         {
             string s = dataGridView4.Rows[i].Cells[0].Value.ToString()
                 +dataGridView4.Rows[i].Cells[1].Value.ToString()
                + dataGridView4.Rows[i].Cells[2].Value.ToString()
                +dataGridView4.Rows[i].Cells[3].Value.ToString();
                 sb.Append(s);
                    
                 sb.AppendLine();
             }
             sb.Remove(sb.Length - 1, 1); // Just to avoid copying last empty row
             Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
           
     }*/
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ArrayList crlist = new ArrayList();
            dataGridView5.Columns.Add("رقم الحل", "رقم الحل");
            dataGridView5.Columns.Add("رقم المجموعة", "رقم المجموعة");
            dataGridView5.Columns.Add("التثقيل", "التثقيل");
            dataGridView5.Columns.Add("التشابه", "التشابه");
            for (int i = 0; i < DG_Criteria.Rows.Count; i++)
                if (Convert.ToBoolean(DG_Criteria.Rows[i].Cells["Checkbox"].Value) == true)
                    crlist.Add(DG_Criteria.Rows[i].Cells["FeatureName"].Value.ToString());

            count_citeria = crlist.Count;
            int count = 0;

            // Criteria Comparsions------------------------
            for (int p = 0; p < AllclusterdCases.Count; p++)
            {
            create:
                double[,] cri_arr = new double[crlist.Count, crlist.Count];
                for (int i = 0; i < crlist.Count - 1; i++)
                {
                    Random r = new Random();
                    cri_arr[i, i] = 1;
                    cri_arr[i, i + 1] = r.Next(2, 9);
                    cri_arr[i + 1, i] = 1 / cri_arr[i, i + 1];
                }
                cri_arr[crlist.Count - 1, crlist.Count - 1] = 1;
                int m = 0;
                for (int i = 0; i < crlist.Count; i++)
                    for (int k = i + 2; k < crlist.Count; k++)
                    {
                        Random r = new Random();
                        if (cri_arr[i, k - 1] > cri_arr[k - 1, k])
                            m = (int)cri_arr[i, k - 1];
                        else
                            m = (int)cri_arr[k - 1, k];

                        cri_arr[i, k] = r.Next(m, 9);
                        cri_arr[k, i] = 1 / cri_arr[i, k];

                    }
                // check
                Case _problem = AllclusterdCases.Keys.ElementAt(p);
                List<Case> cases = AllclusterdCases.Values.ElementAt(p);
                MyAHPModel model = new MyAHPModel(count_citeria, cases.Count);
                if (model.CheckConsistency(cri_arr, crlist.Count))
                {
                    AllCriteriaComp.Add(cri_arr);
                    model.AddCriteria(cri_arr);
                    // Choices Comparsions------------------------
                    Dictionary<int, double[,]> AllChoiceComp = CreateChoiceComp(cases);
                    for (int u = 0; u < count_citeria; u++)
                        model.AddCriterionRatedChoices(u, AllChoiceComp[u]);
                    model.CalculatedCriteria();
                    int result = 0;
                    double[] choices = model.CalculatedChoices(out result);
                    int ii = 0;
                    double s = 0, si = 0;
                    double[,] w = new double[cases.Count, 2];
                    foreach (Case c in cases)
                    {

                        EuclideanSimilarity sim = new EuclideanSimilarity();
                        si = sim.Similarity(c, _problem);
                        //  s = System.Math.Round(choices.GetElement(ii, 0) * 100, 0);
                        s = choices[ii] * 100;
                        dataGridView5.Rows.Add();
                        dataGridView5.Rows[count].Cells[0].Value = c.GetFeature("id").GetFeatureValue();
                        dataGridView5.Rows[count].Cells[1].Value = p.ToString();
                        dataGridView5.Rows[count].Cells[2].Value = s.ToString();
                        dataGridView5.Rows[count].Cells[3].Value = si.ToString();
                        w[ii, 0] = Convert.ToDouble(c.GetFeature("id").GetFeatureValue());
                        w[ii, 1] = s;
                        count++;
                        ii++;
                    }
                    AllclusterdResults.Add(_problem, w);

                }
                else goto create;

            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView6_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            if ((conn.State == ConnectionState.Closed) && (comboBox2.SelectedValue.ToString() != "System.Data.DataRowView"))
            {
                ArrayList myarr = Db.get_cases_as_arraylist(comboBox2.SelectedValue.ToString(), comboBox2.Text);
                DataTable mydt = Db.get_table_contetnt_condition("tbl_cases", "case_name", comboBox2.Text);
                mybestchoice = new BestChoice(myarr, mydt.Rows[0]["standrize_type"].ToString(), mydt.Rows[0]["kmeans_method"].ToString(), mydt.Rows[0]["clusters_num"].ToString());
                conn.Open();

                string select_name = "select * from tbl_dic where caseid ='" + comboBox2.Text + "'";
                //   string select_name = " SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.Columns where TABLE_NAME = '" + comboBox1.SelectedValue + "'";
                SqlCommand cmd = new SqlCommand(select_name, conn);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                int p = sd.Fill(dt);
            }
                conn.Close();
            
        }

        private void button13_Click(object sender, EventArgs e)
        {
            dg_randomcases.Columns.Clear();
            all_cases = Db.get_all_cases(mybestchoice.TableName, mybestchoice.CaseName);
            DataTable dt = Db.get_table_contetnt_condition("tbl_cases", "case_name", comboBox2.Text);
            string standrize_table = dt.Rows[0]["standrize_table"].ToString();
            string standrize_type = dt.Rows[0]["standrize_type"].ToString();
           // dt = Db.get_table_contetnt_condition(standrize_table, "cluster", "-1");

            Case Max_Mean = Db.get_Case_data(standrize_table, -2);
            Case Min_Sd = Db.get_Case_data(standrize_table, -3);
            Random r = new Random();
            exp_random_cases = new List<Case>();
            for (int j = 0; j < Max_Mean.GetFeatures().Count; j++)
            {
                Feature max_f = (Feature)Max_Mean.GetFeatures()[j];
                dg_randomcases.Columns.Add(max_f.GetFeatureName(), max_f.GetFeatureName());
            }
            for (int i = 0; i < 100; i++)
            {
                double x = 0;
                Case c = new Case(0, Max_Mean.GetCaseName(), "");
                dg_randomcases.Rows.Add();
                for (int j = 0; j < Max_Mean.GetFeatures().Count; j++)
                {
                    Feature max_f = (Feature)Max_Mean.GetFeatures()[j];
                    Feature min_f = (Feature)Min_Sd.GetFeatures()[j];
                    if (max_f.GetFeatureName() == "id")
                        x = (double)i;
                    else if (max_f.GetFeatureName() == "cluster")
                        x = -1;
                    else if (max_f.GetFeatureType() >= 5 && max_f.GetFeatureType() <= 9)// mstring, string, ordinal, categorical
                    {
                        int k = r.Next(0, all_cases.Count);
                        Feature f = (Feature)all_cases[k].GetFeatures()[j];
                        c.AddFeature(max_f.GetFeatureName(), max_f.GetFeatureType(), f.GetFeatureValue(), max_f.GetWeight(), max_f.GetIsKey(), max_f.GetIsIndex(), max_f.GetFeatureUnit());
                        dg_randomcases.Rows[i].Cells[j].Value = f.GetFeatureValue();
                        continue;
                    }
                    else if (max_f.GetFeatureType() == 1 )// mstring, string, ordinal, categorical
                    {
                        int k = r.Next(0,2);
                        bool o = false;
                        if (k == 1) o = true;
                        c.AddFeature(max_f.GetFeatureName(), max_f.GetFeatureType(), o, max_f.GetWeight(), max_f.GetIsKey(), max_f.GetIsIndex(), max_f.GetFeatureUnit());
                        dg_randomcases.Rows[i].Cells[j].Value = o.ToString();
                        continue;
                    }
                    else if (max_f.GetFeatureType() == 2)
                    {

                        int max = Convert.ToInt32(max_f.GetFeatureValue());
                        int min = Convert.ToInt32(min_f.GetFeatureValue());
                        x = r.Next(min,max+1); 
                    }
                    else 
                    {

                        double max = Convert.ToDouble(max_f.GetFeatureValue());
                        double min = Convert.ToDouble(min_f.GetFeatureValue());
                        x = r.NextDouble(min, max+0.05);
                        x = Math.Round(x, 1);
                    }

                    c.AddFeature(max_f.GetFeatureName(), max_f.GetFeatureType(), x, max_f.GetWeight(), max_f.GetIsKey(), max_f.GetIsIndex(), max_f.GetFeatureUnit());
                    dg_randomcases.Rows[i].Cells[j].Value = x;
                }
                //  i have to put general seetings for normalization 
                c = BestChoice.StandardizeCase(c, Max_Mean, Min_Sd, standrize_type);
                exp_random_cases.Add(c);
            }

          
            
        }

        private void button14_Click(object sender, EventArgs e)
        {
            

            richTextBox2.Text = "Without clustering\n";
            //double begin = Convert.ToDouble(txt_first.Text);
            //double end = Convert.ToDouble(txt_end.Text);
            //double first = begin;
            double first = 0.1;
            dg_exp_no_cluster_results.Columns.Clear();
          //  dg_exp_no_cluster_results.Columns.Add("رقم رغبة الزبون", "رقم رغبة الزبون");
          //  dg_exp_no_cluster_results.Columns.Add("عدد الحالات الناتحة", "عدد الحالات الناتحة");
            dg_exp_no_cluster_results.Columns.Add("الحد الأعلى لنسبة التشابه المدخلة من قبل الزبون", "الحد الأعلى لنسبة التشابه المدخلة من قبل الزبون");
            string s = "نسبة تشابه الحل ذو الترتيب ";
            for (int i = 1; i < 8;i++ )
                dg_exp_no_cluster_results.Columns.Add(s + i.ToString(), s + i.ToString());
            dg_exp_no_cluster_results.Rows.Add();
           // dg_exp_no_cluster_results.Columns.Add("الوقت المستهلك", "الوقت المستهلك");
            //int j = 0;
            ArrayList myarr = Db.get_cases_as_arraylist_condition(mybestchoice.Tablename_Standardization, mybestchoice.CaseName);
          List<Case> myarrcases=Db.get_all_cases(mybestchoice.Tablename_Standardization, mybestchoice.CaseName);
          //int simcount = 0;

        
          while (first >= 0.1 && first <1)
           {
               // double avgtime = 0;
                // repeat 100 times for each experiment
                //for (int uu = 0; uu < 1; uu++)
                //{
                 //   ;
                 //   double time = 0;
               int count_time = 0;
                   double avg_no_res = 0;

                    // non clustered cases
                    
                    set_xml_sim(first.ToString());
                    DefaultEngine _engine = new DefaultEngine();
                    string _env = @"E:\D Drive\_SAWSAN MASTER\MobileApplication\Fahp_cbr_app\Fahp_cbr_app\CBR\config.xml";
                    List<Case> NonClusterdArrCases;
                    AllNonClusterdCases = new Dictionary<Case, List<Case>>();

                    for (int f = 0; f < exp_random_cases.Count; f++)
                    {
                        // get particular cluster
                        _engine.SetEnvironmentVariable(_env);
                        _engine.SetProblem(exp_random_cases[f]);
                        _engine.Startup();
                        // calculate time for each case
                        var watch = System.Diagnostics.Stopwatch.StartNew();
                        ArrayList cases = _engine.Run_caseRetrieval_partial(myarr);
                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;
                        count_solution = cases.Count;
                        NonClusterdArrCases = new List<Case>();
                        if (count_solution != 0)
                        {
                            count_time++;
                           // time += Convert.ToDouble(elapsedMs);
                            Stat ss = null;
                            Case cc = null;
                            for (int i = 0; i < cases.Count; i++)
                            {
                                ss = (Stat)(cases[i]);
                                cc = ss.GetCBRCase();
                                NonClusterdArrCases.Add(cc);
                            }
                            AllNonClusterdCases.Add(exp_random_cases[f], NonClusterdArrCases);

                            /* dg_exp_no_cluster_results.Rows.Add();
                             dg_exp_no_cluster_results.Rows[j].Cells[0].Value = f.ToString();
                             dg_exp_no_cluster_results.Rows[j].Cells[1].Value = NonClusterdArrCases.Count;
                             dg_exp_no_cluster_results.Rows[j].Cells[2].Value = first.ToString();
                             dg_exp_no_cluster_results.Rows[j].Cells[3].Value = elapsedMs.ToString();
                             j++;*/
                             avg_no_res += NonClusterdArrCases.Count;
                             
                        } // end if
                    } // end for randomcases

                   /* get_comparison_withoutclustering();
                    sort_without_clustering();
                    
                    double[,] simarr = new double[18, 7];
                    EuclideanSimilarity ssi = new EuclideanSimilarity();
                    double sim=0;
                    int caseid =0;
                    dg_exp_no_cluster_results.Rows[simcount].Cells[0].Value = first.ToString();
                    for (int i = 0; i < 7; i++)
                    {
                        int sols = 0;
                     for (int h = 0; h < AllNonClusterdResults.Count; h++)
                      {
                          if (i > AllNonClusterdResults.ElementAt(h).Value.GetLength(0) -1)
                              continue;
                            sols++;
                            caseid = Convert.ToInt32(AllNonClusterdResults.ElementAt(h).Value[i,0]);
                            Case c = myarrcases[caseid];
                            sim = ssi.Similarity(exp_random_cases[h], c);
                            simarr[simcount, i] += sim ;
                      }
                           simarr[simcount, i] /= sols;
                           dg_exp_no_cluster_results.Rows[simcount].Cells[i + 1].Value = simarr[simcount, i];
                            
                     }
                  /*  time = time / count_time;
                    //richTextBox2.Text = richTextBox2.Text + time.ToString() + "    for sim  " + first.ToString() + "\n";
                    avg_no_res = Math.Round(avg_no_res / count_time);
                    txt_no_res.Text = txt_no_res.Text + avg_no_res.ToString() + "\t sim is" + first.ToString() + "\n";
                    avgtime += time;*/
                 // end for 100
                //avgtime = Math.Round(avgtime / 100, 2);
               // richTextBox2.Text += " Time =  " + avgtime.ToString() + "\t for sim " + first.ToString() + "\n";
              avg_no_res /= count_time;
                richTextBox2.Text = richTextBox2.Text + avg_no_res.ToString() + "\t sim is" + first.ToString() + "\n";
                first = first + 0.05;
                //simcount++;
                //dg_exp_no_cluster_results.Rows.Add();
              

            }// end while sim
                
        }

        private void button15_Click(object sender, EventArgs e)
        {
            
            //double begin = Convert.ToDouble(txt_first.Text);
          //  double end = Convert.ToDouble(txt_end.Text);
           // double first = begin;
            richTextBox3.Text = "With Clustering \n";
            dg_exp_cluster_results.Columns.Add("الحد الأعلى لنسبة التشابه المدخلة من قبل الزبون", "الحد الأعلى لنسبة التشابه المدخلة من قبل الزبون");
            string s = "نسبة تشابه الحل رقم ";
            for (int i = 1; i < 8; i++)
                dg_exp_cluster_results.Columns.Add(s + i.ToString(), s + i.ToString());
            dg_exp_cluster_results.Rows.Add();
            List<Case> myarrcases = Db.get_all_cases(mybestchoice.Tablename_Standardization, mybestchoice.CaseName);
            // get centroids for case
            double first = 0.1;
            List<Case> centroids = new List<Case>();
            centroids =Db. get_cases_condition_cluster(mybestchoice.Tablename_Standardization, mybestchoice.CaseName, "cluster", "-1");
            Dictionary<int, ArrayList> cases_by_centroids = new Dictionary<int, ArrayList>();
            for (int i=0;i<centroids.Count;i++)
            {
                int k= Convert.ToInt32(centroids[i].GetFeature("id").GetFeatureValue());
                cases_by_centroids.Add(k,Db.get_cases_condition_cluster_arraylist(mybestchoice.Tablename_Standardization, mybestchoice.CaseName, "cluster", k.ToString()));
            }
            
            // with Clustering double first = Convert.ToDouble(txt_first.Text);
            /*dg_exp_cluster_results.Columns.Clear();
            dg_exp_cluster_results.Columns.Add("رقم رغبة الزبون", "رقم رغبة الزبون");
            dg_exp_cluster_results.Columns.Add("عدد الحالات الناتحة", "عدد الحالات الناتحة");
            dg_exp_cluster_results.Columns.Add("نسبة التشابه", "نسبة التشابه");
            dg_exp_cluster_results.Columns.Add("الوقت المستهلك", "الوقت المستهلك");
            //int jj = 0;*/
            //List<double> times = new List<double>();
            
            while (first >= 0.1 && first < 1)
            {
                //double avgtime = 0;
                //int c = 0;
                // repeat 100 times for each experiment
                //for (int uu = 0; uu < 1; uu++)
                //{
                    /*
                    double time = 0;
                    double alltime = 0;*/
                    int count_time = 0;
                    double avg_res = 0;
                    AllclusterdCases = new Dictionary<Case, List<Case>>();
                    List<Case> ClusterdArrCases;
                    // know which cluster
                    Kmeans kmeans = new Kmeans();
                    // experiment is 100  random cases
                    for (int u = 0; u < exp_random_cases.Count; u++)
                    {
                        ClusterdArrCases = new List<Case>();
                        // var watch = System.Diagnostics.Stopwatch.StartNew();
                        int t = kmeans.findCluster_customize(exp_random_cases[u], centroids);
                        // watch.Stop();
                        // var elapsedMs = watch.ElapsedMilliseconds;
                        // time = Convert.ToDouble(elapsedMs);
                        // get particular cluster
                        current_cases_partial = cases_by_centroids[t];
                        set_xml_sim(first.ToString());
                        DefaultEngine _engine = new DefaultEngine();
                        string _env = @"E:\D Drive\_SAWSAN MASTER\MobileApplication\Fahp_cbr_app\Fahp_cbr_app\CBR\config.xml";
                        _engine.SetEnvironmentVariable(_env);
                        _engine.SetProblem(exp_random_cases[u]);
                        // Run the reasoning engine
                        _engine.Startup();
                        // watch = System.Diagnostics.Stopwatch.StartNew();
                        ArrayList mcases = _engine.Run_caseRetrieval_partial(current_cases_partial);
                        // watch.Stop();
                        // elapsedMs = watch.ElapsedMilliseconds;
                        // time += Convert.ToDouble(elapsedMs);
                        //DG_Solution.Rows.Clear();
                        count_solution = mcases.Count;

                        if (count_solution != 0)
                        {
                            Stat ss = null;
                            Case cc = null;
                            count_time++;
                            //alltime = alltime + time;
                            for (int i = 0; i < mcases.Count; i++)
                            {
                                ss = (Stat)(mcases[i]);
                                cc = ss.GetCBRCase();
                                ClusterdArrCases.Add(cc);
                            }
                            AllclusterdCases.Add(exp_random_cases[u], ClusterdArrCases);
                            /* dg_exp_cluster_results.Rows.Add();
                             dg_exp_cluster_results.Rows[jj].Cells[0].Value = u.ToString();
                             dg_exp_cluster_results.Rows[jj].Cells[1].Value = ClusterdArrCases.Count;
                             dg_exp_cluster_results.Rows[jj].Cells[2].Value = first.ToString();
                             dg_exp_cluster_results.Rows[jj].Cells[3].Value = elapsedMs.ToString();
                             jj++;*/
                            avg_res += ClusterdArrCases.Count;
                        }
                    }
                        avg_res /= count_time;
                        richTextBox3.Text = richTextBox3.Text + avg_res.ToString() + "\t sim is" + first.ToString() + "\n";
                        first = first + 0.05;

                        /* }
                         if (count_time != 0)
                         {
                             alltime = alltime / count_time;// +ttime;
                             alltime = Math.Round(alltime, 0);
                             avgtime += alltime;
                             c++;
                         }=
                         // richTextBox3.Text = richTextBox3.Text +" Time =  "+ alltime.ToString() + "\t for round " +uu.ToString()+"\n";
                        // avg_res = Math.Round(avg_res / count_time);
                         //txt_res.Text = txt_res.Text + avg_res.ToString() + "\t sim is" + first.ToString() + "\n";
                    
                     }// // end 100 time
                    // avgtime = Math.Round(avgtime / c, 2);
                    // richTextBox3.Text += " Time =  " + avgtime.ToString() + "\t for sim " + first.ToString() + "\n";
                 //    first = first + 0.05;
               //  } // end while for sim*/
                   
                    /* get_comparison_withclustering();
                     sort_with_clustering();

             double[,] simarr = new double[18, 7];
             EuclideanSimilarity ssi = new EuclideanSimilarity();
             double sim = 0;
             int caseid = 0;
             dg_exp_cluster_results.Rows[simcount].Cells[0].Value = first.ToString();
             for (int i = 0; i < 7; i++)
             {
                 int sols = 0;
                 for (int h = 0; h < AllclusterdResults.Count; h++)
                 {
                     if (i > AllclusterdResults.ElementAt(h).Value.GetLength(0) - 1)
                         continue;
                     sols++;
                     caseid = Convert.ToInt32(AllclusterdResults.ElementAt(h).Value[i, 0]);
                     Case c = myarrcases[caseid];
                     sim = ssi.Similarity(exp_random_cases[h], c);
                     simarr[simcount, i] += sim;
                 }
                 simarr[simcount, i] /= sols;
                 dg_exp_cluster_results.Rows[simcount].Cells[i + 1].Value = simarr[simcount, i];

             }
                     * simcount++;
                 dg_exp_cluster_results.Rows.Add();*/
                    
                

            }// end while sim
        }

        private void button16_Click(object sender, EventArgs e)
        {

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < dg_exp_no_cluster_results.Rows.Count-1; i++)
            {
                string s = dg_exp_no_cluster_results.Rows[i].Cells[0].Value.ToString() + ","
                   + dg_exp_no_cluster_results.Rows[i].Cells[1].Value.ToString() + ","
                  + dg_exp_no_cluster_results.Rows[i].Cells[2].Value.ToString() + ","
                  + dg_exp_no_cluster_results.Rows[i].Cells[3].Value.ToString();

                sb.Append(s);

                sb.AppendLine();
            }
            sb.Remove(sb.Length - 1, 1); // Just to avoid copying last empty row
            Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
        }

        private void button17_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dg_exp_cluster_results.Rows.Count-1; i++)
            {
                string s = dg_exp_cluster_results.Rows[i].Cells[0].Value.ToString() + ","
                   + dg_exp_cluster_results.Rows[i].Cells[1].Value.ToString() + ","
                  + dg_exp_cluster_results.Rows[i].Cells[2].Value.ToString() + ","
                  + dg_exp_cluster_results.Rows[i].Cells[3].Value.ToString();
                
                sb.Append(s);

                sb.AppendLine();
            }
            sb.Remove(sb.Length - 1, 1); // Just to avoid copying last empty row
            Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
        }

        private void tabPage10_Click(object sender, EventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {
            
        }

        private void button18_Click_1(object sender, EventArgs e)
        {
             dg_randomcases.Columns.Clear();
           
            DataTable dt = Db.get_table_contetnt_condition("tbl_cases", "case_name", comboBox2.Text);
            string standrize_table = dt.Rows[0]["standrize_table"].ToString();
            string standrize_type = dt.Rows[0]["standrize_type"].ToString();

            Case Max_Mean = Db.get_Case_data(standrize_table, -2);
            Case Min_Sd = Db.get_Case_data(standrize_table, -3);

            exp_random_cases = new List<Case>();
            dg_randomcases.Columns.Clear();
            dg_randomcases.DataSource = null;
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

                if (ListCases.Count != 0)
                {
                    for (int j = 0; j < ListCases[0].GetFeatures().Count; j++)
                    {
                        Feature feature = (Feature)ListCases[0].GetFeatures()[j];
                        dg_randomcases.Columns.Add(feature.GetFeatureName().ToString(), feature.GetFeatureName().ToString());
                    }

                    dg_randomcases.Rows.Add(ListCases.Count);
                }
                for (int i = 0; i < ListCases.Count; i++)
                {
                    Case c = ListCases[i];
                    for (int j = 0; j < c.GetFeatures().Count; j++)
                    {
                        Feature f = (Feature)c.GetFeatures()[j];
                        dg_randomcases.Rows[i].Cells[j].Value = f.GetFeatureValue();
                    }
                    //  i have to put general seetings for normalization 
                    c = BestChoice.StandardizeCase(c, Max_Mean, Min_Sd, standrize_type);
                    exp_random_cases.Add(c);
                }

            }

           
        }

        private void button19_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < dg_randomcases.Rows.Count - 1; i++)
            {  
                string s="";
                for (int j = 0; j < dg_randomcases.Columns.Count; j++)
                 s += dg_randomcases.Rows[i].Cells[j].Value.ToString() + ",";
                sb.Append(s);
                sb.AppendLine();
            }
            sb.Remove(sb.Length - 1, 1); // Just to avoid copying last empty row
            Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
        }

        private void button12_Click_1(object sender, EventArgs e)
        {

             
        }

        private void button12_Click_2(object sender, EventArgs e)
        {



        }

        private void button21_Click(object sender, EventArgs e)
        {

            ArrayList crlist = new ArrayList();
            dataGridView4.Columns.Add("رقم الحل", "رقم الحل");
            dataGridView4.Columns.Add("رقم المجموعة", "رقم المجموعة");
            dataGridView4.Columns.Add("التثقيل", "التثقيل");
            dataGridView4.Columns.Add("التشابه", "التشابه");
            for (int i = 0; i < DG_Criteria.Rows.Count; i++)
                if (Convert.ToBoolean(DG_Criteria.Rows[i].Cells["Checkbox"].Value) == true)
                    crlist.Add(DG_Criteria.Rows[i].Cells["FeatureName"].Value.ToString());

            count_citeria = crlist.Count;
            int count = 0;

            // Criteria Comparsions------------------------
            for (int p = 0; p < AllNonClusterdCases.Count; p++)
            {
            create:
                double[,] cri_arr = new double[crlist.Count, crlist.Count];
                for (int i = 0; i < crlist.Count - 1; i++)
                {
                    Random r = new Random();
                    cri_arr[i, i] = 1;
                    cri_arr[i, i + 1] = r.Next(2, 9);
                    cri_arr[i + 1, i] = 1 / cri_arr[i, i + 1];
                }
                cri_arr[crlist.Count - 1, crlist.Count - 1] = 1;
                int m = 0;
                for (int i = 0; i < crlist.Count; i++)
                    for (int k = i + 2; k < crlist.Count; k++)
                    {
                        Random r = new Random();
                        if (cri_arr[i, k - 1] > cri_arr[k - 1, k])
                            m = (int)cri_arr[i, k - 1];
                        else
                            m = (int)cri_arr[k - 1, k];

                        cri_arr[i, k] = r.Next(m, 9);
                        cri_arr[k, i] = 1 / cri_arr[i, k];

                    }
                // check
                Case _problem = AllNonClusterdCases.Keys.ElementAt(p);
                List<Case> cases = AllNonClusterdCases.Values.ElementAt(p);
                MyAHPModel model = new MyAHPModel(count_citeria, cases.Count);
                if (model.CheckConsistency(cri_arr, crlist.Count))
                {
                    AllCriteriaComp.Add(cri_arr);
                    model.AddCriteria(cri_arr);
                    // Choices Comparsions------------------------
                    Dictionary<int, double[,]> AllChoiceComp = CreateChoiceComp(cases);
                    for (int u = 0; u < count_citeria; u++)
                        model.AddCriterionRatedChoices(u, AllChoiceComp[u]);
                    model.CalculatedCriteria();
                    int result = 0;
                    double[] choices = model.CalculatedChoices(out result);
                    int ii = 0;
                    double s = 0, si = 0;
                    double[,] w = new double[cases.Count, 2];
                    foreach (Case c in cases)
                    {

                        EuclideanSimilarity sim = new EuclideanSimilarity();
                        si = sim.Similarity(c, _problem);
                        //  s = System.Math.Round(choices.GetElement(ii, 0) * 100, 0);
                        s = choices[ii] * 100;
                        dataGridView4.Rows.Add();
                        dataGridView4.Rows[count].Cells[0].Value = c.GetFeature("id").GetFeatureValue();
                        dataGridView4.Rows[count].Cells[1].Value = p.ToString();
                        dataGridView4.Rows[count].Cells[2].Value = s.ToString();
                        dataGridView4.Rows[count].Cells[3].Value = si.ToString();
                        count++;
                        w[ii, 0] = Convert.ToDouble(c.GetFeature("id").GetFeatureValue());
                        w[ii, 1] = s;
                        // listBox1.Items.Add(" تثقيل الحل " + c.GetFeature("id").GetFeatureValue()+"\t من الحل رقم"+p.ToString() + "\t" + s + "\t" + "Similarity =   " + si);
                        ii++;
                    }
                    AllNonClusterdResults.Add(_problem, w);

                }
                else goto create;

            }
            
        }

        private void button20_Click(object sender, EventArgs e)
        {
            ArrayList crlist = new ArrayList();
            dataGridView5.Columns.Add("رقم الحل", "رقم الحل");
            dataGridView5.Columns.Add("رقم المجموعة", "رقم المجموعة");
            dataGridView5.Columns.Add("التثقيل", "التثقيل");
            dataGridView5.Columns.Add("التشابه", "التشابه");
            for (int i = 0; i < DG_Criteria.Rows.Count; i++)
                if (Convert.ToBoolean(DG_Criteria.Rows[i].Cells["Checkbox"].Value) == true)
                    crlist.Add(DG_Criteria.Rows[i].Cells["FeatureName"].Value.ToString());

            count_citeria = crlist.Count;
            int count = 0;

            // Criteria Comparsions------------------------
            for (int p = 0; p < AllclusterdCases.Count; p++)
            {
            create:
                double[,] cri_arr = new double[crlist.Count, crlist.Count];
                for (int i = 0; i < crlist.Count - 1; i++)
                {
                    Random r = new Random();
                    cri_arr[i, i] = 1;
                    cri_arr[i, i + 1] = r.Next(2, 9);
                    cri_arr[i + 1, i] = 1 / cri_arr[i, i + 1];
                }
                cri_arr[crlist.Count - 1, crlist.Count - 1] = 1;
                int m = 0;
                for (int i = 0; i < crlist.Count; i++)
                    for (int k = i + 2; k < crlist.Count; k++)
                    {
                        Random r = new Random();
                        if (cri_arr[i, k - 1] > cri_arr[k - 1, k])
                            m = (int)cri_arr[i, k - 1];
                        else
                            m = (int)cri_arr[k - 1, k];

                        cri_arr[i, k] = r.Next(m, 9);
                        cri_arr[k, i] = 1 / cri_arr[i, k];

                    }
                // check
                Case _problem = AllclusterdCases.Keys.ElementAt(p);
                List<Case> cases = AllclusterdCases.Values.ElementAt(p);
                MyAHPModel model = new MyAHPModel(count_citeria, cases.Count);
                if (model.CheckConsistency(cri_arr, crlist.Count))
                {
                    AllCriteriaComp.Add(cri_arr);
                    model.AddCriteria(cri_arr);
                    // Choices Comparsions------------------------
                    Dictionary<int, double[,]> AllChoiceComp = CreateChoiceComp(cases);
                    for (int u = 0; u < count_citeria; u++)
                        model.AddCriterionRatedChoices(u, AllChoiceComp[u]);
                    model.CalculatedCriteria();
                    int result = 0;
                    double[] choices = model.CalculatedChoices(out result);
                    int ii = 0;
                    double s = 0, si = 0;
                    double[,] w = new double[cases.Count, 2];
                    foreach (Case c in cases)
                    {

                        EuclideanSimilarity sim = new EuclideanSimilarity();
                        si = sim.Similarity(c, _problem);
                        //  s = System.Math.Round(choices.GetElement(ii, 0) * 100, 0);
                        s = choices[ii] * 100;
                        dataGridView5.Rows.Add();
                        dataGridView5.Rows[count].Cells[0].Value = c.GetFeature("id").GetFeatureValue();
                        dataGridView5.Rows[count].Cells[1].Value = p.ToString();
                        dataGridView5.Rows[count].Cells[2].Value = s.ToString();
                        dataGridView5.Rows[count].Cells[3].Value = si.ToString();
                        w[ii, 0] = Convert.ToDouble(c.GetFeature("id").GetFeatureValue());
                        w[ii, 1] = s;
                        count++;
                        ii++;
                    }
                    AllclusterdResults.Add(_problem, w);

                }
                else goto create;

            }
        }

        private void button19_Click_1(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < dg_randomcases.Rows.Count - 1; i++)
            {
                string s = "";
                for (int j = 1; j < dg_randomcases.Columns.Count-2; j++)
                    s += dg_randomcases.Rows[i].Cells[j].Value.ToString() + ",";
                s += dg_randomcases.Rows[i].Cells[dg_randomcases.Columns.Count - 2].Value.ToString();
                sb.Append(s);
                sb.AppendLine();
            }
            sb.Remove(sb.Length - 1, 1); // Just to avoid copying last empty row
            Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
        }

        private void button22_Click(object sender, EventArgs e)
        {

            richTextBox2.Text = "Without clustering\n";
            //double begin = Convert.ToDouble(txt_first.Text);
            //double end = Convert.ToDouble(txt_end.Text);
            //double first = begin;
            double first = 0.1;
            dg_exp_no_cluster_results.Columns.Clear();
            //  dg_exp_no_cluster_results.Columns.Add("رقم رغبة الزبون", "رقم رغبة الزبون");
            //  dg_exp_no_cluster_results.Columns.Add("عدد الحالات الناتحة", "عدد الحالات الناتحة");
            dg_exp_no_cluster_results.Columns.Add("الحد الأعلى لنسبة التشابه المدخلة من قبل الزبون", "الحد الأعلى لنسبة التشابه المدخلة من قبل الزبون");
            string s = "نسبة تشابه الحل ذو الترتيب ";
            for (int i = 1; i < 8; i++)
                dg_exp_no_cluster_results.Columns.Add(s + i.ToString(), s + i.ToString());
            dg_exp_no_cluster_results.Rows.Add();
            // dg_exp_no_cluster_results.Columns.Add("الوقت المستهلك", "الوقت المستهلك");
            //int j = 0;
            ArrayList myarr = Db.get_cases_as_arraylist_condition(mybestchoice.Tablename_Standardization, mybestchoice.CaseName);
            List<Case> myarrcases = Db.get_all_cases(mybestchoice.Tablename_Standardization, mybestchoice.CaseName);
            //int simcount = 0;


            while (first >= 0.1 && first < 1)
            {
                
  
                int count_time = 0; 
                double time = 0;
                // non clustered cases
                set_xml_sim(first.ToString());
                DefaultEngine _engine = new DefaultEngine();
                string _env = @"E:\D Drive\_SAWSAN MASTER\MobileApplication\Fahp_cbr_app\Fahp_cbr_app\CBR\config.xml";
                List<Case> NonClusterdArrCases;
                AllNonClusterdCases = new Dictionary<Case, List<Case>>();

                for (int f = 0; f < exp_random_cases.Count; f++)
                {
                    // get particular cluster
                    _engine.SetEnvironmentVariable(_env);
                    _engine.SetProblem(exp_random_cases[f]);
                    _engine.Startup();
                    // calculate time for each case
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    ArrayList cases = _engine.Run_caseRetrieval_partial(myarr);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    count_solution = cases.Count;
                    NonClusterdArrCases = new List<Case>();
                    if (count_solution != 0)
                    {
                        count_time++;
                        time += Convert.ToDouble(elapsedMs);
                        Stat ss = null;
                        Case cc = null;
                        for (int i = 0; i < cases.Count; i++)
                        {
                            ss = (Stat)(cases[i]);
                            cc = ss.GetCBRCase();
                            NonClusterdArrCases.Add(cc);
                        }
                        AllNonClusterdCases.Add(exp_random_cases[f], NonClusterdArrCases);

                    } // end if
                } // end for randomcases

                
                 time = time / count_time;
                 richTextBox2.Text = richTextBox2.Text + time.ToString() + "    for sim  " + first.ToString() + "\n";
                 first = first + 0.05;
                


            }// end while sim
                
        }

        private void button23_Click(object sender, EventArgs e)
        {

            //double begin = Convert.ToDouble(txt_first.Text);
            //  double end = Convert.ToDouble(txt_end.Text);
            // double first = begin;
            richTextBox3.Text = "With Clustering \n";
            dg_exp_cluster_results.Columns.Add("الحد الأعلى لنسبة التشابه المدخلة من قبل الزبون", "الحد الأعلى لنسبة التشابه المدخلة من قبل الزبون");
            string s = "نسبة تشابه الحل رقم ";
            for (int i = 1; i < 8; i++)
                dg_exp_cluster_results.Columns.Add(s + i.ToString(), s + i.ToString());
            dg_exp_cluster_results.Rows.Add();
            List<Case> myarrcases = Db.get_all_cases(mybestchoice.Tablename_Standardization, mybestchoice.CaseName);
            // get centroids for case
            double first = 0.1;
            List<Case> centroids = new List<Case>();
            centroids = Db.get_cases_condition_cluster(mybestchoice.Tablename_Standardization, mybestchoice.CaseName, "cluster", "-1");
            Dictionary<int, ArrayList> cases_by_centroids = new Dictionary<int, ArrayList>();
            for (int i = 0; i < centroids.Count; i++)
            {
                int k = Convert.ToInt32(centroids[i].GetFeature("id").GetFeatureValue());
                cases_by_centroids.Add(k, Db.get_cases_condition_cluster_arraylist(mybestchoice.Tablename_Standardization, mybestchoice.CaseName, "cluster", k.ToString()));
            }

            // with Clustering double first = Convert.ToDouble(txt_first.Text);
            /*dg_exp_cluster_results.Columns.Clear();
            dg_exp_cluster_results.Columns.Add("رقم رغبة الزبون", "رقم رغبة الزبون");
            dg_exp_cluster_results.Columns.Add("عدد الحالات الناتحة", "عدد الحالات الناتحة");
            dg_exp_cluster_results.Columns.Add("نسبة التشابه", "نسبة التشابه");
            dg_exp_cluster_results.Columns.Add("الوقت المستهلك", "الوقت المستهلك");
            //int jj = 0;*/
            //List<double> times = new List<double>();

            while (first >= 0.1 && first < 1)
            {
                //double avgtime = 0;
                //int c = 0;
                // repeat 100 times for each experiment
                //for (int uu = 0; uu < 1; uu++)
                //{
                
                double time = 0;
                double alltime = 0;
                int count_time = 0;
                
                AllclusterdCases = new Dictionary<Case, List<Case>>();
                List<Case> ClusterdArrCases;
                // know which cluster
                Kmeans kmeans = new Kmeans();
                // experiment is 100  random cases
                for (int u = 0; u < exp_random_cases.Count; u++)
                {
                    ClusterdArrCases = new List<Case>();
                     var watch = System.Diagnostics.Stopwatch.StartNew();
                     int t = kmeans.findCluster_customize(exp_random_cases[u], centroids);
                     watch.Stop();
                     var elapsedMs = watch.ElapsedMilliseconds;
                     time = Convert.ToDouble(elapsedMs);
                    // get particular cluster
                    current_cases_partial = cases_by_centroids[t];
                    set_xml_sim(first.ToString());
                    DefaultEngine _engine = new DefaultEngine();
                    string _env = @"E:\D Drive\_SAWSAN MASTER\MobileApplication\Fahp_cbr_app\Fahp_cbr_app\CBR\config.xml";
                    _engine.SetEnvironmentVariable(_env);
                    _engine.SetProblem(exp_random_cases[u]);
                    // Run the reasoning engine
                    _engine.Startup();
                     watch = System.Diagnostics.Stopwatch.StartNew();
                    ArrayList mcases = _engine.Run_caseRetrieval_partial(current_cases_partial);
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    time += Convert.ToDouble(elapsedMs);
                    count_solution = mcases.Count;
                    if (count_solution != 0)
                    {
                        Stat ss = null;
                        Case cc = null;
                        count_time++;
                        alltime = alltime + time;
                        for (int i = 0; i < mcases.Count; i++)
                        {
                            ss = (Stat)(mcases[i]);
                            cc = ss.GetCBRCase();
                            ClusterdArrCases.Add(cc);
                        }
                        AllclusterdCases.Add(exp_random_cases[u], ClusterdArrCases);

                    }
                }
                alltime /= count_time;
                richTextBox3.Text = richTextBox3.Text + alltime.ToString() + "\t sim is" + first.ToString() + "\n";
                first = first + 0.05;




            }// end while sim
        }
    }
}