using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderStrings
{
    class Key
    {
        public string name { get; set; }

        public Key(string name)
        {
            this.name = name;
        }
    }

    class KeyValue
    {
        public int value { get; set; }

        public KeyValue(string valueString)
        {
            value = Convert.ToInt32(valueString);
        }
    }

    class KeyValueLengthUpTo50
    {
        public string value { get; set; }

        public KeyValueLengthUpTo50(string valueString)
        {
            value = valueString;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class KeySorterLexicographic : IComparer
    {
        public int Compare(object o1, object o2)
        {
            Key p1 = o1 as Key;
            Key p2 = o2 as Key;

            int compare = p1.name.CompareTo(p2.name);

            return compare;
        }
    }

    public class KeySorterNumeric : IComparer
    {
        public int Compare(object o1, object o2)
        {
            KeyValue p1 = o1 as KeyValue;
            KeyValue p2 = o2 as KeyValue;

            int compare = p1.value < p2.value ? -1 : 1;

            return compare;
        }
    }

    public class KeySorterNumericUpTo50 : IComparer
    {
        public int Compare(object o1, object o2)
        {
            KeyValueLengthUpTo50 p1 = o1 as KeyValueLengthUpTo50;
            KeyValueLengthUpTo50 p2 = o2 as KeyValueLengthUpTo50;

            var firstString = removeLeadingZeros(p1.value);
            var secondString = removeLeadingZeros(p2.value);

            var length1 = firstString.Length;
            var length2 = secondString.Length;

            var lengthEqual = length1 == length2;
            int compare = -1;
            if (!lengthEqual)
            {
                compare = length1 < length2 ? -1 : 1;
            }
            else
            {
                int index = 0;

                while (index < length1)
                {
                    var first = firstString[index];
                    var second = secondString[index];

                    var difference = first - second;
                    if (difference == 0)
                    {
                        index++;
                    }
                    else
                    {
                        compare = difference < 0 ? -1 : 1;
                        break;
                    }
                }
            }

            return compare;
        }

        private string removeLeadingZeros(string numbers)
        {
            if (numbers == null || numbers.Length == 0)
            {
                return string.Empty;
            }

            int countZero = 0;
            var length = numbers.Length;

            for (int i = 0; i < length; i++)
            {
                var visit = numbers[i];

                if (visit != '0')
                {
                    break;
                }

                countZero = i + 1;
            }

            return numbers.Substring(countZero);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ProcessCode();
            //RunTestcase(); 
        }

        public static void RunTestcase()
        {
            var numbers = new string[] { "92 022", "82 12", "77 13" };
            var configurations = "2 false numeric";
            var reverse = SortByInstruction(numbers, configurations);
        }

        public static void ProcessCode()
        {
            int n = Convert.ToInt32(Console.ReadLine().ToString());

            var numbers = new string[n];
            for (int i = 0; i < n; i++)
            {
                numbers[i] = Console.ReadLine();
            }

            var configurations = Console.ReadLine();

            var reverse = SortByInstruction(numbers, configurations);

            if (!reverse)
            {
                foreach (var item in numbers)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                var length = numbers.Length;

                for (int i = length - 1; i >= 0; i--)
                {
                    Console.WriteLine(numbers[i]);
                }
            }
        }

        /// <summary>
        /// key - 2
        /// reverse - true, false
        /// sort format - numeric, lexicographic
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="configurations"></param>
        /// <returns></returns>
        public static bool SortByInstruction(string[] numbers, string configurations)
        {
            var parameters = configurations.Split(' ');

            int key = Convert.ToInt32(parameters[0]);
            bool reverseCheck = Convert.ToBoolean(parameters[1]);
            string order = parameters[2];

            var isNumeric = order.Contains("numeric");
            var isLexicographical = order.Contains("lexicographical");

            int length = numbers.Length;

            if (isNumeric)
            {
                var comparer = new KeySorterNumericUpTo50();
                var values = getKthValueNumericLengthUpTo50(numbers, key);
                Array.Sort(values, numbers, comparer);
            }
            else
            {
                var comparer = new KeySorterLexicographic();
                Key[] keys = getKthValueLexicographical(numbers, key);
                Array.Sort(keys, numbers, comparer);
            }

            return reverseCheck;
        }

        private static KeyValue[] getKthValueNumeric(string[] numbers, int key)
        {
            int length = numbers.Length;

            var kthValues = new KeyValue[length];

            for (int i = 0; i < length; i++)
            {
                kthValues[i] = new KeyValue(numbers[i].Split(' ')[key - 1]);
            }

            return kthValues;
        }

        private static KeyValueLengthUpTo50[] getKthValueNumericLengthUpTo50(string[] numbers, int key)
        {
            int length = numbers.Length;

            var kthValues = new KeyValueLengthUpTo50[length];

            for (int i = 0; i < length; i++)
            {
                kthValues[i] = new KeyValueLengthUpTo50(numbers[i].Split(' ')[key - 1]);
            }

            return kthValues;
        }

        private static Key[] getKthValueLexicographical(string[] numbers, int key)
        {
            int length = numbers.Length;

            var kthKeys = new Key[length];

            for (int i = 0; i < length; i++)
            {
                kthKeys[i] = new Key(numbers[i].Split(' ')[key - 1]);
            }

            return kthKeys;
        }
    }
}
