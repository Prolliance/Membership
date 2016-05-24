using System;
using System.Diagnostics;

namespace Prolliance.Membership.Test.Win
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch wacth = new Stopwatch();
            wacth.Start();



            Console.WriteLine("用时: " + wacth.ElapsedMilliseconds);
            Console.Read();
        }
    }
}