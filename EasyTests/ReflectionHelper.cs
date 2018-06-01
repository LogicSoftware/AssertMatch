using System;

namespace EasyTests
{
    public class ReflectionHelper
    {
        public static Type GetTypeOrUnderlyingType(Type type)
        {
            return IsNullable(type) ? Nullable.GetUnderlyingType(type) : type;
        }

        public static bool IsNullable(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof (Nullable<>);
        }
    }
}