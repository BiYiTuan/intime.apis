using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Passbook.Generator.Fields
{
    public class Location
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public double? Altitude { get; set; }

        public string RelevantText { get; set; }

        internal void Write(JsonWriter writer)
        {

            writer.WriteStartObject();

            writer.WritePropertyName("longitude");
            writer.WriteValue(this.Longitude);


            writer.WritePropertyName("latitude");
            writer.WriteValue(this.Latitude);

            if (Altitude != null)
            {
                writer.WritePropertyName("altitude");
                writer.WriteValue(this.Altitude.Value);
            }

            if (!String.IsNullOrEmpty(RelevantText))
            {
                writer.WritePropertyName("relevantText");
                writer.WriteValue(this.RelevantText);
            }

            writer.WriteEndObject();
        }
    }
}
