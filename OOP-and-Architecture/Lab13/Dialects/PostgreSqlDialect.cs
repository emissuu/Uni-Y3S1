namespace Lab13.Dialects;

public class PostgreSqlDialect : ISqlDialect
{
    public string GetQuery(IReadOnlyDictionary<string, object?> parameters)
    {
        string query = "SELECT ";

        query += $"* FROM \"{parameters["@entityName"]}\" ";

        if (parameters["@orderBy"] is not null && parameters["@descending"] is bool descending)
        {
            string direction = descending ? "DESC" : "";
            query += $"ORDER BY \"{parameters["@orderBy"]}\" {direction} ";
        }

        if (parameters["@take"] is not null)
        {
            query += $"LIMIT {parameters["@take"]} ";
        }
        
        query += ';';

        return query;
    }
}