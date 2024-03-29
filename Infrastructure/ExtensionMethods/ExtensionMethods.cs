using System.Linq.Expressions;

namespace Infrastructure.ExtensionMethods;

public static class ExtensionMethods
{
    public static IQueryable<T> OrderByPropertyName<T>(this IQueryable<T> q, string sortField, bool ascending)
    {
        var param = Expression.Parameter(typeof(T), "p");
        var prop = Expression.Property(param, sortField);
        var exp = Expression.Lambda(prop, param);
        string method = ascending ? "OrderBy" : "OrderByDescending";
        Type[] types = new Type[] { q.ElementType, exp.Body.Type };
        var rs = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
        return q.Provider.CreateQuery<T>(rs);   
    }
}