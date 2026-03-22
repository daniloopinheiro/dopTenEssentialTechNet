using FastEndpoints;

namespace VerticalSliceFastEndpoints.HelloWorld;

public sealed class MyEndpoint : Endpoint<MyRequest, MyResponse>
{
    public override void Configure()
    {
        Post("/hello/world");
        AllowAnonymous();
    }

    public override async Task HandleAsync(MyRequest r, CancellationToken c)
    {
        var response = MyEndpointHandler.Handle(r);
        await Send.OkAsync(response, c);
    }
}
