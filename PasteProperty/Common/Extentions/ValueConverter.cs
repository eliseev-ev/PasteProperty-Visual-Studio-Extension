namespace PasteProperty.Common.Extentions
{
    public static class ValueConverter
    {
        public static string ToField(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            value.Trim();

            if (value[0] == '_')
            {
                value = value.Substring(1);
            }
            value = char.ToUpper(value[0]) + value.Substring(1);

            return value;

        }
    }
}
