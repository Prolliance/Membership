
using System.Collections.Generic;
namespace Prolliance.Membership.DataTransfer.Models
{
    /// <summary>
    /// 应用表模型 (唯一条件：Key)
    /// </summary>
    public class AppInfo : ModelBase
    {
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
    }
}
