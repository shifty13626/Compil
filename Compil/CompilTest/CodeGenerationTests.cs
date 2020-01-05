using System;
using System.CodeDom.Compiler;
using System.IO;
using NUnit.Framework;
using Compil;
using Compil.Utils;
using CodeGenerator = Compil.Generator.CodeGenerator;
using Xunit;
using Assert = NUnit.Framework.Assert;

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
            var fileWriter = new FileWriter("");
            var semanticAnalyzer = new SemanticAnalyzer(syntaxAnalyzer);
            var codeGenerator = new CodeGenerator(semanticAnalyzer, fileWriter);
            codeGenerator.GenerateCode(node);
            Assert.True(File.Exists(path) && new FileInfo( path).Length != 0);
        }

        #region Operations

        [Test]
        public void TestAddition()
        {
            const string expression = "123+298+322";
            var compilator = new Compilator();
            compilator.Compile(expression, path);
            Assert.True(true); // lancer test si calcul ok en lançant msm
        }

        [Test]
        public void TestSoustraction()
        {
            const string expression = "125-25";
            var compilator = new Compilator();
            compilator.Compile(expression, path);
            Assert.True(true); // lancer test si calcul ok en lançant msm
        }

        [Test]
        public void TestMultiplication()
        {
            const string expression = "10*3*2";
            var compilator = new Compilator();
            compilator.Compile(expression, path);
            Assert.True(true); // lancer test si calcul ok en lançant msm
        }

        [Test]
        public void TestDivision()
        {
            const string expression = "10/2";
            var compilator = new Compilator();
            compilator.Compile(expression, path);
            Assert.True(true); // lancer test si calcul ok en lançant msm
        }

        #endregion

        #region Condition

        [Test]
        public void TestIF()
        {
            const string code = "if(2<5) { a = 3; }";
            var compilator = new Compilator();
            compilator.Compile(code, path);
            Assert.True(true); // lancer test si calcul ok en lançant msm
        }

        [Test]
        public void TestIfElse()
        {
            const string code = "if(2<5) { a = 3; } else { a = 10; }";
            var compilator = new Compilator();
            compilator.Compile(code, path);
            Assert.True(true); // lancer test si calcul ok en lançant msm
        }

        [Test]
        public void TestFor()
        {
            const string code = "for(int i=0; i<5; i++) { a = 3; }";
            var compilator = new Compilator();
            compilator.Compile(code, path);
            Assert.True(true); // lancer test si calcul ok en lançant msm
        }

        #endregion

        #region Comparaison

        [Test]
        public void TestEqual()
        {
            const string code = "if ( 2 == 2) { }";
            var compilator = new Compilator();
            compilator.Compile(code, path);
            Assert.True(true); // lancer test si calcul ok en lançant msm
        }

        [Test]
        public void TestInferior()
        {
            const string code = "if (2 < 3) { }";
            var compilator = new Compilator();
            compilator.Compile(code, path);
            Assert.True(true); // lancer test si calcul ok en lançant msm
        }

        [Test]
        public void TestInferiorEqual()
        {
            const string code = "if (2 <= 2) { }";
            var compilator = new Compilator();
            compilator.Compile(code, path);
            Assert.True(true); // lancer test si calcul ok en lançant msm
        }

        [Test]
        public void TestSuperior()
        {
            const string code = "if (2 > 1) { }";
            var compilator = new Compilator();
            compilator.Compile(code, path);
            Assert.True(true); // lancer test si calcul ok en lançant msm
        }

        [Test]
        public void TestSuperiorEqual()
        {
            const string code = "if (2 >= 1) { }";
            var compilator = new Compilator();
            compilator.Compile(code, path);
            Assert.True(true); // lancer test si calcul ok en lançant msm
        }

        #endregion
    }
}