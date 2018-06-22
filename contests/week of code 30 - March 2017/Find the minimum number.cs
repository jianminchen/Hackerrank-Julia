using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindTheMinimumNumber
{
    /// <summary>
    /// https://www.hackerrank.com/contests/w30/challenges/find-the-minimum-number
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine(OutputMinFunctionCalls(n));
        }

        /// <summary>
        ///  2 <= n <= 50
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string OutputMinFunctionCalls(int n)
        {
            if (n == 2)
            {
                return "min(int, int)";
            }

            return "min(int, " + OutputMinFunctionCalls(n - 1) + ")";
        }
    }
}
