using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    static void Main(String[] args)
    {
        ProcessInput();
        // RunTestcase(); 
    }

    public static void RunTestcase()
    {
        var heights = new int[3][];
        heights[0] = new int[] { 1, 3, 4 };
        heights[1] = new int[] { 2, 2, 3 };
        heights[2] = new int[] { 1, 2, 4 };

        calculateHorizontalConnected(heights);
    }

    public static void ProcessInput()
    {
        var arguments = Console.ReadLine().Split(' ');

        int longth = Convert.ToInt32(arguments[0]);
        int width = Convert.ToInt32(arguments[1]);

        var heights = new int[longth][];

        for (int index = 0; index < longth; index++)
        {
            var rowOfHeight = Console.ReadLine().Split(' ');

            heights[index] = Array.ConvertAll(rowOfHeight, Int32.Parse);
        }

        int result = CalculateSurfaceArea(heights);
        Console.WriteLine(result);
    }

    /// <summary>
    /// How to calculate the surface area?
    /// 
    /// </summary>
    /// <param name="heights"></param>
    /// <returns></returns>
    public static int CalculateSurfaceArea(int[][] heights)
    {
        const int NumberOfOneCube = 6;

        var totalUnits = getTotalUnits(heights);
        var maximumSurface = totalUnits * NumberOfOneCube;

        var horizontalCut = calculateHorizontalConnected(heights);
        //Console.WriteLine(horizontalCut);

        var verticalCut120Degree = calculateVerticalCut120Degree(heights);
        // Console.WriteLine(verticalCut120Degree);

        var verticalCut30Degree = calculateVerticalCut30Degree(heights);
        // Console.WriteLine(verticalCut30Degree);

        return maximumSurface - horizontalCut - verticalCut120Degree - verticalCut30Degree;
    }

    // vertical cut - 30 degree
    // First cut: 
    //                new int[] {1, 2, 1}
    //                new int[] {3, 2, 2}
    // Second cut:    new int[] {3, 2, 2}
    //                new int[] {4, 3, 3}
    private static int calculateVerticalCut30Degree(int[][] heights)
    {
        int connected = 0;
        int rows = heights.Length;
        int cols = heights[0].Length;

        for (int col = 1; col < cols; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var current = heights[row][col];
                var previous = heights[row][col - 1];

                connected += (int)Math.Min(current, previous);
            }
        }

        return connected * 2;
    }

    // vertical cut  - 120 degree
    // First cut: 
    //                 new int[] {1, 3, 4}
    //                 new int[] {2, 2, 3}
    //  minimum value  new int[] {1, 2, 3}   (1 + 2 + 3) * 2
    // Second cut:
    //                 new int[] {2, 2, 3}
    //                 new int[] {1, 3, 4}
    // minimum value   new int[] {1, 2, 3}  ( 1 + 2 + 3) * 2
    private static int calculateVerticalCut120Degree(int[][] heights)
    {
        int connected = 0;

        for (int i = 1; i < heights.Length; i++)
        {
            var item = heights[i];
            for (int j = 0; j < item.Length; j++)
            {
                var current = heights[i][j];
                var previous = heights[i - 1][j];

                connected += (int)Math.Min(current, previous);
            }
        }

        return connected * 2;
    }

    // horizontal cut, top down,  new int[]{4, 4, 3, 3, 2, 2, 2, 1, 1, 1}
    //                                          2      2        3        
    //                                                2 + 2    2+2+3
    //                                                 4        7
    private static int calculateHorizontalConnected(int[][] heights)
    {
        var max = getMaximum(heights);

        var countingSort = sortByCounting(heights, max);

        return countHorizontalConnected(countingSort);
    }

    private static int countHorizontalConnected(int[] sorted)
    {
        var length = sorted.Length;

        var previous = 0;
        var connected = 0;
        var subArraySum = 0;

        for (int i = length - 1; i > 0; i--)
        {
            var current = sorted[i];

            if (previous == 0)
            {
                previous = current; // bug found through debugging. 
                continue;
            }

            var skipped = current == 0;
            subArraySum += previous;
            connected += subArraySum;

            if (!skipped)
            {
                previous = current;
            }
        }

        return connected * 2;
    }

    private static int[] sortByCounting(int[][] heights, int max)
    {
        var sorted = new int[max + 1];

        foreach (int[] item in heights)
        {
            foreach (var number in item)
            {
                sorted[number]++;
            }
        }

        return sorted;
    }

    private static int getMaximum(int[][] heights)
    {
        int maxValue = int.MinValue;
        foreach (int[] item in heights)
        {
            var current = item.Max();
            maxValue = current > maxValue ? current : maxValue;
        }

        return maxValue;
    }

    private static int getTotalUnits(int[][] heights)
    {
        int sum = 0;
        foreach (var item in heights)
        {
            sum += item.Sum();
        }

        return sum;
    }
}
