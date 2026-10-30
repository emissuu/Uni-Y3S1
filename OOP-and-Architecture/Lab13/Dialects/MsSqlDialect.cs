namespace Lab13.Dialects;

public class MsSqlDialect : ISqlDialect
{
    public string GetQuery(IReadOnlyDictionary<string, object?> parameters)
    {
        string query = "SELECT ";

        if (parameters["@take"] is not null)
        {
            query += $"TOP ({parameters["@take"]}) ";
        }

        query += $"* FROM [{parameters["@entityName"]}] ";

        if (parameters["@orderBy"] is not null && parameters["@descending"] is bool descending)
        {
            string direction = descending ? "DESC" : "ASC";
            query += $"ORDER BY [{parameters["@orderBy"]}] {direction} ";
        }

        query += ';';

        return query;
    }
}