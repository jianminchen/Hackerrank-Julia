using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudulentActivityNotifications
{
    class Program
    {
        /*
         * https://www.hackerrank.com/contests/openbracket/challenges/fraudulent-activity-notifications
         * 7:45am
         * start to read the problem statement
         * 8:14am
         * To save time, store first d days expenditures into the array, 
         * sort the array. O(dlogd) time complexity
         * To iterate, how to use existing sorted result, remove first one 
         * in the array, and then, add a new one to the sorted result. 
         * 
         * Extra space - sort array -> LinkedList -> remove first one in the array, 
         * and then add one more in the LinkedList 
         * 
         * 8:25am start to write code
         * Write first version - be careful on time complexity
         * Avoid timeout issues
         * 8:52am
         * http://stackoverflow.com/questions/2349589/is-this-a-good-way-to-iterate-through-a-net-linkedlist-and-remove-elements
         * 
         * 9:05am 
         * exit the function
         * Ready to do static analysis 
         * 9:11am
         * first execution on HackerRank
         * 9:18am
         * score 0 out of 20
         * pass test case #0 and #6, 
         * timeout on test case #1 - #5
         * 
         * 
         * Second trial: 
         * use binary search to speed up, O(logd) instead of linear search O(d)
         * work on code refactoring on 9:27am
         * 
         * copy existing array 
         * http://stackoverflow.com/questions/943635/getting-a-sub-array-from-an-existing-array
         * 
         * time complexity: 
         * set up the upper limit to (1)
         * http://stackoverflow.com/questions/21122143/what-is-the-time-complexity-for-copying-list-back-to-arrays-and-vice-versa-in-ja
         * LinkedList.ToArray() - C#, 
         * 
         * 10:11am 
         * start to review the code 
         * 
         * 10:24am
         * Bug to avoid:
         * Add position vs Remove position 
         * 
         * 10:40am
         * Java AddRange 
         * C# bulk copy 
         * http://stackoverflow.com/questions/9836471/why-is-addrange-faster-than-using-a-foreach-loop
         *
         * 
         * 12:25pm 
         * Find the bug
         * deadloop
         * write a test function using test case
         * 9 5
         * 2 3 4 2 3 6 8 4 5
         * 
         *  * start at 8:34am
         * 
         *
         * 10:16 
         * Write bucketSort version
         * complete the code on 10:17pm
         */
        static void Main(string[] args)
        {
            processing();
            //testing(); 
            //testCase2();
        }
        /*
         * test case: 
         *  * 9 5
         * 2 3 4 2 3 6 8 4 5
         */
        private static void testing()
        {
            int n = 9;
            int d = 5;
            string[] expenditures = "2 3 4 2 3 6 8 4 5".Split(' ');

            int result = calUsingBucketSort(n, d, expenditures);
        }

        /*
        * test case: 
        *   5 4
        * 1 2 3 4 4 
        */
        private static void testCase2()
        {
            int n = 5;
            int d = 4;
            string[] expenditures = "1 2 3 4 4".Split(' ');

            int result = calUsingBucketSort(n, d, expenditures);
        }
        /*
         * standalone function:
         * 12:27am 
         */
        private static void processing()
        {
            string[] input = Console.ReadLine().Split(' ');
            int n = Convert.ToInt32(input[0]);
            int d = Convert.ToInt32(input[1]);

            string[] expenditures = Console.ReadLine().Split(' ');
            Console.WriteLine(calUsingBucketSort(n, d, expenditures));
        }


        /*
         * 9:36pm start to write code
         * 
         * Read the problem statement
         * And then, find out the solution to solve timeout issue
         * using bucket sort. 
         * 
         */
        private static int calUsingBucketSort(
            int n,
            int d,
            string[] expenditures)
        {
            int SIZE = 201;
            int[] dPriorDays = new int[SIZE];

            for (int i = 0; i < d; i++)
            {
                int exp = Convert.ToInt32(expenditures[i]);
                dPriorDays[exp]++;
            }

            int count = 0;
            int start = 0;
            for (int i = d; i < n; i++)
            {
                int toAdd = Convert.ToInt32(expenditures[i]);
                double medium = getMedium(dPriorDays, d);

                if (toAdd >= 2 * medium)
                    count++;

                int toRemove = Convert.ToInt32(expenditures[start++]);
                dPriorDays[toRemove]--;
                dPriorDays[toAdd]++;
            }

            return count;
        }

        /*
         * 9:55pm 
         * 
         */
        private static double getMedium(int[] arr, int days)
        {
            int SIZE = 201;
            int sum = 0;

            bool isEven = days % 2 == 0;
            int[] stats = new int[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                sum += arr[i];
                stats[i] = sum;
            }

            if (!isEven)
            {
                return mediumByIncrement(stats, days / 2) * 1.0;
            }
            else
                return (mediumByIncrement(stats, days / 2 - 1)
                        + mediumByIncrement(stats, days / 2)) / 2.0;

        }

        /*
         * 10:07pm 
         */
        private static int mediumByIncrement(int[] stats, int lookup)
        {
            for (int i = 0; i < stats.Length; i++)
            {
                if (stats[i] > lookup)
                    return i;
            }

            return -1;
        }
    }
}
