using System;


	using System.Collections;

	/// <summary>
	/// ICaseBase ��ժҪ˵����
	/// </summary>
	public interface ICaseBase:IEnv
	{
		ArrayList GetCases(Case c);
		void RestoreCase(Case c);
	}

