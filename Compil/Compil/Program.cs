using Compil.Utils;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Compil;
using Compil.Generator;
using System.Linq;
using Compil.LauncherVM;

namespace Compil
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // check number parameters on command
                if (args.Length == 0)
                {
                    Help();
                    Console.ReadKey();
                    return;
                }

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
                // write first command of code
                fileWriter.InitFile();

                // Display all token in form of a tree.
                var node = syntaxAnalyzer.Instruction();
                node.Print("", false);

                var codeGenerator = new CodeGenerator(fileWriter);
                codeGenerator.GenerateCode(node);
                
                // add code generate on the file output code
                fileWriter.WriteFile();

                /*
                // Launch code on VM
                var launcher = new Launcher(Path.Combine("D:\\Documents\\Projets\\Compil\\msm\\msm"), "msm");
                launcher.CopyOutFile();
                launcher.LaunchCodeOnVm();
                */

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


        /// <summary>
        /// Method to display parameters of programm on console
        /// </summary>
        public static void Help()
        {
            Console.WriteLine("Compil APP4 Capodano-Hamel");
            Console.WriteLine("Compil.exe [fileCToRead]");
            Console.WriteLine("Press a key to exit...");
        }
    }
}