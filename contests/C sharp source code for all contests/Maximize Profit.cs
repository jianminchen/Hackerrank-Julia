using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    static void Main(String[] args)
    {
        string[] tokens_n = Console.ReadLine().Split(' ');
        int numberOfCurrencies = Convert.ToInt32(tokens_n[0]);
        int amountOfBitcoins = Convert.ToInt32(tokens_n[1]);
        int bitcoinToDollar = Convert.ToInt32(tokens_n[2]);

        string[] a_temp = Console.ReadLine().Split(' ');

        int[] cryptoToDollar = Array.ConvertAll(a_temp, Int32.Parse);

        string[] b_temp = Console.ReadLine().Split(' ');
        int[] bitcoinToCrypto = Array.ConvertAll(b_temp, Int32.Parse);

        int result = maximizeProfit(cryptoToDollar, bitcoinToCrypto, amountOfBitcoins, bitcoinToDollar);
        Console.WriteLine(Math.Max(result, amountOfBitcoins * bitcoinToDollar));
    }

    static int maximizeProfit(int[] cryptoToDollar, int[] bitcoinToCrypto, int amountOfBitcoins, int k)
    {
        int bitcoinToDollarMaxValue = Int32.MinValue;
        var length = cryptoToDollar.Length;

        for (int i = 0; i < length; i++)
        {
            var current = cryptoToDollar[i] * bitcoinToCrypto[i];
            bitcoinToDollarMaxValue = current > bitcoinToDollarMaxValue ? current : bitcoinToDollarMaxValue;
        }

        return amountOfBitcoins * bitcoinToDollarMaxValue;
    }
}
