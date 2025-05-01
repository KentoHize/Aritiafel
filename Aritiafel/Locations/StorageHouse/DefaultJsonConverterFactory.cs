using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aritiafel.Locations.StorageHouse
{
    public enum ReferenceTypeReadAndWritePolicy
    {
        AssemblyQualifiedName = 0,
        TypeFullName,
        TypeNestedName
    }

    public enum SpecialCharHandlingPolicy
    {
        Change = 0,
        Unchange
    }

    /// <summary>
    /// System.Text.Json加強版，繼承後製作自訂JsonConverter
    /// </summary>
    public class DefaultJsonConverterFactory : JsonConverterFactory
    {
        public string ReferenceTypeString
        {
            get => _ReferenceTypeString;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException(nameof(ReferenceTypeString));
                _ReferenceTypeString = value;
            }
        }

        private string _ReferenceTypeString = "__ReferenceType";
        public string SpecialCharPrefix
        {
            get => _SpecialCharPrefix;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(SpecialCharPrefix));
                _SpecialCharPrefix = value;
            }
        }
        private string _SpecialCharPrefix = "AR";
        public ReferenceTypeReadAndWritePolicy ReferenceTypeReadAndWritePolicy { get; set; }
        public SpecialCharHandlingPolicy SpecialCharHandlingPolicy { get; set; }
        public override bool CanConvert(Type typeToConvert)
        {
            if (SpecialCharHandlingPolicy == SpecialCharHandlingPolicy.Change &&
                (typeToConvert == typeof(string) || typeToConvert == typeof(char)))
                return true;
            if ((typeToConvert.IsClass || typeToConvert.IsInterface) &&
                !typeToConvert.FullName.StartsWith("System") &&
                !typeToConvert.FullName.StartsWith("Microsoft"))
                return true;
            return false;
        }
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
            => (JsonConverter)Activator.CreateInstance(
                typeof(DefaultJsonConverter<>).MakeGenericType(new Type[] { typeToConvert }),
                BindingFlags.Instance | BindingFlags.Public, null, new object[] { ReferenceTypeReadAndWritePolicy, SpecialCharHandlingPolicy, _ReferenceTypeString, _SpecialCharPrefix }, null);

        public DefaultJsonConverterFactory(ReferenceTypeReadAndWritePolicy referenceTypeReadAndWritePolicy = ReferenceTypeReadAndWritePolicy.TypeFullName,
            SpecialCharHandlingPolicy specialCharHandlingPolicy = SpecialCharHandlingPolicy.Change)
        {
            ReferenceTypeReadAndWritePolicy = referenceTypeReadAndWritePolicy;
            SpecialCharHandlingPolicy = specialCharHandlingPolicy;
        }

        protected class DefaultJsonConverter<T> : JsonConverter<T>
        {
            protected string ReferenceTypeString { get; set; }
            protected string SpecialCharPrefix { get; set; }
            public ReferenceTypeReadAndWritePolicy ReferenceTypeReadAndWritePolicy { get; set; }
            public SpecialCharHandlingPolicy SpecialCharHandlingPolicy { get; set; }

            private static readonly object skipObject = new object();

            public DefaultJsonConverter()
                : this(ReferenceTypeReadAndWritePolicy.TypeFullName, SpecialCharHandlingPolicy.Change, "__ReferenceType", "AR")
            { }
            public DefaultJsonConverter(ReferenceTypeReadAndWritePolicy referenceTypeReadAndWritePolicy,
                SpecialCharHandlingPolicy specialCharHandlingPolicy,
                string referenceTypeString, string specialCharPrefix)
            {
                ReferenceTypeReadAndWritePolicy = referenceTypeReadAndWritePolicy;
                SpecialCharHandlingPolicy = specialCharHandlingPolicy;
                ReferenceTypeString = referenceTypeString;
                SpecialCharPrefix = specialCharPrefix;
            }
            public virtual void SetPropertyValue(string propertyName, object instance, object value)
                => instance.GetType().GetProperty(propertyName)?.SetValue(instance, value);
            public virtual object GetPropertyValueAndWrite(string propertyName, object instance, bool skip = false)
                => skip ? skipObject : instance.GetType().GetProperty(propertyName)?.GetValue(instance);
            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (typeToConvert == typeof(string))
                    return (T)Convert.ChangeType(RecoverSpecialChar(reader.GetString()), typeof(T));
                else if (typeToConvert == typeof(char))
                    return (T)Convert.ChangeType(RecoverSpecialChar(reader.GetString())[0], typeof(T));

                if (reader.TokenType != JsonTokenType.StartObject)
                    throw new JsonException($"Token is not StartObject: {reader.TokenType}");
                reader.Read();
                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException($"Token is not PropertyName: {reader.TokenType}");
                string propertyName = reader.GetString();
                object result;
                if (propertyName == ReferenceTypeString)
                {
                    reader.Read();
                    if (ReferenceTypeReadAndWritePolicy == ReferenceTypeReadAndWritePolicy.AssemblyQualifiedName)
                        result = Activator.CreateInstance(Type.GetType(reader.GetString()));
                    else if (ReferenceTypeReadAndWritePolicy == ReferenceTypeReadAndWritePolicy.TypeFullName)
                        result = Activator.CreateInstance(Type.GetType($"{reader.GetString()}, {typeToConvert.Assembly.FullName}"));
                    else
                        result = Activator.CreateInstance(Type.GetType($"{typeToConvert.Namespace}.{reader.GetString()}, {typeToConvert.Assembly.FullName}"));
                    reader.Read();
                    if (reader.TokenType != JsonTokenType.PropertyName)
                        throw new JsonException($"Token is not PropertyName: {reader.TokenType}");
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
                                    SetPropertyValue(propertyName, result, JsonSerializer.Deserialize(
                                        buffer.ToString(), resultType.GetProperty(propertyName).PropertyType,
                                        options));
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
                                    SetPropertyValue(propertyName, result, JsonSerializer.Deserialize(
                                        buffer.ToString(), resultType.GetProperty(propertyName).PropertyType,
                                        options));
                                buffer.Clear();
                            }
                            break;
                        case JsonTokenType.True:
                        case JsonTokenType.False:
                            if (depth != 0)
                                buffer.AppendFormat("{0},", reader.GetBoolean().ToString().ToLower());
                            else
                                if (resultType.GetProperty(propertyName).CanWrite)
                                SetPropertyValue(propertyName, result, reader.GetBoolean());
                            break;
                        case JsonTokenType.Number:
                            string numberString = new string(reader.ValueSpan.ToArray().Select(m => (char)m).ToArray());
                            if (depth != 0)
                                buffer.AppendFormat("{0},", numberString);
                            else if (resultType.GetProperty(propertyName).CanWrite)
                            {
                                Type pType = resultType.GetProperty(propertyName).PropertyType;
                                if (pType.IsEnum)
                                    SetPropertyValue(propertyName, result, Convert.ToInt32(numberString));
                                else if (Nullable.GetUnderlyingType(pType) != null)
                                    SetPropertyValue(propertyName, result, Convert.ChangeType(numberString, Nullable.GetUnderlyingType(pType)));
                                else
                                    SetPropertyValue(propertyName, result, Convert.ChangeType(numberString, pType));
                            }
                            break;
                        case JsonTokenType.Null:
                            if (depth != 0)
                                buffer.Append("null,");
                            else
                                if (resultType.GetProperty(propertyName).CanWrite)
                                SetPropertyValue(propertyName, result, null);
                            break;
                        case JsonTokenType.String:
                            if (depth != 0)
                                buffer.AppendFormat("\"{0}\",", reader.GetString());
                            else if (resultType.GetProperty(propertyName).CanWrite)
                                if (resultType.GetProperty(propertyName).PropertyType.IsEnum)
                                    SetPropertyValue(propertyName, result,
                                        Enum.Parse(resultType.GetProperty(propertyName).PropertyType,
                                        reader.GetString()));
                                else if (resultType.GetProperty(propertyName).PropertyType == typeof(char))
                                    if (SpecialCharHandlingPolicy == SpecialCharHandlingPolicy.Change)
                                        SetPropertyValue(propertyName, result, RecoverSpecialChar(reader.GetString())[0]);
                                    else
                                        SetPropertyValue(propertyName, result, reader.GetString()[0]);
                                else
                                    if (SpecialCharHandlingPolicy == SpecialCharHandlingPolicy.Change)
                                    SetPropertyValue(propertyName, result, RecoverSpecialChar(reader.GetString()));
                                else
                                    SetPropertyValue(propertyName, result, reader.GetString());
                            break;
                        default:
                            throw new JsonException("Default");
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
                if (valueType == typeof(string) || valueType == typeof(char))
                {
                    writer.WriteStringValue(ReplaceSpecialChar(value.ToString()));
                    return;
                }

                PropertyInfo[] pis = valueType.GetProperties();
                writer.WriteStartObject();
                if (valueType != typeof(T))
                    if (ReferenceTypeReadAndWritePolicy == ReferenceTypeReadAndWritePolicy.AssemblyQualifiedName)
                        writer.WriteString(ReferenceTypeString, valueType.AssemblyQualifiedName);
                    else if (ReferenceTypeReadAndWritePolicy == ReferenceTypeReadAndWritePolicy.TypeFullName)
                        writer.WriteString(ReferenceTypeString, valueType.FullName);
                    else
                        writer.WriteString(ReferenceTypeString, valueType.GetNestedTypeName());
                foreach (PropertyInfo pi in pis)
                {
                    //跳過Indexer及Static
                    if (pi.GetAccessors(true)[0].IsStatic || pi.GetIndexParameters().Length != 0)
                        continue;
                    object p_value = GetPropertyValueAndWrite(pi.Name, value);

                    if (p_value == null)
                        writer.WriteNull(pi.Name);
                    else if (p_value == skipObject)
                        continue;
                    else
                    {
                        if (SpecialCharHandlingPolicy == SpecialCharHandlingPolicy.Change && (p_value is string || p_value is char))
                            p_value = ReplaceSpecialChar(p_value.ToString());
                        JsonConverter jc = options.GetConverter(p_value.GetType());
                        writer.WritePropertyName(pi.Name);
                        jc.GetType().GetMethod("Write").Invoke(jc, new object[] { writer, p_value, options });
                    }
                }
                writer.WriteEndObject();
            }
            private string RecoverSpecialChar(string s)
            {
                int startIndex = 0;
                string result = s.Clone() as string;
                startIndex = result.IndexOf($"[{SpecialCharPrefix}:", startIndex);
                while (startIndex != -1 && result.Length > startIndex + SpecialCharPrefix.Length + 6)
                {
                    if (result[startIndex + 8] == ']')
                    {
                        result = result.Remove(startIndex + SpecialCharPrefix.Length + 6, 1);
                        result = result.Replace(result.Substring(startIndex, SpecialCharPrefix.Length + 6),
                            ((char)Convert.ToByte(result.Substring(startIndex + SpecialCharPrefix.Length + 2, 4), 16)).ToString());
                    }
                    startIndex++;
                    startIndex = result.IndexOf($"[{SpecialCharPrefix}:", startIndex);
                }
                return result;
            }
            private string ReplaceSpecialChar(char c)
            {
                if ((c >= 0 && c <= 31) || c == '\"' || c == '\\')
                    return string.Format("[{0}:{1:X4}]", SpecialCharPrefix, (int)c);
                return c.ToString();
            }
            private string ReplaceSpecialChar(string s)
            {
                StringBuilder result = new StringBuilder();
                for (int i = 0; i < s.Length; i++)
                    result.Append(ReplaceSpecialChar(s[i]));
                return result.ToString();
            }
        }
    }

}
