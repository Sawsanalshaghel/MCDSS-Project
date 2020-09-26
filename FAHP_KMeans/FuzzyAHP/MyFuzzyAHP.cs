using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fahp_cbr_app
{
    public class MyFuzzyAHP
    {
        public double[] CriteriaWeights;
        public double[] ChoicesWeights;
        double[] RI = { 0.0, 0.0, 0.58, 0.90, 1.12, 1.24, 1.32, 1.41, 1.45, 1.49, 1.51, 1.48, 1.56, 1.57, 1.59 };
        Dictionary<int, double[,]> alternatives_per_criterion;// priorities
        Dictionary<int, double[]> alternatives_weights__per_criterion;// final weights
        int criteria_count=0;
        int alternatives_count=0;
        double[,] criteria;        
        public List<double> fuzzy_Si;
        FuzzyNumber[] fuzzy_synthetic_extent;
        public MyFuzzyAHP()
        { }
        public MyFuzzyAHP(int criteria_count, int alternatives_count)
        {
            this.criteria_count = criteria_count;
            this.alternatives_count = alternatives_count;
            this.criteria = new double[criteria_count, criteria_count];
           
            this.alternatives_per_criterion = new Dictionary<int, double[,]>(); // priorities
            this.alternatives_weights__per_criterion = new Dictionary<int, double[]>(); // final weights
            CriteriaWeights = new double[criteria_count];
            ChoicesWeights = new double[alternatives_count];
        }
        public MyFuzzyAHP(int criteria_count)
        {
            this.criteria_count = criteria_count;
            this.criteria = new double[criteria_count, criteria_count];
            
            CriteriaWeights = new double[criteria_count];
            ChoicesWeights = new double[alternatives_count];
        }

        public double[,] Create_Criteria_Comparison_Array()
        {
         create:
                double[,] cri_arr = new double[criteria_count, criteria_count];
                for (int i = 0; i < criteria_count - 1; i++)
                {
                    Random r = new Random();
                    cri_arr[i, i] = 1;
                    cri_arr[i, i + 1] = r.Next(2, 10);
                    cri_arr[i + 1, i] = 1 / cri_arr[i, i + 1];
                }
                cri_arr[criteria_count- 1, criteria_count - 1] = 1;
                int m = 0;
                for (int i = 0; i < criteria_count; i++)
                    for (int k = i + 2; k < criteria_count; k++)
                    {
                        Random r = new Random();
                        if (cri_arr[i, k - 1] > cri_arr[k - 1, k])
                            m = (int)cri_arr[i, k - 1];
                        else
                            m = (int)cri_arr[k - 1, k];

                        cri_arr[i, k] = r.Next(m, 10);
                        cri_arr[k, i] = 1 / cri_arr[i, k];

                    }

            if (!CheckConsistency(cri_arr, criteria_count)) goto create;
            return cri_arr;
    
    }

        //----------------------------------------------------------------------------
        public double[,] Create_Criteria_Comparison_Array(int criteria_count)
        {
        create:
            int dice=0;
            double[,] cri_arr = new double[criteria_count, criteria_count];
            Random r = new Random();
            for (int i = 0; i < criteria_count - 1; i++)
            {
               
                cri_arr[i, i] = 1;
                dice = r.Next(0, 2);
                if (dice == 1)
                {
                    cri_arr[i, i + 1] = r.Next(2, 10);
                    cri_arr[i + 1, i] = 1 / cri_arr[i, i + 1];
                }
                else
                {
                    cri_arr[i + 1, i] = r.Next(2, 10);
                    cri_arr[i, i + 1] = 1 / cri_arr[i + 1 , i];
                }
                
            }
            cri_arr[criteria_count - 1, criteria_count - 1] = 1;
            int res = 0;
            double first = 0;
            double second = 0;
           // double third = 0;
            for (int i = 0; i < criteria_count; i++)
                for (int k = i + 2; k < criteria_count; k++)
                {
                    //Random r = new Random();
                  //  if (
                        first = cri_arr[i, k - 1];
                        second= cri_arr[k - 1, k];
                        if (first  > 1 && second  > 1  )
                        {
                            res = (int)first + (int)second;
                             if (res > 9) res=9;
                             cri_arr[i, k] = r.Next(res, 10);
                             cri_arr[k, i] = 1 / cri_arr[i, k];
                        }
                        else if (first  < 1 && second  < 1)
                        {
                            
                            res = (int)(1 / first) + (int)(1 / second);
                            if (res > 9) res = 9;
                            cri_arr[k, i] = r.Next(res, 10);
                            cri_arr[i, k] = 1 / cri_arr[k, i];
                        }
                        else
                            if (first > 1 && second  < 1 )
                            {
                                second = 1 / second;
                                res = Math.Abs(((int)first - (int)second));
                                if (res  < 2) res = 2;
                                if (first > second)
                                {

                                    cri_arr[i, k] = res;
                                    cri_arr[k, i] = 1 / cri_arr[i, k];
                                }else
                                {
                                    cri_arr[k, i] = res;
                                    cri_arr[i, k]  = 1 / cri_arr[k, i];
                                }
                            }
                            else if (first  < 1 && second  > 1)
                            {
                                first = 1 / first;
                                res = Math.Abs(((int)first - (int)second));
                                if (res < 2) res = 2;
                                if (first < second)
                                {
                                    cri_arr[i, k] = res;
                                    cri_arr[k, i] = 1 / cri_arr[i, k];
                                }
                                else
                                {
                                    cri_arr[k, i] = res;
                                    cri_arr[i, k] = 1 / cri_arr[k, i];
                                }
                            }
                     

                }

            if (!CheckConsistency(cri_arr, criteria_count)) goto create;
            return cri_arr;

        }
        //-----------------------------------------------------------
        public bool CheckConsistency (double [,] comp_arr, int compared_elements_count )
        {

          
                return true;
        }
        public void AddCriteria(double[,] criteria)
        {
              
              this.criteria = criteria;
          }
        double [] CalculateWeights(double[,] pairwise_comparison_arr)
        {
            int compared_count = pairwise_comparison_arr.GetLength(0);
            double[] final_weights = new double[compared_count];
            FuzzyNumber[,] fuzzy_arr = new FuzzyNumber[compared_count, compared_count];
            FuzzyNumber[] fuzzytemp = new FuzzyNumber[compared_count];
            fuzzy_synthetic_extent = new FuzzyNumber[compared_count];
            
            FuzzyNumber temp = new FuzzyNumber();
            TriangleFuzzyScale.create_FuzzyScale(0.75);

            #region create fuzzy numbers matrix depending on Saati scale
            for (int i = 0; i < compared_count ; i++)
            {
                fuzzy_arr[i, i] = new FuzzyNumber(TriangleFuzzyScale.FuzzyNumber[1].l, TriangleFuzzyScale.FuzzyNumber[1].m, TriangleFuzzyScale.FuzzyNumber[1].u);

            }
            for (int i = 0; i < compared_count - 1; i++)
                for (int j = i + 1; j < compared_count; j++)
                {
                    double weight = pairwise_comparison_arr[i, j];
                    fuzzy_arr[j, i] = new FuzzyNumber();
                    fuzzy_arr[i, j] = new FuzzyNumber();
                   if (weight > 1)
                    { 
                        fuzzy_arr[i, j].l = TriangleFuzzyScale.FuzzyNumber[Convert.ToInt32(weight)].l;
                        fuzzy_arr[i, j ].m = TriangleFuzzyScale.FuzzyNumber[Convert.ToInt32(weight)].m;
                        fuzzy_arr[i, j ].u = TriangleFuzzyScale.FuzzyNumber[Convert.ToInt32(weight)].u;
                        
                        fuzzy_arr[j, i ].l = TriangleFuzzyScale.FuzzyNumber[Convert.ToInt32(weight)].Reversed().l;
                        fuzzy_arr[j, i ] .m= TriangleFuzzyScale.FuzzyNumber[Convert.ToInt32(weight)].Reversed().m;
                        fuzzy_arr[j, i ].u = TriangleFuzzyScale.FuzzyNumber[Convert.ToInt32(weight)].Reversed().u;

                    }
                    else
                    {
                        weight = Math.Round((1 / weight),0);

                        fuzzy_arr[j, i].l = TriangleFuzzyScale.FuzzyNumber[Convert.ToInt32(weight)].l;
                        fuzzy_arr[j, i].m = TriangleFuzzyScale.FuzzyNumber[Convert.ToInt32(weight)].m;
                        fuzzy_arr[j, i].u = TriangleFuzzyScale.FuzzyNumber[Convert.ToInt32(weight)].u;

                        fuzzy_arr[i, j].l = TriangleFuzzyScale.FuzzyNumber[Convert.ToInt32(weight)].Reversed().l;
                        fuzzy_arr[i, j].m = TriangleFuzzyScale.FuzzyNumber[Convert.ToInt32(weight)].Reversed().m;
                        fuzzy_arr[i, j].u = TriangleFuzzyScale.FuzzyNumber[Convert.ToInt32(weight)].Reversed().u;

                       

                    }

                }
            #endregion
            
            #region // calculate fuzzy_synthetic_extent Si
            for (int i = 0; i < compared_count; i++)
            {
                fuzzytemp[i] = new FuzzyNumber();
                for (int j = 0; j < compared_count; j++)
                {

                    fuzzytemp[i].l += fuzzy_arr[i, j].l;
                    fuzzytemp[i].m += fuzzy_arr[i, j].m;
                    fuzzytemp[i].u += fuzzy_arr[i, j].u;
                }
            }
                

            temp = new FuzzyNumber();
            for (int j = 0; j < compared_count; j++)
            {
                     
                temp.l += fuzzytemp[j].l;
                temp.m += fuzzytemp[j].m;
                temp.u += fuzzytemp[j].u;
            }
            temp.l = 1 / temp.l;
            temp.m = 1 / temp.m;
            temp.u = 1 / temp.u;
              


            for (int i = 0; i < compared_count; i++)
            {
                fuzzy_synthetic_extent[i] = new FuzzyNumber();
                fuzzy_synthetic_extent[i].l = fuzzytemp[i].l * temp.u;
                fuzzy_synthetic_extent[i].m = fuzzytemp[i].l * temp.m;
                fuzzy_synthetic_extent[i].u = fuzzytemp[i].l * temp.l;
            }
#endregion  
 #region // calculate Si>=Sk
            double total_fuzzy_weights = 0;
            for (int first_number = 0; first_number < compared_count ; first_number++) 
              {
                  fuzzy_Si = new List<double>();
                  for (int second_number = 0; second_number < compared_count; second_number++)
                  {
                      if (first_number != second_number)
                      {
                          if (fuzzy_synthetic_extent[first_number].m >= fuzzy_synthetic_extent[second_number].m)
                          {
                              fuzzy_Si.Add(1);
                          }
                          else
                              if (fuzzy_synthetic_extent[second_number].l >= fuzzy_synthetic_extent[first_number].u)
                              {
                                  fuzzy_Si.Add(0);
                              }
                              else
                              {
                                  fuzzy_Si.Add((fuzzy_synthetic_extent[second_number].l - fuzzy_synthetic_extent[first_number].u) / ((fuzzy_synthetic_extent[first_number].m - fuzzy_synthetic_extent[first_number].u) - (fuzzy_synthetic_extent[second_number].m - fuzzy_synthetic_extent[second_number].l)));
                              }
                      }
                  }// end second for
                      final_weights[first_number] = fuzzy_Si.Min();
                      total_fuzzy_weights += final_weights[first_number];
              }
#endregion

            #region Normalize Fuzzy weights
            for (int i = 0; i < compared_count; i++)
            {
                final_weights[i] = Math.Round((final_weights[i] / total_fuzzy_weights), 2);
            }
            #endregion

            return final_weights;

        }
        public  void  CalculatedCriteria()
        {
            CriteriaWeights = CalculateWeights(criteria);
           
        }
        public void AddCriterionRatedChoices(int criterion, double[,] choices_arr)
        {
            double[,] choices = new double[alternatives_count, alternatives_count];
            for (int i = 0; i < alternatives_count; i++)
                for (int j = 0; j < alternatives_count; j++)
                    choices[i, j] = choices_arr[i, j];
            alternatives_per_criterion.Add(criterion, choices);
        }
        public void AddCriterionRatedChoices(Dictionary<int, double[,]> choices)
        {

            alternatives_per_criterion = choices;
        }
        public double[] CalculatedChoices()
        {
             
              double[,] alternatives= new double[alternatives_count, alternatives_count];
              double[] alternative_weights_criterion = new double[alternatives_count];
              for (int k = 0;  k  <  criteria_count ; k++)
              {

                  double[] choice_weights = new double[alternatives_count];
                
                  alternatives = alternatives_per_criterion[k];

                  choice_weights = CalculateWeights(alternatives);

                  alternatives_weights__per_criterion.Add(k, choice_weights);
              }

              for (int i = 0; i < criteria_count ; i++)
              {
                  alternative_weights_criterion = alternatives_weights__per_criterion[i];

                   for (int j = 0; j < alternatives_count ; j++)
                       ChoicesWeights[j] += alternative_weights_criterion[j] * CriteriaWeights[i];
              }
              return ChoicesWeights;
          }

        public double[] Calculated_One__Choice(out bool consistency, double cri_weight, double[,] alternatives)
        {
            double[] final_result = new double[alternatives_count];
            double[] alternative_weights_criterion = new double[alternatives_count];
            alternative_weights_criterion = CalculateWeights(alternatives);
            consistency = CheckConsistency(alternatives, alternatives_count);
            for (int j = 0; j < alternatives_count; j++)
                final_result[j] = alternative_weights_criterion[j] * cri_weight;
              return final_result;
        }


        public Dictionary<int, double[,]> Create_All_Criteria_Choice_Comparison_Array(List<Case> cases)
        {
            Dictionary<int, double[,]> AllChoiceComp = new Dictionary<int, double[,]>();

            int count_citeria = cases[0].GetFeatures().Count - 2;

            for (int I = 0; I < count_citeria; I++)// exclude id  cluster
            {
                GC.Collect();

                double[,] comp = new double[cases.Count, cases.Count];
                for (int i = 0; i < cases.Count; i++)
                    for (int j = 0; j < cases.Count; j++)
                        comp[i, j] = 1;
                List<double> sol_value = new List<double>();
                for (int i = 0; i < cases.Count; i++)
                {
                    Case c = cases[i];
                    Feature f = (Feature)c.GetFeatures()[I + 1];
                    if (!(f.GetFeatureType() == 2 || f.GetFeatureType() == 4 || f.GetFeatureType() == 9))
                        goto end; // all comparision is 1 for strings
                    sol_value.Add(Convert.ToDouble(f.GetFeatureValue()));
                }
                double max = sol_value.Max();
                double min = sol_value.Min();
                double sub = max - min;
                sub = sub / 8;
                bool i_isBigger = false;
                double value = 0;
                for (int i = 0; i < sol_value.Count; i++)
                    for (int j = i + 1; j < sol_value.Count; j++)
                    {
                        if (sol_value[i] == sol_value[j])
                            comp[i, j] = 1;
                        else
                        {
                            if (sol_value[i] > sol_value[j])
                                i_isBigger = true;
                            else
                                i_isBigger = false;
                            value = Math.Abs(sol_value[i] - sol_value[j]);
                            if (value <= sub)
                                comp[i, j] = 2;
                            else
                                if (value > sub && value <= 2 * sub)
                                    comp[i, j] = 3;
                                else
                                    if (value > 2 * sub && value <= 3 * sub)
                                        comp[i, j] = 4;
                                    else
                                        if (value > 3 * sub && value <= 4 * sub)
                                            comp[i, j] = 5;
                                        else
                                            if (value > 4 * sub && value <= 5 * sub)
                                                comp[i, j] = 6;
                                            else
                                                if (value > 5 * sub && value <= 6 * sub)
                                                    comp[i, j] = 7;
                                                else
                                                    if (value > 6 * sub && value <= 7 * sub)
                                                        comp[i, j] = 8;
                                                    else
                                                        if (value > 7 * sub && value <= 8 * sub)
                                                            comp[i, j] = 9;
                            if (!i_isBigger)
                            { comp[j, i] = comp[i, j]; comp[i, j] = 1 / comp[j, i]; }
                            else
                            { comp[j, i] = 1 / comp[i, j]; }

                        } // end else
                    }

            end: AllChoiceComp.Add(I, comp);
            }
            return AllChoiceComp;
        }



        public  double[,] CreateOne_CeiteriaChoiceComp(int I,List<Case> cases)
        {
            Dictionary<int, double[,]> AllChoiceComp = new Dictionary<int, double[,]>();

            int count_citeria = cases[0].GetFeatures().Count - 2;
            GC.Collect();
            double[,] comp = new double[cases.Count, cases.Count];
            for (int i = 0; i < cases.Count; i++)
                for (int j = 0; j < cases.Count; j++)
                    comp[i, j] = 1;
            List<double> sol_value = new List<double>();
            for (int i = 0; i < cases.Count; i++)
            {
                Case c = cases[i];
                Feature f = (Feature)c.GetFeatures()[I+1];

                    if (!(f.GetFeatureType() == 2 || f.GetFeatureType() == 4 || f.GetFeatureType() == 9))
                        goto end; // all comparision is 1 for strings
                    sol_value.Add(Convert.ToDouble(f.GetFeatureValue()));
            }
            double max = sol_value.Max();
            double min = sol_value.Min();
            double sub = max - min;
            sub = sub / 8;
            bool i_isBigger = false;
            double value = 0;
            for (int i = 0; i < sol_value.Count; i++)
                for (int j = i + 1; j < sol_value.Count; j++)
                {
                    if (sol_value[i] == sol_value[j])
                        comp[i, j] = 1;
                    else
                    {
                        if (sol_value[i] > sol_value[j])
                            i_isBigger = true;
                        else
                            i_isBigger = false;
                        value = Math.Abs(sol_value[i] - sol_value[j]);
                        if (value <= sub)
                            comp[i, j] = 2;
                        else
                            if (value > sub && value <= 2 * sub)
                                comp[i, j] = 3;
                            else
                                if (value > 2 * sub && value <= 3 * sub)
                                    comp[i, j] = 4;
                                else
                                    if (value > 3 * sub && value <= 4 * sub)
                                        comp[i, j] = 5;
                                    else
                                        if (value > 4 * sub && value <= 5 * sub)
                                            comp[i, j] = 6;
                                        else
                                            if (value > 5 * sub && value <= 6 * sub)
                                                comp[i, j] = 7;
                                            else
                                                if (value > 6 * sub && value <= 7 * sub)
                                                    comp[i, j] = 8;
                                                else
                                                    if (value > 7 * sub && value <= 8 * sub)
                                                        comp[i, j] = 9;
                        if (!i_isBigger)
                        { comp[j, i] = comp[i, j]; comp[i, j] = 1 / comp[j, i]; }
                        else
                        { comp[j, i] = 1 / comp[i, j]; }

                    } // end else
                }


                end: return comp;
        }
    }
}
