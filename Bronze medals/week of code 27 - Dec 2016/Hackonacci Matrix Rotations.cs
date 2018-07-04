using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GackonacciMatrixRotations
{
    class HackerNode
    {
        public bool n1, n2, n3, n;
        public HackerNode(bool v1, bool v2, bool v3, bool v)
        {
            n1 = v1;
            n2 = v2;
            n3 = v3;
            n = v;
        }
    }

    class Program
    {
        static HashSet<long> hackerBase = new HashSet<long>();
        static Dictionary<long, bool> hackerMatrix = new Dictionary<long, bool>();

        static void Main(string[] args)
        {
            MatrixRotation();

            //TestRotateClockwise180();
            //TestRotateClockwise90();
            //TestRotateClockwise270(); 
        }

        /*
         * rows <= 2000
         */
        private static void SetHackerBase(int rows)
        {
            for (int i = 1; i <= rows; i++)
                for (int j = 1; j <= rows; j++)
                {
                    long keyValue = (long)i * (long)j;
                    keyValue *= keyValue;

                    if (!hackerBase.Contains(keyValue))
                        hackerBase.Add(keyValue);
                }
        }

        /*
         * 
         */
        private static void CalculateHackValue(int rows)
        {
            long maxValue = rows * rows * rows * rows;

            bool[] hackData = new bool[] { false, true, false };

            for (int i = 1; i <= 3; i++)
            {
                if (hackerBase.Contains(i))
                    hackerMatrix.Add(i, hackData[i - 1]);
            }

            bool current = true;
            int start = 4;

            for (int i = start; i <= maxValue; i++)
            {
                if (hackData[0] == hackData[2])
                    current = true;
                else
                    current = false;

                if (hackerBase.Contains(i))
                    hackerMatrix.Add(i, current);

                // set for next iteration
                hackData[0] = hackData[1];
                hackData[1] = hackData[2];
                hackData[2] = current;
            }
        }

        /*
         * http://stackoverflow.com/questions/15725840/copy-one-2d-array-to-another-2d-array
         */
        private static void TestRotateClockwise180()
        {
            char[,] matrix = new char[4, 4];

            CaculateMatrix(matrix, 4);
            char[,] beforeRotation = matrix.Clone() as char[,];

            int number = RotateClockWise180(matrix, 4);

            Debug.Assert(2 * number == 6);
        }

        private static void TestRotateClockwise90()
        {
            char[,] matrix = new char[4, 4];

            CaculateMatrix(matrix, 4);
            char[,] beforeRotation = matrix.Clone() as char[,];

            int number = RotateClockwise90(matrix, 4);

            Debug.Assert(number == 10);
        }

        private static void TestRotateClockwise270()
        {
            char[,] matrix = new char[4, 4];

            CaculateMatrix(matrix, 4);
            char[,] beforeRotation = matrix.Clone() as char[,];

            int number1 = RotateClockwise90(matrix, 4);
            int number2 = RotateClockWise180(matrix, 4);

            int number = CalculateCellsChange(beforeRotation, matrix, 4);
            Debug.Assert(number == 10);
        }

        private static void MatrixRotation()
        {
            int[] data = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
            int rows = data[0];
            int queries = data[1];

            int[] angles = new int[queries];

            SetHackerBase(rows);
            CalculateHackValue(rows);

            for (int i = 0; i < queries; i++)
            {
                angles[i] = Convert.ToInt32(Console.ReadLine());
            }

            IList<int> results = CompareMatrixAfterClockwiseRotate(rows, queries, angles);
            foreach (int value in results)
            {
                Console.WriteLine(value);
            }
        }

        /*
         * Func spec: 
         * 1. Matrix rotation clockwise
         * 2. Matrix will rotate degree angle[i], i from 0 to queries
         * 3. Matrix's rows:     rows
         *    Matrix's columns:  rows
         *    matrix item value: X or Y
         *    depending on even or odd
         * 4. hackonacci((i*j)^2) is even, it is X
         *    odd, it's Y
         */
        private static IList<int> CompareMatrixAfterClockwiseRotate(int rows, int queries, int[] angles)
        {
            IList<int> result = new List<int>();

            Dictionary<int, int> calculatedData = PrepareResults(rows);

            foreach (int angle in angles)
            {
                int rotateDegree = angle % 360;

                result.Add(calculatedData[rotateDegree]);
            }

            return result;
        }

        private static Dictionary<int, int> PrepareResults(int rows)
        {
            char[,] beforeRotation = new char[rows, rows];

            CaculateMatrix(beforeRotation, rows);

            Dictionary<int, int> calculatedData = new Dictionary<int, int>();

            int[] angles = new int[] { 0, 90, 180, 270 };

            char[,] matrixAfter90 = new char[rows, rows];

            foreach (int angle in angles)
            {
                if (angle == 0)
                {
                    calculatedData.Add(0, 0);
                }
                else if (angle == 90)
                {
                    char[,] matrix = beforeRotation.Clone() as char[,];
                    int number = RotateClockwise90(matrix, rows);
                    calculatedData.Add(90, number);
                    matrixAfter90 = matrix.Clone() as char[,];
                }
                else if (angle == 180)
                {
                    char[,] matrix = beforeRotation.Clone() as char[,];
                    int number = 2 * RotateClockWise180(matrix, rows);
                    calculatedData.Add(180, number);
                }
                else if (angle == 270)
                {
                    RotateClockWise180(matrixAfter90, rows);
                    int number = CalculateCellsChange(beforeRotation, matrixAfter90, rows);
                    calculatedData.Add(270, number);
                }
            }

            return calculatedData;
        }

        private static int RotateClockwise90(char[,] matrix, int rows)
        {
            int start = 0;
            int end = rows - 1;
            int count = 0;
            while (start < end)
            {
                // save first column
                char[] leftColumnCopy = new char[rows];
                for (int row = start; row <= end; row++)
                {
                    leftColumnCopy[row] = matrix[row, start];
                }

                // move last row to first column
                for (int col = start; col <= end; col++)
                {
                    char move = matrix[end, col];
                    char replaced = leftColumnCopy[col];
                    if (move - replaced != 0)
                    {
                        count++;
                        matrix[col, start] = move;
                    }
                }

                // move last column to last row
                for (int row = end - 1; row >= start; row--)
                {
                    char move = matrix[row, end];
                    char replaced = matrix[end, start + end - row];
                    if (move - replaced != 0)
                    {
                        count++;
                        matrix[end, start + end - row] = move;
                    }
                }

                // move first row to last column
                for (int col = end - 1; col >= start; col--)
                {
                    char move = matrix[start, col];

                    if (col == start)
                        move = leftColumnCopy[start];

                    char replaced = matrix[end - 1 - (end - 1 - col), end];
                    if (move - replaced != 0)
                    {
                        count++;
                        matrix[end - 1 - (end - 1 - col), end] = move;
                    }
                }

                // use saved first column's copy, put on the first row
                for (int row = start + 1; row <= end - 1; row++)
                {
                    char move = leftColumnCopy[row];
                    char replaced = matrix[start, end - 1 - (row - start - 1)];
                    if (move - replaced != 0)
                    {
                        count++;
                        matrix[start, end - 1 - (row - start - 1)] = move;
                    }
                }

                start++;
                end--;
            }

            return count;
        }

        /*
         * swap the top and bottom row, and then, swap first column and last column except corners
         * return: swap count, if the two chars are different, then increase the count. 
         */
        private static int RotateClockWise180(char[,] matrix, int rows)
        {
            int start = 0;
            int end = rows - 1;
            int count = 0;
            while (start < end)
            {
                // swap top and bottom row
                for (int col = start; col <= end; col++)
                {
                    if (matrix[start, col] - matrix[end, end - col + start] != 0)
                    {
                        swap(matrix, start, col, end, end - (col - start));
                        count++;
                    }
                }

                // swap first column and last column 
                for (int row = start + 1; row < end; row++)
                {
                    if (matrix[row, start] - matrix[end - row + start, end] != 0)
                    {
                        swap(matrix, row, start, end - (row - start), end);
                        count++;
                    }
                }

                start++;
                end--;
            }

            return count;
        }

        private static void RotateClockwise270(char[,] matrix, int rows)
        {
            RotateClockwise90(matrix, rows);
            RotateClockWise180(matrix, rows);
        }

        private static void swap(char[,] matrix, int x1, int y1, int x2, int y2)
        {
            char tmp = matrix[x1, y1];
            matrix[x1, y1] = matrix[x2, y2];
            matrix[x2, y2] = tmp;
        }

        private static void CaculateMatrix(char[,] matrix, int rows)
        {
            for (int row = 0; row < rows; row++)
                for (int col = 0; col < rows; col++)
                {
                    long value = (long)(row + 1) * (long)(col + 1);
                    value *= value;
                    bool isEven = hackerMatrix[value];

                    matrix[row, col] = isEven ? 'X' : 'Y';
                }
        }

        private static int CalculateCellsChange(char[,] beforeRotation, char[,] matrix, int rows)
        {
            int count = 0;
            for (int row = 0; row < rows; row++)
                for (int col = 0; col < rows; col++)
                {
                    if (beforeRotation[row, col] - matrix[row, col] != 0)
                        count++;
                }

            return count;
        }
    }
}