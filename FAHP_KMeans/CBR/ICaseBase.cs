using System;


	using System.Collections;

	/// <summary>
	/// ICaseBase 的摘要说明。
	/// </summary>
	public interface ICaseBase:IEnv
	{
		ArrayList GetCases(Case c);
		void RestoreCase(Case c);
	}

