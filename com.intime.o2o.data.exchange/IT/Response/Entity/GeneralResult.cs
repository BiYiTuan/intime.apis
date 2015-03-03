using System.Runtime.Serialization;

namespace com.intime.o2o.data.exchange.IT.Response.Entity
{
    [DataContract]
    public class GeneralResult
    {
        [DataMember(Name = "desc")]
        public string Desc { get; set; }

        internal virtual string[] ParseDesc()
        {
            return this.Desc.Replace("|||", "|").Replace("||", "|").TrimEnd('|').Split('|');
        }
    }
}