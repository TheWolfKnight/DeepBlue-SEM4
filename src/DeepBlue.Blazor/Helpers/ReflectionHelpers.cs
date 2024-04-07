
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DeepBlue.Blazor.Helpers;

public static class ReflectionHelpers
{
  public static string GetDisplayName(this PropertyInfo prop)
  {
    IEnumerable<Attribute> attrs = prop.GetCustomAttributes(typeof(DisplayNameAttribute));

    for (int i = 0; i < attrs.Count(); ++i)
    {
      Attribute attr = attrs.ElementAt(i);
      if (attr is DisplayNameAttribute displayName)
        return displayName.DisplayName;
    }

    return prop.Name;
  }

  public static IEnumerable<PropertyInfo> GetDisplayProperties(this Type type)
  {
    IEnumerable<PropertyInfo> props = type.GetProperties();
    return props
      .Where(prop => prop
                      .GetCustomAttributes()
                      .Any(attr => attr is DisplayNameAttribute));
  }
}
