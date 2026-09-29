using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Serialization;

/// <summary>
/// High-performance XML mapper connecting English domain models to Italian electronic invoice XML elements.
/// Tolerant of namespaces, prefixes, formatting variations, and empty elements.
/// </summary>
public static class XmlMapper
{
    private static readonly ConcurrentDictionary<Type, PropertyMapping[]> TypeCache = new();

    private record PropertyMapping(
        PropertyInfo Property,
        string? ElementName,
        string? AttributeName,
        bool IsList,
        Type ItemType,
        bool IsComplexType);

    private static PropertyMapping[] GetMappings(Type type)
    {
        return TypeCache.GetOrAdd(type, t =>
        {
            var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var list = new List<PropertyMapping>();

            foreach (var prop in props)
            {
                if (prop.GetCustomAttribute<XmlIgnoreAttribute>() != null)
                    continue;

                var elementAttr = prop.GetCustomAttribute<XmlElementAttribute>();
                var attributeAttr = prop.GetCustomAttribute<XmlAttributeAttribute>();

                if (elementAttr == null && attributeAttr == null)
                    continue;

                var isList = typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string) && prop.PropertyType != typeof(byte[]);
                var itemType = isList ? (prop.PropertyType.IsGenericType ? prop.PropertyType.GetGenericArguments()[0] : typeof(object)) : prop.PropertyType;
                var isComplex = !IsSimpleType(itemType);

                list.Add(new PropertyMapping(
                    prop,
                    elementAttr?.ElementName ?? prop.Name,
                    attributeAttr?.AttributeName,
                    isList,
                    itemType,
                    isComplex
                ));
            }

            return list.ToArray();
        });
    }

    private static bool IsSimpleType(Type type)
    {
        var underlying = Nullable.GetUnderlyingType(type) ?? type;
        return underlying.IsPrimitive
            || underlying.IsEnum
            || underlying == typeof(string)
            || underlying == typeof(decimal)
            || underlying == typeof(DateTime)
            || underlying == typeof(Guid)
            || underlying == typeof(byte[]);
    }

    public static T Deserialize<T>(XElement element) where T : new()
    {
        var target = new T();
        DeserializeInto(element, target);
        return target;
    }

    public static object Deserialize(XElement element, Type targetType)
    {
        var target = Activator.CreateInstance(targetType)!;
        DeserializeInto(element, target);
        return target;
    }

    public static void DeserializeInto(XElement element, object target)
    {
        var mappings = GetMappings(target.GetType());

        foreach (var mapping in mappings)
        {
            if (mapping.AttributeName != null)
            {
                var attr = element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, mapping.AttributeName, StringComparison.OrdinalIgnoreCase));
                if (attr != null && !string.IsNullOrWhiteSpace(attr.Value))
                {
                    var val = ConvertValue(attr.Value, mapping.Property.PropertyType);
                    mapping.Property.SetValue(target, val);
                }
                continue;
            }

            if (mapping.ElementName == null)
                continue;

            if (mapping.IsList)
            {
                var childElements = element.Elements().Where(e => string.Equals(e.Name.LocalName, mapping.ElementName, StringComparison.OrdinalIgnoreCase)).ToList();
                if (childElements.Count > 0)
                {
                    var list = (IList?)mapping.Property.GetValue(target);
                    if (list == null)
                    {
                        var listType = typeof(List<>).MakeGenericType(mapping.ItemType);
                        list = (IList)Activator.CreateInstance(listType)!;
                        mapping.Property.SetValue(target, list);
                    }

                    foreach (var child in childElements)
                    {
                        if (mapping.IsComplexType)
                        {
                            var item = Activator.CreateInstance(mapping.ItemType)!;
                            DeserializeInto(child, item);
                            list.Add(item);
                        }
                        else
                        {
                            var val = ConvertValue(child.Value, mapping.ItemType);
                            if (val != null)
                                list.Add(val);
                        }
                    }
                }
            }
            else
            {
                var child = element.Elements().FirstOrDefault(e => string.Equals(e.Name.LocalName, mapping.ElementName, StringComparison.OrdinalIgnoreCase));
                if (child != null)
                {
                    if (mapping.IsComplexType)
                    {
                        // Check if empty self-closing element
                        if (child.IsEmpty && !child.HasElements && !child.HasAttributes)
                            continue;

                        var childObj = mapping.Property.GetValue(target);
                        if (childObj == null)
                        {
                            childObj = Activator.CreateInstance(mapping.Property.PropertyType);
                            mapping.Property.SetValue(target, childObj);
                        }
                        if (childObj != null)
                        {
                            DeserializeInto(child, childObj);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(child.Value))
                    {
                        var val = ConvertValue(child.Value, mapping.Property.PropertyType);
                        mapping.Property.SetValue(target, val);
                    }
                }
            }
        }
    }

    private static object? ConvertValue(string stringValue, Type targetType)
    {
        var underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (underlying == typeof(string))
            return stringValue.Trim();

        if (underlying == typeof(decimal))
        {
            if (decimal.TryParse(stringValue.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var dec))
                return dec;
            return null;
        }

        if (underlying == typeof(int))
        {
            if (int.TryParse(stringValue.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var i))
                return i;
            return null;
        }

        if (underlying == typeof(DateTime))
        {
            if (DateTime.TryParse(stringValue.Trim(), CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                return dt;
            if (DateTime.TryParse(stringValue.Trim(), out var localDt))
                return localDt;
            return null;
        }

        if (underlying == typeof(byte[]))
        {
            try
            {
                return Convert.FromBase64String(stringValue.Trim());
            }
            catch
            {
                return null;
            }
        }

        if (underlying.IsEnum)
        {
            try
            {
                return Enum.Parse(underlying, stringValue.Trim(), true);
            }
            catch
            {
                return null;
            }
        }

        try
        {
            return Convert.ChangeType(stringValue.Trim(), underlying, CultureInfo.InvariantCulture);
        }
        catch
        {
            return null;
        }
    }

    public static XElement Serialize(object source, string rootElementName, XNamespace defaultNamespace)
    {
        var root = new XElement(defaultNamespace + rootElementName);
        SerializeInto(root, source, defaultNamespace);
        return root;
    }

    public static void SerializeInto(XElement element, object source, XNamespace defaultNamespace)
    {
        var mappings = GetMappings(source.GetType());

        foreach (var mapping in mappings)
        {
            var value = mapping.Property.GetValue(source);
            if (value == null)
                continue;

            if (mapping.AttributeName != null)
            {
                var strVal = FormatValue(value);
                if (!string.IsNullOrEmpty(strVal))
                    element.SetAttributeValue(mapping.AttributeName, strVal);
                continue;
            }

            if (mapping.ElementName == null)
                continue;

            if (mapping.IsList)
            {
                if (value is IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        if (item == null)
                            continue;

                        var childElement = new XElement(mapping.ElementName);
                        if (mapping.IsComplexType)
                        {
                            SerializeInto(childElement, item, defaultNamespace);
                            if (childElement.HasElements || childElement.HasAttributes || !string.IsNullOrEmpty(childElement.Value))
                                element.Add(childElement);
                        }
                        else
                        {
                            var formatted = FormatValue(item);
                            if (!string.IsNullOrEmpty(formatted))
                            {
                                childElement.Value = formatted;
                                element.Add(childElement);
                            }
                        }
                    }
                }
            }
            else
            {
                if (mapping.IsComplexType)
                {
                    var childElement = new XElement(mapping.ElementName);
                    SerializeInto(childElement, value, defaultNamespace);
                    if (childElement.HasElements || childElement.HasAttributes || !string.IsNullOrEmpty(childElement.Value))
                        element.Add(childElement);
                }
                else
                {
                    var formatted = FormatValue(value);
                    if (!string.IsNullOrEmpty(formatted))
                    {
                        element.Add(new XElement(mapping.ElementName, formatted));
                    }
                }
            }
        }
    }

    private static string FormatValue(object value)
    {
        return value switch
        {
            decimal d => d.ToString("0.00######", CultureInfo.InvariantCulture),
            DateTime dt => dt.Hour == 0 && dt.Minute == 0 && dt.Second == 0
                ? dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                : dt.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
            byte[] bytes => Convert.ToBase64String(bytes),
            _ => Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty
        };
    }
}
