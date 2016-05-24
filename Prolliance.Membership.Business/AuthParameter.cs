
namespace Prolliance.Membership.Business
{
    /// <summary>
    /// 登陆录参数
    /// </summary>
    public class AuthParameter
    {
        /// <summary>
        /// 验证类型 
        /// </summary>
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

        /// <summary>
        /// 应用标识
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 应用密钥
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 应用部署IP
        /// </summary>
        public string AppIp { get; set; }
    }
}
