using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ComponentHub.DB.Configuration.Converters;

internal sealed class EnumListStringConverter<TEnum>: ValueConverter<IEnumerable<TEnum>, string> where TEnum: struct, Enum
{
    public EnumListStringConverter()
        : base(
            enumValue => string.Join(",", enumValue.ToString()),
            enumString => enumString.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(Enum.Parse<TEnum>))
    {
    }
}