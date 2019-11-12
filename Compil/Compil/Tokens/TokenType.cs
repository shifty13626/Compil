namespace Compil.Tokens
{
    public enum TokenType
    {
        IDENTIFIER,
        CONSTANT,

        // Special
        END_OF_FILE,

        NOT,

        PLUS,
        MINUS,
        MULTIPLY,
        DIVIDE,
        MODULO,
        POWER,

        IF,
        ELSE,
        FOR,
        WHILE,
        DO,
        SWITCH,
        CASE,
        INT,
        VOID,
        BREAK,
        CONTINUE,

        COMP_EQUAL,
        COMP_DIFFERENT,
        COMP_SUPPERIOR,
        COMP_INFERIOR,
        COMP_SUPPERIOR_OR_EQUAL,
        COMP_INFERIOR_OR_EQUAL,

        PAR_OPEN,
        PAR_CLOSE,

        OR,
        AND,

        EQUAL,

        // Brackets
        BRACKET_OPEN,
        BRACKET_CLOSE,

        SEMICOLON,
        COMA,
        COMMENT_LINE,
        COMMENT_BLOCK_START,
        COMMENT_BLOCK_END,
        
        VAR,
        
        FUNCTION,
        RETURN
    }
}
