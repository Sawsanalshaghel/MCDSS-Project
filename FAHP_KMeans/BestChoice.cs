using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
namespace Fahp_cbr_app
{
    class BestChoice
    {

        public List<Case> Cases = new List<Case>();
        public KmeansPlus.PointClusters Clusters;
        List<Case> Centroids = new List<Case>();
        public Case Problem;
        public Case StandrizeProblem;
        private ArrayList arraycases = new ArrayList();
        private string casename = "";
        private string features = "";
        private string tablename = "";
        private string tablename_standardization = "";
        private string features_standrize_types = "";
        private string[] rowsAsstring = {};
        private Case case_max_mean;
        private Case case_min_sd;
        private string standrize_type;
        private string kmeans_method;
        private string clusters_num;
        private double cost_function=0;
       

        public ArrayList ArrayCases
        {

            get { return this.arraycases; }
            set { this.arraycases = value; }
        }


        public string CaseName 
        {

            get { return this.casename; }
            set { this.casename = value; }
        }
        public string TableName 
        {

            get { return this.tablename; }
            set { tablename = value; }
        }

        public string Tablename_Standardization
        {
            get { return this.tablename_standardization; }
            set { tablename_standardization = value; }
        }

       
        public string Features_Standrize_Types
        {

            get { return this.features_standrize_types; }
            set { this.features_standrize_types = value; }
        }

        public string Features_without_Types
        {

            get { return this.features; }
            set { this.features = value; }
        }
        public string[] RowsAsString
        {

            get { return rowsAsstring; }
            set { this.rowsAsstring = value; }
        }

        public string Standrize_Type
        {

            get { return this.standrize_type; }
            set { this.standrize_type = value; }
        }

        public string Kmeans_Method
        {

            get { return this.kmeans_method; }
            set { this.kmeans_method = value; }
        }
        public string Clusters_Num
        {

            get { return this.clusters_num; }
            set { this.clusters_num = value; }
        }

        public Case Case_Max_Mean
        {

            get { return this.case_max_mean; }
            set { this.case_max_mean = value; }
        }

        public Case Case_Min_SD
        {

            get { return this.case_min_sd; }
            set { this.case_min_sd = value; }
        }

        public double Cost_Function
        {

            get { return this.cost_function; }
            set { this.cost_function = value; }
        }

   

        public BestChoice(ArrayList ArrayCases,string Standrize_Type,string Kmeans_Method, string Clusters_Num)
        {
            this.ArrayCases = ArrayCases;
            Case b = (Case)ArrayCases[0];
            this.CaseName = b.GetCaseName();
            this.TableName = "tbl_" + this.CaseName;
            this.Standrize_Type = Standrize_Type;
            this.Kmeans_Method = Kmeans_Method;
            this.Clusters_Num = Clusters_Num;
            this.Tablename_Standardization = "tbl_" + this.Standrize_Type + this.CaseName;
        }

