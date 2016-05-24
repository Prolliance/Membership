using Prolliance.Membership.ServiceClients.Manifests;
using System;
using System.Collections.Generic;

namespace Prolliance.Membership.ServiceClients.Models
{
    /// <summary>
    /// 应用类型 
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class App : ModelBase
    {
        internal const string SERVICE_TYPE = "app";

        /// <summary>
        /// 默认构造
        /// </summary>
        public App() { }

        /// <summary>
        /// 代码
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// 白名单列表
        /// </summary>
        public string WhiteList { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "RemoveAllCache", null);
        }

        /// <summary>
        /// 动态执行SQL
        /// </summary>
        /// <param name="dynamicSql"></param>
        public static void ExecuteDynamicSql(string dynamicSql)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "ExecuteDynamicSql", new { dynamicSql = dynamicSql });
        }

        /// <summary>
        /// 向 Membership 导入权限清单文本
        /// 一般仅用于 Web 应用，在应用程序启动时间，仅调用一次
        /// </summary>
        /// <param name="manifest">权限清单 JSON 文本</param>
        public static void ImportManifestText(string manifest)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "ImportManifestText", new
            {
                manifest = manifest
            });
        }

        /// <summary>
        /// 向 Membership 导入权限对象列表
        /// 一般仅用于 Web 应用，在应用程序启动时间，仅调用一次
        /// </summary>
        /// <param name="targetList">权限对象列表</param>
        public static void ImportManifest(List<Target> targetList)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "ImportManifest", new
            {
                targetList = targetList
            });
        }

        /// <summary>
        /// 向 Membership 导入权限对象清单
        /// 一般仅用于 Web 应用，在应用程序启动时间，仅调用一次
        /// </summary>
        /// <param name="manifest">权限清单对象</param>
        public static void ImportManifest(AppManifestBase manifest)
        {
            ImportManifestText(manifest.ExportManifestText());
        }

        /// <summary>
        /// 获取所有应用
        /// </summary>
        /// <returns></returns>
        public static List<App> GetAppList()
        {
            return ServiceClient.Get<List<App>>(SERVICE_TYPE, "GetAppList", null);
        }

        /// <summary>
        /// 获取当前应用的所有权限对象
        /// </summary>
        /// <returns>权限对象列表</returns>
        public static List<Target> GetTargetList(string appKey)
        {
            return ServiceClient.Post<List<Target>>(SERVICE_TYPE, "GetTargetList", new { otherAppKey = appKey });
        }

        /// <summary>
        /// 获取所有操作权限
        /// </summary>
        /// <returns></returns>
        public static List<Operation> GetOperationList()
        {
            return ServiceClient.Get<List<Operation>>(SERVICE_TYPE, "GetOperationList", null);
        }

        /// <summary>
        /// 更新一组权限对象（不会影响参数中不包括的 Target 对象，也不会影响已有的权限配置）
        /// </summary>
        /// <param name="targetList">权限对象列表</param>
        public static void UpdateTargetList(List<Target> targetList)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "UpdateTargetList", new
            {
                targetList = targetList
            });
        }

        /// <summary>
        /// 添加一组权限对象
        /// </summary>
        /// <param name="targetList">权限对象列表</param>
        public static void AddTargetList(List<Target> targetList)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "AddTargetList", new
            {
                targetList = targetList
            });
        }

        /// <summary>
        /// 删除一组权限对象
        /// </summary>
        /// <param name="targetList">权限对象列表</param>
        public static void DeleteTargetList(List<Target> targetList)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "DeleteTargetList", new
            {
                targetList = targetList
            });
        }

        /// <summary>
        /// 添加一组权限操作
        /// </summary>
        /// <param name="operationList">权限操作列表</param>
        public static void AddOperationList(List<Operation> operationList)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "AddOperationList", new
            {
                operationList = operationList
            });
        }

        /// <summary>
        /// 删除一组权限操作
        /// </summary>
        /// <param name="operationList">权限操作列表</param>
        /// <returns></returns>
        public static void DeleteOperationList(List<Operation> operationList)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "DeleteOperationList", new
            {
                operationList = operationList
            });
        }

    }
}
