namespace Lab13.Dialects;

public interface ISqlDialect
{
    public string GetQuery(IReadOnlyDictionary<string, object?> parameters);
}