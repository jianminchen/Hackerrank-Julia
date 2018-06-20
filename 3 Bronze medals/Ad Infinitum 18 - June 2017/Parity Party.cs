using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    static void Main(String[] args)
    {
        string[] tokens_n = Console.ReadLine().Split(' ');
        int n = Convert.ToInt32(tokens_n[0]);
        int a = Convert.ToInt32(tokens_n[1]);
        int b = Convert.ToInt32(tokens_n[2]);
        int c = Convert.ToInt32(tokens_n[3]);
        int result = parityParty(n, a, b, c);
        Console.WriteLine(result);
    }

    /// <summary>
    /// n - n kinds of candies, n is also total of candies
    /// a - odd number of candies
    /// b - even number of candies
    /// c - don't care of candies 
    /// </summary>
    /// <param name="n"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    static int parityParty(int n, int a, int b, int c)
    {
        // Complete this function
        var odd = new int[a];
        var even = new int[b];
        var doNotCare = new int[c];

        return parityPartyCount(n, a, b, c, odd, even, doNotCare);
    }

    private static int parityPartyCount(int n, int a, int b, int c, int[] odd, int[] even, int[] doNotCare)
    {
        int modulo = 7340033;

        if (n == 0)
        {
            bool passBoth = checkOdd(odd) || checkEven(even);
            if (passBoth)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        if (n < (checkOddNumberMinimum(odd) + checkEvenNumberMinimum(even)))
        {
            return 0;
        }

        int sum = 0;

        for (int i = 0; i < odd.Length; i++)
        {
            odd[i] += 1;
            sum += parityPartyCount(n - 1, a, b, c, odd, even, doNotCare);
            sum = sum % modulo;
            odd[i] -= 1;
        }

        for (int i = 0; i < even.Length; i++)
        {
            even[i] += 1;
            sum += parityPartyCount(n - 1, a, b, c, odd, even, doNotCare);
            sum = sum % modulo;
            even[i] -= 1;
        }

        for (int i = 0; i < doNotCare.Length; i++)
        {
            doNotCare[i] += 1;
            sum += parityPartyCount(n - 1, a, b, c, odd, even, doNotCare);
            sum = sum % modulo;
            doNotCare[i] -= 1;
        }

        return sum;
    }

    /// <summary>
    /// Satified means that each person should be odd number
    /// </summary>
    /// <param name="numbers"></param>
    /// <returns></returns>
    private static bool checkOdd(int[] numbers)
    {
        foreach (var number in numbers)
        {
            if (number % 2 == 0)
            {
                return false;
            }
        }

        return true;
    }

    private static int checkOddNumberMinimum(int[] numbers)
    {
        int count = 0;
        foreach (var number in numbers)
        {
            if (number % 2 == 0)
            {
                count++;
            }
        }

        return count;
    }

    private static int checkEvenNumberMinimum(int[] numbers)
    {
        int count = 0;
        foreach (var number in numbers)
        {
            if (number % 2 == 1)
            {
                count++;
            }
        }

        return count;
    }

    private static bool checkEven(int[] numbers)
    {
        foreach (var number in numbers)
        {
            if (number % 2 == 1)
            {
                return false;
            }
        }

        return true;
    }

}