using System.Collections;

namespace System.Resources
{
    public class ResourceSet
    {
        public void ReadResources()
        {
            throw new NotImplementedException();
        }
        public ResXResourceReader Reader { get;  set; }
        public Hashtable Table { get; set; }

        public virtual Type GetDefaultReader()
        {
            return null;
        }

        public virtual Type GetDefaultWriter()
        {
            return null;
        }
    }
}