#if DEBUG
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxScore_usingBitArray
{
    class MaxScore_usingBitMask
    {
        /// <summary>
        /// source code reference is here:
        /// https://www.hackerrank.com/contests/rookierank-3/challenges/max-score/forum/comments/299005
        ///
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            ProcessInput();
            //RunTestcase(); 
        }

        public static void ProcessInput()
        {
            int n = Convert.ToInt32(Console.ReadLine());

            var data = Console.ReadLine().Split(' ');
            long[] numbers = Array.ConvertAll(data, Int64.Parse);

            long maxScore = GetMaximumScore(numbers);
            Console.WriteLine(maxScore);
        }

        /// <summary>
        /// How to calculate score? 
        /// Please read the problem statement. 
        /// For test case int[]{1,2}, if first number array 1 is selected first, score 0 % 1 = 0. 
        /// Sum will be 1, and then number 2 will be scored as 1 % 2 = 1. Total score is 0 + 1 = 1. 
        /// There are 2 options to enumerate 2 numbers, maximum score is to choose maximum one of 
        /// those two options. 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static long GetMaximumScore(long[] array)
        {
            return getMaxScore(array, 0, array.Sum());
        }

        public static Dictionary<int, long> memo = new Dictionary<int, long>();

        /// <summary>
        /// Bit mask technique to solve timeout issue - 3 seconds time limit. 
        /// use the following article on topcoder for reference: 
        /// https://www.topcoder.com/community/data-science/data-science-tutorials/a-bit-of-fun-fun-with-bits/
        /// The following function uses an integer to represent a set, with a 1 bit representing
        /// a member that is present and a 0 bit one that is absent. 
        /// The following set operations are used in the function:
        /// Set Union  A | B
        /// Clear bit 
        /// A &= ~(1 << bit)
        /// Test bit  (A & 1 << bit) != 0
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="bitmask"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        private static long getMaxScore(long[] numbers, int bitmask, long sum)
        {
            if (memo.ContainsKey(bitmask))
            {
                return memo[bitmask];
            }

            var maximumScore = 0L;

            for (var i = 0; i < numbers.Length; i++)
            {
                var bitToCheck = 1 << i; // set ith bit

                if ((bitmask & bitToCheck) != 0) // test bit
                {
                    continue;
                }

                bitmask |= bitToCheck; // set union 

                var current = numbers[i];

                var score = ((sum - current) % current) + getMaxScore(numbers, bitmask, sum - current);

                bitmask &= ~bitToCheck; // backtracking, clear the bit - ith bit 
                maximumScore = Math.Max(score, maximumScore);
            }

            memo[bitmask] = maximumScore;

            return maximumScore;
        }
    }

#if DEBUG
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void Test1()
        {                        
            var array = new long[] { 1, 2};

            MaxScore_usingBitMask.memo.Clear(); 
            long maxScore = MaxScore_usingBitMask.GetMaximumScore(array);
            System.Diagnostics.Debug.Assert(maxScore == 1); 
        }

        [TestMethod]
        public void Test2()
        {
            var array = new long[] { 1, 2, 1 };

            MaxScore_usingBitMask.memo.Clear();
            long maxScore = MaxScore_usingBitMask.GetMaximumScore(array);
            System.Diagnostics.Debug.Assert(maxScore == 1);
        }  
    }

#endif
}