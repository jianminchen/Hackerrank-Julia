using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridlandMetro
{
    class Program
    {
        /*
        * start: 6:54pm 
        * http://stackoverflow.com/questions/14336416/using-icomparer-for-sorting
        */
        public class MyComparer : IComparer<Tuple<int, int>>
        {
            public int Compare(Tuple<int, int> x, Tuple<int, int> y)
            {
                return (x.Item1 - y.Item1);
            }
        }

        /*
         * start: 3:00pm 
         * read problem statement
         * 
         * start to code: 3:21pm 
         * write down the idea:
         * declare a two dimension array 10^9 x 10^9, bit
         * mark that it is reserved for track, 
         * avoid counting more than once. 
         * 100MB space - 10^18 bit
         * Let us give it try using the above idea
         * take 20 minutes break 
         * 
         * start to code: 3:55pm 
         * end of codeing: 4:11pm
         * 
         * start: 4:23pm
         * out-of-memory for array bool[size,size], 
         * so try to use bool[k,size]
         * 4:54 - out of memory too
         * 
         * start: 5:34pm 
         * Use Ditionary - still out-of-memory
         * 
         * 5:57 start to bug fix
         * use BitArray to replace bool array - out-of-space bug is gone
         * But timeout issue needs to be fixed
         * 
         * 
         * 7:23pm start to fix the wrong answer issue
         *  4 4 3
            2 2 3
            3 1 4
         * 
         *  The answer should be 16, my answer is 9
         *  
         * 8:55pm 4.17 out of 25 
         * start to review the code, and then figure out the issues 
         * 
         */
        static void Main(string[] args)
        {
            //test3(); 

            program();
        }

        /*
         * 9:30pm 
         * Fix the bug - 
         * IndexOf() find occurrence after the first instance
         */
        private static void test3()
        {
            int nR = 10;
            int mC = 10;
            int k = 5;

            int[] map = new int[k];
            IList<int> rows = new List<int>();
            IList<Tuple<int, int>> tuples = new List<Tuple<int, int>>();

            int[,] data = new int[5, 3]{
                {2,8, 9},
                {2,2,3},
                {1,1,4}, 
                {2,1,4},
                {2,6,7}
            };

            for (int i = 0; i < k; i++)
            {
                int row = data[i, 0];
                int startC = data[i, 1];
                int endC = data[i, 2];

                rows.Add(row);
                tuples.Add(new Tuple<int, int>(startC, endC));
            }

            Console.WriteLine(calculate(rows, tuples, nR, mC));
        }

        private static void test2()
        {
            int nR = 4;
            int mC = 4;
            int k = 3;

            int[] map = new int[k];
            IList<int> rows = new List<int>();
            IList<Tuple<int, int>> tuples = new List<Tuple<int, int>>();

            int[,] data = new int[3, 3]{
                {2,2,3},
                {2,1,4}, 
                {2,1,1}
            };

            for (int i = 0; i < k; i++)
            {
                int row = data[i, 0];
                int startC = data[i, 1];
                int endC = data[i, 2];

                rows.Add(row);
                tuples.Add(new Tuple<int, int>(startC, endC));
            }

            Console.WriteLine(calculate(rows, tuples, nR, mC));
        }

        private static void test()
        {
            int nR = 4;
            int mC = 4;
            int k = 3;

            int[] map = new int[k];
            IList<int> rows = new List<int>();
            IList<Tuple<int, int>> tuples = new List<Tuple<int, int>>();

            int[,] data = new int[3, 3]{
                {2,2,3},
                {3,1,4}, 
                {4,4,4}
            };

            for (int i = 0; i < k; i++)
            {
                int row = data[i, 0];
                int startC = data[i, 1];
                int endC = data[i, 2];

                rows.Add(row);
                tuples.Add(new Tuple<int, int>(startC, endC));
            }

            Console.WriteLine(calculate(rows, tuples, nR, mC));
        }

        private static void program()
        {
            string[] numA = Console.ReadLine().Split(' ');
            int nR = Convert.ToInt32(numA[0]);
            int mC = Convert.ToInt32(numA[1]);

            int k = Convert.ToInt32(numA[2]);

            int[] map = new int[k];
            IList<int> rows = new List<int>();
            IList<Tuple<int, int>> tuples = new List<Tuple<int, int>>();

            for (int i = 0; i < k; i++)
            {
                string[] arr = Console.ReadLine().Split(' ');

                int row = Convert.ToInt32(arr[0]);
                int startC = Convert.ToInt32(arr[1]);
                int endC = Convert.ToInt32(arr[2]);

                rows.Add(row);
                tuples.Add(new Tuple<int, int>(startC, endC));
            }

            Console.WriteLine(calculate(rows, tuples, nR, mC));
        }

        /*
         * start: 5:11pm 
         * start to work on the function 
         * end: 5:28pm 
         * Walk through the code
         */
        private static string calculate(
            IList<int> rows,
            IList<Tuple<int, int>> tuples,
            int nR,
            int mC
            )
        {
            int[] sortedRows = rows.ToArray();

            Array.Sort(sortedRows);

            int prev = -1;
            long sum = 0;

            IList<Tuple<int, int>> intervals = new List<Tuple<int, int>>();

            int count = 0;
            int runnerPrev = 0;
            foreach (int row in sortedRows)
            {
                // int runner = Array.IndexOf(rows.ToArray(), row) + ((row == prev) ? count : 0);  // bug: score 4.17 out of 16                
                int runner = Array.IndexOf(rows.ToArray(), row);
                if (row == prev)
                {
                    runner = Array.IndexOf(rows.ToArray(), row, runnerPrev + 1);
                };

                Tuple<int, int> colR = tuples[runner];
                int start = colR.Item1;
                int end = colR.Item2;

                if (prev == -1 ||
                    row != prev)
                {
                    if (prev != -1)
                    {
                        sum += increment(intervals);
                        intervals.Clear();
                        count = 0;
                    }

                    intervals.Add(new Tuple<int, int>(start, end));
                    count++;
                    runnerPrev = runner;
                }
                else
                {
                    intervals.Add(new Tuple<int, int>(start, end));
                    count++;
                    runnerPrev = runner;
                }

                prev = row; // bug fix at 7:40pm
            }

            // edge case
            if (intervals.Count > 0)
                sum += increment(intervals);

            long mul = (long)nR;
            mul *= (long)mC;

            return ((long)(nR * mC) - sum).ToString();
        }

        /*
         * start: 6:38pm 
         * 
         * Need to write efficient interval algorithm
         * No time out
         * source code reference:
         * 1. http://xiaoyaoworm.com/blog/2016/06/27/%E6%96%B0leetcode56-merge-intervals/
         * 2. http://juliachencoding.blogspot.ca/2016/07/leetcode-56-merge-intervals.html
         * 
         * end: 7:07 
         * walk through the code
         */
        private static long increment(IList<Tuple<int, int>> intervals)
        {
            Tuple<int, int>[] sorted = intervals.ToArray();
            IComparer<Tuple<int, int>> myComparer = new MyComparer();
            Array.Sort(sorted, myComparer);
            //Array.Reverse(arr); 

            Tuple<int, int> curr = sorted[0];
            long sum = 0;
            for (int i = 1; i < sorted.Length; i++)
            {
                Tuple<int, int> runner = sorted[i];

                if (curr.Item2 < runner.Item1)
                {
                    sum += curr.Item2 - curr.Item1 + 1;
                    curr = runner;
                }

                int start = curr.Item1;
                int end = Math.Max(curr.Item2, runner.Item2);

                curr = new Tuple<int, int>(start, end);
            }

            // edge case: 
            sum += curr.Item2 - curr.Item1 + 1;

            return sum;
        }
    }


}
