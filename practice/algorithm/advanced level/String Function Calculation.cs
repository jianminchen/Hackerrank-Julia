using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

class Solution
{
    /// <summary>
    /// code review on June 22, 2018
    /// https://www.hackerrank.com/challenges/string-function-calculation/problem
    /// </summary>
    public struct myTuple
    {
        public int originalIndex;
        public int firstHalf;
        public int secondHalf;
    }

    public static myTuple[] CreateSuffixArray(string s)
    {
        // suffixRank table holds the rank of each string on each iteration  
        // suffixRank[i][j] denotes rank of jth suffix at ith iteration  
        int[,] suffixRank = new int[20, 1000000];
        // Initialize size of string as N
        int N = s.Length;

        // Initialize suffix ranking on the basis of only single character
        // for single character ranks will be 'a' = 0, 'b' = 1, 'c' = 2 ... 'z' = 25
        for (int i = 0; i < N; i++)
            suffixRank[0, i] = s[i] - 'a';

        // Create a tuple array for each suffix
        myTuple[] L = new myTuple[N];

        // Iterate log(n) times i.e. till when all the suffixes are sorted
        // 'stp' keeps the track of number of iteration
        // 'cnt' store length of suffix which is going to be compared

        // On each iteration initialize the tuple for each suffix array
        // with values computed from previous iteration
        for (int cnt = 1, stp = 1; cnt < N; cnt *= 2, stp++)
        {

            for (int i = 0; i < N; i++)
            {
                L[i].firstHalf = suffixRank[stp - 1, i];
                L[i].secondHalf = i + cnt < N ? suffixRank[stp - 1, i + cnt] : -1;
                L[i].originalIndex = i;
            }

            // On the basis of tuples obtained sort the tuple array
            // function to compare two suffix in O(1)  
            // first check whether first half chars of 'a' are equal to first half chars of 'b'  
            // if they are equal compare second half else compare decide on rank of first half 
            Array.Sort(L, (first, second) =>
            {
                var firstHalfComparison = first.firstHalf.CompareTo(second.firstHalf);
                if (firstHalfComparison != 0)
                    return firstHalfComparison;
                else return first.secondHalf.CompareTo(second.secondHalf);
            });

            // Initialize rank for rank 0 suffix after sorting to its original index
            // in suffixRank array
            suffixRank[stp, L[0].originalIndex] = 0;


            // compare ith ranked suffix ( after sorting ) to (i - 1)th ranked suffix
            // if they are equal till now assign same rank to ith as that of (i - 1)th
            // else rank for ith will be currRank ( i.e. rank of (i - 1)th ) plus 1, i.e
            //( currRank + 1 )        
            for (int i = 1, currRank = 0; i < N; i++)
            {
                if (L[i - 1].firstHalf != L[i].firstHalf || L[i - 1].secondHalf != L[i].secondHalf)
                    ++currRank;

                suffixRank[stp, L[i].originalIndex] = currRank;
            }
        }
        return L;
    }

    public static int[] CreateLCPArray(string s, int[] SA)
    {
        int n = s.Length, k = 0;
        int[] LCP = new int[n];
        int[] rank = new int[n];

        for (int i = 0; i < n; i++) rank[SA[i]] = i;

        for (int i = 0; i < n; i++)
        {
            if (k > 0) k--;
            else k = 0;

            if (rank[i] == n - 1) k = 0;
            else
            {
                int j = SA[rank[i] + 1];
                while (i + k < n && j + k < n && s[i + k] == s[j + k]) k++;
                LCP[rank[i]] = k;
            }
        }
        return LCP;
    }

    public static int CalculateNumberOfInstances(int[] LCP, int[] SA, string s)
    {
        int curMax = s.Length;
        int repCount = 1;

        for (int i = 0; i < s.Length; i++)
        {

            int lengthOfSS = LCP[i];

            int iterator = i;
            while ((iterator < s.Length) && (LCP[iterator] >= lengthOfSS) && (lengthOfSS > 0))
            {
                repCount++;
                iterator++;
            }

            if (iterator != 0) iterator = i - 1;
            while ((iterator >= 0) && (LCP[iterator] >= lengthOfSS) && (lengthOfSS > 0))
            {
                repCount++;
                iterator--;
            }

            if ((repCount * lengthOfSS) > curMax)
            {

                curMax = repCount * lengthOfSS;
            }

            repCount = 1;
        }

        return curMax;
    }

    static void Main(String[] args)
    {
        string s = Console.ReadLine();
        myTuple[] TupleSA = CreateSuffixArray(s);

        int[] SA = new int[s.Length];
        for (int i = 0; i < s.Length; i++)
        {
            SA[i] = TupleSA[i].originalIndex;
        }
        int[] LCP = CreateLCPArray(s, SA);

        Console.WriteLine(CalculateNumberOfInstances(LCP, SA, s));

    }
}