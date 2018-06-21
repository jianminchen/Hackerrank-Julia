using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    static void Main(String[] args)
    {
        int t = Convert.ToInt32(Console.ReadLine());
        for (int index = 0; index < t; index++)
        {
            string[] tokens_a = Console.ReadLine().Split(' ');

            int a = Convert.ToInt32(tokens_a[0]);
            int b = Convert.ToInt32(tokens_a[1]);
            int c = Convert.ToInt32(tokens_a[2]);
            int d = Convert.ToInt32(tokens_a[3]);

            string[] tokens_p = Console.ReadLine().Split(' ');
            int p = Convert.ToInt32(tokens_p[0]);
            int q = Convert.ToInt32(tokens_p[1]);

            int answer = productOfLegoTypes(a, b, c, d, p, q);
            Console.WriteLine(answer);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    /// <param name="p"></param>
    /// <param name="q"></param>
    /// <returns></returns>
    static int productOfLegoTypes(int a, int b, int c, int d, int p, int q)
    {
        return a * b * c * d / (p * q);
    }
}

