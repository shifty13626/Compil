using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compil.Utils;
using Compil;

namespace Compil {
    class Program {
        public static void Main(string[] args) {
            try {
                // read source file
                Console.WriteLine("File to read : " + args[args.Length - 1]);
                var pathFile = Path.Combine(args[args.Length - 1]);
                var codeTemp = File.ReadAllText(pathFile);
                
                Console.WriteLine("Contenu fichier : ");
                Console.WriteLine(codeTemp);

                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                Console.WriteLine();

                // lexicalAnalyser
                var lexicalAnalyser = new LexicalAnalyzer(codeTemp, 0);
                // parserAnalyzer
                var syntaxAnalyzer = new SyntaxAnalyzer(lexicalAnalyser);

                // Display all token in form of a tree.
                var node = syntaxAnalyzer.Expression(0);
                node.Print("", false);
                
                Console.WriteLine();

                // wait exit
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
            } catch (EncoderFallbackException e) {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}