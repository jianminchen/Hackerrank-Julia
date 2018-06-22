using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscciFlower
{
    /// <summary>
    /// https://www.hackerrank.com/contests/womens-codesprint-3/challenges/ascii-flower
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var input = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

            var flower = PrepareFlower(input);

            foreach (string s in flower)
            {
                Console.WriteLine(s);
            }
        }

        /*
         * input[0] - row
         * input[1] - column 
         * 
         */
        public static IList<string> PrepareFlower(int[] input)
        {
            int rows = input[0];
            int columns = input[1];

            var singleFlower = new List<string>();
            singleFlower.Add("..O..");
            singleFlower.Add("O.o.O");
            singleFlower.Add("..O..");

            var flowerPatterns = new List<string>();

            for (int row = 0; row < rows; row++)
            {
                foreach (string s in singleFlower)
                {
                    var flowerRow = new StringBuilder();

                    for (int col = 0; col < columns; col++)
                    {
                        flowerRow.Append(s);
                    }

                    flowerPatterns.Add(flowerRow.ToString());
                }
            }

            return flowerPatterns;
        }
    }
}
