using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

/// <summary>
/// code study:
/// https://www.hackerrank.com/rest/contests/morgan-stanley-codeathon-2017/challenges/shell-sort-command/hackers/wes01/download_solution
/// 
/// code review: 
/// Nov. 14, 2017
/// Study IEnumerable<string> Skip API
/// https://msdn.microsoft.com/en-us/library/bb358985(v=vs.110).aspx
/// 
/// And also need to figure out 
/// </summary>
class Solution
{
    static void Main(String[] args)
    {
        int n = int.Parse(Console.ReadLine());
        var list = new string[n][];

        for (int i = 0; i < n; i++)
        {
            list[i] = (i.ToString() + " " + Console.ReadLine()).Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
        }

        var commands = Console.ReadLine().Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

        bool numeric = commands[2] == "numeric";
        bool reversal = commands[1] == "true";

        int column = int.Parse(commands[0]);

        Array.Sort(list, (a, b) =>
        {
            var key1 = Get(a, column);
            var key2 = Get(b, column);

            int comparison = numeric ? NumericComparer(key1, key2, a[0], b[0]) : StringCompare(key1, key2);
            if (comparison != 0)
            {
                return comparison;
            }

            return int.Parse(a[0]).CompareTo(int.Parse(b[0])); // compare the index instead
        });

        if (reversal)
        {
            Array.Reverse(list);
        }

        foreach (var x in list)
        {
            Console.WriteLine(string.Join(" ", x.Skip(1)));
        }
    }


    public static int StringCompare(string key1, string key2)
    {
        return key1.CompareTo(key2);
    }

    /// <summary>
    /// code review on Nov. 14,2017
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static int NumericComparer(string a, string b, string index1, string index2)
    {
        var orderString1 = a.TrimStart('0');
        var orderString2 = b.TrimStart('0');

        int length1 = orderString1.Length;
        int length2 = orderString2.Length;

        if (length1 < length2)
        {
            return -1;
        }

        if (length2 < length1)
        {
            return 1;
        }

        if (orderString1 == orderString2) // add one more case
        {
            var aValue = Convert.ToInt32(index1);
            var bValue = Convert.ToInt32(index2);

            return aValue.CompareTo(bValue);
        }

        return orderString1.CompareTo(orderString2);
    }

    /// <summary>
    /// code review on Nov. 14, 2017
    /// </summary>
    /// <param name="columns"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static string Get(string[] columns, int index)
    {
        if (index >= columns.Length)
        {
            return "-1";
        }

        return columns[index];
    }
}