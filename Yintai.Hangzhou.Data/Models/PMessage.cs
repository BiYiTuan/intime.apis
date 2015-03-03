using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class PMessageEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public bool IsVoice { get; set; }
        public string TextMsg { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int FromUser { get; set; }
        public int ToUser { get; set; }
        public bool IsAuto { get; set; }

        #region Overrides of BaseEntity

        /// <summary>
        /// KeyMemberId
        /// </summary>
        public override object EntityId
        {       
                get { return Id; }
 
        }

        #endregion
    }
}
