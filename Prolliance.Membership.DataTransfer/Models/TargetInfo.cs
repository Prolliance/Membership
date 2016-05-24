
using System.Collections.Generic;
namespace Prolliance.Membership.DataTransfer.Models
{
    /// <summary>
    /// 权限对象表模型（唯一条件：AppKey + Code）
    /// </summary>
    public class TargetInfo : ModelBase
    {

        #region 属性

        /// <summary>
        /// Key
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 作用域动态表达式
        /// </summary>
        public string Scope { get; set; }
        public string AppKey { get; set; }

        #endregion

        public List<OperationInfo> OperationList { get; set; }
    }
}
