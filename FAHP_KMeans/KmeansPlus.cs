using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Security.Cryptography;

namespace Fahp_cbr_app
{
    public class KmeansPlus
    {



    //Output object
        public class PointClusters
        {
            private Dictionary<Case, List<Case>> _pc = new Dictionary<Case, List<Case>>();

            public Dictionary<Case, List<Case>> PC
            {
                get { return _pc; } set { _pc = value; }
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
                get { return _seedpoint; } set { _seedpoint = value; }
            }

            public double[] Weights
            {
                get { return _Weights; } set { _Weights = value; }
            }

            public double Sum
            {
                get { return _Sum; } set { _Sum = value; }
            }

            public double MinD
            {
                get { return _minD; } set { _minD = value; }
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

        //Bog standard k-means.
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
                }
                else
                {   //Existing cluster centre   
                    cluster.PC[keyPoint].Add(p);
                }

                //Reset
                sameDPoint.Clear();
                minD = double.MaxValue;
            }

            //Bulletproof check - it it come out of the wash incorrect then re-seed.
            if (cluster.PC.Count != k)
            {
                cluster.PC.Clear();
                seedPoints = GetSeedPoints(allPoints, k);
                goto begin;
            }

            List<Case> newSeeds = GetCentroid(cluster);

            //Determine exit
            foreach (Case newSeed in newSeeds)
            {
                if (!cluster.PC.ContainsKey(newSeed))
                    exit = false;
            }

            if ((exit) || (iter==1000))
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
            int sumf = 0; double sumd = 0; bool sumb = false; List<string> sumst=new List<string>() ;
            Feature f = new Feature();

            foreach (List<Case> cluster in pcs.PC.Values)
            {
                newSeed = new Case(cluster[0].GetCaseID(), cluster[0].GetCaseName(), cluster[0].GetCaseDescription());
                for (int j = 0; j < cluster[0].GetFeatures().Count; j++)
                {
                    foreach (Case p in cluster)
                    {
                        f = (Feature)p.GetFeatures()[j];
                        if (f.GetFeatureName() == "id")
                            break; 
                        if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_INT)
                            sumf += Convert.ToInt32(f.GetFeatureValue());
                        else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT)
                            sumd += Convert.ToDouble(f.GetFeatureValue());
                        else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_BOOL)
                            sumb = (sumb || Convert.ToBoolean(f.GetFeatureValue()));
                        else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_STRING)
                            sumst.Add(f.GetFeatureValue().ToString());
                    }
                    if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_INT)
                        newSeed.AddFeature(f.GetFeatureName(), f.GetFeatureType(), sumf / cluster.Count(), 1.0, false, false, "");
                    else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_FLOAT)
                        newSeed.AddFeature(f.GetFeatureName(), f.GetFeatureType(), sumd / cluster.Count(), 1.0, false, false, "");
                    else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_BOOL)
                        newSeed.AddFeature(f.GetFeatureName(), f.GetFeatureType(), sumb, 1.0, false, false, "");
                    else if (f.GetFeatureType() == FeatureType.TYPE_FEATURE_STRING)
                    {
                        Random rd = new Random(); // for calculating random numbers
                        int rnd = rd.Next(sumst.Count);
                        newSeed.AddFeature(f.GetFeatureName(), f.GetFeatureType(), sumst[rnd].ToString(), 1.0, false, false, "");

                    }
                    //  newSeed = new Case(sumX / cluster.Count, sumY / cluster.Count);
                    // newSeeds.Add(newSeed);
                    
                }
                newSeeds.Add(newSeed);
                sumf = 0;
                sumd = 0;
                sumb = false;
                sumst.Clear();
            }

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
                    continue;

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
            return sim.Compare(P1, P2); // vyzvraschaem distance
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


