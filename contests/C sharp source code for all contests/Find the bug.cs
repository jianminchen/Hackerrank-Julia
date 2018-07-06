using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    static void Main(String[] args)
    {
        int n = Convert.ToInt32(Console.ReadLine());
        string[] grid = new string[n];
        for (int grid_i = 0; grid_i < n; grid_i++)
        {
            grid[grid_i] = Console.ReadLine();
        }
        // Return an array containing [r, c]
        int[] result = findTheBug(grid);
        Console.WriteLine(String.Join(",", result));
    }

    public static int[] findTheBug(string[] grid)
    {
        int row = 0;
        foreach (var item in grid)
        {
            int col = item.IndexOf('X');
            if (col >= 0)
            {
                return new int[] { row, col };
            }

            row++;
        }

        return new int[] { -1, -1 };
    }
}
