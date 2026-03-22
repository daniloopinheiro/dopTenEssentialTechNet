using System.ComponentModel.DataAnnotations;

namespace OptionsConfig;

public class MongoOptions
{
    public static string Section = "Mongo";

    [Required]
    public string ConnectionString { get; init; } = null!;

    [Required]
    [MinLength(10)]
    public string DatabaseName { get; init; } = null!;
}
