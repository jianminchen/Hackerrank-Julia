using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralAdvertising
{
    class Program
    {
        /*
         * https://www.hackerrank.com/contests/openbracket/challenges/strange-advertising
         * 6:00am - read the problem statement half hour
         * 7:34am
         * start to write code
         * concern: 
         * How big is the result? Long or big integer
         * 3^10 = 81 * 81 * 9 = (close to ) 64000
         * 1 <= n <= 50 
         * (3/2)^50, not 3^50
         * 1.5^10 < 1000, 
         * 
         * 7:43am exit the function
         */

        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());

            long sum = 0;
            int no = 5;
            for (int i = 0; i < n; i++)
            {
                int half = no / 2;
                sum += half;
                no = half * 3;
            }

            Console.WriteLine(sum);
        }


    }
}
