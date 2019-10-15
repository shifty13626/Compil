using Compil.Utils;
using System;
using System.IO;
using System.Text;

namespace Compil
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // read source file
                Console.WriteLine("File to read : " + args[args.Length - 1]);
                string pathFile = Path.Combine(args[args.Length - 1]);
                string codeTemp = File.ReadAllText(pathFile);

                Console.WriteLine("File content : ");
                Console.WriteLine(codeTemp);

                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                Console.WriteLine();

                // lexicalAnalyser
                var lexicalAnalyser = new LexicalAnalyzer(codeTemp, 0);
                // parserAnalyzer
                var syntaxAnalyzer = new SyntaxAnalyzer(lexicalAnalyser);
                // File writer
                var fileWriter = new FileWriter();

                fileWriter.InitFile();
                fileWriter.WriteFile();


                // Display all token in form of a tree.
                var node = syntaxAnalyzer.Expression(0);
                node.Print("", false);

                Console.WriteLine();

                // wait exit
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
            }
            catch (EncoderFallbackException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}