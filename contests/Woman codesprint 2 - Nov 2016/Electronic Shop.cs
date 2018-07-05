using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{

    static void Main(String[] args)
    {
        string[] tokens_s = Console.ReadLine().Split(' ');
        int s = Convert.ToInt32(tokens_s[0]);
        int n = Convert.ToInt32(tokens_s[1]);
        int m = Convert.ToInt32(tokens_s[2]);
        string[] keyboards_temp = Console.ReadLine().Split(' ');
        int[] keyboards = Array.ConvertAll(keyboards_temp, Int32.Parse);
        string[] pendrives_temp = Console.ReadLine().Split(' ');
        int[] pendrives = Array.ConvertAll(pendrives_temp, Int32.Parse);

        int max = Int32.MinValue;
        for (int i = 0; i < keyboards.Length; i++)
            for (int j = 0; j < pendrives.Length; j++)
            {
                int tmp = keyboards[i] + pendrives[j];
                if (tmp <= s && tmp > max)
                    max = tmp;
            }

        Console.WriteLine((max > 0) ? max.ToString() : "-1");
    }
}
