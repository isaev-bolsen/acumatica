using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acumatica
    {
    class Program
        {
        static void Main(string[] args)
            {
            WebMailDriver wmd = new WebMailDriver();
            wmd.Auth();
            Console.WriteLine(wmd.sendMessage());

            
            }
        }
    }
