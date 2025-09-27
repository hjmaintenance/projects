namespace MIT.Razor.Pages.Component.DataEdits.Data
{
    public class RadioGroupData
    {
        public string? GUID { get; set; } = Guid.NewGuid().ToString();
        public object? DisplayName { get; set; }
        public object? Value { get; set; }
        public override string ToString()
        {
            return DisplayName.ToString();
        }
    }
}
