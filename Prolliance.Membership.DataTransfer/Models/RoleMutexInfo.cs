
namespace Prolliance.Membership.DataTransfer.Models
{
    /// <summary>
    /// 角色互斥关系表模型（唯一条件：Group + Type + RoleCode）
    /// </summary>
    public partial class RoleMutexInfo : ModelBase
    {
        #region 属性
        /// <summary>
        /// 角色Id
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// 互斥类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 互斥关系组名
        /// </summary>
        public string Group { get; set; }
        #endregion
    }
}
