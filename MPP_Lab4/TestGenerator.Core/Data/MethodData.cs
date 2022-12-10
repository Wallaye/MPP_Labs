using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TestGenerator.Core.Data;

public class MethodData
{
    public ClassDeclarationSyntax ClassDeclarationSyntax { get; }
    public UsingDirectiveSyntax UsingDirectiveSyntax { get; }
}