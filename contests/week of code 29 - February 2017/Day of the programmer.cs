using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayOfProgrammer
{
    /// <summary>
    /// https://www.hackerrank.com/contests/w29/challenges/day-of-the-programmer/problem
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            int year = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine(CalculateDayOfProgrammer(year));
        }

        public static string CalculateDayOfProgrammer(int year)
        {
            int start = 1700;

            bool isJulianCalendar = year >= start && year <= 1917;
            bool isGregorianCalendar = year >= 1919;
            int[] daysInFirst8Months = new int[] { 31, 28, 31, 30, 31, 30, 31, 31 };

            int offDaysInFeb = 0;
            if (year == 1998)
            {
                offDaysInFeb = 13;
            }

            if (isJulianCalendar && year % 4 == 0)
            {
                daysInFirst8Months[1] = 29;
            }

            if (isGregorianCalendar && (year % 400 == 0 || (year % 4 == 0 && year % 100 > 0)))
            {
                daysInFirst8Months[1] = 29;
            }

            daysInFirst8Months[1] -= offDaysInFeb;

            int theDayOfSept = 256 - daysInFirst8Months.Sum();

            return theDayOfSept + ".09." + year;
        }
    }
}
