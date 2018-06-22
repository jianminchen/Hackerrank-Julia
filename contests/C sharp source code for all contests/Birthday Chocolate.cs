using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birthdayBar
{
    /// <summary>
    /// https://www.hackerrank.com/contests/womens-codesprint-3/challenges/the-birthday-bar/problem
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            var chocolateBar = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

            var birthdayDay = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
            Console.WriteLine(CalculateTotalWayByBirthday(chocolateBar, birthdayDay));
        }

        /*
         * birthday - [0] birth day, [1] month
         * sum - birth day value 
         * number of consecutive squares - birth month
         */
        public static int CalculateTotalWayByBirthday(int[] chocolateBar, int[] birthdayDay)
        {
            int day = birthdayDay[0];
            int month = birthdayDay[1];

            int n = chocolateBar.Length;

            int count = 0;

            for (int start = 0, end = start + month; end <= n; start++, end++)
            {
                int sum = 0;
                for (int i = start; i < end; i++)
                {
                    sum += chocolateBar[i];
                }

                if (sum == day)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
