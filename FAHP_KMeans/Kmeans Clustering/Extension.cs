using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Fahp_cbr_app
{
    public static class Extension
    {
        /// <summary>
        /// Euclidean distance between two points in a two dimensional space.
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>The distance</returns>
        public static double Distance(this Case p1, Case p2)
        {
            // call eucladian distance beteen tow cases
            EuclideanSimilarity sim = new EuclideanSimilarity();
            return sim.Similarity(p1, p2); 
        }

        /// <summary>
        /// Finds the minimum element in a mapped sequence returning both the element and its index.
        /// </summary>
        /// <typeparam name="T">The original type of the sequence, must implement IComparable</typeparam>
        /// <typeparam name="U">The type in which each element is mapped to</typeparam>
        /// <param name="this">The input sequence</param>
        /// <param name="mapping">The mapping T -> U</param>
        /// <returns>A tuple whose first element is the index and the second one is the smallest element</returns>
        public static Tuple<int, U> Min<T, U>(this IEnumerable<T> @this, Func<T, U> mapping)
            where U : IComparable<U>
        {

            int i = 0, minIndex = 0;
            U minValue = mapping(@this.First());
            foreach (T element in @this)
            {
                U value = mapping(element);
                if (value.CompareTo(minValue) < 0)
                {
                    minValue = value;
                    minIndex = i;
                }

                i += 1;
            }

            return new Tuple<int, U>(minIndex, minValue);
        }

        /// <summary>
        /// Enumerates a sequence yielding every element along with its index
        /// </summary>
        /// <typeparam name="T">Type of the sequence</typeparam>
        /// <param name="sequence">Sequence to enumerate</param>
        /// <returns>A tuple whose first element is the index and second element is the 
        /// corresponding element in the input sequence</returns>
        public static IEnumerable<Tuple<int, T>> Enumerate<T>(this IEnumerable<T> sequence)
        {
            int i = 0;
            foreach (T element in sequence)
            {
                yield return new Tuple<int, T>(i, element);
                i += 1;
            }
        }
    }
}
