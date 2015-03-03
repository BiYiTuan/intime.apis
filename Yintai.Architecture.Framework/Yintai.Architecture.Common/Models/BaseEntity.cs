namespace Yintai.Architecture.Common.Models
{
    [System.Serializable]
    public abstract class BaseEntity
    {
        /// <summary>
        /// KeyMemberId
        /// </summary>
        public abstract object EntityId { get; }
    }
}
