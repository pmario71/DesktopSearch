using DesktopSearch.Core.DataModel;
using DesktopSearch.Core.DataModel.Code;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopSearch.Core.ElasticSearch
{
    /// <summary>
    /// Fixes Serialization issue when contract defines interfaces and JSON library cannot deserialize it back to the right type.
    /// </summary>
    public class CustomIDescriptorConverter : CustomCreationConverter<IList<IDescriptor>>
    {
        private readonly Dictionary<int, Func<JToken, IDescriptor>> _dictionary;

        public CustomIDescriptorConverter()
        {
            var dict = new Dictionary<int, Func<JToken, IDescriptor>>()
            {
                { (int)MemberType.Field, t => t.ToObject<FieldDescriptor>() },
                { (int)MemberType.Method, t => t.ToObject<MethodDescriptor>() },
                { (int)MemberType.Property, t => t.ToObject<FieldDescriptor>() },
            };
            _dictionary = dict;
        }


        public override IList<IDescriptor> Create(Type objectType)
        {
            return null;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token == null || token.Type != JTokenType.Array)
                return null;

            var list = new List<IDescriptor>();
            foreach (var item in token.Children())
            {
                list.Add(_dictionary[item["type"].Value<int>()](item));
            }

            return list;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
