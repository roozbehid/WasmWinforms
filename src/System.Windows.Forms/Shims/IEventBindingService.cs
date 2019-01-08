using System.ComponentModel;

namespace System.Windows.Forms
{
    public interface IEventBindingService
    {
        PropertyDescriptorCollection GetEventProperties(EventDescriptorCollection events);
        PropertyDescriptor GetEventProperty(EventDescriptor defaultEvent);
    }
}