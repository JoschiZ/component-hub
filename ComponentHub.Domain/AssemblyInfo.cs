
using StronglyTypedIds;

[assembly:StronglyTypedIdDefaults(converters: StronglyTypedIdConverter.SystemTextJson | StronglyTypedIdConverter.EfCoreValueConverter | StronglyTypedIdConverter.TypeConverter)]