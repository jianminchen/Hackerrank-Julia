using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaximumGcd
{
    /// <summary>
    /// week of code 34 
    /// https://www.hackerrank.com/contests/w34/challenges/maximum-gcd-and-sum/problem
    /// </summary>
    class Program
    {
        static void Main(String[] args)
        {
            Process();
            //RunTestcase(); 
        }

        static void RunTestcase()
        {
            var array1 = new int[] { 3, 1, 4, 2, 8 };
            var array2 = new int[] { 5, 2, 12, 8, 3 };

            var gcd = maximumGcdAndSum(array1, array2);
        }

        static void Process()
        {
            int n = Convert.ToInt32(Console.ReadLine());

            var A_temp = Console.ReadLine().Split(' ');
            var A = Array.ConvertAll(A_temp, Int32.Parse);

            var B_temp = Console.ReadLine().Split(' ');
            var B = Array.ConvertAll(B_temp, Int32.Parse);

            int res = maximumGcdAndSum(A, B);
            Console.WriteLine(res);
        }

        /// <summary>
        /// code review on July 24, 2017
        /// time complexity: 
        /// 
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="brr"></param>
        static int maximumGcdAndSum(int[] arr, int[] brr)
        {
            int N = (int)1e6 + 6;
            // count array stores the count of each number in array A
            var countA = getCount(arr);

            // multipleA[i] stores the largest multiple of i, present in A.
            // read sieve wiki article to get the idea to remove non-prime number
            var multipleA = getMultiple(countA);

            var countB = getCount(brr);
            var multipleB = getMultiple(countB);

            int maximumSumOfPair = 0;

            /// iterate in decreasing order, find the first one - great common divisor
            for (int i = N - 1; i >= 1; i--)
            {
                if (multipleA[i] > 0 && multipleB[i] > 0)
                {
                    maximumSumOfPair = i;
                    break;
                }
            }

            return multipleA[maximumSumOfPair] + multipleB[maximumSumOfPair];
        }

        /// <summary>
        /// code review on July 24, 2017
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private static int[] getCount(int[] arr)
        {
            const int N = (int)1e6 + 6;

            var count = new int[N];

            int n = arr.Length;
            for (int i = 0; i < n; ++i)
            {
                ++count[arr[i]];
            }

            return count;
        }

        /// <summary>
        /// code review 
        /// sieve - prime number sieve idea
        /// 
        /// </summary>
        /// <param name="countArray"></param>
        /// <returns></returns>
        private static int[] getMultiple(int[] countArray)
        {
            const int N = (int)1e6 + 6;
            var multiple = new int[N];

            for (int i = 1; i < N; ++i)
            {
                for (int j = i; j < N; j += i)
                {
                    if (countArray[j] > 0)
                    {
                        multiple[i] = Math.Max(multiple[i], j);
                    }
                }
            }

            return multiple;
        }
    }
}
