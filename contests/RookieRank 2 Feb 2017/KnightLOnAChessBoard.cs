using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightLOnAChessboard
{
    /*
     * problem statement:
     * https://www.hackerrank.com/contests/rookierank-2/challenges/knightl-on-chessboard
     * start time: 9:30am, 2/11/2017
     * end time: 12:11pm, 2/11/2017 
     */
    class Node
    {
        public Tuple<int, int> element { get; set; }
        public int steps { get; set; }

        public Node(Tuple<int, int> node, int value)
        {
            element = new Tuple<int, int>(node.Item1, node.Item2);
            steps = value;
        }
    }

    class KnightL
    {
        private int nSquaresHorizontal { get; set; }
        private int mSquaresVertically { get; set; }
        private int chessBoardSize { get; set; }

        /*
         * row from 1 to size - 1
         * col from 1 to size - 1
         * where size is from 5 to 25
         */
        public KnightL(int row, int col, int size)
        {
            nSquaresHorizontal = row;
            mSquaresVertically = col;
            chessBoardSize = size;
        }

        public static void Visit(
            int row,
            int col,
            int n,
            Queue<int> queue,
            bool[] visited)
        {
            if (row < 0 || col < 0 || row >= n || col >= n)
            {
                return;
            }

            int position = row * n + col;
            if (visited[position])
            {
                return;
            }

            visited[position] = true;
            queue.Enqueue(position);
        }

        public static int CalculateStepsFromLeftTopToBottomRight(
                KnightL knightL)
        {
            int n = knightL.chessBoardSize;
            int dx = knightL.nSquaresHorizontal;
            int dy = knightL.mSquaresVertically;
            bool[] visited = new bool[n * n];
            int steps = 0;

            var queue = new Queue<int>();

            visited[0] = true;
            queue.Enqueue(0);

            while (queue.Count > 0)
            {
                int count = queue.Count;

                for (int i = 0; i < count; i++)
                {
                    int position = queue.Dequeue();
                    int row = (position / n);
                    int col = (position % n);

                    if (row == n - 1 && col == n - 1)
                    {
                        // Found solution.
                        return steps;
                    }

                    Visit(row + dx, col + dy, n, queue, visited);
                    Visit(row + dx, col - dy, n, queue, visited);
                    Visit(row + dy, col + dx, n, queue, visited);
                    Visit(row + dy, col - dx, n, queue, visited);
                    Visit(row - dx, col + dy, n, queue, visited);
                    Visit(row - dx, col - dy, n, queue, visited);
                    Visit(row - dy, col + dx, n, queue, visited);
                    Visit(row - dy, col - dx, n, queue, visited);
                }

                steps++;
            }

            return -1;
        }
    }

    public class ChessBoardWithMinimumSteps
    {
        private int Size { get; set; } // code review: public -> private
        public int[][] Step { get; set; }

        public ChessBoardWithMinimumSteps(int value)
        {
            Size = value;
            Step = new int[Size][];
            for (int i = 0; i < Size; i++)
            {
                Step[i] = new int[Size];
            }
        }

        /*
         * Go over each row from 0 to n - 1, 
         * for each row, go over column from 0 to n - 1
         *
         */
        public void CalculateChessBoardMinimumSteps()
        {
            for (int rowIncrement = 1; rowIncrement < Size; rowIncrement++)
            {
                for (int colIncrement = 1; colIncrement < Size; colIncrement++)
                {
                    KnightL knightL = new KnightL(rowIncrement, colIncrement, Size);

                    Step[rowIncrement - 1][colIncrement - 1] = KnightL.CalculateStepsFromLeftTopToBottomRight(knightL);
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ProcessInput();
            //RunSampleTestcase(); 
        }

        public static void RunSampleTestcase()
        {
            ChessBoardWithMinimumSteps myChessBoard = new ChessBoardWithMinimumSteps(5);

            myChessBoard.CalculateChessBoardMinimumSteps();

            int[][] steps = myChessBoard.Step;

            for (int i = 0; i < steps.Length - 1; i++)
            {
                StringBuilder concatented = new StringBuilder();
                for (int j = 0; j < steps[0].Length - 1; j++)
                {
                    concatented.Append(steps[i][j] + " ");
                }

                Console.WriteLine(concatented.ToString());
            }
        }

        public static void ProcessInput()
        {
            int size = Convert.ToInt32(Console.ReadLine());

            ChessBoardWithMinimumSteps myChessBoard = new ChessBoardWithMinimumSteps(size);

            myChessBoard.CalculateChessBoardMinimumSteps();

            int[][] steps = myChessBoard.Step;

            for (int i = 0; i < steps.Length - 1; i++)
            {
                StringBuilder concatented = new StringBuilder();
                for (int j = 0; j < steps[0].Length - 1; j++)
                {
                    concatented.Append(steps[i][j] + " ");
                }

                Console.WriteLine(concatented.ToString());
            }
        }
    }
}
