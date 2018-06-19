using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestMask
{
    /// <summary>
    /// https://stackoverflow.com/questions/3912112/check-if-a-number-is-non-zero-using-bitwise-operators-in-c
    /// https://stackoverflow.com/questions/3160659/innovative-way-for-checking-if-number-has-only-one-on-bit-in-signed-int
    /// http://www.catonmat.net/blog/low-level-bit-hacks-you-absolutely-must-know/
    /// </summary>
    class Program
    {
        static void Main(String[] args)
        {
            ProcessInput();
            //RunTestcase(); 
        }

        public static void RunTestcase()
        {
            int length = 10;
            var numbers = new int[] { 1, 2, 4, 8, 16, 32, 64, 256, 512, 128 };

            // int length = 3;
            // var numbers = new int[] {7, 14,28 }; 

            int[][] memo = new int[length][];

            for (int i = 0; i < length; i++)
            {
                memo[i] = new int[27];
            }

            int[] foundMemo = new int[length];

            int result = getBestMask(numbers, memo, foundMemo);
        }

        public static void ProcessInput()
        {
            int n = Convert.ToInt32(Console.ReadLine());
            string[] row = Console.ReadLine().Split(' ');
            int[] numbers = Array.ConvertAll(row, Int32.Parse);
            int length = numbers.Length;

            int[][] memo = new int[length][];

            for (int i = 0; i < length; i++)
            {
                memo[i] = new int[27];
            }

            int[] foundMemo = new int[length];

            int result = getBestMask(numbers, memo, foundMemo);
            Console.WriteLine(result);
        }

        /// <summary>
        /// http://www.catonmat.net/blog/low-level-bit-hacks-you-absolutely-must-know/
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static int getBestMask(int[] numbers, int[][] memo, int[] foundMemo)
        {
            int result = 0;
            int length = numbers.Length;

            for (int n = 26; n >= 0; n--)
            {
                bool foundOne = false;

                // found one?
                var set = new HashSet<int>();
                for (int i = 0; i < numbers.Length; i++)
                {
                    if (foundMemo[i] == 1)
                    {
                        continue;
                    }

                    var visit = numbers[i];
                    if (checkNthBit(visit, n))
                    {
                        set.Add(i);
                    }

                    if (checkNthBit(visit, n) && (foundOne || (visit % (1 << n) == 0)))
                    {
                        foundOne = true;
                        foundMemo[i] = 1;
                    }
                }

                // mark visited 
                if (foundOne)
                {
                    foreach (var id in set)
                    {
                        foundMemo[id] = 1;
                    }
                }

                // reset hashset
                set.Clear();

                // iterate again 
                if (foundOne)
                {
                    result += 1 << n;
                }
            }

            return result;
        }

        private static bool checkFoundMemoNotComplete(int[] foundMemo)
        {
            foreach (var item in foundMemo)
            {
                if (item == 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool checkNthBit(int number, int n)
        {
            return (number & (1 << n)) > 0;
        }
    }
}
