using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compil.Utils;

namespace Compil
{
    class LexicalAnalyzer
    {
        private string code;
        private int index;

        private HashSet<string> keyWord = new HashSet<string> { "if", "else", "for", "while", "do", "switch", "case", "int", "void" };

        private Token CurrentNextToken;
        private int currentTokenLength = 0;
        private int CurrentLine;

        public LexicalAnalyzer(string code, int index)
        {
            this.code = code;
            this.index = index;
        }


        /// <summary>
        /// Get next token
        /// </summary>
        /// <returns></returns>
        public Token Next()
        {
            if (CurrentNextToken != null)
            {
                return CurrentNextToken;
            }

            Token result = DetectNext();
            CurrentNextToken = result;
            return result;
        }

        /// <summary>
        /// Jump next token
        /// </summary>
        public void Skip()
        {

        }

        /// <summary>
        /// if next token is searching token -> ok
        /// else error
        /// </summary>
        public void Accept()
        {
            
        }

        private Token DetectNext()
        {
            // Constants handle
            if (char.IsDigit(code[index]))
            {
                currentTokenLength = 1;
                string buffer = code[index].ToString();

                if (index == code.Length - 1)
                {
                    return new Token() { Type = TokenType.CONSTANT, Value = int.Parse(buffer) };
                }
                
                int i = index + 1;
                while (char.IsDigit(code[i]))
                {
                    buffer += code[i].ToString();
                    i++;
                    currentTokenLength++;
                }

                return new Token() { Type = TokenType.CONSTANT, Value = int.Parse(buffer) };
            }
            
            // Identifier handle
            if (char.IsLetter(code[index]))
            {
                currentTokenLength = 1;
                string buffer = code[index].ToString();

                if (index == code.Length - 1)
                {
                    return new Token() { Type = TokenType.CONSTANT, Value = int.Parse(buffer) };
                }
                
                int i = index + 1;
                while (char.IsLetter(code[i]) || char.IsDigit(code[i]))
                {
                    buffer += code[i].ToString();
                    i++;
                    currentTokenLength++;
                }

                return new Token() { Type = TokenType.CONSTANT, Value = int.Parse(buffer) };
            }

            currentTokenLength++;
            switch (code[index])
            {
                case '+':
                    return new Token() { Type = TokenType.OP_PLUS };
                case '-':
                    return new Token() { Type = TokenType.OP_MINUS };
                case '*':
                    return new Token() { Type = TokenType.OP_MULTIPLY };
                case '/':
                    return new Token() { Type = TokenType.OP_DIVIDE };
                case '%':
                    return new Token() { Type = TokenType.OP_MODULO };
                case '^':
                    return new Token() { Type = TokenType.OP_POWER };
                case '(':
                    return new Token() { Type = TokenType.PAR_OPEN };
                case ')':
                    return new Token() { Type = TokenType.PAR_CLOSE };
                case ' ':
                case '\n':
                    // fin du token
                    break;
                default:
                    break;
            }

            throw new NotImplementedException();
        }
    }
}
