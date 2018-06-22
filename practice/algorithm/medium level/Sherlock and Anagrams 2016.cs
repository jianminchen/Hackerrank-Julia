using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
class Solution
{
    /// <summary>
    /// code review by June chen 6/22/2018
    /// https://www.hackerrank.com/challenges/sherlock-and-anagrams/problem
    /// </summary>
    /// <param name="args"></param>
    static void Main(String[] args)
    {
        /* Enter your code here. Read input from STDIN. Print output to STDOUT. Your class should be named Solution */
        int n = Convert.ToInt16(Console.ReadLine());
        for (int i = 0; i < n; i++)
        {
            Console.WriteLine(anagrammaticPair(Console.ReadLine().Trim()));
        }
    }

    public static int anagrammaticPair(string s)
    {
        if (s == null || s.Length == 0) return 0;
        int n = s.Length;

        int count = 0;
        for (int i = 1; i <= n - 1; i++)
        {
            count += aPair(s, i);
        }

        return count;
    }
    /*
     * get the anagrammatic pair of length m
     * 
     * substring S[i,j] with length m, at most n - m, n is length of string
     */
    private static int aPair(string s, int m)
    {
        Hashtable table = getTable(s, m);

        int count = 0;
        foreach (string s1 in table.Keys)
        {
            IList<int> list = (IList<int>)table[s1];

            count += getPairs(list, m);
        }
        return count;
    }

    private static int getPairs(IList<int> list, int m)
    {
        int n = list.Count;
        return n * (n - 1) / 2;
    }

    private static Hashtable getTable(string s, int m)
    {
        Hashtable table = new Hashtable();

        int n = s.Length;
        for (int i = 0; i <= n - m; i++)
        {
            string tmp = s.Substring(i, m);

            //if (table.Contains(tmp))
            string key = "";
            if (tableContainsAnagram(table, tmp, ref key))
            {
                IList<int> list = (IList<int>)table[key];
                IList<int> newList = new List<int>(list);
                newList.Add(i);
                table[key] = newList;
            }
            else
            {
                IList<int> list = new List<int>();
                list.Add(i);
                table.Add(tmp, list);
            }
        }

        return table;
    }

    private static bool tableContainsAnagram(Hashtable table, string tmp, ref string key)
    {
        foreach (string s in table.Keys)
        {
            if (isAnagram(s, tmp))
            {
                key = s;
                return true;
            }
        }
        return false;
    }

    /*
     * Two strings are anagram, count of char should be same
     */
    private static bool isAnagram(string s1, string s2)
    {
        int[] cA = new int[26];
        foreach (char c in s1)
        {
            cA[c - 'a']++;
        }

        foreach (char c in s2)
        {
            cA[c - 'a']--;
        }

        foreach (int val in cA)
        {
            if (val != 0)
                return false;
        }

        return true;
    }
}