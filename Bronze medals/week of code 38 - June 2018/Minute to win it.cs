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

/// <summary>
/// code review June 24, 2018
/// https://www.hackerrank.com/contests/w38/challenges/minute-to-win-it
/// </summary>
class Solution
{

    // Complete the minuteToWinIt function below.
    static int minuteToWinIt(int[] numbers, int k)
    {
        // Return the minimum amount of time in minutes.               
        var length = numbers.Length;
        var hashSet = new HashSet<int>();
        for (int i = 0; i < length; i++)
        {
            hashSet.Add(i);
        }

        int index = 0;
        var maxMatching = 1;
        while (index < length)
        {
            if (!hashSet.Contains(index))
            {
                index++;
                continue;
            }

            var current = numbers[index];
            var matching = 1;
            for (int i = index + 1; i < length; i++)
            {
                var iterate = numbers[i];
                var expected = current + (i - index) * k;
                if (expected == iterate)
                {
                    hashSet.Remove(i);
                    matching++;
                }
            }

            maxMatching = matching > maxMatching ? matching : maxMatching;
            index++;
        }

        return length - maxMatching;
    }

    static void Main(string[] args)
    {
        TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

        string[] nk = Console.ReadLine().Split(' ');

        int n = Convert.ToInt32(nk[0]);

        int k = Convert.ToInt32(nk[1]);

        int[] a = Array.ConvertAll(Console.ReadLine().Split(' '), aTemp => Convert.ToInt32(aTemp))
        ;
        int result = minuteToWinIt(a, k);

        textWriter.WriteLine(result);

        textWriter.Flush();
        textWriter.Close();
    }
}
