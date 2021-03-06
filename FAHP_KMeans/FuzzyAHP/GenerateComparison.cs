﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fahp_cbr_app
{
    static public  class GenerateComparison
    {
        static public double[,] CriteriaComparisonMatrix;
        static public Dictionary<int, double[,]> ChoicesComparisonMatrix;
        static bool CheckConsistency(double[,] comp_arr, int compared_elements_count)
        {

            double[] criteria_weights = new double[compared_elements_count];
            double[] RI = { 0.0, 0.0, 0.58, 0.90, 1.12, 1.24, 1.32, 1.41, 1.45, 1.49, 1.51, 1.48, 1.56, 1.57, 1.59 };
            double[] temp = new double[compared_elements_count];
            for (int i = 0; i < compared_elements_count; i++)
                for (int j = 0; j < compared_elements_count; j++)
                    temp[i] += comp_arr[j, i];
            for (int i = 0; i < compared_elements_count; i++)
            {
                for (int j = 0; j < compared_elements_count; j++)
                {
                    criteria_weights[i] += comp_arr[i, j] / temp[j];
                }
                criteria_weights[i] = criteria_weights[i] / compared_elements_count;
            }
            double[] ymax = new double[compared_elements_count];
            // check consistency
            for (int i = 0; i < compared_elements_count; i++)
                for (int j = 0; j < compared_elements_count; j++)
                    ymax[i] += comp_arr[i, j] * criteria_weights[j];
            double total_ymax = 0;
            for (int i = 0; i < compared_elements_count; i++)
            {
                ymax[i] = ymax[i] / criteria_weights[i];
                total_ymax += ymax[i];
            }
            total_ymax = total_ymax / compared_elements_count;
            if (compared_elements_count <= 15)
            {
                double ci = (total_ymax - compared_elements_count) / (compared_elements_count - 1);
                double cr = ci / RI[compared_elements_count - 1];
                cr = Math.Round(cr, 1);
                if (cr > 0.1) return false;
            }
            else
            {
                double a = 0.1;
                double q = compared_elements_count + a * (1.7699 * compared_elements_count - 4.3513);
                if (total_ymax > q) return false;
            }

            return true;
        }
        public static  double[,] Create_Criteria_Comparison_Array(int criteria_count)
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
            CriteriaComparisonMatrix = cri_arr;
            return cri_arr;

        }

        public static Dictionary<int, double[,]> Create_All_Criteria_Choice_Comparison_Array(List<Case> cases)
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
            ChoicesComparisonMatrix = AllChoiceComp;
            return AllChoiceComp;
        }

    }
}
