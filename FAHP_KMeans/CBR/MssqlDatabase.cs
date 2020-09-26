#region copyright (C) xia wu,pian jin xiang
/************************************************************************************
' Copyright (C) 2005 xia wu,pian jin xiang 
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion
using System;

using System.Collections;
using System.Data.OleDb;
using System.Data.SqlClient;

	
	/// <summary>
	/// MssqlDatabase 
	/// <author>xw_cn@163.com</author>
	/// <version>1.0</version>
	/// <creationdate>2006/0/5</creationdate>
	/// <modificationdate></modificationdate>>
	/// <history></history>
	/// </summary>
	public class MssqlDatabase:IDb
	{
		private DataSource _dataSource = null;
		public MssqlDatabase(DataSource dataSource)
		{
			_dataSource = dataSource;
		}
		public ArrayList GetCases()
		{
            string dictionaryTable = _dataSource.GetDictionaryTable();
            string dataTable = _dataSource.GetDataTable();
            string caseID = _dataSource.GetCaseID();

            return DbHelper.GetCases(_dataSource, dictionaryTable, dataTable, caseID);
		}




        public class DbHelper
        {
            private static readonly string FIELD_FEATURENAME = "FeatureName";
            private static readonly string FIELD_FEATURETYPE = "FeatureType";
            private static readonly string FIELD_KEY = "FeatureKey";
            private static readonly string FIELD_INDEX = "FeatureIndex";
            private static readonly string FIELD_WEIGHT = "FeatureWeight";
            private static readonly string FIELD_CASEID = "CaseID";
            private static readonly string FIELD_FEATUREUNIT = "FeatureUnit";
            private static SqlDataReader ExecuteQuery(SqlCommand cmd, string sql)
            {
                cmd.CommandText = sql;
                SqlDataReader reader = cmd.ExecuteReader();

                return reader;
            }
            public static ArrayList GetCases(DataSource ds,
                string dictionaryTable,
                string dataTable, string caseID)
            {
                string sql = "select * from " + dictionaryTable
                    + "	where " + FIELD_CASEID + "='"
                    + caseID + "'";
                try
                {

                    //read case structure
                    Case c = CaseFactory.CreateInstance(caseID);
                    SqlConnection conn = new SqlConnection(ds.GetConnectionString());
                    SqlCommand cmd = new SqlCommand(sql, conn); 
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string name = reader[FIELD_FEATURENAME].ToString().Trim();
                        string type = reader[FIELD_FEATURETYPE].ToString().Trim();
                        string key = reader[FIELD_KEY].ToString().Trim();
                        string index = reader[FIELD_INDEX].ToString().Trim();
                        string weight = reader[FIELD_WEIGHT].ToString().Trim();
                        string unit = reader[FIELD_FEATUREUNIT].ToString().Trim();
                        
                        c.AddFeature(name, GetType(type), null,
                            Convert.ToDouble(weight), GetBOOL(key), GetBOOL(index), unit);
                    }
                    reader.Close();

                    //read cases
                    ArrayList features = c.GetFeatures();
                    ArrayList cases = new ArrayList();
                    if (ds.GetConditions() != null)
                        sql = "select * from " + dataTable + " where " + FIELD_CASEID + "='"
                            + caseID + "' and " + ds.GetConditions();
                    else
                        sql = "select * from " + dataTable;//+ " where " + FIELD_CASEID + "='" + caseID + "'";
                           

                    reader = ExecuteQuery(cmd, sql);

                    while (reader.Read())
                    {
                        int clusterId = (int)reader["cluster"];
                        if (clusterId != -1)
                        {
                            Case theCase = CaseFactory.CreateInstance(caseID);
                            foreach (Feature f in features)
                            {
                                string featureName = f.GetFeatureName();
                                object featureValue = reader[featureName];
                                theCase.AddFeature(f.GetFeatureName(), f.GetFeatureType(),
                                    featureValue, f.GetWeight(),
                                    f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                            }
                            cases.Add(theCase);
                        }
                    }
                    reader.Close();
                    cmd.Cancel();
                    conn.Close();

                    return cases;

                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return null;
                }

            }
            private static int GetType(string s)
            {
                if (s.Equals(Token.TYPE_BOOL))
                {
                    return FeatureType.TYPE_FEATURE_BOOL;
                }
                else if (s.Equals(Token.TYPE_FLOAT))
                {
                    return FeatureType.TYPE_FEATURE_FLOAT;
                }
                else if (s.Equals(Token.TYPE_IMAGE))
                {
                    return FeatureType.TYPE_FEATURE_IMAGE;
                }
                else if (s.Equals(Token.TYPE_INT))
                {
                    return FeatureType.TYPE_FEATURE_INT;
                }
                else if (s.Equals(Token.TYPE_MSTRING))
                {
                    return FeatureType.TYPE_FEATURE_MSTRING;
                }
                else if (s.Equals(Token.TYPE_STRING))
                {
                    return FeatureType.TYPE_FEATURE_STRING;
                }
                else
                {
                    return FeatureType.TYPE_FEATURE_UNDEFINED;
                }
            }
            private static bool GetBOOL(string s)
            {
                if (s.Equals(Token.TOKEN_TRUE))
                {
                    return true;
                }
                return false;
            }
        }


		public void StoreCases(ArrayList cases)
		{			
		}
		#region IDb 成员

		public void SetEnv(string env)
		{
			// TODO:  添加 MssqlDatabase.SetEnv 实现
		}

		public string GetEnv()
		{
			// TODO:  添加 MssqlDatabase.GetEnv 实现
			return null;
		}

		#endregion
	}

