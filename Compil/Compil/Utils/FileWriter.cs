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
        /// Write first command start
        /// </summary>
        public void InitFile()
        {
            Console.WriteLine(".start");
            code += ".start" + "\n";
        }


        /// <summary>
        /// Method to write a line on the file
        /// </summary>
        /// <param name="cmd"></param>
        public void WriteCommand(string cmd, bool debug = false)
        {
            Console.WriteLine(cmd);
            code += cmd + "\n";
            if (debug)
            {
                code += "dbg\n";
                Console.WriteLine("dbg");
            }
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
