namespace System.Windows.Forms
{
    public class ComponentConverter : TypeConverter
    {
        private Type type;

        public ComponentConverter(Type type)
        {
            this.type = type;
        }
    }
}