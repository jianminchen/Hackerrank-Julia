using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zeroMoveNim
{
    class Program
    {
        static void Main(string[] args)
        {
            // For hackerRank
            RunHackerRank();

            // For my testing 
            // RunSampleTestCases(); 
        }

        private static void RunHackerRank()
        {
            IList<char> results = ProcessZeroMoveGame();
            Output(results);

        }

        private static void Output(IList<char> data)
        {
            foreach (char c in data)
            {
                Console.WriteLine(c);
            }
        }

        private static void RunSampleTestCases()
        {
            // First player 'W',   second player 'L'

            //Debug.Assert( ProcessZerorMoveNimGame(2, new int[] { 2, 2 }) == 'L');     // Second player
            //Debug.Assert( ProcessZerorMoveNimGame(2, new int[] { 1, 2 }) == 'W');     // go to first one
            //Debug.Assert( ProcessZerorMoveNimGame(3, new int[] { 1, 2, 3 }) == 'W');  // go to first  'W'
            //Debug.Assert(ProcessZerorMoveNimGame(3, new int[] { 2, 2, 4 }) == 'W');   // go to first
            Debug.Assert(ProcessZerorMoveNimGame_usingNimSum(3, new int[] { 2, 3, 4 }) == 'L');   // go to second
        }

        private static void RunSampleTestCase1()
        {
            int result = ProcessZerorMoveNimGame_usingNimSum(2, new int[] { 2, 2 });
        }

        private static IList<char> ProcessZeroMoveGame()
        {
            IList<char> results = new List<char>();

            int nQueries = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < nQueries; i++)
            {
                int nPiles = Convert.ToInt32(Console.ReadLine());
                int[] piles = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

                results.Add(ProcessZerorMoveNimGame_usingNimSum(nPiles, piles));
            }

            return results;
        }


        /*
         * ProcessZerorMoveNimGame_usingNimSum
         * https://en.wikipedia.org/wiki/Nim
         * 
         */
        private static char ProcessZerorMoveNimGame_usingNimSum(int nPiles, int[] piles)
        {
            int nimSum = 0;

            for (int i = 0; i < piles.Length; i++)
            {
                nimSum = nimSum ^ piles[i];
            }

            int countZeroMove = nPiles;

            if (countZeroMove % 2 == 0)
            {
                return (nimSum == 0) ? 'L' : 'W';
            }
            else
            {
                return (nimSum == 0) ? 'W' : 'L';
            }
        }
    }
}
