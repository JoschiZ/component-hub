using Microsoft.EntityFrameworkCore;

namespace ComponentHub.Shared.Helper;

internal static class DbContextExtensions
{
    public static int GetMaxLength<TEntity>(this DbContext context, string property, int defaultValue = int.MaxValue)
    {
        var entity = context.Model.FindEntityType(typeof(TEntity));
        var entityProperty = entity?.FindProperty(property);
        var maxLength = entityProperty?.GetMaxLength() ?? defaultValue;

        return maxLength;
    }
}