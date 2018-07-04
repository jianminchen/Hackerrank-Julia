using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace breakingTheRecords
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());

            int[] scores = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

            int[] breakingRecords = CalculateBreakingRecords(scores);

            foreach (int value in breakingRecords)
            {
                Console.Write(value + " ");
            }
        }

        /*
         * 
         */
        public static int[] CalculateBreakingRecords(int[] scores)
        {
            if (scores == null || scores.Length == 0)
            {
                return new int[] { 0, 0 };
            }

            int breakHighestScore = 0;
            int breakLowestScore = 0;
            int highestScore = scores[0];
            int lowestScore = scores[0];

            for (int i = 1; i < scores.Length; i++)
            {
                int score = scores[i];
                if (highestScore < score)
                {
                    highestScore = score;
                    breakHighestScore++;
                }

                if (lowestScore > score)
                {
                    lowestScore = score;
                    breakLowestScore++;
                }
            }

            return new int[] { breakHighestScore, breakLowestScore };
        }
    }
}
