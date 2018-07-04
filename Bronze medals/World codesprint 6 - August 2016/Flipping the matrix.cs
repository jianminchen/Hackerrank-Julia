using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlippingTheMatrix
{
    class Program
    {
        static void Main(string[] args)
        {
            int queries = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < queries; i++)
            {
                int n = Convert.ToInt32(Console.ReadLine().Trim());

                int dim = 2 * n;
                int[,] arr = new int[dim, dim];

                for (int k = 0; k < dim; k++)
                {
                    string[] arr2 = Console.ReadLine().Split(' ');

                    for (int j = 0; j < dim; j++)
                    {
                        arr[k, j] = Convert.ToInt32(arr2[j]);
                    }
                }
                Console.WriteLine(calMaxQuadrant(arr, dim));
            }
        }


        /*
         * 
         */
        public static long calMaxQuadrant(int[,] arr, int dim)
        {
            long sum = 0;

            int n = dim / 2;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    // Four choices 
                    int last = 2 * n - 1;
                    int[] arr2 = new int[4]{
                        arr[i,j],
                        arr[last - i, j], 
                        arr[i,last - j], 
                        arr[last - i, last - j]};
                    sum += arr2.Max();
                }

            return sum;
        }
    }
}
