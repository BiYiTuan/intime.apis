namespace Yintai.Hangzhou.Contract.DTO.Request.Device
{
    public class DeviceRegisterRequest : CoordinateRequest
    {
        public string DeviceToken { get; set; }
        public string Uid { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
