using TestGenerator.Core.Generator;

GeneratorOptions options = new(5, 5, 5, @"D:\BSUIR\5sem\СПП\MPP_Labs\MPP_Lab4\TestClasses",
    @"D:\BSUIR\5sem\СПП\MPP_Labs\MPP_Lab4\GeneratedTests");
TestGenerator.Core.TestsGenerator.GeneratorOptions = options;
TestGenerator.Core.TestsGenerator.Generate().Wait();