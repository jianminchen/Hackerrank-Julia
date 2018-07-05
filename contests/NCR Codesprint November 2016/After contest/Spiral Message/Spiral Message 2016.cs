using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiralMessage
{
    class Program
    {
        /*
         * https://www.hackerrank.com/contests/ncr-codesprint/challenges/spiral-message
         *       
         * 
         */
        static void Main(string[] args)
        {
            //RunSampleTestCase(); 
            SpiralMessage();
        }

        private static void RunTestcases()
        {
            TestOneChar();
            TestOneRow();
            TestingOneColumn();
            RunSampleTestCase();
        }

        private static void RunSampleTestCase()
        {
            IList<string> input = new List<string>();
            input.Add("a##ar");
            input.Add("a#aa#");
            input.Add("xxwsr");

            string spiralMessage = SpiralMessageFromLowerLeftClockWise(input);
            Debug.Assert(spiralMessage.CompareTo("xaa##ar#rswx#aa") == 0);

            var result = spiralMessage.Split('#').ToList();
            result.RemoveAll(str => string.IsNullOrEmpty(str));
            Debug.Assert(result.Count == 4);
        }

        /*
         * One column test
         */
        private static void TestingOneColumn()
        {
            IList<string> input = new List<string>();
            input.Add("a");
            input.Add("#");
            input.Add("#");
            input.Add("a");
            input.Add("#");

            Debug.Assert(SpiralMessageFromLowerLeftClockWise(input).CompareTo("#a##a") == 0);
        }

        private static void TestOneRow()
        {
            IList<string> input = new List<string>();
            input.Add("a##a#");
            Debug.Assert(SpiralMessageFromLowerLeftClockWise(input).CompareTo("a##a#") == 0);
        }

        private static void TestOneChar()
        {
            IList<string> input = new List<string>();
            input.Add("a");
            Debug.Assert(SpiralMessageFromLowerLeftClockWise(input).CompareTo("a") == 0);
        }

        private static void SpiralMessage()
        {
            int[] arr = ToInt(Console.ReadLine().Split(' '));
            int rows = arr[0], cols = arr[1];

            IList<string> input = new List<string>();
            for (int i = 0; i < rows; i++)
            {
                input.Add(Console.ReadLine().Trim());
            }
            var result = SpiralMessageFromLowerLeftClockWise(input).Split('#').ToList();
            result.RemoveAll(str => string.IsNullOrEmpty(str));

            Console.WriteLine(result.Count());
        }

        /*
         * Dec. 8, 2016
         * Function spec: 
         * 1. Clockwise direction
         * 2. Start from lower left corner
         * 
           Base cases: one node, one row, one column 
           
         * Return: 
         * The spiral message
         */
        private static string SpiralMessageFromLowerLeftClockWise(IList<string> data)
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

                bool downToUp = false;
                bool leftToRight = false;
                bool upToDown = false;
                bool rightToLeft = false;

                // base cases: one node, one row, one column
                if (isOneNode)
                    downToUp = true;
                else if (isOneRow)
                {
                    downToUp = true;
                    leftToRight = true;
                }
                else if (isOneCol)
                {
                    downToUp = true;
                }
                else
                {
                    downToUp = true;
                    leftToRight = true;
                    upToDown = true;
                    rightToLeft = true;
                }

                // 1. to upward
                if (downToUp)
                    for (int i = endX; i >= startX; i--)
                    {
                        char runner = data[i][startY];

                        sb.Append(runner);
                    }

                // 2. to right   
                if (leftToRight)
                    for (int j = startY + 1; j <= endY; j++)
                    {
                        char runner = data[startX][j];
                        sb.Append(runner);
                    }

                // 3. downward
                if (upToDown)
                    for (int i = startX + 1; i <= endX; i++)
                    {
                        char runner = data[i][endY];

                        sb.Append(runner);
                    }

                // 4. to left 
                if (rightToLeft)
                    for (int j = endY - 1; j > startY; j--)
                    {
                        char runner = data[endX][j];
                        sb.Append(runner);
                    }

                startX++;
                endX--;
                startY++;
                endY--;
            }

            return sb.ToString();
        }

        private static int[] ToInt(string[] arr)
        {
            int len = arr.Length;
            int[] result = new int[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = Convert.ToInt32(arr[i]);
            }

            return result;
        }
    }
}