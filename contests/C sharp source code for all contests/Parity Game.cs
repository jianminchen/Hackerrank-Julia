using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    /// <summary>
    /// code review June 22, 2018
    /// https://www.hackerrank.com/contests/hourrank-22/challenges/parity-game
    /// </summary>
    /// <param name="n"></param>
    /// <param name="A"></param>
    /// <returns></returns>
    private static int smallestSizeSubsequence(int n, int[] A)
    {
        int length = A.Length;
        int countOdd = 0;
        for (int i = 0; i < length; i++)
        {
            var visit = A[i];
            bool isOdd = visit % 2 == 1;
            if (isOdd)
            {
                countOdd++;
            }
        }

        if (countOdd % 2 == 0)
        {
            return 0;
        }
        else
        {
            if (length == 1) return -1;
            return 1;
        }
    }

    static void Main(String[] args)
    {
        ProcessInput();
        // RunTestcase();
    }

    public static void RunTestcase()
    {
        smallestSizeSubsequence(5, new int[] { 1, 2, 3, 4, 5 });
    }

    public static void ProcessInput()
    {
        int n = Convert.ToInt32(Console.ReadLine());
        string[] A_temp = Console.ReadLine().Split(' ');
        int[] A = Array.ConvertAll(A_temp, Int32.Parse);
        int result = smallestSizeSubsequence(n, A);
        Console.WriteLine(result);
    }
}
