using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Service.Contract
{
    public interface IUserAccountService
    {
        /// <summary>
        /// 增加收藏数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="foverCount"></param>
        /// <param name="updateUserId"></param>
        void AddFover(int userId, int foverCount, int updateUserId);

        /// <summary>
        /// 减少收藏数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="foverCount"></param>
        /// <param name="updateUserId"></param>
        void SubFover(int userId, int foverCount, int updateUserId);

        /// <summary>
        /// 增加积点
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pointCount"></param>
        /// <param name="updateUserId"></param>
        void AddPoint(int userId, int pointCount, int updateUserId);

        /// <summary>
        /// 减少积点
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pointCount"></param>
        /// <param name="updateUserId"></param>
        void SubPoint(int userId, int pointCount, int updateUserId);

        /// <summary>
        /// 增加优惠码数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="couponCount"></param>
        /// <param name="updateUserId"></param>
        void AddCoupon(int userId, int couponCount, int updateUserId);

        /// <summary>
        /// 减少优惠码数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="couponCount"></param>
        /// <param name="updateUserId"></param>
        void SubCoupon(int userId, int couponCount, int updateUserId);

        /// <summary>
        /// 增加消费数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="couponCount"></param>
        /// <param name="updateUserId"></param>
        void AddConsumption(int userId, int couponCount, int updateUserId);

        /// <summary>
        /// 减少消费数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="couponCount"></param>
        /// <param name="updateUserId"></param>
        void SubConsumption(int userId, int couponCount, int updateUserId);

        /// <summary>
        /// 增加分享数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="shareCount"></param>
        /// <param name="updateUserId"></param>
        void AddShare(int userId, int shareCount, int updateUserId);

        /// <summary>
        /// 增加我喜欢数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="iLikeCount"></param>
        /// <param name="updateUserId"></param>
        void AddILike(int userId, int iLikeCount, int updateUserId);

        /// <summary>
        /// 减少我喜欢数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="iLikeCount"></param>
        /// <param name="updateUserId"></param>
        void SubILike(int userId, int iLikeCount, int updateUserId);

        /// <summary>
        /// 增加喜欢我
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="likeMeCount"></param>
        /// <param name="updateUserId"></param>
        void AddLikeMe(int userId, int likeMeCount, int updateUserId);

        /// <summary>
        /// 减少喜欢我
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="likeMeCount"></param>
        /// <param name="updateUserId"></param>
        void SubLikeMe(int userId, int likeMeCount, int updateUserId);
    }

    public interface IUserService : IUserAccountService
    {
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userName">不区分大小写</param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserModel Get(string userName, string password);

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserModel Get(int userId);

        /// <summary>
        /// 喜欢用户
        /// </summary>
        /// <returns></returns>
        UserModel LikeAdd(int likeUserId, int likedUserId);

        /// <summary>
        /// 不喜欢用户
        /// </summary>
        /// <returns></returns>
        UserModel LikeSubtract(int likeUserId, int likedUserId);

        /// <summary>
        /// 绑定用户到店铺
        /// </summary>
        /// <param name="userId">被绑定人</param>
        /// <param name="bindStoreId">被绑定的店铺</param>
        /// <param name="authUserId">操作人</param>
        /// <returns></returns>
        UserModel BindStore(int userId, int bindStoreId, int authUserId);

        /// <summary>
        /// 绑定用户到店铺
        /// </summary>
        /// <param name="userId">被解除绑定人</param>
        /// <param name="bindStoreId">被解除绑定的店铺</param>
        /// <param name="authUserId">操作人</param>
        /// <returns></returns>
        UserModel UnStore(int userId, int bindStoreId, int authUserId);

        /// <summary>
        /// 设置用户等级
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userLevel"></param>
        /// <param name="authUserId"></param>
        /// <returns></returns>
        UserModel UserLevelUpdate(int userId, UserLevel userLevel, int authUserId);

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userRole"></param>
        /// <param name="authUserId"></param>
        /// <returns></returns>
        UserModel UserRoleInsert(int userId, UserRole userRole, int authUserId);

        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userRole"></param>
        /// <param name="authUserId"></param>
        /// <returns></returns>
        UserModel UserRoleDeleted(int userId, UserRole userRole, int authUserId);


        void SetCardBinder(int userId, bool? binded);
    }
}
