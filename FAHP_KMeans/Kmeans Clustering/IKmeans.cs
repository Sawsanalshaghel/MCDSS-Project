using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace Fahp_cbr_app.Kmeans_Clustering
{
    class IKmeans
    {

        public class PointClusters
        {
            private Dictionary<Case, List<Case>> _pc = new Dictionary<Case, List<Case>>();
            private Dictionary<Case, double> DistortionError = new Dictionary<Case, double>();
            private double SK; //sum distortion error 
            private double FK; //cost function
            public Dictionary<Case, List<Case>> PC
            {
                get { return _pc; }
                set { _pc = value; }
            }
            public Dictionary<Case, double> dist_error
            {
                get { return DistortionError; }
                set { DistortionError = value; }
            }

            public double Fk
            {
                get { return FK; }
                set { FK = value; }
            }
            public double Sk
            {
                get { return SK; }
                set { SK = value; }
            }


        }

        int iter = 0;
        //Intermediate calculation object
        public struct PointDetails
        {
            private Case _seedpoint;
            private double[] _Weights;
            private double _Sum;
            private double _minD;

            public Case SeedPoint
            {
                get { return _seedpoint; }
                set { _seedpoint = value; }
            }

            public double[] Weights
            {
                get { return _Weights; }
                set { _Weights = value; }
            }

            public double Sum
            {
                get { return _Sum; }
                set { _Sum = value; }
            }

            public double MinD
            {
                get { return _minD; }
                set { _minD = value; }
            }
        }


        /// <summary>
        /// Basic (non kd-tree) implementation of kmeans++ algorithm. 
        /// cf. http://en.wikipedia.org/wiki/K-means%2B%2B
        /// Excellent for financial diversification cf. 
        /// Clustering Techniques for Financial Diversification, March 2009
        /// cf http://www.cse.ohio-state.edu/~johansek/clustering.pdf 
        /// Zach Howard & Keith Johansen
        /// Note1: If unsure what value of k to use, try: k ~ (n/2)^0.5
        /// cf. http://en.wikipedia.org/wiki/Determining_the_number_of_clusters_in_a_data_set
        /// </summary>
        /// <param name="allPoints">All points in ensemble</param>
        /// <param name="k">Number of clusters</param>
        /// <returns></returns>
        public PointClusters GetKMeansPP(List<Case> allPoints, int k)
        {
            //1. Preprocess KMeans (obtain optimized seed points)
            List<Case> seedPoints = GetSeedPoints(allPoints, k);
            //2. Regular KMeans algorithm
            PointClusters resultado = GetKMeans(allPoints, seedPoints, k);

            return resultado;
        }

        public PointClusters GetKMeansPP_K(List<Case> allPoints)
        {
            //1. Preprocess KMeans (obtain optimized seed points)
            //List<Case> seedPoints = GetSeedPoints(allPoints, k);

            //2. Regular KMeans algorithm
            //PointClusters resultado = GetKMeans(allPoints, seedPoints, k);
            //2. Best K for KMeans algorithm
            PointClusters resultado = GetBestKMeans(allPoints);
            return resultado;
        }

        //Bog standard k-means.
        private double Ak(int k, int Nd)
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
        private PointClusters GetBestKMeans(List<Case> allPoints)
        {

            Dictionary<int, PointClusters> AllClusters = new Dictionary<int, PointClusters>();
            PointClusters cluster = new PointClusters();
            List<Case> seedPoints = new List<Case>();
            double Sk = 0;
            // Number of Attributes
            int Nd = allPoints[0].GetFeatures().Count - 2;//except id , cluster
            seedPoints = GetSeedPoints(allPoints, 1);
            cluster = GetKMeans(allPoints, seedPoints, 1);
            foreach (KeyValuePair<Case, double> pair in cluster.dist_error)
                Sk += pair.Value;
            cluster.Sk = Sk;
            cluster.Fk = 1;
            AllClusters.Add(1, cluster);

            for (int i = 2; i <= 5; i++)//19
            {
                seedPoints = GetSeedPoints(allPoints, i);
                cluster = GetKMeans(allPoints, seedPoints, i);
                foreach (KeyValuePair<Case, double> pair in cluster.dist_error)
                    cluster.Sk += pair.Value;
                if (AllClusters[i - 1].Sk == 0)
                    cluster.Fk = 1;
                else
                {
                    double ak = 1;
                    if (Nd > 1)
                        ak = Convert.ToDouble(Ak(i, Nd));
                    cluster.Fk = cluster.Sk / (ak * AllClusters[i - 1].Sk);
                }
                AllClusters.Add(i, cluster);
            }


            double min = double.MaxValue;
            int kk = 0;
            foreach (KeyValuePair<int, PointClusters> pair in AllClusters)
                if (pair.Value.Fk < min)
                { kk = pair.Key; cluster = pair.Value; min = pair.Value.Fk; }

            return cluster;
        }
        private PointClusters GetKMeans(List<Case> allPoints, List<Case> seedPoints, int k)
        {

        begin: PointClusters cluster = new PointClusters();
            double[] Distances = new double[k];
            double minD = double.MaxValue;
            List<Case> sameDPoint = new List<Case>();
            bool exit = true;

            //Cycle thru all points in ensemble and assign to nearest centre 
            foreach (Case p in allPoints)
            {
                foreach (Case sPoint in seedPoints)
                {
                    double dist = GetEuclideanD(p, sPoint);
                    if (dist < minD)
                    {
                        sameDPoint.Clear();
                        minD = dist;
                        sameDPoint.Add(sPoint);
                    }
                    else if (dist == minD)
                    {
                        if (!sameDPoint.Contains(sPoint))
                            sameDPoint.Add(sPoint);
                    }
                }

                //Extract nearest central point. 
                Case keyPoint;
                if (sameDPoint.Count > 1)
                {
                    int index = GetRandNumCrypto(0, sameDPoint.Count);
                    keyPoint = sameDPoint[index];
                }
                else
                    keyPoint = sameDPoint[0];

                //Assign ensemble point to correct central point cluster
                if (!cluster.PC.ContainsKey(keyPoint))  //New
                {
                    List<Case> newCluster = new List<Case>();
                    newCluster.Add(p);
                    cluster.PC.Add(keyPoint, newCluster);
                    cluster.dist_error.Add(keyPoint, minD);
                }
                else
                {   //Existing cluster centre   
                    cluster.PC[keyPoint].Add(p);
                    double value = cluster.dist_error[keyPoint];
                    cluster.dist_error[keyPoint] += minD;
                }

                //Reset
                sameDPoint.Clear();
                minD = double.MaxValue;
            }

            //Bulletproof check - it it come out of the wash incorrect then re-seed.
            if (cluster.PC.Count != k)
            {
                cluster.PC.Clear();
                cluster.dist_error.Clear();
                seedPoints = GetSeedPoints(allPoints, k);
                goto begin;
            }

            List<Case> newSeeds = GetCentroid(cluster);
            List<Case> n = new List<Case>();
            bool found = true;
            //Determine exit
            // check if centers don't change, equality between cases
            foreach (Case newSeed in newSeeds) // last centers
            {
                foreach (KeyValuePair<Case, List<Case>> item in cluster.PC)//current centers
                {
                    found = true;
                    foreach (Feature f in item.Key.GetFeatures())
                    {
                        Feature seedf = newSeed.GetFeature(f.GetFeatureName());
                        if (!(f.GetFeatureValue().ToString() == seedf.GetFeatureValue().ToString()))
                        { found = false; break; }
                    }
                    if (found) break;
                }
                if (!found) exit = false;
                //  if (!cluster.PC.ContainsKey(newSeed)) wrong for equal objects
                //   exit = false;
            }

            if ((exit) || (iter == 1000))
                return cluster;
            else
            {
                iter++;
                return GetKMeans(allPoints, newSeeds, k);
            }
        }

        /// <summary>
        /// Get the centroid of a set of points
        /// cf. http://en.wikipedia.org/wiki/Centroid
        /// Consider also: Metoid cf. http://en.wikipedia.org/wiki/Medoids
        /// </summary>
        /// <param name="pcs"></param>
        /// <returns></returns>
        private List<Case> GetCentroid(PointClusters pcs)
        {
            List<Case> newSeeds = new List<Case>(pcs.PC.Count);
            Case newSeed;
            int sumf = 0; double sumd = 0; bool sumb = false;
            List<string> sumst = new List<string>();
            List<int> sums_count = new List<int>();
            Feature f = new Feature();
            int t = 0;
            foreach (List<Case> cluster in pcs.PC.Values)
            {
                newSeed = new Case(cluster[0].GetCaseID(), cluster[0].GetCaseName(), cluster[0].GetCaseDescription());
                for (int j = 0; j < cluster[0].GetFeatures().Count; j++)
                {
                    foreach (Case p in cluster)
                    {
                        f = (Feature)p.GetFeatures()[j];
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
                    if (f.GetFeatureName() == "cluster")
                        newSeed.AddFeature(f.GetFeatureName(), f.GetFeatureType(), -1, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                    else if (f.GetFeatureName() == "id")
                        newSeed.AddFeature(f.GetFeatureName(), f.GetFeatureType(), t, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                    else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_INT)
                        newSeed.AddFeature(f.GetFeatureName(), f.GetFeatureType(), sumf / cluster.Count(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                    else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT)
                        newSeed.AddFeature(f.GetFeatureName(), f.GetFeatureType(), sumd / cluster.Count(), f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                    else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_BOOL)
                        newSeed.AddFeature(f.GetFeatureName(), f.GetFeatureType(), sumb, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                    else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_STRING || f.GetFeatureType() == FeatureType.TYPE_FEATURE_CATEGORICAL)
                    {
                        int max = sums_count.IndexOf(sums_count.Max());
                        string string_value = sumst[max];
                        newSeed.AddFeature(f.GetFeatureName(), f.GetFeatureType(), string_value, f.GetWeight(), f.GetIsKey(), f.GetIsIndex(), f.GetFeatureUnit());
                    }
                    sumf = 0;
                    sumd = 0;
                    sumb = false;
                    sumst.Clear();
                    sums_count.Clear();
                } // end feature
                newSeeds.Add(newSeed);
                sumf = 0;
                sumd = 0;
                sumb = false;
                sumst.Clear();
                sums_count.Clear();
                t++;
            }// end clusters

            return newSeeds;
        }


        private List<Case> GetSeedPoints(List<Case> allPoints, int k)
        {
            List<Case> seedPoints = new List<Case>(k);
            PointDetails pd;
            List<PointDetails> pds = new List<PointDetails>();
            int index = 0;

            //1. Choose 1 random point as first seed
            int firstIndex = GetRandNorm(0, allPoints.Count);
            Case FirstPoint = allPoints[firstIndex];
            seedPoints.Add(FirstPoint);

            for (int i = 0; i < k - 1; i++)
            {
                if (seedPoints.Count >= 2)
                {
                    //Get point with min distance
                    PointDetails minpd = GetMinDPD(pds);
                    index = GetWeightedProbDist(minpd.Weights, minpd.Sum);
                    Case SubsequentPoint = allPoints[index];
                    seedPoints.Add(SubsequentPoint);

                    pd = new PointDetails();
                    pd = GetAllDetails(allPoints, SubsequentPoint, pd);
                    pds.Add(pd);
                }
                else
                {
                    pd = new PointDetails();
                    pd = GetAllDetails(allPoints, FirstPoint, pd);
                    pds.Add(pd);
                    index = GetWeightedProbDist(pd.Weights, pd.Sum);
                    Case SecondPoint = allPoints[index];
                    seedPoints.Add(SecondPoint);

                    pd = new PointDetails();
                    pd = GetAllDetails(allPoints, SecondPoint, pd);
                    pds.Add(pd);
                }
            }

            return seedPoints;
        }

        /// <summary>
        /// Very simple weighted probability distribution. NB: No ranking involved.
        /// Returns a random index proportional to to D(x)^2
        /// </summary>
        /// <param name="w">Weights</param>
        /// <param name="s">Sum total of weights</param>
        /// <returns>Index</returns>
        private int GetWeightedProbDist(double[] w, double s)
        {
            double p = GetRandNumCrypto();
            double q = 0d;
            int i = -1;

            while (q < p)
            {
                i++;
                q += (w[i] / s);
            }
            return i;
        }

        //Gets a pseudo random number (of normal quality) in range: [0, 1)
        private double GetRandNorm()
        {
            Random seed = new Random();
            return seed.NextDouble();
        }

        //Gets a pseudo random number (of normal quality) in range: [min, max)
        private int GetRandNorm(int min, int max)
        {
            Random seed = new Random();
            return seed.Next(min, max);
        }

        //Pseudorandom number (of crypto strength) in range: [min,max) 
        private int GetRandNumCrypto(int min, int max)
        {
            byte[] salt = new byte[8];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(salt);
            return (int)((double)BitConverter.ToUInt64(salt, 0) / UInt64.MaxValue * (max - min)) + min;
        }

        //Pseudorandom number (of crypto strength) in range: [0.0,1.0) 
        private double GetRandNumCrypto()
        {
            byte[] salt = new byte[8];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(salt);
            return (double)BitConverter.ToUInt64(salt, 0) / UInt64.MaxValue;
        }


        //Gets the weight, sum, & min distance. Loop consolidation essentially.
        private PointDetails GetAllDetails(List<Case> allPoints, Case seedPoint, PointDetails pd)
        {
            double[] Weights = new double[allPoints.Count];
            double minD = double.MaxValue;
            double Sum = 0d;
            int i = 0;

            foreach (Case p in allPoints)
            {
                if (p == seedPoint) //Delta is 0
                {
                    i++;
                    continue;
                }

                Weights[i] = GetEuclideanD(p, seedPoint);
                Sum += Weights[i];
                if (Weights[i] < minD)
                    minD = Weights[i];
                i++;
            }

            pd.SeedPoint = seedPoint;
            pd.Weights = Weights;
            pd.Sum = Sum;
            pd.MinD = minD;

            return pd;
        }

        /// <summary>
        /// Simple Euclidean distance
        /// cf. http://en.wikipedia.org/wiki/Euclidean_distance
        /// Consider also: Manhattan, Chebyshev & Minkowski distances
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <returns></returns>
        private double GetEuclideanD(Case P1, Case P2)
        {
            EuclideanSimilarity sim = new EuclideanSimilarity();// the Pythagorean theorem we calculate the distance between two points
            return sim.Dissimilarity(P1, P2); // vyzvraschaem distance
            // return sim.Similarity(P1, P2); // vyzvraschaem distance
        }

        //Gets min distance from set of PointDistance objects. If similar then chooses random item.
        private PointDetails GetMinDPD(List<PointDetails> pds)
        {
            double minValue = double.MaxValue;
            List<PointDetails> sameDistValues = new List<PointDetails>();

            foreach (PointDetails pd in pds)
            {
                if (pd.MinD < minValue)
                {
                    sameDistValues.Clear();
                    minValue = pd.MinD;
                    sameDistValues.Add(pd);
                }
                if (pd.MinD == minValue)
                {
                    if (!sameDistValues.Contains(pd))
                        sameDistValues.Add(pd);
                }
            }

            if (sameDistValues.Count > 1)
                return sameDistValues[GetRandNumCrypto(0, sameDistValues.Count)];
            else
                return sameDistValues[0];
        }

    }
}
