namespace Compil.Exceptions {
    public class SyntaxErrorException : CompilerException {
        public SyntaxErrorException(string message) : base(message) {
        }
    }
}