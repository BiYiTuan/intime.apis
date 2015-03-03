using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.service.analysis.DTO
{
    abstract class EventResponse
    {
        private dynamic _json;

        protected abstract string ParaName { get; }

        public EventResponse SetContent(dynamic jsonObject)
        {
            _json = jsonObject;
            return this;
        }
        public IEnumerable<EventItem> Items
        {
            get
            {
                if (_json == null || !IsSuccess)
                    return null;
                if (_json.parameters == null)
                    return null;
                IEnumerable<dynamic> keys = _json.parameters.key;
                if (keys.Count() <= 0)
                    return null;
                IEnumerable<dynamic> values = keys.Where(k => k["@name"] == ParaName).First().value;

                return values.Select(p => new EventItem()
                {
                    Id = p["@name"],
                    Count = p["@totalCount"]
                });


            }
        }
        public bool IsSuccess
        {
            get
            {
                return _json.code == null;
            }
        }

        public string Error
        {
            get
            {
                return _json.message;
            }
        }
    }
}
