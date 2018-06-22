using System;
using System.Collections.Generic;
using System.IO;
class Solution
{
    /// <summary>
    /// code review - Julia June 22, 2018
    /// https://www.hackerrank.com/challenges/reverse-shuffle-merge/problem
    /// </summary>
    /// <param name="args"></param>
    static void Main(String[] args)
    {
        //runTestCases(); 
        Console.WriteLine(reverseShuffleMerge(Console.ReadLine()));
    }

    public static void runTestCases()
    {
        string[] testCases = new string[4] { "abcacb", "aaccbb", "cacbab", "baabcc" };

        List<string> outputStrs = new List<string>();
        foreach (string s in testCases)
        {
            string s2 = reverseShuffleMerge(s);

            outputStrs.Add(s2);
        }

        string[] result = outputStrs.ToArray();
    }

    public static string reverseShuffleMerge(string input)
    {
        if (input == null || input.Length == 0)
            return null;

        int len = input.Length;
        int TOTALCHARS = 26;

        int[] moreToAdd = new int[TOTALCHARS];
        int[] moreToSkip = new int[TOTALCHARS];

        foreach (char c in input)
        {
            moreToAdd[c - 'a']++;
        }

        for (int i = 0; i < TOTALCHARS; i++)
        {
            moreToAdd[i] = moreToAdd[i] / 2;
            moreToSkip[i] = moreToAdd[i];
        }

        Stack<char> stack = new Stack<char>();

        for (int i = len - 1; i >= 0; i--)
        {
            char runner = input[i];
            int index = runner - 'a';

            while (stack.Count > 0 &&
                   moreToAdd[index] > 0 &&
                   (char)stack.Peek() > runner &&
                   moreToSkip[(char)stack.Peek() - 'a'] > 0
                )
            {
                // "abcacb", c is on top of stack, 
                // 'c' - 'a' = 2, 
                // moreToAdd[2] = 0, moreToSkip[2] = 1; 
                // pop out 'c', and then, adjust the value for 'c'
                // moreToAdd[2] = 1, and moreToSkip[2] = 0.
                // while will be excuted two times, one for 'c', one for 'b'
                char backTracked = (char)stack.Pop();  // bug: do not overwrite runner

                int oldIndex = backTracked - 'a';

                moreToAdd[oldIndex]++;
                moreToSkip[oldIndex]--;
            }

            if (moreToAdd[index] > 0)
            {
                stack.Push(runner);
                moreToAdd[index]--;
            }
            else
            {
                moreToSkip[index]--;
            }
        }

        char[] arr = stack.ToArray();
        Array.Reverse(arr);

        return new string(arr);
    }
}