        public BestChoice(ArrayList ArrayCases, List<Case> cases,string Standrize_Type,string Kmeans_Method)
        {
            this.ArrayCases = ArrayCases;
            Case b = (Case)ArrayCases[0];
            this.CaseName = b.GetCaseName();
            this.TableName = "tbl_" + this.CaseName;
            this.Standrize_Type = Standrize_Type;
            this.Kmeans_Method = Kmeans_Method;
            this.Cases = cases;
            this.Tablename_Standardization = "tbl_" + this.Standrize_Type + this.CaseName;
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
            value = new XAttribute("value", "$CaseID=" + CaseName + "* $DbType=Mssql *  $DataSource=Data Source=.;Initial Catalog=PhoneDB;Integrated Security=True * $DictionaryTable=tbl_dic * $DataTable=" +TableName);
            parameters_node.Add(name);
            parameters_node.Add(value);
            parameters.Add(parameters_node);
            file.Add(parameters);
            file.Save(path);
        }

        
        void make_rows_features()
        {
                        Case b = (Case)ArrayCases[0];
            Feature ff = null;
            string features = "";
            string Standrize_features = "";
            string rawfeatures="";

            // make case features as columns and types for insert statment
            for (int j = 0; j < b.GetFeatures().Count; j++)
            {

                ff = (Feature)b.GetFeatures()[j];
                if (j != (b.GetFeatures().Count - 1))
                {
                    features += ff.GetFeatureName() + "  " + Token.GetMsSqlType(ff.GetFeatureType()) + ",";
                    Standrize_features += "S_" + ff.GetFeatureName() + "  " + Token.GetMsSqlType(ff.GetFeatureType()) + ",";
                    rawfeatures += ff.GetFeatureName() + "  ,";
                }
                else
                {
                    features += ff.GetFeatureName() + "  " + Token.GetMsSqlType(ff.GetFeatureType());
                    Standrize_features += "S_" + ff.GetFeatureName() + "  " + Token.GetMsSqlType(ff.GetFeatureType());
                    rawfeatures += ff.GetFeatureName() ;
                }
            }
            // rows as string


            this.RowsAsString = get_rows_as_string(ArrayCases);
            this.Features_Standrize_Types = features;// +"," + Standrize_features;
            this.Features_without_Types = rawfeatures;
        }
        public List<Case> CasesData(ArrayList ArrayCases)
        {
            List<Case> _CasesData = new List<Case>();
           
            foreach (Case c in ArrayCases)
            {
                Case newcase = new Case(c.GetCaseID(), c.GetCaseName(), c.GetCaseDescription());
                for (int j = 0; j < c.GetFeatures().Count; j++)
                {
                    Feature f = (Feature)c.GetFeatures()[j];
                    newcase.AddFeature(f.GetFeatureName(), f.GetFeatureType(), f.GetFeatureValue(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                }
                _CasesData.Add(newcase);
            }
            return _CasesData;
        }

//===================================================================================================
        public static Case Unstandrize(Case c, Case Case_Max_Mean, Case Case_Min_SD, string standrize_type)
        {
            Case result = new Case(c.GetCaseID(), c.GetCaseName(), c.GetCaseDescription());
            if (standrize_type == "Q")
            {
                for (int i = 0; i < c.GetFeatures().Count; i++)
                {
                    Feature fa = (Feature)c.GetFeatures()[i];
                    Feature ma = (Feature)Case_Max_Mean.GetFeatures()[i];
                    Feature sa = (Feature)Case_Min_SD.GetFeatures()[i];

                    if (fa.GetFeatureName() == "id")
                        result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_INT)
                    {
                        int ivalue = (Convert.ToInt32(fa.GetFeatureValue()) * Convert.ToInt32(sa.GetFeatureValue())) + Convert.ToInt32(ma.GetFeatureValue());
                        result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), ivalue, fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    }
                    else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT)
                    {
                        double dvalue = (Convert.ToDouble(fa.GetFeatureValue()) * Convert.ToDouble(sa.GetFeatureValue())) + Convert.ToDouble(ma.GetFeatureValue());
                        result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), dvalue, fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    }
                    else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_BOOL)
                        result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_STRING)
                        result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                }
                return result;


            }
            else if (standrize_type == "MN")
            {
                for (int j = 0; j < c.GetFeatures().Count; j++)
                {
                    Feature fa = (Feature)c.GetFeatures()[j];
                    Feature max = (Feature)Case_Max_Mean.GetFeatures()[j];
                    Feature min = (Feature)Case_Min_SD.GetFeatures()[j];

                    if (fa.GetFeatureName() == "id")
                        result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_INT)
                    {
                        int ivalue = (Convert.ToInt32(fa.GetFeatureValue()) * (Convert.ToInt32(max.GetFeatureValue()) - Convert.ToInt32(min.GetFeatureValue()))) + Convert.ToInt32(min.GetFeatureValue());
                        result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), ivalue, fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    }
                    else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT)
                    {
                        double dvalue = (Convert.ToDouble(fa.GetFeatureValue()) * (Convert.ToDouble(max.GetFeatureValue()) - Convert.ToDouble(min.GetFeatureValue()))) + Convert.ToDouble(min.GetFeatureValue());
                        result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), dvalue, fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    }
                    else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_BOOL)
                        result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_STRING)
                        result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                }
                return result;
            }

            return c;
        }
 //==================================================================================================
        public static Case StandardizeCase(Case c, Case Cmean_max, Case C_sd_min, string Standrize_Type)
        {
            Case newcase = new Case(c.GetCaseID(), c.GetCaseName(), c.GetCaseDescription());
            if (Standrize_Type == "Q")
            {
                for (int j = 0; j < c.GetFeatures().Count; j++)
                {
                    Feature fa = (Feature)c.GetFeatures()[j];
                    Feature ma = (Feature)Cmean_max.GetFeatures()[j];
                    Feature sa = (Feature)C_sd_min.GetFeatures()[j];

                    if (fa.GetFeatureName() == "id" || fa.GetFeatureName() == "cluster")
                        newcase.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_INT)
                    {
                        int ivalue = (Convert.ToInt32(fa.GetFeatureValue()) - Convert.ToInt32(ma.GetFeatureValue())) / Convert.ToInt32(sa.GetFeatureValue());
                        newcase.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), ivalue, fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    }
                    else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT)
                    {
                        double dvalue = (Convert.ToDouble(fa.GetFeatureValue()) - Convert.ToDouble(ma.GetFeatureValue())) / Convert.ToDouble(sa.GetFeatureValue());
                        newcase.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), dvalue, fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    }
                    else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_BOOL)
                        newcase.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_STRING || fa.GetFeatureType() == FeatureType.TYPE_FEATURE_CATEGORICAL)
                        newcase.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                } // end features

                return newcase;
            }
            else
                if (Standrize_Type == "MN")
                {
                    for (int j = 0; j < c.GetFeatures().Count; j++)
                    {
                        Feature fa = (Feature)c.GetFeatures()[j];
                        Feature max = (Feature)Cmean_max.GetFeatures()[j];
                        Feature min = (Feature)C_sd_min.GetFeatures()[j];

                        if (fa.GetFeatureName() == "id" || fa.GetFeatureName() == "cluster")
                            newcase.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                        else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_INT)
                        {
                            int ivalue = (Convert.ToInt32(fa.GetFeatureValue()) - Convert.ToInt32(min.GetFeatureValue())) / (Convert.ToInt32(max.GetFeatureValue()) - Convert.ToInt32(min.GetFeatureValue()));
                            newcase.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), ivalue, fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                        }
                        else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT)
                        {
                            double dvalue = (Convert.ToDouble(fa.GetFeatureValue()) - Convert.ToDouble(min.GetFeatureValue())) / (Convert.ToDouble(max.GetFeatureValue()) - Convert.ToDouble(min.GetFeatureValue()));
                            newcase.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), dvalue, fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                        }
                        else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_BOOL)
                            newcase.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                        else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_STRING || fa.GetFeatureType() == FeatureType.TYPE_FEATURE_CATEGORICAL)
                            newcase.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    } // end features

                    return newcase;
                }

            return c;
        }

