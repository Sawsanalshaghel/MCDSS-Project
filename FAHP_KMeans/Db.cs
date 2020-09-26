using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;
namespace Fahp_cbr_app
{
    class Db
    {
       

       // public static string constr = @"Data Source=.\sqldb;AttachDbFilename=" + Application.StartupPath + @"\StringDb.mdf;Integrated Security=True";
        public static string constr = @" Data Source=.;Initial Catalog=PhoneDB;Integrated Security=True";

        public static SqlConnection myconnection = new SqlConnection(constr);
        //--------------------------------------------------------------------------
        public static string[] get_rows_as_string(List<Case> myarr)
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

        //---------------------------------------------------------------------
        public static string get_features_WithTypes(Case b )
        {
            Feature ff = null;
            string features = "";

            // make case features as columns and types for insert statment
            for (int j = 0; j < b.GetFeatures().Count; j++)
            {

                ff = (Feature)b.GetFeatures()[j];
                if (j != (b.GetFeatures().Count - 1))
                    features += ff.GetFeatureName() + "  " + Token.GetMsSqlType(ff.GetFeatureType()) + ",";
                   

                else
                    features += ff.GetFeatureName() + "  " + Token.GetMsSqlType(ff.GetFeatureType());

            }
            return features;
        }
        //---------------------------------------------------------------------------

        public static string get_features_WithOutTypes(Case b)
        {

            Feature ff = null;

            string rawfeatures = "";

            // make case features as columns and types for insert statment
            for (int j = 0; j < b.GetFeatures().Count; j++)
            {

                ff = (Feature)b.GetFeatures()[j];
                if (j != (b.GetFeatures().Count - 1))

                    rawfeatures += ff.GetFeatureName() + "  ,";
                else
                    rawfeatures += ff.GetFeatureName();

            }
            return rawfeatures;
        }

        //----------------------------------------------------------------------------
        public static string get_sol_ids(string tablename, string con)//, out int count)
        {
           
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where " +con;
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt_data = new DataTable();
            sd.Fill(dt_data);
            myconnection.Close();

            string ids = "";
            List<Case> myarr = new List<Case>();
            if (dt_data.Rows.Count > 0)
            {
                ids = "(";
                for (int i = 0; i < dt_data.Rows.Count - 1; i++)
                    ids += dt_data.Rows[i]["id"].ToString() + ",";
                string y = dt_data.Rows[dt_data.Rows.Count - 1]["id"].ToString();
                ids += y + " )";
            }
           // count = dt_data.Rows.Count;
            return ids;
        }
        //-----------------------------------------------------------------------------
        public static void set_xml(string sim, string casename,string tablename)
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
            value = new XAttribute("value", "$CaseID=" + casename + "* $DbType=Mssql *  $DataSource=Data Source=.;Initial Catalog=PhoneDB;Integrated Security=True * $DictionaryTable=tbl_dic * $DataTable=" + tablename);
            parameters_node.Add(name);
            parameters_node.Add(value);
            parameters.Add(parameters_node);
            file.Add(parameters);
            file.Save(path);
        }
        public static List<Case> read_file()
        {
                List<Case> ListCases=new List<Case>();
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
                    ListCases = myfile.GetListCases();
                 
                } // end if
              
