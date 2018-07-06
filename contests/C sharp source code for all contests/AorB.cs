
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace AtoB
{
    class Program
    {
        static void Main(string[] args)
        {
            bool testCode = false;

            if (!testCode)
            {
                int totalNo = Convert.ToInt32(Console.ReadLine());
                for (int i = 0; i < totalNo; i++)
                {
                    int K = Convert.ToInt32(Console.ReadLine());
                    string A = Console.ReadLine();
                    string B = Console.ReadLine();
                    string C = Console.ReadLine();

                    string[] result = convertAToB_strVersion(K, A, B, C);
                    foreach (string s in result)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            else
            {
                string[] result = testFunction();
                //string[] result = testFunction_B();

                //testFunction_C();  

            }
        }

        public static void testFunction_C()
        {
            string[] valueFrom = { "C0", "0", "C0" };

            string valueFromA = "";
            string valueFromB = "";
            int count = getFirstNumberFirst1To0_LetSecondOneTake1(valueFrom[0], valueFrom[1], valueFrom[2], 3, ref valueFromA, ref valueFromB);
        }

        public static string[] testFunction()
        {
            int K = 8;
            string A = "2B";
            string B = "9F";
            string C = "58";

            return convertAToB_strVersion(K, A, B, C);
        }

        public static string[] testFunction_B()
        {
            int K = 5;
            string A = "B9";
            string B = "40";
            string C = "5A";

            return convertAToB_strVersion(K, A, B, C);
        }

        /*
         * Test case: 
         * 2B Hexdecimal 
         * - 3 bits
         *   00101011
         *   00001000
         *   
         * 9F  - 5 bits
         *   10011111
         *   01011000
         *   
         * Final result: 
         *   01011000
         *   
         * A, B, C < 16 ^ (5*10000), how big the number
         * 
         * The trick is not to convert to integer at all, try to use string all the time
         * 
         *   
         * http://stackoverflow.com/questions/74148/how-to-convert-numbers-between-hexadecimal-and-decimal-in-c
         */



        public static string[] convertAToB_strVersion(
            int no,
            string A,
            string B,
            string C)
        {


            string valueA_new = "";
            // Find bits in A is 1, but in C is 0
            // then, set bit to 0

            int value1 = getBitNumber_From1To0_strVersion(A, B, C, ref valueA_new);

            string valueB_new = "";
            // Find bits in B is 1, but in C is 0, 
            int value2 = getBitNumber_From1To0_strVersion(B, "", C, ref valueB_new);


            // Find bits in B is 0, but in C is 1, in A is 0 - need to change 0 to 1
            //int value3 = getBitNumber_From0to1(valueA, valueB, valueC, ref valueB_new);  //bug002 - valueB should valueB_new
            //int value3 = getBitNumber_From0to1_strVersion(A, valueB_new, C, ref valueB_new);  // second argument - not valueB
            int value3 = getChangeBits_From0To1(A, valueB_new, C, ref valueB_new);  // second argument - not valueB

            int sum = value1 + value2 + value3;
            if (sum == no)
                return new string[2] { removeStarting0(valueA_new), removeStarting0(valueB_new) };
            else if (sum < no)
            {
                // try to make A' as small as possible, and then, try to make B' as small as possible 
                int total = no - sum;
                int noUsed = getFirstNumberFirst1To0_LetSecondOneTake1(valueA_new, valueB_new, C, total, ref valueA_new, ref valueB_new);
                return new string[2] { removeStarting0(valueA_new), removeStarting0(valueB_new) };
            }
            else
                return new string[1] { "-1" };
        }

        private static string removeStarting0(string s)
        {
            StringBuilder sb = new StringBuilder();

            bool skipFirst0s = true;
            foreach (char c in s)
            {
                if (c == '0' && skipFirst0s)
                {
                    continue;
                }
                else
                {
                    skipFirst0s = false;
                    sb.Append(c);
                }
            }

            return (sb.ToString().Length == 0) ? "0" : sb.ToString();
        }
        /*
         * Input is HexDecimalString 
         * Output is HexDecimalString 
         * 
         * 
         * 
         * June 27, 2016
         * 
         * The design has to be correct on matching comparison between A and AorB hexadecimal chars. 
         * D8   A
         * A01  AorB
         * so, D should compare to 0, not A
         * 
         * The trick is to reverse the string: 
         * 8D
         * 10A
         * then, both starting from index 0, compare one to one:
         * 8 vs 1
         * D vs 0
         * 
         * If the comparison order is not a concern. 
         */
        public static int getBitNumber_From1To0_strVersion(string valueFrom, string stringValueFromB, string valueTo, ref string newValue)
        {
            int count = 0;

            string from = Reverse(valueFrom);
            string to = Reverse(valueTo);
            string fromB = Reverse(stringValueFromB);

            StringBuilder sb_Out = new StringBuilder();

            for (int i = 0; i < from.Length; i++)
            {
                int intFrom = getInt(from[i]);

                int intTo = (i < to.Length) ? getInt(to[i]) : 0;

                StringBuilder sb = new StringBuilder();
                while (intFrom > 0)
                {
                    int firstBitFrom = intFrom & 1;
                    int firstBitTo = intTo & 1;

                    if ((firstBitFrom == 1 && firstBitTo == 0))
                    {
                        count++;
                        firstBitFrom = 0;

                    }

                    sb.Append(firstBitFrom);

                    // right shift 1 bit 
                    intFrom = intFrom >> 1;
                    intTo = intTo >> 1;
                }

                sb_Out.Append(getCharFromBinaryNumber(Reverse(sb.ToString())));
            }

            newValue = Reverse(sb_Out.ToString()); // bug001 - need to reverse the binary string 
            return count;
        }

        /*
         * if there is bits left to use, then go back to first number, and scan left to right, 
         * set first value first 1 to 0, and update second value that bit to 1 instead, 2 changes/ one time. 
         * 
         */
        public static int getFirstNumberFirst1To0_LetSecondOneTake1(
            string valueFrom,
            string stringValueFromB,
            string valueTo,
            int number,
            ref string newValue,
            ref string newValueB)
        {
            int count = 0;

            // starting from least significant bit first. 
            string from = Reverse(valueFrom);
            string to = Reverse(valueTo);
            string fromB = Reverse(stringValueFromB);

            StringBuilder sb_Out = new StringBuilder();
            StringBuilder sb_Out_B = new StringBuilder();

            for (int i = from.Length - 1; i >= 0; i--)
            {
                int intFrom = (i < from.Length) ? getInt(from[i]) : 0;
                int intTo = (i < to.Length) ? getInt(to[i]) : 0;
                int intFrom_B = (i < fromB.Length) ? getInt(fromB[i]) : 0;

                StringBuilder sb = new StringBuilder();
                StringBuilder sb_B = new StringBuilder();

                int[] arr = new int[] { 8, 4, 2, 1 };
                //while (intFrom > 0 && count < number)
                foreach (int value in arr)
                {
                    if (count >= number)
                        break;

                    int afterAnd_From = intFrom & value;
                    int afterAnd_To = intTo & value;
                    int afterAnd_FromB = intFrom_B & value;

                    if ((afterAnd_From == value && afterAnd_To == value))
                    {
                        bool change2 = number - count >= 2;

                        char[] settings = { '1', (char)(afterAnd_FromB / value + '0') };

                        if (afterAnd_FromB == value)
                        {
                            settings[0] = '0';
                            count++;
                        }
                        else if (change2)
                        {
                            settings[0] = '0';
                            settings[1] = '1';
                            count += 2;
                        }

                        sb.Append(settings[0]);
                        sb_B.Append(settings[1]);
                    }
                    else
                    {
                        sb.Append((char)(afterAnd_From / value + '0'));
                        sb_B.Append((char)(afterAnd_FromB / value + '0'));
                    }
                }

                sb_Out.Append(getCharFromBinaryNumber(sb.ToString()));
                sb_Out_B.Append(getCharFromBinaryNumber(sb_B.ToString()));
            }

            newValue = sb_Out.ToString(); // bug001 - need to reverse the binary string 
            newValueB = sb_Out_B.ToString();

            return count;
        }

        /*
         * input is string of binary number, at most 4 bits "1100"
         */
        public static char getCharFromBinaryNumber(string s)
        {
            int sum = 0;

            foreach (char c in s)
            {
                int value = (c - '0');
                sum = value + sum * 2;
            }

            if (sum >= 0 && sum <= 9)
                return (char)(sum + '0');
            else
            {
                string charStr = "ABCDEF";
                char[] charArr = charStr.ToCharArray();

                return charArr[sum - 10];
            }
        }

        public static int getInt(char c)
        {
            int number = c - '0';
            if (number >= 0 && number <= 9)
                return number;
            else
            {
                string charStr = "ABCDEF";
                char[] charArr = charStr.ToCharArray();
                int value = Array.IndexOf(charArr, c);
                return 10 + value;
            }
        }


        /*
         * http://stackoverflow.com/questions/228038/best-way-to-reverse-a-string
         */
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }


        /*
         * 2B Hexdecimal 
         * - 3 bits
         *   00101011
         *   00001000
         *   
         * 9F  - 5 bits
         *   10011111
         *   01011000
         *   
         * Final result: 
         *   01011000
         *   
         * A | B = C
         * 
         * assertion: C.Length >= A.Length, 
         *            C.Length >= B.Length
         *            
         */
        public static int getChangeBits_From0To1(string valueRef, string valueFrom, string valueTo, ref string newValue)
        {
            int count = 0;

            string from = Reverse(valueFrom);
            string to = Reverse(valueTo);
            string refStr = Reverse(valueRef);

            StringBuilder sb_Out = new StringBuilder();

            for (int i = 0; i < to.Length; i++)
            {
                int intTo = (i < valueTo.Length) ? getInt(to[i]) : 0;
                int intFrom = (i < valueFrom.Length) ? getInt(from[i]) : 0;
                int intRef = (i < valueRef.Length) ? getInt(refStr[i]) : 0;

                StringBuilder sb = new StringBuilder();

                while (intTo > 0 || intFrom > 0)
                {
                    // check first bit from rightmost 
                    int firstBitFrom = intFrom & 1;
                    int firstBitTo = intTo & 1;
                    int firstBitRef = intRef & 1;

                    if (firstBitFrom == 0 && firstBitTo == 1 && firstBitRef == 0)
                    {
                        count++;
                        firstBitFrom = 1;
                    }

                    sb.Append((char)(firstBitFrom + '0'));

                    // right shift 1 bit 
                    intTo = intTo >> 1;
                    intFrom = intFrom >> 1;
                    intRef = intRef >> 1;
                }

                sb_Out.Append(getCharFromBinaryNumber(Reverse(sb.ToString())));
            }

            newValue = Reverse(sb_Out.ToString()); // bug001 - need to reverse the binary string 
            return count;
        }
    }
}
