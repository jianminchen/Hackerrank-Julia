using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CultureConference_studyCode
{
    /// <summary>
    /// culture conference 
    /// https://www.hackerrank.com/contests/rookierank-3/challenges/culture-conference
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());

            var edges = new int[n - 1][];

            for (int i = 0; i < n - 1; i++)
            {
                string[] data = Console.ReadLine().Split(' ');
                edges[i] = Array.ConvertAll(data, Int32.Parse);
            }

            var result = GetMinimumNumberOfEmployees(n, edges);

            Console.WriteLine(result);
        }

        /// <summary>
        /// Very straightforward idea to solve the problem. 
        /// time complexity  - each node is checked once
        /// space complexity - 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="edges"></param>
        /// <returns></returns>
        public static int GetMinimumNumberOfEmployees(int n, int[][] edges)
        {
            var burntList = new int[n];
            var supervisor = new int[n];
            var subordinates = new HashSet<int>[n];

            for (int i = 0; i < n; i++)
            {
                subordinates[i] = new HashSet<int>();
            }

            for (int i = 1; i < n; i++)
            {
                int supervisorId, burntOut;
                supervisorId = edges[i - 1][0];
                burntOut = edges[i - 1][1];

                supervisor[i] = supervisorId;

                subordinates[supervisorId].Add(i);

                burntList[i] = (burntOut == 0) ? 0 : 1;
            }

            int count = 0;
            bool checkFlag;

            for (int i = n - 1; i >= 0; i--)
            {
                var current = i;

                // loop over all subordinates
                checkFlag = false;
                foreach (var item in subordinates[current])
                {
                    // check if there is a burnt out subordinate                    
                    if (burntList[item] == 0)
                    {
                        // if yes, make this green and all subordinates and superior green
                        checkFlag = true;

                        // choose to send current employee to the conference, 
                        // then its supervisor and its children will not burn out. 
                        burntList[current] = 1;
                        burntList[item] = 1;
                        burntList[supervisor[current]] = 1;
                    }
                }

                if (checkFlag)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
