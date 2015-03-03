using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model
{
    public abstract class BaseModel
    {
        public static T FromEntity<T>(dynamic entity) where T : class
        {
            return FromEntity<T>(entity, null) as T;
        }

        public static T FromEntity<T>(dynamic entity, Action<T> moreMapping) where T : class
        {
            if (entity == null)
                return default(T);
            T model = AutoMapper.Mapper.DynamicMap(entity, entity.GetType(), typeof(T)) as T;
            if (moreMapping != null)
            {
                moreMapping(model);
            }
            return model;
        }
    }
}
