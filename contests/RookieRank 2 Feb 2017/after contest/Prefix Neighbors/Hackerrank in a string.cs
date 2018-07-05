using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackerrankIsAString
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessInput();
            //RunSampleTestcase(); 
        }

        public static void RunSampleTestcase()
        {
            string[] rows = new string[] { "hereiamstackerrank" };

            var results = ContainHackerrank(rows);
        }

        public static void ProcessInput()
        {
            int queries = Convert.ToInt32(Console.ReadLine());

            string[] mRows = new string[queries];

            for (int i = 0; i < queries; i++)
            {
                mRows[i] = Console.ReadLine();
            }

            IEnumerable<bool> containHackerrank = ContainHackerrank(mRows);
            foreach (bool value in containHackerrank)
            {
                if (value)
                {
                    Console.WriteLine("YES");
                }
                else
                {
                    Console.WriteLine("NO");
                }
            }
        }
        /*
         * 2 <= q <= 100
         * 10 <= string's length <= 10000
         */
        public static IEnumerable<bool> ContainHackerrank(string[] rows)
        {
            const string key = "hackerrank";
            int length = rows.Length;
            var containKey = new bool[length];

            int index = 0;
            foreach (string row in rows)
            {
                // Need to find if row contains key
                bool found = ScanRightToLeft(row, key);
                containKey[index] = found;

                index++;
            }

            return containKey;
        }

        /*
         * Try to scan from right to left 
         */
        private static bool ScanRightToLeft(string source, string patterns)
        {
            int patternsIndex = patterns.Length - 1;
            int patternsLength = patterns.Length;

            for (int i = source.Length - 1; i >= 0; i--)
            {
                char current = source[i];
                if (patternsIndex >= 0 && current == patterns[patternsIndex])
                {
                    patternsIndex--;
                }

                if (patternsIndex < 0)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
