// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.
using Masa.Utils.Caller.Core;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Web;

[assembly: InternalsVisibleTo("Masa.Utils.Data.Promethus.Test")]

namespace System.Net.Http;

internal static class HttpClientExtensition
{
    public static async Task<string> GetAsync(this ICallerProvider caller, string url,object data)
    {
        var request=new HttpRequestMessage(HttpMethod.Get, $"{url}?{data?.ToUrlParam()}");
        var response= await caller.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// not support System.text.json
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="isEnumString"></param>
    /// <param name="isCamelCase"></param>
    /// <param name="isUrlEncode"></param>
    /// <returns></returns>
    public static string? ToUrlParam(this object obj, bool isEnumString = true, bool isCamelCase = true, bool isUrlEncode = true)
    {
        return GetValue(obj, string.Empty, isEnumString, isCamelCase, isUrlEncode);
    }

    private static string? GetValue(object obj, string preStr, bool isEnumString = false, bool isCamelCase = true, bool isUrlEncode = true)
    {
        if (obj == null) return null;
        var type = obj.GetType();
        if (type == typeof(string))//string
        {
            var str = (string)obj;
            return AppendValue(preStr, str, "=", isUrlEncode);
        }
        else if (type.IsValueType)
        {
            if (type.IsEnum)//enum
            {
                var str = isEnumString ? obj.ToString() : Convert.ToInt32(obj).ToString();
                return AppendValue(preStr, str, "=", isUrlEncode);
            }
            else if (!type.IsPrimitive) //struct
            {
                return GetObjValue(type, obj, preStr, isEnumString, isCamelCase, isUrlEncode);
            }
            else //sample value
            {
                var str = obj.ToString();
                return AppendValue(preStr, str, "=", isUrlEncode);
            }
        }
        else if (type.IsArray || type.GetInterfaces().Any(t => t.Name.IndexOf("IEnumerable") == 0))
        {
            return GetEnumerableValue(obj, preStr, isEnumString, isCamelCase, isUrlEncode);
        }
        else if (type.IsClass)
        {
            return GetObjValue(type, obj, preStr, isEnumString, isCamelCase, isUrlEncode);
        }
        else
        {
            //
            return null;
        }
    }

    private static string GetObjValue(Type type, object obj, string preStr, bool isEnumString = false, bool isCamelCase = true, bool isUrlEncode = true)
    {
        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);
        var list = new List<string>();
        if (properties.Length > 0)
        {
            foreach (var item in properties)
            {
                var str = GetMemerInfoValue(item, item.GetValue(obj), preStr, isEnumString, isCamelCase, isUrlEncode);
                if (string.IsNullOrEmpty(str))
                    continue;
                list.Add(str);
            }
        }

        if (fields.Length > 0)
        {
            foreach (var item in fields)
            {
                var str = GetMemerInfoValue(item, item.GetValue(obj), preStr, isEnumString, isCamelCase, isUrlEncode);
                if (string.IsNullOrEmpty(str))
                    continue;
                list.Add(str);
            }
        }

        if (!list.Any())
            return default!;
        list.Sort();
        return string.Join('&', list);
    }

    private static string? GetMemerInfoValue(MemberInfo info, object? value, string preStr, bool isEnumString = false, bool isCamelCase = true, bool isUrlEncode = true)
    {
        if (value == null)
            return null;

        var name = info.Name;
        if (isCamelCase)
            name = ToCamelCase(name);

        return GetValue(value, AppendValue(preStr, name, ".", isUrlEncode) ?? default!, isEnumString, isCamelCase, isUrlEncode);
    }

    private static string? GetEnumerableValue(object obj, string preStr, bool isEnumString = false, bool isCamelCase = true, bool isUrlEncode = true)
    {
        StringBuilder builder = new StringBuilder(4096);
        var list = new List<string>();
        foreach (var item in (IEnumerable)obj)
        {
            if (item is KeyValuePair<string, object> keyValue)
            {
                var name = keyValue.Key;
                if (isCamelCase)
                    name = ToCamelCase(name);
                var str = GetValue(keyValue.Value, AppendValue(preStr, name, ".", isUrlEncode) ?? default!, isEnumString, isCamelCase, isUrlEncode);
                if (!string.IsNullOrEmpty(str))
                    list.Add(str);
            }
            else
            {
                var str = GetValue(item, $"{preStr}{(isUrlEncode ? HttpUtility.UrlEncode("[]", Encoding.UTF8) : "[]")}", isEnumString, isCamelCase, isUrlEncode);
                if (!string.IsNullOrEmpty(str))
                    list.Add(str);
            }
        }
        if (!list.Any())
            return default!;
        list.Sort();
        return string.Join('&', list);
    }

    private static string? AppendValue(string preStr, string? value, string splitChar, bool isUrlEncode)
    {
        if (string.IsNullOrEmpty(preStr) || string.IsNullOrEmpty(value))
            return value;
        if (isUrlEncode)
            return $"{preStr}{splitChar}{HttpUtility.UrlEncode(value, Encoding.UTF8)}";
        else
            return $"{preStr}{splitChar}{value}";
    }

    public static string ToCamelCase(this string str)
    {
        if (string.IsNullOrEmpty(str)) return str;
        var c = str[0];
        if (c - 'A' >= 0 && c - 'Z' <= 0)
            return $"{(char)(c + 32)}{str.AsSpan().Slice(1)}";

        return str;
    }
}
