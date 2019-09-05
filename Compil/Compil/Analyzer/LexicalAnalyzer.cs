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
        public int index;
        
        public Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>()
        {
            {"if", TokenType.IF},
            {"else", TokenType.ELSE},
            {"for", TokenType.FOR},
            {"while", TokenType.WHILE},
            {"do", TokenType.DO},
            {"switch", TokenType.SWITCH},
            {"case", TokenType.CASE},
            {"int", TokenType.INT},
            {"void", TokenType.VOID}
        };
        
        
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
            index += currentTokenLength;
            currentTokenLength = 0;
            CurrentNextToken = null;
        }

        /// <summary>
        /// if next token is searching token -> ok
        /// else error
        /// </summary>
        public void Accept(TokenType type)
        {
            if (Next().Type != type)
            {
                throw new Exception("Bad token");
            }
            Skip();
        }

        private Token DetectNext()
        {

            if (index == code.Length)
            {
                return new Token() { Type = TokenType.END_OF_FILE };
            }
            
            while (code[index] == ' ' || code[index] == '\t' || code[index] == '\n')
            {
                index++;
            }

            // Constants handle
            if (char.IsDigit(code[index]))
            {
                currentTokenLength = 1;
                string buffer = code[index].ToString();

                if (index == code.Length - 1)
                {
                    return new Token() { Type = TokenType.TOK_CONST, Value = int.Parse(buffer) };
                }
                
                int i = index + 1;
                while (char.IsDigit(code[i]))
                {
                    buffer += code[i].ToString();
                    i++;
                    currentTokenLength++;
                }

                return new Token() { Type = TokenType.TOK_CONST, Value = int.Parse(buffer) };
            }
            
            // Identifier and keywords handle
            if (char.IsLetter(code[index]))
            {
                currentTokenLength = 1;
                string buffer = code[index].ToString();

                if (index == code.Length - 1)
                {
                    // Look into keywords dictionnary to get the adequate token type
                    if (keywords.ContainsKey(buffer))
                    {
                        return new Token() { Type = keywords[buffer], Name = buffer };
                    }
                    
                    return new Token() { Type = TokenType.IDENTIFIER, Name = buffer };
                }
                
                int i = index + 1;
                while (char.IsLetter(code[i]) || char.IsDigit(code[i]))
                {
                    buffer += code[i].ToString();
                    i++;
                    currentTokenLength++;
                }

                // Look into keywords dictionnary to get the adequate token type
                if (keywords.ContainsKey(buffer))
                {
                    return new Token() { Type = keywords[buffer], Name = buffer };
                }
                    
                return new Token() { Type = TokenType.IDENTIFIER, Name = buffer };
            }
            
            // ==
            if (code[index] == '=')
            {
                currentTokenLength = 1;
                string buffer = code[index].ToString();
                
                if (index == code.Length - 1)
                {
                    return new Token() { Type = TokenType.AFFECT_EQUAL };
                }

                if (code[index + 1] == '=')
                {
                    currentTokenLength++;
                    return new Token() { Type = TokenType.COMP_EQUAL };
                }

                return new Token() { Type = TokenType.AFFECT_EQUAL };
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
                case '{':
                    return new Token() { Type = TokenType.BLOCK_START };
                case '}':
                    return new Token() { Type = TokenType.BLOCK_END };
                case '&':
                    return new Token() { Type = TokenType.BOOL_AND };
                case '|':
                    return new Token() { Type = TokenType.BOOL_OR };
                case ';':
                    return new Token() { Type = TokenType.SEMICOLON };
                default:
                    break;
            }

            throw new NotImplementedException();
        }
    }
}
