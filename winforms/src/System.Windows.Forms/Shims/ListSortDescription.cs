using System.ComponentModel;

namespace System.Windows.Forms
{
    public class ListSortDescription
    {
        private PropertyDescriptor prop_desc;
        private ListSortDirection sort_direction;

        public ListSortDescription(PropertyDescriptor prop_desc, ListSortDirection sort_direction)
        {
            this.prop_desc = prop_desc;
            this.sort_direction = sort_direction;
        }

        public PropertyDescriptor PropertyDescriptor { get; set; }
        public ListSortDirection SortDirection { get; set; }
    }
}