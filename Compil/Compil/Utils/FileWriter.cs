using System;
using System.IO;

namespace Compil.Utils
{
    /// <summary>
    /// Class to write programm generate by the compilator on the file to execut it on VM
    /// </summary>
    public class FileWriter
    {
        private readonly string pathFileCode;
        private string code;

        /// <summary>
        /// Constructor class
        /// </summary>
        public FileWriter()
        {
            code = String.Empty;
            pathFileCode = Path.Combine(Environment.CurrentDirectory, "code.txt");
        }

        /// <summary>
        /// Method to write a line on the file
        /// </summary>
        /// <param name="cmd"></param>
        public void WriteCommand(string cmd, bool debug = false)
        {
            code += cmd + "\n";
            if (debug)
            {
                code += "dup" + "\n";
                code += "dbg" + "\n";
            }
        }

        /// <summary>
        /// Write label of new function
        /// </summary>
        /// <param name="name"></param>
        public void DeclareFunction(string name)
        {
            code += "\n";
            code += "." + name + "\n";
        }

        /// <summary>
        /// Write block start launcher on code generated
        /// </summary>
        public void DeclareStart()
        {
            DeclareFunction("start");
            code += "prep main" + "\n";
            code += "call 0" + "\n";
            code += "halt";
        }


        /// <summary>
        /// Write content on string on file result code
        /// </summary>
        public void WriteFile()
        {
            try
            {
                code += "dbg\n";
                code += "halt";
                if (File.Exists(pathFileCode))
                    File.Delete(pathFileCode);

                Console.WriteLine();
                Console.WriteLine("Code generated :");
                Console.WriteLine(code);

                File.WriteAllText(pathFileCode, code);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
