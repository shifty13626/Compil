namespace Compil.Utils
{
    public enum NodeType
    {
        IDENTIFIER,
        CONSTANT,

        // Operators
        MINUS,
        PLUS,
        NOT,

        OP_PLUS,
        OP_MINUS,
        OP_MULTIPLY,
        OP_DIVIDE,
        OP_MODULO,
        OP_POWER,

        // Comparison
        COMP_EQUAL,
        COMP_DIFFERENT,
        COMP_SUPPERIOR,
        COMP_INFERIOR,
        COMP_SUPPERIOR_OR_EQUAL,
        COMP_INFERIOR_OR_EQUAL,

        // Boolean
        AND,
        OR,
        IF,
        ELSE,
        FOR,
        WHILE,
        DO,
        SWITCH,
        CASE,
        INT,
        VOID
    }
}
