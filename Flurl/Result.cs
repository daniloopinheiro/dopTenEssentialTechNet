namespace FlurlHttpResilience;

/// <summary>Envelope simples para o pipeline Polly sobre chamadas Flurl.</summary>
public sealed class Result<T>
{
    public bool IsSuccess { get; init; }

    public T? Value { get; init; }

    public string? Error { get; init; }

    public static Result<T> Ok(T value) => new() { IsSuccess = true, Value = value };

    public static Result<T> Fail(string error) => new() { IsSuccess = false, Error = error };
}
