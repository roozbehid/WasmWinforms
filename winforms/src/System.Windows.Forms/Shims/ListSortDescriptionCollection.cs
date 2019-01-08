namespace System.Windows.Forms
{
    public class ListSortDescriptionCollection
    {
        private ListSortDescription[] sort_descs;

        public ListSortDescriptionCollection(ListSortDescription[] sort_descs)
        {
            this.sort_descs = sort_descs;
        }
    }
}