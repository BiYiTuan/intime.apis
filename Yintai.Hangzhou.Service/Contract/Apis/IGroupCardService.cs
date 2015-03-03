using System;
using System.Runtime.Serialization;

namespace Yintai.Hangzhou.Service.Contract.Apis
{
    public interface IGroupCardService
    {
        GroupCardPointResult GetPoint(GroupCardPointRequest request);

        GroupCardInfoResult GetInfo(GroupCardInfoRequest request);

        GroupExchangeResult Exchange(GroupExchangeRequest request);
    }

    [DataContract(Name = "vipCard")]
    public abstract class GroupCardRequest
    {
    }

    [DataContract(Name = "vipCard")]
    public class GroupCardInfoRequest
    {
        [DataMember(Name = "cardno")]
        public string CardNo { get; set; }

        [DataMember(Name = "passwd")]
        public string Passwd { get; set; }
    }

    [DataContract(Name = "vipCard")]
    public class GroupCardPointRequest
    {
        [DataMember(Name = "cardno")]
        public string CardNo { get; set; }
    }
    [DataContract(Name = "vipCard")]
    public class GroupExchangeRequest
    {
        [DataMember(Name = "cardno")]
        public string CardNo { get; set; }
        [DataMember(Name = "identityno")]
        public string IdentityNo { get; set; }
    }
    [DataContract]
    public abstract class GroupCardResult
    {
    }

    [DataContract]
    public class GroupCardPointResult : GroupCardResult
    {
        [DataMember(Name = "Success")]
        public bool Success { get; set; }

        [DataMember(Name = "ErrorCode")]
        public int ErrorCode { get; set; }

        [DataMember(Name = "Desc")]
        public string Desc { get; set; }

        [DataMember(Name = "Point")]
        public decimal Point { get; set; }
    }

    public enum GroupCardRetType
    {
        Default = 0,

        Ok = 1
    }

    [DataContract]
    public class GroupCardInfoResult : GroupCardResult
    {
        [DataMember(Name = "Ret")]
        public int Ret { get; set; }

        [IgnoreDataMember]
        public GroupCardRetType RetType
        {
            get { return (GroupCardRetType)Ret; }
            set { }
        }

        [DataMember(Name = "Desc")]
        public string Desc { get; set; }

        [DataMember(Name = "Lvl")]
        public string Lvl { get; set; }

        [DataMember(Name = "Type")]
        public string Type { get; set; }
    }
    [DataContract]
    public class GroupExchangeResult : GroupCardResult
    {
        [DataMember(Name = "Success")]
        public bool Success { get; set; }

        [DataMember(Name = "ErrorCode")]
        public int ErrorCode { get; set; }

        [DataMember(Name = "Desc")]
        public string Desc { get; set; }

        [DataMember(Name = "Point")]
        public decimal Point { get; set; }
    }
}
