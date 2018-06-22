using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    static void Main(String[] args)
    {
        var queries = Convert.ToInt32(Console.ReadLine());

        for (int index = 0; index < queries; index++)
        {
            int k = Convert.ToInt32(Console.ReadLine());
            int n = Convert.ToInt32(Console.ReadLine());

            var strings = Console.ReadLine().Split(' ');

            var numbers = Array.ConvertAll(strings, Int32.Parse);

            int result = CalculateMaximumProfit(k, n, numbers);
            Console.WriteLine(result);
        }
    }

    /// <summary>
    /// copy the code written for Leetcode 123
    /// Source code link is 
    /// https://gist.github.com/jianminchen/6acc423e75760ad7d5313550bd52643e
    /// </summary>
    /// <param name="maximumTrades"></param>
    /// <param name="n"></param>
    /// <param name="prices"></param>
    /// <returns></returns>
    static int CalculateMaximumProfit(int maximumTrades, int n, int[] prices)
    {
        if (prices.Length <= 1)
        {
            return 0;
        }

        int maxProfit = 0;

        int length = prices.Length;

        var profit = new int[maximumTrades + 1][];
        for (int i = 0; i < maximumTrades + 1; i++)
        {
            profit[i] = new int[length];
        }

        for (int i = 1; i < maximumTrades + 1; i++)
        {
            // at most K =2 transaction, i is number of transaction
            int maximum = profit[i - 1][0] - prices[0];  // maximum value for all possible purchase time
            for (int j = 1; j < prices.Length; j++)
            {
                // j - time stamp
                // maximum subproblem
                var lastPurchaseTime = j;
                var current = profit[i - 1][lastPurchaseTime] - prices[lastPurchaseTime];
                maximum = Math.Max(maximum, current);

                // recurrence formula 
                profit[i][j] = Math.Max(profit[i][j - 1], prices[j] + maximum);

                maxProfit = Math.Max(profit[i][j], maxProfit);
            }
        }

        return maxProfit;
    }
}
