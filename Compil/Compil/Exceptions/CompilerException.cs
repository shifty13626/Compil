using System;
using System.Linq.Expressions;

namespace Compil.Exceptions {
    public abstract class CompilerException : Exception {
        public int Line { get; set; }
        public int Column { get; set; }

        public CompilerException(string message) : base(message) {
        }
    }
}