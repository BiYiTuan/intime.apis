using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Yintai.Hangzhou.Contract.Response;
using System.Linq;

namespace Yintai.Hangzhou.Contract.DTO.Response.Brand
{
    [DataContract]
    public class BrandInfo : BaseResponse
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "englishname")]
        public string EnglishName { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [IgnoreDataMember]
        public int CreatedUser { get; set; }

        [IgnoreDataMember]
        public System.DateTime CreatedDate { get; set; }

        [IgnoreDataMember]
        public System.DateTime UpdatedDate { get; set; }

        [IgnoreDataMember]
        public int UpdatedUser { get; set; }

        [DataMember(Name = "logo")]
        public string Logo { get; set; }

        [DataMember(Name = "website")]
        public string WebSite { get; set; }

        [IgnoreDataMember]
        public int Status { get; set; }

        [DataMember(Name = "group")]
        public string Group { get; set; }
    }

    [DataContract]
    public class BrandInfoResponse : BrandInfo
    {
    }

    [DataContract]
    public class GroupStructItemInfoResponse<T> : BaseResponse
    {
        /// <summary>
        /// Name
        /// </summary>
        [DataMember(Name = "groupname")]
        public string GroupName { get; set; }

        [DataMember(Name = "groupval")]
        public List<T> GroupValue { get; set; }
    }

    [DataContract]
    public class GroupStructInfoResponse<T>
    {
        public GroupStructInfoResponse()
        {
            InternalValues = new Dictionary<string, List<T>>();
        }

        public bool ContainsKey(string key)
        {
            return InternalValues.ContainsKey(key);
        }

        public List<T> this[string key]
        {
            get { return InternalValues[key]; }
            set { InternalValues[key] = value; }
        }

        public void Add(string key, List<T> value)
        {
            InternalValues.Add(key, value);
        }

        [IgnoreDataMember]
        protected Dictionary<string, List<T>> InternalValues { get; set; }

        [DataMember(Name = "values")]
        public IEnumerable<GroupStructItemInfoResponse<T>> Values
        {
            get
            {
                var sortKeys = InternalValues.Keys.OrderBy(v => v);

                foreach (var key in sortKeys)
                {
                    yield return new GroupStructItemInfoResponse<T>
                        {
                            GroupName = key,
                            GroupValue = InternalValues[key]
                        };
                }
            }

            set { }
        }
    }

    public class T : IDictionary<string, List<T>>
    {
        public IEnumerator<KeyValuePair<string, List<T>>> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<string, List<T>> item)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, List<T>> item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, List<T>>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, List<T>> item)
        {
            throw new System.NotImplementedException();
        }

        public int Count { get; private set; }
        public bool IsReadOnly { get; private set; }
        public bool ContainsKey(string key)
        {
            throw new System.NotImplementedException();
        }

        public void Add(string key, List<T> value)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetValue(string key, out List<T> value)
        {
            throw new System.NotImplementedException();
        }

        public List<T> this[string key]
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public ICollection<string> Keys { get; private set; }
        public ICollection<List<T>> Values { get; private set; }
    }
}
