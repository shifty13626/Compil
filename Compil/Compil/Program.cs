using Compil.Utils;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Compil;
using Compil.Generator;
using System.Linq;
using Compil.Tokens;

namespace Compil
{
    static class Program
    {
        static void Main(string[] args)
        {
            try {
                // check number parameters on command
                if (args.Length == 0) {
                    Help();
                    return;
                }

                // read source file
                var pathFile = Path.Combine(args[0]);
                Console.WriteLine("File to read : " + pathFile);
                var outputFile = Path.Combine(args[args.Length - 1]);
                Console.WriteLine("Output file : " + outputFile);
                var codeTemp = File.ReadAllText(pathFile);

                Console.WriteLine("File content : ");
                Console.WriteLine(codeTemp);

                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                Console.WriteLine();

                // lexicalAnalyser
                var lexicalAnalyzer = new LexicalAnalyzer(codeTemp, 0);
                // parserAnalyzer
                var syntaxAnalyzer = new SyntaxAnalyzer(lexicalAnalyzer);
                // File writer
                var fileWriter = new FileWriter(outputFile);

                // Display all token in form of a tree.
                var semanticAnalyzer = new SemanticAnalyzer(syntaxAnalyzer);
                var codeGenerator = new CodeGenerator(semanticAnalyzer, fileWriter);

                while (lexicalAnalyzer.Next().Type != TokenType.END_OF_FILE) {
                    var node = syntaxAnalyzer.Function();
                    semanticAnalyzer.Analyze(node);
                    node.VariablesCount = semanticAnalyzer.VariablesCount;

                    node.Print("", false);
                    codeGenerator.GenerateCode(node);
                    semanticAnalyzer = new SemanticAnalyzer(syntaxAnalyzer);
                }

                // add code generate on the file output code
                fileWriter.DeclareStart();
                fileWriter.WriteFile();

                // wait exit
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
            } catch (EncoderFallbackException e) {
                Console.WriteLine(e.StackTrace);
            } catch (ArgumentNullException e) {
                Console.WriteLine("Null argument enter for launch program");
                Console.WriteLine(e.Message);
            }
        }


        /// <summary>
        /// Method to display parameters of program on console
        /// </summary>
        public static void Help()
        {
            Console.WriteLine("Compil APP4 Capodano-Hamel");
            Console.WriteLine("Compil.exe [fileCToRead]");
            Console.WriteLine("Press a key to exit...");
            Console.ReadKey();
            return;
        }
    }
}