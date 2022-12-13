using System.Threading.Tasks.Dataflow;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TestGenerator.Core.Data;
using TestGenerator.Core.Exceptions;
using TestGenerator.Core.Generator;

namespace TestGenerator.Core;

public static class TestsGenerator
{
    public static GeneratorOptions GeneratorOptions { get; set; }
    public static Task Generate()
    {
        var readDirectoryBlock = ReadDirectoryBlock();
        var readFileBlock = ReadFileBlock();
        var splitClassesBlock = SplitClassesBlock();
        var generateTestsBlock = GenerateTestsBlock();
        var writeTestsBlock = WriteTestsBlock();
        
        var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
        readDirectoryBlock.LinkTo(readFileBlock, linkOptions);
        readFileBlock.LinkTo(splitClassesBlock, linkOptions);
        splitClassesBlock.LinkTo(generateTestsBlock, linkOptions);
        generateTestsBlock.LinkTo(writeTestsBlock, linkOptions);

        readDirectoryBlock.Post(GeneratorOptions.SourceDir);
        readDirectoryBlock.Complete();

        return writeTestsBlock.Completion;
    }
    
    private static TransformManyBlock<string, string> ReadDirectoryBlock()
    {
        return new TransformManyBlock<string, string>(path =>
        {
            if (!Directory.Exists(path))
            {
                throw new GeneratorOptionsException("No such source directory", GeneratorOptions.SourceDir);
            }

            return Directory.EnumerateFiles(path, "*.cs");
        });
    }

