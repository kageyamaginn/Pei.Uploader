using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string han = "afdasfd";
            Console.WriteLine( R<String>());
        }

        public static TR R<TR>()
        {
            string hh = "adfasdfads";
            return (TR)(hh as object);
        }
    }
}
