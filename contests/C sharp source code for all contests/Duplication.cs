using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
class Solution
{
    static void Main(String[] args)
    {
        var lookup = duplicate10Times();

        int querires = Convert.ToInt32(Console.ReadLine());
        for (int i = 0; i < querires; i++)
        {
            int x = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine(lookup[x]);
        }
    }


    static string duplicate10Times()
    {
        var start = "0";

        int index = 0;
        while (index < 12)
        {
            // same order iterate, and also flip the number:  0 <-> 1
            StringBuilder flip = new StringBuilder();
            for (int i = 0; i < start.Length; i++)
            {
                var current = start[i];

                flip.Append(current == '0' ? '1' : '0');
            }

            start += flip.ToString();
            index++;
        }

        return start;
    }
}