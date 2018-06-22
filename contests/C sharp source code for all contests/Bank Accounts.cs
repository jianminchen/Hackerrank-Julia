using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    /// <summary>
    /// https://www.hackerrank.com/contests/gs-codesprint/challenges/bank-accounts
    /// </summary>
    /// <param name="args"></param>
    static void Main(String[] args)
    {
        int q = Convert.ToInt32(Console.ReadLine());
        for (int a0 = 0; a0 < q; a0++)
        {
            string[] tokens_n = Console.ReadLine().Split(' ');

            int n = Convert.ToInt32(tokens_n[0]);
            int k = Convert.ToInt32(tokens_n[1]);
            int x = Convert.ToInt32(tokens_n[2]);
            int d = Convert.ToInt32(tokens_n[3]);

            string[] p_temp = Console.ReadLine().Split(' ');
            int[] p = Array.ConvertAll(p_temp, Int32.Parse);

            string result = CalculateUsingFeeOrUpfront(n, k, x, d, p);

            Console.WriteLine(result);
        }
    }

    public static string CalculateUsingFeeOrUpfront(int noOfTransactions, int minimum, int percentage, int upfront, int[] payments)
    {
        var decision = new string[] { "upfront", "fee" };

        double cost = 0;

        for (int i = 0; i < noOfTransactions; i++)
        {
            var current = payments[i];
            cost += Math.Max(minimum, percentage * current / 100.0);
        }

        bool isFee = cost <= upfront;
        if (isFee)
        {
            return decision[1];
        }
        else
        {
            return decision[0];
        }
    }
}
