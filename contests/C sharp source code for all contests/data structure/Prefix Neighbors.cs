using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongestCommonPrefix_Trie
{
    public class StringComparer : IComparer<string>
    {
        public int Compare(string left, string right)
        {
            return left.CompareTo(right);
        }
    }

    class TrieNodeSelected
    {
        public TrieNode trieNode { get; set; }
        public bool isIncluding { get; set; }

        public TrieNodeSelected(TrieNode node, bool value)
        {
            trieNode = node;
            isIncluding = value;
        }
    }

    /*
     * First writing:
     * source code reference:
     * http://www.geeksforgeeks.org/longest-common-prefix-set-5-using-trie/
     * 
     * TrieNode class design: 
     * 6 APIs:
     * member variables:
     * 26 children node
     * isLeaf bool variable
     * TrieNode[] children
     * 
     * - TrieNode's bascis 
     * GetNode
     * 
     * - Construct a trie
     * AddOneStringToTrie -
     * AddStringsToTrie - 
     * 
     * - Query a trie  
     * 
     * - More advanced feature: 
     * build a trie first, and then query the trie. 
     * 
     * 
     */


    class TrieNode
    {
        private static readonly int ALPHABET_SZIE = 26;
        private static readonly char BASE = 'A';

        private TrieNode[] children = new TrieNode[ALPHABET_SZIE];

        private TrieNode parentNode { get; set; }
        private string name { get; set; }
        private bool isLeaf { get; set; }
        private bool isInDictionary { get; set; }

        // string's maximum length is 11
        private Dictionary<string, TrieNodeSelected>[] nodesByLevels = new Dictionary<string, TrieNodeSelected>[12];

        /*
         * Need to add more information for prefix neighbors processing
         */
        public void AddOneStringToTrie(TrieNode root, string input)
        {
            int length = input.Length;

            TrieNode runner = root;

            for (int i = 0; i < length; i++)
            {
                int index = CharToIndex(input[i]);

                if (runner.children[index] == null)
                {
                    runner.children[index] = PrepareTrieNode(false);
                    runner.children[index].parentNode = runner;
                    runner.children[index].name = input.Substring(0, i + 1);
                }

                // mark the string as a node in the dictionary
                if (i == length - 1)
                {
                    runner.children[index].isInDictionary = true;

                    nodesByLevels[i + 1].Add(input, new TrieNodeSelected(runner.children[index], true));
                }

                runner = runner.children[index];
            }

            // mark last node as leaf
            runner.isLeaf = true;
        }

        // A Function to construct trie
        public void AddStringsToTrie(string[] inputStrings, TrieNode root)
        {
            for (int i = 0; i < inputStrings.Length; i++)
            {
                AddOneStringToTrie(root, inputStrings[i]);
            }

            return;
        }

        /*
         * Need to do a few things:
         * 1. Set up a node 
         * 2. Set up nodesByLevels
         */
        public static TrieNode PrepareTrieNode(bool setNodesBylevel)
        {
            TrieNode node = new TrieNode();

            node.isLeaf = false;
            for (int i = 0; i < ALPHABET_SZIE; i++)
            {
                node.children[i] = null;
            }

            if (!setNodesBylevel)
            {
                return node;
            }

            // create dictionary for TrieNode
            for (int i = 0; i < 12; i++)
            {
                node.nodesByLevels[i] = new Dictionary<string, TrieNodeSelected>();
            }

            return node;
        }

        /*
         * code review 2/12/2017 
         * 1:53am
         * Fail all test cases after test case 10
         */
        public HashSet<string> WalkFromBottomLevelUpDescendingOrder(TrieNode root)
        {
            var nodesByLevels = root.nodesByLevels;
            var chooseSubset = new HashSet<string>();

            for (int i = 11; i > 0; i--)
            {
                var levelNodes = nodesByLevels[i];

                foreach (var key in levelNodes.Keys)
                {
                    var selectedKey = levelNodes[key];
                    var trieNode = selectedKey.trieNode;
                    var isIncluding = selectedKey.isIncluding;

                    if (!isIncluding)
                    {
                        continue;
                    }

                    // add the string into choose subset
                    if (trieNode.name != null && trieNode.name.Length > 0)
                    {
                        chooseSubset.Add(trieNode.name);
                    }

                    // set its prefix neighbor node to exclude from chooseSubset   
                    while (trieNode != null)
                    {
                        var parentNode = trieNode.parentNode;

                        if (parentNode == null || parentNode.name == null)
                        {
                            break;
                        }

                        string newKey = parentNode.name;

                        int parentLevel = parentNode.name.Length;

                        if (!nodesByLevels[parentLevel].Keys.Contains(parentNode.name))
                        {
                            trieNode = parentNode; // move to next iteration 
                            continue;
                        }

                        nodesByLevels[parentLevel][newKey] = new TrieNodeSelected(parentNode, false);
                        break;  // found the prefix neighbor node, exit
                    }
                }
            }

            return chooseSubset;
        }

        /*
         * Add all strings's ascii value together
         * A - 65
         */
        public long CalculateBenefitValue(HashSet<string> input)
        {
            long sum = 0;

            foreach (string s in input)
            {
                foreach (char c in s)
                {
                    sum += c - 'A' + 65;
                }
            }

            return sum;
        }

        /*
         * start from 'A' - need to clarify 
         */
        public int CharToIndex(char c)
        {
            return c - BASE;
        }

        // Driver program to test above function
        public static void Main(string[] args)
        {
            ProcessInput();

            //RunSampleTestcase2(); 
        }

        public static void ProcessInput()
        {
            int length = Convert.ToInt32(Console.ReadLine());
            string[] input = Console.ReadLine().Split(' ');

            Console.WriteLine(RunBenefitValueProgram(input));
        }

        /*
         * 
         */
        public static void RunSampleTestcase2()
        {
            //string[] inputStrings = { "A", "AB", "AC", "ABD", "B" };            
            // string[] inputStrings = {"A","B" };
            //string[] inputStrings = {"AB","AC","A"};

            // this test case helps to fix the design issue - how to update prefix neighbor's status
            // not necessary parent node, go up to the node in the orginal dictionary. 
            string[] inputStrings = { "A", "AB", "ABCDEFGHIJK" };

            long benefitValue = RunBenefitValueProgram(inputStrings);
        }

        public static long RunBenefitValueProgram(string[] inputStrings)
        {
            // First, build a trie 
            TrieNode root = TrieNode.PrepareTrieNode(true);
            root.AddStringsToTrie(inputStrings, root);

            // Perform a walk by level from 11 to 1 in descending order, and go over the string by descending order
            var chosenSubset = root.WalkFromBottomLevelUpDescendingOrder(root);

            long benefitValue = root.CalculateBenefitValue(chosenSubset);

            return benefitValue;
        }
    }
}