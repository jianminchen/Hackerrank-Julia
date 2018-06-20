using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    static void Main(String[] args)
    {
        int n = Convert.ToInt32(Console.ReadLine());
        string[] a_temp = Console.ReadLine().Split(' ');
        int[] numbers = Array.ConvertAll(a_temp, Int32.Parse);
        int result = FindMinimumNumber(numbers);
        Console.WriteLine(result);
    }

    static int FindMinimumNumber(int[] numbers)
    {
        int length = numbers.Length;

        int leftSum = 0;
        for (int i = 0; i < length / 2; i++)
        {
            leftSum += numbers[i];
        }

        int rightSum = 0;
        for (int i = length / 2; i < length; i++)
        {
            rightSum += numbers[i];
        }

        return Math.Abs(leftSum - rightSum);
    }
}
