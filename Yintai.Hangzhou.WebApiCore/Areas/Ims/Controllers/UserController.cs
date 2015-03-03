using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Hangzhou.Contract.DTO.Request;
using Yintai.Hangzhou.Contract.DTO.Response;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.WebSupport.Mvc;
using com.intime.fashion.common.Extension;

namespace Yintai.Hangzhou.WebApiCore.Areas.Ims.Controllers
{
    public class UserController : RestfulController
    {
        private IFavoriteRepository _favorRepo;
        private IEFRepository<IMS_GiftCardOrderEntity> _orderRepo;
        public UserController(IFavoriteRepository favorRepo, IEFRepository<IMS_GiftCardOrderEntity> orderRepo)
        {
            _favorRepo = favorRepo;
            _orderRepo = orderRepo;
        }
        [RestfulAuthorize]
        public ActionResult Favor_Store(PagerInfoRequest request, int authuid)
        {
            var linq = Context.Set<FavoriteEntity>().Where(f => f.Status == (int)DataStatus.Normal && f.User_Id == authuid
                && f.FavoriteSourceType == (int)SourceType.Store)
                        .Join(Context.Set<IMS_AssociateEntity>(), o => o.FavoriteSourceId, i => i.Id, (o, i) => i)
                        .Join(Context.Set<UserEntity>(), o => o.UserId, i => i.Id, (o, i) => new { A = o, U = i });

            int totalCount = linq.Count();
            int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
            linq = linq.OrderByDescending(l => l.A.CreateDate).Skip(skipCount).Take(request.Pagesize);
            var result = linq.ToList().Select(l => new
            {
                id = l.A.Id,
                image = l.U.Logo.Image100Url(),
                name = l.U.Nickname,
                phone = l.U.Mobile
            });
            var response = new PagerInfoResponse<dynamic>(request.PagerRequest, totalCount)
            {
                Items = result.ToList<dynamic>()
            };

            return this.RenderSuccess<PagerInfoResponse<dynamic>>(c => c.Data = response);

        }

        [RestfulAuthorize]
        public ActionResult Favor_Combo(PagerInfoRequest request, int authuid)
        {
            var linq = Context.Set<FavoriteEntity>().Where(f => f.Status == (int)DataStatus.Normal && f.User_Id == authuid
               && f.FavoriteSourceType == (int)SourceType.Combo)
                        .Join(Context.Set<IMS_ComboEntity>().GroupJoin(Context.Set<ResourceEntity>().Where(r => r.Status == (int)DataStatus.Normal && r.SourceType == (int)SourceType.Combo),
                                                                o => o.Id,
                                                                i => i.SourceId,
                                                                (o, i) => new { IC = o, ICR = i.OrderByDescending(icr => icr.SortOrder).FirstOrDefault() }),
                                         o => o.FavoriteSourceId, i => i.IC.Id, (o, i) => new
                                         {
                                             type = (int)SourceType.Combo,
                                             id = (int)i.IC.Id,
                                             name = i.IC.Desc,
                                             is_online = i.IC.Status,
                                             price = i.IC.Price,
                                             create_date = o.CreatedDate,
                                             image = i.ICR.Name,
                                             store_id = o.Store_Id
                                         });
            var linq2 = Context.Set<FavoriteEntity>().Where(f => f.Status == (int)DataStatus.Normal && f.User_Id == authuid
               && f.FavoriteSourceType == (int)SourceType.GiftCard)
                        .Join(Context.Set<IMS_GiftCardEntity>().GroupJoin(Context.Set<ResourceEntity>().Where(r => r.Status == (int)DataStatus.Normal && r.SourceType == (int)SourceType.GiftCard),
                                                                o => o.Id,
                                                                i => i.SourceId,
                                                                (o, i) => new { IC = o, ICR = i.OrderByDescending(icr => icr.SortOrder).FirstOrDefault() }),
                                         o => o.FavoriteSourceId, i => i.IC.Id, (o, i) => new
                                         {
                                             type = (int)SourceType.Combo,
                                             id = (int)i.IC.Id,
                                             name = i.IC.Name,
                                             is_online = i.IC.Status,
                                             price = 0m,
                                             create_date = o.CreatedDate,
                                             image = i.ICR.Name,
                                             store_id = o.Store_Id
                                         });
            linq = linq.Union(linq2);
            int totalCount = linq.Count();
            int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
            linq = linq.OrderByDescending(l => l.create_date).Skip(skipCount).Take(request.Pagesize);
            var result = linq.ToList().Select(l => new
            {
                type = l.type,
                id = l.id,
                name = l.name,
                is_online = l.is_online,
                price = l.price,
                create_date = l.create_date,
                image = l.image.Image320Url(),
                store_id = l.store_id
            });
            var response = new PagerInfoResponse<dynamic>(request.PagerRequest, totalCount)
            {
                Items = result.ToList<dynamic>()
            };
            return this.RenderSuccess<PagerInfoResponse<dynamic>>(c => c.Data = response);
        }


