using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minimumLoss
{
    class Program
    {
        static void Main(string[] args)
        {
            process();
            //testcase2(); 
        }

        private static void process()
        {
            int n = int.Parse(Console.ReadLine());

            Int64[] prices = new Int64[n];

            string[] arr = Console.ReadLine().Split(' ');
            prices = Array.ConvertAll(arr, Int64.Parse);

            Console.WriteLine(minimumLossCal(n, prices));
        }

        private static void testcase2()
        {
            Int64[] prices = new Int64[5] { 20, 7, 8, 2, 5 };

            Console.WriteLine(minimumLossCal(5, prices));
        }

        private static void testcase3()
        {
            Int64[] prices = new Int64[4] { 2, 3, 4, 1 };

            Console.WriteLine(minimumLossCal(4, prices));
        }


        /*
         * minimum loss
         * finished time: 4:02pm
         * 
         * read Java TreeSet floor method:
         * https://www.tutorialspoint.com/java/util/treeset_floor.htm
         * 
         * http://stackoverflow.com/questions/4872946/linq-query-to-select-top-five
         * 
         * http://stackoverflow.com/questions/11549580/find-key-with-max-value-from-sorteddictionary
         * 
         * http://stackoverflow.com/questions/1635497/orderby-descending-in-lambda-expression
         * 
         * timeout issue - try to find LINQ has a solution or not
         * http://stackoverflow.com/questions/14675108/sortedset-sortedlist-with-better-linq-performance
         * 
         * 
         */
        private static Int64 minimumLossCal(int n, Int64[] prices)
        {
            SortedSet<Int64> data = new SortedSet<Int64>();

            Int64 minLoss = Int64.MaxValue;

            for (int i = n - 1; i >= 0; i--)
            {
                Int64 curPrice = prices[i];
                Int64 minVal = curPrice - minLoss + 1;
                Int64 maxVal = curPrice - 1;
                if (minVal <= maxVal)
                {
                    var smaller = data.GetViewBetween(minVal, maxVal);
                    if (smaller.Any())
                    {
                        minLoss = curPrice - smaller.Max;
                    }
                }

                data.Add(curPrice);
            }

            return minLoss;
        }
    }
}