using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/*
 * Problem statement:
 * https://www.hackerrank.com/contests/rookierank-2/challenges/prefix-neighbors
 * 
 * Solution notes:
 * This problem can be solved using Trie and DP. Create a trie from the 
 * given set of strings. 
 * Find the prefix neighbor of each string. Now create a graph such that 
 * each string is a node and there exist a bidirectional edge between two 
 * nodes only if they are prefix neighbors. Now find the maximum weighted
 * independent set.
 * 
 * time complexity: O(N*max_length_of_string)
 * required knowledge: Trie
 * 
 * Maximum weighted independent set:
 * https://en.wikipedia.org/wiki/Independent_set_(graph_theory)
 */
class Solution
{
    /*
     * code review: 
     * How to set up a Trie with 26 children? 
     * n distinct strings are stored in a dictionary, using a data structure Trie. 
     * If the string is in the dictionary, then we mark the string in the Trie as 
     * IsInDictionary, WordInDictionary. 
     * @IsInDictionary - the word is in the original strings
     * @WordInDictionary   - any string in the orginal strings 
     */
    internal class TrieWithPrefixNeighbor
    {
        private IDictionary<char, TrieWithPrefixNeighbor> Children { get; set; }
        private bool IsWord { get; set; }
        private string PrefixNeighbor { get; set; }

        public TrieWithPrefixNeighbor()
        {
            Children = new Dictionary<char, TrieWithPrefixNeighbor>();
            IsWord = false;
            PrefixNeighbor = "";
        }

        /*
         * Turn API to private - based on code review
         * 
         * AddWordToTrieByOneCharATime
         * function is designed to complete the following tasks:
         * Add one word in the dictionary to Trie using recursive function
         * Add one char a time by scanning a word from left to right. 
         * 
         * Tricky part is to set prefix neighbor in the process. 
         * 
         * @index
         * @charArray
         * @word  - 
         * @neighbor - prefix neighbor, it is empty string if there is no prefix neighbor
         * 
         * function return: 
         * Tuple<string, string> - string and its prefix neighbor
         */
        private Tuple<string, string> addWordToTrieByOneCharATime(
            int scanIndex,
            char[] charArray,
            string word,
            string neighbour)
        {
            bool isEndOfString = scanIndex == charArray.Length;
            if (isEndOfString)
            {
                IsWord = true;
                PrefixNeighbor = word;

                return new Tuple<string, string>(PrefixNeighbor, neighbour);
            }

            char visiting = charArray[scanIndex];

            if (!Children.ContainsKey(visiting))
            {
                Children[visiting] = new TrieWithPrefixNeighbor();
            }

            string updatedNeighbor = IsWord ? PrefixNeighbor : neighbour;

            // update neighbor string - if IsInDictionary is true, then it is 
            // to set as a prefix neighbor
            return Children[visiting].addWordToTrieByOneCharATime(
                scanIndex + 1,
                charArray,
                word,
                updatedNeighbor);
        }

        /*
         * public API - only two arguments
         * @word
         * @neighbor
         */
        public Tuple<string, string> AddWordToTrie(string word, string neighbour)
        {
            return addWordToTrieByOneCharATime(
                   0,
                   word.ToCharArray(),
                   word,
                   neighbour);
        }
    }

    /*
     * study LINQ - GroupBy
     * https://msdn.microsoft.com/en-us/library/bb534304(v=vs.110).aspx
     * 
     * 1. Group string by first char, 26 variations from 'A', 'B', ..., 'Z'
     * 2. For each group, sort strings by string's length in ascending order
     * 3. For example, group of strings starting from char 'A', 
     *    "A","AB","ACD"
     * 4. benefit value is to add all chars' ascii value.   
     */
    static long Process(string[] dict)
    {
        var benefitValue = 0L;

        var groupped = dict.GroupBy(x => x[0]);

        // maximum 26 groups, 'A','B', ..., 'Z'
        foreach (var group in groupped)
        {
            // sort by string's length in ascending order
            var sortedStrings = group.OrderBy(x => x.Length);

            var trie = new TrieWithPrefixNeighbor();
            var banned = new HashSet<string>();
            var stack = new Stack<Tuple<string, string>>();

            foreach (var word in sortedStrings)
            {
                stack.Push(trie.AddWordToTrie(word, ""));
            }

            // Enumerate the stack, the longest string will be iterated first. 
            // Maximum independent set is kind of greedy as well. 
            foreach (var tuple in stack)
            {
                if (!banned.Contains(tuple.Item1))
                {
                    benefitValue += tuple.Item1.ToCharArray().Aggregate(0L, (val, next) => val + (long)next);
                    banned.Add(tuple.Item2);
                }
            }
        }

        return benefitValue;
    }

    static void Main(String[] args)
    {
        ProcessInput();

        //RunSampleTestcase2(); 
    }
    /*
     * Trie
     *       A          B
     *     AB
     *   ABC
     *  ABCD 
     *  
     * Things to learn through the debug:
     * How many tries are used in the coding? 
     * Two tries, one is for A started strings: {"A","ABCD"}
     * second one is for B started strings: {"B"}
     */
    static void RunSampleTestcase()
    {
        string[] dict = new string[] { "A", "ABCD", "B" };
        var points = Process(dict);

        Console.WriteLine(points);
    }

    /*
    *  input string[] = new string[]{"A","ABC","AC"}
    * Trie
    *       A          
    *     AB   AC
    *   ABC
    *   
    *  Try to debug the code and figure out how to find prefix neighbor of "ABC". 
    *  How many return calls for "ABC" in the function: 
    *  AddWordToTrieByOneCharATime
    */
    static void RunSampleTestcase2()
    {
        string[] dictionary = new string[] { "A", "ABC", "AC" };
        var points = Process(dictionary);

        Console.WriteLine(points);
    }

    static void ProcessInput()
    {
        var n = Convert.ToInt32(Console.ReadLine());
        var dict = Console.ReadLine().Split(' ');
        var points = Process(dict);

        Console.WriteLine(points);
    }
}
