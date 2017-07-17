using System;

namespace SSD_Status.WPF.ViewModels
{
    internal class EnumerableViewModel<T> where T : struct, IConvertible
    {
        internal EnumerableViewModel(T value, string description)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            Value = value;
            Description = description;
        }

        public T Value { get; private set; }
        public string Description { get; private set; }
    }
}
