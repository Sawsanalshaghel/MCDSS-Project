using System;


	using System.IO;
	using System.Collections;

	using System.Reflection;

	/// <summary>
	/// CBRContextManager 的摘要说明。
	/// </summary>
	public class CBRContextManager
	{
		private static Hashtable _ctxs = new Hashtable();

		public  static void Load(string engineName, string env)
		{
			bool isExist = File.Exists(env);
			if (isExist)
			{
				ICBRContext ctx = CBRContextFactory.newInstance();
				if (ctx != null)
				{
					
					CreateCBRContext(ctx, engineName, env);
					_ctxs.Add(engineName, ctx);
				}
			}
			else
			{
				throw new ContextException("context env " + env 
					+ "is not existed");
			}
		}
		public static ICBRContext GetCBRContext(string engineName)
		{
			return (ICBRContext)_ctxs[engineName];
		}
		private static void CreateCBRContext(ICBRContext ctx, string engineName, 
			string env)
		{
			XMLConfigFile f = new XMLConfigFile(env);
			ConfigInfo config = f.GetConfigInfo();
			if (config != null)
			{
				ManagerHelper helper = new ManagerHelper();
				helper.SetEnv(engineName);
				helper.CreateCBRContext(config, ctx);
			}
		}
		public class ManagerHelper
		{

			private string _env = null;
			public void SetEnv(string env)
			{
				_env = env;
			}
			public void CreateCBRContext(ConfigInfo config, ICBRContext ctx)
			{
				//create case retrieve method
				ctx.SetCaseRetrievalMethod((IMethod)CreateInstance(config, 
						MappingKey.KEY_CASE_RETRIEVEL_METHOD));
				//create the computing similarity method
				ctx.SetSimilarity((ISimilarity)CreateInstance(config, 
						MappingKey.KEY_SIMILARITY));
				//create the case reuse method
                ctx.SetCaseReuseMethod((IMethod)CreateInstance(config, 
						MappingKey.KEY_CASE_REUSE_METHOD) );
				//create the strategy of case reuse
				ctx.SetCaseReuseStrategy((ICaseReuseStrategy)CreateInstance(config, 
						MappingKey.KEY_CASE_REUSE_STRATEGY));
				 
				//create the case revise method
				ctx.SetCaseReviseMethod((IMethod)CreateInstance(config, 
							MappingKey.KEY_CASE_REVISE_METHOD));
				 
				//create the case restore method
				ctx.SetCaseRestoreMethod((IMethod)CreateInstance(config, 
							MappingKey.KEY_CASE_RESTORE_METHOD));
				//create the input method of case base
				ctx.SetCaseBaseInput((ICaseBaseInput)CreateInstance(config, 
							MappingKey.KEY_CASEBASE_INPUT));
				//create the case base method
				ctx.SetCaseBase((ICaseBase)CreateInstance(config, 
							MappingKey.KEY_CASEBASE));
				//init the similarity threhold parameter
				string threhold = (string)CreateParameter(config,
					MappingKey.KEY_PARAM_SIMILARITY_THREHOLD);
				if (threhold != null)
					ctx.SetSimilarityThrehold(Convert.ToDouble(threhold));
				//init the case base input type
				string type = (string)CreateParameter(config,
					MappingKey.KEY_PARAM_CASEBASE_INPUT_TYPE);
				if (type != null)
					ctx.SetCaseBaseInputType(Convert.ToInt32(type));
				//init the case base url
				string url = (string)CreateParameter(config,
					MappingKey.KEY_PARAM_CASEBASE_URL);
				ctx.SetCaseBaseURL(url);
			}
			private object CreateParameter(ConfigInfo config,
				string key)
			{
				Hashtable parameter = config.GetParameters();
				
				return parameter[key];
			}
			private object CreateInstance(ConfigInfo config,
				string key)
			{
				
				Hashtable mapping = config.GetMappingEntries();
				Hashtable dllPath = config.GetOuterDLLPaths();
				string impl = (string)mapping[key];
				string path = (string)dllPath[key];

				if (impl == null)
				{
					if (Version.DEBUG)
						System.Console.WriteLine("the method node's field <impl> is null");
					return null;
				}
				if (path != null)
				{
					IEnv e = (IEnv)DynamicCreator.CreateInstance(impl, path);
					e.SetEnv(_env);
					return e;
				}

				Type type = null;
				try
				{
					Assembly assembly =  this.GetType().Assembly;
					type = assembly.GetType(impl);
				}
				catch(Exception e)
				{
					System.Console.WriteLine(e.Message);
					return null;
				}

				IEnv env = (IEnv)DynamicCreator.CreateInstance(type);
				env.SetEnv(_env);

				return env;
			}
		}
		public class MappingKey
		{
			public static readonly string KEY_SIMILARITY = "ISimilarity";
			public static readonly string KEY_CASE_RETRIEVEL_METHOD = "ICaseRetrievalMethod";
			public static readonly string KEY_CASE_REUSE_METHOD = "ICaseReuseMethod";
			public static readonly string KEY_CASE_REVISE_METHOD = "ICaseReviseMethod";
			public static readonly string KEY_CASE_RESTORE_METHOD = "ICaseRestoreMethod";
			public static readonly string KEY_CASE_REUSE_STRATEGY = "ICaseReuseStrategy";
			public static readonly string KEY_CASEBASE_INPUT = "ICaseBaseInput";
			public static readonly string KEY_CASEBASE = "ICaseBase";
			public static readonly string KEY_PARAM_CASEBASE_INPUT_TYPE = "CaseBaseInputType";
			public static readonly string KEY_PARAM_SIMILARITY_THREHOLD = "SimilarityThrehold";
			public static readonly string KEY_PARAM_CASEBASE_URL = "CaseBaseURL";
		}
	}

