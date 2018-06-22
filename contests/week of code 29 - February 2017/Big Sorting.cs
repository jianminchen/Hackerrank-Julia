using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigSorting_B
{
    /// <summary>
    /// https://www.hackerrank.com/contests/w29/challenges/big-sorting
    /// </summary>
    public class StringComparer : IComparer<string>
    {
        public int Compare(string s1, string s2)
        {
            int length1 = s1.Length;
            int length2 = s2.Length;

            if (length1 != length2)
            {
                if (length1 > length2)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }

            int start = 0;
            while (start < length1)
            {
                if (s1[start] != s2[start])
                {
                    if (s1[start] > s2[start])
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }

                start++;
            }

            return 0;
        }
    }

    class BigSorting_B
    {
        static void Main(string[] args)
        {
            var unsorted = ProcessInput();
            // var unsorted = new string[] {"314567","1","3","10","3","5" };
            // var sorted  = new SortedSet<string>(unsorted, new StringComparer());
            Array.Sort(unsorted, new StringComparer());

            foreach (string s in unsorted)
            {
                Console.WriteLine(s);
            }
        }

        public static string[] ProcessInput()
        {
            int n = Convert.ToInt32(Console.ReadLine());
            string[] unsorted = new string[n];
            for (int unsorted_i = 0; unsorted_i < n; unsorted_i++)
            {
                unsorted[unsorted_i] = Console.ReadLine();
            }

            return unsorted;
        }
    }
}
