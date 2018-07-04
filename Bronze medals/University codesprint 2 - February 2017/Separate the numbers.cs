using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeparateTheNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessInput();
            //RunSampleTestcase(); 
        }

        public static void RunSampleTestcase()
        {
            //var results = SeparateTheNumbersChecking(new string[] { "91011" });
            var result2 = SeparateTheNumbersChecking(new string[] { "99100" });
            //var result2 = SeparateTheNumbersChecking(new string[] { "99100101102103104105106107108109110" });
        }

        public static void ProcessInput()
        {
            int n = Convert.ToInt32(Console.ReadLine());

            string[] input = new string[n];
            for (int i = 0; i < n; i++)
            {
                input[i] = Console.ReadLine();
            }

            long[] SeparateData = SeparateTheNumbersChecking(input);

            foreach (long value in SeparateData)
            {
                if (value == -1)
                {
                    Console.WriteLine("NO");
                }
                else
                {
                    Console.WriteLine("YES " + value);
                }
            }
        }

        public static long[] SeparateTheNumbersChecking(string[] input)
        {
            var results = new long[input.Length];

            int index = 0;
            foreach (string value in input)
            {
                results[index] = CheckStringIncreasingSplitable(value);
                index++;
            }

            return results;
        }

        /*
         * 1.string can be splited in an array of integer
         * 2. integers are in increasing order from left to right, increment by one
         * 3. no leading zero
         * 4. split into 2 or more positive integer
         */
        private static long CheckStringIncreasingSplitable(string s)
        {
            int notFound = -1;

            if (IsLeadingZero(s))
            {
                return notFound;
            }

            int length = s.Length;
            int half = length / 2;

            string longMaxLength = Int64.MaxValue.ToString();

            // iterate on the length of first number
            for (int startLength = 1; startLength <= half && startLength <= longMaxLength.Length; startLength++)
            {
                string start = s.Substring(0, startLength);
                if (startLength == longMaxLength.Length &&
                    start.CompareTo(longMaxLength) >= 0)
                {
                    return notFound;
                }

                if (startingFromNumber(s, start))
                {
                    return Convert.ToInt64(start);
                }
            }

            return notFound;
        }

        /*
         * how to test the function?
         * 1. Two or more positive integers - ok
         * 2. increment value by one - ok
         * 3. no leading zero
         * 
         */
        private static bool startingFromNumber(string s, string start)
        {
            int minimumLength = start.Length;
            int length = s.Length;
            int quotient = length / minimumLength;

            if (quotient < 2)
            {
                return false;
            }

            StringBuilder currentNumbers = new StringBuilder();

            currentNumbers.Append(start);

            int index = 1;
            int currentLength = minimumLength;
            long baseValue = Convert.ToInt64(start);
            long nextValue = baseValue;

            while (index <= quotient &&
                   currentLength < length &&
                   nextValue < Int64.MaxValue)
            {
                nextValue = baseValue + index;
                currentNumbers.Append(nextValue);

                currentLength += nextValue.ToString().Length;

                if (currentLength > length ||
                    s.Substring(0, currentLength).CompareTo(currentNumbers.ToString()) != 0)
                {
                    return false;
                }

                index++;
            }

            return (currentLength == length);
        }

        private static bool IsLeadingZero(string s)
        {
            return s[0] == '0';
        }
    }
}
