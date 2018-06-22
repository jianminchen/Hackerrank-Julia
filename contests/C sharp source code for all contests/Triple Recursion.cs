using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    static void Main(String[] args)
    {
        string[] tokens_n = Console.ReadLine().Split(' ');
        int rows = Convert.ToInt32(tokens_n[0]);
        int initialValue = Convert.ToInt32(tokens_n[1]);
        int diagonalIncrement = Convert.ToInt32(tokens_n[2]);

        var nextRow = new int[rows];
        for (int index = 0; index < rows; index++)
        {
            TripleRecursion(rows, initialValue, diagonalIncrement, nextRow, index);
            Console.WriteLine(string.Join(" ", nextRow));
        }
    }

    public static void TripleRecursion(int rows, int initialValue, int diagonalIncrement, int[] nextRow, int index)
    {
        var isFirstRow = index == 0;
        if (isFirstRow)
        {
            nextRow[0] = initialValue;
            for (int i = 1; i < rows; i++)
            {
                nextRow[i] = nextRow[i - 1] - 1;
            }

            return;
        }

        var startValue = nextRow[0];
        var diagonalValue = nextRow[index - 1];

        for (int col = 0; col < index; col++)
        {
            nextRow[col]--;
        }

        nextRow[index] = diagonalValue + diagonalIncrement;

        for (int col = index + 1; col < rows; col++)
        {
            nextRow[col] = nextRow[col - 1] - 1;
        }
    }
}
