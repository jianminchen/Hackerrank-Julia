using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
class Solution
{
    internal class CombinationWithEuler
    {
        /* the code is copied from here
         * https://comeoncodeon.wordpress.com/category/algorithm/
         * https://www.quora.com/How-do-I-find-the-value-of-nCr-1000000007-for-the-large-number-n-n-10-6-in-C
         */
        /* This function calculates (a^b)%MOD */
        public static long pow(int a, int b, int MOD)
        {
            long x = 1, y = a;
            while (b > 0)
            {
                if (b % 2 == 1)
                {
                    x = (x * y);
                    if (x > MOD) x %= MOD;
                }
                y = (y * y);
                if (y > MOD) y %= MOD;
                b /= 2;
            }
            return x;
        }

        /*  Modular Multiplicative Inverse
            Using Euler's Theorem
            a^(phi(m)) = 1 (mod m)
            a^(-1) = a^(m-2) (mod m) */
        public static long InverseEuler(int n, int MOD)
        {
            return pow(n, MOD - 2, MOD);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="r"></param>
        /// <param name="MOD"></param>
        /// <returns></returns>
        public static long ChooseKFromNWithModule(int n, int r, int MOD)
        {
            int[] f = new int[n + 1];

            f[0] = 1;
            f[1] = 1;
            for (int i = 2; i <= n; i++)
            {
                f[i] = (f[i - 1] * i) % MOD;
            }

            return (f[n] * ((InverseEuler(f[r], MOD) * InverseEuler(f[n - r], MOD)) % MOD)) % MOD;
        }
    }

    static void Main(String[] args)
    {
        ProcessInput();
        //RunTestcase2();
        //RunTestcase3(); 
        //TestFindFirstDigitMoreThanK(); 
        //testCalculateValue(); 
        //TestFindFirstDigit_2(); 

        //testcase22();
        //testcase23();

        //TestCombinationCalcuation();
    }

    /// <summary>
    /// input: 
    /// 4 3
    /// 9 15 27 14
    /// 
    /// output: 10 1
    /// </summary>
    public static void testcase22()
    {
        int n = 4;
        int k = 3;

        var numbers = new long[] { 9, 15, 27, 14 };

        var result = FindMaximalNumberFirst(n, k, numbers);
        Console.WriteLine(String.Join("\n", result));
    }

    /// <summary>
    /// input:
    /// 3 2
    /// 41 36 33
    /// 
    /// output: 33 1 
    /// </summary>
    public static void testcase23()
    {
        int n = 3;
        int k = 2;

        var numbers = new long[] { 41, 36, 33 };

        var result = FindMaximalNumberFirst(n, k, numbers);
        Console.WriteLine(String.Join("\n", result));
    }

    public static long testCalculateValue()
    {
        int[] digits = new int[64];
        digits[1] = 1;
        digits[3] = 1;

        return calculateBiggestKAndValue(digits);
    }

    public static void TestFindFirstDigitMoreThanK()
    {
        long[] sorted = new long[] { 0, 1, 2 };
        int foundValue = 0;
        int[] binaryArray = new int[64];
        int result = findNextLeftmostBit(ref sorted, 2, ref foundValue);
    }

    public static void TestFindFirstDigit_2()
    {
        long[] sorted = new long[] { 3, 6, 7 };
        int foundValue = 0;
        int[] binaryArray = new int[64];
        int result = findNextLeftmostBit(ref sorted, 3, ref foundValue);

        int result2 = findNextLeftmostBit(ref sorted, 2, ref foundValue);
    }

    public static void RunTestcase()
    {

        int n = 3;
        int k = 2;

        var numbers = new long[] { 3, 5, 6 };

        var result = FindMaximalNumberFirst(n, k, numbers);
        Console.WriteLine(String.Join("\n", result));
    }

    public static void RunTestcase2()
    {
        int n = 4;
        int k = 2;

        long number = (long)Math.Pow(2, 62);

        int start = Convert.ToString(1000000000000000000, 2).Length;
        long search = (long)Math.Pow(2, start);
        var numbers = new long[] { 21, 19, 22, 20 };

        var result = FindMaximalNumberFirst(n, k, numbers);
        Console.WriteLine(String.Join("\n", result));
    }

    public static void RunTestcase3()
    {
        int n = 4;
        int k = 3;

        var numbers = new long[] { 9, 15, 27, 14 };

        var result = FindMaximalNumberFirst(n, k, numbers);
        Console.WriteLine(String.Join("\n", result));
    }

    public static void ProcessInput()
    {
        string[] tokens_n = Console.ReadLine().Split(' ');
        int n = Convert.ToInt32(tokens_n[0]);
        int k = Convert.ToInt32(tokens_n[1]);

        var numbers = new long[n];

        for (int i = 0; i < n; i++)
        {
            numbers[i] = Convert.ToInt64(Console.ReadLine());
        }

        var result = FindMaximalNumberFirst(n, k, numbers);
        Console.WriteLine(String.Join("\n", result));
    }

