
namespace Prolliance.Membership.DataTransfer.Models
{
    /// <summary>
    /// 登陆录参数
    /// </summary>
    public class AuthParameterInfo
    {
        public string Type { get; set; }
        /// <summary>
        /// 账号,必需唯一，必需指定
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

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

        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string AppIp { get; set; }
    }
}
