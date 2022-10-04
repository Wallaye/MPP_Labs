using Tracer.Core;
using Tracer.Examples;

var tracer = new Tracer.Core.Tracer();
var foo = new Foo(tracer);
var bar = new Bar(tracer);

Task task1 = Task.Run(() => foo.PublicMethod());
Task task2 = Task.Run(() => bar.InnerMethod());
Task task3 = Task.Run(() => foo.PublicMethod());
foo.PublicMethod();
bar.InnerMethod();
task1.Wait();
task2.Wait();
task3.Wait();

var result = tracer.GetTraceResult();
Console.WriteLine("Where to load results(1-Console, 2 - Files)?");
var s = Console.ReadLine();
if (s == "1")
{
    JSONSerializer jSer = new();
    jSer.Serialize(result, Console.OpenStandardOutput());
    Console.WriteLine();
    XMLSerializer xSer = new();
    xSer.Serialize(result, Console.OpenStandardOutput());
}
else
{
    JSONSerializer jSer = new JSONSerializer();
    using (FileStream file = new("Result.json", FileMode.Create))
    {
        jSer.Serialize(result, file);
    }
    XMLSerializer xSer = new XMLSerializer();
    using (FileStream file = new("Result.xml", FileMode.Create))
    {
        xSer.Serialize(result, file);
    }
}
