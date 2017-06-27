namespace SSD_Status.WPF.ViewModels
{
    public class GridPropertyViewModel
    {
        public GridPropertyViewModel(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public string Value { get; private set; }
    }
}
