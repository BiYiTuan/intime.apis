using com.intime.fashion.common;
using System;
using System.Linq;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Contract;
using Yintai.Hangzhou.Service.Manager;

namespace Yintai.Hangzhou.Service.Impl
{
    public class UserService : BaseService, IUserService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly MappingManagerV2 _mapping;

        public UserService(MappingManagerV2 m, IUserAccountRepository userAccountRepository, ICustomerRepository customerRepository, IUserRoleRepository userRoleRepository, IRoleRepository roleRepository)
        {
            _customerRepository = customerRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _userAccountRepository = userAccountRepository;
            _mapping = m;
        }

        #region methods

        private void AddCount(int userId, AccountType accountType, int setCount, int updateUserId)
        {
            //var numCount = _userAccountRepository.AddCount(userId, accountType, setCount, updateUserId);

            //if (numCount != 1)
            //{
            //    Logger.Warn(String.Format("增加{4}发生影响行数不为1，userid={0},foverCount={1},updateId={2},numCount={3}", userId, setCount, updateUserId, numCount, accountType.ToString()));
            //}
        }

        private void SubCount(int userId, AccountType accountType, int setCount, int? updateUserId)
        {
            //var numCount = _userAccountRepository.SubCount(userId, accountType, setCount, updateUserId);

            //if (numCount != 1)
            //{
            //    Logger.Warn(String.Format("减少{4}发生影响行数不为1，userid={0},foverCount={1},updateId={2},numCount={3}", userId, setCount, updateUserId, numCount, accountType.ToString()));
            //}
        }

        /// <summary>
        /// 获取用户实体
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        private UserEntity GetEntity(int uid)
        {
            return _customerRepository.GetItem(uid);
        }

        #endregion

        public UserModel Get(string userName, string password)
        {
            var entity = _customerRepository.Get(m=>m.Name == userName).FirstOrDefault();

            if (entity == null)
            {
                return null;
            }
            if (!SecurityHelper.CheckEqual(password, entity.Password))
                return null;
            return _mapping.UserModelMapping(entity);
            // return MappingManager.UserModelMapping(entity);
        }

        public UserModel Get(int userId)
        {
            var entity = GetEntity(userId);

            if (entity == null)
            {
                return null;
            }

            return _mapping.UserModelMapping(entity);
            //return MappingManager.UserModelMapping(entity);
        }

