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

            Stack<Node> stack = new Stack<Node>();
            stack.Push(node);

            long maximumCandies = long.MinValue;

            // memorizatio to save the time
            var lookupExisting = new Dictionary<string, HashSet<Node>>();
            bool extraSecondExisting = node.exraSecondsExisting;

            while (stack.Count > 0)
            {
                Node visit = stack.Pop();

                int startRow = visit.startRow;
                int startCol = visit.startCol;
                int seconds = visit.seconds;

                // duplicate work - skip if need 
                if (lookupAndCheckDuplicated(visit, rows, cols, lookupExisting, extraSecondExisting))
                {
                    continue;
                }

                // Reach the last node 
                bool lastNodeReached = isLastNode(startRow, startCol, rows, cols);
                if (lastNodeReached)
                {
                    updateMaximumCandies(matrix, visit, ref maximumCandies);
                    continue;
                }

                if (seconds <= 0)
                {
                    continue;
                }

                // go to right
                if (startCol < cols - 1)
                {
                    visitRightOrDownNeighbors(matrix, visit, stack, 0, 1);
                }

                // go down
                if (startRow < rows - 1)
                {
                    visitRightOrDownNeighbors(matrix, visit, stack, 1, 0);
                }
            }

            return maximumCandies;
        }

        /// <summary>
        /// go right or go down 
        /// </summary>
        /// <param name="matrix"></param>       
        /// <param name="visit"></param>        
        /// <param name="stack"></param>
        /// <param name="incrementX"></param>
        /// <param name="incrementY"></param>
        private static void visitRightOrDownNeighbors(int[][] matrix, Node visit, Stack<Node> stack, int incrementX, int incrementY)
        {
            int startRow = visit.startRow;
            int startCol = visit.startCol;
            int seconds = visit.seconds;

            var current = matrix[startRow][startCol];
            var currentMax = visit.MaxValue;

            var nextNode = new Node();

            nextNode.MaxValue = (current > currentMax) ? current : currentMax;
            nextNode.seconds = seconds - 1;
            nextNode.startRow = startRow + incrementX;
            nextNode.startCol = startCol + incrementY;
            nextNode.Sum = visit.Sum + current;

            stack.Push(nextNode);

            // debug code
            //var currentKey = encode(startRow, startCol);
            //var trackMessage = getTrackMessage(currentKey);
            //nextNode.trackNode = (visit.trackNode == null) ? trackMessage : visit.trackNode + trackMessage;
        }

        /// <summary>
        /// Make the code clear to test 
        /// </summary>
        /// <param name="matrix"></param>       
        /// <param name="visit"></param>        
        /// <param name="maximumCandies"></param>
        private static void updateMaximumCandies(int[][] matrix, Node visit, ref long maximumCandies)
        {
            int startRow = visit.startRow;
            int startCol = visit.startCol;
            int seconds = visit.seconds;

            var current = matrix[startRow][startCol];
            var currentMax = visit.MaxValue;

            long sum = visit.Sum + current;
            long secondsLeft = seconds;
            int maxValue = (current > currentMax) ? current : currentMax;

            // calculate candies - if there are extra time, maxValue will be collected multiple times - secondsLeft
            long currentCandies = sum + secondsLeft * maxValue;

            maximumCandies = (currentCandies > maximumCandies) ? currentCandies : maximumCandies;
        }

        /// <summary>
        /// too complicate! need to make it simple
        /// the logic looks too complicated. 
        /// </summary>
        /// <param name="visit"></param>
        /// <param name="startRow"></param>
        /// <param name="startCol"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="lookupExisting"></param>
        /// <param name="extraSecondExisting"></param>
        /// <returns></returns>
        private static bool lookupAndCheckDuplicated(
            Node visit,
            int rows,
            int cols,
            Dictionary<string, HashSet<Node>> lookupExisting,
            bool extraSecondExisting)
        {
            int startRow = visit.startRow;
            int startCol = visit.startCol;

            var currentKey = encode(startRow, startCol);

            if (isLastNode(startRow, startCol, rows, cols))
            {
                return false;
            }

            if (lookupExisting.ContainsKey(currentKey))
            {
                // compare to existing one, if it is not better then stop                
                bool skipCurrent = skipChecking(lookupExisting, currentKey, visit, extraSecondExisting);
                if (skipCurrent)
                {
                    return true;
                }
                else if (replaceCurrent(lookupExisting, currentKey, visit, extraSecondExisting))
                {
                    var set = new HashSet<Node>();
                    set.Add(visit);

                    lookupExisting[currentKey] = set;
                }
                else
                {
                    var existing = lookupExisting[currentKey];
                    existing.Add(visit);
                    lookupExisting[currentKey] = existing;
                }
            }
            else
            {
                var set = new HashSet<Node>();
                set.Add(visit);

                lookupExisting.Add(currentKey, set);
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="startCol"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        private static bool isNotLastNode(int startRow, int startCol, int rows, int cols)
        {
            return startRow != (rows - 1) || startCol != (cols - 1);
        }

        private static string getTrackMessage(string key)
        {
            return "[" + key + "]";
        }

        /// <summary>
        /// Need to figure out the way to save time 
        /// </summary>
        /// <param name="lookupExisting"></param>
        /// <param name="currentKey"></param>
        /// <param name="visit"></param>
        /// <returns></returns>
        private static bool skipChecking(Dictionary<string, HashSet<Node>> lookupExisting, string currentKey, Node visit, bool extraSecondsExisting)
        {
            var found = lookupExisting[currentKey];

            foreach (var item in found)
            {
                if (extraSecondsExisting && (item.Sum >= visit.Sum && item.MaxValue >= visit.MaxValue))
                {
                    return true;
                }

                if (!extraSecondsExisting && item.Sum >= visit.Sum)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookupExisting"></param>
        /// <param name="currentKey"></param>
        /// <param name="visit"></param>
        /// <param name="extraSecondsExisting"></param>
        /// <returns></returns>
        private static bool replaceCurrent(Dictionary<string, HashSet<Node>> lookupExisting, string currentKey, Node visit, bool extraSecondsExisting)
        {
            var found = lookupExisting[currentKey];

            foreach (var item in found)
            {
                if (extraSecondsExisting)
                {
                    if (visit.Sum > item.Sum && visit.MaxValue > item.MaxValue)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (!extraSecondsExisting)
                {
                    if (visit.Sum > item.Sum)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static string encode(int row, int col)
        {
            return row + "-" + col;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="startCol"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        private static bool isLastNode(int startRow, int startCol, int rows, int cols)
        {
            return startRow == (rows - 1) && startCol == (cols - 1);
        }
    }
}
