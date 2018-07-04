using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    static void Main(String[] args)
    {
        int a = Convert.ToInt32(Console.ReadLine());
        long[] triple = pythagoreanTriple(a);
        Console.WriteLine(String.Join(" ", triple));
    }

    static long[] pythagoreanTriple(int a)
    {
        bool isOdd = a % 2 == 1;
        if (isOdd)
        {
            return pythagoreanTriple_Odd(a);
        }
        else
        {
            return pythagoreanTriple_Even(a);
        }
    }

    static long[] pythagoreanTriple_Even(int a)
    {
        // 2K + 1 = (k+1)^2 - K^2
        // Complete this function        
        int m = a / 2;
        int n = 1;
        long value1 = Math.Abs((long)m * m - (long)n * n);
        long value2 = (long)m * m + (long)n * n;

        return new long[] { a, value1, value2 };
    }

    static long[] pythagoreanTriple_Odd(int a)
    {
        // 2K + 1 = (k+1)^2 - K^2
        // Complete this function
        int k = (a - 1) / 2;
        int m = k + 1;
        int n = k;
        long value1 = 2 * (long)m * n;
        long value2 = (long)m * m + (long)n * n;

        return new long[] { a, value1, value2 };
    }
}
