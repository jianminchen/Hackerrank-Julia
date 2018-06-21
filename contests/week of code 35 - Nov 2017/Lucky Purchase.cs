using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    public class Laptop
    {
        public string Name { get; set; }
        public string ValueString { get; set; }
        public int Value { get; set; }
    }

    static void Main(String[] args)
    {
        ProcessInput();
    }

    public static void ProcessInput()
    {
        int n = Convert.ToInt32(Console.ReadLine());

        var nameValue = new List<Laptop>();

        for (int index = 0; index < n; index++)
        {
            var laptop = new Laptop();
            var arguments = Console.ReadLine().Split(' ');

            laptop.Name = arguments[0];
            laptop.Value = Convert.ToInt32(arguments[1]);
            laptop.ValueString = arguments[1];

            nameValue.Add(laptop);
        }

        Console.WriteLine(GetMinimumPriceLaptopOnly7And4(nameValue));
    }

    public static string GetMinimumPriceLaptopOnly7And4(IList<Laptop> nameValue)
    {
        const string notFound = "-1";

        var minValue = Int32.MaxValue;
        var nameForMinimum = "";
        bool found = false;
        for (int i = 0; i < nameValue.Count; i++)
        {
            var name = nameValue[i].Name;
            var valueString = nameValue[i].ValueString;
            var value = nameValue[i].Value;

            var isWithSameNumberOf7And4 = check7And4(valueString);

            if (!isWithSameNumberOf7And4)
            {
                continue;
            }

            var foundSmallerOne = value < minValue;

            minValue = foundSmallerOne ? value : minValue;
            nameForMinimum = foundSmallerOne ? name : nameForMinimum;
            found = true;
        }

        return found ? nameForMinimum : notFound;
    }

    private static bool check7And4(string valueString)
    {
        var countOf4 = 0;
        var countOf7 = 0;

        var length = valueString.Length;

        for (int i = 0; i < valueString.Length; i++)
        {
            var visit = valueString[i];
            var digitIndex = "0123456789".IndexOf(visit);
            var isDigit = digitIndex != -1;

            bool is4 = visit == '4';
            bool is7 = visit == '7';

            if (!isDigit)
            {
                continue;
            }

            var is7Or4DigitsOnly = is4 || is7;

            if (!is7Or4DigitsOnly) // can be +, . etc. 
            {
                return false;
            }

            if (is4)
            {
                countOf4++;
            }

            if (is7)
            {
                countOf7++;
            }
        }

        return countOf4 == countOf7;
    }
}
