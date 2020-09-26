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
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Data.SqlClient;
namespace Fahp_cbr_app
{

    public partial class frm_results : Form
    {
        
        public static bool ObjCreated = false;
        SqlConnection conn = new SqlConnection(@" Data Source=.;Initial Catalog=PhoneDB;Integrated Security=True");
        
        double finalresutnon = 0;
        double finalresutwith = 0;
      //  int count_solution = 0;
       // int count_citeria = 0;
        Dictionary<Case, List<Case>> AllNonClusterdCases;
        Dictionary<Case, List<Case>> AllclusterdCases;
        Dictionary<Case, double[,]> AllNonClusterdResults;
        Dictionary<Case, double[,]> AllclusterdResults;
        List< double[,]> AllNonClusterdResults_;
        List<double[,]> AllclusterdResults_;
        List<double[,]> AllCriteriaComp;
        
        BestChoice mybestchoice;
        Statistics statistics;
       
        public frm_results()
        {
            InitializeComponent();
        }

        private void frm_results_Load(object sender, EventArgs e)
        {

        }


   private double[,] Compar()
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


            
            int ccc = 0;
            finalresutnon = 0;
            finalresutwith = 0;
       for (int i = 0; i < AllNonClusterdResults.Count; i++)
           if (AllNonClusterdResults.Values.ElementAt(i).GetLength(0)>0)
           { 
            int id = Convert.ToInt32(AllNonClusterdResults.Values.ElementAt(i)[0, 0]);//first sol
            EuclideanSimilarity sim = new EuclideanSimilarity();
            // similarity in last col
            finalresutnon += sim.Similarity(AllNonClusterdResults.Keys.ElementAt(i), mybestchoice.Cases[id]);
            ccc++;
           }
       finalresutnon /= ccc;
       ccc = 0;

       for (int i = 0; i < AllclusterdResults.Count; i++)
           if (AllclusterdResults.Values.ElementAt(i).GetLength(0) > 0)
           {
               int id = Convert.ToInt32(AllclusterdResults.Values.ElementAt(i)[0, 0]);//first sol
               EuclideanSimilarity sim = new EuclideanSimilarity();
               // similarity in last col
               finalresutwith += sim.Similarity(AllclusterdResults.Keys.ElementAt(i), mybestchoice.Cases[id]);
               ccc++;
           }
       finalresutwith /= ccc;

            //  take the the smaller dim to compare the tow lists 
            int mind = 0;
            if (AllNonClusterdResults.Count < AllclusterdResults.Count)
                mind = AllNonClusterdResults.Count;
            else
                mind = AllclusterdResults.Count;


            double[,] finalresults = new double[mind, 8];
          //  int countsim = 0;
            int count = 0;
            int c = 0;
            for (int i = 0; i < AllNonClusterdResults.Count; i++)
                for (int k = count; k < AllclusterdResults.Count; k++)
                 if (AllNonClusterdResults.Keys.ElementAt(i) == AllclusterdResults.Keys.ElementAt(k))
                 {
                    finalresults[c, 0] = Convert.ToDouble(AllNonClusterdResults.Keys.ElementAt(i).GetFeature("id").GetFeatureValue());
                    int dim = 0;
                    if ((AllNonClusterdResults.Values.ElementAt(i).Length == 2) || (AllclusterdResults.Values.ElementAt(k).Length == 2))
                      dim = 1;
                    else
                     if (AllNonClusterdResults.Values.ElementAt(i).GetLength(0) < AllclusterdResults.Values.ElementAt(k).GetLength(0))
                        dim = AllNonClusterdResults.Values.ElementAt(i).GetLength(0);
                     else
                        dim = AllclusterdResults.Values.ElementAt(k).GetLength(0);
                    if (dim > 7) dim = 7;
                     for (int j = 0; j < dim; j++) // first 7 solutions
                         if (AllNonClusterdResults.Values.ElementAt(i)[j, 0] == AllclusterdResults.Values.ElementAt(k)[j, 0])
                         { 
                             finalresults[c, j + 1]++;
                           
                             
                         }
                     //if (finalresults[c, 1] != 0) countsim++;
                     count = k+1;
                     c++;
                     break;
            }
            return finalresults;
        }

