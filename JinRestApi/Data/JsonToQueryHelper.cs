using System.Text.Json;
using Microsoft.Extensions.Primitives;

public static class JsonToQueryHelper
{
    public static IQueryCollection ConvertJsonToQuery(string json)
    {
        var dict = new Dictionary<string, StringValues>(StringComparer.OrdinalIgnoreCase);
        using var doc = JsonDocument.Parse(json);

        foreach (var prop in doc.RootElement.EnumerateObject())
        {
            if (prop.Name.Equals("filters", StringComparison.OrdinalIgnoreCase))
            {
                // [{ "field":"id", "op":"eq", "value":"10"}]
                foreach (var filter in prop.Value.EnumerateArray())
                {
                    var field = filter.GetProperty("field").GetString();
                    var op = filter.TryGetProperty("op", out var o) ? o.GetString() : "eq";
                    var value = filter.GetProperty("value").ToString();
                    var key = $"{field}_{op}";
                    dict[key] = value;
                }
            }
            else if (prop.Name.Equals("sorts", StringComparison.OrdinalIgnoreCase))
            {
                // [{ "field":"id", "dir":"asc"}]
                var orderParts = new List<string>();
                foreach (var sort in prop.Value.EnumerateArray())
                {
                    var field = sort.GetProperty("field").GetString();
                    var dir = sort.TryGetProperty("dir", out var d) ? d.GetString() : "asc";
                    orderParts.Add($"{field} {dir}");
                }
                dict["orderBy"] = string.Join(",", orderParts);
            }
            else
            {
                dict[prop.Name] = prop.Value.ToString();
            }
        }

        return new QueryCollection(dict);
    }
}
