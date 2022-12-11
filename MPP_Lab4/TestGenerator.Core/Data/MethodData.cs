using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TestGenerator.Core.Data;

public class MethodData
{
    public ClassDeclarationSyntax ClassDeclarationSyntax { get; set; }
    public UsingDirectiveSyntax UsingDirectiveSyntax { get; set; }
}