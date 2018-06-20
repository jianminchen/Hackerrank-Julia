using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    /// it is not sorted and binary search not working
    /// x^2 - ax = yb - y^2, 1 <= x <= c, 1 <= y <= d
    static int countSolutions(int a, int b, int c, int d)
    {
        int count = 0;
        // Complete this function
        for (int x = 1; x <= c; x++)
        {
            var leftBase = x * x;
            var rightBase = x * a;

            var difference = leftBase - rightBase;
            bool lessThanZero = difference < 0;
            bool equalToZero = difference == 0;
            int search = Math.Abs(difference); // always positive

            if (lessThanZero)
            {
                int start = b;  // y >= b
                int end = d;

                var index = binarySearchValueY(search, start, end, b, 1);
                if (index >= 0)
                {
                    count++;
                }
            }
            else if (equalToZero)
            {
                if (b >= 1 && b <= d)
                {
                    count++;
                }
            }
            else
            {
                // test case 1, 2, 3, 11, 12 timeout
                // x^2 - ax = yb - y^2, 1 <= x <= c, 1 <= y <= d                 
                int lastOne = Math.Min(b, d);
                //int lastOne = d; 
                /*
                int maximum = lastOne * b;

                if (search > maximum)
                {
                    continue; 
                }
                */
                // apply binary search in two ranges
                // [1, b/2]   - ascending  order     

                if (binarySearchValueY(search, 1, b / 2, b, -1) >= 0)
                {
                    count++;
                }

                // [b/2, end] - descending order                
                if (binarySearchValueY_Descending(search, b / 2 + 1, lastOne, b, -1) >= 0)
                {
                    count++;
                }
            }
        }

        return count;
    }

    /// <summary>
    /// (y - b) y = -1 * (x - a) x 
    /// </summary>
    /// <param name="search"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private static int binarySearchValueY(int search, int start, int end, int b, int sign)
    {
        if (start <= end)
        {
            int middle = start + (end - start) / 2;
            var value = middle * (middle - b) * sign;
            if (value == search)
            {
                return middle;
            }
            else if (value < search)
            {
                start = middle + 1;
            }
            else
            {
                end = middle - 1;
            }
        }

        return -1;
    }

    /// <summary>
    /// yb-y^2 = y (b - y)
    /// </summary>
    /// <param name="search"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private static int binarySearchValueY_Descending(int search, int start, int end, int b, int sign)
    {
        if (start <= end)
        {
            int middle = start + (end - start) / 2;
            var value = middle * (middle - b) * sign;
            if (value == search)
            {
                return middle;
            }
            else if (value > search)
            {
                start = middle + 1;
            }
            else
            {
                end = middle - 1;
            }
        }

        return -1;
    }

    static void Main(String[] args)
    {
        ProcessInput();
        //RunTestcase(); 
    }

    public static void ProcessInput()
    {
        int q = Convert.ToInt32(Console.ReadLine());
        for (int a0 = 0; a0 < q; a0++)
        {
            string[] tokens_a = Console.ReadLine().Split(' ');
            int a = Convert.ToInt32(tokens_a[0]);
            int b = Convert.ToInt32(tokens_a[1]);
            int c = Convert.ToInt32(tokens_a[2]);
            int d = Convert.ToInt32(tokens_a[3]);
            int result = countSolutions(a, b, c, d);
            Console.WriteLine(result);
        }
    }

    public static void RunTestcase()
    {
        int result = countSolutions(1, 1, 1, 1);
    }
}
