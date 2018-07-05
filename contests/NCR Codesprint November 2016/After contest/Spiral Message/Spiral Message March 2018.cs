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
        /// <summary>
        /// https://www.hackerrank.com/contests/ncr-codesprint/challenges/spiral-message
        /// </summary>
        /// <param name="args"></param>
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
            var input = new List<string>();
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
            var input = new List<string>();
            input.Add("a");
            input.Add("#");
            input.Add("#");
            input.Add("a");
            input.Add("#");

            Debug.Assert(SpiralMessageFromLowerLeftClockWise(input).CompareTo("#a##a") == 0);
        }

        private static void TestOneRow()
        {
            var input = new List<string>();
            input.Add("a##a#");
            Debug.Assert(SpiralMessageFromLowerLeftClockWise(input).CompareTo("a##a#") == 0);
        }

        private static void TestOneChar()
        {
            var input = new List<string>();
            input.Add("a");
            Debug.Assert(SpiralMessageFromLowerLeftClockWise(input).CompareTo("a") == 0);
        }

        private static void SpiralMessage()
        {
            var split = Console.ReadLine().Split(' ');

            var arr = Array.ConvertAll(split, int.Parse);

            var rows = arr[0];
            var cols = arr[1];

            var input = new List<string>();

            for (int i = 0; i < rows; i++)
            {
                input.Add(Console.ReadLine().Trim());
            }

            var result = SpiralMessageFromLowerLeftClockWise(input).Split('#').ToList();

            result.RemoveAll(str => string.IsNullOrEmpty(str));

            Console.WriteLine(result.Count());
        }

        /// <summary>
        /// March 20, 2018
        /// I clean up code and make it more readable. 
        /// 
        /// Dec. 8, 2016
        /// Function spec: 
        /// 1. Clockwise direction
        /// 2. Start from lower left corner
        /// 3. String can be counted using the hash mark (#)
        /// Return:
        /// Spiral message instead of count of words
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string SpiralMessageFromLowerLeftClockWise(IList<string> data)
        {
            var rows = data.Count;
            var cols = data[0].Length;

            int rowStart = 0;
            int rowEnd = rows - 1;

            int colStart = 0;
            int colEnd = cols - 1;

            var sb = new StringBuilder();

            while (rowStart <= rowEnd &&
                   colStart <= colEnd
                )
            {
                int nRows = rowEnd - rowStart + 1;
                int mCols = colEnd - colStart + 1;

                bool isOneNode = nRows == 1 && mCols == 1;
                bool isOneRow = nRows == 1;
                bool isOneCol = mCols == 1;

                // 1. to upward                
                for (int row = rowEnd; row >= rowStart; row--)
                {
                    sb.Append(data[row][colStart]);
                }

                // base case 1:
                if (isOneNode || isOneCol)
                {
                    break;
                }

                for (int col = colStart + 1; col <= colEnd; col++)
                {
                    sb.Append(data[rowStart][col]);
                }

                // base case 2:
                if (isOneRow)
                {
                    break;
                }

                for (int row = rowStart + 1; row <= rowEnd; row++)
                {
                    sb.Append(data[row][colEnd]);
                }

                for (int col = colEnd - 1; col > colStart; col--)
                {
                    sb.Append(data[rowEnd][col]);
                }

                rowStart++;
                rowEnd--;

                colStart++;
                colEnd--;
            }

            return sb.ToString();
        }
    }
}