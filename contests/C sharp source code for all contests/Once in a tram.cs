using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    static void Main(String[] args)
    {
        Process();
        //testSubroutine(); 
    }

    static void testSubroutine()
    {
        //var result = getDigitSum(101, 3); 
        var result = onceInATram(555555);
    }

    static void Process()
    {
        int x = Convert.ToInt32(Console.ReadLine());
        string result = onceInATram(x);
        Console.WriteLine(result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    static string onceInATram(int x)
    {
        int firstThreeDigit = x / 1000;
        int lastThreeDigit = x % 1000;

        int digitsSum = getDigitSum(firstThreeDigit, 3);

        int index = 0;
        int increment = x;
        while (increment <= 999999)
        {
            increment++;
            var last3Digits = getDigitSum(increment % 1000, 3);
            var first3Digits = getDigitSum(increment / 1000, 3);

            if (last3Digits == first3Digits)
            {
                return increment.ToString();
            }

            index++;
        }

        return "";
    }

    /// <summary>
    /// number < 1000
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    private static int getDigitSum(int number, int digit)
    {
        if (number == 0 || digit == 0)
        {
            return 0;
        }

        int value = (int)Math.Pow(10, digit - 1);
        return number / value + getDigitSum(number % value, digit - 1);
    }
}