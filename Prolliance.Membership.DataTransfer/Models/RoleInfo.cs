
namespace Prolliance.Membership.DataTransfer.Models
{
    /// <summary>
    /// 角色表模型（唯一条件：Code）
    /// </summary>
    public class RoleInfo : ModelBase
    {
        #region 属性
        /// <summary>
        /// 主键
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
        /// 是否可用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        #endregion
    }
}
