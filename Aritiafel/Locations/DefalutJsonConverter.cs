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

        private class DefalutJsonConverterInner<T> : JsonConverter<T>
        {
            public DefalutJsonConverterInner()
            { }

            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {

                //return base.Read(ref reader, typeToConvert, options);                
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
