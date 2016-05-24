
using System.Collections.Generic;

namespace Prolliance.Membership.DataTransfer.Models
{
    /// <summary>
    /// 组织表模型 (唯一条件：Code)
    /// </summary>
    public class OrganizationInfo : ModelBase
    {
        #region 普通属性
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 全称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public string ParentId { set; get; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        public Dictionary<string, object> Extensions { get; set; }  

        #endregion

    }
}
