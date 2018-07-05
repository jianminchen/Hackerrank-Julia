using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointFiltering
{
    class Bucket
    {
        public int key;
        public int zValue;

        public string[] threeDPoints;
    }
    class Program
    {
        /*
         * 11:20 start to read the question 
         *       read 10+ minutes
         * Put some design notes together 
         * bucket (size of b)
         * Add/ Delete/ Replenish
         * Req: maintain the size of b all the time
         * 
         * Main List
         * Sort
         * remove/ first b
         * remove top1 
         * not empty
         * 
         * Time complexity: Sort using nlog(n)
         * Space complexity: Use Dictionary to store the points
         * Use integer to express 1.000 - 1000 instead of 1
         * 
         * 2:35pm start to test the program 
         * 2:59 wrong anser after 1 key is removed from bucket
         *      continue to fix the bug related to replenish the bucket 
         * 3:07 passed the sample test case
         * 
         * Summary:
         * Reading and design: 30 minutes
         * Coding: 1:27pm - 3:07pm
         * Testing: 30 minutes (2:35pm - 3:07pm)
         */
        static void Main(string[] args)
        {
            string[] arr1 = Console.ReadLine().Split(' ');
            int n = Convert.ToInt32(arr1[0]);
            int b = Convert.ToInt32(arr1[1]);

            Dictionary<int, string[]> dataZ = new Dictionary<int, string[]>(); // Z value is the key 


            int[] zValue = new int[n]; // Z: -60.50 -> keep 3 decimal: -60500, in other words, times 1000

            for (int i = 0; i < n; i++)
            {
                string[] arr2 = Console.ReadLine().Split(' ');
                zValue[i] = convertToInt(arr2[3]);
                dataZ.Add(zValue[i], arr2);

            }

            Array.Sort(zValue); // Find out the result of sorting - ascending order
            Array.Reverse(zValue);

            Dictionary<int, Bucket> bucket = newBucket(dataZ, zValue, b);

            int start = b;
            while (true)
            {
                string s = Console.ReadLine();
                if (s == null || s.Length == 0)
                    break;

                string[] arr3 = s.Split(' ');

                Console.WriteLine(processQueries(
                    bucket,
                    dataZ,
                    zValue,
                    arr3,
                    ref start));
            }
        }

        /*
         *  Assumming that zValue array is in descending order
         *  Assuming that n >= b
         */
        private static Dictionary<int, Bucket> newBucket(
            Dictionary<int, string[]> dataZ,
            int[] zValue,
            int b)
        {
            Dictionary<int, Bucket> data = new Dictionary<int, Bucket>();

            for (int i = 0; i < b; i++)
            {
                int zV = zValue[i];

                Bucket bucket = getBucket(zV, dataZ);

                data.Add(bucket.key, bucket);
            }

            return data;
        }

        /*
         * Extract to one standalone function
         * 2:22pm - extract it to a standalone function 
         * 2:26pm - exit the function
         */
        private static Bucket getBucket(int zValue, Dictionary<int, string[]> dataZ)
        {
            string[] arr = dataZ[zValue];

            int key = Convert.ToInt32(arr[0]);

            Bucket bucket = new Bucket();
            bucket.key = key;
            bucket.zValue = zValue;

            string[] subArray = new string[3];
            Array.Copy(arr, 1, subArray, 0, 3);
            bucket.threeDPoints = subArray;

            return bucket;
        }

        /*
         * -1 -> -1000
         * 3 decimals 
         * 0.001 -> 1
         */
        private static int convertToInt(string s)
        {
            return Convert.ToInt32(Convert.ToDouble(s) * 1000);
        }

        /*
         * 1:27pm start to work on - add message array
         * 1:38pm write remove/ find functionality  
         * 1:44pm write prepareFind function 
         * 2:16pm work on remove part 
         * 2:32pm exit the function 
         */
        private static string processQueries(
            Dictionary<int, Bucket> buckets,
            Dictionary<int, string[]> dataZ,
            int[] zValue,
            string[] input,
            ref int start)
        {
            string[] message = new string[]{
                "Point doesn't exist in the bucket.", 
                "No more points can be deleted.",
                "Point id k removed."   // replace k with real number later
            };

            char[] remove = new char[2] { 'R', 'r' };
            char[] find = new char[2] { 'F', 'f' };

            string result = string.Empty;

            char action = input[0][0];
            int k = Convert.ToInt32(input[1]);

            bool isRemove = (Array.IndexOf(remove, action) != -1);
            bool isFind = (Array.IndexOf(find, action) != -1);

            if (isFind)
            {
                if (buckets.ContainsKey(k))
                {
                    return prepareFind(k, buckets);
                }
                else
                    return message[0];
            }
            else if (isRemove)
            {
                if (!buckets.ContainsKey(k))
                    return message[0];

                if (start == zValue.Length)
                    return message[1];

                buckets.Remove(k);

                // replenish the bucket once it's removed
                int zV = zValue[start];

                Bucket bucketNew = getBucket(zV, dataZ);

                buckets.Add(bucketNew.key, bucketNew);

                start++; // move to next pos 

                return message[2].Replace("k", k.ToString());
            }

            return result;
        }

        /*
         * 1:44pm write prepareFind function 
         * 2:10pm exit the function 
         */
        private static string prepareFind(int k, Dictionary<int, Bucket> bucket)
        {
            Bucket b = bucket[k];

            string[] arr = b.threeDPoints;

            string res = k.ToString() + " = (";
            for (int i = 0; i < 3; i++)
            {
                res += to3Decimal(arr[i]);

                if (i < 2)
                    res += ",";
            }
            res += ")";

            return res;
        }

        /*
         * 2:02pm work on to3Decimal function 
         * 2:12pm exit the function
         */
        private static string to3Decimal(string s)
        {
            int len = s.Length;
            int pos = Array.IndexOf(s.ToCharArray(), '.', 0);
            if (pos == -1)
            {
                return s + ".000";
            }
            else if (len - pos < 4)
            {
                int no = pos + 4 - len;
                char[] arr = Enumerable.Repeat('0', no).ToArray();
                return s + new string(arr);
            }

            return s;
        }
    }
}
