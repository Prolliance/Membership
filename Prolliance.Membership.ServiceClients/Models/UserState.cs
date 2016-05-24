using System;

namespace Prolliance.Membership.ServiceClients.Models
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public class UserState : ModelBase
    {
        #region 属性
        /// <summary>
        /// 默认构造
        /// </summary>
        public UserState() { }
        /// <summary>
        /// 账号
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
        #endregion

        #region 实例方法

        /// <summary>
        /// 根据状态获取用户
        /// </summary>
        /// <returns>用户</returns>
        public User GetUser()
        {
            return User.GetUser(this);
        }
        #endregion
    }
}
