using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class FuzzyNumber
    {
        
            public double l = 0;
            public double m = 0;
            public double u = 0;

        public FuzzyNumber()
        {
        }
        public FuzzyNumber(double l, double m ,  double u )
        {
            this.l = l;
            this.m = m;
            this.u = u;

        }

        public FuzzyNumber Reversed()
        {
            FuzzyNumber f = new FuzzyNumber(1 / this.u, 1 / this.m, 1 / this.l);
            return f;
        }
        
    }

