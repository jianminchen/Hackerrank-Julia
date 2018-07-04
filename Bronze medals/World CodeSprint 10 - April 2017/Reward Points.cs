using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RewardPoints
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

            var totalPoints = 0;
            foreach (var number in numbers)
            {
                totalPoints += Math.Min(number, 10) * 10;
            }

            Console.WriteLine(totalPoints);
        }
    }
}
