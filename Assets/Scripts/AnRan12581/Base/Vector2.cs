using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Vector2Json.SaveSystem
{
    public static class AddSerializedJson
    {
        public static void AddAllConverter()
        {
            AddVector2Converter();
        }

        private static void AddVector2Converter()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Converters = { new Vector2Converter() }
            };
        }
    }

    public class Vector2Converter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value,
            JsonSerializer serializer)
        {
            var vector = (Vector2)value;
            var obj = new JObject
            {
                { "x", vector.x },
                { "y", vector.y }
            };
            obj.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var x = (float)obj["x"];
            var y = (float)obj["y"];

            return new Vector2(x, y);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Vector2);
        }
    }
}


namespace Vector3Json.SaveSystem
{
    public static class AddSerializedJson
    {
        public static void AddAllConverter()
        {
            AddVector3Converter();
        }

        private static void AddVector3Converter()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Converters = { new Vector3Converter() }
            };
        }
    }

    public class Vector3Converter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value,
            JsonSerializer serializer)
        {
            var vector = (Vector3)value;
            var obj = new JObject
            {
                { "x", vector.x },
                { "y", vector.y },
                { "z", vector.z }
            };
            obj.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var x = (float)obj["x"];
            var y = (float)obj["y"];
            var z = (float)obj["z"];
            return new Vector3(x, y, z);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Vector3);
        }
    }
}
