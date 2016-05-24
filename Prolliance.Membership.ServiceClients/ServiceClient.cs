using AjaxEngine.AjaxHandlers;
using AjaxEngine.Extends;
using System.Collections.Generic;
using System.Reflection;

namespace Prolliance.Membership.ServiceClients
{

    /// <summary>
    /// SDK 客户端
    /// </summary>
    public static class ServiceClient
    {
        private static Client Create()
        {
            return new Client();
        }

        /// <summary>
        /// 参数配置对象
        /// </summary>
        public static ServiceOptions Options { get; set; }

        private static Dictionary<string, object> HandleData(Dictionary<string, object> data)
        {
            if (data == null) return null;
            if (Options != null)
            {
                data["appKey"] = Options.AppKey;
                data["secret"] = Options.Secret;
            }
            return data;
        }

        private static Dictionary<string, object> ConvertToDictionary(object data)
        {
            Dictionary<string, object> rs = new Dictionary<string, object>();
            if (data != null)
            {
                PropertyInfo[] propertyList = data.GetProperties();
                foreach (PropertyInfo property in propertyList)
                {
                    rs[property.Name] = data.GetPropertyValue(property.Name);
                }
            }
            return rs;
        }

        /// <summary>
        /// 进行 Get 请求
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="type">服务类别</param>
        /// <param name="method">请求的方法</param>
        /// <param name="data">传送的数据（匿名对象）</param>
        /// <returns>返回的对象</returns>
        public static T Get<T>(string type, string method, object data) where T : new()
        {
            return Get<T>(type, method, ConvertToDictionary(data));
        }

        /// <summary>
        /// 进行 Post 请求
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="type">服务类别</param>
        /// <param name="method">请求的方法</param>
        /// <param name="data">传送的数据（匿名对象）</param>
        /// <returns>返回的对象</returns>
        public static T Post<T>(string type, string method, object data) where T : new()
        {
            return Post<T>(type, method, ConvertToDictionary(data));
        }

        /// <summary>
        /// 进行 Get 请求
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="type">服务类别</param>
        /// <param name="method">请求的方法</param>
        /// <param name="data">传送的数据（字典）</param>
        /// <returns>返回的对象</returns>
        public static T Get<T>(string type, string method, Dictionary<string, object> data) where T : new()
        {
            data = data ?? new Dictionary<string, object>();
            var url = string.Format("{0}/service/{1}.ashx", Options.ServiceUri, type);
            data["method"] = method;
            ServiceResult<T> rs = Create().Get<ServiceResult<T>>(url, HandleData(data));
            if (rs.State == ServiceState.Success)
            {
                return rs.Data;
            }
            else
            {
                throw new ServiceErrorException(rs.State.ToString()) { State = rs.State };
            }
        }

        /// <summary>
        /// 进行 Post 请求
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="type">服务类别</param>
        /// <param name="method">请求的方法</param>
        /// <param name="data">传送的数据（字典）</param>
        /// <returns>返回的对象</returns>
        public static T Post<T>(string type, string method, Dictionary<string, object> data) where T : new()
        {
            data = data ?? new Dictionary<string, object>();
            var url = string.Format("{0}/service/{1}.ashx", Options.ServiceUri, type);
            data["method"] = method;
            ServiceResult<T> rs = Create().Post<ServiceResult<T>>(url, HandleData(data));
            if (rs.State == ServiceState.Success)
            {
                return rs.Data;
            }
            else
            {
                throw new ServiceErrorException(rs.State.ToString()) { State = rs.State };
            }
        }

        /// <summary>
        /// 初始化 SDK
        /// </summary>
        /// <param name="options">初始化选项</param>
        public static void Init(ServiceOptions options)
        {
            Options = options;
        }
    }
}
