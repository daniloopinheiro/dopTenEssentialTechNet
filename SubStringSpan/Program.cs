const string CsvLine = "1592,Ergonomic Plastic Sausages,Clothing,84.41,2025-07-02T02:34:14";
const string Category = "Clothing";

bool bySubstring = MatchWithSubstring(CsvLine, Category);
bool bySpan = MatchWithSpan(CsvLine, Category);

Console.WriteLine($"CSV: {CsvLine}");
Console.WriteLine($"Categoria esperada: {Category}");
Console.WriteLine();
Console.WriteLine($"Substring(32, 8) == category : {bySubstring}");
Console.WriteLine($"AsSpan().Slice(32, 8) ~ category : {bySpan}");
Console.WriteLine($"Mesmo resultado: {bySubstring == bySpan}");

/// <summary>Extrai com alocação de nova string no heap.</summary>
static bool MatchWithSubstring(string csvLine, string category)
{
    var lineCategory = csvLine.Substring(32, 8);
    return lineCategory == category;
}

/// <summary>Extrai como visão sobre o buffer da string, sem alocar a fatia.</summary>
static bool MatchWithSpan(string csvLine, string category)
{
    var lineCategory = csvLine.AsSpan().Slice(32, 8);
    return lineCategory.SequenceEqual(category.AsSpan());
}
