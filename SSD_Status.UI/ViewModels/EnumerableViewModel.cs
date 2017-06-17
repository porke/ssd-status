using System;

namespace SSD_Status.WPF.ViewModels
{
    internal class EnumerableViewModel<T> where T : struct, IConvertible
    {
        internal EnumerableViewModel(T type, string description)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            Type = type;
            Description = description;
        }

        public T Type { get; private set; }
        public string Description { get; private set; }
    }
}
