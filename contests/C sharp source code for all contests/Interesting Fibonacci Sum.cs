using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingFibonacciSum
{
    class InterestingFibonacciSum_OutofMemory
    {
        /*
         * https://www.hackerrank.com/contests/walmart-codesprint-algo/challenges/fibonacci-sum-1
         * Oct. 29, 2016
         * Spent two hours to read problem statement
         * 11:30am - 1:30am
         * 
         * 7:12pm - start to write some code
         * Figure out things in detail
         * 
         * 9:12pm complete the coding
         * 9:12pm put into HackerRank to test the code 
         * 
         */
        static void Main(string[] args)
        {
            //testing3(); 
            process();
        }

        private static void testing1()
        {
            int n = 2;
            int[] arr = { 1, 1 };

            // int result = calculate(n, arr);
        }

        private static void testing2()
        {
            int n = 3;
            int[] arr = { 1, 2, 3 };

            //  int result = calculate(n, arr);
        }

        private static void testing3()
        {
            int q = 2;

            IList<Dictionary<long, int>> total = new List<Dictionary<long, int>>();
            long maxValue = long.MinValue;

            int n = 2;
            int[] arr = new int[n];

            arr = cToInt("1 1".Split(' '));

            calculate(n, arr, total, ref maxValue);

            int n2 = 3;
            int[] arr2 = new int[3];
            arr2 = cToInt("1 2 3".Split(' '));

            calculate(n2, arr2, total, ref maxValue);


            int[] res = fiboSmart2(total, maxValue);

            foreach (int no in res)
            {
                Console.WriteLine(no);
            }
        }

        /*
         * 9:33pm 
         * first, test the function is working 
         * ignore out-of-memory issue 
         */
        private static void process()
        {
            int q = Convert.ToInt32(Console.ReadLine());

            IList<Dictionary<long, int>> total = new List<Dictionary<long, int>>();
            long maxValue = long.MinValue;
            for (int i = 0; i < q; i++)
            {
                int n = Convert.ToInt32(Console.ReadLine());   // 10^5 upper limit, size of array
                int[] arr = new int[n];

                arr = cToInt(Console.ReadLine().Split(' '));

                calculate(n, arr, total, ref maxValue);
            }

            int[] res = fiboSmart2(total, maxValue);

            foreach (int no in res)
            {
                Console.WriteLine(no);
            }
        }

        /*
         * start: 7:15pm 
         * end: 7:19pm
         */
        private static int[] cToInt(string[] arr)
        {
            if (arr == null || arr.Length == 0)
                return new int[0];

            int n = arr.Length;
            int[] res = new int[n];

            for (int i = 0; i < n; i++)
            {
                res[i] = Convert.ToInt32(arr[i]);
            }

            return res;
        }

        /*
         * 12:49am 
         * work on the solution to solve timeout issue 
         */
        private static void calculate(
            int n,
            int[] arr,
            IList<Dictionary<long, int>> total,
            ref long maxValue)
        {
            long[] sum1 = sumC(n, arr);

            Dictionary<long, int> coll = new Dictionary<long, int>();

            for (int i = 0; i < n; i++)
                for (int j = i; j < n; j++)
                {
                    int L = i;
                    int R = j;

                    long AL2R = (L == R) ? arr[L] : (sum1[R] - sum1[L] + arr[L]);

                    if (AL2R > maxValue)
                        maxValue = AL2R;

                    if (coll.ContainsKey(AL2R))
                        coll[AL2R]++;
                    else
                        coll.Add(AL2R, 1);
                }

            total.Add(coll);
            return;
        }

        /*
         * 8:25pm 
         * To reduce the variation of calculation from O(n^2) to O(n)
         * sum(A) = sumC[R] - sumC[L]
         * 
         * 8:40pm 
         * L, R two variable, each is O(n), so LR pair will be O(n^2)
         * but, we use sumC to get the difference based on O(n) size array
         * 
         * 8:55pm 
         * cut the size to 10^9 + 7 - cannot do that! 
         * module calculation 
         */
        private static long[] sumC(int n, int[] arr)
        {
            long[] sum = new long[n];

            sum[0] = arr[0];
            for (int i = 1; i < n; i++)
            {
                sum[i] = (sum[i - 1] + arr[i]);
            }

            return sum;
        }

        /*
         * 12:52am 
         * work on a solution 
         * IList<Dictionary<int, int>> total
         */
        private static int[] fiboSmart2(
            IList<Dictionary<long, int>> total,
            long maxValue)
        {
            int q = total.Count;

            int SIZE = 1000000000 + 7;
            int module = SIZE;

            int tmp0 = 0;
            int tmp1 = 1;
            int[] count = new int[q];

            update(1, 1, total, count);

            int sum = 0;
            for (int i = 2; i <= maxValue; i++)
            {
                sum = (tmp0 + tmp1) % SIZE;

                update(i, sum, total, count);

                tmp0 = tmp1;
                tmp1 = sum;
            }

            return count;
        }

        /*
         * Oct. 30, 2016
         * 1:49am 
         * work on the algorithm
         */
        private static void update(
            int key,
            int fib,
            IList<Dictionary<long, int>> total,
            int[] count)
        {
            int SIZE = 1000000000 + 7;

            for (int i = 0; i < total.Count; i++)
            {
                Dictionary<long, int> item = total[i];
                if (item.ContainsKey(key))
                {
                    int no = item[key];
                    count[i] = (count[i] + fib * no) % SIZE;
                }
            }
        }
    }
}
