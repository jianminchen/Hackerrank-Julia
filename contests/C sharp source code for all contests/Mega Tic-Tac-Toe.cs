using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaTicTacToe
{
    public class Node
    {
        public int[] right;  // horizontal 
        public int[] down;    // vertical 
        public int[] cross;     // diagonal

        public Node(int m)
        {
            this.right = new int[m];
            this.down = new int[m];
            this.cross = new int[m];
        }
    }

    class MegaTicTacToe
    {
        /*
         * https://www.hackerrank.com/contests/ncr-codesprint/challenges/mega-tic-tac-toe
         * 
         * 3:03pm 
         * using Dynamic programming 
         * space concern, 512MB, 
         * 
         * Try to declare the array first
         * 
         * if there is memory issue, only keep the two rows - previous row/ current row 
         * 
         * start: 3:08pm 
         * end: 4:05pm 
         * start to run on HackerRank 
         * 6:24pm - pass sample test case 
         * 6:24pm - submit the code, pass all test cases
         * 
         * Bugs in writing:
         * 1. copyNode function - ref issue
         * 2. dynamic programming recurrence formula 
         *    line 182 - 184 
         *    
         * 
         * 
         * 
         */
        static void Main(string[] args)
        {
            process();
            //testCase1();
            //testCase3(); 
            //testCase4(); 
        }

        private static void process()
        {
            int g = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < g; i++)
            {
                int[] arr = ToInt(Console.ReadLine().Split(' '));
                int n = arr[0], m = arr[1], k = arr[2];
                string[] data = new string[n];
                for (int j = 0; j < n; j++)
                    data[j] = Console.ReadLine();

                Console.WriteLine(dpAlgo(n, m, k, data));
            }
        }

        private static void testCase1()
        {
            int n = 3;
            int m = 3;
            int k = 3;
            string[] data = new string[3] { "XOX", "XOX", "XXX" };

            dpAlgo(n, m, k, data);
        }

        private static void testCase3()
        {
            int n = 3;
            int m = 3;
            int k = 3;
            string[] data = new string[3] { "O-X", "XOO", "XOO" };

            dpAlgo(n, m, k, data);
        }

        private static void testCase4()
        {
            int n = 3;
            int m = 3;
            int k = 3;
            string[] data = new string[3] { "X-X", "O-O", "X-X" };

            dpAlgo(n, m, k, data);
        }


        /*
         * start: 3:13pm
         */
        private static string dpAlgo(int n, int m, int k, string[] data)
        {
            string[] message = new string[3] { "WIN", "LOSE", "NONE" };

            Node previousX = new Node(m);
            Node previousO = new Node(m);
            Node currentX = new Node(m);
            Node currentO = new Node(m);

            bool upToK_O = false;
            bool upToK_X = false;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    char runner = data[i][j];

                    recurrence(currentO, previousO, runner, i, j, 'O');
                    recurrence(currentX, previousX, runner, i, j, 'X');

                    if (currentO.right[j] >= k ||
                        currentO.down[j] >= k ||
                        currentO.cross[j] >= k)
                        upToK_O = true;

                    if (currentX.right[j] >= k ||
                        currentX.down[j] >= k ||
                        currentX.cross[j] >= k)
                        upToK_X = true;
                }

                copyNode(ref previousO, currentO, m);
                copyNode(ref previousX, currentX, m);
            }

            if (upToK_O && upToK_X)
                return message[2];
            else if (upToK_O)
                return message[0];
            else if (upToK_X)
                return message[1];
            else
                return message[2];
        }

        private static void copyNode(ref Node copyTo, Node copyFrom, int len)
        {
            for (int i = 0; i < len; i++)
            {
                copyTo.cross[i] = copyFrom.cross[i];
                copyTo.down[i] = copyFrom.down[i];
                copyTo.right[i] = copyFrom.right[i];
            }
        }

        /*
         * start: 3:51pm
         */
        private static void recurrence(
            Node cur,
            Node prev,
            char runner,
            int row,
            int col,
            char key
            )
        {
            bool isZero = false;
            if ((!isX(runner) && !isO(runner)) ||
                 (isX(key) && isO(runner)) ||
                 (isO(key) && isX(runner)))
                isZero = true;

            cur.right[col] = (isZero) ? 0 : ((col >= 1) ? (cur.right[col - 1] + 1) : 1);
            cur.down[col] = (isZero) ? 0 : ((row >= 1) ? (prev.down[col] + 1) : 1);
            cur.cross[col] = (isZero) ? 0 : ((row >= 1 && col >= 1) ? (prev.cross[col - 1] + 1) : 1);
        }

        private static bool isX(char c)
        {
            return ((c - 'X') == 0);
        }

        private static bool isO(char c)
        {
            return ((c - 'O') == 0);
        }

        private static int[] ToInt(string[] arr)
        {
            int len = arr.Length;
            int[] res = new int[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = Convert.ToInt32(arr[i]);
            }

            return res;
        }
    }
}
