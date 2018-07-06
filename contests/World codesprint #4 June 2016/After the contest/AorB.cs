using System;
using System.Collections.Generic;
using System.IO;
class Solution
{
    static void Main(String[] args)
    {
        new A_or_B().Solve();
    }
}

class A_or_B
{
    public void Solve()
    {
        var n = int.Parse(Console.ReadLine());

        while (--n >= 0)
        {
            var k = int.Parse(Console.ReadLine());
            var a = ToBits(Console.ReadLine());
            var b = ToBits(Console.ReadLine());
            var c = ToBits(Console.ReadLine());

            if (!Modify(k, a, b, c))
            {
                Console.WriteLine(-1);
            }
            else
            {
                Console.WriteLine(ToString(a));
                Console.WriteLine(ToString(b));
            }
        }
    }

    string ToString(int[] a)
    {
        var c = "0123456789ABCDEF";
        var s = new char[a.Length >> 2];
        var j = 0;

        for (var i = 0; i < a.Length; i += 4)
        {
            s[i >> 2] = c[(a[i] << 3) | (a[i + 1] << 2) | (a[i + 2] << 1) | (a[i + 3])];
        }

        for (; j + 1 < s.Length && s[j] == '0'; ++j) ;

        return new string(s, j, s.Length - j);
    }

    bool Modify(int k, int[] a, int[] b, int[] c)
    {
        var n = a.Length;

        for (var i = 0; i < n; ++i)
        {
            if (c[i] == 0)
            {
                if (a[i] == 1)
                {
                    if (k == 0)
                    {
                        return false;
                    }

                    a[i] = 0;
                    --k;
                }

                if (b[i] == 1)
                {
                    if (k == 0)
                    {
                        return false;
                    }

                    b[i] = 0;
                    --k;
                }
            }
            else if (a[i] == 0 && b[i] == 0)
            {
                if (k == 0)
                {
                    return false;
                }

                b[i] = 1;
                --k;
            }
        }

        for (var i = 0; i < n && k > 0; ++i)
        {
            if (a[i] == 1)
            {
                if (b[i] == 1)
                {
                    a[i] = 0;
                    --k;
                }
                else if (k > 1)
                {
                    a[i] = 0;
                    b[i] = 1;
                    k -= 2;
                }
            }
        }

        return true;
    }

    int[] ToBits(string s)
    {
        var a = new int[200004];

        for (int i = 0, j = a.Length - 4 * s.Length; i < s.Length; ++i, j += 4)
        {
            switch (s[i])
            {
                case '0':
                    {
                        break;
                    }
                case '1':
                    {
                        a[j + 3] = 1;
                        break;
                    }
                case '2':
                    {
                        a[j + 2] = 1;
                        break;
                    }
                case '3':
                    {
                        a[j + 2] = 1;
                        a[j + 3] = 1;
                        break;
                    }
                case '4':
                    {
                        a[j + 1] = 1;
                        break;
                    }
                case '5':
                    {
                        a[j + 1] = 1;
                        a[j + 3] = 1;
                        break;
                    }
                case '6':
                    {
                        a[j + 1] = 1;
                        a[j + 2] = 1;
                        break;
                    }
                case '7':
                    {
                        a[j + 1] = 1;
                        a[j + 2] = 1;
                        a[j + 3] = 1;
                        break;
                    }
                case '8':
                    {
                        a[j + 0] = 1;
                        break;
                    }
                case '9':
                    {
                        a[j + 0] = 1;
                        a[j + 3] = 1;
                        break;
                    }
                case 'A':
                    {
                        a[j + 0] = 1;
                        a[j + 2] = 1;
                        break;
                    }
                case 'B':
                    {
                        a[j + 0] = 1;
                        a[j + 2] = 1;
                        a[j + 3] = 1;
                        break;
                    }
                case 'C':
                    {
                        a[j + 0] = 1;
                        a[j + 1] = 1;
                        break;
                    }
                case 'D':
                    {
                        a[j + 0] = 1;
                        a[j + 1] = 1;
                        a[j + 3] = 1;
                        break;
                    }
                case 'E':
                    {
                        a[j + 0] = 1;
                        a[j + 1] = 1;
                        a[j + 2] = 1;
                        break;
                    }
                case 'F':
                    {
                        a[j + 0] = 1;
                        a[j + 1] = 1;
                        a[j + 2] = 1;
                        a[j + 3] = 1;
                        break;
                    }
                default:
                    {
                        Console.WriteLine(s[i]);
                        throw new Exception("" + s[i]);
                    }
            }
        }

        return a;
    }
}