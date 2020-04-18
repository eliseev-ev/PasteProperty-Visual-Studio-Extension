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
            value = char.ToLower(value[0]) + value.Substring(1);

            return value;
        }

        public static string ToPrivateField(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            value.Trim();

            if (value[0] == '_')
            {
                value = "_" + char.ToLower(value[1]) + value.Substring(2);
            }
            else
            {
                value = "_" + char.ToLower(value[0]) + value.Substring(1);
            }
            return value;
        }


        public static string ToProperty(this string value)
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
