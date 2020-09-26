using System;



	using System.Collections;

	/// <summary>
	/// DefaultRetrieval 的摘要说明。
	/// </summary>
	public class DefaultCaseRetrieval:ICaseRetrieval
	{
		public DefaultCaseRetrieval()
		{
		}
		#region ICaseRetrieval 成员

		private string _env = null;
		public void SetEnv(string env)
		{
			_env = env;
		}
		public string GetEnv()
		{
			return _env;
		}

		public ArrayList RetrievalCases(Case problem)
		{
			if (_env == null)
			{
				throw new ContextException("environment variable is not set");
			}
			ICBRContext ctx = CBRContextManager.GetCBRContext(_env);
			if (ctx == null)
			{
				throw new ContextException("context is not set");
			}

			IMethod m = ctx.GetCaseRetrievalMethod();
			if (m == null)
			{
				throw new ContextException(
							"context's GetCaseRetrievalMethod is not set");
			}
			ctx.SetMatchedCase((ArrayList)m.Execute(problem));

			return (ArrayList)m.Execute(problem);
		}

        public ArrayList RetrievalCases_partial(Case problem, ArrayList testingcases)
        {
            if (_env == null)
            {
                throw new ContextException("environment variable is not set");
            }
            ICBRContext ctx = CBRContextManager.GetCBRContext(_env);
            if (ctx == null)
            {
                throw new ContextException("context is not set");
            }

            IMethod m = ctx.GetCaseRetrievalMethod();
            if (m == null)
            {
                throw new ContextException(
                            "context's GetCaseRetrievalMethod is not set");
            }
            ctx.SetMatchedCase((ArrayList)m.Execute_partial(problem, testingcases));

            return (ArrayList)m.Execute_partial(problem, testingcases);
        }
		#endregion
	}