    private static TransformBlock<string, string> ReadFileBlock()
    {
        return new TransformBlock<string, string>(async fileName =>
        {
            if (!File.Exists(fileName))
            {
                throw new ArgumentException("No such file");
            }

            string result = "";
            using (var file = File.OpenText(fileName))
            {
                result = await file.ReadToEndAsync();
            }

            return result;
        }, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = GeneratorOptions.MaxThreadsToRead});
    }

    private static TransformManyBlock<string, MethodData> SplitClassesBlock()
    {
        return new TransformManyBlock<string, MethodData>(GetClasses);
    }

    private static IEnumerable<MethodData> GetClasses(string code)
    {
        var root = CSharpSyntaxTree.ParseText(code).GetRoot();
        return root.DescendantNodes().OfType<ClassDeclarationSyntax>().Select(c =>
        {
            return new MethodData()
            {
                ClassDeclarationSyntax = c,
                UsingDirectiveSyntax = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(
                    root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().First().Name.ToFullString()))
            };
        });
    }

    private static TransformBlock<MethodData, TestFile> GenerateTestsBlock()
    {
        return new TransformBlock<MethodData, TestFile>(GenerateTests);
    }

    private static TestFile GenerateTests(MethodData arg)
    {
        var classDeclaration = arg.ClassDeclarationSyntax;
        var methods = classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>()
            .Where(m => m.Modifiers.Any(mod => mod.IsKind(SyntaxKind.PublicKeyword)));
        var objectName = classDeclaration.Identifier.Text + "Object";
        var methodDeclarations = methods.Select(m =>
        {
            var hasParams = m.ParameterList.Parameters.Count > 0;
            List<LocalDeclarationStatementSyntax> vars = new();
            ArgumentListSyntax args = SyntaxFactory.ArgumentList();
            if (hasParams)
            {
                    int n = m.ParameterList.Parameters.Count;

                    foreach (var p in m.ParameterList.Parameters)
                    {
                        n--;
                        if (n > 0)
                        {
                            args = args.AddArguments(SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                    new SyntaxNodeOrToken[]{
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName(p.Identifier.Text)),
                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                        }).ToArray());
                        }
                        else
                        {
                            args = args.AddArguments(SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                    new SyntaxNodeOrToken[]{
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName(p.Identifier.Text)),
                                                        }).ToArray());
                        }
                    }

                    vars = m.ParameterList.Parameters.Select(p =>
                    {
                        return SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            p.Type)
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier(p.Identifier.Text))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.DefaultLiteralExpression,
                                            SyntaxFactory.Token(SyntaxKind.DefaultKeyword)))))))
                        .WithSemicolonToken(SyntaxFactory.Token(
                            SyntaxFactory.TriviaList(),
                            SyntaxKind.SemicolonToken,
                            SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed)));
                    }).ToList() ;

                    vars[0] = vars[0].WithLeadingTrivia(SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed,
                                                SyntaxFactory.Comment("//Arrange")));
            }
            bool isVoid = ((PredefinedTypeSyntax)m.ReturnType).Keyword.Text.Equals("void");
            var invokeMethod = isVoid ? 
                SyntaxFactory.ExpressionStatement(SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(
                                            SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed, SyntaxFactory.Whitespace("    ")),
                                        objectName,
                                            SyntaxFactory.TriviaList())),
                                        SyntaxFactory.IdentifierName(m.Identifier.Text)))
                                    .WithArgumentList(
                                        args
                                        ))
                    .WithSemicolonToken(SyntaxFactory.Token(
                            SyntaxFactory.TriviaList(),
                            SyntaxKind.SemicolonToken,
                            SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed)))
                    .WithLeadingTrivia(SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed,
                                                SyntaxFactory.Comment("//Act")))
                    as StatementSyntax
                :
                SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(
                        m.ReturnType)
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier(
                                                "actual"
                                                ))
                            .WithInitializer(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(
                                            objectName)),
                                        SyntaxFactory.IdentifierName(m.Identifier.Text)))
                                    .WithArgumentList(
                                        args
                                        )
                                    )))))
                .WithSemicolonToken(SyntaxFactory.Token(
                        SyntaxFactory.TriviaList(),
                        SyntaxKind.SemicolonToken,
                        SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed, SyntaxFactory.CarriageReturnLineFeed)))
                .WithLeadingTrivia(SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed,
                                                SyntaxFactory.Comment("//Act")))
                ;

            LocalDeclarationStatementSyntax expectedVar = null;
            ExpressionStatementSyntax assertInvoke = null;
            if (!isVoid) 
            {
                expectedVar = SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(
                        m.ReturnType)
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier("expected"))
                            .WithInitializer(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.DefaultLiteralExpression,
                                        SyntaxFactory.Token(SyntaxKind.DefaultKeyword)))))))
                .WithSemicolonToken(SyntaxFactory.Token(
                        SyntaxFactory.TriviaList(),
                        SyntaxKind.SemicolonToken,
                        SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed)));
                
                assertInvoke = SyntaxFactory.ExpressionStatement(SyntaxFactory
                    .InvocationExpression(SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(
                            SyntaxFactory.TriviaList(SyntaxFactory.Whitespace("    ")),
                        "Assert",
                            SyntaxFactory.TriviaList())),
                        SyntaxFactory.IdentifierName("That")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(SyntaxFactory.IdentifierName("actual")),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(SyntaxFactory
                                        .InvocationExpression(SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(
                                                SyntaxFactory.TriviaList(SyntaxFactory.Whitespace("    ")),
                                            "Is",
                                                SyntaxFactory.TriviaList())),
                                            SyntaxFactory.IdentifierName("EqualTo")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                        new SyntaxNodeOrToken[]
                                                        {
                                                            SyntaxFactory.Argument(SyntaxFactory.IdentifierName("expected"))
                                                        }))))
                                    }))))
                    .WithSemicolonToken(SyntaxFactory.Token(
                        SyntaxFactory.TriviaList(),
                        SyntaxKind.SemicolonToken,
                        SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed)));
            }
            
            var assertFail = SyntaxFactory.ExpressionStatement(SyntaxFactory
                    .InvocationExpression(SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(
                            SyntaxFactory.TriviaList(SyntaxFactory.Whitespace("    ")),
                            "Assert",
                            SyntaxFactory.TriviaList())),
                        SyntaxFactory.IdentifierName("Fail")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(
                            new SyntaxNodeOrToken[]
                            {
                                SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(
                                    SyntaxKind.StringLiteralExpression,
                                    SyntaxFactory.Literal("autogenerated")))
                            }))))
                .WithSemicolonToken(SyntaxFactory.Token(
                    SyntaxFactory.TriviaList(),
                    SyntaxKind.SemicolonToken,
                    SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed)));
            List<StatementSyntax> statements = new List<StatementSyntax>();

            if (hasParams)
                statements.AddRange(vars);
            statements.Add(invokeMethod);
            if (!isVoid)
            {
                statements.Add(expectedVar
                    .WithLeadingTrivia(SyntaxFactory.Comment("//Assert")));
                statements.Add(assertInvoke);
                statements.Add(assertFail);
            }
            else
            {
                statements.Add(assertFail.WithLeadingTrivia(SyntaxFactory.Comment("//Assert")));
            }

            return SyntaxFactory
                            .MethodDeclaration(
                                SyntaxFactory.PredefinedType(
                                    SyntaxFactory.Token(
                                        SyntaxFactory.TriviaList(),
                                        SyntaxKind.VoidKeyword,
                                        SyntaxFactory.TriviaList(SyntaxFactory.Space))),
                                    SyntaxFactory.Identifier(m.Identifier.Text + "Test"))
                            .WithAttributeLists(SyntaxFactory.SingletonList(SyntaxFactory
                                .AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("Test"))))
                                .WithOpenBracketToken(SyntaxFactory.Token(
                                    SyntaxFactory.TriviaList(SyntaxFactory.Whitespace("  ")),
                                    SyntaxKind.OpenBracketToken,
                                    SyntaxFactory.TriviaList()))
                                .WithCloseBracketToken(SyntaxFactory.Token(
                                    SyntaxFactory.TriviaList(),
                                    SyntaxKind.CloseBracketToken,
                                    SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed)))))
                                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxFactory.TriviaList(
                                    SyntaxFactory.Whitespace("  ")),
                                    SyntaxKind.PublicKeyword,
                                    SyntaxFactory.TriviaList(SyntaxFactory.Space))))
                                .WithParameterList(SyntaxFactory.ParameterList().WithCloseParenToken(
                                    SyntaxFactory.Token(SyntaxFactory.TriviaList(),
                                    SyntaxKind.CloseParenToken,
                                    SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed))))
                                .WithBody(SyntaxFactory
                                .Block(statements)
                                .WithOpenBraceToken(SyntaxFactory.Token(SyntaxFactory.TriviaList(
                                    SyntaxFactory.Whitespace("  ")),
                                    SyntaxKind.OpenBraceToken,
                                    SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed)))
                                .WithCloseBraceToken(SyntaxFactory.Token(SyntaxFactory.TriviaList(
                                    SyntaxFactory.Whitespace("  ")),
                                    SyntaxKind.CloseBraceToken,
                                    SyntaxFactory.TriviaList(
                                        SyntaxFactory.CarriageReturnLineFeed,
                                        SyntaxFactory.Whitespace(""),
                                        SyntaxFactory.CarriageReturnLineFeed))));
            }
            );
        
        var ctrs = classDeclaration.DescendantNodes().OfType<ConstructorDeclarationSyntax>().OrderByDescending( ct => ct.ParameterList.Parameters.Count);
        ConstructorDeclarationSyntax ourCtr = null;
        foreach(var ctr in ctrs){
            foreach(var arg_ in ctr.ParameterList.Parameters)
            {
                if(arg_.Type.ToString()[0] == 'I' && char.IsUpper(arg_.Type.ToString()[1]))
                {
                    ourCtr = ctr;
                    break;
                }
            }
        }
        
        
        IEnumerable<FieldDeclarationSyntax> mockFields = new List<FieldDeclarationSyntax>();
        IEnumerable<ExpressionStatementSyntax> initMocks = new List<ExpressionStatementSyntax>();
        IEnumerable<ArgumentSyntax> initObjArgs = new List<ArgumentSyntax>();

        if(ourCtr != null)
        {
            mockFields = ourCtr.ParameterList.Parameters.Select(p =>
            {
                return SyntaxFactory.FieldDeclaration(
                                 SyntaxFactory.VariableDeclaration(
                                     SyntaxFactory.GenericName(
                                         SyntaxFactory.Identifier("Mock"))
                                     .WithTypeArgumentList(
                                         SyntaxFactory.TypeArgumentList(
                                             SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                 SyntaxFactory.IdentifierName(p.Type.ToString())))))
                                 .WithVariables(
                                     SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                         SyntaxFactory.VariableDeclarator(
                                             SyntaxFactory.Identifier(p.Identifier.Text)))))
                             .WithModifiers(
                                 SyntaxFactory.TokenList(
                                     SyntaxFactory.Token(SyntaxKind.PrivateKeyword)));
            });

            initMocks = ourCtr.ParameterList.Parameters.Select(p =>
            {
                return SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.AssignmentExpression(
                                            SyntaxKind.SimpleAssignmentExpression,
                                            SyntaxFactory.IdentifierName(p.Identifier.Text),
                                            SyntaxFactory.ObjectCreationExpression(
                                                SyntaxFactory.GenericName(
                                                    SyntaxFactory.Identifier("Mock"))
                                                .WithTypeArgumentList(
                                                    SyntaxFactory.TypeArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                            SyntaxFactory.IdentifierName(p.Type.ToString())))))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList())));
            });

            initObjArgs = ourCtr.ParameterList.Parameters.Select(p =>
            {
                return SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(p.Identifier.Text),
                            SyntaxFactory.IdentifierName("Object")));
            });
        }

        var setUpMethod = SyntaxFactory.MethodDeclaration(
                            SyntaxFactory.PredefinedType(
                                SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                            SyntaxFactory.Identifier("SetUp"))
                        .WithAttributeLists(
                            SyntaxFactory.SingletonList<AttributeListSyntax>(
                                SyntaxFactory.AttributeList(
                                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                        SyntaxFactory.Attribute(
                                            SyntaxFactory.IdentifierName("SetUp"))))))
                        .WithModifiers(
                            SyntaxFactory.TokenList(
                                SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                        .WithBody(
                            SyntaxFactory.Block(
                                initMocks
                                )
                            .AddStatements(SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.AssignmentExpression(
                                        SyntaxKind.SimpleAssignmentExpression,
                                        SyntaxFactory.IdentifierName(objectName),
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName(classDeclaration.Identifier.Text))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                )
                                            .AddArguments(initObjArgs.ToArray()))))));

        var testCode = SyntaxFactory.CompilationUnit()
        .AddUsings(
            SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")),
            SyntaxFactory.UsingDirective(SyntaxFactory.QualifiedName(SyntaxFactory.QualifiedName(
                SyntaxFactory.IdentifierName("System"),
                SyntaxFactory.IdentifierName("Collections")
                ),
                SyntaxFactory.IdentifierName("Generic")
            )),
            SyntaxFactory.UsingDirective(SyntaxFactory.QualifiedName(
                SyntaxFactory.IdentifierName("System"),
                SyntaxFactory.IdentifierName("Linq")
            )),
            SyntaxFactory.UsingDirective(SyntaxFactory.QualifiedName(
                SyntaxFactory.IdentifierName("System"),
                SyntaxFactory.IdentifierName("Text")
            )),
            SyntaxFactory.UsingDirective(SyntaxFactory.QualifiedName(
                SyntaxFactory.IdentifierName("NUnit"),
                SyntaxFactory.IdentifierName("Framework")
            )),
            SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("Moq")),
            SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("Autogenerated")),
            arg.UsingDirectiveSyntax
            )
        .AddMembers(
            SyntaxFactory.NamespaceDeclaration(
                    SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName("Autogenerated"),
                    SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(SyntaxFactory.TriviaList(), "Tests",
                    SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed))))
                )
            .WithNamespaceKeyword(
                SyntaxFactory.Token(
                    SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed, SyntaxFactory.CarriageReturnLineFeed),
                    SyntaxKind.NamespaceKeyword,
                    SyntaxFactory.TriviaList(SyntaxFactory.Space)))
            .WithOpenBraceToken(
                SyntaxFactory.Token(SyntaxFactory.TriviaList(), SyntaxKind.OpenBraceToken,
                SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed)))
            .WithCloseBraceToken(
                SyntaxFactory.Token(SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed),
                SyntaxKind.CloseBraceToken,
                SyntaxFactory.TriviaList()))
            .AddMembers(
                SyntaxFactory.ClassDeclaration(SyntaxFactory.Identifier(
                        SyntaxFactory.TriviaList(),
                        classDeclaration.Identifier.Text + "Test",
                        SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed)))
                .WithAttributeLists(SyntaxFactory.SingletonList(SyntaxFactory
                            .AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("TestFixture"))))
                            .WithOpenBracketToken(SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(SyntaxFactory.Whitespace(" ")),
                                SyntaxKind.OpenBracketToken,
                                SyntaxFactory.TriviaList()))
                            .WithCloseBracketToken(SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(),
                                SyntaxKind.CloseBracketToken,
                                SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed)))))
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(
                        SyntaxFactory.TriviaList(SyntaxFactory.Whitespace(" ")),
                        SyntaxKind.PublicKeyword,
                        SyntaxFactory.TriviaList(SyntaxFactory.Space))))
                .WithKeyword(SyntaxFactory.Token(
                        SyntaxFactory.TriviaList(),
                        SyntaxKind.ClassKeyword,
                        SyntaxFactory.TriviaList(SyntaxFactory.Space)))
                .WithOpenBraceToken(SyntaxFactory.Token(
                        SyntaxFactory.TriviaList(SyntaxFactory.Whitespace(" ")),
                        SyntaxKind.OpenBraceToken,
                        SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed)))
                .WithCloseBraceToken(
                    SyntaxFactory.Token(
                        SyntaxFactory.TriviaList(SyntaxFactory.Whitespace(" ")),
                        SyntaxKind.CloseBraceToken,
                        SyntaxFactory.TriviaList()))
                .AddMembers(mockFields.ToArray())
                .AddMembers(
                        SyntaxFactory.FieldDeclaration(
                            SyntaxFactory.VariableDeclaration(
                                SyntaxFactory.IdentifierName(classDeclaration.Identifier))
                            .WithVariables(
                                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    SyntaxFactory.VariableDeclarator(
                                        SyntaxFactory.Identifier(objectName)))))
                        .WithModifiers(
                            SyntaxFactory.TokenList(
                                SyntaxFactory.Token(SyntaxKind.PrivateKeyword)))
                    )
                .AddMembers(setUpMethod)
                .AddMembers(methodDeclarations.ToArray())
                )
            )
        .NormalizeWhitespace();

        return new TestFile(classDeclaration.Identifier.Text + "Test.cs", testCode.ToFullString());
    }

    private static ActionBlock<TestFile> WriteTestsBlock()
    {
        return new ActionBlock<TestFile>(async testFile =>
        {
            if (!Directory.Exists(GeneratorOptions.DestDir))
            {
                throw new GeneratorOptionsException("Invalid destination directory", GeneratorOptions.DestDir);
            }

            using (var file = File.CreateText($"{GeneratorOptions.DestDir}/{testFile.FileName}"))
            {
                await file.WriteAsync(testFile.Content);
            }
        }, new ExecutionDataflowBlockOptions(){MaxDegreeOfParallelism = GeneratorOptions.MaxThreadsToWrite});
    }
    
}