using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KthZero
{
    class Program
    {
        /*
         5:48pm - start to work on time out issue 
         * 6:52pm - bug fix: 
         * stack over flow 
         * Are arays or lists passed by default by refrence in C#? 
         * Need to pass ref
        */
        static void Main(string[] args)
        {
            string[] arr = Console.ReadLine().Split(' ');

            //int size = Convert.ToInt32(arr[0]);
            int queries = Convert.ToInt32(arr[1]);

            string[] table = Console.ReadLine().Split(' ');

            HashSet<int> zeroData = getZeroData(table);
            int[] zeroArray = zeroData.ToArray();
            Array.Sort(zeroArray);

            for (int i = 0; i < queries; i++)
            {
                string[] arr2 = Console.ReadLine().Split(' ');
                int symbol = Convert.ToInt32(arr2[0]);
                int kth = Convert.ToInt32(arr2[1]);

                if (symbol == 1)
                {
                    Console.WriteLine(getKthZero(
                        table,
                        zeroData,
                        zeroArray,
                        kth));
                }
                else if (symbol == 2)
                {
                    updateQuery(
                        table,
                        zeroData,
                        ref zeroArray,
                        arr2);
                }
            }
        }

        /*
         
         */
        private static HashSet<int> getZeroData(string[] arr)
        {
            HashSet<int> data = new HashSet<int>();

            for (int i = 0; i < arr.Length; i++)
            {
                int no = Convert.ToInt32(arr[i]);
                if (no == 0)
                    data.Add(i);
            }

            return data;
        }

        /*
        
         */
        private static string getKthZero(
            string[] table,
            HashSet<int> zeroData,
            int[] zeroArray,
            int kth)
        {
            string NO = "NO";

            // Need to make sure the sync of zeroData and zeroArray
            if (kth >= 1 &&
                kth <= zeroArray.Length
                )
                return zeroArray[kth - 1].ToString();
            else
                return NO;
        }

        /*
       
         */
        private static void updateQuery(
            string[] table,
            HashSet<int> zeroData,
            ref int[] zeroArray,
            string[] para
            )
        {
            int kth = Convert.ToInt32(para[1]);
            int newValue = Convert.ToInt32(para[2]);

            if (kth < 0 || kth > table.Length)
                return;

            bool isIn = zeroData.Contains(kth);
            if (isIn && newValue != 0)
            {
                zeroData.Remove(kth);

                zeroArray = removeOne(
                                 zeroArray,
                                 kth);
            }
            else if (!isIn && newValue == 0)
            {
                zeroData.Add(kth);

                zeroArray = addOne(
                        zeroArray,
                        kth
                    );
            }

            table[kth] = newValue.ToString();
        }

        /*
         * 6:02pm - start to code
         * Fix time out issue - maintain the zero array - not using sorting, only sort once; 
         * late, just O(n) to create a new array, better than O(nlogn) sorting 
         */
        private static int[] removeOne(
            int[] zeroArray,
            int kth)
        {
            int len = zeroArray.Length;
            int[] newZA = new int[len - 1];

            int count = 0;
            for (int i = 0; i < zeroArray.Length; i++)
            {
                int val = zeroArray[i];

                if (val != kth)
                    newZA[count++] = val;
            }

            return newZA;
        }

        /*
        * 6:07pm - start to code
        * Fix time out issue - maintain the zero array - not using sorting, only sort once; 
        * late, just O(n) to create a new array, better than O(nlogn) sorting 
        */
        private static int[] addOne(
            int[] zeroArray,
            int kth)
        {
            int len = zeroArray.Length;
            int[] newZA = new int[len + 1];

            int count = 0;
            bool addNew = false;
            for (int i = 0; i < len; i++)
            {
                int val = zeroArray[i];

                if (val < kth)
                    newZA[count++] = val;
                else if (val > kth && !addNew)
                {
                    addNew = true;
                    newZA[count++] = kth;
                    newZA[count++] = zeroArray[i];
                }
                else if (val > kth && addNew)
                    newZA[count++] = zeroArray[i];
            }

            // edge case
            if (!addNew)
            {
                newZA[count] = kth;
            }

            return newZA;
        }
    }
}
