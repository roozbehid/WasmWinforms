using System;
using System.ComponentModel;

namespace System.Drawing
{
    internal class TypeDescriptor
    {
        public static PropertyDescriptorCollection GetProperties(object type, Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        public static TypeConverter GetConverter(Type type)
        {
            throw new NotImplementedException();
        }
    }
}