using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stoneDivisionStudyCode
{
    /*
    * 
    * Problem statement:
    * https://www.hackerrank.com/contests/womens-codesprint-2/challenges/stone-division-2
    *        
    */
    class StoneDivsion
    {
        public static Dictionary<long, long> memo = new Dictionary<long, long>();

        static void Main(string[] args)
        {
            ProcessInput();
            //RunTestcase(); 
        }

        public static void ProcessInput()
        {
            int queries = int.Parse(Console.ReadLine());

            for (int query = 0; query < queries; query++)
            {
                memo.Clear();

                string[] input = Console.ReadLine().Split(' ');

                long n = long.Parse(input[0]);
                int m = int.Parse(input[1]);

                long[] predefinedSet = new long[1003];

                predefinedSet = Array.ConvertAll(Console.ReadLine().Split(' '), long.Parse);

                long answer = CalculateMaximumPossibleMoves(n, predefinedSet, m);

                Console.WriteLine(answer);
            }
        }

        /*
         * 1
         * 12 3
         * 2  3  4
         * Go over problem statement and explanation 
         * of sample test case
         */
        public static void RunTestcase()
        {
            memo.Clear();

            long n = 12;
            int m = 3;

            var predefinedSet = new long[3] { 2, 3, 4 };

            long answer = CalculateMaximumPossibleMoves(n, predefinedSet, m);

            Debug.Assert(answer == 4);
        }

        /*
        * Recursive and memorization two techniques                                   
        *             
        * Recursive function design - 
        * 
        * Timeout issue - using memorization
        * maximum size of integers < 1000
        * 1 <= m <= 1000 
        */
        public static long CalculateMaximumPossibleMoves(
            long pile,
            long[] predefinedSet,
            int sizeOfPredefinedSet)
        {
            // look up memorization dictionary 
            if (memo.ContainsKey(pile))
            {
                return memo[pile];
            }

            // go over all possible options to find maximum number of moves
            long maximumNumberOfMoves = 0;
            for (int i = 0; i < sizeOfPredefinedSet; i++)
            {
                long divisor = predefinedSet[i];

                if (pile % divisor != 0 || (pile / divisor <= 1))
                {
                    continue;
                }

                // solve a subproblem using recurisve function - a small pile
                long numberOfMoves = CalculateMaximumPossibleMoves(divisor, predefinedSet, sizeOfPredefinedSet);

                // how many piles - count in the factor of piles
                // first divide into small piles, then each small pile will be processed. 
                numberOfMoves = 1 + (pile / divisor) * numberOfMoves;

                // keep the maximum one only
                maximumNumberOfMoves = Math.Max(maximumNumberOfMoves, numberOfMoves);
            }

            memo[pile] = maximumNumberOfMoves;

            return maximumNumberOfMoves;
        }
    }
}