using System;
using System.Collections;

namespace System.Windows.Forms
{
    public class PropertyDescriptorCollection
    {
        private object p;

        public int Count { get; set; }

        public PropertyDescriptorCollection(object p)
        {
            this.p = p;
        }

        public virtual IEnumerator GetEnumerator()
        {
            return null;
        }
        public virtual PropertyDescriptor this[int index]
        {
            get
            {
                return null;
            }
        }
        public virtual PropertyDescriptor this[string name]
        {
            get
            {
                return null;
            }
        }
        public PropertyDescriptor Find(string bindingField, bool v)
        {
            throw new NotImplementedException();
        }
    }
}