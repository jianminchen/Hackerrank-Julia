using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace collidingCircles
{
    class collidingCircles
    {
        static void Main(string[] args)
        {
            ProcessInput();
            //RunTestcase();
            //RunTestcase2(); 
        }

        public static void RunTestcase()
        {
            List<int> balls = new List<int>(new int[] { 1, 2, 3 });

            double expectedTotalArea = 0;

            double orginalTotal = CalculateTotalArea(balls);
            double adjustment = 0;
            CalculateExpectedTotalAreaAfterKSeconds(balls, 1, 1, ref expectedTotalArea, adjustment, orginalTotal);
        }


        public static void RunTestcase2()
        {
            List<int> balls = new List<int>(new int[] { 1, 2, 3 });

            double expectedTotalArea = 0;
            double orginalTotal = CalculateTotalArea(balls);
            double adjustment = 0;
            CalculateExpectedTotalAreaAfterKSeconds(balls, 2, 1, ref expectedTotalArea, adjustment, orginalTotal);
        }

        public static void ProcessInput()
        {
            var firstRow = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
            int numbers = firstRow[0];
            int kSeconds = firstRow[1];

            var balls = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

            double expectedTotalArea = 0;
            double orginalTotal = CalculateTotalArea(balls);
            double adjustment = 0;
            CalculateExpectedTotalAreaAfterKSeconds(balls.ToList(), kSeconds, 1, ref expectedTotalArea, adjustment, orginalTotal);

            Console.WriteLine(expectedTotalArea);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="balls"></param>
        /// <param name="kseconds"></param>
        /// <returns></returns>
        public static void CalculateExpectedTotalAreaAfterKSeconds(
            List<int> balls,
            int kseconds,
            double probability,
            ref double expectedTotalArea,
            double adjustment,
            double orginalTotal)
        {
            if (kseconds == 0 || (balls.Count == 1))
            {
                expectedTotalArea += (orginalTotal + adjustment) * probability;
                return;
            }

            int length = balls.Count;
            int cases = length > 1 ? (length * (length - 1) / 2) : 1;

            double nextProbablity = probability / cases;  // remove from double loops
            double twoPI = Math.PI * 2;  // remove from double loop

            if (kseconds == 1)
            {
                expectedTotalArea += (orginalTotal + adjustment) * probability;
                expectedTotalArea += calculateDifference(balls) * nextProbablity;
                return;
            }

            for (int i = 0; i < length - 1; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    var first = balls[i];
                    var second = balls[j];
                    var collide = first + second;

                    //List<int> copy = new List<int>(balls);

                    // first update, and then remove, avoid out-of-index error
                    //copy[i] = collide;
                    //copy.RemoveRange(j, 1);                    
                    balls[i] = collide;
                    balls.RemoveRange(j, 1);

                    double currentAdjustment = twoPI * first * second;

                    CalculateExpectedTotalAreaAfterKSeconds(
                        balls,
                        kseconds - 1,
                        nextProbablity,
                        ref expectedTotalArea,
                        currentAdjustment + adjustment,
                        orginalTotal
                        );

                    balls[i] = first;
                    balls.Insert(j, second);
                }
            }

            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="balls"></param>
        /// <returns></returns>
        private static double CalculateTotalArea(IList<int> balls)
        {
            double totalArea = 0;

            for (int i = 0; i < balls.Count; i++)
            {
                totalArea += balls[i] * balls[i];
            }

            return Math.PI * totalArea;
        }

        /// <summary>
        /// lower O(n*n) to O(n) calculation 
        /// intead of ai * aj, 0 <= i, j <  n, O(nn) calculation
        /// use difference of sum to reduce to O(n)
        /// </summary>
        /// <param name="balls"></param>
        /// <returns></returns>
        private static double calculateDifference(IList<int> balls)
        {
            double sum = 0;
            double squareSum = 0;
            foreach (var item in balls)
            {
                sum += item;
                squareSum += item * item;
            }

            return Math.PI * (sum * sum - squareSum);
        }
    }
}
