namespace System.ComponentModel.DataAnnotations
{
    public class NonDefaultAttribute : ValidationAttribute
    {
        private const string DEFAULT_ERROR_MESSAGE = "The field {0} must be a non-default value.";

        private static ConcurrentDictionary<string, object> defaultInstancesCache = new();

        public NonDefaultAttribute() : base(DEFAULT_ERROR_MESSAGE)
        {
        }

        public override bool IsValid(object? value)
        {
            if (value is null)
                return true;

            var type = value.GetType();

            if (!defaultInstancesCache.TryGetValue(type.FullName!, out var defaultInstance))
            {
                defaultInstance = Activator.CreateInstance(Nullable.GetUnderlyingType(type) ?? type);
                defaultInstancesCache[type.FullName!] = defaultInstance!;
            }

            return !Equals(value, defaultInstance);
        }
    }
}
