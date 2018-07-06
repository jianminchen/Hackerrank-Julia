using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HikingSelfies
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine().Trim());
            int x = Convert.ToInt32(Console.ReadLine().Trim());

            int comb = (int)Math.Pow(2, n) - 1;
            if (comb >= x)
                Console.WriteLine(comb - x);
            else
                Console.WriteLine(x - comb);
        }
    }
}
