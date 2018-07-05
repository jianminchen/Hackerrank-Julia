using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameOfProjects
{
    class Program
    {
        static void Main(string[] args)
        {
            process();
            //testing(); 
        }

        private static void testing()
        {
            string test1 = calculateWinner(1, 10, 11);
            string test2 = calculateWinner(7, 20, 5);

            string test3 = calculateWinner(5, 10, 17);
            string test4 = calculateWinner(5, 10, 21);
        }

        private static void process()
        {
            int g = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < g; i++)
            {
                string[] arr = Console.ReadLine().Split(' ');
                int begin = Convert.ToInt32(arr[0]);
                int end = Convert.ToInt32(arr[1]);
                int k = Convert.ToInt32(arr[2]);

                Console.WriteLine(calculateWinner(begin, end, k));
            }
        }
        /*
         * 9:52pm
         * Cannot believe that this function brings 15 out of 50 in score
         * this is hard algorithm
         * 
         * So, there are some bugs in this algorithm, try to reason and improve it. 
         * 0 - begin - disputable area, who should win
         * 
         */
        private static string calculateWinner(int begin, int end, int k)
        {
            string[] message = new string[2] { "Alice", "Bob" };
            int winner = 0; // 0 - Alice, 1 - Bob

            int startPos = begin;
            int firstRange = end - begin;
            int secondRange = begin;

            int module = 2 * end + 1;
            int res = k % module;
            int div = k / module;
            bool isEven = div % 2 == 0;

            //if (isEven)
            {
                if (res < begin)
                    winner = 0;
                if (res <= end && res >= begin)
                    winner = 0;
                if (res > end && res <= (begin + end))
                    winner = 1;
                if (res > begin + end)
                    winner = 0;
            }
            /*
        else
        {
            if (res < begin)
                winner = 1;
            if (res <= end && res >= begin)
                winner = 1;
            if (res > end && res <= (begin + end))
                winner = 0;
            if (res > begin + end)
                winner = 1;
        }
        */
            return message[winner];
        }
    }
}
