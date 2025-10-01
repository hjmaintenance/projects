using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

public static class DynamicHelper
{
    public static IDictionary<string, object?> ToDictionary(object obj)
    {
        var dict = new Dictionary<string, object?>();

        // DynamicClass 같은 경우
        if (obj.GetType().Name.StartsWith("DynamicClass"))
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(obj))
            {
                dict[prop.Name] = prop.GetValue(obj);
            }
        }
        else
        {
            // 일반적인 익명 타입/POCO
            foreach (var prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                dict[prop.Name] = prop.GetValue(obj);
            }
        }

        return dict;
    }
}
