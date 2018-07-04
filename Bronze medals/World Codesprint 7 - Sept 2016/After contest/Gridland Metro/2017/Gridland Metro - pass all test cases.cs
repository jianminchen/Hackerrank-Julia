using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridlandMetro
{
    /*
     * problem statement:
     * https://www.hackerrank.com/contests/world-codesprint-7/challenges/gridland-metro
     */
    class Program
    {
        static void Main(string[] args)
        {
            ProcessInput();
        }

        public static void ProcessInput()
        {
            string[] summaryRow = Console.ReadLine().Split(' ');
            int rowNumber = Convert.ToInt32(summaryRow[0]);
            int columnNumber = Convert.ToInt32(summaryRow[1]);
            int k = Convert.ToInt32(summaryRow[2]);

            var tracks = new List<Tuple<int, int, int>>();

            for (int i = 0; i < k; i++)
            {
                var rowData = Console.ReadLine().Split(' ');

                int row = Convert.ToInt32(rowData[0]);
                int colStart = Convert.ToInt32(rowData[1]);
                int colEnd = Convert.ToInt32(rowData[2]);

                tracks.Add(new Tuple<int, int, int>(row, colStart, colEnd));
            }
            tracks.Sort();

            long sum = (long)rowNumber * columnNumber;

            Console.WriteLine(sum -
                    CalculateNumberOfCellsTakenByTrainTracks(tracks));
        }

        public static long CalculateNumberOfCellsTakenByTrainTracks(
            IList<Tuple<int, int, int>> tracks
            )
        {
            // Set currentColEnd to -1 so that this track's length is 0.
            int currentRow = -1;
            int currentColStart = 0;
            int currentColEnd = -1;
            long cellsTakenByTrack = 0;

            foreach (var track in tracks)
            {
                int row = track.Item1;
                int colStart = track.Item2;
                int colEnd = track.Item3;

                if (row != currentRow || colStart > currentColEnd)
                {
                    // No overlap with current track.
                    // First add length of current track to total.
                    cellsTakenByTrack += (currentColEnd - currentColStart + 1);
                    // Now set current track to this track.
                    currentRow = row;
                    currentColStart = colStart;
                    currentColEnd = colEnd;
                }
                else if (colEnd > currentColEnd)
                {
                    // Extend current track.
                    currentColEnd = colEnd;
                }
            }

            cellsTakenByTrack += (currentColEnd - currentColStart + 1);
            return cellsTakenByTrack;
        }
    }
}