
namespace Prolliance.Membership.DataTransfer.Utils
{
    /// <summary>
    /// 服务响应状态
    /// </summary>
    public enum ServiceState
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 200,
        /// <summary>
        /// 未知服务器错误
        /// </summary>
        Error = 500,
        /// <summary>
        /// 无效的授权
        /// </summary>
        InvalidAuth = 400,
        /// <summary>
        /// 无效的 Token
        /// </summary>
        InvalidToken = 401,
        /// <summary>
        /// 无效的 App 凭据
        /// </summary>
        InvalidAppCredentials = 402,
        /// <summary>
        /// 无效的用户身份凭据
        /// </summary>
        InvalidUserCredentials = 403
    }
}
