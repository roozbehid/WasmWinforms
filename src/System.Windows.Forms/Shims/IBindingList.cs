using System.ComponentModel;

namespace System.Windows.Forms
{
    public interface IBindingList
    {
        bool AllowEdit { get; }
        bool AllowNew { get; }
        bool AllowRemove { get;  }
        bool IsSorted { get;  }
        ListSortDirection SortDirection { get; }
        PropertyDescriptor SortProperty { get; }
        bool SupportsSearching { get;  }
        bool SupportsSorting { get;  }

        event ListChangedEventHandler ListChanged;

        void AddIndex(PropertyDescriptor property);
        void RemoveIndex(PropertyDescriptor property);
        object AddNew();
        void ApplySort(PropertyDescriptor prop, ListSortDirection direction);
        int Find(PropertyDescriptor prop, object key);
        void RemoveSort();
    }
}