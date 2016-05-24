using System;

namespace Prolliance.Membership.DataTransfer.Models
{
    /// <summary>
    /// 用户会话状态表模型（唯一条件：Account）
    /// </summary>
    public class UserStateInfo : ModelBase
    {
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

    }
}
