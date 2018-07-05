using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimumAbsoluteDifferenceInAnArray
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());

            long[] values = Array.ConvertAll(Console.ReadLine().Split(' '), long.Parse);

            Console.WriteLine(CalculateMinimumAbsolutionDifference(values));
        }

        /*
         * First, sort the array, and then find the minimum absolution value of difference 
         * 2 < n < 100000
         * -10^9 < value < 10^9
         */
        public static long CalculateMinimumAbsolutionDifference(long[] values)
        {
            Array.Sort(values);

            long minValue = Math.Abs(values[1] - values[0]);

            for (int i = 2; i < values.Length; i++)
            {
                long newValue = Math.Abs(values[i] - values[i - 1]);
                minValue = (newValue < minValue) ? newValue : minValue;
            }

            return minValue;
        }
    }
}
