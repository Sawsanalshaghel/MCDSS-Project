using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fahp_cbr_app.Kmeans_Clustering
{
    class KmeansPP
    {
            int kClusters;
        List<List<Case>> data;
        public KmeansPP(int _k, List<List<Case>> _data)
        {
            data = _data;
            kClusters = _k;
        }
        bool notEqual(List<int> a, List<int> b)
        {
            for (int i = 0; i < a.Count; ++i)
                if (a[i] != b[i])
                    return true;
            return a.Count==0?true:false;
        }
        public List<List<Case>> runKMean()
        {
            List<List<double>> C;

            if (kClusters == data.Count)
                return new List<List<Case>>(data);
            Initialize(out C);

            List<int> S = new List<int>(), SOld = new List<int>();
            for (int i = 0; i < data.Count; ++i) { S.Add(-1); SOld.Add(-2); }
            while (notEqual(S, SOld))
            {
                SOld = new List<int>(S);
                updateS(C, S);
                updateC(C, S);
            }
            return C;
        }
        void updateC(List<List<Case>> C, List<int> S)
        {
            for (int i = 0; i < C.Count; ++i)
                C[i] = mean(S,i);
        }
        void adder(List<double> src, List<double> dst)
        {
            for (int i = 0; i < src.Count; ++i)
                src[i] += dst[i];
        }
        List<Case> mean(List<int> S, int idx)
        { 
            List<Case> curMean = new List<Case>();
            double clusterSize = 0;
            for (int i = 0; i < data[0].Count; ++i)
                curMean.Add(0);
            for (int i = 0; i < S.Count; ++i)
                if (S[i] == idx)
                {
                    clusterSize++;
                    for (int j = 0; j < curMean.Count; ++j)
                        curMean[j] += data[i][j];
                }
            for (int i = 0; i < curMean.Count; ++i)
                curMean[i]/=clusterSize;
            return curMean;
        }
        void updateS(List<List<Case>> C, List<int> S)
        {
            for (int i = 0; i < S.Count; ++i)
                S[i] = minDistID(C, i);
        }
        void Initialize(out List<List<double>> C)
        {
            Random rand = new Random();
            C = new List<List<Case>>();
            C.Add(data[(int)(rand.NextDouble() * data.Count)]);

            for (int i = 1; i < kClusters; ++i)
                C.Add(data[getRandPoint(getCumProb(C),rand.NextDouble())]);
        }
        int getRandPoint(List<Case> cProb,double randNum)
        {
            for (int i = 0; i < cProb.Count; ++i)
                if (randNum < cProb[i])
                    return i;
            return cProb.Count - 1;
        }
        List<double> getCumProb(List<List<Case>> C)
        {
            List<double> cProb = new List<double>();
            double phi = phiError(C);
            for (int i = 0; i < data.Count; ++i)
                cProb.Add(minDist(C, i) / phi);
            
            for (int i = 1; i < data.Count; ++i)
                cProb[i] += cProb[i-1];
            
            return cProb;
        }
        double phiError(List<List<Case>> C)
        { 
            double error=0;
            for(int i=0;i<data.Count;++i)
                error += minDist(C,i); 
            return error;
        }
        double minDist(List<List<Case>> C, int dataIdx)
        {
            double minVal = 1e9;
            for (int j = 0; j < C.Count; ++j)
                minVal = Math.Min(minVal, dist(C[j], data[dataIdx]));
            return minVal;
        }
        int minDistID(List<List<Case>> C, int dataIdx)
        {
            double minVal = 1e9;
            int minIdx = -1;
            for (int j = 0; j < C.Count; ++j)
            {
                double curDist = dist(C[j], data[dataIdx]);
                if ( curDist < minVal)
                {
                    minIdx = j;
                    minVal = curDist;
                }
            }
            return minIdx;
        }
        double dist(List<Case> a, List<Case> b)
        { 
            double d = 0;
            for (int i = 0; i < a.Count; ++i)
                d += (a[i] - b[i])*(a[i] - b[i]);
            return d;
        }
        double computeCost(List<Case> point)
        { 
            double cost=0;
            for (int i = 0; i < data.Count; ++i)
                cost += dist(point, data[i]);
            return cost;
        }
    }
}
