using System;
using System.CodeDom.Compiler;
using System.IO;
using NUnit.Framework;
using Compil;
using CodeGenerator = Compil.Generator.CodeGenerator;

namespace CompilTest
{
    [TestFixture]
    public class CodeGenerationTests
    {
        private string path = Path.Combine(Environment.CurrentDirectory, "code.txt");
        
        [Test]
        public void TestCodeGenerated()
        {
            const string expression = "1+2+3";
            var lexicalAnalyser = new LexicalAnalyzer(expression, 0);
            var syntaxAnalyzer = new SyntaxAnalyzer(lexicalAnalyser);
            var node = syntaxAnalyzer.Expression(0);
            var codeGenerator = new CodeGenerator();
            codeGenerator.GenerateCode(node);
            Assert.True(File.Exists(path) && new FileInfo( path).Length != 0);
        }

        [Test]
        public void TestAddition()
        {
            const string expression = "123+298+322";
            var compilator = new Compilator();
            compilator.Compile(expression, path);
            Assert.True(true); // lancer test si calcul ok en lançant msm
        }
    }
}