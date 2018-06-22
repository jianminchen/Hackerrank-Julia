using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HourankNo1
{
    /// <summary>
    /// June 22, 2018
    /// https://www.hackerrank.com/contests/hourrank-19/challenges/recover-the-array
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            int number = Convert.ToInt32(Console.ReadLine());

            int[] numbers = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

            int count = 0;
            for (int i = 0; i < number; )
            {
                int skip = numbers[i];
                i += skip + 1;
                count++;
            }

            Console.WriteLine(count);
        }
    }
}
