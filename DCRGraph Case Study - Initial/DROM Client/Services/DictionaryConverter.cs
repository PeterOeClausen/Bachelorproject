using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DROM_Client.Models.BusinessObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DROM_Client.Services
{
    public class DictionaryConverter : JsonConverter
    {
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            IDictionary<Item, int> result;
            /*
            if (reader.TokenType == JsonToken.StartArray)
            {
                JArray legacyArray = (JArray)JArray.ReadFrom(reader);

                result = legacyArray.ToDictionary(
                    el => el["Key"],
                    el => el["Value"]);
            }
            else
            {
            */
                result =
                    (IDictionary<Item, int>)
                        serializer.Deserialize(reader, typeof(IDictionary<Item, int>));
            

            return result;
        }

        public override void WriteJson(
            JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IDictionary<Item, int>).IsAssignableFrom(objectType);
        }

        public override bool CanWrite
        {
            get { return false; }
        }
    }
}
