using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;

namespace Fahp_cbr_app
{
    public class Kmeans
    {
        static int maxIteration = 500; //the maximum number of iterations to not fixated
        public int k; // Number of centroids
        public int iteration;// Current iteration
        public List<List<Case>> clusters;// Set of clusters, which include a set of points
        public double[] distances;// number of clusters sum of distances between each point in its cluster and centroid
        public List<Case> centroids;// Coordinates of the centroids
        public List<Case> points;// Coordinates of all the available points

        public Kmeans()// Constructor to initialize all the relevant variables
        {
            
        }
        public Kmeans(int k, List<Case> p)// Constructor to initialize all the relevant variables
        {
            this.k = k; // we set the number of centroids (how will the clusters)
            this.points = p; // ask our points
            clusters = new List<List<Case>>(); // initially no clusters
            distances = new double[k];// initially distances
            centroids = new List<Case>(); // initially no centroid
            firstClusters(); // run function calculates the initial centroid
        }


        private void firstClusters()// Selects the first centroid
        {
          //  Case first = points[0]; // First centroid is selected as the first point of all
           // centroids.Add (first);
            List <int> rr=new List <int>();
            int c=0;// add the centroid to centroid list
             while (c !=k)  // is necessary to generate a centroid, so the cycle to a
            {
                Random rnd = new Random(); // for calculating random numbers
                int rand = rnd.Next(points.Count);
             
                if (!rr.Contains(rand))
                {
                    centroids.Add(points[rand]);
                    c++;
                    rr.Add(rand);
                }
            }
        /*    for (int i = 1; i <k; ++ i) // is necessary to generate a centroid, so the cycle to a
            {
                //#1
                double sum = 0; // stores the sum of the square of the distance between all of the points nearest to them centroid centroid
                foreach (Case p in points) // loop through all the points
                {
                    double d = nearestCentroid (p); // calculate the distance from the current point to the nearest centroid to it
                    double dx = d * d; // calculate the square of the distance
                    sum += dx; // add to the total
                }
                double rand = rnd.NextDouble () * sum; // multiply the amount found in the random number
                sum = 0; // Zero sum
                foreach (Case p in points) // again pass through all the points
                {
                    double d = nearestCentroid (p); // calculate the distance from the current point to the nearest centroid to it
                    double dx = d * d; // calculate the square of the distance
                    sum += dx; // add to the total
                    if (sum >= rand) // if this amount was more than previously calculated rand, the current point is the centroid
                    {
                        centroids.Add (p); // add the centroid to centroid list
                        break; // exit the loop and start looking sluduyuschy centroid go to # 1
                    }
                }
            }*/
        }

        private double nearestCentroid(Case a) // the distance to the nearest centroid
        {
            if (centroids.Count == 0) return 0; // if the centroid is not present, then the distance is 0
            double min = distance (a, centroids [0]); // calculate the distance to the first in the list of the centroid
            foreach (Case p in centroids) // Loop through all the centroids
            {
                double d = distance (a, p); // vychisyaem distance to the centroid of the current
                if (d < min && p != a) // if this distance is less than previously calculated, it means the centroid closer
                {
                    min = d; // exhibiting a minimum distance
                }
            }
            return min; // once walked all means centroid distance vychisdenno, return it
        }

        private double distance(Case a, Case b) // the distance between two points
        {
            EuclideanSimilarity sim = new EuclideanSimilarity();// the Pythagorean theorem we calculate the distance between two points
        //    return sim.Compare(a, b); // vyzvraschaem distance 1/distance
            return sim.Dissimilarity(a, b);
 
        }
        private double distance2(Case a, Case b) // the distance between two points
        {
            EuclideanSimilarity sim = new EuclideanSimilarity();// the Pythagorean theorem we calculate the distance between two points
            //    return sim.Compare(a, b); // vyzvraschaem distance 1/distance
            return sim.Similarity(a, b);

        }


        public double Ak(int k, int Nd)
        {
            if (k == 2)
            {
                decimal r = (Decimal)3 / (Decimal)(4 * Nd);
                decimal re = (Decimal)1 - r;
                double res = Convert.ToDouble(re);
                return res;
            }
            else
                return (Ak(k - 1, Nd) + ((1 - Ak(k - 1, Nd)) / 6));
        }
       

