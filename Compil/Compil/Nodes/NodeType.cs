using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compil.Nodes
{
    public enum NodeType
    {
        VARIABLE,
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
        
        AFFECT,
        
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
        CONDITION,
        ELSE,
        FOR,
        WHILE,
        BREAK,
        LOOP,
        DO,
        SWITCH,
        CASE,
        INT,
        VOID,
        
        BLOCK,
        EXPRESSION,
        DECLARE
    }
}
