using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Reflection;

namespace JinRestApi.Helpers;

/// <summary>
/// 객체 관련 확장 메서드
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// 객체를 ExpandoObject로 변환하고, Enum 속성에 대해서는 '...Name' 속성을 추가합니다.
    /// </summary>
    /// <param name="source">변환할 소스 객체</param>
    /// <returns>변환된 ExpandoObject</returns>
    public static dynamic ToExpandoWithEnumNames(this object source)
    {
        if (source is null) return null;

        dynamic expando = new ExpandoObject();
        var properties = (IDictionary<string, object?>)expando;

        foreach (var prop in source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (prop.GetIndexParameters().Length != 0) continue;

            var camelCaseName = char.ToLowerInvariant(prop.Name[0]) + prop.Name[1..];
            var value = prop.GetValue(source, null);
            properties[camelCaseName] = value;

            if (value is Enum enumValue)
            {
                var memberInfo = enumValue.GetType().GetField(enumValue.ToString());
                var displayAttribute = memberInfo?.GetCustomAttribute<DisplayAttribute>();
                properties[camelCaseName + "Name"] = displayAttribute?.Name ?? enumValue.ToString();
            }
        }
        return expando;
    }
}