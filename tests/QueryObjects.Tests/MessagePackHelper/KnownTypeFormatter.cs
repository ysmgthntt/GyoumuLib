using MessagePack.Formatters;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace QueryObjects.Tests.MessagePackHelper
{
    internal sealed class KnownTypeFormatter<T> : IMessagePackFormatter<T?>
    {
        private readonly Dictionary<string, Type> _knownTypes = new();

        private delegate void SerializeMethod(object formatter, ref MessagePackWriter writer, object value, MessagePackSerializerOptions options);
        private delegate object DeserializeMethod(object formatter, ref MessagePackReader reader, MessagePackSerializerOptions options);

        private readonly ConcurrentDictionary<string, SerializeMethod> _serializers = new();
        private readonly ConcurrentDictionary<string, DeserializeMethod> _deserializers = new();

        public KnownTypeFormatter(Type[] knownTypes)
        {
            foreach (var type in knownTypes)
                _knownTypes.Add(type.Name, type);
        }

        public void Serialize(ref MessagePackWriter writer, T? value, MessagePackSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNil();
            }
            else
            {
                var type = value.GetType();
                var typeName = type.Name;
                writer.Write(typeName);

                var formatter = options.Resolver.GetFormatterDynamic(type);
                if (formatter is null)
                    throw new FormatterNotRegisteredException(typeName);

                if (!_serializers.TryGetValue(typeName, out var serializeMethod))
                {
                    TypeInfo ti = type.GetTypeInfo();

                    Type formatterType = typeof(IMessagePackFormatter<>).MakeGenericType(type);
                    ParameterExpression param0 = Expression.Parameter(typeof(object), "formatter");
                    ParameterExpression param1 = Expression.Parameter(typeof(MessagePackWriter).MakeByRefType(), "writer");
                    ParameterExpression param2 = Expression.Parameter(typeof(object), "value");
                    ParameterExpression param3 = Expression.Parameter(typeof(MessagePackSerializerOptions), "options");

                    MethodInfo serializeMethodInfo = formatterType.GetRuntimeMethod("Serialize", new[] { typeof(MessagePackWriter).MakeByRefType(), type, typeof(MessagePackSerializerOptions) })!;

                    MethodCallExpression body = Expression.Call(
                        Expression.Convert(param0, formatterType),
                        serializeMethodInfo,
                        param1,
                        ti.IsValueType ? Expression.Unbox(param2, type) : Expression.Convert(param2, type),
                        param3);

                    serializeMethod = Expression.Lambda<SerializeMethod>(body, param0, param1, param2, param3).Compile();

                    _serializers.TryAdd(typeName, serializeMethod);
                }

                serializeMethod(formatter, ref writer, value, options);
            }
        }

        public T? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            if (reader.IsNil)
            {
                reader.ReadNil();
                return default(T);
            }
            else
            {
                var typeName = reader.ReadString()!;
                if (!_knownTypes.TryGetValue(typeName, out var type))
                    return default(T);

                var formatter = options.Resolver.GetFormatterDynamic(type);
                if (formatter is null)
                    throw new FormatterNotRegisteredException(typeName);

                if (!_deserializers.TryGetValue(typeName, out var deserializeMethod))
                {
                    TypeInfo ti = type.GetTypeInfo();

                    Type formatterType = typeof(IMessagePackFormatter<>).MakeGenericType(type);
                    ParameterExpression param0 = Expression.Parameter(typeof(object), "formatter");
                    ParameterExpression param1 = Expression.Parameter(typeof(MessagePackReader).MakeByRefType(), "reader");
                    ParameterExpression param2 = Expression.Parameter(typeof(MessagePackSerializerOptions), "options");

                    MethodInfo deserializeMethodInfo = formatterType.GetRuntimeMethod("Deserialize", new[] { typeof(MessagePackReader).MakeByRefType(), typeof(MessagePackSerializerOptions) })!;

                    MethodCallExpression deserialize = Expression.Call(
                        Expression.Convert(param0, formatterType),
                        deserializeMethodInfo,
                        param1,
                        param2);

                    Expression body = deserialize;
                    if (ti.IsValueType)
                    {
                        body = Expression.Convert(deserialize, typeof(object));
                    }

                    deserializeMethod = Expression.Lambda<DeserializeMethod>(body, param0, param1, param2).Compile();

                    _deserializers.TryAdd(typeName, deserializeMethod);
                }

                return (T)deserializeMethod(formatter, ref reader, options);
            }
        }
    }
}
