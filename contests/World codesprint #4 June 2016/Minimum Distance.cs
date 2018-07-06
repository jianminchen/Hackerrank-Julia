using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{

    static void Main(String[] args)
    {
        int n = Convert.ToInt32(Console.ReadLine());
        string[] A_temp = Console.ReadLine().Split(' ');
        int[] A = Array.ConvertAll(A_temp, Int32.Parse);
        Console.WriteLine(LongestDistance(A));
    }

    public static int LongestDistance(int[] arr)
    {
        Dictionary<int, int> startIndex = new Dictionary<int, int>();

        int min = Int32.MaxValue;
        bool found = false;
        for (int i = 0; i < arr.Length; i++)
        {
            int runner = arr[i];
            if (!startIndex.ContainsKey(runner))
            {
                startIndex[runner] = i;
            }
            else
            {
                found = true;
                int start = startIndex[runner];
                min = (i - start) > min ? min : (i - start);
            }
        }

        if (!found)
            return -1;

        return min;
    }
}
