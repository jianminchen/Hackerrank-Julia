using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{

    static void Main(String[] args)
    {
        string[] tokens_n1 = Console.ReadLine().Split(' ');

        int n1 = Convert.ToInt32(tokens_n1[0]);
        int n2 = Convert.ToInt32(tokens_n1[1]);
        int n3 = Convert.ToInt32(tokens_n1[2]);

        string[] arr1 = Console.ReadLine().Split(' ');
        string[] arr2 = Console.ReadLine().Split(' ');
        string[] arr3 = Console.ReadLine().Split(' ');


        /*
        string[] arr1 = new string[] { "3", "2", "1", "1", "1" };
        string[] arr2 = new string[] { "4", "3", "2" };
        string[] arr3 = new string[] { "1", "1", "4", "1" }; 
        */
        Console.WriteLine(equalStack(arr1, 0, getSum(arr1, 0), arr2, 0, getSum(arr2, 0), arr3, 0, getSum(arr3, 0)));
    }


    public static int equalStack(string[] arr1, int index1, int total1,
        string[] arr2, int index2, int total2,
        string[] arr3, int index3, int total3)
    {
        if ((arr1 == null || index1 == arr1.Length) ||
            (arr2 == null || index2 == arr2.Length) ||
            (arr3 == null || index3 == arr3.Length))
            return 0;

        while (index1 < arr1.Length &&
              index2 < arr2.Length &&
              index3 < arr3.Length
        )
        {
            int sum1 = total1;
            int sum2 = total2;
            int sum3 = total3;

            if (sum1 == sum2 && sum2 == sum3)
                return sum1;

            int top1 = Convert.ToInt32(arr1[index1]);
            int top2 = Convert.ToInt32(arr2[index2]);
            int top3 = Convert.ToInt32(arr3[index3]);

            int[] arr = new int[3] { sum1, sum2, sum3 };

            if (maximumNo(arr, 3) == 0)
            {
                index1++;
                total1 = total1 - top1;
            }
            else if (maximumNo(arr, 3) == 1)
            {
                index2++;
                total2 = total2 - top2;
            }
            else if (maximumNo(arr, 3) == 2)
            {
                index3++;
                total3 = total3 - top3;
            }
        }

        return 0;
    }

    public static int maximumNo(int[] arr, int length)
    {
        int maxIndex = 0;
        for (int i = 1; i < length; i++)
        {
            if (arr[i] > arr[i - 1])
                maxIndex = i;
        }
        return maxIndex;
    }

    public static int getSum(string[] arr1, int index)
    {
        int sum = 0;
        for (int i = index; i < arr1.Length; i++)
        {
            sum += Convert.ToInt32(arr1[i]);
        }
        return sum;
    }
}
