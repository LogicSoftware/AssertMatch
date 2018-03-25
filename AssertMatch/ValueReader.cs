using System.Collections.Generic;
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
            //TODO: support fields accessors
            foreach (PropertyInfo property in _properties)
            {
                if(curr == null)
                {
                    cantReadProperiesChain = true;
                    return null;
                }
                curr = property.GetValue(curr);
            }

            return curr;
        }
    }
}