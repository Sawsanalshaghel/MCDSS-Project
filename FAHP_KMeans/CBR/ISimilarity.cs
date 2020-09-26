using System;


	/// <summary>
	/// ISimilarity 的摘要说明。
	/// </summary>
	public interface ISimilarity:IEnv
	{
        double Similarity(Case problem, Case solution);
	}