        [RestfulAuthorize]
        public ActionResult Favor(IMSUserFavorRequest request, int authuid)
        {
            int sourceType = (int)SourceType.Default;
            switch (request.Type)
            {
                case (int)AssociateFavorType.Combo:
                    sourceType = (int)SourceType.Combo;
                    break;
                case (int)AssociateFavorType.GiftCard:
                    sourceType = (int)SourceType.GiftCard;
                    break;
                case (int)AssociateFavorType.Store:
                    sourceType = (int)SourceType.Store;
                    break;
                default:
                    return this.RenderError(r => r.Message = "收藏类型不支持");
            }
            var favorEntity = Context.Set<FavoriteEntity>().Where(f => f.FavoriteSourceType == sourceType &&
                        f.FavoriteSourceId == request.Id &&
                        f.Store_Id == request.StoreId &&
                        f.User_Id == authuid).FirstOrDefault();
            if (favorEntity == null)
            {
                _favorRepo.Insert(new FavoriteEntity()
                {
                    CreatedDate = DateTime.Now,
                    CreatedUser = authuid,
                    Description = string.Empty,
                    FavoriteSourceId = request.Id,
                    Status = (int)DataStatus.Normal,
                    User_Id = authuid,
                    Store_Id = request.StoreId,
                    FavoriteSourceType = sourceType
                });
            }
            else
            {
                favorEntity.Status = (int)DataStatus.Normal;
                _favorRepo.Update(favorEntity);
            }
            return this.RenderSuccess<dynamic>(null);
        }

        [RestfulAuthorize]
        public ActionResult Unfavor(IMSUserFavorRequest request, int authuid)
        {
            int sourceType = (int)SourceType.Default;
            switch (request.Type)
            {
                case (int)AssociateFavorType.Combo:
                    sourceType = (int)SourceType.Combo;
                    break;
                case (int)AssociateFavorType.GiftCard:
                    sourceType = (int)SourceType.GiftCard;
                    break;
                case (int)AssociateFavorType.Store:
                    sourceType = (int)SourceType.Store;
                    break;
                default:
                    return this.RenderError(r => r.Message = "收藏类型不支持");
            }
            var favorEntity = Context.Set<FavoriteEntity>().Where(f => f.FavoriteSourceType == sourceType
                            && f.FavoriteSourceId == request.Id
                            && f.Store_Id == request.StoreId
                            && f.User_Id == authuid).FirstOrDefault();
            if (favorEntity == null)
                return this.RenderSuccess<dynamic>(null);
            favorEntity.Status = (int)DataStatus.Deleted;
            _favorRepo.Update(favorEntity);

            return this.RenderSuccess<dynamic>(null);
        }

        [RestfulAuthorize]
        public ActionResult Latest_GiftCard(int giftcardid, int timestamp, int authuid)
        {
            var benchTime = new DateTime(1970, 1, 1).AddSeconds(timestamp);
            var order = _orderRepo.Get(x => x.GiftCardItemId == giftcardid && x.CreateDate >= benchTime && x.CreateUser == authuid).OrderByDescending(x=>x.CreateDate).FirstOrDefault();
            if (order == null)
            {
                return this.RenderError(r => r.Message = "无购买记录");
            }
            return this.RenderSuccess<dynamic>(r => r.Data = new { charge_no = order.No, amount = order.Amount, create_date = order.CreateDate });
        }

        [RestfulAuthorize]
        public ActionResult Latest_Address(int authuid)
        {
            var benchTime = DateTime.Today.AddMonths(-1);
            var orderEntity = Context.Set<OrderEntity>().Where(o => o.CustomerId == authuid && o.CreateDate >= benchTime)
                                .OrderByDescending(o=>o.CreateDate).FirstOrDefault();
            if (orderEntity != null)
                return this.RenderSuccess<dynamic>(r => r.Data = new
                {
                    full_address = orderEntity.ShippingAddress,
                    contact_phone = orderEntity.ShippingContactPhone,
                    contact_person = orderEntity.ShippingContactPerson,
                    zip_code = orderEntity.ShippingZipCode
                });
            else
                return this.RenderSuccess<dynamic>(r => r.Data = new { });
        }
    }
}
