using MessagePack.Formatters;
using MessagePack.Resolvers;
using System.Reflection;

namespace QueryObjects.Tests.MessagePackHelper
{
    internal sealed class KnownTypeResolver : IFormatterResolver
    {
        public static readonly MessagePackSerializerOptions StandardAllowPrivateWithKnownTypeOptions = StandardResolverAllowPrivate.Options.WithResolver(
            CompositeResolver.Create(
                StandardResolverAllowPrivate.Instance,
                new KnownTypeResolver()
                ));

        public IMessagePackFormatter<T>? GetFormatter<T>()
            => FormatterCache<T>.Formatter;

        private static class FormatterCache<T>
        {
            public static readonly IMessagePackFormatter<T>? Formatter;

            static FormatterCache()
            {
                var t = typeof(T);
                var knownTypeAttrs = t.GetCustomAttributes(typeof(KnownTypeAttribute), false);
                if (knownTypeAttrs.Length > 0)
                {
                    List<Type> knownTypes = new();
                    foreach (KnownTypeAttribute attr in knownTypeAttrs)
                    {
                        if (attr.Type is not null)
                        {
                            knownTypes.Add(attr.Type);
                        }
                        else if (!string.IsNullOrEmpty(attr.MethodName))
                        {
                            var method = t.GetMethod(attr.MethodName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                            if (method is not null)
                            {
                                var types = method.Invoke(null, Array.Empty<object>());
                                if (types is not null)
                                {
                                    foreach (Type type in (IEnumerable<Type>)types)
                                        knownTypes.Add(type);
                                }
                            }
                        }
                    }
                    Formatter = new KnownTypeFormatter<T>(knownTypes.ToArray());
                }
            }
        }
    }
}
