namespace TestGenerator.Core.Exceptions;

public class GeneratorOptionsException : Exception
{
    public string WrongDir { get; }
    public GeneratorOptionsException(string Message, string path) : base(Message)
    {
        WrongDir = path;
    }
}