        public List<List<List<Case>>> run() // the main course of the algorithm, which returns a point divided into clusters
        {

            List<List<List<Case>>> r = new List<List<List<Case>>>(); // variable storing a plurality of clusters that store the set of points
            int i = 0; // variable counting the number of iterations
            List<Case> newCentroids = new List<Case>(); // variable storing the new centroid centroid
            while (i < maxIteration && previousDistributionChange(newCentroids)) // until the limit is reached, or until the new iiteratsy centroid different from the old
            {
                if ( newCentroids.Count != 0) centroids = newCentroids; // remember the old centroid, the first pass of the algorithm is not new centroid generiovalis, because doing nothing
                clearClusters(); // clear the clusters to re-fill them
                foreach (Case p in points) // Determine the poit on clusters
                {
                    double dis = 0;
                    int c = findCluster(p, ref dis); // for each point of looking her cluster
                    clusters[c].Add(p); // add a point to the desired cluster
                    distances[c] += dis;
                }
                newCentroids = new List<Case>(); // clear the array of new centroid
                foreach (List<Case> list in clusters) // for each cluster, define a new centroid
                {
                    newCentroids.Add(newCentroid(list)); // add a new centroid in many new centroid
                }
                r.Add(clusters); // remember clusters
               i++; // increment iteration znayenie
            }
           iteration = i;
            return r; // return
        }



