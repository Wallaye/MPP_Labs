using TestGenerator.Core.Generator;

GeneratorOptions options = new(5, 5, 5, @"D:\TestGenerator\Classes", @"D:\TestGenerator\Generated");
TestGenerator.Core.TestsGenerator.GeneratorOptions = options;
TestGenerator.Core.TestsGenerator.Generate().Wait();