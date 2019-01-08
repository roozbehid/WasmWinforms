using System;
using System.Collections;
using System.ComponentModel;

namespace System.Drawing
{
    public class TypeConverter : global::System.ComponentModel.TypeConverter
    {
        public virtual PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return null;
        }

        public virtual bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }

        public virtual object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            return null;
        }

        public virtual bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return false;
        }

        public virtual StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return null;
        }

        public virtual bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public virtual bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}