using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ComponentHub.DB.Configuration.Converters;

internal sealed class DateTimeOffsetConverter: ValueConverter<DateTimeOffset, DateTimeOffset>
{
    public DateTimeOffsetConverter()
        : base(
            d => d.ToUniversalTime(),
            d => d.ToUniversalTime())
    {
    }
}