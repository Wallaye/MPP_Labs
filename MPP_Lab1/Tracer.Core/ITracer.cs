namespace Tracer.Core;

public interface ITracer<T>
{
    // вызывается в начале замеряемого метода
    void StartTrace();
    
    // вызывается в конце замеряемого метода 
    void StopTrace();
    
    // получить результаты измерений  
    T GetTraceResult();
}