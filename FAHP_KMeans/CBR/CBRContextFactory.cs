using System;


	/// <summary>
	/// CBRContextFactory ��ժҪ˵����
	/// </summary>
	public class CBRContextFactory
	{
		public static ICBRContext newInstance()
		{
			return new DefaultCBRContext();
		}
	}

