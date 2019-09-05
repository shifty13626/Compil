using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compil.Utils
{
    public enum NodeType
    {
        NODE_CONST,
        NODE_MINUS,
        NODE_PLUS,
        NODE_NOT,
        NODE_OP_PLUS,
        NODE_OP_MINUS,
        NODE_OP_MULTIPLY,
        NODE_OP_DIVIDE,
        NODE_OP_MODULO,
        NODE_OP_POWER,
        NODE_COMP_EQUAL,
        NODE_COMP_NOT,
        NODE_COMP_SUPPERIOR,
        NODE_COMP_INFERIOR,
        NODE_COMP_SUPPERIOR_OR_EQUAL,
        NODE_COMP_INFERIOR_OR_EQUAL,
        NODE_AND,
        NODE_OR,
        NODE_IF,
        NODE_ELSE,
        NODE_FOR,
        NODE_WHILE,
        NODE_DO,
        NODE_SWITCH,
        NODE_CASE,
        NODE_INT,
        NODE_VOID
    }
}
