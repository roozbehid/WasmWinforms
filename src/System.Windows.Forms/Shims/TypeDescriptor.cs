using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Forms
{
    internal class TypeDescriptor
    {
        public static PropertyDescriptorCollection GetProperties(object type)
        {
            throw new NotImplementedException();
        }

        public static PropertyDescriptorCollection GetProperties(object type, Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        public static TypeConverter GetConverter(Type type)
        {
            throw new NotImplementedException();
        }

        internal static EventDescriptorCollection GetEvents(object o)
        {
            throw new NotImplementedException();
        }

        internal static AttributeCollection GetAttributes(object v)
        {
            throw new NotImplementedException();
        }

        internal static EventDescriptorCollection GetEvents(object component, Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        internal static EventDescriptor GetDefaultEvent(object obj)
        {
            throw new NotImplementedException();
        }

        internal static TypeConverter GetConverter(object component)
        {
            throw new NotImplementedException();
        }
    }
}