using Microsoft.Extensions.Options;

namespace OptionsConfig;

public class MyService(IOptions<MongoOptions> options)
{
    public void ShowConfiguration()
    {
        var mongo = options.Value;
        Console.WriteLine($"ConnectionString: {mongo.ConnectionString}");
        Console.WriteLine($"DatabaseName: {mongo.DatabaseName}");
    }
}
