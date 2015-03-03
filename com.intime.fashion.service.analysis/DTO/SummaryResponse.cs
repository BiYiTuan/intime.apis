using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.service.analysis.DTO
{
    public class SummaryResponse
    {
        private dynamic _json;
        public SummaryResponse(dynamic jsonObject)
        {
            _json = jsonObject;
        }

        public int Count
        {
            get
            {
                if (_json == null || !IsSuccess)
                    return 0;
                dynamic values = _json.day;
                if (values is JArray &&
                    values.Count() <= 0)
                    return 0;
                if (values is JArray)
                    return values.First()["@value"];
                else
                    return values["@value"];
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
