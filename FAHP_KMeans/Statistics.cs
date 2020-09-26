using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace Fahp_cbr_app
{
    public class Statistics
    {
        string _env = @"E:\D Drive\_SAWSAN MASTER\MobileApplication\Fahp_cbr_app\Fahp_cbr_app\CBR\config.xml";
        public Dictionary<Case, List<Case>> AllNonClusterdCases = new Dictionary<Case, List<Case>>();
        public Dictionary<Case, List<Case>> AllClusterdCases = new Dictionary<Case, List<Case>>();
        public Dictionary<Case, double[]> AllAhpCriteria;
        public Dictionary<Case, double[]> AllFAhpCriteria;
        public Dictionary<Case, double[]> criteria_weights;
        public Dictionary<Case, Dictionary<double, double[,]>> AllNonClusterdResults;
        public Dictionary<Case, Dictionary<double, double[,]>> AllClusterdResults;
        public Dictionary<Case, double[,]> AllAhpResults ;
        public Dictionary<Case, double[,]> AllFuzzyAhpResults;
        public Dictionary<Case, double[,]> ProbCaseResults;
        public List<Case> ProbCases = new List<Case>();
        public List<Case> exp_random_cases= new  List<Case>();
        public List<Case> exp_random_cases_nonstandrize = new List<Case>();
        public string[] exp_random_cases_ranges ;
        public List<Case> centroids;
        public string tablename;
        public string casename;
        public string standrize_table;
        public string standrize_type;
        public int count_citeria;

        
        public List<string> citeria_non_exist = new List<string>();
        public  Statistics(string tablename,string casename)
        {
            this.tablename = tablename;
            this.casename = casename;
            standrize_table = Db.get_standriazation(tablename);
            standrize_type = Db.get_standriazation_type(tablename);
            centroids = Db.get_Centroids(standrize_table, casename);
            count_citeria = Db.get_criteria(casename).Count;
        }
        public void set_ProbCases(List<Case> probs){
            this.ProbCases=probs;
        }

        public  void GetRandomCases_File()
        {
            exp_random_cases.Clear();
            exp_random_cases_nonstandrize.Clear();
            Case Max_Mean = Db.get_Case_data(standrize_table, -2);
            Case Min_Sd = Db.get_Case_data(standrize_table, -3);
            List<Case> templist = new List<Case>();
            templist = Db.read_file();
            

            if (templist.Count != 0)
            for (int i = 0; i < templist.Count; i++)
            {
                exp_random_cases_nonstandrize.Add(templist[i]);
                  //  i have to put general seetings for normalization a
                Case c = BestChoice.StandardizeCase(templist[i], Max_Mean, Min_Sd, standrize_type);
                exp_random_cases.Add(c);
            }
        }
        //=========================================================================================
        public void GenerateRandomCases()
        {
            exp_random_cases.Clear();
            Case Max_Mean = Db.get_Case_data(standrize_table, -2);
            Case Min_Sd = Db.get_Case_data(standrize_table, -3);
            Random r = new Random();
            List<Case> Cases = Db.get_cases_condition(tablename, casename);

         for (int i = 0; i < 100; i++)
         {
             double x = 0;
             Case c = new Case(0, Max_Mean.GetCaseName(), "");
            
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
                     int k = r.Next(0, Cases.Count);
                     Feature f = (Feature)Cases[k].GetFeatures()[j];
                     c.AddFeature(max_f.GetFeatureName(), max_f.GetFeatureType(), f.GetFeatureValue(), max_f.GetWeight(), max_f.GetIsKey(), max_f.GetIsIndex(), max_f.GetFeatureUnit());
                     continue;
                 }
                 else if (max_f.GetFeatureType() == 1)// mstring, string, ordinal, categorical
                 {
                     int k = r.Next(0, 2);
                     bool o = false;
                     if (k == 1) o = true;
                     c.AddFeature(max_f.GetFeatureName(), max_f.GetFeatureType(), o, max_f.GetWeight(), max_f.GetIsKey(), max_f.GetIsIndex(), max_f.GetFeatureUnit());
                     continue;
                 }
                 else if (max_f.GetFeatureType() == 2)
                 {

                     int max = Convert.ToInt32(max_f.GetFeatureValue());
                     int min = Convert.ToInt32(min_f.GetFeatureValue());
                     x = r.Next(min, max + 1);
                 }
                 else
                 {

                     double max = Convert.ToDouble(max_f.GetFeatureValue());
                     double min = Convert.ToDouble(min_f.GetFeatureValue());
                     x = r.NextDouble(min, max + 0.05);
                     x = Math.Round(x, 1);
                 }

                 c.AddFeature(max_f.GetFeatureName(), max_f.GetFeatureType(), x, max_f.GetWeight(), max_f.GetIsKey(), max_f.GetIsIndex(), max_f.GetFeatureUnit());
             }
                exp_random_cases_nonstandrize.Add(c);
                //  i have to put general seetings for normalization 
                exp_random_cases.Add(BestChoice.StandardizeCase(c, Max_Mean, Min_Sd, standrize_type));
            }
        }
        //====================================================================================
        public List<Case> FindAlternative_NonClustering(Case problem, string sim)
        {
            List<Case> NonClusterdArrCases=new List<Case>();
            DefaultEngine _engine = new DefaultEngine();
            Db.set_xml(sim,casename,tablename);
            ArrayList myarr = Db.get_cases_as_arraylist_condition(standrize_table, casename);
            // get particular cluster
            _engine.SetEnvironmentVariable(_env);
            _engine.SetProblem(problem);
            _engine.Startup();
            ArrayList cases = _engine.Run_caseRetrieval_partial(myarr);
            int count_solution = cases.Count;
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
            }
            return NonClusterdArrCases;

        }
            // with Clustering
        //=================================================================================
        public List<Case> FindAlternative_WithClustering(Case problem, string sim)
        {
            List<Case> ClusterdArrCases = new List<Case>();
            DefaultEngine _engine = new DefaultEngine();
            
            Db.set_xml(sim, casename,tablename);
           // centroids = new List<Case>();
            Kmeans kmeans = new Kmeans();
            ArrayList current_cases_partial = new ArrayList();
            // get centroids
            //centroids = Db.get_Centroids(standrize_table, casename);
            // know which cluster
            ClusterdArrCases = new List<Case>();
            current_cases_partial.Clear();
            int t = kmeans.findCluster_customize(problem, centroids);

            // get particular cluster
            current_cases_partial = Db.get_cases_condition_cluster_arraylist(standrize_table, casename, "cluster", t.ToString());

            _engine.SetEnvironmentVariable(_env);
            _engine.SetProblem(problem);
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
            }

            return ClusterdArrCases;
        }
        //=================================================================================

        public List<Case> FindAlternative_SMF(Case problem, string sim)
        {
            List<Case> ClusterdArrCases = new List<Case>();
            DefaultEngine _engine = new DefaultEngine();

            Db.set_xml(sim, casename, tablename);
            // centroids = new List<Case>();
            Kmeans kmeans = new Kmeans();
            ArrayList current_cases_partial = new ArrayList();
            // get centroids
            //centroids = Db.get_Centroids(standrize_table, casename);
            // know which cluster
            ClusterdArrCases = new List<Case>();
            current_cases_partial.Clear();
            int t = kmeans.findCluster_customize(problem, centroids);

            // get particular cluster
            current_cases_partial = Db.get_cases_condition_cluster_arraylist(standrize_table, casename, "cluster", t.ToString());

            _engine.SetEnvironmentVariable(_env);
            _engine.SetProblem(problem);
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
            }

            return ClusterdArrCases;
        }
        //=================================================================================
        public List<Case> FindAlternative_WithClustering(Case problem)
        {

            Kmeans kmeans = new Kmeans();
            // know which cluster
            int t = kmeans.findCluster_customize(problem, centroids);
            // get particular cluster
            return( Db.get_cases_condition_cluster(standrize_table, casename, "cluster", t.ToString()));

     
           
        }
        //=================================================================================
        void FindAlternative_ALLRandomCases_Sim(string sim)
        {

            for (int f = 0; f < exp_random_cases.Count; f++)
            {

                AllNonClusterdCases.Add(exp_random_cases[f], FindAlternative_NonClustering(exp_random_cases[f], sim));
                AllClusterdCases.Add(exp_random_cases[f], FindAlternative_WithClustering(exp_random_cases[f], sim));
            }
            
        }
        //=======================================================================================================================
        public void FindSolutions_AHP()
        {
            bool consistency;
            double[] choices;
            double[] tempresult;
            AllNonClusterdResults = new Dictionary<Case, Dictionary<double, double[,]>>();
            AllClusterdResults = new Dictionary<Case, Dictionary<double, double[,]>>();
            criteria_weights = new Dictionary<Case, double[]>() ;
            Dictionary<string, double> cri_dic;
            Dictionary<double, double[,]> tempNon;
            Dictionary<double, double[,]> tempWith;
            List<Case> ClusterdArrCases;
            List<Case> NonClusterdArrCases;
            double[,] Non_Results;
            double[,] With_Results;
            ArrayList current_cases_partial = new ArrayList();
            //List<Case> centroids = new List<Case>();
            Kmeans kmeans = new Kmeans();
            Dictionary<int, List<Case>> cases_by_centroids = new Dictionary<int, List<Case>>();


            // get centroids for case
            for (int i = 0; i < centroids.Count; i++)
            {
                int k = Convert.ToInt32(centroids[i].GetFeature("id").GetFeatureValue());
                cases_by_centroids.Add(k, Db.get_cases_condition_cluster(tablename,casename, "cluster", k.ToString()));
            }


            //---------------------------------------------------------------------------------
            for (int u = 0; u < exp_random_cases.Count; u++)
            {
                cri_dic = new Dictionary<string, double>(); 
                double sim = Math.Round(0.10, 2);
                Case _problem = exp_random_cases[u];
                // Criteria Comparsions------------------------
                MyAHPModel model = new MyAHPModel(count_citeria);
                tempNon = new  Dictionary<double, double[,]> ();
                tempWith = new Dictionary<double, double[,]>();
                double[,] cri_arr = model.Create_Criteria_Comparison_Array(count_citeria);

                //-------Criteria Weights----------------------------------------------------------------
                model.AddCriteria(cri_arr);
                double[] cri_wgh = new double[count_citeria];
                List<string> criteria_names= Db.get_criteria(casename);
                
                bool res = false;
                while (!res)
                    if (model.CalculatedCriteria()) res = true;
                cri_wgh = model.CriteriaWeights;


                criteria_weights.Add(_problem, model.CriteriaWeights);


                //-------------------------------AHP--------------------------------------------------
                while (sim < 1)
                {
                   

                    NonClusterdArrCases = FindAlternative_NonClustering(_problem, sim.ToString());
                    ClusterdArrCases = FindAlternative_WithClustering(_problem, sim.ToString());
  
                    if (NonClusterdArrCases.Count != 0)
                    {
                        // non clustering 
                        MyAHPModel model1 = new MyAHPModel(count_citeria, NonClusterdArrCases.Count);
                        model1.AddCriteria(cri_arr);
                        model1.CalculatedCriteria();
                        // Choices Comparsions------------------------
                        consistency = false;
                        choices = new double[NonClusterdArrCases.Count];
                        tempresult = new double[NonClusterdArrCases.Count];
                        for (int uu = 0; uu < count_citeria; uu++)
                        {
                            double[,] comp_choice = model1.CreateOne_CeiteriaChoiceComp(uu, NonClusterdArrCases);
                            while (!consistency)
                            {
                                tempresult = model1.Calculated_One__Choic(out consistency, model1.CriteriaWeights[uu], comp_choice);
                            }
                            consistency = false;
                            for (int h = 0; h < tempresult.GetLength(0); h++)
                                choices[h] += tempresult[h];
                        }
                        int ii = 0;
                        Non_Results = new double[NonClusterdArrCases.Count, 2];
                        foreach (Case cc in NonClusterdArrCases)
                        {
                            Non_Results[ii, 0] = Convert.ToDouble(cc.GetFeature("id").GetFeatureValue());
                            Non_Results[ii, 1] = choices[ii] * 100;
                            ii++;
                        }
                        // sort Clustered
                        for (int i = 0; i < Non_Results.GetLength(0) - 1; i++)
                            for (int j = 0; j < Non_Results.GetLength(0) - i - 1; j++)
                                if (Non_Results[j, 1] < Non_Results[j + 1, 1])
                                {
                                    double temp = Non_Results[j, 1];
                                    Non_Results[j, 1] = Non_Results[j + 1, 1];
                                    Non_Results[j + 1, 1] = temp;
                                    temp = Non_Results[j, 0];
                                    Non_Results[j, 0] = Non_Results[j + 1, 0];
                                    Non_Results[j + 1, 0] = temp;
                                }
                        tempNon.Add(sim,Non_Results);
                        
                    }
                    if (ClusterdArrCases.Count != 0)
                    {
                        // Clustering
                        MyAHPModel model2 = new MyAHPModel(count_citeria, ClusterdArrCases.Count);
                        model2.AddCriteria(cri_arr);
                        model2.CalculatedCriteria();
                        // Choices Comparsions------------------------
                        consistency = false;
                        choices = new double[ClusterdArrCases.Count];
                        tempresult = new double[ClusterdArrCases.Count];
                        for (int uu = 0; uu < count_citeria; uu++)
                        {
                            double[,] comp_choice = model2.CreateOne_CeiteriaChoiceComp(uu, ClusterdArrCases);

                            while (!consistency)
                            {
                                tempresult = model2.Calculated_One__Choic(out consistency, model2.CriteriaWeights[uu], comp_choice);
                            }
                            consistency = false;
                            for (int h = 0; h < tempresult.GetLength(0); h++)
                                choices[h] += tempresult[h];
                        }
                        int ii = 0;
                        With_Results = new double[ClusterdArrCases.Count, 2];
                        foreach (Case cc in ClusterdArrCases)
                        {
                            With_Results[ii, 0] = Convert.ToDouble(cc.GetFeature("id").GetFeatureValue());
                            With_Results[ii, 1] = choices[ii] * 100;
                            ii++;
                        }

                        // sort Clustered
                        for (int i = 0; i < With_Results.GetLength(0) - 1; i++)
                            for (int j = 0; j < With_Results.GetLength(0) - i - 1; j++)
                                if (With_Results[j, 1] < With_Results[j + 1, 1])
                                {
                                    double temp = With_Results[j, 1];
                                    With_Results[j, 1] = With_Results[j + 1, 1];
                                    With_Results[j + 1, 1] = temp;
                                    temp = With_Results[j, 0];
                                    With_Results[j, 0] = With_Results[j + 1, 0];
                                    With_Results[j + 1, 0] = temp;
                                }
                        tempWith.Add(sim, With_Results);
                    } // end if 

                    sim += Math.Round(0.05,2);
                    sim = Math.Round(sim, 2);
                    
                } // end sim  
                AllNonClusterdResults.Add(exp_random_cases[u], tempNon);
                AllClusterdResults.Add(exp_random_cases[u], tempWith);
            } // end for
        }

        // -----------------------------------------------------------------------------------
        public Dictionary<Case ,double[,]> AHP(List<Case> Cases)
        {
            double[,] Non_Results;
            Dictionary<Case ,double[,]> Results = new Dictionary<Case, double[,]>();
            for (int u = 0; u < exp_random_cases.Count; u++)
            {
                Case _problem = exp_random_cases[u];
                // Criteria Comparsions------------------------
                MyAHPModel model = new MyAHPModel();
               
                double[,] cri_arr = model.Create_Criteria_Comparison_Array(count_citeria);
                MyAHPModel model1 = new MyAHPModel(count_citeria, Cases.Count);
                model1.AddCriteria(cri_arr);
                model1.CalculatedCriteria();
                // Choices Comparsions------------------------
                bool consistency = false;
                double[] choices = new double[Cases.Count];
                double[] tempresult = new double[Cases.Count];
                for (int uu = 0; uu < count_citeria; uu++)
                {
                    double[,] comp_choice = model1.CreateOne_CeiteriaChoiceComp(uu, Cases);
                    while (!consistency){
                        tempresult = model1.Calculated_One__Choic(out consistency, model1.CriteriaWeights[uu], comp_choice);
                    }
                    for (int h = 0; h < tempresult.GetLength(0); h++)
                        choices[h] += tempresult[h];
                }
                int ii = 0;
                Non_Results = new double[Cases.Count, 2];
                foreach (Case c in Cases)
                {
                    Non_Results[ii, 0] = Convert.ToDouble(c.GetFeature("id").GetFeatureValue());
                    Non_Results[ii, 1] = choices[ii] * 100;
                    ii++;
                }
                // sort Clustered
                for (int i = 0; i < Non_Results.GetLength(0) - 1; i++)
                    for (int j = 0; j < Non_Results.GetLength(0) - i - 1; j++)
                        if (Non_Results[j, 1] < Non_Results[j + 1, 1])
                        {
                            double temp = Non_Results[j, 1];
                            Non_Results[j, 1] = Non_Results[j + 1, 1];
                            Non_Results[j + 1, 1] = temp;
                            temp = Non_Results[j, 0];
                            Non_Results[j, 0] = Non_Results[j + 1, 0];
                            Non_Results[j + 1, 0] = temp;
                        }
                Results.Add(exp_random_cases[u], Non_Results);
                        
                }
            return(Results);
        }



        //---------------------------------------------------------------------------------------
        public void FindSolutions_FuzzyAHP()
        {
            bool consistency;
            double[] choices;
            double[] tempresult;
            AllAhpCriteria = new Dictionary<Case, double[]>();
            AllFAhpCriteria = new Dictionary<Case, double[]>();
            AllAhpResults = new Dictionary<Case, double[,]>();
            AllFuzzyAhpResults = new Dictionary<Case,double[,]>();
            List<double[,]> tempahp;
            List<double[,]> tempfahp;
            List<Case> AhpArrCases;
            double[,] Ahp_Results;
            double[,] Fahp_Results;
  
            //---------------------------------------------------------------------------------
            for (int u = 0; u < exp_random_cases.Count; u++)
            {
                
                Case _problem = exp_random_cases[u];
                // Criteria Comparsions------------------------
                MyAHPModel model = new MyAHPModel(count_citeria);
                tempahp = new List<double[,]>();
                tempfahp = new List<double[,]>();
                double[,] cri_arr = model.Create_Criteria_Comparison_Array(count_citeria);

                //-------Criteria Weights----------------------------------------------------------------
                model.AddCriteria(cri_arr);
                double[] cri_wgh = new double[count_citeria];
                bool res = false;
                while (!res)
                    if (model.CalculatedCriteria()) res = true;
                cri_wgh = model.CriteriaWeights;
                //-------------------------------AHP--------------------------------------------------
                AhpArrCases = FindAlternative_WithClustering(_problem);
                if (AhpArrCases.Count != 0)
                {
                    // non clustering 
                    MyAHPModel model1 = new MyAHPModel(count_citeria, AhpArrCases.Count);
                    model1.AddCriteria(cri_arr);
                    model1.CalculatedCriteria();
                    AllAhpCriteria.Add(exp_random_cases[u], model1.CriteriaWeights);
                    // Choices Comparsions------------------------
                    consistency = false;
                    choices = new double[AhpArrCases.Count];
                    tempresult = new double[AhpArrCases.Count];
                    for (int uu = 0; uu < count_citeria; uu++)
                    {
                        double[,] comp_choice = model1.CreateOne_CeiteriaChoiceComp(uu, AhpArrCases);
                        while (!consistency)
                        {
                            tempresult = model1.Calculated_One__Choic(out consistency, model1.CriteriaWeights[uu], comp_choice);
                        }
                        consistency = false;
                        for (int h = 0; h < tempresult.GetLength(0); h++)
                            choices[h] += tempresult[h];
                    }
                    int ii = 0;
                    Ahp_Results = new double[AhpArrCases.Count, 2];
                    foreach (Case cc in AhpArrCases)
                    {
                        Ahp_Results[ii, 0] = Convert.ToDouble(cc.GetFeature("id").GetFeatureValue());
                        Ahp_Results[ii, 1] = choices[ii] * 100;
                        ii++;
                    }
                    // sort Clustered
                    for (int i = 0; i < Ahp_Results.GetLength(0) - 1; i++)
                        for (int j = 0; j < Ahp_Results.GetLength(0) - i - 1; j++)
                            if (Ahp_Results[j, 1] < Ahp_Results[j + 1, 1])
                            {
                                double temp = Ahp_Results[j, 1];
                                Ahp_Results[j, 1] = Ahp_Results[j + 1, 1];
                                Ahp_Results[j + 1, 1] = temp;
                                temp = Ahp_Results[j, 0];
                                Ahp_Results[j, 0] = Ahp_Results[j + 1, 0];
                                Ahp_Results[j + 1, 0] = temp;
                            }
                            else if (Ahp_Results[j, 1] == Ahp_Results[j + 1, 1])
                            {
                                 EuclideanSimilarity s = new EuclideanSimilarity();
                                int num1=Convert.ToInt32(Ahp_Results[j, 0]);
                                int num2=Convert.ToInt32(Ahp_Results[j+1, 0]);
                                double sim1 = s.Similarity(AhpArrCases[num1],_problem);
                                double sim2 = s.Similarity(AhpArrCases[num2],_problem);
                                if (sim2 > sim1)
                                {
                                        double temp = Ahp_Results[j, 1];
                                        Ahp_Results[j, 1] = Ahp_Results[j + 1, 1];
                                        Ahp_Results[j + 1, 1] = temp;
                                        temp = Ahp_Results[j, 0];
                                        Ahp_Results[j, 0] = Ahp_Results[j + 1, 0];
                                        Ahp_Results[j + 1, 0] = temp;
                                }
                            }
                   

                    // Fuzzy
                    MyFuzzyAHP model2 = new MyFuzzyAHP(count_citeria, AhpArrCases.Count);
                    model2.AddCriteria(cri_arr);
                    model2.CalculatedCriteria();
                    AllFAhpCriteria.Add(exp_random_cases[u], model2.CriteriaWeights);
                    // Choices Comparsions------------------------
                    consistency = false;
                    choices = new double[AhpArrCases.Count];
                    tempresult = new double[AhpArrCases.Count];
                    for (int uu = 0; uu < count_citeria; uu++)
                    {
                        double[,] comp_choice = model2.CreateOne_CeiteriaChoiceComp(uu, AhpArrCases);

                        while (!consistency)
                        {
                            tempresult = model2.Calculated_One__Choice(out consistency, model2.CriteriaWeights[uu], comp_choice);
                        }
                        consistency = false;
                        for (int h = 0; h < tempresult.GetLength(0); h++)
                            choices[h] += tempresult[h];
                    }
                    ii = 0;
                    Fahp_Results = new double[AhpArrCases.Count, 2];
                    foreach (Case cc in AhpArrCases)
                    {
                        Fahp_Results[ii, 0] = Convert.ToDouble(cc.GetFeature("id").GetFeatureValue());
                        Fahp_Results[ii, 1] = choices[ii] * 100;
                        ii++;
                    }

                    // sort Clustered
                    for (int i = 0; i < Fahp_Results.GetLength(0) - 1; i++)
                        for (int j = 0; j < Fahp_Results.GetLength(0) - i - 1; j++)
                            if (Fahp_Results[j, 1] < Fahp_Results[j + 1, 1])
                            {
                                double temp = Fahp_Results[j, 1];
                                Fahp_Results[j, 1] = Fahp_Results[j + 1, 1];
                                Fahp_Results[j + 1, 1] = temp;
                                temp = Fahp_Results[j, 0];
                                Fahp_Results[j, 0] = Fahp_Results[j + 1, 0];
                                Fahp_Results[j + 1, 0] = temp;
                            }
                    AllAhpResults.Add(exp_random_cases[u], Ahp_Results);
                    AllFuzzyAhpResults.Add(exp_random_cases[u], Fahp_Results);
                } 
            } //end if
        }// end for
        // -----------------------------------------------------------------------------------
    }
}
