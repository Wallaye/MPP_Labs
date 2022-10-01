using Tracer.Core;
using Tracer.Examples;

var tracer = new Tracer.Core.Tracer();
tracer.StartTrace();
var foo = new Foo(tracer);
var bar = new Bar(tracer);

Task task1 = Task.Factory.StartNew(() => foo.PublicMethod());
Task task2 = Task.Factory.StartNew(() => bar.InnerMethod());
foo.PublicMethod();
bar.InnerMethod();
task1.Wait();
task2.Wait();

tracer.StopTrace();
var result = tracer.GetTraceResult();
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