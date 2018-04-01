using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyTests
{
    class ValueReader<T>
    {
        private List<MemberInfo> _properties = new List<MemberInfo>();

        public void AddMember(MemberInfo member)
        {
            _properties.Add(member);
        }

        public object GetValue(T obj, out bool cantReadProperiesChain)
        {
            cantReadProperiesChain = false;
            object curr = obj;
            foreach (var property in _properties)
            {
                if(curr == null)
                {
                    cantReadProperiesChain = true;
                    return null;
                }
                curr = GetValue(curr, property);
            }

            return curr;
        }

        public string GetFormattedValue(T actual, string actualArgName = "")
        {
            var actualValue = GetValue(actual, out var isCantReadProperiesChain);
            var formattedValue = Helper.FormatValue(actualValue);
            if (isCantReadProperiesChain)
            {
                var nullPathName = actualArgName;
                var pathBeforeNull = GetNameBeforeNullInPropsChain(actual);
                if (!string.IsNullOrEmpty(pathBeforeNull))
                {
                    if (!string.IsNullOrEmpty(nullPathName))
                        nullPathName += ".";

                    nullPathName += pathBeforeNull;
                }
                formattedValue = $"{nullPathName} is NULL";
            }

            return formattedValue;
        }

        private object GetValue(object owner, MemberInfo memberInfo)
        {
            if(memberInfo is PropertyInfo pi)
            {
                return pi.GetValue(owner);
            }

            throw new Exception($"MatchTo currently supports only properties checks. But found comparing of {memberInfo.Name}({memberInfo.MemberType})");
        }

        //TODO: better name
        public string GetNameBeforeNullInPropsChain(T obj)
        {
            IEnumerable<string> GetNames()
            {
                object curr = obj;
                foreach (var property in _properties)
                {
                    if (curr == null)
                    {
                        yield break;
                    }
                    curr = GetValue(curr, property);
                    yield return property.Name;
                }
            }

            return string.Join(".", GetNames());
        }

        public string GetMemberName()
        {
            return string.Join(".", _properties.Select(x => x.Name));
        }

        private sealed class PropertiesEqualityComparer : IEqualityComparer<ValueReader<T>>
        {
            public bool Equals(ValueReader<T> x, ValueReader<T> y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return Equals(x.GetMemberName(), y.GetMemberName());
            }

            public int GetHashCode(ValueReader<T> obj)
            {
                return obj.GetMemberName().GetHashCode();
            }
        }

        public static IEqualityComparer<ValueReader<T>> PropertiesComparer { get; } = new PropertiesEqualityComparer();
    }
}