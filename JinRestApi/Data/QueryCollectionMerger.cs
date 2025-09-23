using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

public static class QueryCollectionMerger
{
    public static IQueryCollection Merge(IQueryCollection q1, IQueryCollection q2)
    {
        var dict = new Dictionary<string, StringValues>(StringComparer.OrdinalIgnoreCase);

        // 먼저 q1 넣고
        foreach (var kv in q1)
        {
            dict[kv.Key] = kv.Value;
        }

        // q2 값이 있으면 덮어씀
        foreach (var kv in q2)
        {
            dict[kv.Key] = kv.Value;
        }

        return new QueryCollection(dict);
    }
}
