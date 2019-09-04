using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compil.Utils;
using Compil.Exceptions;

namespace Compil
{
    class LexicalAnalyser
    {
        private string code;
        private int index;

        private HashSet<string> keyWord = new HashSet<string> { "if", "else", "for", "while", "do", "switch", "case", "int", "void" };

        private Token CurrentNextToken;

        public LexicalAnalyser(string code, int index)
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
        public void Accept(TokenType type)
        {
            if (Next().Type == type)
                Skip();
            else
                throw new NotValidCharException();
        }

        private Token DetectNext()
        {

            // Constants handle
            if (Char.IsDigit(code[index]))
            {
                string buffer = code[index].ToString();

                if (index == code.Length - 1)
                {
                    return new Token() { Type = TokenType.CONSTANT, Value = int.Parse(buffer) };
                }

                int i = index + 1;
                while (Char.IsDigit(code[i]))
                {
                    buffer += code[i].ToString();
                    i++;
                }

                return new Token() { Type = TokenType.CONSTANT, Value = int.Parse(buffer) };
            }

            // Binarie operator
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
                case ' ':
                case '\n':
                    // fin du token
                    break;
            }

            throw new NotImplementedException();
        }
    }
}
