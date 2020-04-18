namespace PasteProperty.ValueRepository
{
    public interface IValueRepository
    {
        string GetMainValue();
        string GetValue(int position);
        string SetValue(int position, string value); //fluent
    }
}