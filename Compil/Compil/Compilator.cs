using System.CodeDom.Compiler;
using Compil.Utils;
using Compil.Generator;
using CodeGenerator = Compil.Generator.CodeGenerator;

namespace Compil
{
    public class Compilator
    {
        public void Compile(string inputCode, string outputPath)
        {
            var lexicalAnalyser = new LexicalAnalyzer(inputCode, 0);
            var syntaxAnalyzer = new SyntaxAnalyzer(lexicalAnalyser);
            var fileWriter = new FileWriter();
            var node = syntaxAnalyzer.Expression(0);
            var codeGenerator = new CodeGenerator(fileWriter);
            codeGenerator.GenerateCode(node);
        }
    }
}