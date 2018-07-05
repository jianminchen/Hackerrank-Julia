using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralMessage
{
    class Program
    {
        /*
         * https://www.hackerrank.com/contests/ncr-codesprint/challenges/spiral-message
         * 11:42am start to read problem statement
         * Write a code to work on the sample test case first, we do not need to store
         * result of string, just keep counting. 
         * 3 5 
         * a##ar
         * a#aa#
         * xxwsr
         * 
         * 12:21pm 
         * 4.49/ 20 
         * test cases: 0 - 13
         * pass: 0, 11, 12, 13
         * 
         * 1:07pm 
         * 6.12/ 20 
         * test cases: 0 - 13
         * pass: 
         * 
         * 2:02pm submit the code
         * 
         * 6:35pm 
         * score 7.76 out of 20
         * 0-13 test cases
         * failed: 2, 4, 5, 8, 9, 10
         * 
         */
        static void Main(string[] args)
        {
            process();

            //testing the code 
            //testing(); 
            //testingOneColumn();
            //testOneRow(); 
            //testOneChar();
            //testCase0(); 
        }
        private static void testCase0()
        {
            IList<string> input = new List<string>();
            input.Add("a#sar");
            input.Add("a#a##");
            input.Add("xxwsr");
            input.Add("rrrrr");

            Console.WriteLine(calculate(input));
        }

        /*
         * test case:
         * 3 5
            a##ar
            a#aa#
            xxwsr
         * 
         * 
         * 
         */
        private static void testing()
        {
            IList<string> input = new List<string>();
            input.Add("a##ar");
            input.Add("a#aa#");
            input.Add("xxwsr");

            Console.WriteLine(calculate(input));
        }

        /*
         * One column test
         */
        private static void testingOneColumn()
        {
            IList<string> input = new List<string>();
            input.Add("a");
            input.Add("#");
            input.Add("#");
            input.Add("a");
            input.Add("#");

            Console.WriteLine(calculate(input));
        }

        private static void testOneRow()
        {
            IList<string> input = new List<string>();
            input.Add("a##a#");
            Console.WriteLine(calculate(input));
        }

        private static void testOneChar()
        {
            IList<string> input = new List<string>();
            input.Add("a");
            Console.WriteLine(calculate(input));
        }

        private static void process()
        {
            int[] arr = ToInt(Console.ReadLine().Split(' '));
            int rows = arr[0], cols = arr[1];

            IList<string> input = new List<string>();
            for (int i = 0; i < rows; i++)
            {
                input.Add(Console.ReadLine().Trim());
            }

            Console.WriteLine(calculate(input));
        }

        /*
         * start: 11:53am
         * exit: 12:15
         * static analysis the code
         */
        private static int calculate(IList<string> data)
        {
            int rows = data.Count;
            int cols = data[0].Length;

            int startX = 0, endX = rows - 1;
            int startY = 0, endY = cols - 1;


            StringBuilder sb = new StringBuilder();

            while (startX <= endX &&
                   startY <= endY
                )
            {
                int nRows = endX - startX + 1;
                int mCols = endY - startY + 1;

                bool isOneNode = nRows == 1 && mCols == 1;
                bool isOneRow = nRows == 1;
                bool isOneCol = mCols == 1;

                bool lToR = false;
                bool uToD = false;
                bool rTol = false;
                bool dToU = false;

                if (isOneNode)    // one dot            
                    lToR = true;  // go right 
                else if (nRows == 1)  // one row 
                    lToR = true;
                else if (mCols == 1)  // one column 
                {
                    lToR = true;
                    uToD = true;
                }
                else
                {
                    lToR = true;
                    uToD = true;
                    rTol = true;
                    dToU = true;
                }

                // go over 4 direction
                // to right, downward, to left, to up
                // 1. to right   
                if (lToR)
                    for (int j = startY; j <= endY; j++)
                    {
                        char runner = data[startX][j];
                        sb.Append(runner);
                    }

                // 2. downward
                if (uToD)
                    for (int i = startX + 1; i <= endX; i++)
                    {
                        char runner = data[i][endY];

                        sb.Append(runner);
                    }

                // 3. to left 
                if (rTol)
                    for (int j = endY - 1; j >= startY; j--)
                    {
                        char runner = data[endX][j];
                        sb.Append(runner);
                    }

                // 4. to upward
                if (dToU)
                    for (int i = endX - 1; i > startX; i--)
                    {
                        char runner = data[i][startY];

                        sb.Append(runner);
                    }

                if (isOneNode || isOneRow || isOneCol)
                    break;

                startX++;
                endX--;  // ? 
                startY++;
                endY--;  // ? 
            }

            string[] output = sb.ToString().Split('#');

            int count = 0;
            foreach (string s in output)
            {
                if (s.Trim().Length > 0)
                    count++;
            }
            return count;

            /*
            Dictionary<string, int> words = new Dictionary<string, int>();
            for (int i = 0; i < output.Length; i++)
            {
                string word = output[i].Trim();
                if (word.Length == 0)
                    continue;

                word = rinse(word);
                if (!words.ContainsKey(word))
                    words.Add(word, 1);
            }

            return words.Count;
             */
        }

        private static string rinse(string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                if (isAlphabetic(s[i]))
                    sb.Append(s[i]);
            }

            return sb.ToString();
        }

        private static bool isAlphabetic3(char c)
        {
            return c != '#';
        }

        private static bool isAlphabetic(char c)
        {
            string data = "abcdefghijklmnopqrstuvwxyz";

            if (Array.IndexOf(data.ToCharArray(), c) == -1)
                return false;
            else
                return true;
        }

        private static bool isSpace(char c)
        {
            return c == '#';
        }

        private static bool isAlphabetic2(char c)
        {
            if ((c - 'a') >= 0 && ('z' - c) >= 0)
                return true;
            else
                return false;
        }

        private static int[] ToInt(string[] arr)
        {
            int len = arr.Length;
            int[] res = new int[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = Convert.ToInt32(arr[i]);
            }

            return res;
        }
    }
}
