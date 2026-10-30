using Lab13.Dialects;

namespace Lab13;

class Program
{
    static void Main(string[] args)
    {
        var query1 = SqlBuilder<MsSqlDialect>
            .Select<User>()
            .OrderBy(x => x.CreatedAt)
            .Take(10);
        
        var query2 = SqlBuilder<PostgreSqlDialect>
            .Select<User>()
            .OrderBy(x => x.Login)
            .Take(22);
        
        Console.WriteLine("MsSql: \t\t" + query1);
        Console.WriteLine("PostgreSql: \t" + query2);
    }
}