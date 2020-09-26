using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fahp_cbr_app
{
    static public class  TriangleFuzzyScale
    {
        // static public   Dictionary<int, FuzzyNumber> ReversedFuzzyNumber = new Dictionary<int, FuzzyNumber>();
         static public Dictionary<int, FuzzyNumber> FuzzyNumber = new Dictionary<int, FuzzyNumber>();
        static public bool create_FuzzyScale(double a)
        {
            if (a >= 0.25 && a <= 2)
             if (FuzzyNumber.Count==0)
            {
                FuzzyNumber f = new FuzzyNumber(1, 1, 1 + a);
                FuzzyNumber.Add(1, f);
                f = new FuzzyNumber(9 - a, 9, 9);
                FuzzyNumber.Add(9, f);
                for (int i = 3; i <= 7; i = i + 2)
                {
                    f = new FuzzyNumber(i - a, i, i + a);
                    FuzzyNumber.Add(i, f);
                }
                for (int i = 2; i <= 8; i = i + 2)
                {
                    f = new FuzzyNumber(i - 1, i, i + 1);
                    FuzzyNumber.Add(i, f);
                }

                //RFuzzyNumber
                /*f = new FuzzyNumber(1, 1, 1/(1 + a));
                ReversedFuzzyNumber.Add(1, f);
                f = new FuzzyNumber(1/(9 - a), 1/9, 1/9);
                ReversedFuzzyNumber.Add(1/9, f);
                for (int i = 3; i <= 7; i = i + 2)
                {
                    f = new FuzzyNumber(1/(i - a),1/ i, 1/(i + a));
                    ReversedFuzzyNumber.Add(1/i, f);
                }


                for (int i = 2; i <= 8; i = i + 2)
                {
                    f = new FuzzyNumber(1/(i - 1), 1/i, 1/(i + 1));
                    ReversedFuzzyNumber.Add(1/i, f);
                }*/
                return true;
            }


            return false;
        }
    }
}
