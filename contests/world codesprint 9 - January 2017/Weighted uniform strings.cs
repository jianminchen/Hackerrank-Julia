using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightedUniformStrings
{
    class WeightedUniformStrings
    {
        static void Main(string[] args)
        {
            ProcesInput();
            //RunSampleTestcase();
        }

        public static void RunSampleTestcase()
        {
            HashSet<int> weights = AddAllPossibleUniformContinuousSubstringWeight("abccddde");
        }


        public static void ProcesInput()
        {
            string input = Console.ReadLine();
            int queries = Convert.ToInt32(Console.ReadLine());

            HashSet<int> weights = AddAllPossibleUniformContinuousSubstringWeight(input);

            for (int i = 0; i < queries; i++)
            {
                int weight = Convert.ToInt32(Console.ReadLine());
                if (weights.Contains(weight))
                {
                    Console.WriteLine("Yes");
                }
                else
                {
                    Console.WriteLine("No");
                }
            }
        }

        /*
         * let U be the set of weights for all possible uniform substrings ( contiguous ) of string s
         * 1. substring should be contiguous 
         * 2. uniform substrings - one char in the substrings
         * 3. Try to find all uniform substrings - scan the array once 
         * 
         * // add some debugging code here if need. 
         */
        public static HashSet<int> AddAllPossibleUniformContinuousSubstringWeight(string input)
        {
            HashSet<int> weights = new HashSet<int>();

            //IList<string> debugInfo = new List<string>();
            //StringBuilder builder = new StringBuilder(); 

            int length = input.Length;

            char previous = input[0];
            int weight = previous - 'a' + 1;
            weights.Add(weight);

            //builder.Append(previous); 

            for (int i = 1; i < length; i++)
            {
                char current = input[i];
                if (current - previous != 0)
                {
                    weight = GetWeight(current);
                    weights.Add(weight);
                    previous = current;   // next iteration 

                    //debugInfo.Add(builder.ToString());
                    //builder.Clear();
                    //builder.Append(current); 
                }
                else
                {
                    weight += GetWeight(current);
                    weights.Add(weight);

                    //builder.Append(current);
                    // debugInfo.Add(builder.ToString()); 
                }
            }

            // edge case 
            weights.Add(weight);

            //debugInfo.Add(builder.ToString()); 

            return weights;
        }

        /*
         * 
         */
        public static int GetWeight(char c)
        {
            return (c - 'a' + 1);
        }
    }
}
