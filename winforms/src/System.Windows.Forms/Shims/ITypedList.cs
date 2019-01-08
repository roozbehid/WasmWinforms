using System.ComponentModel;

namespace System.Windows.Forms
{
    public interface ITypedList
    {
        string GetListName(PropertyDescriptor[] pds);
        PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors);
    }
}