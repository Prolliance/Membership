
using System;
namespace Prolliance.Membership.ServiceClients
{
    /// <summary>
    /// 服务响应结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResult<T>
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public ServiceResult()
        {
            this.State = ServiceState.Success;
        }
        /// <summary>
        /// 构造一个成功的响应
        /// </summary>
        /// <param name="data">响应数据</param>
        public ServiceResult(T data)
        {
            this.Data = data;
            this.State = ServiceState.Success;
        }
        /// <summary>
        /// 构造一个错误的响应
        /// </summary>
        /// <param name="state">错误响应状态</param>
        public ServiceResult(ServiceState state)
        {
            if (ServiceState.Success == state)
            {
                throw new Exception("成功的响应，请使用 ServiceResult(T data) 构造。");
            }
            this.State = state;
            this.Data = default(T);
        }
        /// <summary>
        /// 响应的状态
        /// </summary>
        public ServiceState State { get; set; }
        /// <summary>
        /// 响应数据
        /// </summary>
        public T Data { get; set; }
    }
}
