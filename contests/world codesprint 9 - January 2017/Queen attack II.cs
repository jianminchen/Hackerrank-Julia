using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueenAttack2
{
    class QueenAttack2
    {
        internal class Directions
        {
            public static int[] directions_row = new int[] { -1, -1, 0, 1, 1, 1, 0, -1 };
            public static int[] directions_col = new int[] { 0, -1, -1, -1, 0, 1, 1, 1 };

            public int rows;
            public int[] minimumDistance;
            public bool[] minimumDistanceExist;

            public Tuple<int, int> queen { set; get; }

            public Directions(int row, int col, int size)
            {
                queen = new Tuple<int, int>(row, col);

                minimumDistance = new int[8];
                minimumDistanceExist = new bool[8];
                rows = size;
            }

            /*
             * left, right, up, down, or the four diagonals
             * 8 directions - clockwise, starting from left. 
             */
            public bool IsOneOfEightDirections(
                int obstacle_row,
                int obstacle_col)
            {
                return (obstacle_col == queen.Item2 ||
                        obstacle_row == queen.Item1 ||
                    Math.Abs(obstacle_row - queen.Item1) == Math.Abs(obstacle_col - queen.Item2)
                );
            }

            /*
             * Go over 100000 obstacles, and find those 8 minimum one if there is. 
             * 
             */
            public void Keep8MininumObstacles(Tuple<int, int>[] obstacles)
            {
                for (int i = 0; i < obstacles.Length; i++)
                {
                    Tuple<int, int> obstacle = obstacles[i];

                    if (!IsOneOfEightDirections(obstacle.Item1, obstacle.Item2))
                    {
                        continue;
                    }

                    UpdateOneOfDirection(obstacle);
                }
            }

            /*
             * 
             */
            private void UpdateOneOfDirection(Tuple<int, int> obstacle)
            {
                bool isSameRow = obstacle.Item1 == queen.Item1;
                bool isSameCol = obstacle.Item2 == queen.Item2;

                bool isObstacleLeft = obstacle.Item2 < queen.Item2;
                bool isObstacleRight = obstacle.Item2 > queen.Item2;

                bool isObstacleUp = obstacle.Item1 > queen.Item1;
                bool isObstacleDown = obstacle.Item1 < queen.Item1;

                bool isDirectionLeft = isSameRow && isObstacleLeft;
                bool isDirectionRight = isSameRow && isObstacleRight;

                bool isDirectionUp = isSameCol && isObstacleUp;
                bool isDirectionDown = isSameCol && isObstacleDown;

                bool isDirectionUpLeft = isObstacleUp && isObstacleLeft;
                bool isDirectionUpRight = isObstacleUp && isObstacleRight;

                bool isDirectionDownLeft = isObstacleDown && isObstacleLeft;
                bool isDirectionDownRight = isObstacleDown && isObstacleRight;

                // verify left, up, right, down 
                // direction left:
                if (isDirectionLeft)
                {
                    int current = Math.Abs(queen.Item2 - obstacle.Item2);
                    UpdateMinimumDistanceInfo(0, current);
                }

                if (isDirectionUp)
                {
                    int current = Math.Abs(queen.Item1 - obstacle.Item1);
                    UpdateMinimumDistanceInfo(2, current);
                }

                if (isDirectionRight)
                {
                    int current = Math.Abs(queen.Item2 - obstacle.Item2);
                    UpdateMinimumDistanceInfo(4, current);
                }

                if (isDirectionDown)
                {
                    int current = Math.Abs(queen.Item1 - obstacle.Item1);
                    UpdateMinimumDistanceInfo(6, current);
                }

                // verify 4 cross directions
                if (isDirectionUpLeft)
                {
                    int current = Math.Abs(queen.Item1 - obstacle.Item1);
                    UpdateMinimumDistanceInfo(1, current);
                }

                if (isDirectionUpRight)
                {
                    int current = Math.Abs(queen.Item1 - obstacle.Item1);
                    UpdateMinimumDistanceInfo(3, current);
                }

                if (isDirectionDownRight)
                {
                    int current = Math.Abs(queen.Item1 - obstacle.Item1);
                    UpdateMinimumDistanceInfo(5, current);
                }

                if (isDirectionDownLeft)
                {
                    int current = Math.Abs(queen.Item1 - obstacle.Item1);
                    UpdateMinimumDistanceInfo(7, current);
                }
            }

            /*
             * 8 directions, use directions_row, directions_col as instruction 
             */
            private void UpdateMinimumDistanceInfo(int direction, int current)
            {
                if (!minimumDistanceExist[direction])
                {
                    minimumDistanceExist[direction] = true;
                    minimumDistance[direction] = current;
                }
                else
                {
                    if (current < minimumDistance[direction])
                    {
                        minimumDistance[direction] = current;
                    }
                }
            }

            public int CalculateTotal()
            {
                int total = 0;
                for (int i = 0; i < 8; i++)
                {
                    if (minimumDistanceExist[i])
                    {
                        total += minimumDistance[i] - 1;
                        continue;
                    }

                    if (i == 0)
                    {
                        // direction left 
                        total += queen.Item2;
                    }

                    if (i == 2)
                    {
                        // direction up
                        total += rows - queen.Item1 - 1;
                    }

                    if (i == 4)
                    {
                        // direction right
                        total += rows - queen.Item2 - 1;
                    }

                    if (i == 6)
                    {
                        // direction down
                        total += queen.Item1;
                    }

                    if (i == 1)
                    {
                        // direction up-left
                        total += CalculateUpLeftCount();
                    }

                    if (i == 3)
                    {
                        // direction up-right
                        total += CalculateUpRightCount();
                    }

                    if (i == 5)
                    {
                        // direction down-right
                        total += CalculateDownRightCount();
                    }

                    if (i == 7)
                    {
                        // direction down-left
                        total += CalculateDownLeftCount();
                    }
                }

                return total;
            }

            private int CalculateUpLeftCount()
            {
                int row = queen.Item1;
                int col = queen.Item2;
                int count = 0;
                while (row < rows - 1 && col > 0)
                {
                    row++;
                    col--;
                    count++;
                }

                return count;
            }

            private int CalculateUpRightCount()
            {
                int row = queen.Item1;
                int col = queen.Item2;
                int count = 0;
                while (row < rows - 1 && col < rows - 1)
                {
                    row++;
                    col++;

                    count++;
                }

                return count;
            }

            private int CalculateDownRightCount()
            {
                int row = queen.Item1;
                int col = queen.Item2;
                int count = 0;
                while (row > 0 && col < rows - 1)
                {
                    row--;
                    col++;

                    count++;
                }

                return count;
            }

            private int CalculateDownLeftCount()
            {
                int row = queen.Item1;
                int col = queen.Item2;
                int count = 0;
                while (row > 0 && col > 0)
                {
                    row--;
                    col--;

                    count++;
                }

                return count;
            }
        }

        static void Main(string[] args)
        {
            ProcessInput();

            //RunSampleTestcase(); 
            //RunSampleTestcase2(); 
        }

        public static void RunSampleTestcase()
        {
            Directions directions = new Directions(3, 3, 4);
            var obstacles = new Tuple<int, int>[0];

            directions.Keep8MininumObstacles(obstacles);
            Console.WriteLine(directions.CalculateTotal());
        }

        /*
         * 5 3
         * 4 3
         * 5 5
         * 4 2
         * 2 3
         * 
         */
        public static void RunSampleTestcase2()
        {
            Directions directions = new Directions(3, 2, 5);
            var obstacles = new Tuple<int, int>[]{
                new Tuple<int,int>(4,4), 
                new Tuple<int,int>(3,1), 
                new Tuple<int,int>(1,2)
            };

            directions.Keep8MininumObstacles(obstacles);
            Console.WriteLine(directions.CalculateTotal());
        }

        public static void ProcessInput()
        {
            int[] data = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

            int rows = data[0];
            int countObstacles = data[1];

            int[] queen = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
            var obstacles = new Tuple<int, int>[countObstacles];

            for (int i = 0; i < countObstacles; i++)
            {
                int[] obstacle = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                obstacles[i] = new Tuple<int, int>(obstacle[0] - 1, obstacle[1] - 1);
            }

            Directions directions = new Directions(queen[0] - 1, queen[1] - 1, rows);
            directions.Keep8MininumObstacles(obstacles);
            Console.WriteLine(directions.CalculateTotal());
        }

        /*
         * Process all obstacles one by one, determined if the obstacle is on 8 direction. 
         * If it is 8 directions, then update minimum distance one. 
         * The total obstacles are up to 100000, we only need to keep up to 8 obstacles
         */
        public static IList<Tuple<int, int>> ProcessObstaclesKeep8withMinimuDistance(int n, int x, int y, Tuple<int, int>[] obstacles)
        {
            IList<Tuple<int, int>> minimumOne = new List<Tuple<int, int>>();

            return minimumOne;
        }
    }
}
