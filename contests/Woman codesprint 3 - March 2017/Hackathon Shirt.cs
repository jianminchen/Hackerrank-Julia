using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    static void Main(String[] args)
    {
        ProcessInput();
        //RunTestcase_binarySearchIntervals(); 
        //RunTestcase_CalculateMatchesFromVendors(); 
    }

    public static void RunTestcase_CalculateMatchesFromVendors()
    {
        var sizesForParticipants = new int[] { 3, 2 };
        var vendorIntervals = new List<Tuple<int, int>>();
        vendorIntervals.Add(new Tuple<int, int>(3, 4));
        vendorIntervals.Add(new Tuple<int, int>(4, 5));

        CalculateMatchesFromVendors(sizesForParticipants, vendorIntervals);
    }

    public static void RunTestcase_binarySearchIntervals()
    {
        var intervals = new List<Tuple<int, int>>();
        intervals.Add(new Tuple<int, int>(2, 4));
        intervals.Add(new Tuple<int, int>(6, 8));
        intervals.Add(new Tuple<int, int>(10, 12));
        intervals.Add(new Tuple<int, int>(12, 14));
        intervals.Add(new Tuple<int, int>(14, 16));

        bool found = binarySearchIntervals(13, intervals);
    }

    public static void ProcessInput()
    {
        int queries = Convert.ToInt32(Console.ReadLine());
        for (int query = 0; query < queries; query++)
        {
            int numberOfParticipants = Convert.ToInt32(Console.ReadLine());

            string[] sizes_temp = Console.ReadLine().Split(' ');
            int[] sizesForParticipants = Array.ConvertAll(sizes_temp, Int32.Parse);

            int numberOfVendors = Convert.ToInt32(Console.ReadLine());
            var vendorIntervals = new List<Tuple<int, int>>();

            for (int vendor = 0; vendor < numberOfVendors; vendor++)
            {
                string[] tokens_smallest = Console.ReadLine().Split(' ');

                int smallest = Convert.ToInt32(tokens_smallest[0]);
                int largest = Convert.ToInt32(tokens_smallest[1]);

                // your code goes here
                vendorIntervals.Add(new Tuple<int, int>(smallest, largest));
            }

            Console.WriteLine(CalculateMatchesFromVendors(sizesForParticipants, vendorIntervals));
        }
    }

    /*
     * vendors - up to 5*10000 vendors
     * participants - up to 5*10000 participants
     * size - up to 1000 * 1000 * 1000
     * 
     * Sort the array of sizesForParticipants  - nlogn, n is 5 * 100000
     * Sort the vendor intervals - start value - nlogn, n is 5 * 100000
     */
    public static int CalculateMatchesFromVendors(int[] sizesForParticipants, List<Tuple<int, int>> vendorIntervals)
    {
        Array.Sort(sizesForParticipants);

        vendorIntervals.Sort();

        int n = vendorIntervals.Count;
        var mergedIntervals = mergeIntervalsScan(vendorIntervals);

        vendorIntervals = null; // in case mistakely use vendorIntervals

        int count = 0;

        foreach (int size in sizesForParticipants)
        {
            if (binarySearchIntervals(size, mergedIntervals))
            {
                count++;
            }
        }

        return count;
    }

    private static bool SearchIntervals(int size, List<Tuple<int, int>> vendorIntervals, ref int start)
    {
        for (int i = start; i < vendorIntervals.Count; i++)
        {
            if (isInInterval(size, vendorIntervals[i]))
            {
                start = i;
                return true;
            }

            if (size < vendorIntervals[i].Item1)
            {
                return false;
            }
        }

        return false;
    }

    /*
     * Find the interval i, size >= i.Item1, but size < (i+1).Item1 if i + 1 < totalIntervals
     * 
     * Test case: [5, 100] [200,300], look for size 101 
     * Make sure that one interval will work, two intervals will work, 3 intervals will work
     */
    private static bool binarySearchIntervals(int size, List<Tuple<int, int>> vendorIntervals)
    {
        int length = vendorIntervals.Count;

        if (size < vendorIntervals[0].Item1 || size > vendorIntervals[length - 1].Item2)
        {
            return false;
        }

        int begin = 0;
        int end = length - 1;

        while (begin < end && (end - begin) > 1)
        {
            int middle = begin + (end - begin) / 2;
            if (size < vendorIntervals[middle].Item1)
            {
                end = middle;
            }
            else
            {
                begin = middle;
            }
        }

        return (isInInterval(size, vendorIntervals[begin]) ||
               isInInterval(size, vendorIntervals[end]));
    }

    private static bool isInInterval(int size, Tuple<int, int> interval)
    {
        return size >= interval.Item1 && size <= interval.Item2;
    }
    /*
     * Merge intervals - 
     * reduce the time for later lookup
     */
    private static List<Tuple<int, int>> mergeIntervalsScan(List<Tuple<int, int>> vendorIntervals)
    {
        var merged = new List<Tuple<int, int>>();

        var previous = vendorIntervals[0];

        for (int i = 1; i < vendorIntervals.Count; i++)
        {
            var current = vendorIntervals[i];

            // if there is no overlap 
            if (previous.Item2 < current.Item1)
            {
                merged.Add(previous);
                previous = current;
            }
            else
            {
                previous = new Tuple<int, int>(previous.Item1, Math.Max(previous.Item2, current.Item2)); // bug - not use max, use current.Item2
            }
        }

        merged.Add(previous);

        return merged;
    }
}