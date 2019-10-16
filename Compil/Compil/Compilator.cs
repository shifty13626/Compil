using System.CodeDom.Compiler;
using CodeGenerator = Compil.Generator.CodeGenerator;

namespace Compil
{
    public class Compilator
    {
        public void Compile(string inputCode, string outputPath)
        {
            var lexicalAnalyser = new LexicalAnalyzer(inputCode, 0);
            var syntaxAnalyzer = new SyntaxAnalyzer(lexicalAnalyser);
            var node = syntaxAnalyzer.Expression(0);
            var codeGenerator = new CodeGenerator();
            codeGenerator.GenerateCode(node);
        }
    }
}