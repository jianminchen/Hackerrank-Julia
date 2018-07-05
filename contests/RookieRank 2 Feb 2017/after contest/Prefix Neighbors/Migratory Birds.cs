using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigratoryBirds
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());

            int[] ids = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

            Console.WriteLine(CalculateMaximum(ids));
        }

        public static int CalculateMaximum(int[] ids)
        {
            int n = ids.Length;

            int[] idCounts = new int[5];

            for (int i = 0; i < n; i++)
            {
                int id = ids[i] - 1;
                idCounts[id]++;
            }

            int maxValue = idCounts[0];
            int idWithMaxValue = 0;
            for (int i = 1; i < 5; i++)
            {
                if (idCounts[i] > maxValue)
                {
                    idWithMaxValue = i;
                    maxValue = idCounts[i];
                }
            }

            return idWithMaxValue + 1;
        }
    }
}
