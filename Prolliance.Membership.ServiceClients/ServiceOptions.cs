
namespace Prolliance.Membership.ServiceClients
{
    /// <summary>
    /// ServiceCleint 参数配置对象
    /// </summary>
    public class ServiceOptions
    {
        /// <summary>
        /// Membership 服务器的 Uri
        /// </summary>
        public string ServiceUri { get; set; }
        /// <summary>
        /// 应用识别码
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 应用密钥
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 构造一个服务选项对象
        /// </summary>
        /// <param name="serviceUri">服务器的 Uri</param>
        /// <param name="appKey">应用识别码</param>
        /// <param name="secret">应用密钥</param>
        public ServiceOptions(string serviceUri, string appKey, string secret)
        {
            this.ServiceUri = serviceUri;
            this.AppKey = appKey;
            this.Secret = secret;
        }
        /// <summary>
        /// 构造一个服务选项对象
        /// </summary>
        public ServiceOptions() { }
    }
}
