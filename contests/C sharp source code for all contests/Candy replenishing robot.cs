using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyReplenishingRobot
{
    /// <summary>
    /// code review June 22, 2018
    /// https://www.hackerrank.com/contests/w30/challenges/candy-replenishing-robot/problem
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            ProcessInput();
            //RunTestcase(); 
        }

        public static void RunTestcase()
        {
            int n = 8;
            int t = 5;

            var candiesRemoved = new int[] { 3, 1, 7, 5 };
            int candiesToAdd = CalculateTotalNumberOfCandiesAdded(n, t, candiesRemoved);
        }

        public static void ProcessInput()
        {
            string[] tokens_n = Console.ReadLine().Split(' ');
            int n = Convert.ToInt32(tokens_n[0]);
            int t = Convert.ToInt32(tokens_n[1]);
            string[] c_temp = Console.ReadLine().Split(' ');
            int[] candiesRemoved = Array.ConvertAll(c_temp, Int32.Parse);
            // your code goes here
            Console.WriteLine(CalculateTotalNumberOfCandiesAdded(n, t, candiesRemoved));
        }
        /*
         * if candies < 5, add n - candies to the bowl 
         * 1 <= t <= 100
         */
        public static int CalculateTotalNumberOfCandiesAdded(int n, int t, int[] candiesRemoved)
        {
            int candiesToAdd = 0;

            int candies = n;
            for (int i = 0; i < t; i++)
            {
                candies -= candiesRemoved[i];

                if (candies < 5 && i < t - 1)
                {
                    candiesToAdd += n - candies;
                    candies = n;
                }
            }

            return candiesToAdd;
        }
    }
}
