
namespace Prolliance.Membership.DataTransfer.Models
{
    /// <summary>
    /// 岗位表模型 (唯一条件：OrgCode + Code)
    /// </summary>
    public class PositionInfo : ModelBase
    {
        #region 属性
        /// <summary>
        /// 组织Code
        /// </summary>
        public string OrganizationId { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        public string Type { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        public string OrganizationCode { get; set; }
        #endregion
    }
}
