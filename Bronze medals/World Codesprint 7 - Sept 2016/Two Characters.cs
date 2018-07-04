using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoCharacters
{
    class Program
    {
        /*
         * start: 1:36pm
         * 20 minutes reading already
         * continue to read problem statement. 
         * start coding: 
         * Brute force
         * 26 chars, 
         * Get array 26 - countA
         * a and b, countA[0]
         * Time complexity: 26 * 25 * n, n is the string length
         * 
         * start: 2:05pm - bug fixing 
         * score 1 out of 15
         * test case: 0, 2 ok, failed 0-30, 28 test cases
         * exit: 2:14pm 
         * bug fix: 
         * function maximumLength
         * int maxLen = 0;  // instead of Int32.MinValue
         * score 11 out of 15, failed 8 test cases 
         * 
         * start: 2:23pm - bug fixing
         * two distinct characters 
         * so, add one more condition 
         * Math.Min(v1, v2) == 0 - line 65
         * score: 11.50, failed 7 cases, in other words, fix 1 test case
         * 
         * start: 2:26pm - bug fixing 
         * read problem statement again and again
         * prepare a check list:
         * 1.  a to z, 0 to 25, array size 26
         * Lowercase English alphabetic letters
         * 2. |s| <= 1000, >=1 
         * 3. t is two distinct alternating characters
         * 
         * Relax the checking on function maximumLength
         *  // Math.Abs(v1 - v2) > 1 || 
         *  score 13.50/15, failed 3 test cases
         *  End of try
         *  
         * start 2:42pm - bug fixing 
         * 13.50/15 -> failed 3 test cases
         * Try to increase the array size from 26 -> 100 -> 1000 - 
         * it does not work
         * 
         * 2:55pm give up bug fixing, leave as is 
         */
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            string s = Console.ReadLine().Trim();

            Console.WriteLine(maximumLength(n, s));
        }

        /*
         * start: 1:45pm
         * end:  1:56pm 
         * counter example: 
         * ab - alternating, so Math.Abs(v1-v2) > 1 <- wrong
         * two distinct characters:
         * Math.Min(v1, v2) == 0 < exit 
         */
        public static int maximumLength(int n, string s)
        {
            if (n <= 0 || s.Length == 0)
                return 0;

            int SIZE = 26;
            int[] cA = counting(n, s);

            int maxLen = 0;
            for (int i = 0; i < SIZE; i++)
                for (int j = i + 1; j < SIZE; j++)
                {
                    int v1 = cA[i];
                    int v2 = cA[j];

                    int newLen = v1 + v2;
                    if (Math.Min(v1, v2) == 0 ||
                        // Math.Abs(v1 - v2) > 1 || 
                        newLen <= maxLen)
                        break;

                    if (alternatingChecking(i, j, s))
                        maxLen = newLen;
                }

            return maxLen;
        }

        /*
         * start: 1:58pm 
         * exit: 2:02pm 
         */
        public static bool alternatingChecking(int c1, int c2, string s)
        {
            int prev = -1;
            foreach (char c in s)
            {
                int runner = c - 'a';
                if (runner != c1 &&
                    runner != c2)
                    continue;

                if (prev != -1 &&
                    runner == prev)
                    return false;

                prev = runner;
            }

            return true;
        }

        /*
         * start: 1:48pm
         * exit: 1:49pm
         */
        public static int[] counting(int n, string s)
        {
            int[] cA = new int[26];

            foreach (char c in s)
            {
                int val = c - 'a';
                cA[val]++;
            }

            return cA;
        }
    }
}
