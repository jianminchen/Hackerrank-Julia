using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    static void Main(String[] args)
    {
        int q = Convert.ToInt32(Console.ReadLine());
        for (int a0 = 0; a0 < q; a0++)
        {
            string s = Console.ReadLine();
            int result = patternCount(s);
            Console.WriteLine(result);
        }
    }

    public static int patternCount(string s)
    {
        if (s == null || s.Length < 3)
        {
            return 0;
        }

        int index = 0;
        int length = s.Length;
        int foundPattern = 0;

        while (index < length)
        {
            var visit = s[index];
            bool isOne = visit == '1';
            if (!isOne)
            {
                index++;
                continue; // 01
            }

            //go through at least one zero or more zero, and end with 1
            int start = index + 1;
            bool foundZero = false;
            while (start < length && s[start] == '0')
            {
                foundZero = true;
                start++; //1001
            }

            index = start; // 1001, 100a, 1a

            if (foundZero && start < length)
            {
                bool endWithOne = s[start] == '1';
                if (endWithOne)
                {
                    foundPattern += 1;
                }
            }
        }

        return foundPattern;
    }
}
