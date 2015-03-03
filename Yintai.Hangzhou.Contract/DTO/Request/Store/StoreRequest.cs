using System;
using Yintai.Architecture.Common.Models;

namespace Yintai.Hangzhou.Contract.DTO.Request.Store
{
    public class StoreRequest : BaseRequest
    {
        /// <summary>
        /// 店铺Id
        /// </summary>
        public int StoreId { get; set; }
    }

    public class StoreGetAllRequest : CoordinateRequest
    {
        public string Type { get; set; }
        public DateTime Refreshts { get; set; }

        public Timestamp Timestamp
        {
            get { return new Timestamp() { Ts = Refreshts, TsType = TimestampType.Old }; }
        }
    }

    public class StoreGetRefreshRequest : CoordinateRequest
    {
        public DateTime Refreshts { get; set; }

        public Timestamp Timestamp
        {
            get { return new Timestamp() { Ts = Refreshts, TsType = TimestampType.New }; }
        }
    }

    public abstract class StoreInfoRequest : AuthRequest
    {
        public int StoreId { get; set; }
        public int Id
        {
            get { return StoreId; }
            set { StoreId = value; }
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Tel { get; set; }

        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public int Group_Id
        {
            get { return GroupId; }
            set { GroupId = value; }
        }

        public int GroupId { get; set; }

        public int Region_Id
        {
            get { return RegionId; }
            set { RegionId = value; }
        }

        public int RegionId { get; set; }

        public int StoreLevel { get; set; }
    }

    public class StoreCreateRequest : StoreInfoRequest
    {
    }

    public class StoreUpdateRequest : StoreInfoRequest
    {
    }

    public class StoreDestroyRequest : AuthRequest
    {
        public int StoreId { get; set; }
    }
}
