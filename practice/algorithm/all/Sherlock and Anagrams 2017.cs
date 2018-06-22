using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

class Solution
{
    /// <summary>
    /// code review June 22, 2018
    /// https://www.hackerrank.com/challenges/sherlock-and-anagrams/problem
    /// 
    /// </summary>
    public class HashedAnagramString
    {
        /*
         * Make sure that two anagram strings will have some hashed code
         * 
         * @frequencyTable - Dictionary<char, int>
         * The frequency table has to be sorted first and then construct 
         * a string with each char in alphabetic numbers concatenated by 
         * its occurrences. 
         * 
         */
        public static string GetHashedAnagram(int[] frequencyTable)
        {
            StringBuilder key = new StringBuilder();

            for (int i = 0; i < 26; i++)
            {
                int value = frequencyTable[i];
                if (value > 0)
                {
                    char c = (char)(i + 'a');
                    key.Append(c.ToString() + value);
                }
            }

            return key.ToString();
        }
    }

    static void Main(String[] args)
    {
        ProcessInput();
        //RunSampleTestcase();
    }

    public static void RunSampleTestcase()
    {
        var hashedAnagramsDictionary = ConstructHashedAnagramsDictionary("abba");

        var pairs = CalculatePairs(hashedAnagramsDictionary);

        Debug.Assert(pairs == 4);
    }

    public static void ProcessInput()
    {
        var queries = int.Parse(Console.ReadLine());

        while (queries-- > 0)
        {
            var input = Console.ReadLine();

            var hashedAnagramsDictionary = ConstructHashedAnagramsDictionary(input);

            Console.WriteLine(CalculatePairs(hashedAnagramsDictionary));
        }
    }

    /*
     * Prepare fequence table for N substring which starts from index 0
     * Do not need one for each of N*N substring. 
     * 
     * Work on one small test case:
     * "abcd"
     * so the frequency table for substrings, all starts from index = 0:
     * a
     * ab
     * abc
     * abcd
     * Only for those 4 substrings, not all of them. 
     * 
     * Time complexity:  O(N * N)
     * Space complexity: O(26*N), N is the string's length
     */
    public static int[][] PrepareFequencyTableForOnlyNSubstring(string input)
    {
        if (input == null || input.Length == 0)
        {
            return null;
        }

        int length = input.Length;
        int[][] frequencyTables = new int[length][];

        for (int i = 0; i < length; i++)
        {
            frequencyTables[i] = new int[26];
        }

        for (int start = 0; start < length; start++) // go over the string once from the beginning
        {
            char current = input[start];
            int charIndex = current - 'a';

            for (int index = start; index < length; index++)
            {
                frequencyTables[index][charIndex]++;
            }
        }

        return frequencyTables;
    }

    /*
     * What should be taken cared of in the design? 
     * Time complexity: O(26 * N * N), N is the string length. 
     * 
     * 
     * I think that it is same idea as Leetcode 238 Product of array except itself. We 
     * take advantage of alphabetic size is limited and constant, 26 chars, and then work 
     * with substring (denote Si) starting from 0 to i, 0 < i < n ( n is string's length)
     * to calculate frequency table, and any substring starting from i and ending j can be 
     * Sj's frequency table - Si's frequency table for those 26 alphabetic numbers.
     * So, the preprocessed frequency table size is O(N) ( N is the length of string) 
     * instead of O(N^2) based on each of substrings. For any of substrings, there are only 26 
     * calculation to compute for each alphabet number, so the time complexity goes 
     * to O(26N^2) = O(n^2)               
     * 
     * Update hashed anagram counting dictionary - a statistics, basically 
     * tell the fact like this:
     * For example, test case string abba, 
     * substring ab -> hashed key a1b1, value is 2, because there are 
     * two substrings: "ab","ba" having hashed key: "a1b1"
     * Here are all possible hashed keys: 
     * a1   - a, a, 
     * b1   - b, b
     * a1b1 - ab, ba
     * b2   - bb
     * a1b2 - abb, bba
     * a2b2 - abba
     * 
     * Time complexity is O(N^2), not O(N^3). 
     */
    public static Dictionary<string, int> ConstructHashedAnagramsDictionary(string input)
    {
        var hashedAnagramsDictionary = new Dictionary<string, int>();

        var length = input.Length;

        // frequency table memo is using time O(26 * N)
        var fequencyTableMemo = PrepareFequencyTableForOnlyNSubstring(input);

        for (int start = 0; start < length; start++)
        {
            for (int substringLength = 1; start + substringLength <= length; substringLength++)
            {
                var frequencyData = CalculateSubstringFequencyDiff(fequencyTableMemo, start, substringLength);

                var key = HashedAnagramString.GetHashedAnagram(frequencyData);

                // At most there are O(N*N) entry in the dictionary, go over once
                if (hashedAnagramsDictionary.ContainsKey(key))
                {
                    hashedAnagramsDictionary[key]++;
                }
                else
                {
                    hashedAnagramsDictionary.Add(key, 1);
                }
            }
        }

        return hashedAnagramsDictionary;
    }

    /*
    * Frequency table calculation for substring starting from 'start' ending at 'end': 
    * Just a simple minus calculation for each alphabet number. 
    */
    public static int[] CalculateSubstringFequencyDiff(int[][] fequencyTableMemo, int start, int length)
    {
        const int size = 26;
        int[] difference = new int[size];

        for (int i = 0; i < size; i++)
        {
            difference[i] = fequencyTableMemo[start + length - 1][i];
            if (start > 0)
            {
                difference[i] -= fequencyTableMemo[start - 1][i];
            }
        }

        return difference;
    }


    /*
     * The formula to calculate pairs
     * For example, test case string abba, 
     * substring ab -> hashed key a1b1, value is 2, because there are two substrings: "ab","ba" having hashed key: "a1b1"
     * Here are all possible hashed keys: 
     * a1   - a, a, 
     * b1   - b, b
     * a1b1 - ab, ba
     * b2   - bb
     * a1b2 - abb, bba
     * a2b2 - abba
     * So, how many pairs? 
     * should be 4, from 4 hashed keys: a1, b1, a1b1 and a2b2
     */
    public static int CalculatePairs(Dictionary<string, int> hashedAnagrams)
    {
        // get pairs
        int anagrammaticPairs = 0;

        foreach (var count in hashedAnagrams)
        {
            anagrammaticPairs += count.Value * (count.Value - 1) / 2;
        }

        return anagrammaticPairs;
    }
}