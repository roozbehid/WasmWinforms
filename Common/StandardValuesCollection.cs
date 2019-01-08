// taken from referencesource/System/compmod/system/componentmodel/ TypeConverter.cs 

using System;
using System.Collections;

namespace System.ComponentModel
{
    /// <devdoc>
    ///    <para>Represents a collection of values.</para>
    /// </devdoc>
    public class StandardValuesCollection : ICollection {
        private ICollection values;
        private Array       valueArray;
        
        /// <devdoc>
        ///    <para>
        ///       Initializes a new instance of the <see cref='System.ComponentModel.TypeConverter.StandardValuesCollection'/>
        ///       class.
        ///    </para>
        /// </devdoc>
        public StandardValuesCollection(ICollection values) {
            if (values == null) {
                values = new object[0];
            }
            
            Array a = values as Array;
            if (a != null) {
                valueArray = a;
            }
            
            this.values = values;
        }
        
        /// <devdoc>
        ///    <para>
        ///       Gets the number of objects in the collection.
        ///    </para>
        /// </devdoc>
        public int Count {
            get {
                if (valueArray != null) {
                    return valueArray.Length;
                }
                else {
                    return values.Count;
                }
            }
        }
        
        /// <devdoc>
        ///    <para>Gets the object at the specified index number.</para>
        /// </devdoc>
        public object this[int index] {
            get {
                if (valueArray != null) {
                    return valueArray.GetValue(index);
                }
                IList list = values as IList;
                if (list != null) {
                    return list[index];
                }
                // No other choice but to enumerate the collection.
                //
                valueArray = new object[values.Count];
                values.CopyTo(valueArray, 0);
                return valueArray.GetValue(index);
            }
        }
    
        /// <devdoc>
        ///    <para>Copies the contents of this collection to an array.</para>
        /// </devdoc>
        public void CopyTo(Array array, int index) {
            values.CopyTo(array, index);
        }
    
        /// <devdoc>
        ///    <para>
        ///       Gets an enumerator for this collection.
        ///    </para>
        /// </devdoc>
        public IEnumerator GetEnumerator() {
            return values.GetEnumerator();
        }
        
        /// <internalonly/>
        /// <devdoc>
        /// Retrieves the count of objects in the collection.
        /// </devdoc>
        int ICollection.Count {
            get {
                return Count;
            }
        }
    
        /// <internalonly/>
        /// <devdoc>
        /// Determines if this collection is synchronized.
        /// The ValidatorCollection is not synchronized for
        /// speed.  Also, since it is read-only, there is
        /// no need to synchronize it.
        /// </devdoc>
        bool ICollection.IsSynchronized {
            get {
                return false;
            }
        }
    
        /// <internalonly/>
        /// <devdoc>
        /// Retrieves the synchronization root for this
        /// collection.  Because we are not synchronized,
        /// this returns null.
        /// </devdoc>
        object ICollection.SyncRoot {
            get {
                return null;
            }
        }
    
        /// <internalonly/>
        /// <devdoc>
        /// Copies the contents of this collection to an array.
        /// </devdoc>
        void ICollection.CopyTo(Array array, int index) {
            CopyTo(array, index);
        }
    
        /// <internalonly/>
        /// <devdoc>
        /// Retrieves a new enumerator that can be used to
        /// iterate over the values in this collection.
        /// </devdoc>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}