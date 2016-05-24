using Prolliance.Membership.Common;
using Prolliance.Membership.DataPersistence;
using Prolliance.Membership.DataPersistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prolliance.Membership.Business
{
    public class UserState : ModelBase
    {
        internal static DataRepo<UserStateInfo> UserStateInfoRepo = new DataRepo<UserStateInfo>();

        /// <summary>
        /// 账号,必需唯一，必需指定
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 会话秘钥
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 最后活动时间
        /// </summary>
        public DateTime LastActive { get; set; }

        public void Save()
        {
            UserStateInfoRepo.Save(this.MappingTo<UserStateInfo>());
        }

        public void Delete()
        {
            UserStateInfoRepo.Delete(this.MappingTo<UserStateInfo>());
        }

        public User GetUser()
        {
            return User.GetUser(this.Account);
        }

        public static List<UserState> GetStateListByAccount(string account)
        {
            account = account ?? "";
            return GetStateList()
                .Where(state => state.Account.ToLower() == account.ToLower())
                .ToList();
        }

        public static List<UserState> GetStateList()
        {
            return UserStateInfoRepo
               .Read()
               .MappingToList<UserState>();
        }

        public static UserState GetStateById(string id)
        {
            id = id ?? "";
            return GetStateList().FirstOrDefault(state => state.Id == id);
        }
        public static UserState GetState(string account, string deviceId)
        {
            account = account ?? "";
            deviceId = deviceId ?? "";
            return GetStateList()
                .FirstOrDefault(state => state.Account == account
                && state.DeviceId == deviceId);
        }
        public static UserState GetStateByToken(string token)
        {
            token = token ?? "";
            return GetStateList().FirstOrDefault(state => state.Token == token);
        }
    }
}
