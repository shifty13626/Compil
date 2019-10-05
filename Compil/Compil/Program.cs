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
            try
            {
                // read source file
                Console.WriteLine("File to read : " + args[args.Length - 1]);
                string pathFile = Path.Combine(args[args.Length - 1]);
                string codeTemp = File.ReadAllText(pathFile);
                Console.WriteLine("Contenu fichier : ");
                Console.WriteLine(codeTemp);

                Console.WriteLine("Press key to continue.");
                Console.ReadKey();
                Console.WriteLine();

                // lexicalAnalyser
                var lexicalAnalyser = new LexicalAnalyzer(codeTemp, 0);
                // parserAnalyzer
                var syntaxAnalyzer = new SyntaxAnalyzer(lexicalAnalyser);

                // display all token
                while (lexicalAnalyser.Next().Type != TokenType.END_OF_FILE)
                {
                    Console.WriteLine(lexicalAnalyser.Next().Type + " (" + syntaxAnalyzer.Primary().Type + " / " + syntaxAnalyzer.Primary().Value + ") -> ");
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
