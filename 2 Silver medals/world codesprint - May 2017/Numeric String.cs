using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    static void Main(String[] args)
    {
        ProcessInput();
        //RunTestcase(); 
    }

    public static void RunTestcase()
    {
        GetMagicNumber("1221211111111111111", 3, 3, 5);
    }

    public static void ProcessInput()
    {
        string s = Console.ReadLine();
        string[] row = Console.ReadLine().Split(' ');

        int substringLength = Convert.ToInt32(row[0]);
        int baseNo = Convert.ToInt32(row[1]);
        int module = Convert.ToInt32(row[2]);

        long result = GetMagicNumber(s, substringLength, baseNo, module);
        Console.WriteLine(result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    /// <param name="k"></param>
    /// <param name="baseNo"></param>
    /// <param name="module"></param>
    /// <returns></returns>
    public static long GetMagicNumber(string s, int k, int baseNo, int module)
    {
        int length = s.Length;

        long sumSubstrings = 0;
        IList<long> memos = new List<long>();

        long memo = 0;
        for (int i = 0; i < k; i++)
        {
            var visit = s[i] - '0';
            memo *= baseNo;
            memo += visit;
            memo = memo % module;
        }

        memos.Add(memo);

        sumSubstrings += memo % module;

        int firstDigit = getFirstDigit(baseNo, k, module);

        for (int i = k; i < length; i++)
        {
            long remove = s[i - k] - '0';
            remove = remove * firstDigit % module;

            var visit = s[i] - '0';
            memo = memo > remove ? (memo - remove) : (memo + module - remove);
            memo *= baseNo;
            memo += visit;

            memo = memo % module;
            sumSubstrings += memo;
            memos.Add(memo);
        }

        return sumSubstrings;
    }

    /// <summary>
    ///  k has upper bound 3 * 100000
    /// </summary>
    /// <param name="baseNo"></param>
    /// <param name="k"></param>
    /// <param name="module"></param>
    /// <returns></returns>
    private static int getFirstDigit(int baseNo, int k, int module)
    {
        // in theory - Math.Pow(baseNo, k - 1)
        int residue = 1;
        for (int i = 0; i < k - 1; i++)
        {
            residue = residue * baseNo % module;
        }

        return residue;
    }
}