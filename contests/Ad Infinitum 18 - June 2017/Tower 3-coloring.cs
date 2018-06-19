using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    static void Main(String[] args)
    {
        //RunTestcase(); 
        ProcessInput();
        //var result = power(10, 3); 
    }

    public static void RunTestcase()
    {
        var memo = new Dictionary<int, int>();
        prepareMemo(memo, 500);

        int result = towerColoring(1001, memo, 500);
    }

    public static void ProcessInput()
    {
        int n = Convert.ToInt32(Console.ReadLine());
        const int memoSize = 1000;
        var memo = new Dictionary<int, int>();
        prepareMemo(memo, memoSize);
        int result = towerColoring(n, memo, memoSize);
        Console.WriteLine(result);
    }

    static void prepareMemo(Dictionary<int, int> memo, int size)
    {
        // Recurrence formula 
        // F(n) = F(n-1) * F(n-1) * F(n-1), F(0) = 27^0), F(1) = 27 = 27^1, n >= 1              

        memo.Add(0, 1);
        memo.Add(1, 27);
        for (int i = 2; i <= size; i++)
        {
            var previous = (long)memo[i - 1];
            var current = previous;
            current = multiplication(current, previous);
            current = multiplication(current, previous);

            memo.Add(i, (int)current);
        }
    }

    /// <summary>
    /// n is in the range of 1 to 1000000000
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    static int towerColoring(int n, Dictionary<int, int> memo, int size)
    {
        long modulo = 1000 * 1000 * 1000 + 7;

        if (n <= size)
        {
            return memo[n];
        }

        int residue = n % size;
        int divisor = n / size;

        var baseValue = (long)memo[size];
        var lookup = (long)memo[residue];
        long result = lookup * power(baseValue, (long)divisor) % modulo;
        return (int)result;
    }

    private static long power(long baseValue, long power)
    {
        long modulo = 1000 * 1000 * 1000 + 7;

        long result = 1;
        for (int i = 0; i < power; i++)
        {
            result *= baseValue % modulo;
        }

        return result;
    }

    private static long multiplication(long number1, long number2)
    {
        long modulo = 1000 * 1000 * 1000 + 7;
        return number1 * number2 % modulo;
    }
}