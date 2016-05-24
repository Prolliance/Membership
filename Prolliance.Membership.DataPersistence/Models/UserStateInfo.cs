using System;

namespace Prolliance.Membership.DataPersistence.Models
{
    /// <summary>
    /// 用户会话状态表模型（唯一条件：Account + DeviceId）
    /// </summary>
    public class UserStateInfo : IModel
    {
        public string Id { get; set; }
        public string Account { get; set; }
        public string Token { get; set; }
        public string Device { get; set; }
        public string DeviceId { get; set; }
        public string Ip { get; set; }
        public DateTime LastActive { get; set; }

    }
}
