using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawBook
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            int p = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine(GetMinimumNumberOfPages(n, p));
        }

        private static int GetMinimumNumberOfPages(int n, int p)
        {
            int pageNo = p / 2 + 1;
            int lastPage = n / 2 + 1;
            return Math.Min(pageNo - 1, lastPage - pageNo);
        }
    }
}
