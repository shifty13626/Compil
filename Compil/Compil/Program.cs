using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compil.Utils;
using Compil.Analyzer;

namespace Compil
{
    class Program
    {
        static void Main(string[] args)
        {
            string code = @"void main(int azerty) { int b = baba + 3; a =            123-543; }";

            try
            {
                // read source file
                Console.WriteLine("File to read : " + args[args.Length - 1]);
                string pathFile = Path.Combine(args[args.Length - 1]);
                string codeTemp = File.ReadAllText(pathFile);
                Console.WriteLine("Contenu fichier : ");
                Console.WriteLine(codeTemp);


                Console.WriteLine("code file : " + code);
                Console.WriteLine("Press key to continue.");
                Console.ReadKey();
                Console.WriteLine();

                // lexicalAnalyser
                var lexicalAnalyser = new LexicalAnalyzer(code, 0);
                // parserAnalyzer
                var parserAnalyzer = new Parser(lexicalAnalyser);

                Node node;
                while (lexicalAnalyser.Next().Type != TokenType.END_OF_FILE)
                {
                    Console.WriteLine(lexicalAnalyser.Next().Type + " (" + parserAnalyzer.Primary().Type + " / " + parserAnalyzer.Primary().Value + ") -> ");
                    lexicalAnalyser.Skip();
                }
                Console.Write(lexicalAnalyser.Next().Type);

                Console.WriteLine();

                // wait exit
                Console.WriteLine("\nPress key to exit.");
                Console.ReadKey();
            }
            catch(EncoderFallbackException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