//--------------------------------------------------------------------------------------------------------------------
      public List<Case> Z_Score_Normalization(  ArrayList ArrayCases)
        {
            List<Case> _normalizedDataToCluster = new List<Case>();
            // z = (x - Mean) / StandardDiviation; //where u is the mean (average)  
            int sumf = 0; double sumd = 0; ; List<string> sumst = new List<string>();
            Case Temp = (Case)ArrayCases[0];
            Case_Max_Mean = new Case(Temp.GetCaseID(), "Mean", Temp.GetCaseDescription());
            Case_Min_SD = new Case(Temp.GetCaseID(), "SD", Temp.GetCaseDescription());
            Feature feature = null;
            for (int j = 0; j < Temp.GetFeatures().Count; j++)
            {

                foreach (Case c in ArrayCases)
                {
                    feature = (Feature)c.GetFeatures()[j];
                    if (feature.GetFeatureName() == "id" || feature.GetFeatureName() == "cluster")
                        break;
                    if (feature.GetFeatureType() == FeatureType.TYPE_FEATURE_INT)
                        sumf += Convert.ToInt32(feature.GetFeatureValue());
                    else if (feature.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT)
                        sumd += Convert.ToDouble(feature.GetFeatureValue());
                    else  // if string or bool
                        break;
                }
                // Mean
                int sumIMean = sumf / ArrayCases.Count;
                double sumDMean = sumd / ArrayCases.Count;

                
               
                if (feature.GetFeatureName() == "cluster")
                    Case_Max_Mean.AddFeature("cluster", FeatureType.TYPE_FEATURE_INT, -2, feature.GetWeight(), feature.GetIsKey(), feature.GetIsIndex(), "Mean");
                else if (feature.GetFeatureName() == "id")
                    Case_Max_Mean.AddFeature(feature.GetFeatureName(), feature.GetFeatureType(), 0, feature.GetWeight(), feature.GetIsKey(), feature.GetIsIndex(), feature.GetFeatureUnit());
                else if (feature.GetFeatureType() == FeatureType.TYPE_FEATURE_STRING || feature.GetFeatureType() == FeatureType.TYPE_FEATURE_CATEGORICAL)
                    Case_Max_Mean.AddFeature(feature.GetFeatureName(), feature.GetFeatureType(), "", feature.GetWeight(), feature.GetIsKey(), feature.GetIsIndex(), feature.GetFeatureUnit());
                else if (feature.GetFeatureType() == FeatureType.TYPE_FEATURE_INT)
                    Case_Max_Mean.AddFeature(feature.GetFeatureName(), feature.GetFeatureType(), sumIMean, feature.GetWeight(), feature.GetIsKey(), feature.GetIsIndex(), feature.GetFeatureUnit());
                else if (feature.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT)
                    Case_Max_Mean.AddFeature(feature.GetFeatureName(), feature.GetFeatureType(), sumDMean, feature.GetWeight(), feature.GetIsKey(), feature.GetIsIndex(), feature.GetFeatureUnit());
                else if (feature.GetFeatureType() == FeatureType.TYPE_FEATURE_BOOL)
                    Case_Max_Mean.AddFeature(feature.GetFeatureName(), feature.GetFeatureType(), "", feature.GetWeight(), feature.GetIsKey(), feature.GetIsIndex(), feature.GetFeatureUnit());

                // Variance =sum(sqr( x-mean))

                int ISum = 0;
                double DSum = 0;
                
                foreach (Case c in ArrayCases)
                {
                    feature = (Feature)c.GetFeatures()[j];
                    if (feature.GetFeatureName() == "id" ||feature.GetFeatureName() == "cluster" )
                        break;
                    if (feature.GetFeatureType() == FeatureType.TYPE_FEATURE_INT)
                    {
                        double value = Convert.ToDouble(feature.GetFeatureValue());
                        value = value - sumIMean;
                        ISum += Convert.ToInt32(Math.Pow(value, 2));
                    }
                    else if (feature.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT)
                    {
                        double value = Convert.ToDouble(feature.GetFeatureValue());
                        value = value - sumDMean;
                        DSum += Math.Pow(value, 2);
                    }
                    else  // if string or bool
                        break;
                }
                // standard deviation
                int ISD = (int)Math.Sqrt(ISum / ArrayCases.Count);
                double DSD = Math.Sqrt(DSum / ArrayCases.Count);
                // standard deviation
                if (feature.GetFeatureName() == "cluster")
                    Case_Min_SD.AddFeature("cluster", FeatureType.TYPE_FEATURE_INT, -3, feature.GetWeight(), feature.GetIsKey(), feature.GetIsIndex(), "SD");
                else if (feature.GetFeatureName() == "id")
                    Case_Min_SD.AddFeature(feature.GetFeatureName(), feature.GetFeatureType(), 0, feature.GetWeight(), feature.GetIsKey(), feature.GetIsIndex(), feature.GetFeatureUnit());
                else if (feature.GetFeatureType() == FeatureType.TYPE_FEATURE_STRING || feature.GetFeatureType() == FeatureType.TYPE_FEATURE_CATEGORICAL)
                    Case_Min_SD.AddFeature(feature.GetFeatureName(), feature.GetFeatureType(), "", feature.GetWeight(), feature.GetIsKey(), feature.GetIsIndex(), feature.GetFeatureUnit());
                else if (feature.GetFeatureType() == FeatureType.TYPE_FEATURE_INT)
                    Case_Min_SD.AddFeature(feature.GetFeatureName(), feature.GetFeatureType(), ISD, feature.GetWeight(), feature.GetIsKey(), feature.GetIsIndex(), feature.GetFeatureUnit());
                else if (feature.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT)
                    Case_Min_SD.AddFeature(feature.GetFeatureName(), feature.GetFeatureType(), DSD, feature.GetWeight(), feature.GetIsKey(), feature.GetIsIndex(), feature.GetFeatureUnit());
                else if (feature.GetFeatureType() == FeatureType.TYPE_FEATURE_BOOL)
                    Case_Min_SD.AddFeature(feature.GetFeatureName(), feature.GetFeatureType(), "", feature.GetWeight(), feature.GetIsKey(), feature.GetIsIndex(), feature.GetFeatureUnit());

            } // end normalize for features

            
           
            

            foreach (Case c in ArrayCases)
            {
                Case newcase = new Case(Temp.GetCaseID(), Temp.GetCaseName(), Temp.GetCaseDescription());
                for (int j = 0; j < Temp.GetFeatures().Count; j++)
                {
                    Feature fa = (Feature)c.GetFeatures()[j];
                    Feature ma = (Feature)Case_Max_Mean.GetFeatures()[j];
                    Feature sa = (Feature)Case_Min_SD.GetFeatures()[j];

                    if (fa.GetFeatureName() == "id" || fa.GetFeatureName() == "cluster")
                        newcase.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_INT)
                    {
                        int ivalue = (Convert.ToInt32(fa.GetFeatureValue()) - Convert.ToInt32(ma.GetFeatureValue())) / Convert.ToInt32(sa.GetFeatureValue());
                        newcase.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), ivalue, fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    }
                    else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT)
                    {
                        double dvalue = (Convert.ToDouble(fa.GetFeatureValue()) - Convert.ToDouble(ma.GetFeatureValue())) / Convert.ToDouble(sa.GetFeatureValue());
                        newcase.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), dvalue, fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    }
                    else //if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_BOOL)
                        newcase.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                   // else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_STRING || fa.GetFeatureType() == FeatureType.TYPE_FEATURE_CATEGORICAL)
                    //    newcase.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                } // end features
                _normalizedDataToCluster.Add(newcase);
            } // end cases

            return _normalizedDataToCluster;
        }