        /// <summary>
        /// 喜欢
        /// </summary>
        /// <param name="likeUserId">发起喜欢人</param>
        /// <param name="likedUserId">被喜欢人</param>
        /// <returns></returns>
        public UserModel LikeAdd(int likeUserId, int likedUserId)
        {
            AddILike(likeUserId, 1, likeUserId);
            AddLikeMe(likedUserId, 1, likedUserId);


            //var likeUser = GetEntity(likeUserId);
            //var likedUser = GetEntity(likedUserId);

            //if (likeUser == null || likedUser == null)
            //{
            //    return null;
            //}

            ////++likecount
            //likeUser.LikeCount++;
            //likedUser.LikedCount++;

            //SaveEntity(likeUser);
            //SaveEntity(likedUser);

            return Get(likeUserId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="likeUserId">发起喜欢人</param>
        /// <param name="likedUserId">被喜欢人</param>
        /// <returns></returns>
        public UserModel LikeSubtract(int likeUserId, int likedUserId)
        {
            SubILike(likeUserId, 1, likeUserId);
            SubLikeMe(likedUserId, 1, likedUserId);


            //var likeUser = GetEntity(likeUserId);
            //var likedUser = GetEntity(likedUserId);

            //if (likeUser == null || likedUser == null)
            //{
            //    return null;
            //}

            ////++likecount
            //likeUser.LikeCount--;
            //likedUser.LikedCount--;

            //SaveEntity(likeUser);
            //SaveEntity(likedUser);

            return Get(likeUserId);
        }

        public UserModel BindStore(int userId, int bindStoreId, int authUserId)
        {
            var bindUser = _customerRepository.GetItem(userId);

            bindUser.Store_Id = bindStoreId;
            bindUser.UpdatedDate = DateTime.Now;
            bindUser.UpdatedUser = authUserId;

            _customerRepository.Update(bindUser);

            return _mapping.UserModelMapping(bindUser);
            // return MappingManager.UserModelMapping(bindUser);
        }

        public UserModel UnStore(int userId, int bindStoreId, int authUserId)
        {
            var bindUser = _customerRepository.GetItem(userId);

            //if (bindStoreId == bindUser.Store_Id)
            //{
            bindUser.Store_Id = 0;
            //}

            bindUser.UpdatedDate = DateTime.Now;
            bindUser.UpdatedUser = authUserId;

            _customerRepository.Update(bindUser);

            return _mapping.UserModelMapping(bindUser);
            // return MappingManager.UserModelMapping(bindUser);
        }

        public UserModel UserLevelUpdate(int userId, UserLevel userLevel, int authUserId)
        {
            var user = _customerRepository.GetItem(userId);
            user.UserLevel = (int)userLevel;
            user.UpdatedDate = DateTime.Now;
            user.UpdatedUser = authUserId;

            return _mapping.UserModelMapping(user);
            // return MappingManager.UserModelMapping(bindUser);
        }

        public UserModel UserRoleInsert(int userId, UserRole userRole, int authUserId)
        {
            var user = _customerRepository.GetItem(userId);

            var userRoles = _userRoleRepository.GetListByUserId(userId);
            var role = _roleRepository.GetItemByRoleVal((int)userRole);

            if (userRoles != null && userRoles.Any(v => v.Role_Id == role.Id))
            {
                return _mapping.UserModelMapping(user);
                // return MappingManager.UserModelMapping(bindUser);
            }

            _userRoleRepository.Insert(new UserRoleEntity
            {
                CreatedDate = DateTime.Now,
                CreatedUser = authUserId,
                Role_Id = role.Id,
                Status = (int)DataStatus.Normal,
                User_Id = userId
            });

            user.UpdatedDate = DateTime.Now;
            user.UpdatedUser = authUserId;

            return _mapping.UserModelMapping(user);
            //return MappingManager.UserModelMapping(user);
        }

        public UserModel UserRoleDeleted(int userId, UserRole userRole, int authUserId)
        {
            var user = _customerRepository.GetItem(userId);
            var userRoles = _userRoleRepository.GetListByUserId(userId);
            var role = _roleRepository.GetItemByRoleVal((int)userRole);

            if (userRoles != null)
            {
                //找到
                var entity = userRoles.SingleOrDefault(v => v.Role_Id == role.Id);
                if (entity != null)
                {
                    entity.Status = (int)DataStatus.Deleted;
                    _userRoleRepository.Delete(entity);
                }
            }

            return _mapping.UserModelMapping(user);
            //return MappingManager.UserModelMapping(user);
        }

        public void SetCardBinder(int userId, bool? binded)
        {
            _customerRepository.SetCardBinded(userId, binded);
        }

        public void AddFover(int userId, int foverCount, int updateUserId)
        {
            AddCount(userId, AccountType.FavorCount, foverCount, updateUserId);
        }

        public void SubFover(int userId, int foverCount, int updateUserId)
        {
            SubCount(userId, AccountType.FavorCount, foverCount, updateUserId);
        }

        public void AddPoint(int userId, int pointCount, int updateUserId)
        {
            AddCount(userId, AccountType.Point, pointCount, updateUserId);
        }

        public void SubPoint(int userId, int pointCount, int updateUserId)
        {
            SubCount(userId, AccountType.Point, pointCount, updateUserId);
        }

        public void AddCoupon(int userId, int couponCount, int updateUserId)
        {
            AddCount(userId, AccountType.Coupon, couponCount, updateUserId);
        }

        public void SubCoupon(int userId, int couponCount, int updateUserId)
        {
            SubCount(userId, AccountType.Coupon, couponCount, updateUserId);
        }

        public void AddConsumption(int userId, int consumptionCount, int updateUserId)
        {
            AddCount(userId, AccountType.ConsumptionCount, consumptionCount, updateUserId);
        }

        public void SubConsumption(int userId, int consumptionCount, int updateUserId)
        {
            SubCount(userId, AccountType.ConsumptionCount, consumptionCount, updateUserId);
        }

        public void AddShare(int userId, int shareCount, int updateUserId)
        {
            AddCount(userId, AccountType.ShareCount, shareCount, updateUserId);
        }

        public void AddILike(int userId, int iLikeCount, int updateUserId)
        {
            AddCount(userId, AccountType.IlikeCount, iLikeCount, updateUserId);
        }

        public void SubILike(int userId, int iLikeCount, int updateUserId)
        {
            SubCount(userId, AccountType.IlikeCount, iLikeCount, updateUserId);
        }

        public void AddLikeMe(int userId, int likeMeCount, int updateUserId)
        {
            AddCount(userId, AccountType.LikeMeCount, likeMeCount, updateUserId);
        }

        public void SubLikeMe(int userId, int likeMeCount, int updateUserId)
        {
            SubCount(userId, AccountType.LikeMeCount, likeMeCount, updateUserId);
        }
    }
}
