using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalindromicTable
{
    class PalindromicTable
    {
        static void Main(string[] args)
        {
            ProcessInput();
            //RunTestcase2();
        }

        public static void RunTestcase()
        {
            int n = 3;
            int m = 3;

            var table = new int[n][];
            table[0] = new int[] { 1, 1, 1 };
            table[1] = new int[] { 1, 0, 1 };
            table[2] = new int[] { 1, 1, 1 };

            var output = FindLargestPalindromicRectangle(table);

            foreach (var item in output)
            {
                Console.WriteLine(item);
            }
        }

        public static void RunTestcase2()
        {
            int n = 3;
            int m = 5;

            var table = new int[n][];
            table[0] = new int[] { 1, 2, 0, 3, 2 };
            table[1] = new int[] { 0, 1, 2, 3, 4 };
            table[2] = new int[] { 0, 9, 8, 9, 0 };

            var output = FindLargestPalindromicRectangle(table);

            foreach (var item in output)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ProcessInput()
        {
            var tokens_n = Console.ReadLine().Split(' ');
            int n = Convert.ToInt32(tokens_n[0]);
            int m = Convert.ToInt32(tokens_n[1]);

            var table = new int[n][];
            for (int row = 0; row < n; row++)
            {
                string[] table_temp = Console.ReadLine().Split(' ');

                table[row] = Array.ConvertAll(table_temp, Int32.Parse);
            }

            var output = (FindLargestPalindromicRectangle(table));

            foreach (var item in output)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// n * m <= 100,000  
        /// 
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static string[] FindLargestPalindromicRectangle(int[][] numbers)
        {
            var result = FindLargestPalindromicRectangleHelper(numbers);

            if (result.Length <= 1)
            {
                return new string[0];
            }

            return prepareOutput(result);
        }

        /// <summary>
        /// code review on June 15, 2017 9:50 pm
        /// output the strings
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private static string[] prepareOutput(int[] result)
        {
            if (result.Length <= 1)
            {
                return new string[0];
            }

            int rows = result[2] - result[0] + 1;
            int cols = result[3] - result[1] + 1;

            int length = rows * cols;

            var rectangle = string.Empty;

            bool isFirst = true;
            foreach (var item in result)
            {
                if (!isFirst)
                {
                    rectangle += " ";
                }

                rectangle += item;
                isFirst = false;
            }

            return new string[] { length.ToString(), rectangle };
        }

        /// <summary>
        /// code review on June 15, 2017 9:50 pm
        /// Look for the bugs causing test case 31 - 43 wrong answers
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        private static int[] FindLargestPalindromicRectangleHelper(int[][] numbers)
        {
            int rows = numbers.Length;
            int columns = numbers[0].Length;

            var dictionary = prepareDictionary(rows, columns);
            var values = dictionary.OrderByDescending(x => x.Key);

            foreach (var number in values)
            {
                var options = dictionary[number.Key];
                foreach (var item in options)
                {
                    var selectedRows = item[0];
                    var selectedCols = item[1];

                    // first row can be 0th row to (rows - selectedRows)th row
                    for (int row = 0; row <= rows - selectedRows; row++)
                    {
                        var palindromicMemo = new int[10];

                        // first column can be 0th column to (columns - selectedCols)th column

                        for (int col = 0; col <= columns - selectedCols; col++)
                        {
                            var output = new int[] { row, col, row + selectedRows - 1, col + selectedCols - 1 };

                            // start from row, col, only calculate the board summary once
                            if (col == 0)
                            {
                                countDigits(numbers, row, col, selectedRows, selectedCols, palindromicMemo);

                                if (isPalindromic(palindromicMemo))
                                {
                                    return output;
                                }

                                continue;
                            }

                            // remove col - 1 column, add col + selectedCols 
                            for (int i = row; i < row + selectedRows; i++)
                            {
                                var removeCol = col - 1;
                                var addCol = col + selectedCols - 1;

                                palindromicMemo[numbers[i][removeCol]]--;
                                palindromicMemo[numbers[i][addCol]]++;
                            }

                            if (isPalindromic(palindromicMemo))
                            {
                                return output;
                            }
                        }
                    }
                }
            }

            return new int[0];
        }

        /// <summary>
        /// code review on June 15, 2017 9:50 pm
        /// memo will be updated with the count of digits
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="memo"></param>
        private static void countDigits(int[][] numbers, int row, int col, int rows, int cols, int[] memo)
        {
            for (int i = row; i < row + rows; i++)
            {
                for (int j = col; j < col + cols; j++)
                {
                    memo[numbers[i][j]]++;
                }
            }
        }

        /// <summary>
        /// code review on June 15, 2017 9:50 pm
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        private static Dictionary<int, IList<int[]>> prepareDictionary(int rows, int cols)
        {
            var memo = new Dictionary<int, IList<int[]>>();

            for (int row = rows; row >= 1; row--)
            {
                for (int col = cols; col >= 1; col--)
                {
                    var newEntry = new int[] { row, col };

                    var multiplication = row * col;
                    if (memo.ContainsKey(multiplication))
                    {
                        var list = memo[multiplication];
                        list.Add(newEntry);

                        memo[multiplication] = list;
                    }
                    else
                    {
                        var list = new List<int[]>();
                        list.Add(newEntry);

                        memo.Add(multiplication, list);
                    }
                }
            }

            return memo;
        }

        /// <summary>
        /// code review on June 15, 2017 9:50 pm
        /// aacaa - 2k + 1, length of palindrome string is odd 
        /// abba  - 2k, length of palindrome string is even
        /// </summary>
        /// <param name="memo"></param>
        /// <returns></returns>
        private static bool isPalindromic(int[] memo)
        {
            if (!oddCountLessEqualThanOne(memo))
            {
                return false;
            }

            // one digit string is palindrome
            if (checkOneDigitString(memo))
            {
                return true;
            }

            // more than one digit, at least one nonzero number
            // edge case: 00100 had leading zero, only one of '1' which cannot be the lead character
            // 100100 - ok 
            if (checkNonZeroSum(memo) <= 1) // 0 or 1
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// June 16, 2016
        /// failed last two test cases, code review 
        /// </summary>
        /// <param name="memo"></param>
        /// <returns></returns>
        private static bool checkOneDigitString(int[] memo)
        {
            return memo.Sum() == 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memo"></param>
        /// <returns></returns>
        private static int checkNonZeroSum(int[] memo)
        {
            int count = 0;

            for (int i = 1; i <= 9; i++)
            {
                count += memo[i];
            }

            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memo"></param>
        /// <returns></returns>
        private static bool oddCountLessEqualThanOne(int[] memo)
        {
            int isOdd = 0;

            foreach (var number in memo)
            {
                if (number % 2 == 1)
                {
                    isOdd++;
                }
            }

            return isOdd <= 1;
        }

        /// <summary>
        /// code review on June 15, 2017 9:50 pm
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        private static int[] saveToMemo(int[][] numbers)
        {
            int rows = numbers.Length;
            int cols = numbers[0].Length;

            var memo = new int[10];  // 0 - 9           

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var current = numbers[row][col];
                    memo[current]++;
                }
            }

            return memo;
        }

    }
}