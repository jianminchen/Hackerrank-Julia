using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    static void Main(String[] args)
    {
        ProcessInput();
        //RunTestcase(); 
    }

    public static void RunTestcase()
    {
        //int result = QueryKHappiness(n, k);
    }

    public static void ProcessInput()
    {
        int queries = Convert.ToInt32(Console.ReadLine());
        for (int index = 0; index < queries; index++)
        {
            string[] tokens_n = Console.ReadLine().Split(' ');
            int n = Convert.ToInt32(tokens_n[0]);
            int k = Convert.ToInt32(tokens_n[1]);
            // Find the number of ways to arrange 'n' people such that at least 'k' of them will be happy
            // The return value must be modulo 10^9 + 7
            int result = QueryKHappiness(n, k);
            Console.WriteLine(result);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="n"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    static int QueryKHappiness(int n, int k)
    {
        // Complete this function
        var ids = new int[n];
        for (int i = 0; i < n; i++)
        {
            ids[i] = i;
        }

        var left = new HashSet<int>();
        foreach (var id in ids)
        {
            left.Add(id);
        }
        var used = new HashSet<int>();

        long total = 0;
        FindKHappiness(n, left, used, 0, -1, false, k, 0, ref total);
        return (int)total % (1000 * 1000 * 1000 + 7);
    }

    /// <summary>
    /// previous -1 - no previous
    /// >=0, value of previous 
    /// 
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="used"></param>
    /// <param name="start"></param>
    /// <returns></returns>
    private static void FindKHappiness(int n, HashSet<int> left, HashSet<int> used, int start, int previous, bool checkPrevious, int minimum, int kHappiness, ref long total)
    {
        if (start == n)
        {
            if (kHappiness >= minimum)
            {
                total++;
            }

            return;
        }

        foreach (var id in left)
        {
            var nextUsed = getNextUsed(used, id);
            var nextLeft = getNextLeft(left, id);

            if (previous >= 0)
            {
                if (previous > id)
                {
                    FindKHappiness(n, nextLeft, nextUsed, start + 1, id, false, minimum, 1 + kHappiness, ref total); // current is happy
                }
                else
                {
                    // current is unknown, need to check the next one if there is one                    
                    if (checkPrevious)
                    {
                        FindKHappiness(n, nextLeft, nextUsed, start + 1, id, true, minimum, 1 + kHappiness, ref total);
                    }
                    else
                    {
                        FindKHappiness(n, nextLeft, nextUsed, start + 1, id, true, minimum, kHappiness, ref total);
                    }
                }
            }
            else // no previous
            {
                FindKHappiness(n, nextLeft, nextUsed, start + 1, id, true, minimum, kHappiness, ref total);
            }
        }

        return;
    }

    private static HashSet<int> getNextUsed(HashSet<int> used, int id)
    {
        var nextUsed = new HashSet<int>(used);
        nextUsed.Add(id);

        return nextUsed;
    }

    private static HashSet<int> getNextLeft(HashSet<int> left, int id)
    {
        var nextLeft = new HashSet<int>(left);
        nextLeft.Remove(id);

        return nextLeft;
    }
}
