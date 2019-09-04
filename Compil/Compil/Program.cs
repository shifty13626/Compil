using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compil.Utils;

namespace Compil
{
    class Program
    {
        static void Main(string[] args)
        {
            string code = @"void main(int azerty) { int b = baba + 3; a =            123-543; }";

            try
            {
                Console.WriteLine("code file : " + code);
                Console.ReadKey();

                // lexicalAnalyser
                var lexicalAnalyser = new LexicalAnalyzer(code, 0);

                while (lexicalAnalyser.Next().Type != TokenType.END_OF_FILE)
                {
                    Console.Write(lexicalAnalyser.Next().Type + " -> ");
                    lexicalAnalyser.Skip();
                }
                Console.Write(lexicalAnalyser.Next().Type);
                
            }
            catch(EncoderFallbackException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
