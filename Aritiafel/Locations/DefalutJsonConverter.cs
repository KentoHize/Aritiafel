using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Aritiafel.Locations.StorageHouse
{
    /// <summary>
    /// System.Text.Json加強版，繼承後製作自訂JsonConverter
    /// </summary>
    public class DefalutJsonConverter : JsonConverterFactory
    {
        private const string ReferenceType = "__ReferenceType";
        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.IsClass &&
                !typeToConvert.Assembly.FullName.StartsWith("System") &&
                !typeToConvert.Assembly.FullName.StartsWith("Mircosoft"))
                return true;
            return false;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
            => (JsonConverter)Activator.CreateInstance(
                typeof(DefalutJsonConverterInner<>).MakeGenericType(new Type[] { typeToConvert }),
                BindingFlags.Instance | BindingFlags.Public, null, new object[] { }, null);

        protected class DefalutJsonConverterInner<T> : JsonConverter<T>
        {
            public DefalutJsonConverterInner()
            { }

            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartObject)
                    throw new JsonException();
                reader.Read();
                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException();
                string propertyName = reader.GetString();                
                object result;
                if (propertyName == ReferenceType)
                {
                    reader.Read();
                    result = Activator.CreateInstance(Type.GetType(reader.GetString()));
                    reader.Read();
                    if (reader.TokenType != JsonTokenType.PropertyName)
                        throw new JsonException();
                    propertyName = reader.GetString();
                }
                else
                    result = Activator.CreateInstance(typeToConvert);

                Type resultType = result.GetType();
                int depth = 0;
                StringBuilder buffer = new StringBuilder();
                while (reader.Read())
                {                 
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.PropertyName:
                            if (depth != 0)
                                buffer.AppendFormat("\"{0}\":", reader.GetString());
                            else
                                propertyName = reader.GetString();
                            break;
                        case JsonTokenType.StartObject:
                            buffer.Append("{ ");
                            depth++;
                            break;
                        case JsonTokenType.StartArray:
                            buffer.Append("[ ");
                            depth++;
                            break;
                        case JsonTokenType.EndObject:
                            if (buffer.ToString() == "")
                                return (T)result;
                            buffer.Remove(buffer.Length - 1, 1);
                            buffer.Append("},");
                            depth--;
                            if (depth == 0)
                            {
                                buffer.Remove(buffer.Length - 1, 1);
                                if (resultType.GetProperty(propertyName).CanWrite)
                                    resultType.GetProperty(propertyName).SetValue(result,
                                    JsonSerializer.Deserialize(buffer.ToString(),
                                    resultType.GetProperty(propertyName).PropertyType, options));
                                buffer.Clear();
                            }
                            break;
                        case JsonTokenType.EndArray:
                            if (buffer.ToString() == "")
                                return (T)result;
                            buffer.Remove(buffer.Length - 1, 1);
                            buffer.Append("],");
                            depth--;
                            if (depth == 0)
                            {
                                buffer.Remove(buffer.Length - 1, 1);
                                if (resultType.GetProperty(propertyName).CanWrite)
                                    resultType.GetProperty(propertyName).SetValue(result,
                                    JsonSerializer.Deserialize(buffer.ToString(),
                                    resultType.GetProperty(propertyName).PropertyType, options));
                                buffer.Clear();
                            }
                            break;
                        case JsonTokenType.True:
                        case JsonTokenType.False:
                            if (depth != 0)
                                buffer.AppendFormat("{0},", reader.GetBoolean().ToString().ToLower());
                            else
                                if (resultType.GetProperty(propertyName).CanWrite)
                                resultType.GetProperty(propertyName).SetValue(result, reader.GetBoolean());
                            break;
                        case JsonTokenType.Number:
                            double d = reader.GetDouble();
                            decimal m = reader.GetDecimal();
                            object o;
                            if (Convert.ToDouble(m) == d)
                                o = m;
                            else
                                o = d;
                            if (depth != 0) 
                                buffer.AppendFormat("{0},", o);
                            else
                                if (resultType.GetProperty(propertyName).CanWrite)
                                    resultType.GetProperty(propertyName).SetValue(result,
                                        Convert.ChangeType(o, resultType.GetProperty(propertyName).PropertyType));
                            break;
                        case JsonTokenType.Null:
                            if (depth != 0)
                                buffer.Append("null,");
                            else
                                if (resultType.GetProperty(propertyName).CanWrite)
                                resultType.GetProperty(propertyName).SetValue(result, null);
                            break;
                        case JsonTokenType.String:
                            if (depth != 0)
                                buffer.AppendFormat("\"{0}\",", reader.GetString());
                            else
                                if (resultType.GetProperty(propertyName).CanWrite)
                                    resultType.GetProperty(propertyName).SetValue(result, reader.GetString());
                            break;
                        default:
                            throw new JsonException("Default is met.");
                    }
                }
                return (T)result;
            }
            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                if (value == null)
                {
                    writer.WriteNullValue();
                    return;
                }

                Type valueType = value.GetType();
                PropertyInfo[] pis = valueType.GetProperties();
                writer.WriteStartObject();
                if (valueType != typeof(T))
                    writer.WriteString(ReferenceType, valueType.AssemblyQualifiedName);
                foreach (PropertyInfo pi in pis)
                {
                    if (pi.GetAccessors(true)[0].IsStatic)
                        continue;
                    object p_value = pi.GetValue(value);
                    if (p_value == null)
                    {
                        writer.WriteNull(pi.Name);
                        continue;
                    }
                    else
                    {
                        JsonConverter jc = options.GetConverter(p_value.GetType());
                        writer.WritePropertyName(pi.Name);
                        jc.GetType().GetMethod("Write").Invoke(jc, new object[] { writer, p_value, options });
                    }
                }
                writer.WriteEndObject();
            }
        }
    }

}