//---------------------------------------------------------------------------------------------------------------------
      public List<Case> Max_Min_Scaling(ArrayList ArrayCases)
      {
          List<Case> _MinMaxDataToCluster = new List<Case>();
          Case Temp = (Case)ArrayCases[0];
          Case_Max_Mean = new Case(Temp.GetCaseID(), "Max", Temp.GetCaseDescription());
          Case_Min_SD = new Case(Temp.GetCaseID(), "Min", Temp.GetCaseDescription());
          for (int j = 0; j < Temp.GetFeatures().Count; j++)
          {
            Feature f = (Feature)Temp.GetFeatures()[j];
            if (f.GetFeatureName() == "cluster") 
            {
                Case_Max_Mean.AddFeature("cluster", FeatureType.TYPE_FEATURE_INT, -2, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), "Max");
                Case_Min_SD.AddFeature("cluster", FeatureType.TYPE_FEATURE_INT, -3, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), "Min");
            }
            else if (f.GetFeatureName() == "id" )
            {
                Case_Max_Mean.AddFeature(f.GetFeatureName(), f.GetFeatureType(), 0, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                Case_Min_SD.AddFeature(f.GetFeatureName(), f.GetFeatureType(), 0, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
            }
            else if ((f.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT) || (f.GetFeatureType() == FeatureType.TYPE_FEATURE_INT))// numeriacl 
            {
                List<double> numbers = new List<double>();
                foreach (Case c in ArrayCases)
                {
                    Feature t = (Feature)c.GetFeatures()[j];
                    numbers.Add(Convert.ToDouble(t.GetFeatureValue()));
                }
                Case_Max_Mean.AddFeature(f.GetFeatureName(), FeatureType.TYPE_FEATURE_FLOAT, numbers.Max(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                Case_Min_SD.AddFeature(f.GetFeatureName(), FeatureType.TYPE_FEATURE_FLOAT, numbers.Min(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
            }
            else if ( (f.GetFeatureType() == FeatureType.TYPE_FEATURE_Ordinal))// ordinal
            {
                List<double> numbers = new List<double>();
                foreach (Case c in ArrayCases)
                {
                    Feature t = (Feature)c.GetFeatures()[j];
                    numbers.Add(Convert.ToDouble(t.GetFeatureValue()));
                }
                Case_Max_Mean.AddFeature(f.GetFeatureName(), f.GetFeatureType(), numbers.Max(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                Case_Min_SD.AddFeature(f.GetFeatureName(), f.GetFeatureType(), numbers.Min(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
            }
            else  // bool, string, categorical
            {
                Case_Max_Mean.AddFeature(f.GetFeatureName(), f.GetFeatureType(), "", f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                Case_Min_SD.AddFeature(f.GetFeatureName(), f.GetFeatureType(), "", f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
            }
                                        
       }

        // x (norm)= (x-x(min))/(x(max)-x(min))
        foreach (Case c in ArrayCases)
        {
            Case newcase = new Case(Temp.GetCaseID(), Temp.GetCaseName(), Temp.GetCaseDescription());
            for (int j = 0; j < Temp.GetFeatures().Count; j++)
            {
                Feature fa = (Feature)c.GetFeatures()[j];
                Feature max = (Feature)Case_Max_Mean.GetFeatures()[j];
                Feature min = (Feature)Case_Min_SD.GetFeatures()[j];

                if (fa.GetFeatureName() == "id" || fa.GetFeatureName() == "cluster")
                    newcase.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                else if ((fa.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT) || (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_INT) )// numerical 
                {
                    double dvalue=0;

                    if (Convert.ToDouble(min.GetFeatureValue()) == Convert.ToDouble(max.GetFeatureValue()))
                        dvalue = Convert.ToDouble(fa.GetFeatureValue());
                    else
                       dvalue = (Convert.ToDouble(fa.GetFeatureValue()) - Convert.ToDouble(min.GetFeatureValue())) / (Convert.ToDouble(max.GetFeatureValue()) - Convert.ToDouble(min.GetFeatureValue()));
                    newcase.AddFeature(fa.GetFeatureName(), FeatureType.TYPE_FEATURE_FLOAT, dvalue, fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                }
                else if ( (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_Ordinal))// ordinal
                {
                    double dvalue = 0;

                    if (Convert.ToDouble(min.GetFeatureValue()) == Convert.ToDouble(max.GetFeatureValue()))
                        dvalue = Convert.ToDouble(fa.GetFeatureValue());
                    else
                        dvalue = (Convert.ToDouble(fa.GetFeatureValue()) - Convert.ToDouble(min.GetFeatureValue())) / (Convert.ToDouble(max.GetFeatureValue()) - Convert.ToDouble(min.GetFeatureValue()));
                    newcase.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), dvalue, fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                }
                  else // bool, string, categorical
                    newcase.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
               } // end features
            _MinMaxDataToCluster.Add(newcase);
        } // end cases

        return _MinMaxDataToCluster;
                }
      //---------------------------------------------------------------------------------------------------------------------
      public void No_Scaling(ArrayList ArrayCases)
      {
          
          Case Temp = (Case)ArrayCases[0];
          Case_Max_Mean = new Case(Temp.GetCaseID(), "Max", Temp.GetCaseDescription());
          Case_Min_SD = new Case(Temp.GetCaseID(), "Min", Temp.GetCaseDescription());

          double[] maxmin = new double[2];
          for (int j = 0; j < Temp.GetFeatures().Count; j++)
          {
              Feature f = (Feature)Temp.GetFeatures()[j];
              if (f.GetFeatureName() == "cluster")
              {
                  Case_Max_Mean.AddFeature("cluster", FeatureType.TYPE_FEATURE_INT, -2, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), "Max");
                  Case_Min_SD.AddFeature("cluster", FeatureType.TYPE_FEATURE_INT, -3, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), "Min");
              }

              else if (f.GetFeatureName() == "id")
              {
                  Case_Max_Mean.AddFeature(f.GetFeatureName(), f.GetFeatureType(), 0, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                  Case_Min_SD.AddFeature(f.GetFeatureName(), f.GetFeatureType(), 0, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
              }
              else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_INT)
              {
                  List<int> numbers = new List<int>();
                  foreach (Case c in ArrayCases)
                  {
                      Feature t = (Feature)c.GetFeatures()[j];
                      numbers.Add(Convert.ToInt32(t.GetFeatureValue()));
                  }

                  Case_Max_Mean.AddFeature(f.GetFeatureName(), f.GetFeatureType(), numbers.Max(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                  Case_Min_SD.AddFeature(f.GetFeatureName(), f.GetFeatureType(), numbers.Min(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
              }
              else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT)
              {

                  List<double> numbers = new List<double>();
                  foreach (Case c in ArrayCases)
                  {
                      Feature t = (Feature)c.GetFeatures()[j];
                      numbers.Add(Convert.ToDouble(t.GetFeatureValue()));
                  }
                  Case_Max_Mean.AddFeature(f.GetFeatureName(), f.GetFeatureType(), numbers.Max(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                  Case_Min_SD.AddFeature(f.GetFeatureName(), f.GetFeatureType(), numbers.Min(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
              }
              else
              {
                  Case_Max_Mean.AddFeature(f.GetFeatureName(), f.GetFeatureType(), "", f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                  Case_Min_SD.AddFeature(f.GetFeatureName(), f.GetFeatureType(), "", f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
              }
          }


          

          
      }

//---------------------------------------------------------------------------------------------------------------------
        public List<Case> StandardizeData(ArrayList ArrayCases)
        {

            if (Standrize_Type == "Q")

                return Z_Score_Normalization(ArrayCases);
            else
                if (Standrize_Type == "MN")
                    return Max_Min_Scaling(ArrayCases);
                else 
                    if (Standrize_Type == "none")
                     No_Scaling(ArrayCases);
            return CasesData(ArrayCases);
        }


        public string[] get_rows_as_string(ArrayList myarr)
        {  
            string rows = "";
            double f = (double) myarr.Count / 1000;
            if ((((f * 10) % 10) != 0) && (((f * 10) % 10) < 5))
                f = Math.Round(f) + 1;
            else f = f = Math.Round(f);
            int sc = Convert.ToInt32(f);
            string[] myrows = new string[sc];
            Feature ff = null;
            Case b = (Case)myarr[0];

            int c = 0;

               
            for (int i = 0; i < myarr.Count; i++)
            {
                rows += "(";
                b = (Case)myarr[i];
                for (int j = 0; j < b.GetFeatures().Count; j++)
                {
                    
                    ff = (Feature)b.GetFeatures()[j];
                    if (ff.GetFeatureType() == 5 || ff.GetFeatureType() == 6 || ff.GetFeatureType() == 8)
                        rows += "'"+ ff.GetFeatureValue().ToString()+"'";
                    else
                        if (ff.GetFeatureType() == 1 )
                            if ((bool)ff.GetFeatureValue() == true)
                                rows += 1;
                            else rows += 0;
                        else
                            rows += ff.GetFeatureValue().ToString() ;
 
                    if (j < b.GetFeatures().Count - 1)
                        rows += ",";
                    else
                        rows += "";
                }
                    
                if (((i+1)%1000==0) || (i==(ArrayCases.Count-1)))
                {
                    rows += ") ";
                    myrows[c] = rows;
                    rows = "";
                    c++;
                }
                else
                    rows += "), ";
                }

            return myrows;
    
    }

        public string[] get_rows_as_string(List<Case> myarr)
        {
            string rows = "";
            double f = (double)myarr.Count / 1000;
            if ((((f * 10) % 10) != 0) && (((f * 10) % 10) < 5))
                f = Math.Round(f) + 1;
            else f = f = Math.Round(f);
            int sc = Convert.ToInt32(f);
            string[] myrows = new string[sc];
            Feature ff = null;
            Case b = myarr[0];

            int c = 0;


            for (int i = 0; i < myarr.Count; i++)
            {
                rows += "(";
                b = (Case)myarr[i];
                for (int j = 0; j < b.GetFeatures().Count; j++)
                {

                    ff = (Feature)b.GetFeatures()[j];
                    if (ff.GetFeatureType() == 5 || ff.GetFeatureType() == 6 || ff.GetFeatureType() == 8)
                        rows += "'" + ff.GetFeatureValue().ToString() + "'";
                    else
                        if (ff.GetFeatureType() == 1)
                            if ((bool)ff.GetFeatureValue() == true)
                                rows += 1;
                            else rows += 0;
                        else
                            rows += ff.GetFeatureValue().ToString();

                    if (j < b.GetFeatures().Count - 1)
                        rows += ",";
                    else
                        rows += "";
                }

                if (((i + 1) % 1000 == 0) || (i == (myarr.Count - 1)))
                {
                    rows += ") ";
                    myrows[c] = rows;
                    rows = "";
                    c++;
                }
                else
                    rows += "), ";
            }

            return myrows;

        }
     
        public Case Revese_standrize(Case c)
        {
            Case result=new Case(c.GetCaseID(),c.GetCaseName(),c.GetCaseDescription());
            if (standrize_type=="Q")
            {
                for (int i=0; i<c.GetFeatures().Count;i++)
                {
                    Feature fa = (Feature)c.GetFeatures()[i];
                    Feature ma = (Feature)Case_Max_Mean.GetFeatures()[i];
                    Feature sa = (Feature)Case_Min_SD.GetFeatures()[i];

                    if (fa.GetFeatureName() == "id")
                       result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    else  if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_INT) 
                    {
                        int ivalue = (Convert.ToInt32(fa.GetFeatureValue()) * Convert.ToInt32( sa.GetFeatureValue()))+Convert.ToInt32(ma.GetFeatureValue()) ;
                        result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), ivalue, fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    }
                     else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT) 
                    {
                        double dvalue = (Convert.ToDouble(fa.GetFeatureValue()) * Convert.ToDouble( sa.GetFeatureValue()))+Convert.ToDouble(ma.GetFeatureValue()) ;
                         result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), dvalue, fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    }
                    else if  (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_BOOL)
                          result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit()); 
                  else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_STRING)
                          result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                }
                return result;


            }
            else if (standrize_type == "MN")
            {
                for (int j = 0; j < c.GetFeatures().Count; j++)
                {
                    Feature fa = (Feature)c.GetFeatures()[j];
                    Feature max = (Feature)Case_Max_Mean.GetFeatures()[j];
                    Feature min = (Feature)Case_Min_SD.GetFeatures()[j];

                    if (fa.GetFeatureName() == "id")
                        result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_INT)
                    {
                        int ivalue = (Convert.ToInt32(fa.GetFeatureValue()) * (Convert.ToInt32(max.GetFeatureValue()) - Convert.ToInt32(min.GetFeatureValue()))) + Convert.ToInt32(min.GetFeatureValue());
                        result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), ivalue, fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    }
                    else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT)
                    {
                        double dvalue = (Convert.ToDouble(fa.GetFeatureValue()) *(Convert.ToDouble(max.GetFeatureValue()) - Convert.ToDouble(min.GetFeatureValue())))+ Convert.ToDouble(min.GetFeatureValue()) ;
                        result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), dvalue, fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    }
                    else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_BOOL)
                        result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                    else if (fa.GetFeatureType() == FeatureType.TYPE_FEATURE_STRING)
                        result.AddFeature(fa.GetFeatureName(), fa.GetFeatureType(), fa.GetFeatureValue(), fa.GetWeight(), fa.GetIsKey(), fa.GetIsIndex(), fa.GetFeatureUnit());
                }
                return result;
            }
            
                return c;
        }


        public void LoadCases()
        {
           make_rows_features();
           Case a = (Case)ArrayCases[0];
           Db.insert_case_to_dic(a);
           Db.create_case_table(this.TableName, Features_Standrize_Types);
           //a.GetFeatures().Count- (id,cluster)
           Db.insert_into_tblcases(this.CaseName, this.TableName, (a.GetFeatures().Count-2).ToString(), this.Tablename_Standardization,this.Standrize_Type,this.Kmeans_Method,this.Clusters_Num,this.Cost_Function);
           Db.insert_data_to_caseTable(this.TableName, RowsAsString, Features_without_Types);
            //if (Standrize_Type!="none")
                Db.create_standrize_case_table(this.Tablename_Standardization, Features_Standrize_Types);

        }


        public double calucalte_P(  List<Case> cases)
            
        {
            

            EuclideanSimilarity _sim = new EuclideanSimilarity();
            double sim = 0;
            double total = 0;
            foreach (Case c_i in cases)
            {
                foreach (Case c_j in cases)
                    sim += _sim.Dissimilarity(c_i, c_j);
                sim = sim / (cases.Count - 1);
                total += sim;
                sim = 0;
            }
            return 1 / (1 + total);
        }

        public Case Updated_cluster(Case newcase, List<Case> cetroids, List<Case> cases)
        {

            double _sim=0;
            double p = calucalte_P(cases);
            double min = double.MaxValue;
            Case result = null;
            Case temp = null;
            EuclideanSimilarity sim = new EuclideanSimilarity();
         //   double all_sim = 0;
            foreach (Case cetroid in cetroids)
            {
               
                _sim = sim.Similarity(newcase, cetroid);
               // all_sim += _sim;
                if (_sim <= min)  // add to exist cluster
                {

                    temp = cetroid;
                    min = _sim;
                    
                }
            }

            _sim = sim.Similarity(newcase, temp);
            if (_sim > p) 
           // if (all_sim > p)
             result = temp;
            return result;
        }

        public Case Updated_cluster_Min_Centroid(Case newcase, List<Case> cetroids, List<Case> cases)
        {
            double _sim = 0;
            double p = double.MaxValue;

            Case result = null;
            EuclideanSimilarity sim = new EuclideanSimilarity();
            foreach (Case cetroid in cetroids)
            {
                _sim = sim.Similarity(newcase, cetroid);
                if (_sim <= p)  // add to exist cluster
                {

                    result = cetroid;
                    p = _sim;
                }
            }
            return result;

        }


        public List<Case> Re_standrize(Case updated_case)
        {
            int id = Convert.ToInt32(Db.Get_Max(TableName,"id"));
            updated_case.GetFeature("id").SetFeatureValue(id+1);
            if (Standrize_Type == "Q")
            {
                ArrayList myarr = Db.get_cases_as_arraylist_condition(TableName, CaseName);// نحضر البيانات بدون تقييس
                myarr.Add(updated_case);
                // يتم تعديل البيانات المقيسة والمتوسط والانحراف المعياري بدون مقارنات بعد اضافة العنصر
                List<Case> Narray  =  Z_Score_Normalization(myarr);
                //   تخزين كل البيانات المقيسة الانحراف المعياري والمتوسط
                Db.del_data_table(Tablename_Standardization);
                Db.insert_data_to_standrize_caseTable_noID(Tablename_Standardization, get_rows_as_string(Narray));//تخزين كل البيانات المقيسة
                Db.insert_case_to_table(Case_Max_Mean, Tablename_Standardization);//تخزين المتوسط
                Db.insert_case_to_table(Case_Min_SD, Tablename_Standardization);//تخزين الانحراف المعياري 
                Db.insert_case_to_table_without_id(updated_case, TableName);// تخزين العنصر الجديد في الجدول الأصلي
                return Narray;
            }
            else
                if (Standrize_Type == "MN")
                {
                    bool changed = false;
                    double [] max_min = new double [2];
                    List<Case> Sarray = Db.get_cases_condition(Tablename_Standardization, CaseName);
                    Case_Max_Mean = Db.get_minmax_meanstd((Case)ArrayCases[0],Tablename_Standardization, -2);
                    Case_Min_SD = Db.get_minmax_meanstd((Case)ArrayCases[0], Tablename_Standardization, -3);
                    // من أجل كل عمود يتم المقارنة مع البيانات المقيسة وتعديلها بعد المقارنة
                 /*   for (int j = 0; j < updated_case.GetFeatures().Count; j++)
                    {
                        Feature f = (Feature)updated_case.GetFeatures()[j];
                        Feature maxf = (Feature)Case_Max_Mean.GetFeatures()[j]; 
                        Feature minf = (Feature)Case_Min_SD.GetFeatures()[j];

                        //min max مقارنة قيمة كل عمود في بيانات المصفوفة المقيسة وتعديل  
                        if (( (f.GetFeatureType() == 4) || (f.GetFeatureType() == 2)) && (f.GetFeatureName() != "id") && (f.GetFeatureName() != "cluster"))
                        if (Convert.ToDouble(f.GetFeatureValue()) > Convert.ToDouble(maxf.GetFeatureValue()) || Convert.ToDouble(f.GetFeatureValue()) < Convert.ToDouble(minf.GetFeatureValue()))
                        {
                            changed = true;
                            if (Convert.ToDouble(f.GetFeatureValue()) > Convert.ToDouble(maxf.GetFeatureValue()))
                            maxf.SetFeatureValue(f.GetFeatureValue());
                            else if ( Convert.ToDouble(f.GetFeatureValue()) < Convert.ToDouble(minf.GetFeatureValue()))
                                minf.SetFeatureValue(f.GetFeatureValue());
                            foreach (Case c in Sarray)// تعديل على البيانات المقيسة
                            {
                                Feature cf = (Feature)c.GetFeatures()[j];
                                if (cf.GetFeatureName() == "id")
                                    break;
                                else if (cf.GetFeatureType() == FeatureType.TYPE_FEATURE_INT)
                                {
                                    int ivalue = (Convert.ToInt32(cf.GetFeatureValue()) * (Convert.ToInt32(maxf.GetFeatureValue()) - Convert.ToInt32(minf.GetFeatureValue()))) + Convert.ToInt32(minf.GetFeatureValue());
                                    cf.SetFeatureValue(ivalue);
                                }
                                else if (cf.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT)
                                {
                                    double dvalue = (Convert.ToDouble(cf.GetFeatureValue()) * (Convert.ToDouble(maxf.GetFeatureValue()) - Convert.ToDouble(minf.GetFeatureValue()))) + Convert.ToDouble(minf.GetFeatureValue());
                                    cf.SetFeatureValue(dvalue);
                                }
                            } // enf foreach
                        } // end if 
                    } // end for*/
                        // هنا سيدخل بعد التعديل او في حال لم يعدل
                        // اضف الحالة الجديدة بعد تقييسها وارجع المصفوفة النهائية
                    Case s_updated_case = StandardizeCase(updated_case, Case_Max_Mean, Case_Min_SD, Standrize_Type);  
                    Sarray.Add(s_updated_case);
                    Db.insert_case_to_table_without_id(updated_case, TableName);// تخزين العنصر الجديد في الجدول الأصلي
                    //  min max تخزين كل البيانات المقيسة و
                    if (changed)
                    {
                        Db.del_data_table(Tablename_Standardization);
                        Db.insert_data_to_standrize_caseTable_noID(Tablename_Standardization, get_rows_as_string(Sarray));//تخزين كل البيانات المقيسة
                        Db.insert_case_to_table(Case_Max_Mean, Tablename_Standardization);//تخزين المتوسط
                        Db.insert_case_to_table(Case_Min_SD, Tablename_Standardization);//تخزين الانحراف المعياري 

                    }
                    else
                        Db.insert_case_to_table(s_updated_case, Tablename_Standardization);
                    return Sarray;
                    }
            return CasesData(ArrayCases);
        }

    }

}
