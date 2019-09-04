using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compil.Utils
{
    public enum TokenType
    {
        TOK_CONST,
        MINUS,
        PLUS,
        NOT,
        IDENTIFIER,
        END_OF_FILE,
        BEGIN_OF_FILE,
        IF,
        ELSE,
        FOR,
        WHILE,
        DO,
        SWITCH,
        CASE,
        INT,
        VOID,
        OP_PLUS,
        OP_MINUS,
        OP_MULTIPLY,
        OP_DIVIDE,
        OP_MODULO,
        OP_POWER,
        COMP_EQUAL,
        COMP_DIFFERENT,
        COMP_SUPPERIOR,
        COMP_INFERIOR,
        COMP_SUPPERIOR_OR_EQUAL,
        COMP_INFERIOR_OR_EQUAL,
        PAR_OPEN,
        PAR_CLOSE,
        BOOL_OR,
        BOOL_AND,
        AFFECT_EQUAL,
        BLOCK_START,
        BLOCK_END,
        SEMICOLON,
        COMA,
        COMMENT_LINE,
        COMMENT_BLOCK_START,
        COMMENT_BLOCK_END
    }
}
