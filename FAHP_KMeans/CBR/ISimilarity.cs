using System;


	/// <summary>
	/// ISimilarity ��ժҪ˵����
	/// </summary>
	public interface ISimilarity:IEnv
	{
        double Similarity(Case problem, Case solution);
	}

