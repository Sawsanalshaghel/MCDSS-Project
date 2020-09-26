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
	/// <summary>
	/// EuclideanSimilarity 
	/// <author>xw_cn@163.com</author>
	/// <version>1.0</version>
	/// <creationdate>2005/11/28</creationdate>
	/// <modificationdate></modificationdate>>
	/// <history></history>
	/// </summary>
	public class EuclideanSimilarity:ISimilarity
	{
		protected double _alpha = 0.3;
		public EuclideanSimilarity()
		{
		}
		/// <summary>
		/// set the constant alpha which is a position constant
		/// default value is 1
		/// </summary>
		/// <param name="alpha"></param>
		public void SetAlpha(double alpha)
		{
			if (alpha <= 0)
			{
				//throw exception
			}
			_alpha = alpha;
		}
		#region ISimilarity ³ÉÔ±

		protected string _env = null;
		public void SetEnv(string env){_env = env;}
		public string GetEnv(){return _env;}
		/// <summary>
		/// compute the similarity between problem and solution
		/// throw exception NoSupportTypeException
		/// </summary>
		/// <param name="problem"></param>
		/// <param name="solution"></param>
		/// <returns>the value of similarity</returns>
        public double Similarity(Case problem, Case solution)
		{
			if (problem == null || solution == null)
			{
				return 0;
			}
			double totalSimilarity = 0;
			ArrayList problemFeatures = problem.GetFeatures();
			ArrayList solutionFeatures = solution.GetFeatures();
			if (problemFeatures != null && solutionFeatures != null
				&&problemFeatures.Count > 0 && solutionFeatures.Count > 0
				&&problemFeatures.Count == solutionFeatures.Count)
			{
				int length = problemFeatures.Count;

				for (int i = 0; i < length; i++)
				{
					Feature problemf = (Feature)problemFeatures[i];
					string problemFeatureName = problemf.GetFeatureName();
                    if ((problemFeatureName == "id") || (problemFeatureName == "cluster")) //|| (problemFeatureName == "pid"))
                        continue;
					if (problemFeatureName == null 
						|| problemFeatureName.Length <= 0)
					{
						//throw exception
					}
				//	for (int j = 0; j < length; j++)
					//{
						Feature solutionf = (Feature)solutionFeatures[i];
						string solutionFeatureName = solutionf.GetFeatureName();
						if (solutionFeatureName == null 
							|| solutionFeatureName.Length <= 0)
						{
							//throw exception
						}
						
						//compute the similarity if same feature name and same weight 
						//and not key
						if (problemFeatureName.Equals(solutionFeatureName)
							&& problemf.GetWeight() == solutionf.GetWeight()
							&& problemf.GetIsKey() == false
							&& solutionf.GetIsKey() == false
							&& problemf.GetFeatureType() == solutionf.GetFeatureType()
							)
						{
							double weight = problemf.GetWeight();
							double diff = Math.Pow(weight, 2)
								* Compute(problemf, solutionf);

							if (Version.DEBUG)
								System.Console.WriteLine(problemf.GetFeatureName() 
									+ "'s weight: " + weight 
									+ "\n\tdata: " + problemf.GetFeatureValue()
									+ "\t" + solutionf.GetFeatureValue());

							totalSimilarity += diff;
                            //break;
						}
					//}
				}
			}
			else
			{
				//throw exception
			}
			double distance = Math.Sqrt(totalSimilarity);
          
            double result = 1 / (1 + _alpha * distance);
			if (Version.DEBUG)
			{
				System.Console.WriteLine("similarity is " + result);
				//System.Console.WriteLine("alpha is " + _alpha);
			}
			return result;
		}

        //-------------------------------------------------------------------------------

        public double Dissimilarity(Case problem, Case solution)
		{
			if (problem == null || solution == null)
			{
				return 0;
			}
			double totalSimilarity = 0;
			ArrayList problemFeatures = problem.GetFeatures();
			ArrayList solutionFeatures = solution.GetFeatures();
			if (problemFeatures != null && solutionFeatures != null
				&&problemFeatures.Count > 0 && solutionFeatures.Count > 0
				&&problemFeatures.Count == solutionFeatures.Count)
			{
				int length = problemFeatures.Count;

				for (int i = 0; i < length; i++)
				{
					Feature problemf = (Feature)problemFeatures[i];
					string problemFeatureName = problemf.GetFeatureName();
                    if ((problemFeatureName == "id") || (problemFeatureName == "cluster")) //|| (problemFeatureName == "pid"))
                        continue;
					if (problemFeatureName == null 
						|| problemFeatureName.Length <= 0)
					{
						//throw exception
					}
				//	for (int j = 0; j < length; j++)
					//{
						Feature solutionf = (Feature)solutionFeatures[i];
						string solutionFeatureName = solutionf.GetFeatureName();
						if (solutionFeatureName == null 
							|| solutionFeatureName.Length <= 0)
						{
							//throw exception
						}
						
						//compute the similarity if same feature name and same weight 
						//and not key
						if (problemFeatureName.Equals(solutionFeatureName)
							&& problemf.GetWeight() == solutionf.GetWeight()
							&& problemf.GetIsKey() == false
							&& solutionf.GetIsKey() == false
							&& problemf.GetFeatureType() == solutionf.GetFeatureType()
							)
						{
							double weight = problemf.GetWeight();
							double diff = Math.Pow(weight, 2)
								* Compute(problemf, solutionf);

							if (Version.DEBUG)
								System.Console.WriteLine(problemf.GetFeatureName() 
									+ "'s weight: " + weight 
									+ "\n\tdata: " + problemf.GetFeatureValue()
									+ "\t" + solutionf.GetFeatureValue());

							totalSimilarity += diff;
                            //break;
						}
					//}
				}
			}
			else
			{
				//throw exception
			}
			double distance = Math.Sqrt(totalSimilarity);
          
            return distance;
		}

		
		#endregion

		/// <summary>
		/// return the power of differences between problem feature and solution feature
		/// note that now only support numberic feature type
		/// </summary>
		/// <param name="problem"></param>
		/// <param name="solution"></param>
		/// <returns></returns>
		public virtual double Compute(Feature problem, Feature solution)
		{
			if (problem.GetFeatureType() != solution.GetFeatureType())
			{
				//throw exception
			}

			int featureType = problem.GetFeatureType();
			double diff = 0;
			if (featureType == FeatureType.TYPE_FEATURE_BOOL)
			{
                diff = TypeHandle.DoType((System.Boolean)Convert.ToBoolean(problem.GetFeatureValue()),
					(System.Boolean)Convert.ToBoolean(solution.GetFeatureValue()));
			}
			else if (featureType == FeatureType.TYPE_FEATURE_FLOAT)
			{
				diff = TypeHandle.DoType((System.Double)Convert.ToDouble(problem.GetFeatureValue().ToString()),
					(System.Double)Convert.ToDouble(solution.GetFeatureValue().ToString()));
			}
			else if (featureType == FeatureType.TYPE_FEATURE_IMAGE)
			{
			}
			else if (featureType == FeatureType.TYPE_FEATURE_INT)
			{
				diff = TypeHandle.DoType(
					(System.Int32)Convert.ToInt32(problem.GetFeatureValue().ToString()),
					(System.Int32)Convert.ToInt32(solution.GetFeatureValue().ToString()));
			}
            else if (featureType == FeatureType.TYPE_FEATURE_CATEGORICAL)
            {
                diff = TypeHandle.DoType(
                    problem.GetFeatureValue().ToString(),solution.GetFeatureValue().ToString());
            }
            else if (featureType == FeatureType.TYPE_FEATURE_Ordinal)
            {
                diff = TypeHandle.DoType((System.Double)Convert.ToDouble(problem.GetFeatureValue().ToString()),
                    (System.Double)Convert.ToDouble(solution.GetFeatureValue().ToString()));
                int y=Convert.ToInt32(problem.GetFeatureUnit())-1;
                diff = Math.Pow(diff, 2) / y;
                return diff;
            }
			else if (featureType == FeatureType.TYPE_FEATURE_MSTRING)
			{
			}
			else if (featureType == FeatureType.TYPE_FEATURE_STRING)
			{
                string a = problem.GetFeatureValue().ToString().Trim();
                string b = solution.GetFeatureValue().ToString().Trim();
                if (String.IsNullOrEmpty(a) || String.IsNullOrEmpty(b)) return 0;
                char[] separators = { ' ' };
                string[] a_array, b_array;
                a_array = a.Split(separators);
                b_array = b.Split(separators);
                if (a_array[0].ToUpper().Trim() == b_array[0].ToUpper().Trim())
                    diff = 0;
                else diff = 1;
                return diff;
                /*double percent_similar_words=0;
                
                string a = problem.GetFeatureValue().ToString().Trim();
                 string b = solution.GetFeatureValue().ToString().Trim();
                 if (String.IsNullOrEmpty(a) || String.IsNullOrEmpty(b)) return 0;
                char[] separators ={' '};
                string[] a_array, b_array;
                a_array = a.Split(separators);
                b_array = b.Split(separators);
                for (int i = 0; i < a_array.Length; i++)
                    for (int j = 0; j < b_array.Length; j++)
                        if (a_array[i].ToUpper().Trim() == b_array[j].ToUpper().Trim())
                        { percent_similar_words++; break; }
                if (percent_similar_words == 0) percent_similar_words = 1;
                else
                if (a_array.Length > b_array.Length)
                    percent_similar_words = percent_similar_words/a_array.Length;
                else
                    percent_similar_words = percent_similar_words / b_array.Length;
                 * diff = percent_similar_words;
                return diff;
               
                /*
                 string a = problem.GetFeatureValue().ToString().Trim();
                 string b = solution.GetFeatureValue().ToString().Trim();
                 if (String.IsNullOrEmpty(a) || String.IsNullOrEmpty(b)) return 0;
                 int lengthA = a.Length;
                 int lengthB = b.Length;
                 var distances = new int[lengthA + 1, lengthB + 1];
                 for (int i = 0; i <= lengthA; distances[i, 0] = i++) ;
                 for (int j = 0; j <= lengthB; distances[0, j] = j++) ;

                 for (int i = 1; i <= lengthA; i++)
                     for (int j = 1; j <= lengthB; j++)
                     {
                         int cost = b[j - 1] == a[i - 1] ? 0 : 1;
                         distances[i, j] = Math.Min
                             (
                             Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                             distances[i - 1, j - 1] + cost
                             );
                     }
                 diff= distances[lengthA, lengthB];
                 return diff;*/


            }
			else if (featureType == FeatureType.TYPE_FEATURE_UNDEFINED)
			{
			}
			else
			{
				//throw exception
			}
			return Math.Pow(diff, 2);		
		}
       
        static void Swap<T>(ref T arg1, ref T arg2)
        {
            T temp = arg1;
            arg1 = arg2;
            arg2 = temp;
        }
	}


    /*  string source = problem.GetFeatureValue().ToString();
      string target = solution.GetFeatureValue().ToString();
      int threshold = 1;
      int length1 = source.Length;
      int length2 = target.Length;

      // Return trivial case - difference in string lengths exceeds threshhold
      if (Math.Abs(length1 - length2) > threshold) { return int.MaxValue; }

      // Ensure arrays [i] / length1 use shorter length 
      if (length1 > length2)
      {
          Swap(ref target, ref source);
          Swap(ref length1, ref length2);
      }

      int maxi = length1;
      int maxj = length2;

      int[] dCurrent = new int[maxi + 1];
      int[] dMinus1 = new int[maxi + 1];
      int[] dMinus2 = new int[maxi + 1];
      int[] dSwap;

      for (int i = 0; i <= maxi; i++) { dCurrent[i] = i; }

      int jm1 = 0, im1 = 0, im2 = -1;

      for (int j = 1; j <= maxj; j++)
      {

          // Rotate
          dSwap = dMinus2;
          dMinus2 = dMinus1;
          dMinus1 = dCurrent;
          dCurrent = dSwap;

          // Initialize
          int minDistance = int.MaxValue;
          dCurrent[0] = j;
          im1 = 0;
          im2 = -1;

          for (int i = 1; i <= maxi; i++)
          {

              int cost = source[im1] == target[jm1] ? 0 : 1;

              int del = dCurrent[im1] + 1;
              int ins = dMinus1[i] + 1;
              int sub = dMinus1[im1] + cost;

              //Fastest execution for min value of 3 integers
              int min = (del > ins) ? (ins > sub ? sub : ins) : (del > sub ? sub : del);

              if (i > 1 && j > 1 && source[im2] == target[jm1] && source[im1] == target[j - 2])
                  min = Math.Min(min, dMinus2[im2] + cost);

              dCurrent[i] = min;
              if (min < minDistance) { minDistance = min; }
              im1++;
              im2++;
          }
          jm1++;
          if (minDistance > threshold) { return int.MaxValue; }
      }

      int result = dCurrent[maxi];
      return  (result > threshold) ? int.MaxValue : result;*/

    // mismatch distance
    /*   int ratio=0;
                
       string a = problem.GetFeatureValue().ToString().Trim().ToUpper();
       string 
           b = solution.GetFeatureValue().ToString().Trim().ToUpper();
       if (String.IsNullOrEmpty(a) || String.IsNullOrEmpty(b)) return 0;
       string min = a;
       string max = b;
       if (b.Length < a.Length) { min = b; max = a; }
       for (int i=0 ; i<min.Length ;i++)
           for (int j=0 ; j<max.Length ;j++)
         //  if ((min[i] !=' ') && (max.Contains(min[i].ToString())))
           if (min[i] == max[j] ) //&& (max.Contains(min[i].ToString())))
          // {

               ratio++;
            //   max=max.Remove(max.IndexOf(min[i]),1);
                            
          // }
          diff =(double) ratio/(a.Length+b.Length);
              
     */
