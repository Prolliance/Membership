
namespace Prolliance.Membership.Business
{
    /// <summary>
    /// 认证类型
    /// </summary>
    public static class AuthType
    {
        /// <summary>
        /// 密码认证方法（通过用户名和密码验证）
        /// </summary>
        public static readonly string PASSWORD = "password";
        /// <summary>
        /// 仅客户端验证方式（通过接入应用信息验证）
        /// </summary>
        public static readonly string CLIENT = "client";
        /// <summary>
        /// 此种方式暂不支持
        /// 授权码验证（用户在 Auth 页面完成身份验证，向应用发放授权 Code，应用通过 Code 和应用信息换取 Token）
        /// </summary>
        public static readonly string CODE = "code";
        /// <summary>
        /// 简化模式（用户在 Auth 页面完成身份验证，直接向应用发放 Token）
        /// </summary>
        public static readonly string SIMPLIFY = "simplify";
    }
}
