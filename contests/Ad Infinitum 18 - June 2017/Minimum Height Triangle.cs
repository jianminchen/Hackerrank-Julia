using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{

    static void Main(String[] args)
    {
        string[] tokens_base = Console.ReadLine().Split(' ');
        int baseValue = Convert.ToInt32(tokens_base[0]);
        int area = Convert.ToInt32(tokens_base[1]);
        int height = lowestTriangle(baseValue, area);

        Console.WriteLine(height);
    }

    private static int lowestTriangle(int baseValue, int area)
    {
        int height = 2 * area / baseValue;
        if (2 * area % baseValue > 0)
        {
            height++;
        }

        return height;
    }
}