        void get_solutions_without_Clustering(double sim)
        {
            ArrayList myarr = Db.get_cases_as_arraylist_condition(mybestchoice.Tablename_Standardization, mybestchoice.CaseName);
            
          
                // non clustered cases

                mybestchoice.set_xml_sim(sim.ToString());
                DefaultEngine _engine = new DefaultEngine();
                string _env = @"E:\D Drive\_SAWSAN MASTER\MobileApplication\Fahp_cbr_app\Fahp_cbr_app\CBR\config.xml";
                List<Case> NonClusterdArrCases;
                AllNonClusterdCases = new Dictionary<Case, List<Case>>();

                for (int f = 0; f < statistics.exp_random_cases.Count; f++)
                {
                    // get particular cluster
                    _engine.SetEnvironmentVariable(_env);
                    _engine.SetProblem(statistics.exp_random_cases[f]);
                    _engine.Startup();
                    // calculate time for each case
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    ArrayList cases = _engine.Run_caseRetrieval_partial(myarr);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    int count_solution = cases.Count;
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
                        AllNonClusterdCases.Add(statistics.exp_random_cases[f], NonClusterdArrCases);
                    }
                }
        }


        void get_solutions_with_Clustering(double sim)
        {

            ArrayList current_cases_partial = new ArrayList();
            // get centroids for case

            List<Case> centroids = new List<Case>();
            centroids = Db.get_cases_condition_cluster(mybestchoice.Tablename_Standardization, mybestchoice.CaseName, "cluster", "-1");
            Dictionary<int, ArrayList> cases_by_centroids = new Dictionary<int, ArrayList>();
            for (int i = 0; i < centroids.Count; i++)
            {
                int k = Convert.ToInt32(centroids[i].GetFeature("id").GetFeatureValue());
                cases_by_centroids.Add(k, Db.get_cases_condition_cluster_arraylist(mybestchoice.Tablename_Standardization, mybestchoice.CaseName, "cluster", k.ToString()));
            }

                AllclusterdCases = new Dictionary<Case, List<Case>>();
                List<Case> ClusterdArrCases;
                // know which cluster
                Kmeans kmeans = new Kmeans();
                // experiment is 100  random cases
                for (int u = 0; u < statistics.exp_random_cases.Count; u++)
                {
                    ClusterdArrCases = new List<Case>();
                    int t = kmeans.findCluster_customize(statistics.exp_random_cases[u], centroids);
                    current_cases_partial = cases_by_centroids[t];
                    mybestchoice.set_xml_sim(sim.ToString());
                    DefaultEngine _engine = new DefaultEngine();
                    string _env = @"E:\D Drive\_SAWSAN MASTER\MobileApplication\Fahp_cbr_app\Fahp_cbr_app\CBR\config.xml";
                    _engine.SetEnvironmentVariable(_env);
                    _engine.SetProblem(statistics.exp_random_cases[u]);
                    // Run the reasoning engine
                    _engine.Startup();
                    ArrayList mcases = _engine.Run_caseRetrieval_partial(current_cases_partial);
                    int count_solution = mcases.Count;

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
                        AllclusterdCases.Add(statistics.exp_random_cases[u], ClusterdArrCases);

                    }


                }
 
        }


        void get_comparison_withoutclustering()
        {
            AllCriteriaComp = new List<double[,]>();
            AllNonClusterdResults = new Dictionary<Case, double[,]>();
            List<string> crlist = new List<string>();
            crlist = Db.get_criteria(mybestchoice.CaseName);
            int count_citeria = crlist.Count;


            // Criteria Comparsions------------------------
            for (int p = 0; p < AllNonClusterdCases.Count; p++)
            {
            create:
                double[,] cri_arr = new double[crlist.Count, crlist.Count];
           
                // check
                Case _problem = AllNonClusterdCases.Keys.ElementAt(p);
                List<Case> cases = AllNonClusterdCases.Values.ElementAt(p);
                MyAHPModel model = new MyAHPModel(count_citeria, cases.Count);
                cri_arr=model.Create_Criteria_Comparison_Array();
                
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

        void get_comparison_with_clustering()
        {
            AllclusterdResults = new Dictionary<Case, double[,]>();
            List<string> crlist = new  List<string>();
            crlist = Db.get_criteria(mybestchoice.CaseName);
            int count_citeria = crlist.Count;
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
                        count++;
                        ii++;
                    }
                    AllclusterdResults.Add(_problem, w);

                }
                else goto create;

            }
        }



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

       



        void get_comparison_withoutclustering_one_by_one()
        {
            AllCriteriaComp = new List<double[,]>();
            AllNonClusterdResults = new Dictionary<Case, double[,]>();
            List<string> crlist = new List<string>();
            crlist = Db.get_criteria(mybestchoice.CaseName);
            int count_citeria = crlist.Count;

            
            // Criteria Comparsions------------------------
            for (int p = 0; p < AllNonClusterdCases.Count; p++)
            {
                Case _problem = AllNonClusterdCases.Keys.ElementAt(p);
                List<Case> cases = AllNonClusterdCases.Values.ElementAt(p);
                MyAHPModel model = new MyAHPModel(count_citeria, cases.Count);
                double[,] cri_arr=model.Create_Criteria_Comparison_Array();
                AllCriteriaComp.Add(cri_arr);
                model.AddCriteria(cri_arr);
                model.CalculatedCriteria();
                // Choices Comparsions------------------------
                bool consistency=false;
                   double [] choices = new double[ cases.Count];
                double [] tempresult = new double[ cases.Count];
                for (int u = 0; u < count_citeria; u++)
                {
                    double [,] comp_choice=model.CreateOne_CeiteriaChoiceComp(u,cases);
                    tempresult = model.Calculated_One__Choic(out consistency, model.CriteriaWeights[u], comp_choice);
                    if (!consistency) {MessageBox.Show("حالة عدم ثبات في الحلول"+p.ToString()+" no clustering "); return;}
                    for (int h = 0; h < tempresult.GetLength(0); h++)
                        choices[h] +=tempresult[h];
                }
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
                    AllNonClusterdResults.Add(_problem, w);
            }// end for
        } // end procedure





     void get_comparison_withclustering_one_by_one()
     {
         AllCriteriaComp = new List<double[,]>();
         AllclusterdResults = new Dictionary<Case, double[,]>();
         List<string> crlist = new List<string>();
         crlist = Db.get_criteria(mybestchoice.CaseName);
         int count_citeria = crlist.Count;


         // Criteria Comparsions------------------------
         for (int p = 0; p < AllclusterdCases.Count; p++)
         {
             Case _problem = AllclusterdCases.Keys.ElementAt(p);
             List<Case> cases = AllclusterdCases.Values.ElementAt(p);
             MyAHPModel model = new MyAHPModel(count_citeria, cases.Count);
             double[,] cri_arr = model.Create_Criteria_Comparison_Array();
             AllCriteriaComp.Add(cri_arr);
             model.AddCriteria(cri_arr);
             // Choices Comparsions------------------------
             bool consistency = false;
             double[] choices = new double[cases.Count];
             double[] tempresult = new double[cases.Count];
             for (int u = 0; u < count_citeria; u++)
             {
                 model.CalculatedCriteria();
                 double[,] comp_choice = model.CreateOne_CeiteriaChoiceComp(u, cases);
                 tempresult = model.Calculated_One__Choic(out consistency, model.CriteriaWeights[u], comp_choice);
                 if (!consistency) { MessageBox.Show("حالة عدم ثبات في الحلول" + p.ToString() + "  clustering "); return; }
                 for (int h = 0; h < tempresult.GetLength(0); h++)
                     choices[h] += tempresult[h];
             }
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
         }// end for
     }



 void get_comparison_one_by_one()
 {
     AllCriteriaComp = new List<double[,]>();
     AllNonClusterdResults_ = new List<double[,]>();
     AllclusterdResults_ = new List< double[,]>();
     List<string> crlist = new List<string>();
     crlist = Db.get_criteria(mybestchoice.CaseName);
     int count_citeria = crlist.Count;
     //  يجب وضع نفس مصفوفة مقارنة المعايير للحالات نفسها
     // ثم وضع مصفوفة معايير اخرى للبقية





     for (int t = 0; t < statistics.exp_random_cases.Count; t++)
         {
             Case _problem = statistics.exp_random_cases[t];
                 if (AllNonClusterdCases.Values.ElementAt(t) != null && AllclusterdCases.Values.ElementAt(t) != null)
                 {
                     // Criteria Comparsions------------------------
                     MyAHPModel model = new MyAHPModel();
                     double[,] cri_arr = model.Create_Criteria_Comparison_Array(count_citeria);
                     // non clustering 
                     List<Case> cases1 = AllNonClusterdCases.Values.ElementAt(t);
                     MyAHPModel model1 = new MyAHPModel(count_citeria, cases1.Count);
                     AllCriteriaComp.Add(cri_arr);
                     model1.AddCriteria(cri_arr);
                     model1.CalculatedCriteria();
                     // Choices Comparsions------------------------
                     bool consistency = false;
                     double[] choices = new double[cases1.Count];
                     double[] tempresult = new double[cases1.Count];
                     for (int u = 0; u < count_citeria; u++)
                     {
                         double[,] comp_choice = model1.CreateOne_CeiteriaChoiceComp(u, cases1);
                         tempresult = model1.Calculated_One__Choic(out consistency, model1.CriteriaWeights[u], comp_choice);
                         if (!consistency) { MessageBox.Show("حالة عدم ثبات في الحلول" + t.ToString() + " no clustering "); return; }
                         for (int h = 0; h < tempresult.GetLength(0); h++)
                             choices[h] += tempresult[h];
                     }
                     int ii = 0;
                     double[,] w = new double[cases1.Count, 2];
                     foreach (Case c in cases1)
                     {
                         w[ii, 0] = Convert.ToDouble(c.GetFeature("id").GetFeatureValue());
                         w[ii, 1] = choices[ii] * 100;
                         ii++;
                     }
                     AllNonClusterdResults_.Add(w);
                     // Clustering
                     List<Case> cases2 = AllclusterdCases.Values.ElementAt(t);
                     MyAHPModel model2 = new MyAHPModel(count_citeria, cases2.Count);
                     AllCriteriaComp.Add(cri_arr);
                     model2.AddCriteria(cri_arr);
                     // Choices Comparsions------------------------
                     consistency = false;
                     choices = new double[cases2.Count];
                     tempresult = new double[cases2.Count];
                     for (int u = 0; u < count_citeria; u++)
                     {
                         model2.CalculatedCriteria();
                         double[,] comp_choice = model2.CreateOne_CeiteriaChoiceComp(u, cases2);
                         tempresult = model2.Calculated_One__Choic(out consistency, model2.CriteriaWeights[u], comp_choice);
                         if (!consistency) { MessageBox.Show("حالة عدم ثبات في الحلول" + t.ToString() + "  clustering "); return; }
                         for (int h = 0; h < tempresult.GetLength(0); h++)
                             choices[h] += tempresult[h];
                     }
                     ii = 0;
                     w = new double[cases2.Count, 2];
                     foreach (Case c in cases2)
                     {
                         w[ii, 0] = Convert.ToDouble(c.GetFeature("id").GetFeatureValue());
                         w[ii, 1] = choices[ii] * 100;
                         ii++;
                     }
                     AllclusterdResults_.Add( w);
                 } // end if if
             else
              if (AllNonClusterdCases.Values.ElementAt(t) != null && AllclusterdCases.Values.ElementAt(t) == null)
             {
                 
                 // Criteria Comparsions------------------------
                 List<Case> cases1 = AllNonClusterdCases.Values.ElementAt(t);
                 MyAHPModel model1 = new MyAHPModel(count_citeria, cases1.Count);
                 double[,] cri_arr1 = model1.Create_Criteria_Comparison_Array();
                 AllCriteriaComp.Add(cri_arr1);
                 model1.AddCriteria(cri_arr1);
                 model1.CalculatedCriteria();
                 // Choices Comparsions------------------------
                 bool consistency = false;
                 double[] choices = new double[cases1.Count];
                 double[] tempresult = new double[cases1.Count];
                 for (int u = 0; u < count_citeria; u++)
                 {
                     double[,] comp_choice = model1.CreateOne_CeiteriaChoiceComp(u, cases1);
                     tempresult = model1.Calculated_One__Choic(out consistency, model1.CriteriaWeights[u], comp_choice);
                     if (!consistency) { MessageBox.Show("حالة عدم ثبات في الحلول" + t.ToString() + " no clustering "); return; }
                     for (int h = 0; h < tempresult.GetLength(0); h++)
                         choices[h] += tempresult[h];
                 }
                 int ii = 0;
                 double[,] w = new double[cases1.Count, 2];
                 foreach (Case c in cases1)
                 {
                     w[ii, 0] = Convert.ToDouble(c.GetFeature("id").GetFeatureValue());
                     w[ii, 1] = choices[ii] * 100;
                     ii++;
                 }
                 AllNonClusterdResults_.Add( w);
                 AllclusterdResults_.Add(null);
             } // end if
             // Clustering
             else
              if (AllNonClusterdCases.Values.ElementAt(t) == null && AllclusterdCases.Values.ElementAt(t) != null)
              { 
                 // Criteria Comparsions------------------------
                 List<Case> cases2 = AllclusterdCases.Values.ElementAt(t);
                 MyAHPModel model2 = new MyAHPModel(count_citeria, cases2.Count);
                 double[,] cri_arr2 = model2.Create_Criteria_Comparison_Array();
                 AllCriteriaComp.Add(cri_arr2);
                 model2.AddCriteria(cri_arr2);
                 // Choices Comparsions------------------------
                 bool consistency = false;
                 double[] choices = new double[cases2.Count];
                 double[] tempresult = new double[cases2.Count];
                 for (int u = 0; u < count_citeria; u++)
                 {
                     model2.CalculatedCriteria();
                     double[,] comp_choice = model2.CreateOne_CeiteriaChoiceComp(u, cases2);
                     tempresult = model2.Calculated_One__Choic(out consistency, model2.CriteriaWeights[u], comp_choice);
                     if (!consistency) { MessageBox.Show("حالة عدم ثبات في الحلول" + t.ToString() + "  clustering "); return; }
                     for (int h = 0; h < tempresult.GetLength(0); h++)
                         choices[h] += tempresult[h];
                 }
                 int ii = 0;
                 double[,] w = new double[cases2.Count, 2];
                 foreach (Case c in cases2)
                 {
                     w[ii, 0] = Convert.ToDouble(c.GetFeature("id").GetFeatureValue());
                     w[ii, 1] = choices[ii] * 100;
                     ii++;
                 }
                 AllclusterdResults_.Add( w);
                 AllNonClusterdResults_.Add(null);
             } // end if
              else
              {
                  AllclusterdResults_.Add(null);
                  AllNonClusterdResults_.Add(null);
              }
                 
         } // end for
         

 }// end procedure



 void get_solutions(double sim)
 {
     List<Case> ClusterdArrCases;
     List<Case> NonClusterdArrCases;
     AllclusterdCases = new Dictionary<Case, List<Case>>();
     AllNonClusterdCases = new Dictionary<Case, List<Case>>();
     ArrayList current_cases_partial = new ArrayList();
     List<Case> centroids = new List<Case>();
     Kmeans kmeans = new Kmeans();
     DefaultEngine _engine;
     string _env = @"E:\D Drive\_SAWSAN MASTER\MobileApplication\Fahp_cbr_app\Fahp_cbr_app\CBR\config.xml";

     // get all cases
     ArrayList myarr = Db.get_cases_as_arraylist_condition(mybestchoice.Tablename_Standardization, mybestchoice.CaseName);

     // get centroids for case
     centroids = Db.get_cases_condition_cluster(mybestchoice.Tablename_Standardization, mybestchoice.CaseName, "cluster", "-1");
     Dictionary<int, ArrayList> cases_by_centroids = new Dictionary<int, ArrayList>();
     for (int i = 0; i < centroids.Count; i++)
     {
         int k = Convert.ToInt32(centroids[i].GetFeature("id").GetFeatureValue());
         cases_by_centroids.Add(k, Db.get_cases_condition_cluster_arraylist(mybestchoice.Tablename_Standardization, mybestchoice.CaseName, "cluster", k.ToString()));
     }


   

     // set similarity
     mybestchoice.set_xml_sim(sim.ToString());
     // get solutions
     for (int u = 0; u < statistics.exp_random_cases.Count; u++)
     {

         // Without Clustering
         
         _engine = new DefaultEngine();
         _engine.SetEnvironmentVariable(_env);
         _engine.SetProblem(statistics.exp_random_cases[u]);
         _engine.Startup();
         // calculate time for each case
         var watch = System.Diagnostics.Stopwatch.StartNew();
         ArrayList cases = _engine.Run_caseRetrieval_partial(myarr);
         watch.Stop();
         var elapsedMs = watch.ElapsedMilliseconds;
         int count_solution_non = cases.Count;
         NonClusterdArrCases = new List<Case>();
         if (count_solution_non != 0)
         {

             Stat ss = null;
             Case cc = null;
             for (int i = 0; i < cases.Count; i++)
             {
                 ss = (Stat)(cases[i]);
                 cc = ss.GetCBRCase();
                 NonClusterdArrCases.Add(cc);
             }
             AllNonClusterdCases.Add(statistics.exp_random_cases[u], NonClusterdArrCases);
         }
         else
             AllNonClusterdCases.Add(statistics.exp_random_cases[u], null);
         // With Clustering
         ClusterdArrCases = new List<Case>();
         // get particular cluster
         int t = kmeans.findCluster_customize(statistics.exp_random_cases[u], centroids);
         current_cases_partial = cases_by_centroids[t];
         // get solutions
         _engine = new DefaultEngine();
         _engine.SetEnvironmentVariable(_env);
         _engine.SetProblem(statistics.exp_random_cases[u]);
         // Run the reasoning engine
         _engine.Startup();
         ArrayList mcases = _engine.Run_caseRetrieval_partial(current_cases_partial);
         int count_solution_with = mcases.Count;

         if (count_solution_with != 0)
         {
             Stat ss = null;
             Case cc = null;
             for (int i = 0; i < mcases.Count; i++)
             {
                 ss = (Stat)(mcases[i]);
                 cc = ss.GetCBRCase();
                 ClusterdArrCases.Add(cc);
             }
             AllclusterdCases.Add(statistics.exp_random_cases[u], ClusterdArrCases);
         }
         else
             AllclusterdCases.Add(statistics.exp_random_cases[u], null);


     } // end for
 }

 private double[,] Compar_()
 {


     for (int h = 0; h < statistics.exp_random_cases.Count; h++)
     { 
       if (AllclusterdResults_[h] != null)  // sort Clustered
             for (int i = 0; i < AllclusterdResults_[h].GetLength(0) - 1; i++)
                 for (int j = 0; j < AllclusterdResults_[h].GetLength(0) - i - 1; j++)
                     if (AllclusterdResults_[h][j, 1] < AllclusterdResults_[h][j + 1, 1])
                     {
                         double temp = AllclusterdResults_[h][j, 1];
                         AllclusterdResults_[h][j, 1] = AllclusterdResults_[h][j + 1, 1];
                         AllclusterdResults_[h][j + 1, 1] = temp;
                         temp = AllclusterdResults_[h][j, 0];
                         AllclusterdResults_[h][j, 0] = AllclusterdResults_[h][j + 1, 0];
                         AllclusterdResults_[h][j + 1, 0] = temp;
                     }

       if (AllNonClusterdResults_[h] != null)  // soting Without Clustering
           for (int i = 0; i < AllNonClusterdResults_[h].GetLength(0) - 1; i++)
               for (int j = 0; j < AllNonClusterdResults_[h].GetLength(0) - i - 1; j++)
                   if (AllNonClusterdResults_[h][j, 1] < AllNonClusterdResults_[h][j + 1, 1])
                   {
                       double temp = AllNonClusterdResults_[h][j, 1];
                       AllNonClusterdResults_[h][j, 1] = AllNonClusterdResults_[h][j + 1, 1];
                       AllNonClusterdResults_[h][j + 1, 1] = temp;
                       temp = AllNonClusterdResults_[h][j, 0];
                       AllNonClusterdResults_[h][j, 0] = AllNonClusterdResults_[h][j + 1, 0];
                       AllNonClusterdResults_[h][j + 1, 0] = temp;
                   }
     }



     int ccc1 = 0, ccc2 = 0;
     finalresutnon = 0;
     finalresutwith = 0;
     for (int i = 0; i < statistics.exp_random_cases.Count; i++)
     {
         if (AllNonClusterdResults_[i] != null)
         {
             int id = Convert.ToInt32(AllNonClusterdResults_[i][0, 0]);//first sol
             EuclideanSimilarity sim = new EuclideanSimilarity();
             // similarity in last col
             finalresutnon += sim.Similarity(statistics.exp_random_cases[i], mybestchoice.Cases[id]);
             ccc1++;
         }

         if (AllclusterdResults_[i]!= null)
         {
             int id = Convert.ToInt32(AllclusterdResults_[i][0, 0]);//first sol
             EuclideanSimilarity sim = new EuclideanSimilarity();
             // similarity in last col
             finalresutwith += sim.Similarity(statistics.exp_random_cases[i], mybestchoice.Cases[id]);
             ccc2++;
         }
     }
     finalresutnon /= ccc1;
     finalresutwith /= ccc2;

     double[,] finalresults = new double[statistics.exp_random_cases.Count, 7];

     for (int i = 0; i < statistics.exp_random_cases.Count; i++)
             if (AllNonClusterdResults_[i]!=null && AllclusterdResults_[i]!=null )
             {
                 int dim = 0;
                 if ((AllNonClusterdResults_[i].Length == 2) || (AllclusterdResults_[i].Length == 2))
                     dim = 1;
                 else
                     if (AllNonClusterdResults_[i].GetLength(0) < AllclusterdResults_[i].GetLength(0))
                         dim = AllNonClusterdResults_[i].GetLength(0);
                     else
                         dim = AllclusterdResults_[i].GetLength(0);
                 if (dim > 7) dim = 7;
                 for (int j = 0; j < dim; j++) // first 7 solutions
                     if (AllNonClusterdResults_[i][j, 0] == AllclusterdResults_[i][j, 0])
                         finalresults[i, j] = 1; // similarity
             }
     return finalresults;
 }





 


 private void label6_Click(object sender, EventArgs e)
 {

 }

 private void button18_Click_1(object sender, EventArgs e)
 {
     
     implement_ahp();
     calc_matching();
  
          
 }

 private void label2_Click(object sender, EventArgs e)
 {

 }

 private void lbl_non_Click(object sender, EventArgs e)
 {

 }



