using Compil.Utils;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Compil;
using Compil.Generator;

namespace Compil
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                    throw new ArgumentNullException("Null arguments");

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


                // Display all token in form of a tree.
                var node = syntaxAnalyzer.Instruction();
                node.Print("", false);

                var codeGenerator = new CodeGenerator(fileWriter, true);
                codeGenerator.GenerateCode(node);
                
                fileWriter.WriteFile();

                // wait exit
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
            }
            catch (EncoderFallbackException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("Null argument enter for launch programm");
                Console.WriteLine(e.Message);
            }
        }
    }
}