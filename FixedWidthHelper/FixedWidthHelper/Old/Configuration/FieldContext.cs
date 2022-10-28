using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FixedWidthHelper.Configuration
{
    public class FieldContext
    {
        private PropertyInfo[] _FwFields { get; set; }
        public PropertyInfo[] FwFields => _FwFields;

        public FieldContext(Type type)
        {
            _FwFields = type.GetProperties(); //Loop through the class properties
        }

        public FixedWidthFieldAttribute GetAttribute(PropertyInfo Property)
        {
            if (Attribute.IsDefined(Property, typeof(FixedWidthFieldAttribute))) //Get FixedWidthFields
            {
                return (FixedWidthFieldAttribute)Property.GetCustomAttribute(typeof(FixedWidthFieldAttribute), false);
            }
            return null;
        }
    }
}