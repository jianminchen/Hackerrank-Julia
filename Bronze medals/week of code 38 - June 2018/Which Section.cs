using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;

class Solution
{

    /// <summary>
    /// code review on June 24, 2018
    /// https://www.hackerrank.com/contests/w38/challenges/which-section/problem
    /// Failed a few test cases, so I made a few changes:
    /// 1. prefixSum array length is increased to length + 1.
    /// 2. whichSection function for loop upperbound n -> n + 1
    /// 3. if statement k < prefixSum[i] <- add equal sign
    /// </summary>
    /// <param name="n"></param>
    /// <param name="k"></param>
    /// <param name="numbers"></param>
    /// <returns></returns>
    static int whichSection(int n, int k, int[] numbers)
    {
        // Return the section number you will be assigned to assuming you are student number k.
        var prefixSum = getPrefixSum(numbers);
        for (int i = 0; i < n + 1; i++)
        {
            if (k <= prefixSum[i])
            {
                return i;
            }
        }

        return n + 1;
    }
    static int[] getPrefixSum(int[] numbers)
    {
        var length = numbers.Length;
        var prefixSum = new int[length + 1];

        int sum = 0;
        for (int i = 0; i < length; i++)
        {
            prefixSum[i] = sum;
            sum += numbers[i];
        }

        prefixSum[length] = sum;

        return prefixSum;
    }

    static void Main(string[] args)
    {
        TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

        int t = Convert.ToInt32(Console.ReadLine());

        for (int tItr = 0; tItr < t; tItr++)
        {
            string[] nkm = Console.ReadLine().Split(' ');

            int n = Convert.ToInt32(nkm[0]);

            int k = Convert.ToInt32(nkm[1]);

            int m = Convert.ToInt32(nkm[2]);

            int[] a = Array.ConvertAll(Console.ReadLine().Split(' '), aTemp => Convert.ToInt32(aTemp))
            ;
            int result = whichSection(n, k, a);

            textWriter.WriteLine(result);
        }

        textWriter.Flush();
        textWriter.Close();
    }
}