                return ListCases;
        }
        public static List<Case> get_cases(string tablename, Case _inputcase)
        {
            List<Case> current_cases = new List<Case>();
            SqlCommand cmd = new SqlCommand("select * from " + tablename, myconnection);
            myconnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Case mycase = new Case(1, _inputcase.GetCaseName(), "selecting");
                mycase.AddFeature("id", Token.GetType("int"), reader["id"].ToString().Trim(), 0, false, false,"num");
                for (int j = 0; j < _inputcase.GetFeatures().Count; j++)
                {
                    Feature f = (Feature)_inputcase.GetFeatures()[j];
                    mycase.AddFeature(f.GetFeatureName(), f.GetFeatureType(), reader[f.GetFeatureName()].ToString().Trim(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                }
                current_cases.Add(mycase);
            }
            reader.Close();
            myconnection.Close();
            return current_cases;
        }

        public static List<Case> get_cases_condition(string tablename, string casename)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where cluster not in (-1,-2,-3)";
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt_data = new DataTable();
            sd.Fill(dt_data);
            myconnection.Close();

            DataTable dt_dif = get_from_dic(casename);
            List<Case> myarr = new List<Case>();
            for (int i = 0; i < dt_data.Rows.Count; i++)
            {
                Case c = new Case(0, casename, "");
                c.AddFeature("id", Token.GetType("int"), dt_data.Rows[i]["id"], 0, false, false, "num");
                for (int j = 0; j < dt_dif.Rows.Count; j++)
                {
                    string name = dt_dif.Rows[j]["FeatureName"].ToString().Trim();
                    int type = Convert.ToInt32(Token.GetType(dt_dif.Rows[j]["FeatureType"].ToString().Trim()));
                    double weight = Convert.ToDouble(dt_dif.Rows[j]["FeatureWeight"].ToString().Trim());
                    bool index = Convert.ToBoolean(dt_dif.Rows[j]["FeatureIndex"].ToString().Trim());
                    bool key = Convert.ToBoolean(dt_dif.Rows[j]["FeatureKey"].ToString().Trim());
                    string unit = dt_dif.Rows[j]["FeatureUnit"].ToString().Trim();
                    c.AddFeature(name, type, dt_data.Rows[i][j + 1].ToString(), weight, key, index, unit);
                }
                c.AddFeature("cluster", Token.GetType("int"), dt_data.Rows[i]["cluster"], 0, false, false, "num");
                myarr.Add(c);
            }
            return myarr;
        }

        //---------------------------------------------------------------------
        public static List<Case> get_cases_ids(string tablename, string casename,string ids)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where cluster not in (-1,-2,-3) and id in " + ids ;
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt_data = new DataTable();
            sd.Fill(dt_data);
            myconnection.Close();

            DataTable dt_dif = get_from_dic(casename);
            List<Case> myarr = new List<Case>();
            for (int i = 0; i < dt_data.Rows.Count; i++)
            {
                Case c = new Case(0, casename, "");
                c.AddFeature("id", Token.GetType("int"), dt_data.Rows[i]["id"], 0, false, false, "num");
                for (int j = 0; j < dt_dif.Rows.Count; j++)
                {
                    string name = dt_dif.Rows[j]["FeatureName"].ToString().Trim();
                    int type = Convert.ToInt32(Token.GetType(dt_dif.Rows[j]["FeatureType"].ToString().Trim()));
                    double weight = Convert.ToDouble(dt_dif.Rows[j]["FeatureWeight"].ToString().Trim());
                    bool index = Convert.ToBoolean(dt_dif.Rows[j]["FeatureIndex"].ToString().Trim());
                    bool key = Convert.ToBoolean(dt_dif.Rows[j]["FeatureKey"].ToString().Trim());
                    string unit = dt_dif.Rows[j]["FeatureUnit"].ToString().Trim();
                    c.AddFeature(name, type, dt_data.Rows[i][j + 1].ToString(), weight, key, index, unit);
                }
                c.AddFeature("cluster", Token.GetType("int"), dt_data.Rows[i]["cluster"], 0, false, false, "num");
                myarr.Add(c);
            }
            return myarr;
        }


        public static List<Case> get_cases_condition2(string tablename, string casename, string condition, string conditionvalue)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where "+ condition+" = " +conditionvalue  ;
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt_data = new DataTable();
            sd.Fill(dt_data);
            myconnection.Close();

            DataTable dt_dif = get_from_dic(casename);
            List<Case> myarr = new List<Case>();
            for (int i = 0; i < dt_data.Rows.Count; i++)
            {
                Case c = new Case(0, casename, "");
                c.AddFeature("id", Token.GetType("int"), dt_data.Rows[i]["id"], 0, false, false, "num");
                for (int j = 0; j < dt_dif.Rows.Count; j++)
                {
                    string name = dt_dif.Rows[j]["FeatureName"].ToString().Trim();
                    int type = Convert.ToInt32(Token.GetType(dt_dif.Rows[j]["FeatureType"].ToString().Trim()));
                    double weight = Convert.ToDouble(dt_dif.Rows[j]["FeatureWeight"].ToString().Trim());
                    bool index = Convert.ToBoolean(dt_dif.Rows[j]["FeatureIndex"].ToString().Trim());
                    bool key = Convert.ToBoolean(dt_dif.Rows[j]["FeatureKey"].ToString().Trim());
                    string unit = dt_dif.Rows[j]["FeatureUnit"].ToString().Trim();
                    c.AddFeature(name, type, dt_data.Rows[i][j + 1].ToString(), weight, key, index, unit);
                }

                c.AddFeature("cluster", Token.GetType("int"), dt_data.Rows[i]["cluster"], 0, false, false, "num");
                myarr.Add(c);
            }
            return myarr;
        }

        public static List<Case> get_cases_Customized_condition(string tablename, string casename, string condition,  string conditionvalue)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where " + condition +   conditionvalue;
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt_data = new DataTable();
            sd.Fill(dt_data);
            myconnection.Close();

            DataTable dt_dif = get_from_dic(casename);
            List<Case> myarr = new List<Case>();
            for (int i = 0; i < dt_data.Rows.Count; i++)
            {
                Case c = new Case(0, casename, "");
                c.AddFeature("id", Token.GetType("int"), dt_data.Rows[i]["id"], 0, false, false, "num");
                for (int j = 0; j < dt_dif.Rows.Count; j++)
                {
                    string name = dt_dif.Rows[j]["FeatureName"].ToString().Trim();
                    int type = Convert.ToInt32(Token.GetType(dt_dif.Rows[j]["FeatureType"].ToString().Trim()));
                    double weight = Convert.ToDouble(dt_dif.Rows[j]["FeatureWeight"].ToString().Trim());
                    bool index = Convert.ToBoolean(dt_dif.Rows[j]["FeatureIndex"].ToString().Trim());
                    bool key = Convert.ToBoolean(dt_dif.Rows[j]["FeatureKey"].ToString().Trim());
                    string unit = dt_dif.Rows[j]["FeatureUnit"].ToString().Trim();
                    c.AddFeature(name, type, dt_data.Rows[i][j + 1].ToString(), weight, key, index, unit);
                }
                myarr.Add(c);
            }
            
            return myarr;
        }
        public static DataTable get_cases_customized_condition(string tablename, string condition)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where " + condition ;
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt_data = new DataTable();
            sd.Fill(dt_data);
            myconnection.Close();


            return dt_data;
        }


        public static List<Case> get_cases_condition_cluster(string tablename, string casename, string condition, string conditionvalue)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where " + condition + " = " + conditionvalue;
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt_data = new DataTable();
            sd.Fill(dt_data);
            myconnection.Close();

            DataTable dt_dif = get_from_dic(casename);
            List<Case> myarr = new List<Case>();
            for (int i = 0; i < dt_data.Rows.Count; i++)
            {
                Case c = new Case(0, casename, "");
                c.AddFeature("id", Token.GetType("int"), dt_data.Rows[i]["id"], 0, false, false, "num");
                for (int j = 0; j < dt_dif.Rows.Count; j++)
                {
                    string name = dt_dif.Rows[j]["FeatureName"].ToString().Trim();
                    int type = Convert.ToInt32(Token.GetType(dt_dif.Rows[j]["FeatureType"].ToString().Trim()));
                    double weight = Convert.ToDouble(dt_dif.Rows[j]["FeatureWeight"].ToString().Trim());
                    bool index = Convert.ToBoolean(dt_dif.Rows[j]["FeatureIndex"].ToString().Trim());
                    bool key = Convert.ToBoolean(dt_dif.Rows[j]["FeatureKey"].ToString().Trim());
                    string unit = dt_dif.Rows[j]["FeatureUnit"].ToString().Trim();
                    c.AddFeature(name, type, dt_data.Rows[i][j + 1].ToString(), weight, key, index, unit);
                }
                c.AddFeature("cluster", Token.GetType("int"), dt_data.Rows[i]["cluster"], 0, false, false, "num");
                myarr.Add(c);
            }
            return myarr;
        }


        public static ArrayList get_cases_condition_cluster_arraylist(string tablename, string casename, string condition, string conditionvalue)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where " + condition + " = " + conditionvalue;
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt_data = new DataTable();
            sd.Fill(dt_data);
            myconnection.Close();

            DataTable dt_dif = get_from_dic(casename);
            ArrayList myarr = new ArrayList();
            for (int i = 0; i < dt_data.Rows.Count; i++)
            {
                Case c = new Case(0, casename, "");
                c.AddFeature("id", Token.GetType("int"), dt_data.Rows[i]["id"], 0, false, false, "num");
                for (int j = 0; j < dt_dif.Rows.Count; j++)
                {
                    string name = dt_dif.Rows[j]["FeatureName"].ToString().Trim();
                    int type = Convert.ToInt32(Token.GetType(dt_dif.Rows[j]["FeatureType"].ToString().Trim()));
                    double weight = Convert.ToDouble(dt_dif.Rows[j]["FeatureWeight"].ToString().Trim());
                    bool index = Convert.ToBoolean(dt_dif.Rows[j]["FeatureIndex"].ToString().Trim());
                    bool key = Convert.ToBoolean(dt_dif.Rows[j]["FeatureKey"].ToString().Trim());
                    string unit = dt_dif.Rows[j]["FeatureUnit"].ToString().Trim();
                    c.AddFeature(name, type, dt_data.Rows[i][j + 1].ToString(), weight, key, index, unit);
                }
                c.AddFeature("cluster", Token.GetType("int"), dt_data.Rows[i]["cluster"], 0, false, false, "num");
                myarr.Add(c);
            }
            return myarr;
        }

        public static ArrayList get_cases_as_arraylist(string tablename,string casename)
        {
            DataTable dt_data = get_table_contetnt(tablename);
            DataTable dt_dif = get_from_dic(casename);
            ArrayList myarr = new ArrayList();
            for (int i = 0; i < dt_data.Rows.Count; i++)
            {
                Case c = new Case(0, casename, "");
                c.AddFeature("id", Token.GetType("int"), dt_data.Rows[i]["id"], 0, false, false, "num");
                for (int j = 0; j < dt_dif.Rows.Count; j++)
                {
                    string name = dt_dif.Rows[j]["FeatureName"].ToString().Trim();
                    int type = Convert.ToInt32(Token.GetType(dt_dif.Rows[j]["FeatureType"].ToString().Trim()));
                    double weight = Convert.ToDouble(dt_dif.Rows[j]["FeatureWeight"].ToString().Trim());
                    bool index = Convert.ToBoolean(dt_dif.Rows[j]["FeatureIndex"].ToString().Trim());
                    bool key = Convert.ToBoolean(dt_dif.Rows[j]["FeatureKey"].ToString().Trim());
                    string unit = dt_dif.Rows[j]["FeatureUnit"].ToString().Trim();
                    c.AddFeature(name, type, dt_data.Rows[i][j+1].ToString(),weight, key,index, unit);
                }
                myarr.Add(c);
            }
            return myarr;
        }


        public static ArrayList get_cases_as_arraylist_condition(string tablename, string casename)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where cluster not in (-1,-2,-3)";
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt_data = new DataTable();
            sd.Fill(dt_data);
            myconnection.Close();
            
            DataTable dt_dif = get_from_dic(casename);
            ArrayList myarr = new ArrayList();
            for (int i = 0; i < dt_data.Rows.Count; i++)
            {
                Case c = new Case(0, casename, "");
                c.AddFeature("id", Token.GetType("int"), dt_data.Rows[i]["id"], 0, false, false, "num");
                for (int j = 0; j < dt_dif.Rows.Count; j++)
                {
                    string name = dt_dif.Rows[j]["FeatureName"].ToString().Trim();
                    int type = Convert.ToInt32(Token.GetType(dt_dif.Rows[j]["FeatureType"].ToString().Trim()));
                    double weight = Convert.ToDouble(dt_dif.Rows[j]["FeatureWeight"].ToString().Trim());
                    bool index = Convert.ToBoolean(dt_dif.Rows[j]["FeatureIndex"].ToString().Trim());
                    bool key = Convert.ToBoolean(dt_dif.Rows[j]["FeatureKey"].ToString().Trim());
                    string unit = dt_dif.Rows[j]["FeatureUnit"].ToString().Trim();
                    c.AddFeature(name, type, dt_data.Rows[i][j + 1].ToString(), weight, key, index, unit);
                }
                c.AddFeature("cluster", Token.GetType("int"), dt_data.Rows[i]["cluster"], 0, false, false, "num");
                myarr.Add(c);
            }
            return myarr;
        }


        public static ArrayList get_cases_as_arraylist_condition_NoCluster(string tablename, string casename)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where cluster not in (-1,-2,-3)";
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt_data = new DataTable();
            sd.Fill(dt_data);
            myconnection.Close();

            DataTable dt_dif = get_from_dic(casename);
            ArrayList myarr = new ArrayList();
            for (int i = 0; i < dt_data.Rows.Count; i++)
            {
                Case c = new Case(0, casename, "");
                c.AddFeature("id", Token.GetType("int"), dt_data.Rows[i]["id"], 0, false, false, "num");
                for (int j = 0; j < dt_dif.Rows.Count; j++)
                {
                    string name = dt_dif.Rows[j]["FeatureName"].ToString().Trim();
                    int type = Convert.ToInt32(Token.GetType(dt_dif.Rows[j]["FeatureType"].ToString().Trim()));
                    double weight = Convert.ToDouble(dt_dif.Rows[j]["FeatureWeight"].ToString().Trim());
                    bool index = Convert.ToBoolean(dt_dif.Rows[j]["FeatureIndex"].ToString().Trim());
                    bool key = Convert.ToBoolean(dt_dif.Rows[j]["FeatureKey"].ToString().Trim());
                    string unit = dt_dif.Rows[j]["FeatureUnit"].ToString().Trim();
                    c.AddFeature(name, type, dt_data.Rows[i][j + 1].ToString(), weight, key, index, unit);
                }
           
                myarr.Add(c);
            }
            return myarr;
        }

        public static List<string> get_criteria(string casename)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            casename = casename.Trim();
            string select_name = "select FeatureName from tbl_dic where caseid = '" + casename + "'";
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt_dif = new DataTable();
            sd.Fill(dt_dif);
            List<string> myarr = new List<string>();
            for (int j = 0; j < dt_dif.Rows.Count; j++)
                myarr.Add(dt_dif.Rows[j]["FeatureName"].ToString().Trim());
            myconnection.Close();
            return myarr;
        }

        public static double[,] get_criteria_values(string tablename, string colname)
        {
          
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();

            string select_name = "select id," + colname + " from " + tablename + " where cluster not in  (-1,-2,-3)";
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt_dif = new DataTable();
            sd.Fill(dt_dif);
            double[,] criteria_values = new double[dt_dif.Rows.Count, 2];


            for (int j = 0; j < dt_dif.Rows.Count; j++)
            {
                criteria_values[j, 0] = Convert.ToDouble(dt_dif.Rows[j]["id"]);
                criteria_values[j, 0] =Convert.ToDouble( dt_dif.Rows[j][colname]);
            }
                
            myconnection.Close();
            return criteria_values;
        }
        public static string get_criteria_values_string(string feature,string tablename)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            
            string select_name = "select distinct " + feature + " from " + tablename;
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt_dif = new DataTable();
            sd.Fill(dt_dif);
            string values = "";
            for (int j = 0; j < dt_dif.Rows.Count; j++)
                values += dt_dif.Rows[j][feature].ToString().Trim()+"," ;
            myconnection.Close();
            return values;
        }

        public static string [,] get_criteria_type(string casename)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            casename = casename.Trim();
            string select_name = "select FeatureName,FeatureType from tbl_dic where caseid = '" + casename + "'";
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt_dif = new DataTable();
            sd.Fill(dt_dif);
            string[,] myarr = new string [dt_dif.Rows.Count,2];
            for (int j = 0; j < dt_dif.Rows.Count; j++)
            { myarr[j,0]=dt_dif.Rows[j]["FeatureName"].ToString().Trim();
             myarr[j,1]=dt_dif.Rows[j]["FeatureType"].ToString().Trim();}
            myconnection.Close();
            return myarr;
        }
        public static DataTable  get_table_contetnt (string tablename)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename;
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            myconnection.Close();
            return dt;
        }

        public static DataTable get_phone_table_contetnt()
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from  tbl_cases where case_name like '%phone%'";
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            myconnection.Close();
            return dt;
        }

        public static string get_standriazation( string tablename)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select standrize_table from  tbl_cases where tbl_name = '" + tablename + "'";
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            string val =cmd.ExecuteScalar().ToString();
            myconnection.Close();
            return val;
        }

        public static string get_standriazation_type(string tablename)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select standrize_type from  tbl_cases where tbl_name = '" + tablename + "'";
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            string val = cmd.ExecuteScalar().ToString();
            myconnection.Close();
            return val;
        }


        public static DataTable get_table_contetnt_condition(string tablename, string condition_col, string condition_val)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where  " + condition_col + " = '" + condition_val+"'";
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            myconnection.Close();
            return dt;
        }

        public static DataTable get_table_contetnt_condition_in(string tablename, string condition_col, string condition_val)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where  " + condition_col + " in " + condition_val;
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            myconnection.Close();
            return dt;
        }
        

        public static DataTable  get_all_cases (string tablename)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where cluster != -1";
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            myconnection.Close();
            return dt;
        }


        public static Case get_Case(Case _inputcase, string tablename, int cluster)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where cluster = " + cluster;
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sd.Fill(dt);

            Case c = new Case(0, _inputcase.GetCaseName(), _inputcase.GetCaseDescription());

            for (int j = 0; j < _inputcase.GetFeatures().Count; j++)
            {
                Feature f = (Feature)_inputcase.GetFeatures()[j];
                c.AddFeature(f.GetFeatureName(), f.GetFeatureType(), dt.Rows[0][f.GetFeatureName()].ToString().Trim(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
            }
            myconnection.Close();
            return c;
        }
        public static Case get_Case(string casename,string tablename, string id)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where cluster not in (-1,-2,-3) and id ="+id;
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt_data = new DataTable();
            sd.Fill(dt_data);

            DataTable dt_dif = get_from_dic(casename);
            ArrayList myarr = new ArrayList();
            Case c = new Case(0, casename, "");
            for (int i = 0; i < dt_data.Rows.Count; i++)
            {
              
                c.AddFeature("id", Token.GetType("int"), dt_data.Rows[i]["id"], 0, false, false, "num");
                for (int j = 0; j < dt_dif.Rows.Count; j++)
                {
                    string name = dt_dif.Rows[j]["FeatureName"].ToString().Trim();
                    int type = Convert.ToInt32(Token.GetType(dt_dif.Rows[j]["FeatureType"].ToString().Trim()));
                    double weight = Convert.ToDouble(dt_dif.Rows[j]["FeatureWeight"].ToString().Trim());
                    bool index = Convert.ToBoolean(dt_dif.Rows[j]["FeatureIndex"].ToString().Trim());
                    bool key = Convert.ToBoolean(dt_dif.Rows[j]["FeatureKey"].ToString().Trim());
                    string unit = dt_dif.Rows[j]["FeatureUnit"].ToString().Trim();
                    c.AddFeature(name, type, dt_data.Rows[i][j + 1].ToString(), weight, key, index, unit);
                }
                c.AddFeature("cluster", Token.GetType("int"), dt_data.Rows[i]["cluster"], 0, false, false, "num");
            }

            myconnection.Close();
            return c;
        }

        public static Case get_Case_data( string tablename, int cluster)
        {

            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where cluster = " + cluster;
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sd.Fill(dt);

           
            string d = tablename.Substring(6);
            DataTable dt_dif = get_from_dic(d);
            Case c = new Case(0, tablename.Substring(6), "");
            c.AddFeature("id", Token.GetType("int"), dt.Rows[0]["id"], 0, false, false, "num");
            for (int j = 0; j < dt_dif.Rows.Count; j++)
            {
                string name = dt_dif.Rows[j]["FeatureName"].ToString().Trim();
                int type = Convert.ToInt32(Token.GetType(dt_dif.Rows[j]["FeatureType"].ToString().Trim()));
                double weight = Convert.ToDouble(dt_dif.Rows[j]["FeatureWeight"].ToString().Trim());
                bool index = Convert.ToBoolean(dt_dif.Rows[j]["FeatureIndex"].ToString().Trim());
                bool key = Convert.ToBoolean(dt_dif.Rows[j]["FeatureKey"].ToString().Trim());
                string unit = dt_dif.Rows[j]["FeatureUnit"].ToString().Trim();
                c.AddFeature(name, type, dt.Rows[0][j + 1].ToString(), weight, key, index, unit);
            }
            c.AddFeature("cluster", Token.GetType("int"), dt.Rows[0]["cluster"], 0, false, false, "num");

                myconnection.Close();
               return c;

 
        }

        public static Case get_minmax_meanstd(Case _inputcase, string tablename, int cluster)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where cluster = " + cluster;
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sd.Fill(dt);

            Case c = new Case(0, _inputcase.GetCaseName(), _inputcase.GetCaseDescription());

            for (int j = 0; j < _inputcase.GetFeatures().Count; j++)
            {
                Feature f = (Feature)_inputcase.GetFeatures()[j];
                c.AddFeature(f.GetFeatureName(), f.GetFeatureType(), dt.Rows[0][f.GetFeatureName()].ToString().Trim(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
            }
            myconnection.Close();
            return c;
        }

        public static List<Case> get_Centroids(string tablename, string casename)
        {
            List<Case> myarr = new List<Case>();
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where cluster =-1";
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt_data = new DataTable();
            sd.Fill(dt_data);
            myconnection.Close();
            
            DataTable dt_dif = get_from_dic(casename);
            for (int i = 0; i < dt_data.Rows.Count; i++)
            {
                Case c = new Case(0, casename, "");
                c.AddFeature("id", Token.GetType("int"), dt_data.Rows[i]["id"], 0, false, false, "num");
                for (int j = 0; j < dt_dif.Rows.Count; j++)
                {
                    string name = dt_dif.Rows[j]["FeatureName"].ToString().Trim();
                    int type = Convert.ToInt32(Token.GetType(dt_dif.Rows[j]["FeatureType"].ToString().Trim()));
                    double weight = Convert.ToDouble(dt_dif.Rows[j]["FeatureWeight"].ToString().Trim());
                    bool index = Convert.ToBoolean(dt_dif.Rows[j]["FeatureIndex"].ToString().Trim());
                    bool key = Convert.ToBoolean(dt_dif.Rows[j]["FeatureKey"].ToString().Trim());
                    string unit = dt_dif.Rows[j]["FeatureUnit"].ToString().Trim();
                    c.AddFeature(name, type, dt_data.Rows[i][j + 1].ToString(), weight, key, index, unit);
                }
                c.AddFeature("cluster", Token.GetType("int"), dt_data.Rows[i]["cluster"], 0, false, false, "num");
                myarr.Add(c);
            }
            return myarr;
        }

        public static List<Case> get_all_cases(string tablename, string casename)
        {
            List<Case> myarr = new List<Case>();
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + " where cluster not in (-1,-2,-3)";
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt_data = new DataTable();
            sd.Fill(dt_data);
            myconnection.Close();

            DataTable dt_dif = get_from_dic(casename);
            for (int i = 0; i < dt_data.Rows.Count; i++)
            {
                Case c = new Case(0, casename, "");
                c.AddFeature("id", Token.GetType("int"), dt_data.Rows[i]["id"], 0, false, false, "num");
                for (int j = 0; j < dt_dif.Rows.Count; j++)
                {
                    string name = dt_dif.Rows[j]["FeatureName"].ToString().Trim();
                    int type = Convert.ToInt32(Token.GetType(dt_dif.Rows[j]["FeatureType"].ToString().Trim()));
                    double weight = Convert.ToDouble(dt_dif.Rows[j]["FeatureWeight"].ToString().Trim());
                    bool index = Convert.ToBoolean(dt_dif.Rows[j]["FeatureIndex"].ToString().Trim());
                    bool key = Convert.ToBoolean(dt_dif.Rows[j]["FeatureKey"].ToString().Trim());
                    string unit = dt_dif.Rows[j]["FeatureUnit"].ToString().Trim();
                    c.AddFeature(name, type, dt_data.Rows[i][j + 1].ToString(), weight, key, index, unit);
                }
                c.AddFeature("cluster", Token.GetType("int"), dt_data.Rows[i]["cluster"], 0, false, false, "num");
                myarr.Add(c);
            }
            return myarr;
        }

        public static DataTable get_all_orderd(string tablename,string order)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from " + tablename + "  where cluster != -1 order by " + order;
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            myconnection.Close();
            return dt;
        }

  
        public static DataTable get_from_dic(string casename)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select * from  tbl_dic where caseid ='" + casename + "'";
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            myconnection.Close();
            return dt;
        }

        public static double[] Get_Max_Min(string tablename, string columnname)
        {
            double[] minmax = new double[2];
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string getmax_min = " select max ("+columnname+") from "+ tablename ;
            SqlCommand cmd = new SqlCommand(getmax_min, myconnection);
            double max =  Convert.ToDouble(cmd.ExecuteScalar());
            getmax_min = " select min (" + columnname + ") from " + tablename;
            cmd = new SqlCommand(getmax_min, myconnection);
            double min = Convert.ToDouble(cmd.ExecuteScalar());
            minmax[0] = max;
            minmax[1] = min;
            return minmax;
        }

        public static double Get_CostFunction( string case_name)
        {
            double[] minmax = new double[2];
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string get = " select  cost_function from  tbl_cases where tbl_name like '" + case_name + "'" ;
            SqlCommand cmd = new SqlCommand(get, myconnection);
            double d=Convert.ToDouble(cmd.ExecuteScalar());
            myconnection.Close();
            return d;
        }

        public static double Get_Max(string tablename, string columnname)
        {
            double[] minmax = new double[2];
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string getmax_min = " select max (" + columnname + ") from " + tablename;
            SqlCommand cmd = new SqlCommand(getmax_min, myconnection);
            double max = Convert.ToDouble(cmd.ExecuteScalar());
            return max;
        }

        public static int Get_RowsCount(string tablename)
        {
            double[] minmax = new double[2];
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string getmax_min = " select count (id) from " + tablename + " where cluster not in (-1,-2,-3) ";
            SqlCommand cmd = new SqlCommand(getmax_min, myconnection);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count;
        }
        public static int Get_Last_Added_Id(string tablename)
        {
            double[] minmax = new double[2];
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string getmax_min = " SELECT IDENT_CURRENT('"+tablename+"')";
            SqlCommand cmd = new SqlCommand(getmax_min, myconnection);
            int max = Convert.ToInt32(cmd.ExecuteScalar());
            return max;
        }



        public static void update_first(string tablename, string vid, int cluster)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string iscluster = " select count(COLUMN_Name) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + tablename + "' and COLUMN_Name = 'cluster'";
            SqlCommand cmd = new SqlCommand(iscluster, myconnection);
            int count = (int)cmd.ExecuteScalar();
            if (count == 0)
            {
                string insert = " ALTER TABLE dbo. " + tablename + " ADD  cluster int NULL";
                cmd = new SqlCommand(insert, myconnection);
                cmd.ExecuteNonQuery();
            }
            string update_name = "update  " + tablename + " set cluster  =  " + cluster + " where id = " + vid;
            cmd = new SqlCommand(update_name, myconnection);
            cmd.ExecuteNonQuery();
            myconnection.Close();

        }
        public static void update_cluster(string tablename, string vid, int cluster)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string iscluster = " select count(COLUMN_Name) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + tablename + "' and COLUMN_Name = 'cluster'";
            SqlCommand cmd = new SqlCommand(iscluster, myconnection);
            int count =Convert.ToInt32( cmd.ExecuteScalar());
            if (count == 0)
            {
                string insert = " ALTER TABLE dbo. " + tablename + "ADD column cluster int NULL";
                cmd = new SqlCommand(insert, myconnection);
                cmd.ExecuteNonQuery();
            }
            string  update_name = "update  "+ tablename + " set cluster  =  " + cluster + " where id = " + vid;
             cmd = new SqlCommand(update_name, myconnection);
            cmd.ExecuteNonQuery();
            myconnection.Close();
            
        }


        public static void update_clusterNum(string Case_name, int clusters_num)
        {
          if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
          myconnection.Open();
          string update_name = "update tbl_cases set clusters_num  =  " + clusters_num + " where Case_name = '" + Case_name+"'";
          SqlCommand cmd = new SqlCommand(update_name, myconnection);
          cmd.ExecuteNonQuery();
          myconnection.Close();

        }


        public static DataTable get_count_cluster(string tablename)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string select_name = "select count (cluster), cluster from  " + tablename + " where cluster != -1   group by cluster";
            SqlCommand cmd = new SqlCommand(select_name, myconnection);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            myconnection.Close();
            return dt;
        }

        public static void add_centroid(string tablename, Case centroidcase, int t)
        {
            string columns = ""; string values = "";
            for (int i = 1; i < centroidcase.GetFeatures().Count; i++)
            { 
                Feature f= ( Feature ) centroidcase.GetFeatures()[i];
                columns += f.GetFeatureName() + ",";
                if ((f.GetFeatureType() == 5) || (f.GetFeatureType() == 6) || (f.GetFeatureType() == 8))
                values += "'"+f.GetFeatureValue() + "',";
                else
                    if (f.GetFeatureType() == 1)
                    {
                        if (f.GetFeatureValue().ToString() == "True")
                            values += 1 + ",";
                        else
                            values += 0 + ",";

                    }
                    else
                    values += f.GetFeatureValue() + ",";
            } 
            
            string insert_state = "insert into " + tablename + "(id," + columns + "cluster) values ("+t.ToString()+"," + values + "-1)";

            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            SqlCommand cmd = new SqlCommand(insert_state, myconnection);
            cmd.ExecuteNonQuery();
            myconnection.Close();

        }



        public static void update_centroid(string tablename, Case centroidcase, int id)
        {
            string  [] columns =  new string [centroidcase.GetFeatures().Count];
            string  [] values = new string[centroidcase.GetFeatures().Count];
            string data = "";
            for (int i = 1; i < centroidcase.GetFeatures().Count; i++)
            {
                Feature f = (Feature)centroidcase.GetFeatures()[i];
                columns [i]= f.GetFeatureName() ;
                if ((f.GetFeatureType() == 5) || (f.GetFeatureType() == 6) || (f.GetFeatureType() == 8))
                    values[i] = "'" + f.GetFeatureValue() + "'";
                else
                    if (f.GetFeatureType() == 1)
                    {
                        if (f.GetFeatureValue().ToString() == "True")
                            values[i] = "1" ;
                        else
                            values[i] = "0" ;

                    }
                    else
                        values[i] = f.GetFeatureValue().ToString();
                data += columns[i] + "= " + values[i] + ",";
            }

            data = data.TrimEnd(',');
            string insert_state = "update " + tablename + "  set "+ data + " where id = " + id + " and cluster =-1";

            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            SqlCommand cmd = new SqlCommand(insert_state, myconnection);
            cmd.ExecuteNonQuery();
            myconnection.Close();

        }


        public static void del_data_table(string tablename)
        {

            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string del_name = "delete  from  " + tablename ;
            SqlCommand cmd = new SqlCommand(del_name, myconnection);
            cmd.ExecuteNonQuery();
            myconnection.Close();

        }
   

        public static void del_lastcentroid(string tablename)
        {

            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string del_name = "delete  from  " + tablename + " where cluster =-1";
            SqlCommand cmd = new SqlCommand(del_name, myconnection);
            cmd.ExecuteNonQuery();
            myconnection.Close();
     
        }

        public static void del_centroids(string tablename, List<string> ids)
        {

            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string d = "";
            for (int i = 0; i < ids.Count; i++)
                d += ids[i].ToString() + ",";
            d = "(" + d.TrimEnd(',') + ")";
            string del_name = "delete  from  " + tablename + " where id in" + d +"and cluster =-1";
            SqlCommand cmd = new SqlCommand(del_name, myconnection);
            cmd.ExecuteNonQuery();
            myconnection.Close();

        }

        public static void del_case_id(string tablename, List<string> ids)
        {

            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string d="";
            for (int i = 0; i < ids.Count; i++)
                d += ids[i].ToString() + ",";
            d ="("+ d.TrimEnd(',') + ")";
            string del_name = "delete  from  " + tablename + " where id in" +d;
            SqlCommand cmd = new SqlCommand(del_name, myconnection);
            cmd.ExecuteNonQuery();
            myconnection.Close();

        }

        // del repeated case
        public static  void del_repeated_case(string CaseName)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string checkname = "delete from   tbl_dic where upper(caseid)= upper('" + CaseName + "')";
            SqlCommand cmd = new SqlCommand(checkname, myconnection);
            cmd.ExecuteNonQuery();

            checkname = "delete from   tbl_cases where upper(case_name)= upper('" + CaseName + "')";
             cmd = new SqlCommand(checkname, myconnection);
            cmd.ExecuteNonQuery();
            myconnection.Close();
        }

        // is case exist in tbl_dic
        public static int is_case_exist(string  CaseName)
        {
         if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string checkname = "select count(caseid) from tbl_dic where caseid= upper('"+CaseName+"')";
            SqlCommand cmd = new SqlCommand(checkname, myconnection);
            int count = (int)cmd.ExecuteScalar();
            myconnection.Close();
            return count;
           }
        // insert Case Features as rows to tbl_dic
        public static void insert_case_to_dic(Case a)
        {

            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
           for (int i = 0; i < a.GetFeatures().Count; i++)
           {
                    Feature f = (Feature)a.GetFeatures()[i];
                    if (f.GetFeatureName() != "id" && f.GetFeatureName() != "cluster")
                    {
                        string insert =
                            "insert into tbl_dic (CaseId,FeatureName,FeatureType,FeatureWeight,FeatureKey,FeatureIndex,FeatureUnit) values("
                            + "'" + a.GetCaseName() + "',"
                            + "'" + f.GetFeatureName() + "',"
                             + "'" + Token.GetReverseType(f.GetFeatureType()) + "',"
                            + f.GetWeight() + ","
                            + "0" + ","
                            + "0" + ","
                            + "'" + f.GetFeatureUnit() + "'" + ")";
                        SqlCommand cmd = new SqlCommand(insert, myconnection);
                        cmd.ExecuteNonQuery();
                    }
                }
                myconnection.Close();
        }

        public static void insert_case_to_table(Case a, string tablename)
        {

            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            Feature f=null;
            string feature =" ( ";
            string values = " ( ";
            for (int i = 0; i < a.GetFeatures().Count-1; i++)
            {
                f = (Feature)a.GetFeatures()[i];
                feature += f.GetFeatureName() + ",";
                if ((f.GetFeatureType() == 5) || (f.GetFeatureType() == 6) || (f.GetFeatureType() == 8))
                    values += "'" + f.GetFeatureValue() + "',";
                else
                    if (f.GetFeatureType() == 1)
                    {
                        if (f.GetFeatureValue().ToString() == "True")
                            values += 1 + ",";
                        else
                            values += 0 + ",";

                    }
                    else
                        values += f.GetFeatureValue() + ",";
            }
             f = (Feature)a.GetFeatures()[a.GetFeatures().Count-1];
            feature += f.GetFeatureName() + ")";
            values += f.GetFeatureValue() + ")" ;
            string insert = "insert into " + tablename + feature  + "  values " + values;
            SqlCommand cmd = new SqlCommand(insert, myconnection);
                cmd.ExecuteNonQuery();
            myconnection.Close();
        }


        public static void insert_case_to_table_without_id(Case a, string tablename)
        {

            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            Feature f = null;
         
            string values = " ( ";
            for (int i = 0; i < a.GetFeatures().Count - 1; i++)
            {
                f = (Feature)a.GetFeatures()[i];
               
                if ((f.GetFeatureType() == 5) || (f.GetFeatureType() == 6) || (f.GetFeatureType() == 8))
                    values += "'" + f.GetFeatureValue() + "',";
                else
                    if (f.GetFeatureType() == 1)
                    {
                        if (f.GetFeatureValue().ToString() == "True")
                            values += 1 + ",";
                        else
                            values += 0 + ",";

                    }
                    else
                        values += f.GetFeatureValue() + ",";
            }
            f = (Feature)a.GetFeatures()[a.GetFeatures().Count - 1];
           
            values += f.GetFeatureValue() + ")";
            string insert = "insert into " + tablename +  "  values " + values;
            SqlCommand cmd = new SqlCommand(insert, myconnection);
            cmd.ExecuteNonQuery();
            myconnection.Close();
        }

        public static void create_case_table(string table_name, string features)
        {

           
            // create table for the case

            string create_statmant = " create table " + table_name + "( " + features + ") ";// id int Identity(1,1) primary key ," 
            string drop_statmant = " drop table " + table_name;
            string proc_create = "IF   EXISTS (SELECT * FROM sys.tables WHERE name = N'" + table_name + "' AND type = 'U') " +
                          "BEGIN " + drop_statmant + create_statmant + "END else" + create_statmant;
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            SqlCommand cmd = new SqlCommand(proc_create, myconnection);
            cmd.ExecuteNonQuery();
            myconnection.Close();
        }


        public static void create_standrize_case_table(string table_name, string features)
        {


            // create table for the case

            string create_statmant = " create table " + table_name + "( " + features + ") "; //"( id int ," + features + ",cluster int) ";
            string drop_statmant = " drop table " + table_name;
            string proc_create = "IF   EXISTS (SELECT * FROM sys.tables WHERE name = N'" + table_name + "' AND type = 'U') " +
                          "BEGIN " + drop_statmant + create_statmant + "END else" + create_statmant;
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            SqlCommand cmd = new SqlCommand(proc_create, myconnection);
            cmd.ExecuteNonQuery();
            myconnection.Close();
        }


        public static void insert_into_tblcases(string casename, string table_name, string featurescount, string tablename_standardization, string standrize_type, string kmeans_method, string clusters_num, double cost_function)
        {

            // add table name of the case to casestable

            string add_case = "insert into tbl_cases (case_name,tbl_name,features_num,standrize_table,standrize_type,kmeans_method,clusters_num,cost_function) values ('" + casename + "','" + table_name + "'," + featurescount + ",'" + tablename_standardization + "','" + standrize_type + "','" + kmeans_method + "'," + clusters_num + "," + cost_function + ")";
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            SqlCommand cmd = new SqlCommand(add_case, myconnection);
            cmd.ExecuteNonQuery();
            myconnection.Close();
        }

        public static void insert_data_to_standrize_caseTable(string tablename, string[] rows, string features)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string proc_insert = "";
            SqlCommand cmd;
            for (int i = 0; i < rows.Count(); i++)
            {
                proc_insert = "insert into " + tablename + "(" + features + ")" + " values " + rows[i];
                cmd = new SqlCommand(proc_insert, myconnection);
                cmd.ExecuteNonQuery();
            }
            myconnection.Close();
        }


        public static void insert_data_to_standrize_caseTable_noID(string tablename, string[] rows)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string proc_insert = "";
            SqlCommand cmd;
            for (int i = 0; i < rows.Count(); i++)
            {
                proc_insert = "insert into " + tablename  + " values " + rows[i];
                cmd = new SqlCommand(proc_insert, myconnection);
                cmd.ExecuteNonQuery();
            }
            myconnection.Close();
        }
        public static void insert_data_to_caseTable(string tablename,string[] rows , string features)
        {
            if ((myconnection.State == ConnectionState.Open))
                myconnection.Close();
            myconnection.Open();
            string proc_insert ="";
             SqlCommand cmd;
             for (int i = 0; i< rows.Count(); i++)
             {
                 proc_insert = "insert into " + tablename+"("+features+")" + " values " + rows[i];
                 cmd = new SqlCommand(proc_insert, myconnection);
                 cmd.ExecuteNonQuery();
             }
            myconnection.Close();
        }
    }
}

