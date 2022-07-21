using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ESATouristGuide.Helpers
{
    public static class Extensions
    {
        public static TTarget Clone<TSource, TTarget>(this TSource source) where TTarget : new()
        {
            TTarget target = new TTarget();
            var properties = new Hashtable();
            var targetType = source.GetType();
            targetType.GetProperties().ToList().ForEach(p => properties[p.Name] = p);
            foreach (var key in properties.Keys)
            {
                var pPropertyInfo = properties[key] as PropertyInfo;
                Type type = Nullable.GetUnderlyingType(pPropertyInfo.PropertyType);
                if (type == null)
                    type = pPropertyInfo.PropertyType;
                if (IsSimpleType(type))
                {
                    var sPropertyInfo = target.GetType().GetProperty(key.ToString());
                    if (sPropertyInfo != null && sPropertyInfo.CanWrite)
                    {
                        var pValue = pPropertyInfo.GetValue(source);
                        if (pValue.CanConvertTo(type))
                            sPropertyInfo.SetValue(target , Convert.ChangeType(pValue , type));
                        else if (pValue == null)
                            sPropertyInfo.SetValue(target , null);
                    }
                }
            }
            return target;
        }

        public static bool CanConvertTo(this object value , Type type)
        {
            if (value != null)
            {
                try
                {
                    var a = Convert.ChangeType(value , type);
                    return true;
                }
                catch
                {


                }
            }
            return false;
        }

        public static bool IsSimpleType(Type type)
        {
            return
                type.IsPrimitive ||
                new Type[] {
            typeof(Enum),
            typeof(String),
            typeof(Decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid),
            typeof(Uri),
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                type.IsArray && IsSimpleType(type.GetElementType()) ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]));
        }
    }
}
