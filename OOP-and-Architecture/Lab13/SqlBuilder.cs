using System.Linq.Expressions;
using Lab13.Dialects;

namespace Lab13;


public static class SqlBuilder<TDialect> where TDialect : ISqlDialect, new()
{
    public static SqlSelectBuilder<TEntity> Select<TEntity>()
    {
        return new SqlSelectBuilder<TEntity>(new TDialect());
    }
}

public class SqlSelectBuilder<TEntity>(ISqlDialect dialect)
{
    private string? _orderBy;
    private bool _descending = false;
    private int? _take;

    public SqlSelectBuilder<TEntity> OrderBy(Expression<Func<TEntity, object>> expression)
    {
        _orderBy = GetPropertyName(expression);
        _descending = false;
        return this;
    }

    public SqlSelectBuilder<TEntity> OrderByDescending(Expression<Func<TEntity, object>> expression)
    {
        _orderBy = GetPropertyName(expression);
        _descending = true;
        return this;
    }

    public SqlSelectBuilder<TEntity> Take(int take)
    {
        _take = take;
        return this;
    }

    public string CommandText => dialect.GetQuery(CommandParameters);
    
    public override string ToString() => CommandText;

    public IReadOnlyDictionary<string, object?> CommandParameters
    {
        get
        {
            var parameters = new Dictionary<string, object?>();
            
            parameters["@entityName"] = typeof(TEntity).Name;
            if (_take.HasValue)
                parameters["@take"] =  _take.Value;
            parameters["@descending"] = _descending;
            parameters["@orderBy"] = _orderBy;
            
            return parameters;
        }
    }
    
    private string GetPropertyName(Expression<Func<TEntity, object>> expression)
    {
        Expression body = expression.Body;
        
        if (body is UnaryExpression unary)
            body = unary.Operand;

        if (body is not MemberExpression member)
            throw new ArgumentException(
                "Expected a property access"
            );

        return member.Member.Name;
    }
}