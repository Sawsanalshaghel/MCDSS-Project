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


	/// <summary>
	/// TypeHandle 
	/// <author>xw_cn@163.com</author>
	/// <version>1.0</version>
	/// <creationdate>2005/11/31</creationdate>
	/// <modificationdate></modificationdate>>
	/// <history></history>
	/// </summary>
	public class TypeHandle
	{
		public static bool IsNumericFeature(Feature f)
		{
			int type = f.GetFeatureType();
			if (type == FeatureType.TYPE_FEATURE_FLOAT
				|| type == FeatureType.TYPE_FEATURE_INT)
			{
				return true;
			}
			return false;
		}
	
        public static double DoType(double f1, double f2)
		{
			return f1 - f2;
		}
		public static double DoType(int f1, int f2)
		{
			double result = f1 - f2;
			return result;
		}
		public static double DoType(bool f1, bool f2)
		{
			if (f1 == f2)
				return 0;
			return 1;
		}
        public static double DoType(string f1, string f2)
        {
            if (f1.ToUpper() == f2.ToUpper())
                return 0;
            return 1;
        }

      
}
