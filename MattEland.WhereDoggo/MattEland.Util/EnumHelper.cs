using System.ComponentModel;
using System.Reflection;

namespace MattEland.Util;

/// <summary>
/// Contains extension methods related to dealing with Enums.
/// </summary>
public static class EnumHelper
{
    /// <summary>
    /// Gets a user-facing description of the specified enum value by checking the <see cref="DescriptionAttribute"/>.
    /// </summary>
    /// <param name="value">The enum to evaluate</param>
    /// <returns>A string representing the enum value</returns>
    public static string GetFriendlyName(this Enum value)
    {
        Type type = value.GetType();
        string? name = Enum.GetName(type, value);

        if (name != null)
        {
            FieldInfo? field = type.GetField(name);
            if (field != null && Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
            {
                return attr.Description;
            }
        }
        
        return value.ToString();
    }
}