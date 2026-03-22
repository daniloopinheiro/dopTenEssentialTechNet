namespace VerticalSliceFastEndpoints.HelloWorld;

/// <summary>Lógica do caso de uso isolada no “slice” (vertical slice).</summary>
public static class MyEndpointHandler
{
    public static MyResponse Handle(MyRequest request)
    {
        return new MyResponse
        {
            Message = $"Hello, {request.Name}"
        };
    }
}
