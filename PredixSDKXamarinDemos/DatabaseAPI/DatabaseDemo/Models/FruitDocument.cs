using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace DatabaseDemo
{
    public class FruitDocument
    {
        [JsonProperty(PropertyName = "_deleted")]
        public bool _Deleted { get; set; }

        [JsonProperty(PropertyName = "_id")]
        public string _ID { get; set; }

        [JsonProperty(PropertyName = "_rev")]
        public string _Rev { get; set; }

        [JsonProperty(PropertyName = "channels")]
        public List<string> Channels { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "fruit")]
        public string Fruit { get; set; }

        [JsonProperty(PropertyName = "createDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty(PropertyName = "lastChange")]
        public DateTime LastChange { get; set; }

        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
    }
}
