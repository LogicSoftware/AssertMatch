using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AssertMatch
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
    }
}