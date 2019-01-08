using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms
{
    public class PropertyDescriptor 
    {

        public AttributeCollection Attributes { get; internal set; }
        public TypeConverter Converter { get; internal set; }
        public string DisplayName { get; internal set; }
        public bool IsBrowsable { get; internal set; }
        public bool IsReadOnly { get; internal set; }
        public string Name { get; internal set; }
        public Type PropertyType { get; internal set; }
        public string Description { get; internal set; }
        public string Category { get; internal set; }

        internal UITypeEditor GetEditor(Type type)
        {
            throw new NotImplementedException();
        }

        internal object GetValue(object v)
        {
            throw new NotImplementedException();
        }

        internal void SetValue(object v, object value)
        {
            throw new NotImplementedException();
        }

        internal void ResetValue(object v)
        {
            throw new NotImplementedException();
        }

        internal bool ShouldSerializeValue(object propertyOwner)
        {
            throw new NotImplementedException();
        }

        internal bool CanResetValue(object propertyOwner)
        {
            throw new NotImplementedException();
        }
    }
}