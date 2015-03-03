using com.intime.fashion.common.Validators;
using com.intime.fashion.webapi.domain.Request.Assistant;
using System;
using System.Linq;
using System.Web.Mvc;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.WebApiCore.Areas.Ims.Controllers
{
    public partial class AssistantController
    {
        /// <summary>
        /// 导购开店申请        
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Apply(DaogouApplicationRequest request)
        {
            if (request.AuthUser == null || request.AuthUser.Id == 0)
            {
                return this.RenderError(r => r.Message = "未登录的用户");
            }

            if (string.IsNullOrEmpty(request.Phone))
            {
                return this.RenderError(r => r.Message = "电话号码为空!");
            }

            if (!PhoneValidator.ValidateMobile(request.Phone))
            {
                return this.RenderError(r => r.Message = "手机号格式错误!");
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                return this.RenderError(r => r.Message = "姓名为空!");
            }

            if (string.IsNullOrEmpty(request.SectionCode))
            {
                return this.RenderError(r => r.Message = "专柜编码为空!");
            }

            if (request.StoreId <= 0)
            {
                return this.RenderError(r => r.Message = "所属门店为空!");
            }

            var associate = _associateRepo.Get(x => x.UserId == request.AuthUser.Id).FirstOrDefault();


            if (associate != null)
            {
                if (request.AuthUser.Level == UserLevel.DaoGou || request.AuthUser.UserLevel >= request.ApplyType)
                {
                    return this.RenderError(r => r.Message = "用户已是迷你银导购，不能重复申请");
                }
            }

            var history =
                _inviteCodeRequestRepo.Get(x => x.UserId == request.AuthUser.Id && x.Status == 1 && (!x.Approved.HasValue || x.Approved.Value))
                    .FirstOrDefault();
            if (history != null)
            {
                return this.RenderError(r => r.Message = "您已经申请了且尚未审批，不能重复申请");
            }


            if (!_sectionRepo.Contains(x => x.StoreId == request.StoreId && request.SectionCode == x.SectionCode))
            {
                return this.RenderError(r => r.Message = "无效的专柜编码!");
            }

            _inviteCodeRequestRepo.Insert(new IMS_InviteCodeRequestEntity()
            {
                ContactMobile = request.Phone,
                CreateDate = DateTime.Now,
                CreateUser = request.AuthUser.Id,
                Name = request.Name,
                OperatorCode = request.OperatorCode,
                SectionCode = request.SectionCode,
                SectionName = request.SectionName,
                StoreId = request.StoreId,
                RequestType = request.ApplyType,
                UserId = request.AuthUser.Id,
                UpdateDate = DateTime.Now,
                UpdateUser = request.AuthUser.Id,
                Status = (int)DataStatus.Normal,
                IdCard = request.IdCard,
                ApprovedNotificationTimes = 0,
                DemotionNotificationTimes = 0,
            });
            return this.RenderSuccess<dynamic>(null);
        }
    }
}
