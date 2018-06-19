using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhattan2
{
    /// <summary>
    /// https://www.hackerrank.com/contests/booking-womenintech/challenges/manhattan-2
    /// The idea is to start from top left node, always go right or down, and then track the 
    /// sum and max value; 
    /// Because the size of matrix is 100 * 100, queue will cause out-of-memory, use stack, DFS 
    /// search, try to use recursive solution if possible
    /// 200 depth at most, 100 + 100
    /// try to get some points first, and then get the idea - 2:35pm - 7pm 
    /// 7:53pm what is the possible reason to get wrong answer? 
    /// </summary>
    class Manhattan2
    {
        internal class Node
        {
            public long Sum { get; set; }
            public int MaxValue { get; set; }

            public long candies
            {
                get
                {
                    return Sum + SecondsLeft * MaxValue;
                }
            }

            public int SecondsLeft { get; set; }

            public int startRow { get; set; }
            public int startCol { get; set; }
            public int maximumCandies { get; set; }
            public int seconds { get; set; }
            public bool exraSecondsExisting { get; set; }

            // debug the code 
            public string trackNode { get; set; }
        }

        static void Main(string[] args)
        {
            ProcessInput();
            //RunTestcase(); 
        }

        public static void RunTestcase()
        {
            int rows = 5;
            int cols = 5;
            int seconds = 12;

            var matrix = new int[rows][];
            matrix[0] = new int[] { 2, 1, 1, 1, 1 };
            matrix[1] = new int[] { 2, 2, 1, 1, 1 };
            matrix[2] = new int[] { 1, 2, 1, 1, 1 };
            matrix[3] = new int[] { 2, 2, 1, 1, 3 };
            matrix[4] = new int[] { 2, 2, 2, 2, 2 };

            var node = new Node();
            node.maximumCandies = int.MinValue;
            node.startCol = 0;
            node.startRow = 0;
            node.seconds = seconds;
            node.exraSecondsExisting = seconds > rows - 1 + cols - 1;

            var maximumCandies = FindRouteFromLeftTopToBottomRight(matrix, node);
            // should be 29
            Console.WriteLine(maximumCandies);
        }


        public static void ProcessInput()
        {
            var numbers = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
            int rows = numbers[0];
            int cols = numbers[1];
            int seconds = numbers[2];

            var matrix = new int[rows][];
            for (int i = 0; i < rows; i++)
            {
                matrix[i] = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
            }

            if (seconds < rows - 1 + cols - 1)
            {
                Console.WriteLine("Too late");
                return;
            }

            var node = new Node();
            node.maximumCandies = int.MinValue;
            node.startCol = 0;
            node.startRow = 0;
            node.seconds = seconds;
            node.exraSecondsExisting = seconds > rows - 1 + cols - 1;

            var maximumCandies = FindRouteFromLeftTopToBottomRight(matrix, node);

            Console.WriteLine(maximumCandies);
        }

        /// <summary>
        /// Always go two directions
        /// down or right 
        /// recursive may run to error "stack overflow", using stack instead. 
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static long FindRouteFromLeftTopToBottomRight(
            int[][] matrix,
            Node node)
        {
            int rows = matrix.Length;
            int cols = matrix[0].Length;

            var timeToLive = node.seconds + 1;

            var dp = new int[rows, cols, timeToLive];

            // base case
            dp[0, 0, 0] = matrix[0][0];

            // 
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var rowDiff = rows - 1 - row;
                    var colDiff = cols - 1 - col;
                    var minimumToDestination = rowDiff + colDiff;

                    for (int second = 1; second < timeToLive; second++)
                    {
                        // three options, from left or top or same position
                        // default - no top choice
                        var options = new int[] { dp[row, col, second - 1] }; // same position only                       

                        if (row > 0)
                        {
                            options = new int[]{ dp[row - 1, col,     second - 1],  // top
                                                 dp[row,     col,     second - 1]}; // same position
                        }

                        if (col > 0)
                        {
                            options = new int[]{ dp[row,     col - 1, second - 1],  // left                                              
                                                 dp[row,     col,     second - 1]}; // same position
                        };

                        if (row > 0 && col > 0)
                        {
                            options = new int[]{ dp[row,     col - 1, second - 1],  // left
                                                 dp[row - 1, col,     second - 1],  // top
                                                 dp[row,     col,     second - 1]}; // same position
                        }

                        dp[row, col, second] = options.Max();

                        if (dp[row, col, second] > 0)
                        {
                            dp[row, col, second] = options.Max() + matrix[row][col];
                        }
                    }
                }
            }


            return dp[rows - 1, cols - 1, timeToLive - 1];
        }
    }
}