        public Case newCentroid(List<Case> list) // generate new centroid for the cluster 
        {
            int count = list.Count();
            Case result = null;
            int sumf = 0; double sumd = 0; bool sumb = false; 
            List<string> sumst=new List<string>() ;
            List<int> sums_count=new List<int>() ;
            if (count != 0)
            {
                  
                Feature f = new Feature();
                  result = new Case(list[0].GetCaseID(), list[0].GetCaseName(), list[0].GetCaseDescription());
                  for (int j = 0; j < list.First().GetFeatures().Count; j++)
                  {
                      
                      foreach (Case c in list)
                      {
                          f = (Feature)c.GetFeatures()[j];
                          if (f.GetFeatureName() == "id" || f.GetFeatureName() == "cluster")
                              break;
                          if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_INT) 
                              sumf += Convert.ToInt32(f.GetFeatureValue());
                           else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT) 
                              sumd += Convert.ToDouble(f.GetFeatureValue());
                          else if  (f.GetFeatureType() == FeatureType.TYPE_FEATURE_BOOL)
                              sumb = (sumb || Convert.ToBoolean(f.GetFeatureValue()));
                          else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_STRING ||f.GetFeatureType() == FeatureType.TYPE_FEATURE_CATEGORICAL )
                          {  
                             
                                 int position =sumst.IndexOf(f.GetFeatureValue().ToString());
                                 if (position >= 0)
                                  sums_count[position] = sums_count[position] + 1; 
                                 else
                                 { 
                                     sumst.Add(f.GetFeatureValue().ToString());
                                     sums_count.Add(1);
                                 }
                          }
                      }
                      if (f.GetFeatureName() == "cluster")
                          result.AddFeature(f.GetFeatureName(), f.GetFeatureType(), -1, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                      else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_INT)
                          result.AddFeature(f.GetFeatureName(), f.GetFeatureType(), sumf / list.Count(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                      else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT)
                          result.AddFeature(f.GetFeatureName(), f.GetFeatureType(), sumd / list.Count(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                      else if  (f.GetFeatureType() == FeatureType.TYPE_FEATURE_BOOL)
                          result.AddFeature(f.GetFeatureName(), f.GetFeatureType(), sumb, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                      else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_STRING || f.GetFeatureType() == FeatureType.TYPE_FEATURE_CATEGORICAL)
                      {
                             
                          int max=sums_count.IndexOf(sums_count.Max());
                         
                          string string_value=sumst[max];
                          result.AddFeature(f.GetFeatureName(), f.GetFeatureType(), string_value, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                          }

                      sumf = 0;
                      sumd = 0;
                      sumb = false;
                      sumst.Clear();
                      sums_count.Clear();
                  }

            }
             return result;

        }


        public int findCluster(Case a, ref double dis) // looking for the appropriate cluster for a point 
        {
            
            if (centroids.Count == 0) return 0; // if there is no centroid, then 0 
            double min = distance (a, centroids [0]); // calculate the distance to the centroid of the first 
            int i = 0; 
            int l = 0;
            foreach (Case p in centroids) // looping through all the centroids 
            {
                double d = distance (a, p); // calculate the distance to the centroid of the current 
                if (d <= min) // if it is less than before, it's new to kondidat cluster to this point 
                {
                    min = d; // similarty bigger
                    l = i;
                    dis = d;
                } 
                i++; 
            } return l; // return the cluster number
        }


        public int findCluster_customize(Case a, List<Case> centroids) // looking for the appropriate cluster for a point 
        {
            if (centroids.Count == 0) return 0; // if there is no centroid, then 0 
            double min = distance2(a, centroids[0]); // calculate the distance to the centroid of the first 
            int i = 0;
            int l = 0;
            foreach (Case p in centroids) // looping through all the centroids 
            {
                double d = distance2(a, p); // calculate the distance to the centroid of the current 
                if (d > min) // if it is less than before, it's new to kondidat cluster to this point 
                {
                    min = d; // similarty bigger
                    l = i;
                }
                i++;
            } return l; // return the cluster number
        }
        private void clearClusters()//clear the clusters
        {
            clusters = new List<List<Case>>();//соз
            for(int i = 0; i < k; ++i)
            {
                clusters.Add(new List<Case>());
                distances[i] = 0;
            }
        }

        private bool previousDistributionChange(List<Case> list)
        {
            for (int o = 0; o < list.Count; o++)
                if (list[o] == null) list.RemoveAt(o);
                if (list.Count != centroids.Count) return true;
            bool f = false;
            for(int i = 0; i < list.Count; ++i)
            {
                if (list[i] != centroids[i]) return true;
            }
            return f;
        }



        public static Case ReCalcCentroid(List<Case> list, int centroid_id) // generate new centroid for the cluster 
        {
            int count = list.Count();
            List<int> sums_count=new List<int>() ;
            Case result = null;
            int sumf = 0; double sumd = 0; bool sumb = false; List<string> sumst = new List<string>();
            if (count != 0)
            {
                Feature f = new Feature();
                result = new Case(list[0].GetCaseID(), list[0].GetCaseName(), list[0].GetCaseDescription());
                for (int j = 0; j < list.First().GetFeatures().Count; j++)
                {
                    foreach (Case c in list)
                    {

                        f = (Feature)c.GetFeatures()[j];
                        if (f.GetFeatureName() == "id" || f.GetFeatureName() == "cluster")
                            break;
                        if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_INT)
                            sumf += Convert.ToInt32(f.GetFeatureValue());
                        else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT)
                            sumd += Convert.ToDouble(f.GetFeatureValue());
                        else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_BOOL)
                            sumb = (sumb || Convert.ToBoolean(f.GetFeatureValue()));
                        else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_STRING || f.GetFeatureType() == FeatureType.TYPE_FEATURE_CATEGORICAL)
                        {

                            int position = sumst.IndexOf(f.GetFeatureValue().ToString());
                            if (position >= 0)
                                sums_count[position] = sums_count[position] + 1;
                            else
                            {
                                sumst.Add(f.GetFeatureValue().ToString());
                                sums_count.Add(1);
                            }

                        }

                    }
                    if ( f.GetFeatureName() == "cluster")
                        result.AddFeature(f.GetFeatureName(), f.GetFeatureType(), -1, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                    else if ( f.GetFeatureName() == "id")
                        result.AddFeature(f.GetFeatureName(), f.GetFeatureType(), centroid_id, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                    else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_INT)
                        result.AddFeature(f.GetFeatureName(), f.GetFeatureType(), sumf / list.Count(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                    else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT)
                        result.AddFeature(f.GetFeatureName(), f.GetFeatureType(), sumd / list.Count(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                    else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_BOOL)
                        result.AddFeature(f.GetFeatureName(), f.GetFeatureType(), sumb, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                    else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_STRING || f.GetFeatureType() == FeatureType.TYPE_FEATURE_CATEGORICAL)
                    {

                        int max = sums_count.IndexOf(sums_count.Max());
                       
                        string string_value = sumst[max];
                        result.AddFeature(f.GetFeatureName(), f.GetFeatureType(), string_value, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                        /* Random rd = new Random(); // for calculating random numbers
                            int rnd = rd.Next(sumst.Count);
                             result.AddFeature(f.GetFeatureName(), f.GetFeatureType(), sumst[rnd].ToString(), 1.0, false, false,"");*/
                    }


                    sumf = 0;
                    sumd = 0;
                    sumb = false;
                }

            }
            return result;
            
        }


    }
}
