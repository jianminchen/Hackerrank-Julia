using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradingStudents
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = Convert.ToInt16(Console.ReadLine());

            for (int i = 0; i < n; i++)
            {
                int grade = Convert.ToInt16(Console.ReadLine());
                Console.WriteLine(RoundUp(grade));
            }

        }

        /*
         * less than 38 - no round up 
         * 38 - 39 -> 40
         * 84 -> 85 
         * 83 -> 85 
         * 82 -> stay at 82 
         */
        public static int RoundUp(int grade)
        {
            if (grade < 38)
            {
                return grade;
            }

            int residue = grade - (grade / 5) * 5;
            if (residue <= 2)
            {
                return grade;
            }

            return (grade + 5) / 5 * 5;
        }
    }
}
