using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitManhattan
{
    /// <summary>
    /// 12:31pm work on brute force solution first 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            GetMinimumIndex();
            //RunTestcase();                      
        }

        public static void RunTestcase()
        {
            var landmarkLookup = new HashSet<string>();
            var hotelLookup = new HashSet<string>();

            var hotelsByOrder = new List<string>();

            landmarkLookup.Add(encode(1, 1));
            landmarkLookup.Add(encode(2, 2));

            hotelsByOrder.Add(encode(4, 4));
            hotelsByOrder.Add(encode(1, 2));

            long minimumDistance = long.MaxValue;
            long minimumIndex = -1;

            int index = 1;
            foreach (var key in hotelsByOrder)
            {
                var positions = decode(key);
                var currentX = positions[0];
                var currentY = positions[1];

                long currentSum = 0;

                foreach (var mark in landmarkLookup)
                {
                    var destination = decode(mark);
                    var destX = destination[0];
                    var destY = destination[1];

                    currentSum += Math.Abs(currentX - destX);
                    currentSum += Math.Abs(currentY - destY);
                }

                bool findShortOne = currentSum < minimumDistance;
                minimumDistance = findShortOne ? currentSum : minimumDistance;
                minimumIndex = findShortOne ? index : minimumIndex;

                index++;
            }

            Console.WriteLine(minimumIndex);
        }

        public static void GetMinimumIndex()
        {
            var numbers = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
            int rows = numbers[0];
            int cols = numbers[1];
            int landmarks = numbers[2];
            int hotels = numbers[3];

            var landmarkLookup = new HashSet<string>();
            var hotelLookup = new HashSet<string>();

            var hotelsByOrder = new List<string>();

            for (int i = 0; i < landmarks; i++)
            {
                var positions = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                var x = positions[0];
                var y = positions[1];

                landmarkLookup.Add(encode(x, y));
            }

            for (int i = 0; i < hotels; i++)
            {
                var positions = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                var x = positions[0];
                var y = positions[1];

                var key = encode(x, y);
                hotelLookup.Add(key);

                hotelsByOrder.Add(key);
            }

            long result = -1;

            result = FindMinimumIndex_FromHotelToLandmark(hotelsByOrder, landmarkLookup);

            Console.WriteLine(result);
        }

        /// <summary>
        /// time complexity 
        /// hotels - H
        /// landmarks - T
        /// time complexity is O(HT), go over each hotel, look up distance to the each landmarks
        /// </summary>
        /// <param name="hotelsByOrder"></param>
        /// <param name="landmarkLookup"></param>
        /// <returns></returns>
        public static long FindMinimumIndex_FromHotelToLandmark(List<string> hotelsByOrder, HashSet<string> landmarkLookup)
        {
            long minimumDistance = long.MaxValue;
            long minimumIndex = -1;

            int index = 1;
            foreach (var key in hotelsByOrder)
            {
                var positions = decode(key);
                var currentX = positions[0];
                var currentY = positions[1];

                long currentSum = 0;

                foreach (var mark in landmarkLookup)
                {
                    var destination = decode(mark);
                    var destX = destination[0];
                    var destY = destination[1];

                    currentSum += Math.Abs(currentX - destX);
                    currentSum += Math.Abs(currentY - destY);
                }

                bool findShortOne = currentSum < minimumDistance;
                minimumDistance = findShortOne ? currentSum : minimumDistance;
                minimumIndex = findShortOne ? index : minimumIndex;

                index++;
            }

            return minimumIndex;
        }

        private static string encode(int row, int col)
        {
            return row + "," + col;
        }

        private static int[] decode(string key)
        {
            var positions = Array.ConvertAll(key.Split(','), int.Parse);

            return positions;
        }
    }
}