public void implement_ahp()
 {
    
     int dG_WithR = 0;
     int dG_NonR = 0;
     int DG_cri = 0;
     // display
     dg_With_stat.Columns.Clear();
     dg_With_stat.Columns.Add("Porblem No  ", "Porblem No  ");
     dg_With_stat.Columns.Add("Sim  ", "Sim  ");
     dg_With_stat.Columns.Add("SolNum", "Sol num ");
     dg_With_stat.Columns.Add("Sol sim with problem  ", "Sol sim with problem  ");

     dg_Non_stat.Columns.Clear();
     dg_Non_stat.Columns.Add("Porblem No", "Porblem No  ");
     dg_Non_stat.Columns.Add("Sim  ", "Sim  ");
     dg_Non_stat.Columns.Add("SolNum", "Sol Num  ");
     dg_Non_stat.Columns.Add("Sol sim with problem  ", "Sol sim with problem  ");

     dg_criteria.Columns.Add("Porblem", "Porblem No");
     for (int i = 1; i < statistics.exp_random_cases[0].GetFeatures().Count - 1; i++)
     {
         Feature f = (Feature)statistics.exp_random_cases[0].GetFeatures()[i];
         dg_criteria.Columns.Add(f.GetFeatureName(), f.GetFeatureName());

     }
     // call method
     statistics.FindSolutions_AHP();

     //display
     listBox1.Items.Add("خطوة2: إيجاد حلول لكل حالة دراسة");
     listBox1.Items.Add("ايجاد حلول بدون عنقدة");
     listBox1.Items.Add("IK-means++ ايجاد حلول مع عنقدة");
     listBox1.Items.Add("AHP خطوة 3: تطبيق");
     listBox1.Items.Add("خطوة 1.3 توليد المقارنات الزوجية للمعاير وللبدائل");
     listBox1.Items.Add("ولد المقارنات بدون عنقدة");
     listBox1.Items.Add(" IK-means++ ولد المقارنات مع عنقدة");

     // display

     foreach (Case c in statistics.exp_random_cases)
     {
         foreach (KeyValuePair<double, double[,]> pair in statistics.AllNonClusterdResults[c])
         {

             dg_Non_stat.Rows.Add();
             dg_Non_stat.Rows[dG_NonR].Cells[0].Value = c.GetFeature("id").GetFeatureValue().ToString();
             dg_Non_stat.Rows[dG_NonR].Cells[1].Value = pair.Key.ToString();//sim
             dg_Non_stat.Rows[dG_NonR].Cells[2].Value = pair.Value[0, 0];// first sol
             string sol_id = pair.Value[0, 0].ToString();
             EuclideanSimilarity s = new EuclideanSimilarity();
             dg_Non_stat.Rows[dG_NonR].Cells[3].Value = Math.Round(s.Similarity(c, Db.get_Case(statistics.casename, statistics.standrize_table, sol_id)) * 100, 2).ToString();

             dG_NonR++;
         }
         foreach (KeyValuePair<double, double[,]> pair in statistics.AllClusterdResults[c])
         {

             dg_With_stat.Rows.Add();
             dg_With_stat.Rows[dG_WithR].Cells[0].Value = c.GetFeature("id").GetFeatureValue().ToString();
             dg_With_stat.Rows[dG_WithR].Cells[1].Value = pair.Key.ToString();//sim
             dg_With_stat.Rows[dG_WithR].Cells[2].Value = pair.Value[0, 0]; // first sol
             string sol_id = pair.Value[0, 0].ToString();
             EuclideanSimilarity s = new EuclideanSimilarity();
             dg_With_stat.Rows[dG_WithR].Cells[3].Value = Math.Round(s.Similarity(c, Db.get_Case(statistics.casename, statistics.standrize_table, sol_id)) * 100, 2).ToString();
             dG_WithR++;
         }
        
         //criteria
         dg_criteria.Rows.Add();
         dg_criteria.Rows[DG_cri].Cells[0].Value = c.GetFeature("id").GetFeatureValue().ToString();
         for (int i = 0; i < statistics.exp_random_cases[0].GetFeatures().Count - 2; i++)
         {
             dg_criteria.Rows[DG_cri].Cells[i + 1].Value = Math.Round(statistics.criteria_weights[c][i] * 100, 2).ToString() + "%";
         }
         DG_cri++;

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

    // display
    dg_result.Columns.Clear();
    dg_result.Columns.Add("Sim  ", "Sim  ");
    dg_result.Columns.Add("exactly  ", "exactly  ");
    Dictionary<double, int[]> results = new Dictionary<double, int[]>();
    double[,] wpair;
    int[] temp;
    //---------------------------------------------------------------------------------

    // display
    foreach (Case c in statistics.exp_random_cases)
        foreach (KeyValuePair<double, double[,]> npair in statistics.AllNonClusterdResults[c])
            if (statistics.AllClusterdResults[c].ContainsKey(npair.Key))
            {
                wpair = statistics.AllClusterdResults[c][npair.Key];
                if (npair.Value[0, 0] == wpair[0, 0])
                {
                    if (!results.ContainsKey(npair.Key))
                    {
                        temp = new int[2];
                        temp[0] = 1;
                        temp[1] = 1; // 
                        results.Add(npair.Key, temp);
                    }
                    else
                    {
                        results[npair.Key][1] += 1;// عدد مرات وجود نسبة التشابه
                        results[npair.Key][0] += 1;// تطابق الحلين
                    }
                }
                else
                    if (results.ContainsKey(npair.Key))
                    {
                        results[npair.Key][1] += 1; //  عدد مرات وجود نسبة التشابه
                    }
                    else if (!results.ContainsKey(npair.Key))
                    {
                        temp = new int[2];
                        temp[0] = 0;
                        temp[1] = 1;
                        results.Add(npair.Key, temp);
                    }
            } //if
    int g = 0;
    foreach (KeyValuePair<double, int[]> pair in results)
    {
        dg_result.Rows.Add();
        dg_result.Rows[g].Cells[0].Value = pair.Key.ToString();
        dg_result.Rows[g].Cells[1].Value = ((Convert.ToDouble(pair.Value[0]) / Convert.ToDouble(pair.Value[1])) * 100).ToString() + " %";
        g++;
    }


    dg_result.AutoResizeColumns();
    dg_result.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
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

 private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
 {
     if (comboBox2.Text==comboBox2.Items[0].ToString())
     {
         dg_Non_stat.Sort(dg_Non_stat.Columns[0],ListSortDirection.Ascending);
         dg_With_stat.Sort(dg_With_stat.Columns[0], ListSortDirection.Ascending);
     }
     else if (comboBox2.Text==comboBox2.Items[1].ToString())
     {
         dg_Non_stat.Sort(dg_Non_stat.Columns[1], ListSortDirection.Ascending);
         dg_With_stat.Sort(dg_With_stat.Columns[1], ListSortDirection.Ascending);
     }
 }

 private void button1_Click(object sender, EventArgs e)
 {
     List<int[]> narr = new List<int[]>();
     List<int[]> warr = new List<int[]>();
     frm_details fd = new frm_details();



     fd.dg_With_stat.Columns.Clear();
     fd.dg_With_stat.Rows.Clear();

     int[] w;
     double sim;
     int dG_WithR = 0;
     fd.dg_With_stat.Columns.Add("Sim", "Sim  ");
     fd.dg_With_stat.Columns.Add("Num  ", "Num  ");
     int count = Db.Get_RowsCount(statistics.tablename);
     for (int i = 0; i < count; i++)
     {
         fd.dg_With_stat.Columns.Add("Sol num " + (i + 1).ToString(), "Sol num " + (i + 1).ToString());
     }
     List<Case> ClusterdArrCases;
     for (int u = 0; u < statistics.exp_random_cases.Count; u++)
     {
         sim = Math.Round(0.10, 2);
         w = new int[18];
         Case _problem = statistics.exp_random_cases[u];
         // Criteria Comparsions------------------------
         int c = 0;
         while (sim < 1)
         {
             ClusterdArrCases = new List<Case>();

             ClusterdArrCases = statistics.FindAlternative_WithClustering(_problem, sim.ToString());

             fd.dg_With_stat.Rows.Add();
             fd.dg_With_stat.Rows[dG_WithR].Cells[1].Value = _problem.GetFeature("id").GetFeatureValue().ToString();
             fd.dg_With_stat.Rows[dG_WithR].Cells[0].Value = sim.ToString();
             if (ClusterdArrCases.Count > 0)

                 for (int i = 0; i < ClusterdArrCases.Count; i++)
                 {
                     fd.dg_With_stat.Rows[dG_WithR].Cells[i + 2].Value = ClusterdArrCases[i].GetFeature("id").GetFeatureValue().ToString();
                 }
             dG_WithR++;
             w[c] = ClusterdArrCases.Count;
             c++;
             sim += Math.Round(0.05, 2);
             sim = Math.Round(sim, 2);
         }
         warr.Add(w);
     }






     fd.dg_Non_stat.Columns.Clear();
     fd.dg_Non_stat.Rows.Clear();
     int[] n;
     sim = 0;
     int dG_NonR = 0;
     fd.dg_Non_stat.Columns.Add("Sim", "Sim  ");
     fd.dg_Non_stat.Columns.Add("Num  ", "Num  ");
     count = Db.Get_RowsCount(statistics.tablename);

     for (int i = 0; i < count; i++)
     {
         fd.dg_Non_stat.Columns.Add("Sol num " + (i + 1).ToString(), "Sol num " + (i + 1).ToString());
     }
     List<Case> NonClusterdArrCases;
     //---------------------------------------------------------------------------------
     for (int u = 0; u < statistics.exp_random_cases.Count; u++)
     {
         sim = Math.Round(0.10, 2);
         Case _problem = statistics.exp_random_cases[u];
         n = new int[18];
         // Criteria Comparsions------------------------
         int c = 0;
         while (sim < 1)
         {

             NonClusterdArrCases = new List<Case>();
             // solutions
             NonClusterdArrCases = statistics.FindAlternative_NonClustering(_problem, sim.ToString());

             fd.dg_Non_stat.Rows.Add();
             fd.dg_Non_stat.Rows[dG_NonR].Cells[1].Value = _problem.GetFeature("id").GetFeatureValue().ToString();
             fd.dg_Non_stat.Rows[dG_NonR].Cells[0].Value = sim.ToString();
             if (NonClusterdArrCases.Count > 0)

                 for (int i = 0; i < NonClusterdArrCases.Count; i++)
                 {
                     fd.dg_Non_stat.Rows[dG_NonR].Cells[i + 2].Value = NonClusterdArrCases[i].GetFeature("id").GetFeatureValue().ToString();
                 }
             dG_NonR++;
             n[c] = NonClusterdArrCases.Count;
             c++;
             sim += Math.Round(0.05, 2);
             sim = Math.Round(sim, 2);

         }
         narr.Add(n);
     }

      sim = 0.10;
     fd.dG_details.Columns.Add("Sim", "Sim");
     fd.dG_details.Columns.Add("Problem_no", "Problem_no");
     fd.dG_details.Columns.Add("Non Clustering Sols", "Non Clustering Sols");
     fd.dG_details.Columns.Add("With Clustering Sols", "With Clustering Sols");

     int y = 0;
     int r = 0;
     while (sim < 1)
     {
         for (int i = 0; i < statistics.exp_random_cases.Count; i++)
         {
             fd.dG_details.Rows.Add();
             fd.dG_details.Rows[r].Cells[0].Value = sim.ToString();
             fd.dG_details.Rows[r].Cells[1].Value = statistics.exp_random_cases[i].GetFeature("id").GetFeatureValue().ToString();
             fd.dG_details.Rows[r].Cells[2].Value = narr[i][y];
             fd.dG_details.Rows[r].Cells[3].Value = warr[i][y];
             r++;

         }
         y++;
         sim += 0.05;
         sim = Math.Round(sim, 2);
     }

     fd.dG_details.Rows.Add();
     fd.listBox1.Items.Add("Distance between Problem and All Clusters");
     for (int k = 0; k < statistics.exp_random_cases.Count; k++)
     {
         for (int i = 0; i < statistics.centroids.Count; i++)
         {
             EuclideanSimilarity simr = new EuclideanSimilarity();
             fd.listBox1.Items.Add(statistics.exp_random_cases[k].GetFeature("id").GetFeatureValue().ToString()
                 + "   center " + statistics.centroids[i].GetFeature("id").GetFeatureValue().ToString()
                 + " = " + simr.Similarity(statistics.exp_random_cases[k], statistics.centroids[i]) + "\n");
         }
         fd.listBox1.Items.Add("=====================================================================");

     }

     fd.Show();
 }

 private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
 {
     read_files();
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

   }
}