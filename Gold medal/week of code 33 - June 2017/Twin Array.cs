using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    static void Main(String[] args)
    {
        int n = Convert.ToInt32(Console.ReadLine());
        string[] ar1_temp = Console.ReadLine().Split(' ');
        int[] ar1 = Array.ConvertAll(ar1_temp, Int32.Parse);
        string[] ar2_temp = Console.ReadLine().Split(' ');
        int[] ar2 = Array.ConvertAll(ar2_temp, Int32.Parse);
        int result = twinArrays(ar1, ar2);
        Console.WriteLine(result);
    }

    /// <summary>
    /// need to find the two mininum number and their index 
    /// </summary>
    /// <param name="ar1"></param>
    /// <param name="ar2"></param>
    /// <returns></returns>
    static int twinArrays(int[] ar1, int[] ar2)
    {
        var smallestFirst = getSmallestTwo(ar1);
        var smallestSecond = getSmallestTwo(ar2);

        bool isSame = smallestFirst[0] == smallestSecond[0];

        int value1 = ar1[smallestFirst[0]];
        int value2 = ar1[smallestFirst[1]];

        int value3 = ar2[smallestSecond[0]];
        int value4 = ar2[smallestSecond[1]];

        if (!isSame)
        {
            return value1 + value3;
        }
        else
        {
            var sum1 = value1 + value4;
            var sum2 = value2 + value3;
            return Math.Min(sum1, sum2);
        }
    }

    private static int[] getSmallestTwo(int[] array)
    {
        int value1 = array[0];
        int value2 = array[1];

        int smallest = Math.Min(value1, value2);
        int nextToSmallest = Math.Max(value1, value2);

        int smallestIndex = value1 > value2 ? 1 : 0;
        int nextToSmallestIndex = value1 > value2 ? 0 : 1;

        for (int i = 2; i < array.Length; i++)
        {
            var current = array[i];

            smallest = array[smallestIndex];
            nextToSmallest = array[nextToSmallestIndex];

            if (current < smallest)
            {
                var tmpId = smallestIndex;
                smallestIndex = i;
                nextToSmallestIndex = tmpId;
            }
            else if (current < nextToSmallest)
            {
                nextToSmallestIndex = i;
            }
        }

        return new int[] { smallestIndex, nextToSmallestIndex };
    }
}