    /// <summary>
    /// Find maximum number first 
    /// array size is 100000
    /// sort the array first, and then look up 2^63 downward to 2^0, each time determine
    /// if there are more than k numbers bigger than 2^n, if it is, then set binaryDigits[n] = 1, 
    /// </summary>
    /// <param name="n"></param>
    /// <param name="k"></param>
    /// <param name="numbers"></param>
    /// <returns></returns>
    public static long[] FindMaximalNumberFirst(int n, int k, long[] numbers)
    {
        var binaryDigits = new int[64];

        // sort the ascending order
        Array.Sort(numbers);

        var sortedNumbers = new long[n];
        Array.Copy(numbers, 0, sortedNumbers, 0, n);

        int totalNumbers = n;

        while (sortedNumbers.Count() >= k)
        {
            totalNumbers = sortedNumbers.Count();

            int leftmostBit = 0;

            // find leftmost bit with value 1
            var found = findNextLeftmostBit(ref sortedNumbers, k, ref leftmostBit);

            if (found < 0)
            {
                break;
            }

            binaryDigits[leftmostBit] = 1;

            // 
            sortedNumbers = skipFirstNthBit(sortedNumbers, k, found, leftmostBit);
        }

        var maximumBitwiseAndValue = calculateBiggestKAndValue(binaryDigits);

        // long totalNumber = 0;

        //  if (maximumBitwiseAndValue > 0)
        //  {
        long totalNumber = chooseKFromNUsingDynamicProgramming(k, totalNumbers);
        //long totalNumber = CombinationWithEuler.ChooseKFromNWithModule(totalNumbers, k, 1000 * 1000 * 1000 + 7);
        //  }       

        return new long[] { maximumBitwiseAndValue, totalNumber };
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sorted"></param>
    /// <param name="k"></param>
    /// <param name="firstDigit"></param>
    /// <returns></returns>
    private static long[] skipFirstNthBit(long[] sorted, int k, int firstDigit, int powerNumber)
    {
        long baseValue = (long)Math.Pow(2, powerNumber);
        int length = sorted.Length;
        int newLength = length - firstDigit;
        long[] residue = new long[newLength];

        int index = 0;
        for (int i = 0; i < newLength; i++)
        {
            residue[index++] = sorted[firstDigit + i] - baseValue;
        }

        return residue;
    }

    /// <summary>
    /// test the function 
    /// </summary>
    /// <param name="binaryValues"></param>
    /// <returns></returns>
    public static long calculateBiggestKAndValue(int[] binaryValues)
    {
        long sum = 0;
        int baseValue = 1;
        int length = binaryValues.Length;
        for (int i = 0; i < length; i++)
        {
            var current = binaryValues[i];
            sum += baseValue * current;
            baseValue *= 2;
        }

        return sum;
    }

    /// <summary>
    /// number can be expressed in the form of power of 2 
    /// 3  = 1 + 2
    /// 5  = 1 + 4
    /// 13 = 1 + 4 + 8
    /// m = 2^n_1 + 2^n_2 + ... + 2^n_j, 
    /// </summary>
    /// <param name="numbers"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    private static int findNextLeftmostBit(ref long[] numbers, int k, ref int foundValue)
    {
        int length = numbers.Length;
        int start = Convert.ToString(numbers[length - 1], 2).Length;
        start = start - 1;

        for (int power = start; power >= 0; power--)
        {
            long search = (long)Math.Pow(2, power);

            int size = numbers.Length;
            int end = numbers.Length - 1;
            //int index = FindFirstNotSmaller(numbers, search, 0, end);
            int index = FindFirstNotSmaller_recursive(numbers, search, 0, end);


            if (index >= 0 && (size - index) >= k)
            {
                foundValue = power;
                return index;
            }

            if (index >= 0)
            {
                // set the nth bit 1 to 0 in the array if the nth bit is 1
                numbers = maskFirstNBit(numbers, power);
                Array.Sort(numbers);
            }

            // discussion array size 
            if (numbers.Count() < k)
            {
                break;
            }
        }

        return -1;
    }

    /// <summary>
    /// this function is to decrement value by 2^n if it is bigger than 2^n     
    /// </summary>
    /// <param name="numbers"></param>
    /// <param name="power"></param>
    /// <returns></returns>
    private static long[] maskFirstNBit(long[] numbers, int power)
    {
        int length = numbers.Length;
        var newNumbers = new long[length];
        var compare = (long)Math.Pow(2, power);

        for (int i = 0; i < length; i++)
        {
            long current = numbers[i];

            newNumbers[i] = current;
            if (current >= compare)
            {
                newNumbers[i] = current % compare; // 5:58pm 4/29/2017 change - to % 
            }
        }

        return newNumbers;
    }

    /// <summary>
    /// assuming sorted array is ascending order
    /// this binary search algorithm is taken a lot of time to avoid bugs
    /// - find not smaller one, the first one, -1 not found, >=0, find the index
    /// using binary search 
    /// </summary>
    /// <param name="numbers"></param>
    /// <param name="search"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    private static int FindFirstNotSmaller_recursive(long[] numbers, long search, int start, int end)
    {
        // not found
        if (numbers[start] < search && numbers[end] < search)
        {
            return -1;
        }

        if (numbers[start] >= search)  // >=, add = 4/29/2017 8:53pm, after more than 5 hours work
        {
            return start;
        }

        if (end - start <= 1)
        {
            return end;
        }

        int middle = start + (end - start) / 2;
        var middleValue = numbers[middle];
        if (middleValue > search)
        {
            return FindFirstNotSmaller_recursive(numbers, search, start, middle);
        }
        else if (middleValue < search)
        {
            return FindFirstNotSmaller_recursive(numbers, search, middle, end);
        }
        else
        {
            return middle;
        }
    }

    /// <summary>
    /// recursive binary search may cause timeout issue - look into it!
    /// </summary>
    /// <param name="numbers"></param>
    /// <param name="search"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    private static int FindFirstNotSmaller(long[] numbers, long search, int start, int end)
    {
        // not found
        if (numbers[start] < search && numbers[end] < search)
        {
            return -1;
        }

        if (numbers[start] >= search)  // >=, add = 4/29/2017 8:53pm, after more than 5 hours work
        {
            return start;
        }

        if (end - start <= 1)
        {
            return end;
        }

        while (start < end)
        {
            int middle = start + (end - start) / 2;
            var middleValue = numbers[middle];
            if (middleValue > search)
            {
                end = middle;
            }
            else if (middleValue < search)
            {
                start = middle + 1;
            }
            else
            {
                return middle;
            }
        }

        return end;
    }

    /// <summary>
    /// consider alternative one - using internal class - CombinationWithEuler
    /// </summary>
    /// <param name="k"></param>
    /// <param name="totalNumbers"></param>
    /// <returns></returns>
    private static long chooseKFromN(int k, int totalNumbers)
    {
        int half = totalNumbers;
        if (k > half)
        {
            return chooseKFromN(totalNumbers - k, totalNumbers);
        }

        if (k == 0)
        {
            return 1;
        }

        const long moduleBase = 1000 * 1000 * 1000 + 7;

        // select k number from minCount

        long top = 1;
        long divisor = 1;

        int topPointer = totalNumbers;
        int downPointer = 1;
        long max = long.MaxValue;
        // for (int i = totalNumbers, j = 1; i >= 0 && j <= k; i--, j++)
        while (topPointer > 0 || downPointer <= k)
        {
            if (max / top > topPointer)
            {
                top *= topPointer;
                topPointer--;
            }

            if (max / divisor > downPointer)
            {
                divisor *= downPointer;
                downPointer++;
            }

            long gcd = getGreatCommonDivisor(top, divisor);
            top = top / gcd;
            divisor = divisor / gcd;
        }

        return top / divisor % moduleBase;
    }

    public static void TestCombinationCalcuation()
    {
        var result = chooseKFromNUsingDynamicProgramming(3, 5);
        Debug.Assert(result == 10);
    }

    /// <summary>
    /// C(n, k) = C(n-1, k) + C(n-1, k-1)
    /// 
    /// time complexity: n * k 
    /// </summary>
    /// <param name="k"></param>
    /// <param name="totalNumbers"></param>
    /// <returns></returns>
    private static long chooseKFromNUsingDynamicProgramming(int k, int totalNumbers)
    {
        if (k > totalNumbers) return -1;

        const long moduleBase = 1000 * 1000 * 1000 + 7;

        int SIZE = totalNumbers + 1;
        int CHOOSE = k + 1;

        long[][] combinations = new long[SIZE][];
        for (int i = 0; i < SIZE; i++)
        {
            combinations[i] = new long[CHOOSE];
            if (i > 0)
            {
                combinations[i][0] = 1;
                combinations[i][1] = i;
            }
        }

        combinations[0][0] = 0;

        for (int i = 1; i <= totalNumbers; i++)
        {
            for (int j = 2; j <= Math.Min(k, i); j++)
            {
                if (i == j) combinations[i][j] = 1;
                else
                    combinations[i][j] = (combinations[i - 1][j] + combinations[i - 1][j - 1]) % moduleBase;
            }
        }

        return combinations[totalNumbers][k] % moduleBase;
    }


    private static long getGreatCommonDivisor(long a, long b)
    {
        if (b == 0) return a;
        return getGreatCommonDivisor(b, a % b);
    }
}
