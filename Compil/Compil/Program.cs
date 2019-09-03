using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compil
{
    class Program
    {
        static void Main(string[] args)
        {
            string code = "";
            try
            {
                // check file is .c extention
                if (!args[args.Length - 1].Contains(".c"))
                    throw new EncoderFallbackException();

                // read file
                code = File.ReadAllText(args[args.Length - 1]);
                Console.WriteLine("code file : " + code);

                // lexicalAnalyser
                var lexicalAnalyser = new LexicalAnalyser(code, 0);


            }
            catch(EncoderFallbackException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
