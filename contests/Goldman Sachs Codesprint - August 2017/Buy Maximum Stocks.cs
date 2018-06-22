using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    /// <summary>
    /// https://www.hackerrank.com/contests/gs-codesprint/challenges/buy-maximum-stocks/problem
    /// </summary>
    /// <param name="args"></param>
    static void Main(String[] args)
    {
        ProcessInput();
        //RunTestcase(); 
    }

    public static void RunTestcase()
    {
        var n = 3;
        var initialAmount = 45;
        var prices = new int[] { 10, 7, 19 };

        var numberOfStocks = BuyMaximumProducts(n, initialAmount, prices);
    }

    public static void ProcessInput()
    {
        int n = Convert.ToInt32(Console.ReadLine());
        string[] arr_temp = Console.ReadLine().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

        int[] arr = Array.ConvertAll(arr_temp, Int32.Parse);

        long k = Convert.ToInt64(Console.ReadLine());

        long result = BuyMaximumProducts(n, k, arr);

        Console.WriteLine(result);
    }

    /// <summary>
    /// use bucket sort to store total number of stocks to purchase from 1 to 100 dollars
    /// </summary>
    /// <param name="n"></param>
    /// <param name="initialAmount"></param>
    /// <param name="prices"></param>
    /// <returns></returns>
    public static long BuyMaximumProducts(int n, long initialAmount, int[] prices)
    {
        const int MaximumStockPrice = 100;
        var numbers = new long[MaximumStockPrice + 1]; // int -> long

        // get number for each price from 1 to 100        
        for (int day = 0; day < Math.Min(prices.Length, n); day++)
        {
            var price = prices[day];
            var maximumNumberToPurchase = day + 1;

            numbers[price] += maximumNumberToPurchase;
        }

        // go over each price from low to high until running out of money
        long totalNumbers = 0;
        long moneyAvailable = initialAmount;
        for (int value = 1; value <= MaximumStockPrice; value++)
        {
            var numberOfStock = numbers[value];

            if (numberOfStock == 0)
            {
                continue;
            }

            long maximumPurchase = ((long)numberOfStock) * value;

            var purchaseAll = maximumPurchase <= moneyAvailable;
            if (purchaseAll)
            {
                totalNumbers += numberOfStock;
                moneyAvailable -= maximumPurchase;
            }
            else
            {
                totalNumbers += moneyAvailable / value;
                break;
            }
        }

        return totalNumbers;
    }
}